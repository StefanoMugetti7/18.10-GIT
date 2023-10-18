using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Generales.Entidades;
using Generales.FachadaNegocio;
using LavaYa.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Net.Codecrete.QrCodeGenerator;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;

namespace LavaYa.LogicaNegocio
{
    public class EdificiosLN : BaseLN<LavEdificios>
    {
        public override bool Agregar(LavEdificios pParametro)
        {

            if (pParametro.IdEdificio > 0)
                return true;

            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            if (!this.Validaciones(pParametro))
                return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = Validaciones(pParametro);

                    if (resultado && !this.Agregar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        foreach (LavMaquinas item in pParametro.Maquinas)
                        {
                            item.IdEdificio = pParametro.IdEdificio;
                            item.UsuarioLogueado = pParametro.UsuarioLogueado;
                            item.IdMaquina = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "LavMaquinasEdificiosInsertar");
                            if (item.IdMaquina == 0)
                                return false;
                        }
                    }

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdEdificio.ToString());
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }

            }
            return resultado;
        }

        public override bool Modificar(LavEdificios pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;

            if (!this.Validaciones(pParametro))
                return false;

            LavEdificios valorViejo = new LavEdificios();
            valorViejo.IdEdificio = pParametro.IdEdificio;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = Validaciones(pParametro);
                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "LavEdificiosActualizar"))
                        resultado = false;

                    if(resultado)
                    {
                        if(pParametro.Maquinas.Count == 0)
                        {
                            if(resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "LavMaquinasEdificiosLiberarMaquinas"))
                            {
                                resultado = false;
                            }
                        }
                        else
                        {
                            foreach (LavMaquinas item in pParametro.Maquinas)
                            {
                                item.IdEdificio = pParametro.IdEdificio;
                                item.UsuarioLogueado = pParametro.UsuarioLogueado;
                                if (item.EstadoColeccion == EstadoColecciones.Agregado)
                                {
                                    item.IdMaquina = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "LavMaquinasEdificiosInsertar");
                                    if (item.IdMaquina == 0)
                                    {
                                        resultado = false;
                                        break;
                                    }
                                }
                                else if(item.EstadoColeccion == EstadoColecciones.Borrado)
                                {
                                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "LavMaquinasEdificiosBaja"))
                                    {
                                        resultado = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }
        
        public override LavEdificios ObtenerDatosCompletos(LavEdificios pParametro)
        {
            LavEdificios edificio = BaseDatos.ObtenerBaseDatos().Obtener<LavEdificios>("LavEdificiosSeleccionar", pParametro);
            edificio.Maquinas = ObtenerMaquinasCargadas(edificio);

            edificio.Archivos = TGEGeneralesF.ArchivosObtenerLista(edificio);
            var qr = QrCode.EncodeText(edificio.CodigoQR, QrCode.Ecc.Medium);
            Bitmap qrCodeImage = qr.ToBitmap(1, 8);
            using (MemoryStream stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                edificio.CodigoQRImagen = stream.ToArray();
            }
            return edificio;
         }

        public override List<LavEdificios> ObtenerListaFiltro(LavEdificios pParametro)
        {
            throw new NotImplementedException();
        }
        internal bool Agregar(LavEdificios pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdEdificio = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "LavEdificiosInsertar");
            if (pParametro.IdEdificio == 0)
                return false;

            return true;
        }
        private bool Validaciones (LavEdificios pParametro)
        {
            return true;
        }
        public DataTable ObtenerListaGrilla(LavEdificios pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[LavEdificiosSeleccionarDescripcionPorFiltro]", pParametro);
        } 
        public DataTable ObtenerListaGrillaRequerimientos(LavEdificios pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[LavEdificiosObtenerListaGrillaRequerimientos]", pParametro);
        }
        public DataTable ObtenerListaGrillaPuntosVentas(LavEdificios pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[LavEdificiosObtenerListaGrillaPuntosVentas]", pParametro);
        }
        public List<LavEdificios> ObtenerOpcionesContrato()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<LavEdificios>("LavEdificiosObtenerOpcionesContrato", new LavEdificios());
        }
        public List<LavEdificios> ObtenerOpcionesSistemaPago()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<LavEdificios>("LavEdificiosObtenerOpcionesSistemaPago", new LavEdificios());
        }
        public List<LavEdificios> ObtenerOpcionesHorario()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<LavEdificios>("LavEdificiosObtenerOpcionesHorario", new LavEdificios());
        }

        public List<LavMaquinas> ObtenerMaquinasCargadas(LavEdificios pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<LavMaquinas>("LavMaquinasEdificiosObtenerMaquinasCargadas", pParametro);
        }
    }
}
