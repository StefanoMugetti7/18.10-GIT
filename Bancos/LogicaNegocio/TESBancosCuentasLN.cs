using Auditoria;
using Auditoria.Entidades;
using Bancos.Entidades;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Contabilidad.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Xml;

namespace Bancos.LogicaNegocio
{
    class TESBancosCuentasLN : BaseLN<TESBancosCuentas>
    {
        private byte[] ObtenerSelloTiempo(TESBancosCuentasMovimientos pParametro, Database db, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerSelloTiempo("TESBancosCuentasSeleccionar", pParametro, db, tran);
        }

        public override TESBancosCuentas ObtenerDatosCompletos(TESBancosCuentas pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<TESBancosCuentas>("TESBancosCuentasSeleccionar", pParametro);
            pParametro.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }

        public List<TESBancosCuentas> CuentasObtenerListaFiltro(TESBancosCuentas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESBancosCuentas>("TESBancosCuentasSeleccionarParaPlazosFijos", pParametro);
        }



        /// <summary>
        /// Devuelve los movimientos de una cuenta bancaria entre Fechas
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        //internal List<TESBancosCuentasMovimientos> ObtenerMovimientos(TESBancosCuentas pParametro)
        //{
        //    //pParametro.SaldoActual = this.ObtenerDatosCompletos(pParametro).SaldoActual;
        //    TESBancosCuentas filtro = new TESBancosCuentas();
        //    filtro.FechaDesde = pParametro.FechaDesde;
        //    filtro.FechaHasta = pParametro.FechaHasta;
        //    filtro.IdBancoCuenta = pParametro.IdBancoCuenta;
        //    filtro.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.Confirmado;
        //    return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESBancosCuentasMovimientos>("TESBancosCuentasMovimientosSeleccionarFiltro", filtro);
        //}
        internal DataTable ObtenerMovimientos(TESBancosCuentas pParametro)
        {
            //pParametro.SaldoActual = this.ObtenerDatosCompletos(pParametro).SaldoActual;
            TESBancosCuentas filtro = new TESBancosCuentas();
            filtro.FechaDesde = pParametro.FechaDesde;
            filtro.FechaHasta = pParametro.FechaHasta;
            filtro.IdBancoCuenta = pParametro.IdBancoCuenta;
            filtro.PageSize = pParametro.PageSize;
            filtro.PageIndex = pParametro.PageIndex;
            filtro.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.Confirmado;
            filtro.Detalle = pParametro.Detalle;
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("TESBancosCuentasMovimientosSeleccionarFiltro", filtro);
        }

        internal DataTable ObtenerMovimientosPendientes(TESBancosCuentas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("TESBancosCuentasMovimientosSeleccionarPendientes", pParametro);
        }

        internal DataTable ObtenerMovimientosRechazados(TESBancosCuentas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("TESBancosCuentasMovimientosSeleccionarRechazados", pParametro);
        }


        public override List<TESBancosCuentas> ObtenerListaFiltro(TESBancosCuentas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESBancosCuentas>("TESBancosCuentasSeleccionarFiltro", pParametro);
        }
        public List<TESBancosCuentas> ObtenerListaFiltroTransferencia(TESBancosCuentas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESBancosCuentas>("TESBancosCuentasSeleccionarFiltroTransferencia", pParametro);
        }

        public List<TESBancosCuentas> ObtenerListaDestinoDepositoTesoreria(TESBancosCuentas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESBancosCuentas>("TESBancosCuentasSeleccionarDestinoDepositoTesoreria", pParametro);
        }

        internal List<TESBancosCuentas> ObtenerDepositar(TGEFiliales pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESBancosCuentas>("TESBancosCuentasSeleccionarDepositar", pParametro);
        }

        public override bool Agregar(TESBancosCuentas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            if (!this.ValidarDatosCuenta(pParametro))
                return false;

            pParametro.FechaAlta = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = this.Agregar(pParametro, bd, tran);

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
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

        private bool ValidarDatosCuenta(TESBancosCuentas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "TESBancosCuentasValidaciones");
        }

        internal bool Agregar(TESBancosCuentas pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdBancoCuenta = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "TESBancosCuentasInsertar");
            if (pParametro.IdBancoCuenta == 0)
                return false;

            return true;
        }

        public override bool Modificar(TESBancosCuentas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (!this.ValidarDatosCuenta(pParametro))
                return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            TESBancosCuentas valorViejo = new TESBancosCuentas();
            valorViejo.IdBancoCuenta = pParametro.IdBancoCuenta;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TESBancosCuentasActualizar");

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
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

        public TESBancosCuentasMovimientos ObtenerDatosCompletos(TESBancosCuentasMovimientos pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<TESBancosCuentasMovimientos>("TESBancosCuentasMovimientosSeleccionar", pParametro);
            pParametro.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }

        public TESBancosCuentasMovimientos ObtenerDatosCompletos(TESBancosCuentasMovimientos pParametro, Database bd, DbTransaction tran)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<TESBancosCuentasMovimientos>("TESBancosCuentasMovimientosSeleccionar", pParametro, bd, tran);
            return pParametro;
        }

        /// <summary>
        /// Obtiene un movimiento por Tipo Operacion e IdReferencia
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public TESBancosCuentasMovimientos ObtenerDatosCompletosPorOperacion(TESBancosCuentasMovimientos pParametro, Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<TESBancosCuentasMovimientos>("TESBancosCuentasMovimientosSeleccionarPorOperacion", pParametro, bd, tran);
        }

