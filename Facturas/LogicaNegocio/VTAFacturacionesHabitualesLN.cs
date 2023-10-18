using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Facturas.Entidades;
using Servicio.AccesoDatos;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Auditoria;
using Auditoria.Entidades;
using Generales.FachadaNegocio;

namespace Facturas.LogicaNegocio
{
    class VTAFacturacionesHabitualesLN : BaseLN<VTAFacturacionesHabituales>
    {
        public override bool Agregar(VTAFacturacionesHabituales pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.Estado.IdEstado = (int)Estados.Activo;
            pParametro.FechaEvento = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();
                try
                {
                    pParametro.IdFacturacionHabitual = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "VTAFacturacionesHabitualesInsertar");
                    if (pParametro.IdFacturacionHabitual == 0)
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ItemsActualizar(pParametro, new VTAFacturacionesHabituales(), bd, tran))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(new VTAFacturacionesHabituales(), Acciones.Insert, pParametro, bd, tran))
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

        public override bool Modificar(VTAFacturacionesHabituales pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            VTAFacturacionesHabituales valorViejo = new VTAFacturacionesHabituales();
            valorViejo.IdFacturacionHabitual = pParametro.IdFacturacionHabitual;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.FechaEvento = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();
                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "VTAFacturacionesHabitualesActualizar");

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ItemsActualizar(pParametro, valorViejo, bd, tran))
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

        public override VTAFacturacionesHabituales ObtenerDatosCompletos(VTAFacturacionesHabituales pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<VTAFacturacionesHabituales>("VTAFacturacionesHabitualesSeleccionar", pParametro);
            pParametro.FacturacionesHabitualesDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<VTAFacturacionesHabitualesDetalles>("VTAFacturacionesHabitualesDetallesSeleccionar", pParametro);
            return pParametro;
        }

        public override List<VTAFacturacionesHabituales> ObtenerListaFiltro(VTAFacturacionesHabituales pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<VTAFacturacionesHabituales>("VTAFacturacionesHabitualesSeleccionarFiltro", pParametro);
        }

        #region "Items"
        private bool ItemsActualizar(VTAFacturacionesHabituales pParametro, VTAFacturacionesHabituales pValorViejo, Database bd, DbTransaction tran)
        {
            foreach (VTAFacturacionesHabitualesDetalles item in pParametro.FacturacionesHabitualesDetalles)
            {
                switch (item.EstadoColeccion)
                {
                    #region Agregado
                    case EstadoColecciones.Agregado:
                        item.IdFacturacionHabitual = pParametro.IdFacturacionHabitual;
                        item.UsuarioLogueado = pParametro.UsuarioLogueado;
                        item.IdFacturacionHabitualDetalle = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "VTAFacturacionesHabitualesDetallesInsertar");
                        if (item.IdFacturacionHabitualDetalle == 0)
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        break;
                    #endregion
                    #region Modificado
                    case EstadoColecciones.Modificado:
                        item.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "VTAFacturacionesHabitualesDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.FacturacionesHabitualesDetalles.Find(x => x.IdFacturacionHabitualDetalle == item.IdFacturacionHabitualDetalle), Acciones.Update, item, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            return true;
        }
        #endregion
    }
}
