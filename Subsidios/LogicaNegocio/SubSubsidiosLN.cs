using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Subsidios.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria;
using Auditoria.Entidades;
using System.Data;

namespace Subsidios.LogicaNegocio
{
    class SubSubsidiosLN : BaseLN<SubSubsidios>
    {
        public override SubSubsidios ObtenerDatosCompletos(SubSubsidios pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<SubSubsidios>("SubSubsidiosSeleccionar", pParametro);
            pParametro.Escalas = BaseDatos.ObtenerBaseDatos().ObtenerLista<SubEscalas>("SubEscalasSeleccionarPorSubsidio", pParametro);
            return pParametro;
        }

        public override List<SubSubsidios> ObtenerListaFiltro(SubSubsidios pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<SubSubsidios>("SubSubsidiosSeleccionarPorFiltro", pParametro);
        }

        public DataTable ObtenerListaFiltroDT(SubSubsidios pParametro)
        {

            return BaseDatos.ObtenerBaseDatos().ObtenerLista("SubSubsidiosSeleccionarPorFiltro", pParametro);
        }

        public override bool Agregar(SubSubsidios pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            //if (!this.Validar(pParametro))
            //    return false;

            pParametro.FechaEvento = DateTime.Now;
            pParametro.Estado.IdEstado = (int)Estados.Activo;
            //pParametro.FechaAlta = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdSubsidio = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "SubSubsidiosInsertar");
                    if (pParametro.IdSubsidio == 0)
                        resultado = false;

                    if (resultado && !this.ActualizarEscalas(pParametro, new SubSubsidios(), bd, tran))
                        resultado = false;

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

        public override bool Modificar(SubSubsidios pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //if (!this.Validar(pParametro))
            //    return false;

            pParametro.FechaEvento = DateTime.Now;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            SubSubsidios valorViejo = new SubSubsidios();
            valorViejo.IdSubsidio = pParametro.IdSubsidio;
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
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "SubSubsidiosActualizar"))
                        resultado = false;

                    if (resultado && !this.ActualizarEscalas(pParametro, valorViejo, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
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
        public DataTable ObtenerCuentaCorrienteDT(SubSubsidios pParametro)
        {

            return BaseDatos.ObtenerBaseDatos().ObtenerLista("SubSubsidiosSeleccionarCuentaCorriente", pParametro);
        }
        private bool ActualizarEscalas(SubSubsidios pParametro, SubSubsidios pValorViejo, Database db, DbTransaction tran)
        {
            foreach (SubEscalas item in pParametro.Escalas)
            {
                switch (item.EstadoColeccion)
                {
                    #region "Agregado"
                    case EstadoColecciones.Agregado:
                        item.Estado.IdEstado = (int)Estados.Activo;
                        item.IdSubsidio = pParametro.IdSubsidio;
                        item.IdEscala = BaseDatos.ObtenerBaseDatos().Agregar(item, db, tran, "SubEscalasInsertar");
                        if (item.IdEscala == 0)
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        break;
                    #endregion

                    #region "Modificado"
                    case EstadoColecciones.Modificado:
                        item.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, db, tran, "SubEscalasActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.Escalas.Find(x => x.IdEscala == item.IdEscala), Acciones.Update, item, db, tran))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }

                        break;
                    #endregion
                }
            }
            return true;
        }
    }
}
