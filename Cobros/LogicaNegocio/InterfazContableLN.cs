using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contabilidad;
using Comunes.LogicaNegocio;
using Contabilidad.Entidades;
using Generales.Entidades;
using Cobros.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Comunes.Entidades;
using Cargos;
using ProcesosDatos;
using ProcesosDatos.Entidades;
using ProcesamientosDatos.Entidades;
using Cargos.Entidades;
using Servicio.AccesoDatos;

namespace Cobros.LogicaNegocio
{
    public class InterfazContableLN
    {
        public bool OrdenCobroAfiliado(CobOrdenesCobros pParametro, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdOrdenCobro;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pParametro.FechaConfirmacion.Value;// DateTime.Now;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;
            CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, cargo.Importe, modelo, cargo.Periodo == AyudaProgramacionLN.ObtenerPeriodo(DateTime.Now) ? EnumCodigosAsientosModelos.CarDescuentosRecuperar : EnumCodigosAsientosModelos.CarDescuentosNoRecuperados))
            SisProcesosProcesamiento filtro = new SisProcesosProcesamiento();
            filtro.Proceso.IdProceso = (int)EnumSisProcesos.GeneracionCargos;
            decimal ultimoPeriodoFacturado = ProcesosDatosF.ProcesosProcesamientoObtenerUltimoPeriodoProcesado(filtro, bd, tran).Periodo;
            decimal cargosPeriodo = pParametro.OrdenesCobrosDetalles.Where(x=> !x.CuentaCorriente.CargosACobrarTerceros && x.CuentaCorriente.Periodo==ultimoPeriodoFacturado).Sum(x => x.Importe);
            decimal cargosAnteriores = pParametro.OrdenesCobrosDetalles.Where(x => !x.CuentaCorriente.CargosACobrarTerceros && x.CuentaCorriente.Periodo < ultimoPeriodoFacturado).Sum(x => x.Importe);
            decimal cargosACobrarTerceros = pParametro.OrdenesCobrosDetalles.Where(x => x.CuentaCorriente.CargosACobrarTerceros ).Sum(x => x.Importe);
            
