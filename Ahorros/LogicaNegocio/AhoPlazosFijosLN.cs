using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ahorros.Entidades;
using Servicio.AccesoDatos;
using Afiliados.Entidades;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Generales.Entidades;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Comunes;
using Comunes.LogicaNegocio;
using Auditoria;
using Auditoria.Entidades;
using Generales.FachadaNegocio;
using Contabilidad.Entidades;
using System.Data;

namespace Ahorros.LogicaNegocio
{
    class AhoPlazosFijosLN : BaseLN<AhoPlazosFijos>
    {
        public override AhoPlazosFijos ObtenerDatosCompletos(AhoPlazosFijos pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<AhoPlazosFijos>("AhoPlazosFijosSeleccionarDescripcion", pParametro);
            pParametro.Cuenta = new AhoCuentasLN().ObtenerDatosCompletos(pParametro.Cuenta);
            pParametro.Cotitulares = BaseDatos.ObtenerBaseDatos().ObtenerLista<AhoCotitulares>("AhoCotitularesSeleccionarPorPlazoFijo", pParametro);
            pParametro.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }

        public override List<AhoPlazosFijos> ObtenerListaFiltro(AhoPlazosFijos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AhoPlazosFijos>("AhoPlazosFijosSeleccionarFiltro", pParametro);
        }
        public DataTable ObtenerListaFiltroDT(AhoPlazosFijos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("AhoPlazosFijosSeleccionarFiltro", pParametro);
        }
        public override bool Agregar(AhoPlazosFijos pParametro)
        {
            if (pParametro.IdPlazoFijo > 0)
                return true;

            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            bool contabilizar = false;

            //pParametro.FechaVencimiento = pParametro.FechaAlta.AddDays(pParametro.PlazoDias);
            pParametro.FechaVencimiento = pParametro.FechaInicioVigencia.AddDays(pParametro.PlazoDias);

            if (!this.ValidarAgregar(pParametro, new AhoPlazosFijos()))
                return false;

            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.Estado.IdEstado = (int)EstadosPlazosFijos.PendienteConfirmacion;
            pParametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.DepositoPlazoFijo;
            pParametro.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
            //pParametro.Moneda.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;
            pParametro.FechaAlta = DateTime.Now;
            pParametro.Cuenta.SaldoActualOriginal = pParametro.Cuenta.SaldoActual;
            pParametro.Cuenta.SaldoActual = pParametro.Cuenta.SaldoActualOriginal - pParametro.ImporteCapital;
            //pParametro.FechaPago = pParametro.FechaAlta.AddDays(pParametro.PlazoDias);
            pParametro.ImporteInteres = this.CalcularInteres(pParametro);

            if (!Validar(pParametro))
                return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!pParametro.RegistrarCajaAhorros)
                    {
                        //Guardo el Plazo Fijo
                        pParametro.IdPlazoFijo = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "AhoPlazosFijosInsertar");
                        if (pParametro.IdPlazoFijo == 0)
                            resultado = false;
                    }
                    else
                    {
                        pParametro.Estado.IdEstado = (int)EstadosPlazosFijos.Confirmado;
                        pParametro.FechaConfirmacion = pParametro.FechaInicioVigencia;
                        pParametro.UsuarioConfirmacion.IdUsuarioConfirmacion = pParametro.UsuarioLogueado.IdUsuario;
                        //resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pParametro, bd, tran, "AhoPlazosFijosInsertarCuentasAhorros");
                        pParametro.IdPlazoFijo = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "AhoPlazosFijosInsertarCuentasAhorros");
                        if (pParametro.IdPlazoFijo == 0)
                            resultado = false;