        /// <summary>
        /// Agrega movimientos manuales en las cuentas de bancos
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool AgregarMovimiento(TESBancosCuentasMovimientos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            TESBancosCuentasMovimientos movDestino = new TESBancosCuentasMovimientos();

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.FechaAlta = DateTime.Now;
            pParametro.FechaMovimiento = DateTime.Now;
            pParametro.UsuarioAlta.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuario;

            if (!this.Validar(pParametro))
                return false;

            if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaTesoreriaDebito)
            {
                pParametro.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.PendienteConfirmacion;
            }
            else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasDebito)
            {
                pParametro.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.Confirmado;
                pParametro.IdUsuarioConciliacion = pParametro.UsuarioLogueado.IdUsuario;
                pParametro.FechaConciliacion = DateTime.Now;
                movDestino.BancoCuenta.IdBancoCuenta = pParametro.BancoCuentaDestino.IdBancoCuenta;
                movDestino.Importe = pParametro.Importe;
                movDestino.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasCredito;
                movDestino.Detalle = pParametro.Detalle;
                movDestino.ConceptoContable.IdConceptoContable = pParametro.ConceptoContable.IdConceptoContable;
                movDestino.EstadoColeccion = EstadoColecciones.Agregado;
                movDestino.FechaAlta = DateTime.Now;
                movDestino.FechaMovimiento = DateTime.Now;
                movDestino.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.Confirmado;
                movDestino.FechaConfirmacionBanco = pParametro.FechaConfirmacionBanco;
                movDestino.FechaConciliacion = DateTime.Now;
                movDestino.IdUsuarioConciliacion = pParametro.UsuarioLogueado.IdUsuario;
                movDestino.UsuarioLogueado = pParametro.UsuarioLogueado;
            }
            else
            {
                pParametro.FechaConciliacion = DateTime.Now;
                pParametro.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.Confirmado;
            }


            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaTesoreriaDebito)
                    {
                        OperacionConfirmar opValidar = new OperacionConfirmar();
                        opValidar.LoteCajasMovimientosValores = this.ObtenerMovimientoValoresXML(pParametro);
                        if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(opValidar, bd, tran, "TesChequesValidarNumero"))
                        {
                            AyudaProgramacionLN.MapearError(opValidar, pParametro);
                            return false;
                        }
                    }

                    if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "TesBancosCuentasMovimientosvalidaciones"))
                    {
                        pParametro.CodigoMensaje = "ValidarFechaBancoMovimiento";
                        return false;
                    }

                    resultado = this.AgregarMovimiento(pParametro, bd, tran);

                    if (resultado && pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasDebito)
                    {
                        movDestino.IdRefTipoOperacion = pParametro.IdBancoCuentaMovimiento;
                        if (!this.AgregarMovimiento(movDestino, bd, tran))
                        {
                            resultado = false;
                            AyudaProgramacionLN.MapearError(movDestino, pParametro);
                        }
                    }
                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;
                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    CtbAsientosContables asiento;
                    switch (pParametro.TipoOperacion.IdTipoOperacion)
                    {
                        case (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasDebito:
                            asiento = new CtbAsientosContables();
                            asiento.IdTipoOperacion = movDestino.TipoOperacion.IdTipoOperacion;
                            asiento.IdRefTipoOperacion = movDestino.IdBancoCuentaMovimiento;
                            asiento.Filial.IdFilial = movDestino.BancoCuenta.Filial.IdFilial;
                            asiento.FechaAsiento = movDestino.FechaConfirmacionBanco;
                            asiento.DetalleGeneral = movDestino.Detalle;
                            asiento.UsuarioLogueado = movDestino.UsuarioLogueado;
                            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
                            {
                                AyudaProgramacionLN.MapearError(asiento, pParametro);
                                resultado = false;
                            }
                            //InterfazContableLN iContaLN = new InterfazContableLN();
                            //if (resultado && !iContaLN.TransferenciaCuentasInternas(movDestino, pParametro, bd, tran))
                            //{
                            //    AyudaProgramacionLN.MapearError(movDestino, pParametro);
                            //    resultado = false;
                            //}
                            break;
                        case (int)EnumTGETiposOperaciones.TransferenciaTesoreriaDebito:
                        case (int)EnumTGETiposOperaciones.IngresosBancos:
                        case (int)EnumTGETiposOperaciones.EgresosBancos:
                            asiento = new CtbAsientosContables();
                            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
                            asiento.IdRefTipoOperacion = pParametro.IdBancoCuentaMovimiento;
                            asiento.Filial.IdFilial = pParametro.BancoCuenta.Filial.IdFilial;
                            asiento.FechaAsiento = pParametro.FechaConfirmacionBanco;
                            asiento.DetalleGeneral = pParametro.Detalle;
                            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
                            {
                                AyudaProgramacionLN.MapearError(asiento, pParametro);
                                resultado = false;
                            }
                            if (resultado && !this.GenerarMovimientoImpuestoCreditoYDebito(pParametro, bd, tran))
                                resultado = false;
                            //if (resultado && !new InterfazContableLN().IngresosEgresosPorConceptos(pParametro, bd, tran))
                            //    resultado = false;
                            break;
                        default:
                            break;
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

        public bool AnularMovimiento(TESBancosCuentasMovimientos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            TESBancosCuentasMovimientos movPendiente = new TESBancosCuentasMovimientos();

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //if (!this.Validar(pParametro))
            //    return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            //TESBancosCuentasMovimientos valorViejo = new TESBancosCuentasMovimientos();
            //valorViejo.IdBancoCuentaMovimiento = pParametro.IdBancoCuentaMovimiento;
            //valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            //valorViejo = this.ObtenerDatosCompletos(valorViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = this.ActualizarMovimiento(pParametro, bd, tran);

                    //Si anulo una Transferencia de Debito, tengo que anular la transferencia Credito pendiente
                    if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasDebito
                        && pParametro.Estado.IdEstado == (int)EstadosBancosCuentasMovimientos.Baja)
                    {
                        movPendiente.IdRefTipoOperacion = pParametro.IdBancoCuentaMovimiento;
                        movPendiente.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasCredito;
                        movPendiente = this.ObtenerDatosCompletosPorOperacion(movPendiente, bd, tran);
                        if (movPendiente.Estado.IdEstado == (int)EstadosBancosCuentasMovimientos.PendienteConciliacion)
                        {
                            movPendiente.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.Baja;
                            if (resultado && !this.ActualizarMovimiento(movPendiente, bd, tran))
                            {
                                resultado = false;
                                AyudaProgramacionLN.MapearError(movPendiente, pParametro);
                            }
                        }
                        else
                        {
                            resultado = false;
                            pParametro.CodigoMensaje = "ValidarTransferenciaCredito";
                            pParametro.CodigoMensajeArgs.Add(movPendiente.IdBancoCuentaMovimiento.ToString());
                        }
                    }
                    //Si Anulo un Deposito tengo que dejar el Cheque disponible en Bancos
                    else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.DepositosCuentasBancarias
                        && pParametro.Estado.IdEstado == (int)EstadosBancosCuentasMovimientos.Baja)
                    {
                        if (resultado && !new TESChequesLN().AnularDepositoCheque(pParametro, bd, tran))
                            resultado = false;
                    }
                    else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaTesoreriaDebito)
                    {
                        CtbAsientosContables asiento = new CtbAsientosContables();
                        asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
                        asiento.IdRefTipoOperacion = pParametro.IdBancoCuentaMovimiento;
                        asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(asiento, bd, tran, "CtbAsientosContablesAnular"))
                        {
                            AyudaProgramacionLN.MapearError(asiento, pParametro);
                            resultado = false;
                        }
                    }


                    //if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                    //    resultado = false;

                    //if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    //if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                    //    resultado = false;

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

        public bool AnularMovimientoConfirmado(TESBancosCuentasMovimientos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TESBancosCuentasMovimientosAnularConfirmado");

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

        public bool AgregarMovimiento(TESBancosCuentasMovimientos pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdBancoCuentaMovimiento = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "TESBancosCuentasMovimientosInsertar");
            if (pParametro.IdBancoCuentaMovimiento == 0)
                return false;
            //GENERAR MOVIMIENTO POR IMPUESTO AL CRÉDITO o DÉBITO
            //if (!this.GenerarMovimientoImpuestoCreditoYDebito(pParametro, bd, tran))
            //    return false;

            return true;
        }

        internal bool ActualizarMovimiento(TESBancosCuentasMovimientos pParametro, Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TESBancosCuentasMovimientosActualizar");
        }

        //internal bool MovimientoTransferenciaTesoreriaActualizar(TESBancosCuentasMovimientos pParametro, Database bd, DbTransaction tran)
        //{
        //    //bool resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TESBancosCuentasMovimientosActualizarEstado");
        //    bool resultado = true;
        //    if(resultado && !this.GenerarMovimientoImpuestoCreditoYDebito(pParametro, bd, tran))
        //        resultado=false;
        //    return resultado;
        //}

        internal bool ActualizarMovimientosPendientes(TESBancosCuentas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            TESChequesLN cheLN = new TESChequesLN();
            InterfazContableLN iContaLN = new InterfazContableLN();
            TESBancosCuentasMovimientos movOrigen = new TESBancosCuentasMovimientos();
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (pParametro.BancosCuentasMovimientosPendientes.Count(x => x.EstadoColeccion == EstadoColecciones.Modificado) == 0)
            {
                pParametro.CodigoMensaje = "ValidarItemsConciliar";
                return false;
            }

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();
            CtbAsientosContables asiento;
            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    foreach (TESBancosCuentasMovimientos movi in pParametro.BancosCuentasMovimientosPendientes)
                    {
                        switch (movi.EstadoColeccion)
                        {
                            case EstadoColecciones.Agregado:
                                break;
                            case EstadoColecciones.Borrado:
                                break;
                            case EstadoColecciones.Modificado:
                                movi.UsuarioLogueado = pParametro.UsuarioLogueado;
                                //movi.FechaConciliacion = DateTime.Now;
                                movi.IdUsuarioConciliacion = pParametro.UsuarioLogueado.IdUsuario;
                                movi.BancoCuenta.IdBancoCuenta = pParametro.IdBancoCuenta;
                                resultado = this.ActualizarMovimiento(movi, bd, tran);

                                //Salida de Dinero para Tesoreria
                                if (resultado && movi.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaTesoreriaCredito)
                                {
                                    if (movi.Estado.IdEstado == (int)EstadosBancosCuentasMovimientos.Confirmado)
                                    {
                                        asiento = new CtbAsientosContables();
                                        asiento.IdTipoOperacion = movi.TipoOperacion.IdTipoOperacion;
                                        asiento.IdRefTipoOperacion = movi.IdBancoCuentaMovimiento;
                                        //asiento.Filial.IdFilial = pParametro.BancoCuenta.Filial.IdFilial;
                                        asiento.FechaAsiento = movi.FechaConfirmacionBanco;
                                        //asiento.DetalleGeneral = pParametro.Detalle;
                                        asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                                        if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
                                        {
                                            AyudaProgramacionLN.MapearError(asiento, pParametro);
                                            resultado = false;
                                        }
                                    }
                                    else if (movi.Estado.IdEstado == (int)EstadosBancosCuentasMovimientos.Rechazado)
                                    {
                                        //Lo hago por BD porque no puedo agregar Referencias Cruzadas
                                        int movRechazo = BaseDatos.ObtenerBaseDatos().Agregar(movi, bd, tran, "TESBancosCuentasMovimientosAnularTrasferenciaTesoreria");
                                        if (movRechazo == 0)
                                        {
                                            resultado = false;
                                        }
                                        asiento = new CtbAsientosContables();
                                        asiento.IdTipoOperacion = movi.TipoOperacion.IdTipoOperacion;
                                        asiento.IdRefTipoOperacion = movi.IdRefTipoOperacion;
                                        //asiento.Filial.IdFilial = pParametro.BancoCuenta.Filial.IdFilial;
                                        asiento.FechaAsiento = movi.FechaMovimiento;
                                        //asiento.DetalleGeneral = pParametro.Detalle;
                                        asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                                        if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
                                        {
                                            AyudaProgramacionLN.MapearError(asiento, pParametro);
                                            resultado = false;
                                        }
                                    }
                                }
                                else
                                {
                                    if (resultado && !cheLN.AcreditarCheque(movi, bd, tran))
                                    {
                                        resultado = false;
                                    }
                                }

                                //CONTABILIZO MOVIMIENTO BANCARIO CHEQUE (SOLO DEPOSITOS)
                                if (resultado && movi.Estado.IdEstado == (int)EstadosBancosCuentasMovimientos.Confirmado)
                                {
                                    //Cheques Depositos
                                    if (movi.TipoOperacion.TipoMovimiento.IdTipoMovimiento == (int)EnumTGETiposMovimientos.Credito
                                        && movi.TipoOperacion.IdTipoOperacion != (int)EnumTGETiposOperaciones.TransferenciaTesoreriaCredito)
                                    {
                                        if (resultado && !new InterfazContableLN().AcreditacionCheque(movi, bd, tran))
                                            resultado = false;
                                    }

                                    //GENERAR MOVIMIENTO POR IMPUESTO AL CRÉDITO o DÉBITO
                                    if (resultado && !this.GenerarMovimientoImpuestoCreditoYDebito(movi, bd, tran))
                                    {
                                        resultado = false;
                                    }
                                }
                                else if (resultado && movi.Estado.IdEstado == (int)EstadosBancosCuentasMovimientos.Rechazado)
                                {
                                    //Cheques Rechazos
                                    if (movi.TipoOperacion.TipoMovimiento.IdTipoMovimiento == (int)EnumTGETiposMovimientos.Credito
                                        && movi.TipoOperacion.IdTipoOperacion != (int)EnumTGETiposOperaciones.TransferenciaTesoreriaCredito)
                                    {
                                        bool chequeAhorro = false;
                                        TESCheques cheque = BaseDatos.ObtenerBaseDatos().Obtener<TESCheques>("[TESChequesSeleccionarPorBancoCuentaMovimiento]", movi, bd, tran);
                                        if (cheque.IdCheque > 0)
                                        {
                                            // Valida si el Cheque viene de Ahorros Movimientos Cuentas
                                            chequeAhorro = BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(cheque, bd, tran, "TESChequesValidarAhoCuentasMovimientos");
                                        }
                                        //Contabilizao el Rechazo si no viene de Ahorros SOCIOS
                                        if (resultado && !chequeAhorro && !new InterfazContableLN().RechazoCheque(movi, bd, tran))
                                            resultado = false;
                                    }
                                }

                                break;
                            default:
                                break;
                        }

                        if (!resultado)
                        {
                            AyudaProgramacionLN.MapearError(movi, pParametro);
                            break;
                        }
                    }

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

        public XmlDocument ObtenerMovimientoValoresXML(TESBancosCuentasMovimientos pParametro)
        {
            XmlDocument LoteCajasMovimientosValores = new XmlDocument();
            //XmlNode docNode = pParametro.LotePrestamos.CreateXmlDeclaration("1.0", "UTF-8", null);
            //pParametro.LotePrestamos.AppendChild(docNode);

            XmlNode cajasMovimientosValores = LoteCajasMovimientosValores.CreateElement("CajasMovimientosValores");
            LoteCajasMovimientosValores.AppendChild(cajasMovimientosValores);

            XmlNode cajaMovimientoValor;
            XmlAttribute attribute;

            #region Cheque
            cajaMovimientoValor = LoteCajasMovimientosValores.CreateElement("CajaMovimientoValor");

            attribute = LoteCajasMovimientosValores.CreateAttribute("IdTipoValor");
            attribute.Value = ((int)EnumTiposValores.Cheque).ToString();
            cajaMovimientoValor.Attributes.Append(attribute);
            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

            attribute = LoteCajasMovimientosValores.CreateAttribute("Importe");
            attribute.Value = pParametro.Importe.ToString().Replace(',', '.');
            cajaMovimientoValor.Attributes.Append(attribute);
            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

            attribute = LoteCajasMovimientosValores.CreateAttribute("IdBancoCuenta");
            attribute.Value = pParametro.BancoCuenta.IdBancoCuenta.ToString();
            cajaMovimientoValor.Attributes.Append(attribute);
            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

            attribute = LoteCajasMovimientosValores.CreateAttribute("Fecha");
            attribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(pParametro.FechaMovimiento);
            cajaMovimientoValor.Attributes.Append(attribute);
            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

            attribute = LoteCajasMovimientosValores.CreateAttribute("FechaDiferido");
            attribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(pParametro.FechaConfirmacionBanco);
            cajaMovimientoValor.Attributes.Append(attribute);
            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

            attribute = LoteCajasMovimientosValores.CreateAttribute("NumeroCheque");
            attribute.Value = pParametro.NumeroTipoOperacion;
            cajaMovimientoValor.Attributes.Append(attribute);
            cajasMovimientosValores.AppendChild(cajaMovimientoValor);
            #endregion

            return LoteCajasMovimientosValores;
        }

        private bool Validar(TESBancosCuentasMovimientos pParametro)
        {


            if (pParametro.Importe <= 0)
            {
                pParametro.CodigoMensaje = "ValidarImporte";
                return false;
            }
            if (pParametro.BancoCuenta.IdBancoCuenta == pParametro.BancoCuentaDestino.IdBancoCuenta)
            {
                pParametro.CodigoMensaje = "ValidarCuentaOrigenDestino";
                return false;
            }
            return true;
        }

        private bool GenerarMovimientoImpuestoCreditoYDebito(TESBancosCuentasMovimientos pParametro, Database bd, DbTransaction tran)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(pParametro, bd, tran, "TESBancosCuentasMovimientosInsertarImpuestoDebitoCredito"))
            {
                resultado = false;
            }
            return resultado;

            #region Codigo Anterior
            //TESBancosCuentasMovimientos movImpuestoCD = new TESBancosCuentasMovimientos();

            //bool resultado = true;

            //if ((pParametro.TipoOperacion.IdTipoOperacion != (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasCredito
            //    || pParametro.TipoOperacion.IdTipoOperacion != (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasDebito)
            //    && pParametro.Estado.IdEstado == (int)EstadosBancosCuentasMovimientos.Confirmado
            //    && pParametro.BancoCuenta.BancoCuentaTipo.IdBancoCuentaTipo != (int)EnumTesBancosCuentasTipos.TarjetasCredito)
            //{

            //    TGEParametrosValores parametroValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.PorcentajeImpuestosCreditoYDebito);

            //    Si no esta definido el parametro o no tiene valor devuelvo TRUE
            //    decimal impuesto = 0;
            //    if (!decimal.TryParse(parametroValor.ParametroValor, out impuesto))
            //        return true;

            //    if (impuesto == 0)
            //        return true;

            //    movImpuestoCD.IdRefTipoOperacion = pParametro.IdBancoCuentaMovimiento;
            //    movImpuestoCD.EstadoColeccion = EstadoColecciones.Agregado;
            //    movImpuestoCD.FechaAlta = DateTime.Now;
            //    movImpuestoCD.FechaMovimiento = DateTime.Now;
            //    movImpuestoCD.FechaConciliacion = DateTime.Now;
            //    movImpuestoCD.BancoCuenta.IdBancoCuenta = pParametro.BancoCuenta.IdBancoCuenta;
            //    movImpuestoCD.Detalle = pParametro.Detalle;
            //    movImpuestoCD.ConceptoContable.IdConceptoContable = pParametro.ConceptoContable.IdConceptoContable;
            //    movImpuestoCD.EstadoColeccion = EstadoColecciones.Agregado;
            //    movImpuestoCD.FechaAlta = DateTime.Now;
            //    movImpuestoCD.FechaMovimiento = DateTime.Now;
            //    movImpuestoCD.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.Confirmado;
            //    movImpuestoCD.FechaConfirmacionBanco = pParametro.FechaConfirmacionBanco;
            //    movImpuestoCD.FechaConciliacion = DateTime.Now;
            //    movImpuestoCD.IdUsuarioConciliacion = pParametro.UsuarioLogueado.IdUsuario;
            //    movImpuestoCD.UsuarioLogueado = pParametro.UsuarioLogueado;
            //    movImpuestoCD.Importe = Math.Abs(pParametro.Importe) * impuesto;

            //    switch (pParametro.TipoOperacion.TipoMovimiento.IdTipoMovimiento)
            //    {
            //        case (int)EnumTGETiposMovimientos.Credito:
            //            movImpuestoCD.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.ImpuestoCredito;
            //            movImpuestoCD.TipoOperacion.TipoMovimiento.IdTipoMovimiento = (int)EnumTGETiposMovimientos.Credito;
            //            break;
            //        case (int)EnumTGETiposMovimientos.Debito:
            //            movImpuestoCD.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.ImpuestoDebito;
            //            movImpuestoCD.TipoOperacion.TipoMovimiento.IdTipoMovimiento = (int)EnumTGETiposMovimientos.Debito;
            //            break;
            //    }

            //    movImpuestoCD.IdBancoCuentaMovimiento = BaseDatos.ObtenerBaseDatos().Agregar(movImpuestoCD, bd, tran, "TESBancosCuentasMovimientosInsertar");
            //    if (movImpuestoCD.IdBancoCuentaMovimiento == 0)
            //    {
            //        AyudaProgramacionLN.MapearError(movImpuestoCD, pParametro);
            //        resultado = false;
            //    }

            //    if (resultado && !new InterfazContableLN().ImpuestoCreditoDebito(movImpuestoCD, bd, tran))
            //    {
            //        AyudaProgramacionLN.MapearError(movImpuestoCD, pParametro);
            //        resultado = false;
            //    }
            //}
            #endregion
        }

        public List<TESBancosCuentas> ObtenerLista(TESBancosCuentas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESBancosCuentas>("VTAFacturasObtenerListaBancosCuentas", pParametro);
        }
        public List<TESBancosCuentas> ObtenerListaGrupo(TESBancosCuentas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESBancosCuentas>("VTAFacturasObtenerListaBancosCuentasGrupo", pParametro);
        }

        //public bool AgregarMovimientosMultiples(List<TESBancosCuentasMovimientos> pParametro)
        //{
        //    foreach (TESBancosCuentasMovimientos item in pParametro)
        //    {
        //        if (!this.AgregarMovimiento(item))
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
        public bool AgregarMovimientosMultiples(List<TESBancosCuentasMovimientos> pParametro, ref string mensajeRetorno)
        {
            bool resultado = true;
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    foreach (TESBancosCuentasMovimientos item in pParametro)
                    {
                        AyudaProgramacionLN.LimpiarMensajesError(item);
                        TESBancosCuentasMovimientos movDestino = new TESBancosCuentasMovimientos();

                        item.EstadoColeccion = EstadoColecciones.Agregado;
                        item.FechaAlta = DateTime.Now;
                        item.FechaMovimiento = DateTime.Now;
                        item.UsuarioAlta.IdUsuarioAlta = item.UsuarioLogueado.IdUsuario;

                        if (!this.Validar(item))
                        {
                            mensajeRetorno = item.CodigoMensaje;
                            return false;
                        }

                        if (item.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaTesoreriaDebito)
                        {
                            item.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.PendienteConfirmacion;
                        }
                        else if (item.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasDebito)
                        {
                            item.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.Confirmado;
                            item.IdUsuarioConciliacion = item.UsuarioLogueado.IdUsuario;
                            item.FechaConciliacion = DateTime.Now;
                            movDestino.BancoCuenta.IdBancoCuenta = item.BancoCuentaDestino.IdBancoCuenta;
                            movDestino.Importe = item.Importe;
                            movDestino.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasCredito;
                            movDestino.Detalle = item.Detalle;
                            movDestino.ConceptoContable.IdConceptoContable = item.ConceptoContable.IdConceptoContable;
                            movDestino.EstadoColeccion = EstadoColecciones.Agregado;
                            movDestino.FechaAlta = DateTime.Now;
                            movDestino.FechaMovimiento = DateTime.Now;
                            movDestino.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.Confirmado;
                            movDestino.FechaConfirmacionBanco = item.FechaConfirmacionBanco;
                            movDestino.FechaConciliacion = DateTime.Now;
                            movDestino.IdUsuarioConciliacion = item.UsuarioLogueado.IdUsuario;
                            movDestino.UsuarioLogueado = item.UsuarioLogueado;
                        }
                        else
                        {
                            item.FechaConciliacion = DateTime.Now;
                            item.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.Confirmado;
                        }

                        if (item.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaTesoreriaDebito)
                        {
                            OperacionConfirmar opValidar = new OperacionConfirmar();
                            opValidar.LoteCajasMovimientosValores = this.ObtenerMovimientoValoresXML(item);
                            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(opValidar, bd, tran, "TesChequesValidarNumero"))
                            {
                                AyudaProgramacionLN.MapearError(opValidar, item);
                                mensajeRetorno = item.CodigoMensaje;
                                return false;
                            }
                        }

                        if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(item, bd, tran, "TesBancosCuentasMovimientosvalidaciones"))
                        {
                            mensajeRetorno = "ValidarFechaBancoMovimiento";
                            return false;
                        }

                        resultado = this.AgregarMovimiento(item, bd, tran);

                        if (resultado && item.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasDebito)
                        {
                            movDestino.IdRefTipoOperacion = item.IdBancoCuentaMovimiento;
                            if (!this.AgregarMovimiento(movDestino, bd, tran))
                            {
                                resultado = false;
                                AyudaProgramacionLN.MapearError(movDestino, item);
                                mensajeRetorno = item.CodigoMensaje;
                                break;
                            }
                        }
                        //if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(item, bd, tran))
                        //{
                        //    resultado = false;
                        //    break;
                        //}
                        //if (resultado && !TGEGeneralesF.ComentariosActualizar(item, bd, tran))
                        //{
                        //    resultado = false;
                        //    break;
                        //}

                        //if (resultado && !TGEGeneralesF.ArchivosActualizar(item, bd, tran))
                        //{
                        //    resultado = false;
                        //    break;
                        //}

                        CtbAsientosContables asiento;
                        switch (item.TipoOperacion.IdTipoOperacion)
                        {
                            case (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasDebito:
                                asiento = new CtbAsientosContables();
                                asiento.IdTipoOperacion = movDestino.TipoOperacion.IdTipoOperacion;
                                asiento.IdRefTipoOperacion = movDestino.IdBancoCuentaMovimiento;
                                asiento.Filial.IdFilial = movDestino.BancoCuenta.Filial.IdFilial;
                                asiento.FechaAsiento = movDestino.FechaConfirmacionBanco;
                                asiento.DetalleGeneral = movDestino.Detalle;
                                asiento.UsuarioLogueado = movDestino.UsuarioLogueado;
                                if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
                                {
                                    AyudaProgramacionLN.MapearError(asiento, item);
                                    mensajeRetorno = item.CodigoMensaje;
                                    resultado = false;
                                }
                                //InterfazContableLN iContaLN = new InterfazContableLN();
                                //if (resultado && !iContaLN.TransferenciaCuentasInternas(movDestino, pParametro, bd, tran))
                                //{
                                //    AyudaProgramacionLN.MapearError(movDestino, pParametro);
                                //    resultado = false;
                                //}
                                break;
                            case (int)EnumTGETiposOperaciones.TransferenciaTesoreriaDebito:
                            case (int)EnumTGETiposOperaciones.IngresosBancos:
                            case (int)EnumTGETiposOperaciones.EgresosBancos:
                                asiento = new CtbAsientosContables();
                                asiento.IdTipoOperacion = item.TipoOperacion.IdTipoOperacion;
                                asiento.IdRefTipoOperacion = item.IdBancoCuentaMovimiento;
                                asiento.Filial.IdFilial = item.BancoCuenta.Filial.IdFilial;
                                asiento.FechaAsiento = item.FechaConfirmacionBanco;
                                asiento.DetalleGeneral = item.Detalle;
                                asiento.UsuarioLogueado = item.UsuarioLogueado;
                                if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
                                {
                                    AyudaProgramacionLN.MapearError(asiento, item);
                                    mensajeRetorno = item.CodigoMensaje;
                                    resultado = false;
                                }
                                if (resultado && !this.GenerarMovimientoImpuestoCreditoYDebito(item, bd, tran))
                                {
                                    mensajeRetorno = item.CodigoMensaje;
                                    resultado = false;
                                }
                                //if (resultado && !new InterfazContableLN().IngresosEgresosPorConceptos(pParametro, bd, tran))
                                //    resultado = false;
                                break;
                            default:
                                break;
                        }
                    }
                    if (resultado)
                    {
                        tran.Commit();
                        mensajeRetorno = "ResultadoTransaccion";
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
                    mensajeRetorno = "ResultadoTransaccionIncorrecto";
                    //pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        /// <summary>
        /// Devuelve los movimientos pendientes de confirmacion para Bancos
        /// </summary>
        /// <param name="pParametro">IdTesoreria</param>
        /// <returns></returns>
        //public List<TESBancosCuentasMovimientos> ObtenerPendientesConfirmacion(TESBancosCuentas pParametro)
        //{
        //    return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESBancosCuentasMovimientos>("TESBancosMovimientosAConfirmarPorFilial", pParametro);
        //}

        /// <summary>
        /// Metodo para confirmar las operaciones de socios por Bancos
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="pMovimientos"></param>
        /// <returns></returns>
        //public bool ConfirmarOperaciones(Objeto pParametro, List<TESBancosCuentasMovimientos> pMovimientos)
        //{
        //    AyudaProgramacionLN.LimpiarMensajesError(pParametro);

        //    bool resultado = true;

        //    if (pMovimientos.Count == 0)
        //    {
        //        pParametro.CodigoMensaje = "ValidarItemsConfirmar";
        //        return false;
        //    }

        //    TGEEstados estado = TGEGeneralesF.TGEEstadosObtener(new TGEEstados() { IdEstado = (int)EstadosBancosCuentasMovimientos.Confirmado });
        //    Objeto refTipoOperacion;
        //    List<Objeto> refLista = new List<Objeto>();
        //    foreach (TESBancosCuentasMovimientos mov in pMovimientos)
        //    {
        //        refTipoOperacion = this.ObtenerMovimientoPendienteConfirmacion(mov);
        //        if (!Enumerable.SequenceEqual(mov.SelloTiempo, refTipoOperacion.SelloTiempo))
        //        {
        //            pParametro.CodigoMensaje = "Concurrencia";
        //            pParametro.CodigoMensajeArgs.Add(mov.IdRefTipoOperacion.ToString());
        //            return false;
        //        }
        //        refTipoOperacion.EstadoColeccion = EstadoColecciones.Modificado;
        //        refTipoOperacion.Estado = estado;
        //        refTipoOperacion.UsuarioLogueado = pParametro.UsuarioLogueado;
        //        refLista.Add(refTipoOperacion);

        //        mov.Estado.IdEstado = (int)EstadosBancosCuentasMovimientos.PendienteConciliacion;
        //    }

        //    refTipoOperacion=new Objeto();
        //    DbTransaction tran;
        //    DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

        //    using (DbConnection con = bd.CreateConnection())
        //    {
        //        con.Open();
        //        tran = con.BeginTransaction();

        //        try
        //        {
        //            int indice = 0;
        //            foreach (TESBancosCuentasMovimientos mov in pMovimientos)
        //            {
        //                if (!this.AgregarMovimiento(mov, bd, tran))
        //                {
        //                    resultado = false;
        //                    AyudaProgramacionLN.MapearError(mov, pParametro);
        //                    break;
        //                }

        //                refTipoOperacion = refLista[indice];

        //                switch (mov.TipoOperacion.IdTipoOperacion)
        //                {
        //                    #region Modulo de Ahorros
        //                    //case (int)EnumTGETiposOperaciones.DepositoEfectivo:
        //                    //case (int)EnumTGETiposOperaciones.ExtraccionEfectivo:
        //                    case (int)EnumTGETiposOperaciones.AhorroDepositos:
        //                    case (int)EnumTGETiposOperaciones.AhorroExtracciones:
        //                        if (resultado && !AhorroF.MovimientosConfirmar(refTipoOperacion, bd, tran))
        //                            resultado = false;
        //                        break;
        //                    case (int)EnumTGETiposOperaciones.ExtraccionCheque:
        //                        break;
        //                    case (int)EnumTGETiposOperaciones.AcreditacionCheque:
        //                        break;
        //                    case (int)EnumTGETiposOperaciones.DepositoPlazoFijo:
        //                    case (int)EnumTGETiposOperaciones.ExtraccionPlazoFijo:
        //                        if (resultado && !AhorroF.PlazosFijosConfirmar(refTipoOperacion, bd, tran))
        //                            resultado = false;
        //                        break;
        //                    case (int)EnumTGETiposOperaciones.RenovacionPlazosFijos:
        //                        if (resultado && !AhorroF.PlazosFijosConfirmarRenovar(refTipoOperacion, bd, tran))
        //                            resultado = false;
        //                        break;
        //                    case (int)EnumTGETiposOperaciones.CancelacionAnticipadaPlazoFijo:
        //                        if (resultado && !AhorroF.PlazosFijosConfirmarCancelar(refTipoOperacion, bd, tran))
        //                            resultado = false;
        //                        break;
        //                    #endregion

        //                    #region Modulo Cuenta Corriente
        //                    //case (int)EnumTGETiposOperaciones.CuentaCorrienteExtraccion:
        //                    //case (int)EnumTGETiposOperaciones.PagoHaberes:
        //                    //    if (resultado && !CargosF.CuentasCorrientesAgregar((CarCuentasCorrientes)pRefTipoOperacion, bd, tran))
        //                    //        resultado = false;
        //                    //    if (resultado && !InterfacesAS400F.CajasActualizarHaberesExtraccion(pRefTipoOperacion, pParametro.Tesoreria.Filial.IdFilial, pParametro.NumeroCaja, pParametro.UsuarioLogueado.UsuarioAS400, bd, tran))
        //                    //        resultado = false;
        //                    //    break;
        //                    #endregion

        //                    #region Modulo Ordenes de Cobro
        //                    //case (int)EnumTGETiposOperaciones.OrdenesCobrosAfiliados:
        //                    //case (int)EnumTGETiposOperaciones.OrdenesCobrosVarios:
        //                    //    if (resultado && !CobrosF.OrdenesCobrosConfirmar((CobOrdenesCobros)pRefTipoOperacion, bd, tran))
        //                    //        resultado = false;
        //                    //    break;
        //                    #endregion

        //                    #region Modulo de Prestamos
        //                    case (int)EnumTGETiposOperaciones.PrestamosLargoPlazo:
        //                    case (int)EnumTGETiposOperaciones.PrestamosCortoPlazo:
        //                        if (resultado && !PrePrestamosF.PrestamosConfirmar(refTipoOperacion, bd, tran))
        //                            resultado = false;
        //                        break;
        //                    //case (int)EnumTGETiposOperaciones.PrestamosLargoPlazoCancelacion:
        //                    //case (int)EnumTGETiposOperaciones.PrestamosCortoPlazoCancelacion:
        //                    //    if (!PrePrestamosF.PrestamosCancelarConfirmar(refTipoOperacion, bd, tran))
        //                    //        resultado = false;
        //                    //    break;

        //                    #endregion

        //                    default:
        //                        break;
        //                }
        //                // Si da error al guardar el Objeto referencia Mapeo el Error
        //                if (!resultado)
        //                    AyudaProgramacionLN.MapearError(refTipoOperacion, pParametro);

        //                if (!resultado)
        //                    break;
        //            }

        //            if (resultado)
        //            {
        //                tran.Commit();
        //                pParametro.CodigoMensaje = "ResultadoTransaccion";
        //            }
        //            else
        //            {
        //                tran.Rollback();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ExceptionHandler.HandleException(ex, "LogicaNegocio");
        //            tran.Rollback();
        //            pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
        //            pParametro.CodigoMensajeArgs.Add(ex.Message);
        //            return false;
        //        }
        //    }
        //    return resultado;
        //}

        /// <summary>
        /// Devuevle el objeto de referencia para Confirmar la Operacion por Bancos
        /// </summary>
        /// <param name="pParametro">IdRefTipoOperacion, IdTipoOperacion</param>
        /// <returns></returns>
        //private Objeto ObtenerMovimientoPendienteConfirmacion(TESBancosCuentasMovimientos pParametro)
        //{
        //    Objeto resultado = new Objeto();
        //    switch (pParametro.TipoOperacion.IdTipoOperacion)
        //    {
        //        #region Modulo de Ahorros
        //        //case (int)EnumTGETiposOperaciones.DepositoEfectivo:
        //        //case (int)EnumTGETiposOperaciones.ExtraccionEfectivo:
        //        //case (int)EnumTGETiposOperaciones.AhorroDepositos:
        //        case (int)EnumTGETiposOperaciones.AhorroExtracciones:
        //            AhoCuentasMovimientos movimiento = new AhoCuentasMovimientos();
        //            movimiento.IdCuentaMovimiento = pParametro.IdRefTipoOperacion;
        //            movimiento.UsuarioLogueado = pParametro.UsuarioLogueado;
        //            resultado = AhorroF.MovimientosObtenerDatosCompletos(movimiento);
        //            break;
        //        case (int)EnumTGETiposOperaciones.ExtraccionCheque:
        //            break;
        //        case (int)EnumTGETiposOperaciones.AcreditacionCheque:
        //            break;

        //        //case (int)EnumTGETiposOperaciones.DepositoPlazoFijo:
        //        case (int)EnumTGETiposOperaciones.ExtraccionPlazoFijo:
        //        case (int)EnumTGETiposOperaciones.CancelacionAnticipadaPlazoFijo:
        //        case (int)EnumTGETiposOperaciones.RenovacionPlazosFijos:
        //            AhoPlazosFijos plazoFijo = new AhoPlazosFijos();
        //            plazoFijo.IdPlazoFijo = pParametro.IdRefTipoOperacion;
        //            plazoFijo.UsuarioLogueado = pParametro.UsuarioLogueado;
        //            resultado = AhorroF.PlazosFijosObtenerDatosCompletos(plazoFijo);
        //            break;
        //        #endregion

        //        #region Modulo de Cobros
        //        //case (int)EnumTGETiposOperaciones.OrdenesCobrosAfiliados:
        //        //case (int)EnumTGETiposOperaciones.OrdenesCobrosVarios:
        //        //    CobOrdenesCobros ordenCobro = new CobOrdenesCobros();
        //        //    ordenCobro.IdOrdenCobro = pParametro.IdRefTipoOperacion;
        //        //    ordenCobro.UsuarioLogueado = pParametro.UsuarioLogueado;
        //        //    resultado = CobrosF.OrdenesCobrosObtenerDatosCompletos(ordenCobro);
        //        //    break;
        //        #endregion

        //        #region Modulo de Prestamos
        //        case (int)EnumTGETiposOperaciones.PrestamosLargoPlazo:
        //        case (int)EnumTGETiposOperaciones.PrestamosCortoPlazo:
        //        //case (int)EnumTGETiposOperaciones.PrestamosLargoPlazoCancelacion:
        //        //case (int)EnumTGETiposOperaciones.PrestamosCortoPlazoCancelacion:
        //            PrePrestamos prestamo = new PrePrestamos();
        //            prestamo.IdPrestamo = pParametro.IdRefTipoOperacion;
        //            prestamo.UsuarioLogueado = pParametro.UsuarioLogueado;
        //            resultado = PrePrestamosF.PrestamosObtenerDatosCompletos(prestamo);
        //            break;
        //        #endregion

        //        default:
        //            break;
        //    }

        //    return resultado;
        //}
    }
}