            if (cargosPeriodo > 0 && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, cargosPeriodo, modelo, EnumCodigosAsientosModelos.CarDescuentosRecuperar))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            if (cargosAnteriores > 0 && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, cargosAnteriores, modelo, EnumCodigosAsientosModelos.CarDescuentosNoRecuperados))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            if (cargosACobrarTerceros > 0 && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, cargosACobrarTerceros, modelo, EnumCodigosAsientosModelos.CarCargosACobrarTerceros))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            TGEFiliales filial = new TGEFiliales();
            filial = pParametro.FilialCobro;
            filial.IdFilial = pParametro.FilialCobro.IdFilialCobro;

            foreach (InterfazValoresImportes item in pValoresImportes)
            {
                #region "Asiento Caja y Banco"
                if ((item.IdTipoValor == (int)EnumTiposValores.Efectivo)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaCaja, filial, item.IdTipoValor, 0, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }

                if ((item.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.ValDepo))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }

                if ((item.IdTipoValor == (int)EnumTiposValores.Transferencia)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, filial, item.IdTipoValor, item.IdBancoCuenta, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }

                if ((item.IdTipoValor == (int)EnumTiposValores.TarjetaCredito)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TarjetasCreditos))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                #endregion
            }

            if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            return true;
        }

        public bool NotaCreditoCargos(CobOrdenesCobros pParametro, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdOrdenCobro;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pParametro.FechaConfirmacion.Value;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;

            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                resultado = false;
            }
            return resultado;
            #region Backup
            //asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;
            //CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            //SisProcesosProcesamiento ultimaGeneracionCargos = new SisProcesosProcesamiento();
            //ultimaGeneracionCargos.Proceso.IdProceso = (int)EnumSisProcesos.GeneracionCargos;
            //ultimaGeneracionCargos = ProcesosDatosF.ProcesosProcesamientoObtenerUltimoPeriodoProcesado(ultimaGeneracionCargos, bd, tran);

            //decimal cargosPeriodo = pParametro.OrdenesCobrosDetalles.Where(x => x.CuentaCorriente.Periodo == ultimaGeneracionCargos.Periodo).Sum(x => x.Importe);
            //decimal cargosAnteriores = pParametro.OrdenesCobrosDetalles.Where(x => x.CuentaCorriente.Periodo < ultimaGeneracionCargos.Periodo).Sum(x => x.Importe);
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

            //foreach (CobOrdenesCobrosDetalles item in pParametro.OrdenesCobrosDetalles)
            //{
            //    if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.CuentaCorriente.ImporteCobrado, modelo, EnumCodigosAsientosModelos.TablaTiposCargos, item.CuentaCorriente.TipoCargo.CuentaContable))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pParametro);
            //        return false;
            //    } 
            //}

            ////if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, cargosPeriodo + cargosAnteriores, modelo, EnumCodigosAsientosModelos.CarNotasCreditos))
            ////{
            ////    AyudaProgramacionLN.MapearError(asiento, pParametro);
            ////    return false;
            ////}

            //if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            //return true;
            #endregion
        }

        public bool OrdenCobroVarias(CobOrdenesCobros pParametro, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdOrdenCobro;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pParametro.FechaConfirmacion.Value;// DateTime.Now;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;
            CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            foreach (CobOrdenesCobrosDetalles detalle in pParametro.OrdenesCobrosDetalles)
            {
                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, detalle.Importe, modelo, EnumCodigosAsientosModelos.TablaConceptosContables, detalle.ConceptoContable.CuentaContable))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
            }

            TGEFiliales filial = new TGEFiliales();
            filial = pParametro.FilialCobro;
            filial.IdFilial = pParametro.FilialCobro.IdFilialCobro;
            foreach (InterfazValoresImportes item in pValoresImportes)
            {
                #region "Asiento Caja y Banco"
                if ((item.IdTipoValor == (int)EnumTiposValores.Efectivo)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaCaja, filial, item.IdTipoValor, 0, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }

                if ((item.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.ValDepo))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }

                if ((item.IdTipoValor == (int)EnumTiposValores.Transferencia)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, filial, item.IdTipoValor, item.IdBancoCuenta, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }

                if ((item.IdTipoValor == (int)EnumTiposValores.TarjetaCredito)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TarjetasCreditos))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }

                //if ((item.IdTipoValor == (int)EnumTiposValores.ChequeBancoSol)
                //    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.ChequeBancoSol))
                //{
                //    AyudaProgramacionLN.MapearError(asiento, pParametro);
                //    return false;
                //}
                #endregion
            }

            if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            return true;
        }

        public bool OrdenCobroFacturas(CobOrdenesCobros pParametro, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdOrdenCobro;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pParametro.FechaConfirmacion.Value;// DateTime.Now;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;

            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                resultado = false;
            }
            return resultado;

            //asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;
            //CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            ////foreach (CobOrdenesCobrosDetalles detalle in pParametro.OrdenesCobrosDetalles)
            ////{
            ////    if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, detalle.Importe, modelo, EnumCodigosAsientosModelos.VTAFacVenta))
            ////    {
            ////        AyudaProgramacionLN.MapearError(asiento, pParametro);
            ////        return false;
            ////    }
            ////}
            //// Facturas
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.OrdenesCobrosDetalles.Where(x=> !x.EsAnticipo).Sum(x => x.Importe > 0 ? x.Importe : 0), modelo, EnumCodigosAsientosModelos.VTAFacVenta))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.OrdenesCobrosDetalles.Sum(x => x.Importe < 0 ? x.Importe : 0) * -1, modelo, EnumCodigosAsientosModelos.VTANotaCreditoVenta))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}
            ////Nuevo Anticipo --> Anticipo a cuenta cte
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.OrdenesCobrosDetalles.Where(x => x.EsAnticipo).Sum(x => x.Importe > 0 ? x.Importe : 0), modelo, EnumCodigosAsientosModelos.VTAAnticiposClientes))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            //foreach (CobOrdenesCobrosTiposRetenciones retencion in pParametro.OrdenesCobrosTiposRetenciones)
            //{
            //    retencion.TipoRetencion.IdListaValorSistemaDetalle = retencion.TipoRetencion.IdTipoRetencion;
            //    if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, retencion.ImporteTotalRetencion, modelo, EnumCodigosAsientosModelos.TablaListaValorCuentaContable, retencion.TipoRetencion, bd, tran))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pParametro);
            //        return false;
            //    } 
            //}
            ////Descuento de Anticipos --> Aplicacion de Anticipos
            //int items = asiento.AsientosContablesDetalles.Count;
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.OrdenesCobrosAnticipos.Sum(x=>x.ImporteAplicado), modelo, EnumCodigosAsientosModelos.VTAAnticiposClientes))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}
            ////Si agrego el descuento de Anticipo, doy vuelta el Debe con el Haber
            //if (asiento.AsientosContablesDetalles.Count > items)
            //    if (asiento.AsientosContablesDetalles[items].Debe.HasValue)
            //    {
            //        asiento.AsientosContablesDetalles[items].Haber = asiento.AsientosContablesDetalles[items].Debe;
            //        asiento.AsientosContablesDetalles[items].Debe = default(decimal?);
            //    }
            //    else
            //    {
            //        asiento.AsientosContablesDetalles[items].Debe = asiento.AsientosContablesDetalles[items].Haber;
            //        asiento.AsientosContablesDetalles[items].Haber = default(decimal?);
            //    }
            ////Fin Descuento de Anticipos

            //if (pParametro.CargoDescuentoAfiliado)
            //{
            //    if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ImporteSubTotal, modelo, EnumCodigosAsientosModelos.CobCobrosPorCargos))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pParametro);
            //        return false;
            //    }
            //}
            //else
            //{
            //    TGEFiliales filial = new TGEFiliales();
            //    filial = pParametro.FilialCobro;
            //    filial.IdFilial = pParametro.FilialCobro.IdFilialCobro;
            //    foreach (InterfazValoresImportes item in pValoresImportes)
            //    {
            //        #region "Asiento Caja y Banco"
            //        if ((item.IdTipoValor == (int)EnumTiposValores.Efectivo)
            //            && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaCaja, filial, item.IdTipoValor, 0, bd, tran))
            //        {
            //            AyudaProgramacionLN.MapearError(asiento, pParametro);
            //            return false;
            //        }
            //        else if ((item.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
            //            && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.ValDepo))
            //        {
            //            AyudaProgramacionLN.MapearError(asiento, pParametro);
            //            return false;
            //        }
            //        else if ((item.IdTipoValor == (int)EnumTiposValores.Transferencia)
            //            && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, filial, item.IdTipoValor, item.IdBancoCuenta, bd, tran))
            //        {
            //            AyudaProgramacionLN.MapearError(asiento, pParametro);
            //            return false;
            //        }
            //        else if ((item.IdTipoValor == (int)EnumTiposValores.TarjetaCredito)
            //        && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TarjetasCreditos))
            //        {
            //            AyudaProgramacionLN.MapearError(asiento, pParametro);
            //            return false;
            //        }
            //        #endregion
            //    }
            //}

            //if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            //return true;
        }

        public bool OrdenCobroFacturasPrestamos(CobOrdenesCobros pParametro, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdOrdenCobro;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pParametro.FechaConfirmacion.Value;// DateTime.Now;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;

            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                resultado = false;
            }

            asiento.IdTipoOperacion = pParametro.Prestamo.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.Prestamo.IdPrestamo;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pParametro.Prestamo.FechaPrestamo;// DateTime.Now;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                resultado = false;
            }

            return resultado;

            //asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;
            //CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            //// Facturas
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.OrdenesCobrosDetalles.Sum(x => x.Importe > 0 ? x.Importe : 0), modelo, EnumCodigosAsientosModelos.VTAFacVenta))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.OrdenesCobrosDetalles.Sum(x => x.Importe < 0 ? x.Importe : 0) * -1, modelo, EnumCodigosAsientosModelos.VTANotaCreditoVenta))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            //foreach (CobOrdenesCobrosTiposRetenciones retencion in pParametro.OrdenesCobrosTiposRetenciones)
            //{
            //    retencion.TipoRetencion.IdListaValorSistemaDetalle = retencion.TipoRetencion.IdTipoRetencion;
            //    if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, retencion.ImporteTotalRetencion, modelo, EnumCodigosAsientosModelos.TablaListaValorCuentaContable, retencion.TipoRetencion, bd, tran))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pParametro);
            //        return false;
            //    }
            //}

            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.OrdenesCobrosAnticipos.Sum(x => x.ImporteAplicado), modelo, EnumCodigosAsientosModelos.VTAAnticiposClientes))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            //if (pParametro.Prestamo == null || pParametro.Prestamo.IdPrestamo==0)
            //{
            //    pParametro.CodigoMensaje = "OrdenCobroPrestamoNoEncontrado";
            //    return false;
            //}

            //CtbAsientosContables asientoPrestamo = new CtbAsientosContables();
            //if (!Prestamos.PrePrestamosF.AsientoOtorgamientoArmar(asiento, pParametro.Prestamo, new List<InterfazValoresImportes>(), bd, tran))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            //asiento.AsientosContablesDetalles.AddRange(asientoPrestamo.AsientosContablesDetalles);

            //if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            return true;
        }

        public bool OrdenCobroFacturasAdelanto(CobOrdenesCobros pParametro, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdOrdenCobro;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pParametro.FechaConfirmacion.Value;// DateTime.Now;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;
            CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ImporteTotal, modelo, EnumCodigosAsientosModelos.VTAAnticiposClientes))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }
            
            TGEFiliales filial = new TGEFiliales();
            filial = pParametro.FilialCobro;
            filial.IdFilial = pParametro.FilialCobro.IdFilialCobro;
            foreach (InterfazValoresImportes item in pValoresImportes)
            {
                #region "Asiento Caja y Banco"
                if ((item.IdTipoValor == (int)EnumTiposValores.Efectivo)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaCaja, filial, item.IdTipoValor, 0, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }

                if ((item.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.ValDepo))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }

                if ((item.IdTipoValor == (int)EnumTiposValores.Transferencia)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, filial, item.IdTipoValor, item.IdBancoCuenta, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }

                if ((item.IdTipoValor == (int)EnumTiposValores.TarjetaCredito)
                && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TarjetasCreditos))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                #endregion
            }

            if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            return true;
        }

        public bool CancelacionAnticipadaCuotaPrestamo(CobOrdenesCobros pParametro, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            CtbAsientosContables asiento = new CtbAsientosContables();
            
            asiento.IdRefTipoOperacion = pParametro.IdOrdenCobro;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pParametro.FechaConfirmacion.Value;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;

            asiento.IdTipoOperacion = (int)EnumTGETiposOperaciones.PrestamosLargoPlazoCancelacion;
            asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;
            CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            
            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ImporteTotal, modelo, EnumCodigosAsientosModelos.PreCancelaciones))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            pParametro.FilialCobro.IdFilial = pParametro.FilialCobro.IdFilialCobro;
            foreach (InterfazValoresImportes item in pValoresImportes)
            {
                #region "Asiento Caja y Banco"
                if ((item.IdTipoValor == (int)EnumTiposValores.Efectivo)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaCaja, pParametro.FilialCobro, item.IdTipoValor, 0, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                else if ((item.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.ValDepo))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                else if ((item.IdTipoValor == (int)EnumTiposValores.Transferencia)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, pParametro.FilialCobro, item.IdTipoValor, item.IdBancoCuenta, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                #endregion
            }

            if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            return true;
        }
    }
}
