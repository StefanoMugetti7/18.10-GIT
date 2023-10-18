using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
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
    public class MaquinasLN : BaseLN<LavMaquinas>
    {

        public override bool Agregar(LavMaquinas pParametro)
        {

            if (pParametro.IdMaquina > 0)
                return false;

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

                    if (resultado && !this.Agregar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdMaquina.ToString());
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

        public override bool Modificar(LavMaquinas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;

            if (!this.Validaciones(pParametro))
                return false;

            LavMaquinas valorViejo = new LavMaquinas();
            valorViejo.IdMaquina = pParametro.IdMaquina;
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
                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "LavMaquinasActualizar"))
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

        public override LavMaquinas ObtenerDatosCompletos(LavMaquinas pParametro)
        {
            LavMaquinas maquina = BaseDatos.ObtenerBaseDatos().Obtener<LavMaquinas>("LavMaquinasSeleccionar", pParametro);

            var qr = QrCode.EncodeText(maquina.CodigoQR, QrCode.Ecc.Medium);
            Bitmap qrCodeImage = qr.ToBitmap(1, 8);
            using (MemoryStream stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                maquina.CodigoQRImagen = stream.ToArray();
            }
            return maquina;
        }

        public override List<LavMaquinas> ObtenerListaFiltro(LavMaquinas pParametro)
        {
            throw new NotImplementedException();
        }
        internal bool Agregar(LavMaquinas pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdMaquina = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "LavMaquinasInsertar");
            if (pParametro.IdMaquina == 0)
                return false;

            return true;
        }
        private bool Validaciones(LavMaquinas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "LavMaquinasValidar");
        }

        public DataTable ObtenerListaGrilla(LavMaquinas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[LavMaquinasSeleccionarDescripcionPorFiltro]", pParametro);
        }

        public List<LavMaquinas> ObtenerMarcas()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<LavMaquinas>("LavMaquinasObtenerMarcas", new LavMaquinas());
        }
        public List<LavMaquinas> ObtenerModelos(LavMaquinas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<LavMaquinas>("LavMaquinasObtenerModelos", pParametro);
        }

        public List<LavMaquinas> ObtenerEdificios()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<LavMaquinas>("LavMaquinasObtenerEdificios", new LavMaquinas());
        }
        public List<LavMaquinas> ObtenerTiposMaquinas()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<LavMaquinas>("LavMaquinasObtenerTiposMaquinas", new LavMaquinas());
        } 
        public List<LavMaquinas> ObtenerMaquinas()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<LavMaquinas>("LavMaquinasEdificiosObtenerMaquinas", new LavMaquinas());
        }

    }
}
