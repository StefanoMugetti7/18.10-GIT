using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prestamos.Entidades;
using Servicio.AccesoDatos;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Comunes.LogicaNegocio;
using Afiliados.Entidades;
using Comunes;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Prestamos.LogicaNegocio
{
    class PreSimulacionesLN : BaseLN<PrePrestamos>
    {
        /// <summary>
        /// Devuelve una lista de Prestamos Simulacion por Afiliado
        /// </summary>
        /// <param name="pAfiliado"></param>
        /// <returns></returns>
        public List<PrePrestamos> ObtenerPorAfiliado(AfiAfiliados pAfiliado)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<PrePrestamos>("PreSimulacionesSeleccionarPorAfiAfiliados", pAfiliado);
        }

        public override PrePrestamos ObtenerDatosCompletos(PrePrestamos pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<PrePrestamos>("PreSimulacionesSeleccionar", pParametro);
            pParametro.PrestamosCuotas = BaseDatos.ObtenerBaseDatos().ObtenerLista<PrePrestamosCuotas>("PreSimulacionesCuotasSeleccionar", pParametro);
            return pParametro;

        }

        public override List<PrePrestamos> ObtenerListaFiltro(PrePrestamos pParametro)
        {
            throw new NotImplementedException();
            //return BaseDatos.ObtenerBaseDatos().ObtenerLista<PrePrestamos>("PreSimulacionesSeleccionarFiltrar", pParametro);
        }

        public override bool Agregar(PrePrestamos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            if (pParametro.PrestamosCheques.Count > 0)
                pParametro.LoteCheques = pParametro.PrestamosCheques.ToDataTable().ToXmlDocument();
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.Estado.IdEstado = (int)EstadosPrestamos.Activo;

            //if (!this.Validar(pParametro))
            //    return false;

            pParametro.FechaPrestamo = DateTime.Now;

            // Armo la cuponera
            pParametro.ImporteAutorizado = pParametro.ImporteSolicitado;
            new PrePrestamosLN().ArmarCuponera(pParametro);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    //Guardo la Simulacion del Prestamo
                    pParametro.IdSimulacion = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "PreSimulacionesInsertar");
                    if (pParametro.IdSimulacion == 0)
                        resultado = false;
                    else
                    {

                        // Envio a grabar la ListaCuotas
                        if (!this.CuotasActualizar(pParametro, bd, tran))
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

        private bool CuotasActualizar(PrePrestamos pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            foreach (PrePrestamosCuotas Cuotas in pParametro.PrestamosCuotas)
            {
                switch (pParametro.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        Cuotas.IdSimulacion = pParametro.IdSimulacion;
                        Cuotas.IdSimulacionCuota = BaseDatos.ObtenerBaseDatos().Agregar(Cuotas, bd, tran, "PreSimulacionesCuotasInsertar");
                        if (Cuotas.IdSimulacionCuota == 0)
                            resultado = false;
                        break;
                    case EstadoColecciones.Modificado:
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(Cuotas, bd, tran, "PreSimulacionesCuotasActualizar"))
                            resultado = false;
                        break;
                    default:
                        break;
                }
                if (!resultado)
                {
                    AyudaProgramacionLN.MapearError(Cuotas, pParametro);
                    return false;
                }

            }
            return true;
        }

        public override bool Modificar(PrePrestamos pParametro)
        {
            throw new NotImplementedException();
        }
    }
}
