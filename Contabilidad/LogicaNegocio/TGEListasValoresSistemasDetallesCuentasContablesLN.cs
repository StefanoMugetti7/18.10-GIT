using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Servicio.AccesoDatos;
using Auditoria;
using Auditoria.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Comunes.Entidades;
using Contabilidad.Entidades;
using Comunes.LogicaNegocio;
using Comunes;

namespace Contabilidad.LogicaNegocio
{
    class TGEListasValoresSistemasDetallesCuentasContablesLN : BaseLN<TGEListasValoresSistemasDetallesCuentasContables>
    {
        public override TGEListasValoresSistemasDetallesCuentasContables ObtenerDatosCompletos(TGEListasValoresSistemasDetallesCuentasContables pParametro)
        {

            TGEListasValoresSistemasDetallesCuentasContables lista = BaseDatos.ObtenerBaseDatos().Obtener<TGEListasValoresSistemasDetallesCuentasContables>("TGEListasValoresSistemasDetallesCuentasContablesSeleccionar", pParametro);
            lista.ListaValorSistemaDetalle = BaseDatos.ObtenerBaseDatos().Obtener<TGEListasValoresSistemasDetalles>("TGEListasValoresSistemasDetallesSeleccionar", lista.ListaValorSistemaDetalle);
            return lista;
        }

        /// <summary>
        /// Devuelve una ListaValorSistemaDetalleCuentaContable por IdListaValorSistemaDetalle
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public TGEListasValoresSistemasDetallesCuentasContables ObtenerDatosCompletos(TGEListasValoresSistemasDetalles pParametro, Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<TGEListasValoresSistemasDetallesCuentasContables>("TGEListasValoresSistemasDetallesCuentasContablesPorIdListaValorDetalle", pParametro, bd, tran);
        }

        public override List<TGEListasValoresSistemasDetallesCuentasContables> ObtenerListaFiltro(TGEListasValoresSistemasDetallesCuentasContables pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEListasValoresSistemasDetallesCuentasContables>("TGEListasValoresSistemasDetallesCuentasContablesSeleccionarPorFiltro", pParametro);
        }

        public List<TGEListasValoresSistemasDetallesCuentasContables> ObtenerListaPorIdListaValor(TGEListasValoresSistemas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEListasValoresSistemasDetallesCuentasContables>("TGEListasValoresSistemasDetallesCuentasContablesPorIdListaValor", pParametro);
        }


        //TGEListasValoresSistemasDetallesCuentasContablesSeleccionarPorFiltro
        public override bool Agregar(TGEListasValoresSistemasDetallesCuentasContables pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            if (!this.Validar(pParametro, new TGEListasValoresSistemasDetallesCuentasContables()))
                return false;
            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "TGEListasValoresSistemasDetallesCuentasContablesValidaciones"))
            {
                return false;
            }

            pParametro.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;
            pParametro.FechaEvento = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdListaValorSistemaDetalleCuentaContable = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "TGEListasValoresSistemasDetallesCuentasContablesInsertar");
                    if (pParametro.IdListaValorSistemaDetalleCuentaContable == 0)
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

        public override bool Modificar(TGEListasValoresSistemasDetallesCuentasContables pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            TGEListasValoresSistemasDetallesCuentasContables valorViejo = new TGEListasValoresSistemasDetallesCuentasContables();
            valorViejo.IdListaValorSistemaDetalleCuentaContable = pParametro.IdListaValorSistemaDetalleCuentaContable;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            if (!this.Validar(pParametro, new TGEListasValoresSistemasDetallesCuentasContables()))
                return false;
            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "TGEListasValoresSistemasDetallesCuentasContablesValidaciones"))
            {
                return false;
            }

            pParametro.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;
            pParametro.FechaEvento = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {

                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TGEListasValoresSistemasDetallesCuentasContablesActualizar"))
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

        private bool Validar(TGEListasValoresSistemasDetallesCuentasContables pParametro, TGEListasValoresSistemasDetallesCuentasContables pValorViejo)
        {
            if (pParametro.CuentaContable.IdCuentaContable == 0)
            {
                pParametro.CodigoMensaje = "SeleccioneCuentaContable";
                return false;
            }

            return true;
        }
    }
}
