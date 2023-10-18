using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prestamos.Entidades;
using Contabilidad.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Contabilidad;
using Comunes.LogicaNegocio;
using Generales.Entidades;
using Cargos.Entidades;
using ProcesosDatos;
using ProcesosDatos.Entidades;
using ProcesamientosDatos.Entidades;
using Servicio.AccesoDatos;

namespace Prestamos.LogicaNegocio
{
    internal class InterfazContableLN
    {
        public bool AsientoCesiones(PrePrestamosCesiones pParametro, Database bd, DbTransaction tran)
        {
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdPrestamoCesion;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = DateTime.Now;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;
            CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            PrePrestamosCesiones prestamoCesionConta = new PrePrestamosCesiones();
            prestamoCesionConta.IdPrestamoCesion = pParametro.IdPrestamoCesion;
            prestamoCesionConta = BaseDatos.ObtenerBaseDatos().Obtener<PrePrestamosCesiones>("[PrePrestamosCesionesDetallesSeleccionarContabilidad]", prestamoCesionConta, bd, tran);
            if (prestamoCesionConta.IdPrestamoCesion == 0)
            {
                return false;
            }
            
            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, prestamoCesionConta.ImporteAmortizacionContable, modelo, EnumCodigosAsientosModelos.PrePrestamos))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }
            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, prestamoCesionConta.ImporteAmortizacionContableNoCorriente, modelo, EnumCodigosAsientosModelos.PrePrestamosNoCte))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }
            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, prestamoCesionConta.ImporteInteresContable, modelo, EnumCodigosAsientosModelos.PreInteresesACobrar))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }
            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, prestamoCesionConta.ImporteInteresContableNoCorriente, modelo, EnumCodigosAsientosModelos.PreInteresesACobrarNoCte))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }
            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, prestamoCesionConta.ImporteCuotasFacturadas, modelo, EnumCodigosAsientosModelos.CarDescuentosRecuperar))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }
            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, prestamoCesionConta.ImporteCuotasFacturadasPendientes, modelo, EnumCodigosAsientosModelos.CarDescuentosNoRecuperados))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            decimal totalCesion = prestamoCesionConta.ImporteAmortizacionContable + prestamoCesionConta.ImporteAmortizacionContableNoCorriente
                + prestamoCesionConta.ImporteInteresContable + prestamoCesionConta.ImporteInteresContableNoCorriente;
            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, totalCesion, modelo, EnumCodigosAsientosModelos.PrePrestamosCesionados))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }
            decimal totalCargos = prestamoCesionConta.ImporteCuotasFacturadas + prestamoCesionConta.ImporteCuotasFacturadasPendientes;
            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, totalCargos, modelo, EnumCodigosAsientosModelos.CarCargosACobrarTerceros))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }


            if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            return true;
        }

        public bool AsientoOtorgamiento(PrePrestamos pParametro, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            CtbAsientosContables asiento = new CtbAsientosContables();
            if(!this.AsientoOtorgamientoArmar(asiento, pParametro, pValoresImportes, bd, tran))
                return false;

            if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            return true;
        }

        public bool AsientoOtorgamientoArmar(CtbAsientosContables asiento, PrePrestamos pParametro, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            //CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdPrestamo;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pParametro.FechaConfirmacion;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;

            SisProcesosProcesamiento ultimaGeneracionCargos = new SisProcesosProcesamiento();
            ultimaGeneracionCargos.Proceso.IdProceso = (int)EnumSisProcesos.GeneracionCargos;
            ultimaGeneracionCargos = ProcesosDatosF.ProcesosProcesamientoObtenerUltimoPeriodoProcesado(ultimaGeneracionCargos, bd, tran);
            asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;
            CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            DateTime fechaPrimerVto = pParametro.PrestamosCuotas[0].CuotaFechaVencimiento;

            //Prestamos Banco del Sol (Por Diferencia)
            //if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosBancoDelSol)
            //{
            //    if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.SaldoDeuda, modelo, EnumCodigosAsientosModelos.PrePrestamos))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pParametro);
            //        return false;
            //    }
            //    if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.SaldoDeuda - pParametro.ImportePrestamo, modelo, EnumCodigosAsientosModelos.PreInteres))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pParametro);
            //        return false;
            //    }
            //}
            //else
            //{
                decimal saldoDeuda = 0, saldoDeudaNoCte = 0, capital = 0, capitalNoCte = 0, interesACobrar = 0, interesACobrarNoCte = 0;
                capital = pParametro.PrestamosCuotas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Activa
                            && x.CuotaFechaVencimiento <= fechaPrimerVto.AddMonths(12)).Sum(y => y.ImporteAmortizacion);
                capitalNoCte = pParametro.PrestamosCuotas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Activa
                    && x.CuotaFechaVencimiento > fechaPrimerVto.AddMonths(12)).Sum(y => y.ImporteAmortizacion);
                decimal intereses = 0;
                decimal interesesNoCte = 0;
                intereses = interesACobrar = pParametro.PrestamosCuotas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Activa
                            && x.CuotaFechaVencimiento <= fechaPrimerVto.AddMonths(12)).Sum(y => y.ImporteInteres);
                interesesNoCte = interesACobrarNoCte = pParametro.PrestamosCuotas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Activa
                    && x.CuotaFechaVencimiento > fechaPrimerVto.AddMonths(12)).Sum(y => y.ImporteInteres);

                saldoDeuda = capital + interesACobrar;
                saldoDeudaNoCte = capitalNoCte + interesACobrarNoCte;

                if (modelo.AsientosModelosDetalles.Exists(x => x.AsientoModeloDetalleCodigo.CodigoValor == EnumCodigosAsientosModelos.PreInteresesACobrarNoCte.ToString()))
                {
                    saldoDeudaNoCte -= interesACobrarNoCte;
                }
                if (modelo.AsientosModelosDetalles.Exists(x => x.AsientoModeloDetalleCodigo.CodigoValor == EnumCodigosAsientosModelos.PreInteresesACobrar.ToString()))
                {
                    saldoDeuda -= interesACobrar;
                }
                if (modelo.AsientosModelosDetalles.Exists(x => x.AsientoModeloDetalleCodigo.CodigoValor == EnumCodigosAsientosModelos.PrePrestamosNoCte.ToString()))
                {
                    capitalNoCte = saldoDeudaNoCte;
                    capital = saldoDeuda;
                }
                else
                {
                    capital = saldoDeuda + saldoDeudaNoCte;
                }

                if (!modelo.AsientosModelosDetalles.Exists(x => x.AsientoModeloDetalleCodigo.CodigoValor == EnumCodigosAsientosModelos.PreInteresNoCte.ToString()))
                {
                    intereses += interesesNoCte;
                    interesesNoCte = 0;
                }

                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, capital, modelo, EnumCodigosAsientosModelos.PrePrestamos))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, capitalNoCte, modelo, EnumCodigosAsientosModelos.PrePrestamosNoCte))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, interesACobrar, modelo, EnumCodigosAsientosModelos.PreInteresesACobrar))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, interesACobrarNoCte, modelo, EnumCodigosAsientosModelos.PreInteresesACobrarNoCte))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                //Devengamiento -> Cuentas reguladoras de activo
                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, intereses, modelo, EnumCodigosAsientosModelos.PreInteres))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, interesesNoCte, modelo, EnumCodigosAsientosModelos.PreInteresNoCte))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }

                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ImporteGastos, modelo, EnumCodigosAsientosModelos.PreComisiones))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }

                decimal capitalCancelacion = 0, capitalNoCteCancelacion = 0, saldoCancelacion = 0, saldoCancelacionNoCte = 0;
                decimal interesesNoDevengado = 0;
                decimal interesesNoCteNoDevengado = 0;
                //decimal bonificaciones = 0;
                //decimal cancelacionesComisiones = 0;
                //Contabilizo las Cuotas Levantadas/Facturadas
                decimal cargosPeriodo = 0;
                decimal cargosAnteriores = 0;
                List<PrePrestamosCuotas> cuotasNoLevantadas;
                foreach (PrePrestamos cancela in pParametro.Cancelaciones)
                {
                    cuotasNoLevantadas = cancela.PrestamosCuotas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Cancelada && !cancela.CuotasPendientesCuentaCorriente.Any(y => y.IdReferenciaRegistro == x.IdPrestamoCuota)).ToList();
                    capitalCancelacion += cuotasNoLevantadas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Cancelada
                        && x.CuotaFechaVencimiento <= fechaPrimerVto.AddMonths(12)).Sum(y => y.ImporteAmortizacion);
                    capitalNoCteCancelacion += cuotasNoLevantadas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Cancelada
                        && x.CuotaFechaVencimiento > fechaPrimerVto.AddMonths(12)).Sum(y => y.ImporteAmortizacion);
                    interesesNoDevengado += cuotasNoLevantadas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Cancelada
                        && x.CuotaFechaVencimiento <= fechaPrimerVto.AddMonths(12)).Sum(y => y.ImporteInteres);
                    interesesNoCteNoDevengado += cuotasNoLevantadas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Cancelada
                        && x.CuotaFechaVencimiento > fechaPrimerVto.AddMonths(12)).Sum(y => y.ImporteInteres);

                    cargosPeriodo += cancela.CuotasPendientesCuentaCorriente.Where(x => x.Periodo == ultimaGeneracionCargos.Periodo).Sum(x => x.ImporteContabilizar);
                    cargosAnteriores += cancela.CuotasPendientesCuentaCorriente.Where(x => x.Periodo < ultimaGeneracionCargos.Periodo).Sum(x => x.ImporteContabilizar);
                }

                saldoCancelacion = capitalCancelacion + interesesNoDevengado;
                saldoCancelacionNoCte = capitalNoCteCancelacion + interesesNoCteNoDevengado;
                if (modelo.AsientosModelosDetalles.Exists(x => x.AsientoModeloDetalleCodigo.CodigoValor == EnumCodigosAsientosModelos.PreCancelacionesInteresesACobrarNoCte.ToString()))
                {
                    saldoCancelacionNoCte -= interesesNoCteNoDevengado;
                }
                if (modelo.AsientosModelosDetalles.Exists(x => x.AsientoModeloDetalleCodigo.CodigoValor == EnumCodigosAsientosModelos.PreCancelacionesInteresesACobrar.ToString()))
                {
                    saldoCancelacion -= interesesNoDevengado;
                }
                if (modelo.AsientosModelosDetalles.Exists(x => x.AsientoModeloDetalleCodigo.CodigoValor == EnumCodigosAsientosModelos.PreCancelacionesNoCte.ToString()))
                {
                    capitalNoCteCancelacion = saldoCancelacionNoCte;
                    capitalCancelacion = saldoCancelacion;
                }
                else
                {
                    capitalCancelacion = saldoCancelacion + saldoCancelacionNoCte;
                }

                if (!modelo.AsientosModelosDetalles.Exists(x => x.AsientoModeloDetalleCodigo.CodigoValor == EnumCodigosAsientosModelos.PreCancelacionesInteresNoCte.ToString()))
                {
                    interesesNoDevengado += interesesNoCteNoDevengado;
                    interesesNoCteNoDevengado = 0;
                }


                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, capitalCancelacion, modelo, EnumCodigosAsientosModelos.PreCancelaciones))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, capitalNoCteCancelacion, modelo, EnumCodigosAsientosModelos.PreCancelacionesNoCte))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, interesesNoDevengado, modelo, EnumCodigosAsientosModelos.PreCancelacionesInteresesACobrar))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, interesesNoCteNoDevengado, modelo, EnumCodigosAsientosModelos.PreCancelacionesInteresesACobrarNoCte))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, interesesNoDevengado, modelo, EnumCodigosAsientosModelos.PreCancelacionesInteres))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, interesesNoCteNoDevengado, modelo, EnumCodigosAsientosModelos.PreCancelacionesInteresNoCte))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.Cancelaciones.Sum(x => x.Bonificacion), modelo, EnumCodigosAsientosModelos.PreBonificaciones))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.Cancelaciones.Sum(x => x.ComisionCancelacion), modelo, EnumCodigosAsientosModelos.PreCancelacionesComisiones))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }


                // Sumo a las Cutoas Facturadas/Levantadas los Cargos por Exedidos
                cargosPeriodo += pParametro.CargosExcedidos.Where(x => x.Periodo == ultimaGeneracionCargos.Periodo).Sum(x => x.Importe);
                cargosAnteriores += pParametro.CargosExcedidos.Where(x => x.Periodo < ultimaGeneracionCargos.Periodo).Sum(x => x.Importe);
                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, cargosPeriodo, modelo, EnumCodigosAsientosModelos.CarDescuentosRecuperar))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, cargosAnteriores, modelo, EnumCodigosAsientosModelos.CarDescuentosNoRecuperados))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                //Servicios (Facturas)
                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ImporteSolicitudesPagos, modelo, EnumCodigosAsientosModelos.PreServicios))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
            //}

            //Valores Caja y Bancos
            pParametro.FilialPago.IdFilial = pParametro.FilialPago.IdFilialPago.Value;
            foreach (InterfazValoresImportes item in pValoresImportes)
            {
                #region "Asiento Caja y Banco"
                if ((item.IdTipoValor == (int)EnumTiposValores.Efectivo) // || pParametro.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaCaja, pParametro.FilialPago, item.IdTipoValor, 0, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }

                if ((item.IdTipoValor == (int)EnumTiposValores.Transferencia || item.IdTipoValor == (int)EnumTiposValores.Cheque)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, pParametro.FilialPago, item.IdTipoValor, item.IdBancoCuenta, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                if ((item.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.ValDepo, pParametro.FilialPago, item.IdTipoValor, 0, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                #endregion
            }

            return true;
        }

        public bool AsientoCancelacion(PrePrestamos pParametro, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdPrestamo;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pParametro.FechaConfirmacionCancelacion;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;

            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                resultado = false;
            }
            return resultado;
            //CtbAsientosContables asiento = new CtbAsientosContables();
            //asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            //asiento.IdRefTipoOperacion = pParametro.IdPrestamo;
            //asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            //asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            //asiento.FechaAsiento = pParametro.FechaConfirmacionCancelacion;// DateTime.Now;
            //asiento.UsuarioLogueado = pParametro.UsuarioLogueado;

            //SisProcesosProcesamiento ultimaGeneracionCargos = new SisProcesosProcesamiento();
            //ultimaGeneracionCargos.Proceso.IdProceso = (int)EnumSisProcesos.GeneracionCargos;
            //ultimaGeneracionCargos = ProcesosDatosF.ProcesosProcesamientoObtenerUltimoPeriodoProcesado(ultimaGeneracionCargos, bd, tran);
            //asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;
            //CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            ////Contabilizo las Cuotas No Levantadas/Sin Facturar
            //decimal capitalCancelacion = 0, capitalNoCteCancelacion = 0, saldoCancelacion = 0, saldoCancelacionNoCte = 0; ;
            //decimal interesesNoDevengado = 0;
            //decimal interesesNoCteNoDevengado = 0;
            //List<PrePrestamosCuotas> cuotasNoLevantadas = pParametro.PrestamosCuotas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Cancelada && !pParametro.CuotasPendientesCuentaCorriente.Any(y => y.IdReferenciaRegistro == x.IdPrestamoCuota)).ToList();

            //capitalCancelacion = cuotasNoLevantadas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Cancelada
            //        && x.CuotaFechaVencimiento <= pParametro.FechaConfirmacionCancelacion.AddMonths(12)).Sum(y => y.ImporteAmortizacion);
            //capitalNoCteCancelacion = cuotasNoLevantadas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Cancelada
            //            && x.CuotaFechaVencimiento > pParametro.FechaConfirmacionCancelacion.AddMonths(12)).Sum(y => y.ImporteAmortizacion);
            //interesesNoDevengado = cuotasNoLevantadas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Cancelada
            //        && x.CuotaFechaVencimiento <= pParametro.FechaConfirmacionCancelacion.AddMonths(12)).Sum(y => y.ImporteInteres);
            //interesesNoCteNoDevengado = cuotasNoLevantadas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Cancelada
            //        && x.CuotaFechaVencimiento > pParametro.FechaConfirmacionCancelacion.AddMonths(12)).Sum(y => y.ImporteInteres);

            //saldoCancelacion = capitalCancelacion + interesesNoDevengado;
            //saldoCancelacionNoCte = capitalNoCteCancelacion + interesesNoCteNoDevengado;
            //if (modelo.AsientosModelosDetalles.Exists(x => x.AsientoModeloDetalleCodigo.CodigoValor == EnumCodigosAsientosModelos.PreCancelacionesInteresesACobrarNoCte.ToString()))
            //{
            //    saldoCancelacionNoCte -= interesesNoCteNoDevengado;
            //}
            //if (modelo.AsientosModelosDetalles.Exists(x => x.AsientoModeloDetalleCodigo.CodigoValor == EnumCodigosAsientosModelos.PreCancelacionesInteresesACobrar.ToString()))
            //{
            //    saldoCancelacion -= interesesNoDevengado;
            //}
            //if (modelo.AsientosModelosDetalles.Exists(x => x.AsientoModeloDetalleCodigo.CodigoValor == EnumCodigosAsientosModelos.PreCancelacionesNoCte.ToString()))
            //{
            //    capitalNoCteCancelacion = saldoCancelacionNoCte;
            //    capitalCancelacion = saldoCancelacion;
            //}
            //else
            //{
            //    capitalCancelacion = saldoCancelacion + saldoCancelacionNoCte;
            //}

            //if (!modelo.AsientosModelosDetalles.Exists(x => x.AsientoModeloDetalleCodigo.CodigoValor == EnumCodigosAsientosModelos.PreCancelacionesInteresNoCte.ToString()))
            //{
            //    interesesNoDevengado += interesesNoCteNoDevengado;
            //    interesesNoCteNoDevengado = 0;
            //}

            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, capitalCancelacion, modelo, EnumCodigosAsientosModelos.PreCancelaciones))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, capitalNoCteCancelacion, modelo, EnumCodigosAsientosModelos.PreCancelacionesNoCte))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, interesesNoDevengado, modelo, EnumCodigosAsientosModelos.PreCancelacionesInteresesACobrar))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, interesesNoCteNoDevengado, modelo, EnumCodigosAsientosModelos.PreCancelacionesInteresesACobrarNoCte))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, interesesNoDevengado, modelo, EnumCodigosAsientosModelos.PreCancelacionesInteres))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, interesesNoCteNoDevengado, modelo, EnumCodigosAsientosModelos.PreCancelacionesInteresNoCte))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.Bonificacion, modelo, EnumCodigosAsientosModelos.PreBonificaciones))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ComisionCancelacion, modelo, EnumCodigosAsientosModelos.PreCancelacionesComisiones))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            ////Contabilizo las Cuotas Levantadas/Facturadas
            //decimal cargosPeriodo = pParametro.CuotasPendientesCuentaCorriente.Where(x => x.Periodo == ultimaGeneracionCargos.Periodo).Sum(x => x.ImporteContabilizar);
            //decimal cargosAnteriores = pParametro.CuotasPendientesCuentaCorriente.Where(x => x.Periodo < ultimaGeneracionCargos.Periodo).Sum(x => x.ImporteContabilizar);
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, cargosPeriodo, modelo, EnumCodigosAsientosModelos.CarDescuentosRecuperar))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, cargosAnteriores, modelo, EnumCodigosAsientosModelos.CarDescuentosNoRecuperados))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            //pParametro.FilialCancelacion.IdFilial = pParametro.FilialCancelacion.IdFilialCancelacion;
            //foreach (InterfazValoresImportes item in pValoresImportes)
            //{
            //    #region "Asiento Caja y Banco"
            //    if ((item.IdTipoValor == (int)EnumTiposValores.Efectivo) // || pParametro.TipoValorCancelacion.IdTipoValorCancelacion == (int)EnumTiposValores.ChequeTercero)
            //        && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaCaja, pParametro.FilialCancelacion, item.IdTipoValor, 0, bd, tran))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pParametro);
            //        return false;
            //    }
            //    if ((item.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
            //        && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.ValDepo))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pParametro);
            //        return false;
            //    }

            //    if ((item.IdTipoValor == (int)EnumTiposValores.Transferencia)
            //        && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, pParametro.FilialCancelacion, item.IdTipoValor, item.IdBancoCuenta, bd, tran))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pParametro);
            //        return false;
            //    }
            //    #endregion
            //}

            //if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            //return true;
        }
    }
}
