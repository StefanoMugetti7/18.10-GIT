using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contabilidad.Entidades;
using Comunes.Entidades;
using Bancos.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Contabilidad;
using Comunes.LogicaNegocio;
using Tesorerias.Entidades;
using Generales.Entidades;
using Servicio.AccesoDatos;

namespace Tesorerias.LogicaNegocio
{
    public class InterfazContableLN
    {
        internal bool TransferenciaDesdeBancos(TESTesorerias pTesoreria, TESTesoreriasMovimientos pTesMov, TESBancosCuentasMovimientos pMov, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pTesMov.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pMov.IdBancoCuentaMovimiento;
            asiento.Filial = pTesoreria.Filial;
            asiento.FechaAsiento = pTesoreria.FechaAbrir;
            asiento.UsuarioLogueado = pTesMov.UsuarioLogueado;
            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
            {
                AyudaProgramacionLN.MapearError(asiento, pTesoreria);
                resultado = false;
            }
            return resultado;

            //CtbAsientosContables asiento = new CtbAsientosContables();
            //asiento.IdTipoOperacion = pTesMov.TipoOperacion.IdTipoOperacion;
            //asiento.IdRefTipoOperacion = pMov.IdBancoCuentaMovimiento;
            //asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            //asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            //asiento.FechaAsiento = pTesoreria.FechaAbrir;
            //asiento.UsuarioLogueado = pTesMov.UsuarioLogueado;
            //asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;

            //CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pTesMov.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, pMov.BancoCuenta.Filial, (int)EnumTiposValores.Transferencia, pMov.BancoCuenta.IdBancoCuenta, bd, tran))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pTesoreria);
            //    return false;
            //}
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pTesMov.Importe, modelo, EnumCodigosAsientosModelos.TablaCaja, pTesoreria.Filial, (int)EnumTiposValores.Efectivo, 0, bd, tran))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pTesoreria);
            //    return false;
            //}

            //if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pTesoreria);
            //    return false;
            //}

            //return true;
        }

        internal bool IngresosEgresosPorCajas(TESCajas pCaja, TESCajasMovimientos pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdCajaMovimiento;
            asiento.Filial = pCaja.Tesoreria.Filial;
            asiento.FechaAsiento = pParametro.Fecha;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
            {
                AyudaProgramacionLN.MapearError(asiento, pCaja);
                resultado = false;
            }
            return resultado;

            //CtbAsientosContables asiento = new CtbAsientosContables();
            //asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            //asiento.IdRefTipoOperacion = pParametro.IdCajaMovimiento;
            //asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            //asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            //asiento.FechaAsiento = pCaja.FechaAbrir;
            //asiento.DetalleGeneral = pParametro.Descripcion;
            //asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            //asiento.Filial = pCaja.Tesoreria.Filial;
            //asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;

            //CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            //foreach (TESCajasMovimientosConceptosContables movConcepto in pParametro.CajasMovimientosConceptosContables)
            //{
            //    if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, movConcepto.Importe, modelo, EnumCodigosAsientosModelos.TablaConceptosContables, movConcepto.ConceptoContable.CuentaContable))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pCaja);
            //        return false;
            //    }
            //}

            //TGEFiliales filial = new TGEFiliales();
            //filial = pCaja.Tesoreria.Filial;

            //foreach (var val in pParametro.CajasMovimientosValores)
            //{
            //    #region "Asiento Caja y Banco"
            //    if (val.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo
            //        && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, val.Importe, modelo, EnumCodigosAsientosModelos.TablaCaja, filial, val.TipoValor.IdTipoValor, 0, bd, tran))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pCaja);
            //        return false;
            //    }

            //    if (val.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero
            //        && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, val.Importe, modelo, EnumCodigosAsientosModelos.ValDepo))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pCaja);
            //        return false;
            //    }

            //    if (val.TipoValor.IdTipoValor == (int)EnumTiposValores.TarjetaCredito
            //        && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, val.Importe, modelo, EnumCodigosAsientosModelos.TarjetasCreditos))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pCaja);
            //        return false;
            //    }

            //    if (val.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque)
            //    {
            //        var agrupado = val.Cheques.GroupBy(x => new { x.IdBancoCuenta })
            //    .Select(y => new { IdBancoCuenta = y.Key.IdBancoCuenta, Importe = y.Sum(z => z.Importe) });

            //        foreach (var cuenta in agrupado)
            //        {
            //            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, cuenta.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, filial, val.TipoValor.IdTipoValor, cuenta.IdBancoCuenta, bd, tran))
            //            {
            //                AyudaProgramacionLN.MapearError(asiento, pCaja);
            //                return false;
            //            }
            //        }
            //    }
            //    if (val.TipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia)
            //    {
            //        var agrupado = val.BancosCuentasMovimientos.GroupBy(x => new { x.BancoCuenta.IdBancoCuenta })
            //    .Select(y => new { IdBancoCuenta = y.Key.IdBancoCuenta, Importe = y.Sum(z => z.Importe) });

            //        foreach (var cuenta in agrupado)
            //        {
            //            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, cuenta.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, filial, val.TipoValor.IdTipoValor, cuenta.IdBancoCuenta, bd, tran))
            //            {
            //                AyudaProgramacionLN.MapearError(asiento, pCaja);
            //                return false;
            //            }
            //        }
            //    }

            //    #endregion
            //}

            //if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pCaja);
            //    return false;
            //}

            //return true;
        }
    }
}