                        contabilizar = true;

                    }
                    if (resultado && !this.ActualizarCotitulares(pParametro, new AhoPlazosFijos(), bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;
                    //Control Comentarios y Archivos
                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;
                    //Fin control Comentarios y Archivos

                    if (resultado && contabilizar && !new InterfazContableLN().AgregarPlazoFijoCajaAhorro(pParametro, bd, tran))
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
                    pParametro.CodigoMensaje = ex.Message;
                    resultado = false;
                }
                finally {
                    if (!resultado)
                        pParametro.IdPlazoFijo = 0;
                }
            }
            return resultado;
        }

        /// <summary>
        /// Confirma un Plazo Fijo de Ahorro
        /// Metodo para los movimientos de Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool Confirmar(Objeto pParametro, DateTime pFecha, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            bool resultado = true;

            AhoPlazosFijos plazoFijo = (AhoPlazosFijos)pParametro;

            if (plazoFijo.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.DepositoPlazoFijo)
            {
                plazoFijo.FechaConfirmacion = pFecha;
                plazoFijo.FechaInicioVigencia = pFecha;

                //plazoFijo.FechaVencimiento = plazoFijo.FechaConfirmacion.AddDays(plazoFijo.PlazoDias);
                plazoFijo.FechaVencimiento = plazoFijo.FechaInicioVigencia.AddDays(plazoFijo.PlazoDias);
                plazoFijo.Estado.IdEstado = (int)EstadosPlazosFijos.Confirmado;
                plazoFijo.UsuarioConfirmacion.IdUsuarioConfirmacion = plazoFijo.UsuarioLogueado.IdUsuario;

                if (resultado && !new InterfazContableLN().AsientoPlazoFijoAgregar(plazoFijo, pFecha, bd, tran))
                    resultado = false;
            }
            else
            {
                plazoFijo.FechaPago = pFecha;
                plazoFijo.Estado.IdEstado = (int)EstadosPlazosFijos.Pagado;
                if (resultado && !new InterfazContableLN().AsientoPlazoFijoPagar(plazoFijo, pFecha, pValoresImportes, bd, tran))
                    resultado = false;
            }

            //Guardo el Plazo Fijo
            if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(plazoFijo, bd, tran, "AhoPlazosFijosActualizar"))
                resultado = false;

            
            //INTEGRACION CON CUENTAS DE AHORROS

            //if (resultado && plazoFijo.Cuenta.IdCuenta > 0)
            //{

            //    if (!this.Validar(plazoFijo))
            //        return false;

            //      plazoFijo.Cuenta.SaldoActualOriginal = plazoFijo.Cuenta.SaldoActual;
            //    plazoFijo.Cuenta.SaldoActual = plazoFijo.Cuenta.SaldoActualOriginal - plazoFijo.ImporteCapital;

            //    AhoCuentasMovimientos movimiento = new AhoCuentasMovimientos();
            //    movimiento.Cuenta = plazoFijo.Cuenta;
            //    movimiento.FechaMovimiento = plazoFijo.FechaAlta;
            //    movimiento.Estado.IdEstado = (int)EstadosAhorrosCuentasMovimientos.Confirmado;
            //    movimiento.FechaConfirmacion = plazoFijo.FechaAlta;
            //    movimiento.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.ExtraccionPlazoFijo;
            //    movimiento.Importe = plazoFijo.ImporteCapital;
            //    movimiento.SaldoActual = plazoFijo.Cuenta.SaldoActual;
            //    movimiento.Filial.IdFilial = plazoFijo.IdFilial;
            //    movimiento.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
            //    movimiento.UsuarioAlta = plazoFijo.UsuarioAlta;
            //    movimiento.UsuarioLogueado = plazoFijo.UsuarioLogueado;


            //    //Guardo el movimiento de Ahorro
            //    if (resultado && !new AhoCuentasMovimientosLN().Agregar(movimiento, bd, tran))
            //    {
            //        resultado = false;
            //        AyudaProgramacionLN.MapearError(movimiento, plazoFijo);
            //    }

            //    // Guardo el saldo de la cuenta                    
            //    if (resultado && !new AhoCuentasLN().Modificar(plazoFijo.Cuenta, bd, tran))
            //    {
            //        resultado = false;
            //        AyudaProgramacionLN.MapearError(plazoFijo.Cuenta, plazoFijo);
            //    }
            //}


            return resultado;
        }

        /// <summary>
        /// Confirma la Cancelación de un Plazo Fijo de Ahorro
        /// Metodo para los movimientos de Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool ConfirmarCancelar(Objeto pParametro, DateTime pFecha, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            AhoPlazosFijos plazoFijo = (AhoPlazosFijos)pParametro;
            plazoFijo.FechaCancelacion = pFecha;
            plazoFijo.Estado.IdEstado = (int)EstadosPlazosFijos.Cancelado;
            plazoFijo.UsuarioCancelacion.IdUsuarioCancelacion = plazoFijo.UsuarioLogueado.IdUsuario;

            //Actualizo el Plazo Fijo
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(plazoFijo, bd, tran, "AhoPlazosFijosActualizar"))
                resultado = false;

            if (resultado && !new InterfazContableLN().AsientoPlazoFijoCancelar(plazoFijo, pFecha, pValoresImportes, bd, tran))
                resultado = false;

            //INTEGRACION CON CUENTAS DE AHORROS

            //if (resultado && plazoFijo.Cuenta.IdCuenta > 0)
            //{
            //    plazoFijo.Cuenta.SaldoActual = plazoFijo.Cuenta.SaldoActualOriginal + plazoFijo.ImporteCapital;
            //    AhoCuentasMovimientos movimiento = new AhoCuentasMovimientos();
            //    movimiento.Cuenta = plazoFijo.Cuenta;
            //    movimiento.FechaMovimiento = plazoFijo.FechaAlta;
            //    movimiento.Estado.IdEstado = (int)EstadosAhorrosCuentasMovimientos.Confirmado;
            //    movimiento.FechaConfirmacion = DateTime.Now;
            //    movimiento.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.DepositoPlazoFijo;
            //    movimiento.Importe = plazoFijo.ImporteCapital;
            //    movimiento.SaldoActual = plazoFijo.Cuenta.SaldoActual;
            //    movimiento.Filial.IdFilial = plazoFijo.IdFilial;
            //    movimiento.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
            //    movimiento.UsuarioAlta.IdUsuarioAlta = plazoFijo.UsuarioLogueado.IdUsuario;
            //    movimiento.UsuarioLogueado = plazoFijo.UsuarioLogueado;


            //    //Guardo el movimiento de Ahorro
            //    if (resultado && !new AhoCuentasMovimientosLN().Agregar(movimiento, bd, tran))
            //    {
            //        resultado = false;
            //        AyudaProgramacionLN.MapearError(movimiento, plazoFijo);
            //    }

            //    // Guardo el saldo de la cuenta                    
            //    if (resultado && !new AhoCuentasLN().Modificar(plazoFijo.Cuenta, bd, tran))
            //    {
            //        resultado = false;
            //        AyudaProgramacionLN.MapearError(plazoFijo.Cuenta, plazoFijo);
            //    }
            //}

            return resultado;
        }

        /// <summary>
        /// Confirma la renovacion de un Plazo Fijo de Ahorro
        /// Metodo para los movimientos de Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool ConfirmarRenovar(Objeto pParametro, DateTime pFecha, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            bool resultado = true;

            AhoPlazosFijos plazoFijo = (AhoPlazosFijos)pParametro;
            plazoFijo.FechaConfirmacion = pFecha;
            plazoFijo.FechaVencimiento = plazoFijo.FechaInicioVigencia.AddDays(plazoFijo.PlazoDias);
            plazoFijo.Estado.IdEstado = (int)EstadosPlazosFijos.Confirmado;
            plazoFijo.UsuarioConfirmacion.IdUsuarioConfirmacion = plazoFijo.UsuarioLogueado.IdUsuario;

            //Guardo el Plazo Fijo
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(plazoFijo, bd, tran, "AhoPlazosFijosActualizar"))
                resultado = false;

            AhoPlazosFijos pAnterior = new AhoPlazosFijos();
            pAnterior.IdPlazoFijo = plazoFijo.IdPlazoFijoAnterior;
            pAnterior.UsuarioLogueado = plazoFijo.UsuarioLogueado;
            pAnterior = BaseDatos.ObtenerBaseDatos().Obtener<AhoPlazosFijos>("AhoPlazosFijosSeleccionarDescripcion", pAnterior, bd, tran);

            AhoPlazosFijos valorViejo = new AhoPlazosFijos();
            AyudaProgramacionLN.MatchObjectProperties(pAnterior, valorViejo);

            pAnterior.FechaPago = pFecha;
            pAnterior.Estado.IdEstado = (int)EstadosPlazosFijos.Pagado;
            pAnterior.Estado.Descripcion = EstadosPlazosFijos.Pagado.ToString();
            pAnterior.FilialPago.IdFilialPago = plazoFijo.IdFilial;
            pAnterior.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.ExtraccionPlazoFijo);
            pAnterior.UsuarioLogueado = plazoFijo.UsuarioLogueado;
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pAnterior, bd, tran, "AhoPlazosFijosActualizar"))
            {
                resultado = false;
                AyudaProgramacionLN.MapearError(pAnterior, pParametro);
            }
            if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pAnterior, bd, tran))
            {
                resultado = false;
                AyudaProgramacionLN.MapearError(pAnterior, pParametro);
            }

            if (resultado && !new InterfazContableLN().AsientoPlazoFijoRenovar(plazoFijo, pAnterior, pFecha, pValoresImportes, bd, tran))
                resultado = false;

            return resultado;
        }

        /// <summary>
        /// Renueva un Plazo Fijo de forma Manual
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool Renovar(AhoPlazosFijos pRenovarPlazoFijo)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pRenovarPlazoFijo);

            bool resultado = true;

            AhoPlazosFijos pPlazoFijoAnterior = new AhoPlazosFijos();
            pPlazoFijoAnterior.IdPlazoFijo = pRenovarPlazoFijo.IdPlazoFijo;
            pPlazoFijoAnterior = AhorroF.PlazosFijosObtenerDatosCompletos(pPlazoFijoAnterior);

            //pRenovarPlazoFijo.IdPlazoFijo = 0;
            int idPlazoFijoRenovar = pRenovarPlazoFijo.IdPlazoFijo;

            pRenovarPlazoFijo.FechaVencimiento = pRenovarPlazoFijo.FechaInicioVigencia.AddDays(pRenovarPlazoFijo.PlazoDias);
            pRenovarPlazoFijo.ImporteInteres = this.CalcularInteres(pRenovarPlazoFijo);
            if (!this.ValidarAgregar(pRenovarPlazoFijo, pPlazoFijoAnterior))
                return false;

            if (!this.ValidarFechaVigenciaRenovacion(pRenovarPlazoFijo, pPlazoFijoAnterior))
                return false;

            //Para copiar los cotitulares
            foreach (AhoCotitulares cotitular in pRenovarPlazoFijo.Cotitulares)
                cotitular.EstadoColeccion = EstadoColecciones.Agregado;

            pRenovarPlazoFijo.EstadoColeccion = EstadoColecciones.Agregado;
            pRenovarPlazoFijo.Estado.IdEstado = (int)EstadosPlazosFijos.PendienteConfirmacion;

            if (pRenovarPlazoFijo.ImporteCapital == pPlazoFijoAnterior.ImporteCapital+pPlazoFijoAnterior.ImporteInteres)
            {
                pRenovarPlazoFijo.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.RenovacionPlazosFijos;

            }
            else if (pRenovarPlazoFijo.ImporteCapital > pPlazoFijoAnterior.ImporteTotal)
                pRenovarPlazoFijo.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.RenovacionPlazosFijosDepositos;
            else
                pRenovarPlazoFijo.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.RenovacionPlazosFijosExtraccion;

            pRenovarPlazoFijo.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
            //pRenovarPlazoFijo.Moneda.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;
            pRenovarPlazoFijo.FechaAlta = DateTime.Now;
            pRenovarPlazoFijo.Cuenta.SaldoActualOriginal = pRenovarPlazoFijo.Cuenta.SaldoActual;
            pRenovarPlazoFijo.Cuenta.SaldoActual = pRenovarPlazoFijo.Cuenta.SaldoActualOriginal - pRenovarPlazoFijo.ImporteCapital;
            //pRenovarPlazoFijo.FechaPago = pRenovarPlazoFijo.FechaAlta.AddDays(pRenovarPlazoFijo.PlazoDias);
            pRenovarPlazoFijo.ImporteInteres = this.CalcularInteres(pRenovarPlazoFijo);
            pRenovarPlazoFijo.IdPlazoFijoAnterior = pPlazoFijoAnterior.IdPlazoFijo;
            pRenovarPlazoFijo.UsuarioAlta.IdUsuarioAlta = pRenovarPlazoFijo.UsuarioLogueado.IdUsuario;

            pPlazoFijoAnterior.Estado.IdEstado = (int)EstadosPlazosFijos.Pagado;
            pPlazoFijoAnterior.Estado = TGEGeneralesF.TGEEstadosObtener(pPlazoFijoAnterior.Estado);

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            AhoPlazosFijos valorViejo = new AhoPlazosFijos();
            valorViejo.IdPlazoFijo = pPlazoFijoAnterior.IdPlazoFijo;
            valorViejo.UsuarioLogueado = pPlazoFijoAnterior.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);


            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    //if (pPlazoFijoAnterior.ImporteTotal != pRenovarPlazoFijo.ImporteCapital
                    //    && pRenovarPlazoFijo.ImporteCapital > pPlazoFijoAnterior.ImporteTotal + pRenovarPlazoFijo.Cuenta.SaldoActual)
                    //{
                    if (pRenovarPlazoFijo.RegistrarCajaAhorros)
                    {
                        //if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pPlazoFijoAnterior, bd, tran, "AhoPlazosFijosActualizar"))
                        //{
                        //    AyudaProgramacionLN.MapearError(pPlazoFijoAnterior, pRenovarPlazoFijo);
                        //    resultado = false;
                        //}

                        pRenovarPlazoFijo.Estado.IdEstado = (int)EstadosPlazosFijos.Confirmado;
                        pRenovarPlazoFijo.FechaConfirmacion = pRenovarPlazoFijo.FechaInicioVigencia;
                        pRenovarPlazoFijo.UsuarioConfirmacion.IdUsuarioConfirmacion = pRenovarPlazoFijo.UsuarioLogueado.IdUsuario;
                        pRenovarPlazoFijo.IdPlazoFijo = BaseDatos.ObtenerBaseDatos().Agregar(pRenovarPlazoFijo, bd, tran, "AhoPlazosFijosInsertarCuentasAhorros");
                        if (pRenovarPlazoFijo.IdPlazoFijo == 0)
                            resultado = false;

                        if (resultado && !new InterfazContableLN().AgregarPlazoFijoCajaAhorro(pRenovarPlazoFijo, bd, tran))
                            resultado = false;
                    }

                    else
                    {
                        //Guardo el Plazo Fijo
                        pRenovarPlazoFijo.IdPlazoFijo = BaseDatos.ObtenerBaseDatos().Agregar(pRenovarPlazoFijo, bd, tran, "AhoPlazosFijosInsertar");
                        if (pRenovarPlazoFijo.IdPlazoFijo == 0 && pRenovarPlazoFijo.IdPlazoFijo != idPlazoFijoRenovar)
                        {
                            resultado = false;
                        }

                        pPlazoFijoAnterior.Estado.IdEstado = (int)EstadosPlazosFijos.RenovacionPendienteConfirmacion;
                        pPlazoFijoAnterior.Estado = TGEGeneralesF.TGEEstadosObtener(pPlazoFijoAnterior.Estado);
                        if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pPlazoFijoAnterior, bd, tran, "AhoPlazosFijosActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(pPlazoFijoAnterior, pRenovarPlazoFijo);
                            resultado = false;
                        }
                        if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pPlazoFijoAnterior, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(pPlazoFijoAnterior, pRenovarPlazoFijo);
                            resultado = false;
                        }

                        if (pPlazoFijoAnterior.ImporteTotal == pRenovarPlazoFijo.ImporteCapital)
                        {
                            /* El plazo fijo nuevo queda como confirmado y el anterior como pendiente de pago. Luego contabiliza. */
                            pRenovarPlazoFijo.FechaConfirmacion = pRenovarPlazoFijo.FechaInicioVigencia;
                            if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pRenovarPlazoFijo, bd, tran, "AhoPlazoFijoConfirmarRenovaciones"))
                            {
                                AyudaProgramacionLN.MapearError(pRenovarPlazoFijo, pRenovarPlazoFijo);
                                resultado = false;
                            }

                            if (resultado && !new InterfazContableLN().AgregarPlazoFijoCajaAhorro(pRenovarPlazoFijo, bd, tran))
                                resultado = false;
                        }
                    }
                    if (resultado && !this.ActualizarCotitulares(pRenovarPlazoFijo, valorViejo, bd, tran))
                        resultado = false;                    

                    //if (resultado && pRenovarPlazoFijo.ImporteCapital == valorViejo.ImporteTotal && pRenovarPlazoFijo.ConfirmarRenovar)
                    //{
                    //    UsuarioLogueado usu = new UsuarioLogueado();
                    //    AyudaProgramacionLN.MatchObjectProperties(pRenovarPlazoFijo.UsuarioLogueado, usu);
                    //    pRenovarPlazoFijo = BaseDatos.ObtenerBaseDatos().Obtener<AhoPlazosFijos>("AhoPlazosFijosSeleccionarDescripcion", pRenovarPlazoFijo, bd, tran);
                    //    AyudaProgramacionLN.MatchObjectProperties(usu, pRenovarPlazoFijo.UsuarioLogueado);
                    //    resultado = this.ConfirmarRenovar(pRenovarPlazoFijo, bd, tran);
                    //}
                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pRenovarPlazoFijo, bd, tran))
                        resultado = false;
                    //Control Comentarios y Archivos
                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pRenovarPlazoFijo, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pRenovarPlazoFijo, bd, tran))
                        resultado = false;
                    //Fin control Comentarios y Archivos

                    if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pRenovarPlazoFijo, bd, tran, "AhoPlazoFijoRenovacionesPosterior"))
                    {
                        AyudaProgramacionLN.MapearError(pRenovarPlazoFijo, pRenovarPlazoFijo);
                        resultado = false;
                    }

                    /*Parche agregado para contabilizar renovaciones de plazos fijos cuando se decuentan sellados por campos dinamicos
                     La renovacion se confirma AhoPlazoFijoRenovacionesPosterior*/
                    AhoPlazosFijos plazosFijosNuevo = BaseDatos.ObtenerBaseDatos().Obtener<AhoPlazosFijos>("AhoPlazosFijosSeleccionarDescripcion", pRenovarPlazoFijo, bd, tran);
                    if (plazosFijosNuevo.Estado.IdEstado == (int)EstadosPlazosFijos.Confirmado)
                    {
                        if (resultado && !new InterfazContableLN().AgregarPlazoFijoCajaAhorro(pRenovarPlazoFijo, bd, tran))
                            resultado = false;
                    }

                    if (resultado)
                    {
                        tran.Commit();
                        pRenovarPlazoFijo.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        pRenovarPlazoFijo.IdPlazoFijo = pPlazoFijoAnterior.IdPlazoFijo;
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pRenovarPlazoFijo.CodigoMensaje = ex.Message;
                    return false;
                }
            }
            return resultado;
        }

        private bool ValidarAgregar(AhoPlazosFijos pParametro, AhoPlazosFijos pAnterior)
        {
            if (pParametro.Plazo.PlazoDias > pParametro.PlazoDias)
            {
                pParametro.CodigoMensaje = "ValidarPlazoDias";
                pParametro.CodigoMensajeArgs.Add(pParametro.Plazo.PlazoDias.ToString());
                //pParametro.CodigoMensajeArgs.Add(pParametro.PlazoDias);
                return false;
            }
            if (pParametro.TasaInteres == 0)
            {
                pParametro.CodigoMensaje = "ValidarTasaInteres";
                pParametro.CodigoMensajeArgs.Add(pParametro.TasaInteres.ToString("N2"));
                return false;
            }

            if (pParametro.FechaInicioVigencia.Date > DateTime.Now.Date)
            {
                pParametro.CodigoMensaje = "ValidarFechaInicioVigencia";
                pParametro.CodigoMensajeArgs.Add(pParametro.FechaInicioVigencia.ToShortDateString());
                return false;
            }

            if (!TGEGeneralesF.ListasValoresFeriadosValidar(pParametro.FechaVencimiento, pParametro))
                return false;

            if (pParametro.Cuenta.IdCuenta == 0
                && pParametro.TipoRenovacion.IdTipoRenovacion == (int)EnumAhorrosTiposRenovaciones.Capital)
            {
                pParametro.CodigoMensaje = "ValidarTipoRenovacionSinCuenta";
                pParametro.CodigoMensajeArgs.Add(pParametro.FechaInicioVigencia.ToShortDateString());
                return false;
            }

            //if (pParametro.ImporteCapital == pAnterior.ImporteTotal)
            //{
            //    if (!pParametro.ConfirmarRenovar)
            //    {
            //        pParametro.ConfirmarAccion = true;
            //        pParametro.CodigoMensaje = "PlazoFijoConfirmarRenovar";
            //        pParametro.CodigoMensajeArgs.Add(pParametro.ImporteTotal.ToString("C2"));
            //        pParametro.CodigoMensajeArgs.Add(pParametro.FechaVencimiento.ToShortDateString());
            //        return false;
            //    }
            //}

            return true;
        }

        private bool ValidarFechaVigenciaRenovacion(AhoPlazosFijos pRenovarPlazoFijo, AhoPlazosFijos pPlazoFijoAnterior)
        {
            if (pRenovarPlazoFijo.FechaInicioVigencia.Date < pPlazoFijoAnterior.FechaVencimiento.Date)
            {
                pRenovarPlazoFijo.CodigoMensaje = "ValidarFechaInicioVigenciaRenovacion";
                pRenovarPlazoFijo.CodigoMensajeArgs.Add(pPlazoFijoAnterior.FechaVencimiento.ToShortDateString());
                //pParametro.CodigoMensajeArgs.Add(pParametro.PlazoDias);
                return false;
            }
            return true;
        }

        private bool Validar(AhoPlazosFijos pParametro)
        {
            //if (pParametro.ImporteCapital > pParametro.Cuenta.SaldoActual)
            //{
            //    pParametro.CodigoMensaje = "AhoImporteMayorSaldoActual";
            //    pParametro.CodigoMensajeArgs.Add(pParametro.Cuenta.SaldoActual.ToString("C2"));
            //    return false;
            //}
            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "AhoPlazosFijosValidaciones"))
                return false;
            return true;
        }

        /// <summary>
        /// Devuelve el Interes para un Plazo Fijo
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public decimal CalcularInteres(AhoPlazosFijos pParametro)
        {
            return ( Math.Round(pParametro.ImporteCapital * pParametro.TasaInteres / 365 * pParametro.PlazoDias / 100, 2));
        }

        /// <summary>
        /// Anula un Plazo Fijo que no fue Confirmado en Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool Borrar(AhoPlazosFijos pParametro)
        {
            AhoPlazosFijos plazoFijoAnterior = new AhoPlazosFijos();
            //Pendiente Confirmación Deposito o Renovacion
            if (pParametro.Estado.IdEstado == (int)EstadosPlazosFijos.PendienteConfirmacion)
            {
                pParametro.Estado.IdEstado = (int)EstadosPlazosFijos.Baja;
                pParametro.Estado = TGEGeneralesF.TGEEstadosObtener(pParametro.Estado);
                pParametro.FechaCancelacion = DateTime.Now;
                pParametro.ImporteInteres = 0;
                pParametro.UsuarioCancelacion.IdUsuarioCancelacion = pParametro.UsuarioLogueado.IdUsuarioEvento;
                pParametro.FechaCancelacion = DateTime.Now;

                if (pParametro.IdPlazoFijoAnterior > 0)
                {
                    plazoFijoAnterior.IdPlazoFijo = pParametro.IdPlazoFijoAnterior;
                    plazoFijoAnterior = this.ObtenerDatosCompletos(plazoFijoAnterior);
                    plazoFijoAnterior.Estado.IdEstado = (int)EstadosPlazosFijos.Confirmado;
                    plazoFijoAnterior.Estado = TGEGeneralesF.TGEEstadosObtener(plazoFijoAnterior.Estado);
                }
            }
            else if (pParametro.Estado.IdEstado == (int)EstadosPlazosFijos.PendientePago)
            {
                pParametro.Estado.IdEstado = (int)EstadosPlazosFijos.Confirmado;
                pParametro.Estado = TGEGeneralesF.TGEEstadosObtener(pParametro.Estado);
                pParametro.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.DepositoPlazoFijo);
            }
            return this.Modificar(pParametro, plazoFijoAnterior);
        }


        public bool AnularCancelacion(AhoPlazosFijos pParametro)
        {

            //Pendiente Confirmación Deposito o Renovacion
            if (pParametro.Estado.IdEstado == (int)EstadosPlazosFijos.PendienteCancelacion)
            {
                pParametro.Estado.IdEstado = (int)EstadosPlazosFijos.Confirmado;
                pParametro.Estado = TGEGeneralesF.TGEEstadosObtener(pParametro.Estado);
                pParametro.FechaCancelacion = DateTime.Now;
                pParametro.ImporteInteres = this.CalcularInteres(pParametro);


            }
            else
                return false;

            bool resultado = true;
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "AhoPlazosFijosAnularCancelacion"))
                    {
                        AyudaProgramacionLN.MapearError(pParametro, pParametro);
                        resultado = false;
                    }

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        pParametro.IdPlazoFijo = pParametro.IdPlazoFijo;
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
            }
            return resultado;
        }

        /// <summary>
        /// Anula un Plazo Fijo que no fue Confirmado en Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        /// 
        public AhoPlazosFijos RecalcularRenovacionAnticipada(AhoPlazosFijos pParametro)
        {
            var dias = Convert.ToInt32((DateTime.Now.Date - pParametro.FechaInicioVigencia).TotalDays);

            var importe = pParametro.ImporteCapital;
            var interes = pParametro.TasaInteres;

            var importeInteres = (importe * interes / 365 * dias) / 100;
            pParametro.ImporteInteres = Math.Round(importeInteres, 2); 
            pParametro.PlazoDias = dias;
            
            //pParametro.ImporteTotal = total;
            return pParametro;
        }

        public bool RenovacionAnticipada(AhoPlazosFijos pRenovarPlazoFijo)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pRenovarPlazoFijo);

            bool resultado = true;

            AhoPlazosFijos pPlazoFijoAnterior = new AhoPlazosFijos();
            pPlazoFijoAnterior.IdPlazoFijo = pRenovarPlazoFijo.IdPlazoFijo;
            pPlazoFijoAnterior = AhorroF.PlazosFijosObtenerDatosCompletos(pPlazoFijoAnterior);
            pPlazoFijoAnterior = RecalcularRenovacionAnticipada(pPlazoFijoAnterior);
            //pRenovarPlazoFijo.IdPlazoFijo = 0;
            int idPlazoFijoRenovar = pRenovarPlazoFijo.IdPlazoFijo;

            pRenovarPlazoFijo.FechaVencimiento = pRenovarPlazoFijo.FechaInicioVigencia.AddDays(pRenovarPlazoFijo.PlazoDias);
            pRenovarPlazoFijo.ImporteInteres = this.CalcularInteres(pRenovarPlazoFijo);
            if (!this.ValidarAgregar(pRenovarPlazoFijo, pPlazoFijoAnterior))
                return false;

            //Para copiar los cotitulares
            foreach (AhoCotitulares cotitular in pRenovarPlazoFijo.Cotitulares)
                cotitular.EstadoColeccion = EstadoColecciones.Agregado;

            pRenovarPlazoFijo.EstadoColeccion = EstadoColecciones.Agregado;
            pRenovarPlazoFijo.Estado.IdEstado = (int)EstadosPlazosFijos.PendienteConfirmacion;

            if (pRenovarPlazoFijo.ImporteCapital == pPlazoFijoAnterior.ImporteCapital + pPlazoFijoAnterior.ImporteInteres)
                pRenovarPlazoFijo.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.RenovacionPlazosFijos;
            else if (pRenovarPlazoFijo.ImporteCapital > pPlazoFijoAnterior.ImporteTotal)
                pRenovarPlazoFijo.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.RenovacionPlazosFijosDepositos;
            else
                pRenovarPlazoFijo.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.RenovacionPlazosFijosExtraccion;

            pRenovarPlazoFijo.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
            //pRenovarPlazoFijo.Moneda.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;
            pRenovarPlazoFijo.FechaAlta = DateTime.Now;
            pRenovarPlazoFijo.Cuenta.SaldoActualOriginal = pRenovarPlazoFijo.Cuenta.SaldoActual;
            pRenovarPlazoFijo.Cuenta.SaldoActual = pRenovarPlazoFijo.Cuenta.SaldoActualOriginal - pRenovarPlazoFijo.ImporteCapital;
            //pRenovarPlazoFijo.FechaPago = pRenovarPlazoFijo.FechaAlta.AddDays(pRenovarPlazoFijo.PlazoDias);
            pRenovarPlazoFijo.ImporteInteres = this.CalcularInteres(pRenovarPlazoFijo);
            pRenovarPlazoFijo.IdPlazoFijoAnterior = pPlazoFijoAnterior.IdPlazoFijo;
            pRenovarPlazoFijo.UsuarioAlta.IdUsuarioAlta = pRenovarPlazoFijo.UsuarioLogueado.IdUsuario;
            
            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            AhoPlazosFijos valorViejo = new AhoPlazosFijos();
            valorViejo.IdPlazoFijo = pPlazoFijoAnterior.IdPlazoFijo;
          
            valorViejo.UsuarioLogueado = pPlazoFijoAnterior.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);


            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    //if (pPlazoFijoAnterior.ImporteTotal != pRenovarPlazoFijo.ImporteCapital
                    //    && pRenovarPlazoFijo.ImporteCapital > pPlazoFijoAnterior.ImporteTotal + pRenovarPlazoFijo.Cuenta.SaldoActual)
                    //{
                    if (pRenovarPlazoFijo.RegistrarCajaAhorros)
                    {
                        pPlazoFijoAnterior.Estado.IdEstado = (int)EstadosPlazosFijos.Confirmado;
                        if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pPlazoFijoAnterior, bd, tran, "AhoPlazosFijosActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(pPlazoFijoAnterior, pRenovarPlazoFijo);
                            resultado = false;
                        }

                        pRenovarPlazoFijo.Estado.IdEstado = (int)EstadosPlazosFijos.Confirmado;
                        pRenovarPlazoFijo.FechaConfirmacion = pRenovarPlazoFijo.FechaInicioVigencia;
                        pRenovarPlazoFijo.UsuarioConfirmacion.IdUsuarioConfirmacion = pRenovarPlazoFijo.UsuarioLogueado.IdUsuario;
                        pRenovarPlazoFijo.IdPlazoFijo = BaseDatos.ObtenerBaseDatos().Agregar(pRenovarPlazoFijo, bd, tran, "AhoPlazosFijosInsertarCuentasAhorros");
                        if (pRenovarPlazoFijo.IdPlazoFijo == 0)
                            resultado = false;

                        if (resultado && !new InterfazContableLN().AgregarPlazoFijoCajaAhorro(pRenovarPlazoFijo, bd, tran))
                            resultado = false;
                    }

                    else
                    {
                        //Guardo el Plazo Fijo
                        pRenovarPlazoFijo.IdPlazoFijo = BaseDatos.ObtenerBaseDatos().Agregar(pRenovarPlazoFijo, bd, tran, "AhoPlazosFijosInsertar");
                        if (pRenovarPlazoFijo.IdPlazoFijo == 0 && pRenovarPlazoFijo.IdPlazoFijo != idPlazoFijoRenovar)
                        {
                            resultado = false;
                        }

                        pPlazoFijoAnterior.Estado.IdEstado = (int)EstadosPlazosFijos.RenovacionPendienteConfirmacion;
                        pPlazoFijoAnterior.Estado = TGEGeneralesF.TGEEstadosObtener(pPlazoFijoAnterior.Estado);
                        if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pPlazoFijoAnterior, bd, tran, "AhoPlazosFijosActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(pPlazoFijoAnterior, pRenovarPlazoFijo);
                            resultado = false;
                        }
                        if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pPlazoFijoAnterior, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(pPlazoFijoAnterior, pRenovarPlazoFijo);
                            resultado = false;
                        }

                        if (pPlazoFijoAnterior.ImporteTotal == pRenovarPlazoFijo.ImporteCapital)
                        {
                            /* El plazo fijo nuevo queda como confirmado y el anterior como pendiente de pago. Luego contabiliza. */
                            pRenovarPlazoFijo.FechaConfirmacion = pRenovarPlazoFijo.FechaInicioVigencia;
                            if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pRenovarPlazoFijo, bd, tran, "AhoPlazoFijoConfirmarRenovaciones"))
                            {
                                AyudaProgramacionLN.MapearError(pRenovarPlazoFijo, pRenovarPlazoFijo);
                                resultado = false;
                            }

                            if (resultado && !new InterfazContableLN().AgregarPlazoFijoCajaAhorro(pRenovarPlazoFijo, bd, tran))
                                resultado = false;
                        }
                    }

                    if (resultado && !this.ActualizarCotitulares(pRenovarPlazoFijo, valorViejo, bd, tran))
                        resultado = false;

                    //if (resultado && pRenovarPlazoFijo.ImporteCapital == valorViejo.ImporteTotal && pRenovarPlazoFijo.ConfirmarRenovar)
                    //{
                    //    UsuarioLogueado usu = new UsuarioLogueado();
                    //    AyudaProgramacionLN.MatchObjectProperties(pRenovarPlazoFijo.UsuarioLogueado, usu);
                    //    pRenovarPlazoFijo = BaseDatos.ObtenerBaseDatos().Obtener<AhoPlazosFijos>("AhoPlazosFijosSeleccionarDescripcion", pRenovarPlazoFijo, bd, tran);
                    //    AyudaProgramacionLN.MatchObjectProperties(usu, pRenovarPlazoFijo.UsuarioLogueado);
                    //    resultado = this.ConfirmarRenovar(pRenovarPlazoFijo, bd, tran);
                    //}

                    //Control Comentarios y Archivos
                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pRenovarPlazoFijo, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pRenovarPlazoFijo, bd, tran))
                        resultado = false;
                    //Fin control Comentarios y Archivos

                    if (resultado)
                    {
                        tran.Commit();
                        pRenovarPlazoFijo.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        pRenovarPlazoFijo.IdPlazoFijo = pPlazoFijoAnterior.IdPlazoFijo;
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pRenovarPlazoFijo.CodigoMensaje = ex.Message;
                    return false;
                }
            }
            return resultado;
        }

        /// <summary>
        /// Cancela un Plazo Fijo que fue confirmado en Caja.
        /// El plazo fijo queda pendiente de cancelacion por caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool Cancelar(AhoPlazosFijos pParametro)
        {
            pParametro.Estado.IdEstado = (int)EstadosPlazosFijos.PendienteCancelacion;
            pParametro.FechaCancelacion = DateTime.Now;
            pParametro.ImporteInteres = 0;
            pParametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.CancelacionAnticipadaPlazoFijo;
            return this.Modificar(pParametro, new AhoPlazosFijos());
        }

        /// <summary>
        /// Paga un Plazo Fijo que no fue Confirmado en Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool Pagar(AhoPlazosFijos pParametro)
        {
            pParametro.Estado.IdEstado = (int)EstadosPlazosFijos.PendientePago;
            pParametro.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.ExtraccionPlazoFijo);
            return this.Modificar(pParametro, new AhoPlazosFijos());
        }

        /// <summary>
        /// Modifica un Plazo Fijo
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool Modificar(AhoPlazosFijos pParametro, AhoPlazosFijos pPlazoFijoAnterior)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (pParametro.Cuenta.IdCuenta == 0
                && pParametro.TipoRenovacion.IdTipoRenovacion == (int)EnumAhorrosTiposRenovaciones.Capital
                && pParametro.Estado.IdEstado==(int)EstadosPlazosFijos.Confirmado)
            {
                pParametro.CodigoMensaje = "ValidarTipoRenovacionSinCuenta";
                pParametro.CodigoMensajeArgs.Add(pParametro.FechaInicioVigencia.ToShortDateString());
                return false;
            }

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            AhoPlazosFijos valorViejo = new AhoPlazosFijos();
            valorViejo.IdPlazoFijo = pParametro.IdPlazoFijo;
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
                    //Para Cancelaciones de Plazo Fijo en Cajas de Ahorros
                    if (pParametro.Cuenta.IdCuenta > 0
                        && pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.CancelacionAnticipadaPlazoFijo
                        && pParametro.CancelaCajaAhorros)
                    {
                        pParametro.Estado.IdEstado = (int)EstadosPlazosFijos.Confirmado;
                        pParametro.FechaConfirmacion = pParametro.FechaInicioVigencia;
                        pParametro.UsuarioConfirmacion.IdUsuarioConfirmacion = pParametro.UsuarioLogueado.IdUsuario;
                        //resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pParametro, bd, tran, "AhoPlazosFijosInsertarCuentasAhorros");
                        resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "AhoPlazosFijosCancelarAnticipadoCuentasAhorros");
                    }
                    /**** Para Pagar un Plazo Fijo en Cajas de Ahorros. ****/
                    /* Cambio a CancelaCajaAhorros para que quede mas ordenado. */
                    else if (pParametro.Cuenta.IdCuenta > 0
                        && pParametro.CancelaCajaAhorros //pParametro.RegistrarCajaAhorros
                        && pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.ExtraccionPlazoFijo
                        )
                    {
                        pParametro.FechaPago = DateTime.Now;
                        resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "AhoPlazosFijosPagarCuentasAhorros");
                    }
                    else
                    {
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "AhoPlazosFijosActualizar"))
                            resultado = false;
                    }

                    if (resultado && !this.ActualizarCotitulares(pParametro, valorViejo, bd, tran))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && pPlazoFijoAnterior.IdPlazoFijo > 0)
                    {
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(pPlazoFijoAnterior, bd, tran, "AhoPlazosFijosActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(pPlazoFijoAnterior, pParametro);
                            resultado = false;
                        }
                    }

                    //Control Comentarios y Archivos

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;
                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;
                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;
                    //Fin control Comentarios y Archivos

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
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
            }
            return resultado;
        }

        public override bool Modificar(AhoPlazosFijos pParametro)
        {
            return this.Modificar(pParametro, new AhoPlazosFijos());
        }

        private bool ActualizarCotitulares(AhoPlazosFijos pParametro, AhoPlazosFijos pValorViejo, Database bd, DbTransaction tran)
        {
            foreach (AhoCotitulares cotitular in pParametro.Cotitulares)
            {
                switch (cotitular.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        cotitular.IdPlazoFijo = pParametro.IdPlazoFijo;
                        cotitular.Estado.IdEstado = (int)Estados.Activo;
                        cotitular.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuario;
                        cotitular.UsuarioLogueado = pParametro.UsuarioLogueado;
                        cotitular.FechaAlta = DateTime.Now;
                        cotitular.IdCotitular = BaseDatos.ObtenerBaseDatos().Agregar(cotitular, bd, tran, "AhoCotitularesInsertar");
                        if (cotitular.IdCotitular == 0)
                        {
                            AyudaProgramacionLN.MapearError(cotitular, pParametro);
                            return false;
                        }
                        break;
                    case EstadoColecciones.Borrado:
                    case EstadoColecciones.Modificado:
                        cotitular.Estado.IdEstado = (int)Estados.Baja;
                        cotitular.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(cotitular, bd, tran, "AhoCotitularesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(cotitular, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.Cotitulares.Find(x => x.IdCotitular == cotitular.IdCotitular), Acciones.Update, cotitular, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(cotitular, pParametro);
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;
        }
    }
}
