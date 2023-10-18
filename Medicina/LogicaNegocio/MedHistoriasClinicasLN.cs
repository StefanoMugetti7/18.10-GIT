using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Medicina.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Generales.FachadaNegocio;

namespace Medicina.LogicaNegocio
{
    class MedHistoriasClinicasLN : BaseLN<MedHistoriasClinicas>
    {
        public override bool Agregar(MedHistoriasClinicas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            //if (!this.Validar(pParametro))
            //    return false;

            pParametro.FechaEvento = DateTime.Now;
            pParametro.Estado.IdEstado = (int)Estados.Activo;
            pParametro.FechaAlta = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdHistoriaClinica = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "MedHistoriasClinicasInsertar");
                    if (pParametro.IdHistoriaClinica == 0)
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

        public override bool Modificar(MedHistoriasClinicas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //if (!this.Validar(pParametro))
            //    return false;

            pParametro.FechaEvento = DateTime.Now;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            //MedPrestadores valorViejo = new MedPrestadores();
            //valorViejo.IdPrestador = pParametro.IdPrestador;
            //valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            //valorViejo = this.ObtenerDatosCompletos(valorViejo);


            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "MedHistoriasClinicasActualizar"))
                        resultado = false;

                    //if (resultado && !this.ActualizarEvoluciones(pParametro, bd, tran))
                    //    resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
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

        private bool ActualizarEvoluciones(MedHistoriasClinicas pParametro, Database db, DbTransaction tran)
        {
            foreach (MedHistoriasClinicasEvoluciones item in pParametro.HistoriasClinicasEvoluciones)
            {
                item.UsuarioLogueado = pParametro.UsuarioLogueado;
                switch (item.EstadoColeccion)
                {
                    #region "Agregado"
                    case EstadoColecciones.Agregado:
                        item.Estado.IdEstado = (int)Estados.Activo;
                        item.IdHistoriaClinica = pParametro.IdHistoriaClinica;
                        item.IdHistoriaClinicaEvolucion = BaseDatos.ObtenerBaseDatos().Agregar(item, db, tran, "MedHistoriasClinicasEvolucionesInsertar");
                        if (item.IdHistoriaClinicaEvolucion == 0)
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        break;
                    #endregion

                    #region "Modificado"
                    case EstadoColecciones.Modificado:
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, db, tran, "MedHistoriasClinicasEvolucionesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        //if (!AuditoriaF.AuditoriaAgregar(
                        //    pValorViejo.PrestadoresDiasHoras.Find(x => x.IdPrestadorDiaHora == item.IdPrestadorDiaHora), Acciones.Update, item, db, tran))
                        //{
                        //    AyudaProgramacionLN.MapearError(item, pParametro);
                        //    return false;
                        //}

                        break;
                    #endregion
                }
            }
            return true;
        }


        public bool ValidarExiste(MedHistoriasClinicas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "MedHistoriasClinicasValidarExiste");
        }

        public override MedHistoriasClinicas ObtenerDatosCompletos(MedHistoriasClinicas pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<MedHistoriasClinicas>("MedHistoriasClinicasSeleccionar", pParametro);
            pParametro.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }

        public override List<MedHistoriasClinicas> ObtenerListaFiltro(MedHistoriasClinicas pParametro)
        {
            throw new NotImplementedException();
        }
    }
}
