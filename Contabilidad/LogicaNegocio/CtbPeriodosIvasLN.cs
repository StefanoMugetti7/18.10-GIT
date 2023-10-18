using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using Contabilidad.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Servicio.AccesoDatos;
using Auditoria;
using Auditoria.Entidades;
using System.Data;

namespace Contabilidad.LogicaNegocio
{
    class CtbPeriodosIvasLN : BaseLN<CtbPeriodosIvas>
    {
        public override bool Agregar(CtbPeriodosIvas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.FechaCierre = DateTime.Now;
            pParametro.FechaEvento = DateTime.Now;
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();
            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();
                try
                {
                    if (!this.Validar(pParametro, bd, tran))
                        return false;

                    pParametro.IdPeriodoIva = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CtbPeriodosIvasInsertar");
                    if (pParametro.IdPeriodoIva == 0)
                        resultado = false;


                    CtbAsientosContables asiento = new CtbAsientosContables();
                    asiento.IdTipoOperacion = 231;
                    asiento.IdRefTipoOperacion = pParametro.IdPeriodoIva;
                    asiento.FechaAsiento = pParametro.FechaContable;
                    asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                    if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
                    {
                        AyudaProgramacionLN.MapearError(asiento, pParametro);
                        resultado = false;
                    }
                   // return resultado;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                        tran.Rollback();
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
        private bool Validar(CtbPeriodosIvas pParametro, Database bd, DbTransaction tran)
        {
            //string fechaFin = pParametro.EjercicioContable.FechaFin.Year.ToString();
            //fechaFin = string.Concat(fechaFin, pParametro.EjercicioContable.FechaFin.Month.ToString().PadLeft(2, '0'));

            //if (pParametro.Periodo > Convert.ToInt32(fechaFin))
            //{
            //    pParametro.CodigoMensaje = "EjercicioContableSuperado";
            //    return false;
            //}

            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CtbPeriodosIvasValidaciones"))
            {
                return false;
            }
            return true;

        }
        public override bool Modificar(CtbPeriodosIvas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CtbPeriodosIvas periodoIvaViejo = new CtbPeriodosIvas();
            periodoIvaViejo.IdPeriodoIva = pParametro.IdPeriodoIva;
            periodoIvaViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            periodoIvaViejo = this.ObtenerDatosCompletos(periodoIvaViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!this.Validar(pParametro, bd, tran))
                        return false;
                    resultado = this.Modificar(pParametro, bd, tran);
                    if (resultado && !AuditoriaF.AuditoriaAgregar(periodoIvaViejo, Acciones.Update, pParametro, bd, tran))
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
        public override CtbPeriodosIvas ObtenerDatosCompletos(CtbPeriodosIvas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CtbPeriodosIvas>("CtbPeriodosIvasSeleccionar", pParametro);
        }
        public override List<CtbPeriodosIvas> ObtenerListaFiltro(CtbPeriodosIvas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbPeriodosIvas>("CtbPeriodosIvasListarFiltro", pParametro);
        }
        public List<CtbPeriodosIvas> ObtenerLista(CtbPeriodosIvas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbPeriodosIvas>("CtbPeriodosIvasListar", pParametro);
        }
        public CtbPeriodosIvas ObtenerUltimoCerrado(CtbPeriodosIvas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CtbPeriodosIvas>("CtbPeriodosIvasObtenerUltimoCerrado", pParametro);
        }
        public CtbPeriodosIvas ObtenerArmarLiquidacionIVA(CtbPeriodosIvas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CtbPeriodosIvas>("CtbPeriodosIvasArmarLiquidacion", pParametro);
        }
        public bool Modificar(CtbPeriodosIvas pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbPeriodosIvasActualizar"))
                return false;

            return true;
        }
        /// <summary>
        /// Valida si un Periodo esta Cerrado
        /// Devuelve True si esta Cerrado
        /// </summary>
        /// <param name="pParametro">Periodo</param>
        /// <returns></returns>
        public bool ValidarCierre(CtbPeriodosIvas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CtbPeriodosIvasValidarCerrado");
        }
        public CtbPeriodosIvas ObtenerUltimoCerrado()
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CtbPeriodosIvas>("CtbPeriodosIvasObtenerUltimoCerradoSinParam");
        }
    }
}
