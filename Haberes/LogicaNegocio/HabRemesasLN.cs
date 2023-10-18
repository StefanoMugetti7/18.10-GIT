using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Haberes.Entidades;
using Servicio.AccesoDatos;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using Auditoria;
using Auditoria.Entidades;
using Comunes;
using System.Collections;
using Generales.Entidades;
using Generales.FachadaNegocio;
using CuentasPagar.Entidades;
using CuentasPagar.FachadaNegocio;

namespace Haberes.LogicaNegocio
{
    class HabRemesasLN : BaseLN<HabRemesas>
    {
        public override HabRemesas ObtenerDatosCompletos(HabRemesas pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<HabRemesas>("HabRemesasSeleccionar", pParametro);
            HabRemesasDetalles filtro = new HabRemesasDetalles();
            filtro.Periodo = pParametro.Periodo;
            filtro.RemesaTipo.IdRemesaTipo = pParametro.RemesaTipo.IdRemesaTipo;
            filtro.IdEstadoAfiliado = (int)EstadosTodos.Todos;
            filtro.Estado.IdEstado = (int)EstadosRemesasDetalles.SinValidar;
            pParametro.RemesasDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<HabRemesasDetalles>("[HabRemesasDetallesSeleccionarFiltro]", filtro);
            //pParametro.CantidadDepositar = pParametro.RemesasDetalles.Count(x => x.Estado.IdEstado == (int)EstadosRemesasDetalles.Activo);
            //pParametro.ImporteDepositar = pParametro.RemesasDetalles.Where(x => x.Estado.IdEstado == (int)EstadosRemesasDetalles.Activo).Sum(x => x.NetoIAF);
            return pParametro;
        }

        public List<HabRemesasDetalles> ObtenerListaFiltro(HabRemesasDetalles pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<HabRemesasDetalles>("HabRemesasDetallesSeleccionarFiltro", pParametro);
        }

        public List<HabRemesasDetalles> ObtenerPendienteEnvio(HabRemesasDetalles pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<HabRemesasDetalles>("HabRemesasDetallesSeleccionarPendienteEnvio", pParametro);
        }

        private HabRemesasDetalles ObtenerDatosCompletos(HabRemesasDetalles pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<HabRemesasDetalles>("HabRemesasDetallesSeleccionar", pParametro);
        }

        public override List<HabRemesas> ObtenerListaFiltro(HabRemesas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<HabRemesas>("HabRemesasListar", pParametro);
        }

        public bool Modificar(HabRemesasDetalles pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            HabRemesasDetalles valorViejo = new HabRemesasDetalles();
            valorViejo.IdRemesaDetalle = pParametro.IdRemesaDetalle;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = this.Modificar(pParametro, bd, tran);

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                    {
                        //AyudaProgramacionLN.MapearError(valorViejo, pParametro);
                        resultado = false;
                    }

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

        public bool Modificar(HabRemesasDetalles pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "HabRemesasDetallesActualizar"))
                return false;

            return true;
        }

        public override bool Agregar(HabRemesas pParametro)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deposita los Haberes de los retirados en las Cuentas Corrientes
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public override bool Modificar(HabRemesas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            int ejecucion;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //HabRemesasDetalles valorViejo = new HabRemesasDetalles();
            //valorViejo.IdRemesa = pParametro.IdRemesa;
            //valorViejo = this.ObtenerDatosCompletos(valorViejo);
            
            try
            {
                TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.DBTiempoEjecucionProcesosDatos);
                
                Hashtable parametros = new Hashtable();
                parametros.Add("Periodo", pParametro.Periodo);
                parametros.Add("IdRemesaTipo", pParametro.RemesaTipo.IdRemesaTipo);
                parametros.Add("IdUsuarioEvento", pParametro.UsuarioLogueado.IdUsuarioEvento);

                ejecucion = BaseDatos.ObtenerBaseDatos().EjecutarSP(pParametro, parametros, "HabRemesasProcesarDepositos", Convert.ToInt32(valor.ParametroValor));

                //if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                //{
                //    //AyudaProgramacionLN.MapearError(valorViejo, pParametro);
                //    resultado = false;
                //}

                if (ejecucion > 0)
                {
                    pParametro.CodigoMensaje = "ResultadoTransaccion";
                }
                else
                    resultado = false;

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, "LogicaNegocio");
                pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                pParametro.CodigoMensajeArgs.Add(ex.Message);
                resultado = false;
            }
           
            return resultado;
        }

        /// <summary>
        /// Deposita los Haberes de los retirados en las Cuentas Corrientes
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool Cierre(HabRemesas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //HabRemesasDetalles valorViejo = new HabRemesasDetalles();
            //valorViejo.IdRemesaDetalle = pParametro.IdRemesaDetalle;
            //valorViejo = this.ObtenerDatosCompletos(valorViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "[HabRemesasProcesoCierre]");

                    //if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                    //{
                    //    //AyudaProgramacionLN.MapearError(valorViejo, pParametro);
                    //    resultado = false;
                    //}

                    if ((pParametro.ImporteTotal - pParametro.ImporteDepositado) > 0)
                    {
                        if (resultado && !CuentasPagarF.SolicitudPagoAgregarRemesa(pParametro, pParametro.IdRemesa, pParametro.Periodo, pParametro.ImporteTotal - pParametro.ImporteDepositado, bd, tran))
                            resultado = false;
                    }

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

    }
}
