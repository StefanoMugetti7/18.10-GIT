using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CuentasPagar.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Contabilidad.Entidades;
using Generales.Entidades;
using Contabilidad;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using Servicio.AccesoDatos;

namespace CuentasPagar.LogicaNegocio
{
    public class InterfazContableLN 
    {
        public bool AgregarSolicitudPago(CapSolicitudPago pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdSolicitudPago;
            asiento.Filial.IdFilial = pParametro.IdFilial;
            asiento.FechaAsiento = pParametro.FechaContable.Value;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                resultado = false;
            }
            return resultado;

            //CtbAsientosContables asiento = new CtbAsientosContables();
            //asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            //asiento.IdRefTipoOperacion = pParametro.IdSolicitudPago;
            //asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            //asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            //asiento.FechaAsiento =pParametro.FechaContable.Value;
            //asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            //asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;
            //CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            ////Proveedores a Pagar o Cuenta Contable del Proveedor
            //if (pParametro.Entidad.IdCuentaContable.HasValue && pParametro.Entidad.IdCuentaContable> 0 )
            //{
            //    CtbCuentasContables cuenta = new CtbCuentasContables() { IdCuentaContable=pParametro.Entidad.IdCuentaContable.Value};
            //    if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ImporteTotal, modelo, EnumCodigosAsientosModelos.CapSPProv, cuenta))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pParametro);
            //        return false;
            //    }
            //}
            //else if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ImporteTotal, modelo, EnumCodigosAsientosModelos.CapSPProv))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}
            ////Percepciones
            //foreach (CapSolicitudPagoTipoPercepcion percepcion in pParametro.SolicitudPagoTiposPercepciones)
            //{
            //    percepcion.TipoPercepcion.IdListaValorSistemaDetalle = percepcion.TipoPercepcion.IdTipoPercepcion;
            //    if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, percepcion.Importe, modelo, EnumCodigosAsientosModelos.TablaListaValorCuentaContable, percepcion.TipoPercepcion, bd, tran))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pParametro);
            //        return false;
            //    }
            //}
            ////Productos o Servicios
            //foreach (CapSolicitudPagoDetalles item in pParametro.SolicitudPagoDetalles)
            //{
            //    if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Subtotal+item.PrecioNoGravado, modelo, EnumCodigosAsientosModelos.TablaCmpFliaProducto, item.Producto.Familia.CuentaContable, item.CentroCostoProrrateo ))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pParametro);
            //        return false;
            //    }
            //}
            ////IVA Credito Fiscal
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.IvaTotal, modelo, EnumCodigosAsientosModelos.IVACreditoFiscal))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            //if (pParametro.TiposFacturas.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoA
            //    || pParametro.TiposFacturas.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoB
            //    || pParametro.TiposFacturas.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoC)
            //{
            //    foreach (CtbAsientosContablesDetalles detalle in asiento.AsientosContablesDetalles)
            //    {
            //        if (detalle.Haber > 0)
            //        {
            //            detalle.Debe = detalle.Haber;
            //            detalle.Haber = 0;
            //        }
            //        else
            //        {
            //            detalle.Haber = detalle.Debe;
            //            detalle.Debe = 0;
            //        }                    
            //    }
            //}

            //if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}
            
            //return true;
        }

        public bool AnularSolicitudPago(CapSolicitudPago pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdSolicitudPago;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbAsientosContablesAnular"))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                resultado = false;
            }
            return resultado;
            //TGETiposOperaciones tipoOper = new TGETiposOperaciones();
            //if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosCompras)
            //    tipoOper.IdTipoOperacion = (int)EnumTGETiposOperaciones.SolicitudesPagosComprasAnular;
            //if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosAFIP)
            //    tipoOper.IdTipoOperacion = (int)EnumTGETiposOperaciones.SolicitudesPagosAFIPAnular;
            //else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosTerceros)
            //    tipoOper.IdTipoOperacion = (int)EnumTGETiposOperaciones.SolicitudesPagosTercerosAnular;
            //else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosAnticipos)
            //    tipoOper.IdTipoOperacion = (int)EnumTGETiposOperaciones.SolicitudesPagosAnticiposAnular;

            //CtbAsientosContables anular = new CtbAsientosContables();
            //anular.IdRefTipoOperacion = pParametro.IdSolicitudPago;
            //anular.IdTipoOperacion = tipoOper.IdTipoOperacion;
            //anular.UsuarioLogueado = pParametro.UsuarioLogueado;

            //if (!ContabilidadF.AsientosContablesRevertirAsiento(anular, pParametro.TipoOperacion, bd, tran))
            //{
            //    AyudaProgramacionLN.MapearError(anular, pParametro);
            //    return false;
            //}

            //if (!ContabilidadF.AsientosContablesAgregar(anular, bd, tran))
            //{
            //    AyudaProgramacionLN.MapearError(anular, pParametro);
            //    return false;
            //}

            //return true;
        }

        public bool OrdenesPagos(CapOrdenesPagos pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdOrdenPago;
            asiento.Filial.IdFilial = pParametro.FilialPago.IdFilialPago.Value;
            asiento.FechaAsiento = pParametro.FechaPago.Value;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                resultado = false;
            }
            return resultado;

            ////Si la suma de los comprobantes (Facturas y NC) = 0 No contabilizo nada
            ////if (pParametro.SolicitudesPagos.Where(y => y.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosCompras
            ////                || y.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosAnticipos
            ////                || y.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosAFIP
            ////                || y.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosTerceros
            ////                ).Sum(x => x.ImporteParcial) == 0)
            ////{ 
            ////    return true; 
            ////}
            //if (pParametro.SolicitudesPagos.Sum(x => x.ImporteParcial) == 0)
            //{
            //    return true;
            //}

            ////Cabecera Asiento
            //CtbAsientosContables asiento = new CtbAsientosContables();
            //asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            //asiento.IdRefTipoOperacion = pParametro.IdOrdenPago;
            //asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            //asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            //asiento.FechaAsiento = pParametro.FechaConfirmacion.Value;// DateTime.Now;
            //asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            //asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;

            //CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            //var agrupado = pParametro.OrdenesPagosValore.GroupBy(x => new { x.TipoValor.IdTipoValor, x.BancoCuenta.IdBancoCuenta, x.ListaValorSistemaDetalle.IdListaValorSistemaDetalle })
            //    .Select(y => new { IdTipoValor = y.Key.IdTipoValor, IdBancoCuenta = y.Key.IdBancoCuenta, IdListaValorSistemaDetalle = y.Key.IdListaValorSistemaDetalle, Importe = y.Sum(z => z.Importe) });

            //TGEFiliales filial = new TGEFiliales();
            //filial = pParametro.FilialPago;
            //filial.IdFilial = pParametro.FilialPago.IdFilialPago.Value;
            //TGEListasValoresSistemasDetalles listaValorSisDet;
            //foreach (var val in agrupado)
            //{
            //    #region "Asiento Caja y Banco"
            //    if (val.IdTipoValor == (int)EnumTiposValores.Efectivo
            //        && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, val.Importe, modelo, EnumCodigosAsientosModelos.TablaCaja, filial, val.IdTipoValor, 0, bd, tran))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pParametro);
            //        return false;
            //    }
            //    else if (val.IdTipoValor == (int)EnumTiposValores.ChequeTercero
            //        && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, val.Importe, modelo, EnumCodigosAsientosModelos.ValDepo))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pParametro);
            //        return false;
            //    }
            //    else if ((val.IdTipoValor == (int)EnumTiposValores.Cheque
            //        || val.IdTipoValor == (int)EnumTiposValores.Transferencia
            //        || val.IdTipoValor == (int)EnumTiposValores.TarjetaCredito)
            //        && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, val.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, filial, val.IdTipoValor, val.IdBancoCuenta, bd, tran))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pParametro);
            //        return false;
            //    }
            //    else if (val.IdTipoValor == (int)EnumTiposValores.AfipTiposPercepciones
            //            || val.IdTipoValor == (int)EnumTiposValores.AfipTiposRetenciones)
            //    {
            //        listaValorSisDet = new TGEListasValoresSistemasDetalles();
            //        listaValorSisDet.IdListaValorSistemaDetalle = val.IdListaValorSistemaDetalle;
            //        if(!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, val.Importe, modelo, EnumCodigosAsientosModelos.TablaListaValorCuentaContable, listaValorSisDet, bd, tran ))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pParametro);
            //        return false;
            //    }
            //    }
            //    #endregion
            //}

            //decimal importeTotal = pParametro.SolicitudesPagos.Where(y => y.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosCompras
            //                || y.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosAnticipos
            //                || y.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosAFIP
            //                || y.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosTerceros
            //                ).Sum(x => x.ImporteParcial);

            //if (pParametro.Entidad.IdCuentaContable.HasValue && pParametro.Entidad.IdCuentaContable > 0)
            //{
            //    CtbCuentasContables cuenta = new CtbCuentasContables() { IdCuentaContable = pParametro.Entidad.IdCuentaContable.Value };
            //    if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, importeTotal, modelo, EnumCodigosAsientosModelos.CapSPProv, cuenta))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pParametro);
            //        return false;
            //    } 
            //}
            //else if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, importeTotal, modelo, EnumCodigosAsientosModelos.CapSPProv))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.SolicitudesAnticipos.Sum(x => x.ImporteTotal), modelo, EnumCodigosAsientosModelos.CapSPAnticipoDescontar))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.SolicitudesPagos.Where(y => y.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosSubsidios).Sum(x => x.ImporteParcial), modelo, EnumCodigosAsientosModelos.SubPagar))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            //foreach (CapOrdenesPagosTiposRetenciones retencion in pParametro.OrdenesPagosTiposRetenciones)
            //{
            //    retencion.TipoRetencion.IdListaValorSistemaDetalle = retencion.TipoRetencion.IdTipoRetencion;
            //    if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, retencion.ImporteTotalRetencion, modelo, EnumCodigosAsientosModelos.TablaListaValorCuentaContable, retencion.TipoRetencion, bd, tran))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pParametro);
            //        return false;
            //    }
            //}

            //if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            //return true;
        }

        public bool AgregarSolicitudPagoSubsidios(CapSolicitudPago pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdSolicitudPago;
            asiento.Filial.IdFilial = pParametro.IdFilial;
            asiento.FechaAsiento = pParametro.FechaContable.Value;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                resultado = false;
            }
            return resultado;

            //CtbAsientosContables asiento = new CtbAsientosContables();
            //asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            //asiento.IdRefTipoOperacion = pParametro.IdSolicitudPago;
            //asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            //asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            //asiento.FechaAsiento = pParametro.FechaContable.Value;
            //asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            //asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;
            //CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            ////SubSidios a Pagar
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ImporteTotal, modelo, EnumCodigosAsientosModelos.SubPagar))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}
            ////Gastos de Subsidios
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ImporteTotal, modelo, EnumCodigosAsientosModelos.SubGastos))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}
            
            //if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            //return true;
        }

        public bool AnularSolicitudPagoSubsidios(CapSolicitudPago pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdSolicitudPago;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbAsientosContablesAnular"))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                resultado = false;
            }
            return resultado;

            //CtbAsientosContables anular = new CtbAsientosContables();
            //anular.IdRefTipoOperacion = pParametro.IdSolicitudPago;
            //anular.IdTipoOperacion = (int)EnumTGETiposOperaciones.SolicitudesPagosSubsidiosAnular;
            //anular.UsuarioLogueado = pParametro.UsuarioLogueado;

            //if (!ContabilidadF.AsientosContablesRevertirAsiento(anular, new TGETiposOperaciones() { IdTipoOperacion = (int)EnumTGETiposOperaciones.SolicitudesPagosSubsidios }, bd, tran))
            //{
            //    AyudaProgramacionLN.MapearError(anular, pParametro);
            //    return false;
            //}

            //if (!ContabilidadF.AsientosContablesAgregar(anular, bd, tran))
            //{
            //    AyudaProgramacionLN.MapearError(anular, pParametro);
            //    return false;
            //}

            //return true;
        }

        public bool AgregarSolicitudPagoAnticipo(CapSolicitudPago pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdSolicitudPago;
            asiento.Filial.IdFilial = pParametro.IdFilial;
            asiento.FechaAsiento = pParametro.FechaContable.Value;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                resultado = false;
            }
            return resultado;

            //CtbAsientosContables asiento = new CtbAsientosContables();
            //asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            //asiento.IdRefTipoOperacion = pParametro.IdSolicitudPago;
            //asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            //asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            //asiento.FechaAsiento = DateTime.Now;//pParametro.FechaContable.Value;
            //asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            //asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;

            //CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            ////Proveedores a Pagar
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ImporteTotal, modelo, EnumCodigosAsientosModelos.CapSPAnticipo))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ImporteTotal, modelo, EnumCodigosAsientosModelos.CapSPProv))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
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
