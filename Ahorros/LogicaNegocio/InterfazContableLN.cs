using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Ahorros.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Generales.Entidades;
using Contabilidad.Entidades;
using Contabilidad;
using Comunes.LogicaNegocio;
using Servicio.AccesoDatos;

namespace Ahorros.LogicaNegocio
{
    internal class InterfazContableLN
    {

        public bool AsientoDeposito(AhoCuentasMovimientos pParametro, DateTime pFecha, List<InterfazValoresImportes> pValoresImportes, AhoCuentasMovimientos pComisiones, Database bd, DbTransaction tran)
        {
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdCuentaMovimiento;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pFecha;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;

            CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            if (pParametro.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo || pParametro.TipoValor.IdTipoValor == (int)EnumTiposValores.Transferencia)
            {
                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.Importe - pComisiones.Importe, modelo, EnumCodigosAsientosModelos.AhoDepExt))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pComisiones.Importe, modelo, EnumCodigosAsientosModelos.AhoComi))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
            }
            else if (pParametro.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
            {
                if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.Importe, modelo, EnumCodigosAsientosModelos.AhoRecDepo))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
            }
        

            foreach (InterfazValoresImportes item in pValoresImportes)
            {
                #region "Asiento Caja y Banco"
                if ((item.IdTipoValor == (int)EnumTiposValores.Efectivo)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaCaja, pParametro.Filial, item.IdTipoValor , 0, bd, tran))
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
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, pParametro.Filial, item.IdTipoValor, item.IdBancoCuenta, bd, tran))
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

        public bool AsientoExtraccionesDepositosEspeciales(AhoCuentasMovimientos pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            //TGETiposOperaciones tipoOperacion = new TGETiposOperaciones();
            //tipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.AhorroAcreditacionCheque;
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdCuentaMovimiento;
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

            #region backup
            //CtbAsientosContables asiento = new CtbAsientosContables();
            //asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            //asiento.IdRefTipoOperacion = pParametro.IdCuentaMovimiento;
            //asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            //asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            //asiento.FechaAsiento = DateTime.Now;
            //asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(bd, tran).IdEjercicioContable;
            ////asiento.NumeroAsiento = ContabilidadF.AsientosContablesObtenerNumeroAsiento;
            //asiento.UsuarioLogueado = pParametro.UsuarioLogueado;

            //CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.Importe, modelo, EnumCodigosAsientosModelos.AhoDepExt))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            //if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.ReintegroLevantamientoCargos)
            //{
            //    CtbCuentasContables cuenta = new CtbCuentasContables();
            //    cuenta.IdRefTipoOperacionCuentaContable = pParametro.IdRefTipoOperacion;
            //    cuenta = BaseDatos.ObtenerBaseDatos().Obtener<CtbCuentasContables>("CtbCuentasContablesSeleccionarPorTipoCargo", cuenta);
            //    if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.Importe, modelo, EnumCodigosAsientosModelos.TablaTiposCargos, cuenta))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pParametro);
            //        return false;
            //    }
            //}
            //else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.Compensaciones)
            //{
            //    //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.Importe, modelo, EnumCodigosAsientosModelos.AfiCompensaciones))
            //    //{
            //    //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    //    return false;
            //    //}
            //    if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.Importe, modelo, EnumCodigosAsientosModelos.AfiCompensacionesMonotributistas))
            //    {
            //        AyudaProgramacionLN.MapearError(asiento, pParametro);
            //        return false;
            //    }
            //}
            //else
            //{
            //    if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.Importe, modelo, EnumCodigosAsientosModelos.AhoComi))
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
            #endregion
        }

        public bool AsientoExtraccion(AhoCuentasMovimientos pParametro, List<InterfazValoresImportes> pValoresImportes, AhoCuentasMovimientos pComisiones, Database bd, DbTransaction tran)
        {
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdCuentaMovimiento;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pParametro.FechaConfirmacion;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;

            CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.Importe + pComisiones.Importe, modelo, EnumCodigosAsientosModelos.AhoDepExt))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pComisiones.Importe, modelo, EnumCodigosAsientosModelos.AhoComi))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            foreach (InterfazValoresImportes item in pValoresImportes)
            {
                #region "Asiento Caja y Banco"
                if ((item.IdTipoValor == (int)EnumTiposValores.Efectivo)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaCaja, pParametro.Filial, item.IdTipoValor, 0, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }

                if ((item.IdTipoValor == (int)EnumTiposValores.Transferencia || item.IdTipoValor == (int)EnumTiposValores.Cheque)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, pParametro.Filial, item.IdTipoValor, item.IdBancoCuenta, bd, tran))
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

        public bool AsientoAcreditarCheque(AhoCuentasMovimientos pParametro, InterfazValoresImportes pValoresImportes, AhoCuentasMovimientos pComisiones, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            //TGETiposOperaciones tipoOperacion = new TGETiposOperaciones();
            //tipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.AhorroAcreditacionCheque;
            asiento.IdTipoOperacion = (int)EnumTGETiposOperaciones.AhorroAcreditacionCheque;
            asiento.IdRefTipoOperacion = pParametro.IdCuentaMovimiento;
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

            #region backup
            //asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;

            //CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            //////Asiento del Cheque -> Banco vs ValDepo
            ////if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pValoresImportes.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, null, pValoresImportes.IdTipoValor, pValoresImportes.IdBancoCuenta, bd, tran))
            ////{
            ////    AyudaProgramacionLN.MapearError(asiento, pParametro);
            ////    return false;
            ////}
            ////if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pValoresImportes.Importe, modelo, EnumCodigosAsientosModelos.ValDepo))
            ////{
            ////    AyudaProgramacionLN.MapearError(asiento, pParametro);
            ////    return false;
            ////}

            ////Asiento de Ahorros
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pValoresImportes.Importe, modelo, EnumCodigosAsientosModelos.AhoRecDepo))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pValoresImportes.Importe-pComisiones.Importe, modelo, EnumCodigosAsientosModelos.AhoDepExt))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pComisiones.Importe, modelo, EnumCodigosAsientosModelos.AhoComi))
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
            #endregion
        }

        public bool AsientoRechazarCheque(AhoCuentasMovimientos pParametro, InterfazValoresImportes pValoresImportes, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = (int)EnumTGETiposOperaciones.AhorroRechazoCheque;
            asiento.IdRefTipoOperacion = pParametro.IdCuentaMovimiento;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = DateTime.Now;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;

            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                resultado = false;
            }
            return resultado;

            #region backup
            //asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;

            //CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);
            //asiento.IdTipoOperacion = (int)EnumTGETiposOperaciones.AhorroRechazoCheque;

            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pValoresImportes.Importe, modelo, EnumCodigosAsientosModelos.AhoRecDepo))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pValoresImportes.Importe, modelo, EnumCodigosAsientosModelos.ValDepo))
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
            #endregion
        }

        public bool AsientoPlazoFijoAgregar(AhoPlazosFijos pParametro, DateTime pFecha, Database bd, DbTransaction tran)
        {
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdPlazoFijo;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pFecha; // pParametro.FechaInicioVigencia; //DateTime.Now;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;

            CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ImporteCapital, modelo, EnumCodigosAsientosModelos.AhoPFCapital))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }
            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ImporteInteres, modelo, EnumCodigosAsientosModelos.AhoPFInteres))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            TGEFiliales filial = new TGEFiliales();
            filial.IdFilial = pParametro.IdFilial;

            #region "Asiento Caja y Banco"
            if ((pParametro.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo)
                && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ImporteCapital, modelo, EnumCodigosAsientosModelos.TablaCaja, filial, pParametro.TipoValor.IdTipoValor, 0, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }
            #endregion

            if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            return true;
        }

        public bool AsientoPlazoFijoPagar(AhoPlazosFijos pParametro, DateTime pFecha, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdPlazoFijo;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pFecha; // pParametro.FechaPago;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;

            CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ImporteCapital, modelo, EnumCodigosAsientosModelos.AhoPFCapital))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ImporteInteres, modelo, EnumCodigosAsientosModelos.AhoPFInteres))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            TGEFiliales filial = new TGEFiliales();
            filial = pParametro.FilialPago;
            filial.IdFilial = pParametro.FilialPago.IdFilialPago.Value;
            foreach (InterfazValoresImportes item in pValoresImportes)
            {
                #region "Asiento Caja y Banco"
                if ((item.IdTipoValor == (int)EnumTiposValores.Efectivo)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaCaja, filial, item.IdTipoValor, 0, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }

                if ((item.IdTipoValor == (int)EnumTiposValores.Transferencia || item.IdTipoValor==(int)EnumTiposValores.Cheque)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, filial, item.IdTipoValor, item.IdBancoCuenta, bd, tran))
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

        public bool AsientoPlazoFijoCancelar(AhoPlazosFijos pParametro, DateTime pFecha, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdPlazoFijo;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pFecha;// pParametro.FechaCancelacion;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;

            CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ImporteCapital, modelo, EnumCodigosAsientosModelos.AhoPFCapital))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            TGEFiliales filial = new TGEFiliales();
            filial = pParametro.FilialPago;
            filial.IdFilial = pParametro.FilialPago.IdFilialPago.Value;
            foreach (InterfazValoresImportes item in pValoresImportes)
            {
                #region "Asiento Caja y Banco"
                if ((item.IdTipoValor == (int)EnumTiposValores.Efectivo)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaCaja, filial, item.IdTipoValor, 0, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }

                if ((item.IdTipoValor == (int)EnumTiposValores.Transferencia || item.IdTipoValor == (int)EnumTiposValores.Cheque)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, filial, item.IdTipoValor, item.IdBancoCuenta, bd, tran))
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

        public bool AsientoPlazoFijoRenovar(AhoPlazosFijos pParametro, AhoPlazosFijos pAnterior, DateTime pFecha, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdRefTipoOperacion = pParametro.IdPlazoFijo;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pFecha; // pParametro.FechaInicioVigencia;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;

            //ARREGLO PARA RENOVACIONES MULTIPLES VALORES
            if (pValoresImportes.Count > 1)
            {
                pParametro.CodigoMensaje = "ValidarPlazoFijoMultiplesValores";
                return false;
            }
            
            if (pParametro.ImporteCapital>pAnterior.ImporteTotal)
            {
                //Renovacion + Deposito
                asiento.IdTipoOperacion = (int)EnumTGETiposOperaciones.RenovacionPlazosFijosDepositos;
                InterfazValoresImportes interValor = pValoresImportes[0];
                interValor.Importe = interValor.Importe - pAnterior.ImporteTotal;
            }
            else if (pParametro.ImporteCapital < pAnterior.ImporteTotal)
            {
                //Renovacion + Extraccion
                asiento.IdTipoOperacion = (int)EnumTGETiposOperaciones.RenovacionPlazosFijosExtraccion;
                InterfazValoresImportes interValor = pValoresImportes[0];
                interValor.Importe = pAnterior.ImporteTotal - interValor.Importe;
            }
            else
            {
                pValoresImportes.Clear();
            }
            asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;
            CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pAnterior.ImporteCapital, modelo, EnumCodigosAsientosModelos.AhoPFCapitalRenovar))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }
            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pAnterior.ImporteInteres, modelo, EnumCodigosAsientosModelos.AhoPFInteresRenovar))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }
            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.ImporteCapital, modelo, EnumCodigosAsientosModelos.AhoPFCapital))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            TGEFiliales filial = new TGEFiliales();
            filial = pParametro.FilialPago;
            filial.IdFilial = pAnterior.FilialPago.IdFilialPago.Value;
            foreach (InterfazValoresImportes item in pValoresImportes)
            {
                #region "Asiento Caja y Banco"
                if ((item.IdTipoValor == (int)EnumTiposValores.Efectivo)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaCaja, filial, item.IdTipoValor, 0, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }

                if ((item.IdTipoValor == (int)EnumTiposValores.Transferencia || item.IdTipoValor == (int)EnumTiposValores.Cheque)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, filial, item.IdTipoValor, item.IdBancoCuenta, bd, tran))
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

        public bool AgregarPlazoFijoCajaAhorro(AhoPlazosFijos pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdPlazoFijo;
            asiento.Filial.IdFilial = pParametro.IdFilial;
            asiento.FechaAsiento = pParametro.FechaInicioVigencia;
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
