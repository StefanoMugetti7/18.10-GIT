using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bancos.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Contabilidad.Entidades;
using Contabilidad;
using Comunes.LogicaNegocio;
using Generales.Entidades;
using Comunes.Entidades;
using Servicio.AccesoDatos;

namespace Bancos.LogicaNegocio
{
    public class InterfazContableLN
    {
        public bool IngresosEgresosPorConceptos(TESBancosCuentasMovimientos pParametro, Database bd, DbTransaction tran)
        {
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdBancoCuentaMovimiento;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pParametro.FechaConfirmacionBanco;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;
            CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, pParametro.BancoCuenta.Filial, (int)EnumTiposValores.Transferencia, pParametro.BancoCuenta.IdBancoCuenta, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.Importe, modelo, EnumCodigosAsientosModelos.TablaConceptosContables, pParametro.ConceptoContable.CuentaContable))
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

        //public bool AcreditacionRemesas(TESBancosCuentasMovimientos pParametro, Database bd, DbTransaction tran)
        //{
        //    CtbAsientosContables asiento = new CtbAsientosContables();
        //    asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
        //    asiento.IdRefTipoOperacion = pParametro.IdBancoCuentaMovimiento;
        //    asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
        //    asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
        //    asiento.FechaAsiento = DateTime.Now;
        //    asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(bd, tran).IdEjercicioContable;
        //    //asiento.NumeroAsiento = ContabilidadF.AsientosContablesObtenerNumeroAsiento;
        //    asiento.UsuarioLogueado = pParametro.UsuarioLogueado;

        //    CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(pParametro.TipoOperacion, bd, tran);

        //    if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, pParametro.BancoCuenta.Filial, (int)EnumTiposValores.Transferencia, pParametro.BancoCuenta.IdBancoCuenta, bd, tran ))
        //    {
        //        AyudaProgramacionLN.MapearError(asiento, pParametro);
        //        return false;
        //    }
        //    if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.Importe, modelo, EnumCodigosAsientosModelos.HabPoderes))
        //    {
        //        AyudaProgramacionLN.MapearError(asiento, pParametro);
        //        return false;
        //    }


        //    if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
        //    {
        //        AyudaProgramacionLN.MapearError(asiento, pParametro);
        //        return false;
        //    }

        //    return true;
        //}

        //public bool DebitosRemesas(TESBancosCuentasMovimientos pParametro, Database bd, DbTransaction tran)
        //{
        //    CtbAsientosContables asiento = new CtbAsientosContables();
        //    asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
        //    asiento.IdRefTipoOperacion = pParametro.IdBancoCuentaMovimiento;
        //    asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
        //    asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
        //    asiento.FechaAsiento = DateTime.Now;
        //    asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(bd, tran).IdEjercicioContable;
        //    //asiento.NumeroAsiento = ContabilidadF.AsientosContablesObtenerNumeroAsiento;
        //    asiento.UsuarioLogueado = pParametro.UsuarioLogueado;

        //    CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(pParametro.TipoOperacion, bd, tran);

        //    if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, pParametro.BancoCuenta.Filial, (int)EnumTiposValores.Transferencia, pParametro.BancoCuenta.IdBancoCuenta, bd, tran))
        //    {
        //        AyudaProgramacionLN.MapearError(asiento, pParametro);
        //        return false;
        //    }
        //    if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.Importe, modelo, EnumCodigosAsientosModelos.HabPoderesDevolver))
        //    {
        //        AyudaProgramacionLN.MapearError(asiento, pParametro);
        //        return false;
        //    }


        //    if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
        //    {
        //        AyudaProgramacionLN.MapearError(asiento, pParametro);
        //        return false;
        //    }

        //    return true;
        //}

