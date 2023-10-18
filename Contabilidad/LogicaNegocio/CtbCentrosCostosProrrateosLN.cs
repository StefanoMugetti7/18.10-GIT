using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contabilidad.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria;
using Auditoria.Entidades;

namespace Contabilidad.LogicaNegocio
{
    class CtbCentrosCostosProrrateosLN : BaseLN<CtbCentrosCostosProrrateos>
    {
        public override bool Agregar(CtbCentrosCostosProrrateos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.FechaEvento = DateTime.Now;

            if (!this.Validar(pParametro))
                return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdCentroCostoProrrateo = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CtbCentrosCostosProrrateosInsertar");
                    if (pParametro.IdCentroCostoProrrateo == 0)
                        resultado = false;

                    if (resultado && !this.ActualizarDetalles(pParametro, bd, tran))
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

        public override bool Modificar(CtbCentrosCostosProrrateos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (!this.Validar(pParametro))
                return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CtbCentrosCostosProrrateos asientoViejo = new CtbCentrosCostosProrrateos();
            asientoViejo.IdCentroCostoProrrateo = pParametro.IdCentroCostoProrrateo;
            asientoViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            asientoViejo = this.ObtenerDatosCompletos(asientoViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbCentrosCostosProrrateosActualizar");

                    if (resultado && !this.ActualizarDetalles(pParametro, bd, tran))
                        resultado = false;
                                   
                    if (resultado && !AuditoriaF.AuditoriaAgregar(asientoViejo, Acciones.Update, pParametro, bd, tran))
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

        public override CtbCentrosCostosProrrateos ObtenerDatosCompletos(CtbCentrosCostosProrrateos pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CtbCentrosCostosProrrateos>("CtbCentrosCostosProrrateosSeleccionar", pParametro);
            pParametro.CentrosCostosProrrateosDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbCentrosCostosProrrateosDetalles>("CtbCentrosCostosProrrateosDetallesSeleccionarPorCentroCostoProrrateo", pParametro);
            return pParametro;
        }

        public override List<CtbCentrosCostosProrrateos> ObtenerListaFiltro(CtbCentrosCostosProrrateos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbCentrosCostosProrrateos>("CtbCentrosCostosProrrateosSeleccionarFiltros", pParametro);
        }

        public List<CtbCentrosCostosProrrateos> ObtenerCombo(CtbCentrosCostosProrrateos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbCentrosCostosProrrateos>("CTBCentroCostoProrateoFiltrosCombo", pParametro);
        }

        private bool ActualizarDetalles(CtbCentrosCostosProrrateos pParametro, Database bd, DbTransaction tran)
        {
            foreach (CtbCentrosCostosProrrateosDetalles detalle in pParametro.CentrosCostosProrrateosDetalles)
            {
                detalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                switch (detalle.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        detalle.IdCentroCostoProrrateo = pParametro.IdCentroCostoProrrateo.Value;
                        detalle.Estado.IdEstado = (int)Estados.Activo;
                        detalle.IdCentroCostoProrrateoDetalle = BaseDatos.ObtenerBaseDatos().Agregar(detalle, bd, tran, "CtbCentrosCostosProrrateosDetallesInsertar");
                        if (detalle.IdCentroCostoProrrateoDetalle == 0)
                        {
                            AyudaProgramacionLN.MapearError(detalle, pParametro);
                            return false;
                        }
                        break;
                    case EstadoColecciones.Modificado:
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(detalle, bd, tran, "CtbCentrosCostosProrrateosDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(detalle, pParametro);
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        private bool Validar(CtbCentrosCostosProrrateos pParametro)
        {
            if (pParametro.CentrosCostosProrrateosDetalles.Sum(x => x.Porcentaje) != 100)
            {
                pParametro.CodigoMensaje = "CentrosCostosProrrateosSumaPorcentaje";
                return false;
            }
            return true;
        }

    }
}