        internal bool TransferenciaCuentasInternas(TESBancosCuentasMovimientos pMovDestino, TESBancosCuentasMovimientos pMovOrigen, Database bd, DbTransaction tran)
        {
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pMovDestino.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pMovDestino.IdBancoCuentaMovimiento;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pMovDestino.FechaConfirmacionBanco;
            asiento.UsuarioLogueado = pMovDestino.UsuarioLogueado;
            asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;
            CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pMovDestino.Importe, modelo, EnumCodigosAsientosModelos.TablaBancoDestino, pMovDestino.BancoCuenta.Filial, (int)EnumTiposValores.Transferencia, pMovDestino.BancoCuenta.IdBancoCuenta, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pMovDestino);
                return false;
            }
            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pMovOrigen.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, pMovOrigen.BancoCuenta.Filial, (int)EnumTiposValores.Transferencia, pMovOrigen.BancoCuenta.IdBancoCuenta, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pMovDestino);
                return false;
            }

            if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pMovDestino);
                return false;
            }

            return true;
        }

        internal bool TransferenciaDesdeTesoreria(TESBancosCuentasMovimientos pMov, TGEFiliales pFilial, Database bd, DbTransaction tran)
        {
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pMov.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pMov.IdBancoCuentaMovimiento;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pMov.FechaConfirmacionBanco; //DateTime.Now;
            asiento.UsuarioLogueado = pMov.UsuarioLogueado;
            asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;

            CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pMov.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, pMov.BancoCuenta.Filial, (int)EnumTiposValores.Transferencia, pMov.BancoCuenta.IdBancoCuenta, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pMov);
                return false;
            }
            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pMov.Importe, modelo, EnumCodigosAsientosModelos.TablaCaja, pFilial, (int)EnumTiposValores.Efectivo, 0, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pMov);
                return false;
            }

            if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pMov);
                return false;
            }

            return true;
        }

        internal bool AcreditacionCheque(TESBancosCuentasMovimientos pMov, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = (int)EnumTGETiposOperaciones.AcreditacionCheque;
            asiento.IdRefTipoOperacion = pMov.IdBancoCuentaMovimiento;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pMov.FechaConfirmacionBanco; //DateTime.Now;
            asiento.UsuarioLogueado = pMov.UsuarioLogueado;

            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
            {
                AyudaProgramacionLN.MapearError(asiento, pMov);
                resultado = false;
            }
            return resultado;

            //CtbAsientosContables asiento = new CtbAsientosContables();
            //asiento.IdTipoOperacion = (int)EnumTGETiposOperaciones.AcreditacionCheque;
            //asiento.IdRefTipoOperacion = pMov.IdBancoCuentaMovimiento;
            //asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            //asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            //asiento.FechaAsiento = pMov.FechaConfirmacionBanco; //DateTime.Now;
            //asiento.UsuarioLogueado = pMov.UsuarioLogueado;
            //asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;

            //CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pMov.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, pMov.BancoCuenta.Filial, (int)EnumTiposValores.Transferencia, pMov.BancoCuenta.IdBancoCuenta, bd, tran))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pMov);
            //    return false;
            //}
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pMov.Importe, modelo, EnumCodigosAsientosModelos.ValDepo))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pMov);
            //    return false;
            //}

            //if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pMov);
            //    return false;
            //}

            //return true;
        }

        internal bool RechazoCheque(TESBancosCuentasMovimientos pMov, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = (int)EnumTGETiposOperaciones.RechazoCheque;
            asiento.IdRefTipoOperacion = pMov.IdBancoCuentaMovimiento;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pMov.FechaConfirmacionBanco; //DateTime.Now;
            asiento.UsuarioLogueado = pMov.UsuarioLogueado;

            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
            {
                AyudaProgramacionLN.MapearError(asiento, pMov);
                resultado = false;
            }
            return resultado;
        }
        internal bool ImpuestoCreditoDebito(TESBancosCuentasMovimientos pMov, Database bd, DbTransaction tran)
        {
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pMov.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pMov.IdBancoCuentaMovimiento;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pMov.FechaConfirmacionBanco; //DateTime.Now;
            asiento.UsuarioLogueado = pMov.UsuarioLogueado;
            asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;

            CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pMov.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, pMov.BancoCuenta.Filial, (int)EnumTiposValores.Transferencia, pMov.BancoCuenta.IdBancoCuenta, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pMov);
                return false;
            }
            EnumCodigosAsientosModelos codigoAMDet = pMov.TipoOperacion.TipoMovimiento.IdTipoMovimiento == (int)EnumTGETiposMovimientos.Credito ? EnumCodigosAsientosModelos.ImpCredito : EnumCodigosAsientosModelos.ImpDebito;
            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pMov.Importe, modelo, codigoAMDet))
            {
                AyudaProgramacionLN.MapearError(asiento, pMov);
                return false;
            }

            if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pMov);
                return false;
            }

            return true;
        }

        public bool ConciliacionCobranzaExterna(TESCobranzasExternasConciliaciones pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = (int)EnumTGETiposOperaciones.ConciliacionCobranzaExterna;
            asiento.IdRefTipoOperacion = pParametro.IdCobranzaExternaConciliacion;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pParametro.FechaConfirmacion;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;

            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                resultado = false;
            }
            return resultado;

            /*
            CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ImporteNeto, modelo, EnumCodigosAsientosModelos.TablaBanco, null, (int)EnumTiposValores.Transferencia, pParametro.IdBancoCuenta, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ImportePresentado, modelo, EnumCodigosAsientosModelos.TarjetasCreditos))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            foreach (TESCobranzasExternasConciliacionesDeducciones deduccion in pParametro.CobranzaExternaConciliacionDeducciones)
            {
                deduccion.TipoDeduccion.IdListaValorSistemaDetalle = deduccion.TipoDeduccion.IdTipoDeduccion;
                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, deduccion.ImporteDeduccion, modelo, EnumCodigosAsientosModelos.TablaListaValorCuentaContable, deduccion.TipoDeduccion, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
            }
            

            if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }
            */
        }

        public bool PlazosFijosPropiosAgregar(TESPlazosFijos pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = (int)EnumTGETiposOperaciones.PlazosFijosPropios;
            asiento.IdRefTipoOperacion = pParametro.IdPlazoFijo;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pParametro.FechaInicioVigencia;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;

            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                resultado = false;
            }
            return resultado;
        }

        public bool PlazosFijosPropiosAnular(TESPlazosFijos pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = (int)EnumTGETiposOperaciones.PlazosFijosPropios;
            asiento.IdRefTipoOperacion = pParametro.IdPlazoFijo;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;

            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbAsientosContablesAnular"))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                resultado = false;
            }
            return resultado;
        }

        public bool PlazosFijosPropiosAcreditacion(TESPlazosFijos pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = (int)EnumTGETiposOperaciones.PlazosFijosPropiosAcreditacion;
            asiento.IdRefTipoOperacion = pParametro.IdPlazoFijo;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pParametro.FechaVencimiento;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;

            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                resultado = false;
            }
            return resultado;
        }
    }
}
