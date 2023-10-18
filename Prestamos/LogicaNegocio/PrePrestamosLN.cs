using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Prestamos.Entidades;
using Servicio.AccesoDatos;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Afiliados.Entidades;
using Comunes.LogicaNegocio;
using Generales.FachadaNegocio;
using Auditoria;
using Auditoria.Entidades;
using Generales.Entidades;
using Cargos.Entidades;
using System.Collections;
using Cargos;
using ProcesosDatos;
using Contabilidad.Entidades;
using CuentasPagar.Entidades;
using System.Data;
using System.Net.Mail;
using System.Web;
using Servicio.Encriptacion;
using Arba.WebServices;

namespace Prestamos.LogicaNegocio
{
    class PrePrestamosLN : BaseLN<PrePrestamos>
    {

        /// <summary>
        /// Devuelve una lista de Prestamos por Afiliado
        /// </summary>
        /// <param name="pAfiliado"></param>
        /// <returns></returns>
        public DataTable ObtenerPorAfiliado(AfiAfiliados pAfiliado)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("PrePrestamosSeleccionarPorAfiAfiliados", pAfiliado);
        }

        public DataTable ObtenerPorAfiliadoGeneral(PrePrestamos prePrestamos)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[PrePrestamosSeleccionarPorAfiAfiliadosGeneral]", prePrestamos);
        }

        /// <summary>
        /// Devuelve una lista de Prestamos por Afiliado para Cancelar
        /// </summary>
        /// <param name="pAfiliado"></param>
        /// <returns></returns>
        public List<PrePrestamos> ObtenerPorAfiliadoCancelacion(PrePrestamos pPrestamo)
        {
            List<PrePrestamos> resultado = new List<PrePrestamos>();
            List<PrePrestamos> prestamosCancelar = BaseDatos.ObtenerBaseDatos().ObtenerLista<PrePrestamos>("[PrePrestamosSeleccionarPorAfiAfiliadosCancelar]", pPrestamo);
            //PrePrestamos prestamo;
            //foreach (PrePrestamos pre in prestamosCancelar)
            //{
            //    prestamo = this.ObtenerDatosCompletos(pre);
            //    prestamo.FechaCancelacion = DateTime.Now;
            //    this.CalcularImporteCancelar(prestamo);
            //    resultado.Add(prestamo);
            //}
            //return resultado;
            return prestamosCancelar;
        }

        public override PrePrestamos ObtenerDatosCompletos(PrePrestamos pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<PrePrestamos>("PrePrestamosSeleccionar", pParametro);
            pParametro.PrestamosCuotas = BaseDatos.ObtenerBaseDatos().ObtenerLista<PrePrestamosCuotas>("PrePrestamosCuotasSeleccionar", pParametro);
            pParametro.Cancelaciones = BaseDatos.ObtenerBaseDatos().ObtenerLista<PrePrestamos>("PrePrestamosSeleccionarCancelados", pParametro);
            foreach (PrePrestamos prestamo in pParametro.Cancelaciones)
                prestamo.PrestamosCuotas = BaseDatos.ObtenerBaseDatos().ObtenerLista<PrePrestamosCuotas>("PrePrestamosCuotasSeleccionar", prestamo);

            if (pParametro.TipoOperacion.IdTipoOperacion==(int)EnumTGETiposOperaciones.CompraDeCheque)
                pParametro.PrestamosCheques = BaseDatos.ObtenerBaseDatos().ObtenerLista<PrePrestamosCheques>("PrePrestamosChequesSeleccionarPorCheque", pParametro);
            pParametro.CargosExcedidos = BaseDatos.ObtenerBaseDatos().ObtenerLista<CarCuentasCorrientes>("PrePrestamosCuentasCorrientesSeleccionarPorPrestamo", pParametro);
            pParametro.SolicitudesPagos = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapSolicitudPago>("PrePrestamosSolicitudesPagosSeleccionarPorPrestamo", pParametro);
            pParametro.ImporteSolicitudesPagos = pParametro.SolicitudesPagos.Sum(x => x.ImporteTotal);
            pParametro.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;

        }

        public DataTable HabilitarControlesCancelaciones(PrePrestamos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("PrePrestamosDeshabilitarControles",pParametro);
        }

        public DataTable ObtenerCardsBootStrap(PrePrestamos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("PrePrestamosSeleccionarCards", pParametro);
        }

        public DataTable ObtenerDocumentosAsociados(PrePrestamos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("PrePresamosSeleccionarDocumentosAsociados", pParametro);
        }

        public DataTable ObtenerCobrosPorPrestamo(PrePrestamos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CarCuentasCorrientesSeleccionarCobrosPorPrestamoDataTable", pParametro);
        }

        public PrePrestamos ObtenerDatosCancelacion(PrePrestamos pParametro)
        {
            pParametro = this.ObtenerDatosCompletos(pParametro);
            pParametro.PrestamosCuotas.ForEach(x => x.Incluir = true);
            //this.CalcularImporteCancelar(pParametro, true);
            PrePrestamos prestamoCanc = BaseDatos.ObtenerBaseDatos().Obtener<PrePrestamos>("[PrePrestamosCalcularCancelacionPrestamo]", pParametro);
            pParametro.ImporteCancelacion = prestamoCanc.ImporteCancelacion;
            return pParametro;
        }

        public override List<PrePrestamos> ObtenerListaFiltro(PrePrestamos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<PrePrestamos>("PrePrestamosSeleccionarFiltro", pParametro);

        }

        

        public List<PrePrestamos> ObtenerParaCesionar(PrePrestamos pParametros)
        {
            return new List<PrePrestamos>();
        }

        public List<PrePrestamosCuotas> ObtenerCancelacionAnticipada(AfiAfiliados pAfiliado)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<PrePrestamosCuotas>("PrePrestamosCuotasSeleccionarCancelacionAnticipada", pAfiliado);
        }

        public List<CarCuentasCorrientes> ObtenerPendientesCobro(CarCuentasCorrientes pParametro)
        {
            List<CarCuentasCorrientes> lista = BaseDatos.ObtenerBaseDatos().ObtenerLista<CarCuentasCorrientes>("CarCuentasCorrientesSeleccionarPorAfiliadoPendienteCobroSinPrestamo", pParametro); //CargosF.CuentasCorrientesObtenerPendientesCobro(pParametro);          
            //foreach (CarCuentasCorrientes cargo in lista)
            //{
            //    if (cargo.Periodo < AyudaProgramacionLN.ObtenerPeriodo(DateTime.Now)
            //        || cargo.TipoCargo.IdTipoCargo == (int)EnumTiposCargos.DescuentosPendientes)
            //    {
            //        cargo.Incluir = true;
            //    }
            //}
            return lista;
        }

        private TGETiposOperaciones PrestamosOperacionesObtener(TGETiposOperaciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<TGETiposOperaciones>("PrePrestamosTiposOperacionesCancelacionesSeleccionar", pParametro);
        }

        private TGETiposOperaciones PrestamosOperacionesObtenerCancelacion(TGETiposOperaciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<TGETiposOperaciones>("PrePrestamosTiposOperacionesCancelacionesSeleccionarCancelacion", pParametro);
        }

        public override bool Agregar(PrePrestamos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            pParametro.ImporteExcedido = pParametro.ImporteCargosPendientes;

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;


            if (pParametro.IdPrestamo > 0)
                return true;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = this.Agregar(pParametro, bd, tran);

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        pParametro.IdPrestamo = 0;
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.IdPrestamo = 0;
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        public bool Agregar(PrePrestamos pParametro, Database bd, DbTransaction tran)
        {
            if (pParametro.PrestamosCheques.Count > 0)
                pParametro.LoteCheques = pParametro.PrestamosCheques.ToDataTable().ToXmlDocument();

            if (!this.ValidarAgregar(pParametro))
                return false;

            bool resultado = true;

            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "PrePrestamosValidaciones"))
            {
                resultado = false;
            }

            //Guardo el Prestamo
            if (resultado)
            {
                pParametro.IdPrestamo = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "PrePrestamosInsertar");
                if (pParametro.IdPrestamo == 0)
                    resultado = false;
            }

            // Envio a grabar la ListaCuotas
            if (resultado && !this.CuotasActualizar(pParametro, bd, tran))
                resultado = false;

            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pParametro, bd, tran, "PrePrestamosCuotasInsertarRecalculos"))
                resultado = false;

            if (resultado && !this.ActualizarCancelaciones(pParametro, bd, tran))
                resultado = false;

            if (resultado && !this.ActualizarCuentaCorriente(pParametro, bd, tran))
                resultado = false;

            if (resultado && !this.ActualizarSolicitudesPagos(pParametro, bd, tran))
                resultado = false;

            //Cheques
            if (resultado && !this.ChequesActualizar(pParametro, bd, tran))
                resultado = false;

            if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                resultado = false;
            
            //Control Comentarios y Archivos
            if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                resultado = false;

            if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                resultado = false;
            //Fin control Comentarios y Archivos

            if (resultado && !this.ActualizarCampoIPS(pParametro, bd, tran))
                resultado = false;

            return resultado;
        }

        /// <summary>
        /// Arma la cuponera y gastos de un prestamo en el objeto
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool AgregarPrevio(PrePrestamos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            if (pParametro.PrestamosCheques.Count>0)
                pParametro.LoteCheques = pParametro.PrestamosCheques.ToDataTable().ToXmlDocument();

            pParametro.ImporteExcedido = pParametro.ImporteCargosPendientes;

            if (!this.ValidarAgregar(pParametro))
                return false;

            if (!TGEGeneralesF.CamposValidar(pParametro, pParametro.Campos))
                return false;

            //pParametro.Estado.IdEstado = (int)EstadosPrestamos.Activo;
            if (!this.ArmarCuponera(pParametro))
                return false;

            return true;
        }

        private bool EsEjecucionGarantia(PrePrestamos pParametro)
        {
            if (true)
            {
                return true;
            }
            return false;
        }

        public override bool Modificar(PrePrestamos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            if (pParametro.PrestamosCheques.Count > 0)
                pParametro.LoteCheques = pParametro.PrestamosCheques.ToDataTable().ToXmlDocument();

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.Estado = TGEGeneralesF.TGEEstadosObtener(pParametro.Estado);
            //if (!this.Validar(pParametro))
            //    return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            PrePrestamos valorViejo = new PrePrestamos();
            valorViejo.IdPrestamo = pParametro.IdPrestamo;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            //if (EsEjecucionGarantia(pParametro))
            //    pParametro.ImporteCancelaciones = valorViejo.ImporteCancelaciones;

            //valorViejo.Campos = TGEGeneralesF.CamposObtenerListaFiltro(valorViejo, new Objeto());
            //valorViejo.Campos.AddRange( TGEGeneralesF.CamposObtenerListaFiltro(valorViejo, valorViejo.TipoOperacion));
            //valorViejo.Campos.AddRange(TGEGeneralesF.CamposObtenerListaFiltro(valorViejo, valorViejo.Estado));
            //valorViejo.Campos.AddRange(TGEGeneralesF.CamposObtenerListaFiltro(valorViejo, valorViejo.TipoValor));
            //valorViejo.Campos.AddRange(TGEGeneralesF.CamposObtenerListaFiltro(valorViejo, valorViejo.FormaCobroAfiliado));
            TGETiposOperaciones tipoOpBack = pParametro.TipoOperacion;
            switch (pParametro.Estado.IdEstado)
            {
                case (int)EstadosPrestamos.Activo:
                case (int)EstadosPrestamos.PreAutorizado:
                case (int)EstadosPrestamos.Autorizado:
                    if (!this.ValidarAgregar(pParametro))
                        return false;
                    if (!this.ArmarCuponera(pParametro))
                        return false;
                    break;
                case (int)EstadosPrestamos.Anulado:
                    this.MarcarCuponera(pParametro);
                    break;
                case (int)EstadosPrestamos.PendienteCancelacion:
                    //Valido que el Prestamo tenga cuotas pendientes de Facturar
                    if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "PrePrestamosCuotasValidarPendienteGenerarCargos"))
                    {
                        //pParametro.CodigoMensaje = "ValidarCuotasPendientesGenerarCargos";
                        return false;
                    }
                    if (pParametro.ImporteCancelacion <= 0)
                    {
                        pParametro.CodigoMensaje = "ValidarImporteCancelacion";
                        return false;
                    }

                    pParametro.FechaCancelacion = DateTime.Now;
                    pParametro.TipoOperacion = this.PrestamosOperacionesObtenerCancelacion(pParametro.TipoOperacion);
                    if (pParametro.TipoOperacion.IdTipoOperacion == 0)
                    {
                        pParametro.TipoOperacion = tipoOpBack;
                        pParametro.CodigoMensaje = "ValidarPrestamosTiposOperacionesCancelaciones";
                        return false;
                    }

                    //if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosLargoPlazo
                    //    || pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.Prestamos50)
                    //    pParametro.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.PrestamosLargoPlazoCancelacion);
                    //else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosCortoPlazo)
                    //    pParametro.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.PrestamosCortoPlazoCancelacion);
                    //else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosBancoDelSol)
                    //    pParametro.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.PrestamosBancoDelSolCancelacion);
                    //else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosFondosPropios)
                    //    pParametro.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.PrestamosFondosPropiosCancelacion);

                    //this.CalcularImporteCancelar(pParametro);
                    break;
                // Cuando estoy Anulando una Cancelacion
                case (int)EstadosPrestamos.Confirmado:
                    pParametro.TipoOperacion = this.PrestamosOperacionesObtener(pParametro.TipoOperacion);
                    if (pParametro.TipoOperacion.IdTipoOperacion == 0)
                    {
                        pParametro.TipoOperacion = tipoOpBack;
                        pParametro.CodigoMensaje = "ValidarPrestamosTiposOperacionesCancelaciones";
                        return false;
                    }
                    //if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosLargoPlazoCancelacion)
                    //    pParametro.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.PrestamosLargoPlazo);
                    //else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosCortoPlazoCancelacion)
                    //    pParametro.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.PrestamosCortoPlazo);
                    //else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosBancoDelSolCancelacion)
                    //    pParametro.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.PrestamosBancoDelSol);
                    //else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosFondosPropiosCancelacion)
                    //    pParametro.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.PrestamosFondosPropios);
                    break;
                default:
                    break;
            }

            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "PrePrestamosValidaciones"))
            {
                pParametro.TipoOperacion = tipoOpBack;
                return false;
            }

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {

                    resultado = this.Modificar(pParametro, bd, tran);

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;
                   
                    if (resultado && !this.CuotasActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ChequesActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarCancelaciones(pParametro, bd, tran))
                        resultado = false;

                    //Control Comentarios y Archivos
                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;
                    //Fin control Comentarios y Archivos

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarCampoIPS(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        pParametro.TipoOperacion = tipoOpBack;
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    pParametro.TipoOperacion = tipoOpBack;
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        /// <summary>
        /// Metodo (SP) para Anular Prestamos
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool AnularConfirmado(PrePrestamos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            PrePrestamos valorViejo = new PrePrestamos();
            valorViejo.IdPrestamo = pParametro.IdPrestamo;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);


            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.Estado = TGEGeneralesF.TGEEstadosObtener(pParametro.Estado);
            pParametro.FechaCancelacion = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(pParametro, bd, tran, "PrePrestamosAnularProceso");

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;
                    //Control Comentarios y Archivos
                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;
                    //Fin control Comentarios y Archivos


                    if (resultado && !this.ActualizarCampoIPS(pParametro, bd, tran))
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

        public bool AplicarCheque(PrePrestamos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            PrePrestamos valorViejo = new PrePrestamos();
            valorViejo.IdPrestamo = pParametro.IdPrestamo;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);


            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.Estado = TGEGeneralesF.TGEEstadosObtener(pParametro.Estado);
            pParametro.FechaCancelacion = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(pParametro, bd, tran, "PrePrestamosAplicarChequeProceso");

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;
                    //Control Comentarios y Archivos
                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
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
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        /// <summary>
        /// Confirma un Prestamos
        /// Metodo para los movimientos de Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool Confirmar(Objeto pParametro, DateTime pFecha, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            bool resultado = true;

            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "PrePrestamosValidarCancelacion"))
            {
                pParametro.CodigoMensaje = "ValidarCancelacion";
                return false;
            }

            PrePrestamos prestamo = (PrePrestamos)pParametro;
            TGETiposOperaciones operacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.CobroPrestamos);

            prestamo.Estado.IdEstado = (int)EstadosPrestamos.Confirmado;
            prestamo.FechaConfirmacion = pFecha;// DateTime.Now;
            prestamo.UsuarioConfirmacion.IdUsuarioConfirmacion = prestamo.UsuarioLogueado.IdUsuario;

            prestamo.ImporteExcedido = prestamo.ImporteCargosPendientes;

            if (!this.Modificar(prestamo, bd, tran))
                resultado = false;

            foreach (PrePrestamos cancelacion in prestamo.Cancelaciones)
            {
                //Levanto las Cuotas Pendientes en la CTA CTE
                //cancelacion.CuotasPendientesCuentaCorriente = BaseDatos.ObtenerBaseDatos().ObtenerLista<CarCuentasCorrientes>("CarCuentasCorrientesSeleccionarPorPrestamoPendiente", cancelacion, bd, tran);
                //cancelacion.IdTipoOperacionCobro = prestamo.TipoOperacion.IdTipoOperacion;
                //cancelacion.IdRefTipoOperacionCobro = prestamo.IdPrestamo;
                //Cancelo el Prestamo
                if (!this.CancelarConfirmar(cancelacion, pFecha, null, true, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(cancelacion, pParametro);
                    return false;
                }
            }

            //ACTUALIZO LA CUENTA CORRIENTE
            foreach (CarCuentasCorrientes cargo in prestamo.CargosExcedidos)
            {
                CarCuentasCorrientes cobroCtaCte = new CarCuentasCorrientes();
                cobroCtaCte.IdRefCuentaCorriente = cargo.IdCuentaCorriente;
                cobroCtaCte.FechaMovimiento = pFecha;// DateTime.Now;
                cobroCtaCte.Periodo = cargo.Periodo;
                cobroCtaCte.Estado.IdEstado = (int)EstadosCuentasCorrientes.Cobrado;
                cobroCtaCte.IdAfiliado = prestamo.Afiliado.IdAfiliado;
                cobroCtaCte.IdRefTipoOperacion = prestamo.IdPrestamo;
                cobroCtaCte.TipoOperacion.IdTipoOperacion = operacion.IdTipoOperacion; //(int)EnumTGETiposOperaciones.CobroPrestamos;
                cobroCtaCte.Concepto = string.Concat(operacion.TipoOperacion, " ", prestamo.IdPrestamo.ToString());
                //cobroCtaCte.TipoCargo.TipoOperacion.IdTipoOperacion = 
                cobroCtaCte.TipoOperacion.TipoMovimiento.IdTipoMovimiento = (int)EnumTGETiposMovimientos.Credito;
                cobroCtaCte.Importe = cargo.Importe; // -cargo.ImporteCobrado;
                //Por ahora se deja Efectivo
                cobroCtaCte.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
                cobroCtaCte.UsuarioLogueado.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;

                if (!CargosF.CuentasCorrientesAgregar(cobroCtaCte, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(cobroCtaCte, pParametro);
                    return false;
                }

                cargo.Estado.IdEstado = (int)EstadosCuentasCorrientes.Cobrado;
                cargo.ImporteCobrado += cargo.Importe; // -cargo.ImporteCobrado;
                cargo.UsuarioLogueado.IdUsuarioEvento = pParametro.UsuarioLogueado.IdUsuarioEvento;

                if (!CargosF.CuentasCorrientesActualizarEstado(cargo, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(cargo, pParametro);
                    return false;
                }
            }

            if (resultado && !new InterfazContableLN().AsientoOtorgamiento(prestamo, pValoresImportes, bd, tran))
                resultado = false;

            return resultado;
        }

        /// <summary>
        /// Confirma una Cancelacion de Prestamos
        /// Metodo para los movimientos de Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool CancelarConfirmar(Objeto pParametro, DateTime pFecha, List<InterfazValoresImportes> pValoresImportes, bool pNovacion, Database bd, DbTransaction tran)
        {
            bool resultado = true;

            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "PrePrestamosValidarCancelacion"))
            {
                pParametro.CodigoMensaje = "ValidarCancelacion";
                return false;
            }

            PrePrestamos prestamo = (PrePrestamos)pParametro;
            TGETiposOperaciones operacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.CobroPrestamos);

            int periodoCancelacion = Convert.ToInt32(string.Concat(prestamo.FechaCancelacion.Year.ToString(), prestamo.FechaCancelacion.Month.ToString().PadLeft(2, '0')));
            int periodoHoy = Convert.ToInt32(string.Concat(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString().PadLeft(2, '0')));

            ////if (prestamo.FechaCancelacion.Date != DateTime.Now.Date)
            //if (periodoCancelacion != periodoHoy)
            //{
            //    pParametro.CodigoMensaje = "ValidarFechaCancelacion";
            //    pParametro.CodigoMensajeArgs.Add(prestamo.FechaCancelacion.ToShortDateString());
            //    return false;
            //}

            //this.CalcularImporteCancelar(prestamo);

            //Levanto las Cuotas Pendientes en la CTA CTE
            prestamo.CuotasPendientesCuentaCorriente = BaseDatos.ObtenerBaseDatos().ObtenerLista<CarCuentasCorrientes>("CarCuentasCorrientesSeleccionarPorPrestamoPendiente", prestamo, bd, tran);

            CarCuentasCorrientes cobroCtaCte;
            CarCuentasCorrientes cuotaCtaCte;
            foreach (PrePrestamosCuotas cuota in prestamo.PrestamosCuotas)
            {
                if ((cuota.Estado.IdEstado == (int)EstadosCuotas.Activa || cuota.Estado.IdEstado == (int)EstadosCuotas.CobradaParcial)
                    && (pNovacion == false || (pNovacion && cuota.IdRefTipoOperacionCobro.HasValue && cuota.IdRefTipoOperacionCobro.Value > 0))
                    )
                {
                    cuota.Estado.IdEstado = (int)EstadosCuotas.Cancelada;
                    cuota.UsuarioLogueado = pParametro.UsuarioLogueado;
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(cuota, bd, tran, "PrePrestamosCuotasActualizar"))
                    {
                        AyudaProgramacionLN.MapearError(cuota, prestamo);
                        return false;
                        //break;
                    }

                    if (cuota.IdCuentaCorriente.HasValue)
                    {
                        cuotaCtaCte = prestamo.CuotasPendientesCuentaCorriente.Find(x => x.IdCuentaCorriente == cuota.IdCuentaCorriente.Value);
                        cobroCtaCte = new CarCuentasCorrientes();
                        cobroCtaCte.IdRefCuentaCorriente = cuotaCtaCte.IdCuentaCorriente;
                        cobroCtaCte.FechaMovimiento = pFecha;// DateTime.Now;
                        cobroCtaCte.Periodo = cuotaCtaCte.Periodo;
                        cobroCtaCte.Estado.IdEstado = (int)EstadosCuentasCorrientes.Cobrado;
                        cobroCtaCte.IdAfiliado = prestamo.Afiliado.IdAfiliado;
                        cobroCtaCte.IdRefTipoOperacion = prestamo.IdPrestamo;
                        cobroCtaCte.TipoOperacion.IdTipoOperacion = operacion.IdTipoOperacion; //(int)EnumTGETiposOperaciones.CobroPrestamos;
                        cobroCtaCte.Concepto = string.Concat(operacion.TipoOperacion, " ", prestamo.IdPrestamo.ToString());
                        //cobroCtaCte.TipoCargo.TipoOperacion.IdTipoOperacion = 
                        cobroCtaCte.TipoOperacion.TipoMovimiento.IdTipoMovimiento = (int)EnumTGETiposMovimientos.Credito;
                        cobroCtaCte.Importe = cuotaCtaCte.Importe - cuotaCtaCte.ImporteCobrado;
                        //Por ahora se deja Efectivo
                        cobroCtaCte.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
                        cobroCtaCte.UsuarioLogueado = pParametro.UsuarioLogueado;

                        if (!CargosF.CuentasCorrientesAgregar(cobroCtaCte, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(cobroCtaCte, pParametro);
                            return false;
                        }

                        cuotaCtaCte.Estado.IdEstado = (int)EstadosCuentasCorrientes.Cobrado;
                        cuotaCtaCte.ImporteContabilizar = cuotaCtaCte.Importe - cuotaCtaCte.ImporteCobrado;
                        cuotaCtaCte.ImporteCobrado += cuotaCtaCte.Importe - cuotaCtaCte.ImporteCobrado;// -cuota.ImporteCobrado;
                        cuotaCtaCte.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!CargosF.CuentasCorrientesActualizarEstado(cuotaCtaCte, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(cuotaCtaCte, pParametro);
                            return false;
                        }
                    }


                }
            }


            if (prestamo.PrestamosCuotas.Exists(x => x.Estado.IdEstado == (int)EstadosCuotas.Activa || x.Estado.IdEstado == (int)EstadosCuotas.CobradaParcial))
            {
                prestamo.Estado.IdEstado = (int)EstadosPrestamos.Confirmado;
                prestamo.FechaConfirmacionCancelacion = pFecha;// DateTime.Now;
                //prestamo.Estado.IdEstado = (int)EstadosPrestamos.CanceladoCuotasPendientes;
                //prestamo.FechaConfirmacionCancelacion = pFecha;// DateTime.Now;
            }
            else
            {
                prestamo.Estado.IdEstado = (int)EstadosPrestamos.Cancelado;
                prestamo.FechaConfirmacionCancelacion = pFecha;// DateTime.Now;
            }

            if (!this.Modificar(prestamo, bd, tran))
                return false;

            if (!pNovacion)
                if (!new InterfazContableLN().AsientoCancelacion(prestamo, pValoresImportes, bd, tran))
                    return false;

            return resultado;
        }

        //ESTE METODO SE USA PARA ANULAR UN PRESTAMO EN ORDEN DE COBRO
        public bool AnularPrestamo(PrePrestamos pParametro, Database bd, DbTransaction tran)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            pParametro.Estado.IdEstado = (int)EstadosPrestamos.Anulado;
            bool resultado = true;

            this.MarcarCuponera(pParametro);

            resultado = this.Modificar(pParametro, bd, tran);

            if (resultado && !this.CuotasActualizar(pParametro, bd, tran))
                resultado = false;

            if (resultado && !this.ActualizarCancelaciones(pParametro, bd, tran))
                resultado = false;

            if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                resultado = false;

            return resultado;
        }

        
        private bool Modificar(PrePrestamos pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "PrePrestamosActualizar"))
                return false;

            return true;
        }

        private bool ActualizarCancelaciones(PrePrestamos pParametro, Database bd, DbTransaction tran)
        {
            foreach (PrePrestamos prestamo in pParametro.Cancelaciones)
            {
                //ALTA DE PRESTAMO
                //Se agregaron pendiente y preautorizado
                if (pParametro.Estado.IdEstado == (int)EstadosPrestamos.Activo ||
                    pParametro.Estado.IdEstado == (int)EstadosPrestamos.Pendiente ||
                    pParametro.Estado.IdEstado == (int)EstadosPrestamos.PreAutorizado
                    && prestamo.Incluir)
                {
                    /*Prestamos que estaban incluidos en una cancelacion pero se quitaron*/
                    if (prestamo.Incluir == true && prestamo.IncluirOriginal == false)
                    {
                        prestamo.Estado.IdEstado = (int)EstadosPrestamos.Confirmado;
                        prestamo.IdRefPrestamoCancelacion = 0;
                        prestamo.ImporteCancelacion = 0;
                        prestamo.ModificaImporteCancelacion = false;
                        if (!this.Modificar(prestamo, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(prestamo, pParametro);
                            return false;
                        }
                        foreach (PrePrestamosCuotas cuota in prestamo.PrestamosCuotas)
                        {
                            if (cuota.Estado.IdEstado == (int)EstadosCuotas.Activa
                                && cuota.IdRefTipoOperacionCobro == pParametro.IdPrestamo && cuota.IdTipoOperacionCobro == pParametro.TipoOperacion.IdTipoOperacion)
                            {
                                cuota.IdRefTipoOperacionCobro = null;
                                cuota.IdTipoOperacionCobro = null;
                                cuota.UsuarioLogueado = pParametro.UsuarioLogueado;
                                if (!BaseDatos.ObtenerBaseDatos().Actualizar(cuota, bd, tran, "PrePrestamosCuotasActualizar"))
                                {
                                    AyudaProgramacionLN.MapearError(cuota, prestamo);
                                    return false;
                                }
                            }
                        }
                    }
                    /*Prestamos que se van incluir en una cancelacion*/
                    else if (prestamo.Incluir == true && prestamo.IncluirOriginal == true)
                    {
                        prestamo.IdRefPrestamoCancelacion = pParametro.IdPrestamo;
                        prestamo.Estado.IdEstado = (int)EstadosPrestamos.RenovacionPendienteConfirmacion;
                        prestamo.FechaCancelacion = prestamo.FechaPrestamo;
                        if (!this.Modificar(prestamo, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(prestamo, pParametro);
                            return false;
                        }
                        foreach (PrePrestamosCuotas cuota in prestamo.PrestamosCuotas)
                        {
                            /*Cuotas del prestamo que se van a incluir en una cancelacion o que ya existian y permanecen*/
                            if (cuota.Estado.IdEstado == (int)EstadosCuotas.Activa
                                && cuota.Incluir) // && cuota.IncluirOriginal != false
                            {
                                cuota.IdRefTipoOperacionCobro = pParametro.IdPrestamo;
                                cuota.IdTipoOperacionCobro = pParametro.TipoOperacion.IdTipoOperacion;
                                cuota.UsuarioLogueado = pParametro.UsuarioLogueado;
                                if (!BaseDatos.ObtenerBaseDatos().Actualizar(cuota, bd, tran, "PrePrestamosCuotasActualizar"))
                                {
                                    AyudaProgramacionLN.MapearError(cuota, prestamo);
                                    return false;
                                }
                            }
                            /*Cuotas del prestamo que estaban en una cancelacion y se quitaron*/
                            else if (cuota.Estado.IdEstado == (int)EstadosCuotas.Activa
                                && cuota.IncluirOriginal == true
                                && cuota.IdRefTipoOperacionCobro == pParametro.IdPrestamo && cuota.IdTipoOperacionCobro == pParametro.TipoOperacion.IdTipoOperacion)
                            {
                                cuota.IdRefTipoOperacionCobro = null;
                                cuota.IdTipoOperacionCobro = null;
                                cuota.UsuarioLogueado = pParametro.UsuarioLogueado;
                                if (!BaseDatos.ObtenerBaseDatos().Actualizar(cuota, bd, tran, "PrePrestamosCuotasActualizar"))
                                {
                                    AyudaProgramacionLN.MapearError(cuota, prestamo);
                                    return false;
                                }
                            }
                        }
                    }
                }
                //else if
                //CONFIRMACION DE PRESTAMO Y CANCELACION DE HIJOS
                else if (pParametro.Estado.IdEstado == (int)EstadosPrestamos.Confirmado)
                {
                    prestamo.Estado.IdEstado = (int)EstadosPrestamos.Cancelado;
                    prestamo.FechaConfirmacionCancelacion = prestamo.FechaConfirmacion;
                    if (!this.Modificar(prestamo, bd, tran))
                    {
                        AyudaProgramacionLN.MapearError(prestamo, pParametro);
                        return false;
                    }
                }
                //ANULACION DE PRESTAMO Y REVERSION DE HIJOS
                else if (pParametro.Estado.IdEstado == (int)EstadosPrestamos.Anulado)
                {
                    prestamo.Estado.IdEstado = (int)EstadosPrestamos.Confirmado;
                    prestamo.IdRefPrestamoCancelacion = 0;
                    prestamo.ImporteCancelacion = 0;
                    prestamo.ModificaImporteCancelacion = false;
                    if (!this.Modificar(prestamo, bd, tran))
                    {
                        AyudaProgramacionLN.MapearError(prestamo, pParametro);
                        return false;
                    }
                    foreach (PrePrestamosCuotas cuota in prestamo.PrestamosCuotas)
                    {
                        if (cuota.Estado.IdEstado == (int)EstadosCuotas.Activa
                            && cuota.IdRefTipoOperacionCobro == pParametro.IdPrestamo && cuota.IdTipoOperacionCobro == pParametro.TipoOperacion.IdTipoOperacion)
                        {
                            cuota.IdRefTipoOperacionCobro = null;
                            cuota.IdTipoOperacionCobro = null;
                            cuota.UsuarioLogueado = pParametro.UsuarioLogueado;
                            if (!BaseDatos.ObtenerBaseDatos().Actualizar(cuota, bd, tran, "PrePrestamosCuotasActualizar"))
                            {
                                AyudaProgramacionLN.MapearError(cuota, prestamo);
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }


        private bool CuotasActualizar(PrePrestamos pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            foreach (PrePrestamosCuotas Cuotas in pParametro.PrestamosCuotas)
            {
                Cuotas.UsuarioLogueado = pParametro.UsuarioLogueado;
                switch (Cuotas.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        Cuotas.IdPrestamo = pParametro.IdPrestamo;
                        Cuotas.IdPrestamoCuota = BaseDatos.ObtenerBaseDatos().Agregar(Cuotas, bd, tran, "PrePrestamosCuotasInsertar");
                        if (Cuotas.IdPrestamoCuota == 0)
                            resultado = false;
                        break;
                    case EstadoColecciones.Modificado:
                        if (pParametro.Estado.IdEstado == (int)EstadosPrestamos.PendienteCancelacion
                            && Cuotas.Estado.IdEstado == (int)EstadosCuotas.Activa
                                && Cuotas.Incluir)
                        {
                            Cuotas.IdRefTipoOperacionCobro = pParametro.IdPrestamo;
                            Cuotas.IdTipoOperacionCobro = pParametro.TipoOperacion.IdTipoOperacion;
                        }
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(Cuotas, bd, tran, "PrePrestamosCuotasActualizar"))
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

        private bool CalcularGastosPrestamos(PrePrestamos pParametro)
        {
            //TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.PrestamosGastosAdministrativos);
            //decimal valor = 0;
            //if (!decimal.TryParse(paramValor.ParametroValor, out valor))
            //{
            //    pParametro.CodigoMensaje = "ValidarPorcentajeGastosAdministrativos";
            //    return false;
            //}
            pParametro.PorcentajeGastos = pParametro.PrestamoPlan.PorcentajeGasto;
            pParametro.ImporteGastos = Math.Round(pParametro.ObtenerImportePrestamo() * pParametro.PorcentajeGastos / 100, 2) + pParametro.PrestamoPlan.ImporteGasto;
            pParametro.ImportePrestamo = pParametro.ObtenerImportePrestamoGastos();
            return true;
        }

        private bool CalcularGastosAnticipos(PrePrestamos pParametro)
        {
            //TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.PrestamosGastosAdministrativos);
            //decimal valor = 0;
            //if (!decimal.TryParse(paramValor.ParametroValor, out valor))
            //{
            //    pParametro.CodigoMensaje = "ValidarPorcentajeGastosAdministrativos";
            //    return false;
            //}
            pParametro.PorcentajeGastos = pParametro.PrestamoPlan.PorcentajeGasto;
            pParametro.ImporteGastos = Math.Round(pParametro.ObtenerImportePrestamo() * pParametro.PorcentajeGastos / 100, 2) + pParametro.PrestamoPlan.ImporteGasto;
            //pParametro.SaldoDeuda = pParametro.SaldoDeuda + pParametro.ImporteGastos;
            return true;
        }

        /// <summary>
        /// Arma la Cuponera de un presamo
        /// </summary>
        /// <param name="pParametro"></param>
        public bool ArmarCuponera(PrePrestamos pParametro)
        {
            bool resultado = false;
            try
            {
                TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.WSArbaConsultaPadron);
                bool bvalor = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;
                if ( bvalor)
                {
                    ConsultarPadronLN padronLN = new ConsultarPadronLN();
                    ConsultarPadronEntidad entidad = new ConsultarPadronEntidad();
                    if (pParametro.Afiliado.TipoDocumento.IdTipoDocumento == (int)EnumTiposDocumentos.CUIT
                        || pParametro.Afiliado.TipoDocumento.IdTipoDocumento == (int)EnumTiposDocumentos.CUIT)
                       entidad.NumeroCUIT = pParametro.Afiliado.NumeroDocumento;
                    else
                        entidad.NumeroCUIT = pParametro.Afiliado.CUIL;

                    entidad.Fecha = pParametro.FechaPrestamo;
                    if (!padronLN.ConsultarPadron(entidad))
                    {
                        if (entidad.CodigoMensaje == "ErrorConexionARBA")
                        {
                            ConfirmarMensajes msg = new ConfirmarMensajes();
                            msg.CodigoMensaje = "ErrorConexionARBAPrestamos";
                            msg.Mensaje = entidad.Mensaje;
                            msg.TipoMensaje = TipoError.Alerta;
                            pParametro.ConfirmaMensajes.Add(msg);
                        }
                        else
                        {
                            AyudaProgramacionLN.MapearError(entidad, pParametro);
                            return false;
                        }
                    }
                }

                //pParametro.Afiliado.CUIL
                //pParametro.FechaPrestamo
                DataSet ds = BaseDatos.ObtenerBaseDatos().ObtenerDataSet("PrePrestamosArmarCuponera", pParametro);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Mapeador.SetearEntidadPorFila(ds.Tables[0].Rows[0], pParametro);
                        resultado = true;
                    }
                    else
                    {
                        pParametro.CodigoMensaje = "No se encontraron datos para armar el prestamo";
                    }
                }
                if (ds.Tables.Count > 1)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        PrePrestamosCuotas pc;
                        foreach (DataRow dr in ds.Tables[1].Rows)
                        {
                            pc = new PrePrestamosCuotas();
                            pc.Estado.IdEstado = (int)EstadosCuotas.Activa;
                            if (!pParametro.PrestamosCuotas.Exists(x => x.CuotaNumero == (decimal)dr["CuotaNumero"]))
                            {
                                pc.EstadoColeccion = EstadoColecciones.Agregado;
                                pParametro.PrestamosCuotas.Add(pc);
                                pc.IndiceColeccion = pParametro.PrestamosCuotas.IndexOf(pc);
                            }
                            else
                            {
                                //pc = pParametro.PrestamosCuotas.Find(x => x.CuotaNumero == pc.CuotaNumero);
                                pc = pParametro.PrestamosCuotas.Find(x => x.CuotaNumero == (decimal)dr["CuotaNumero"]);
                                if (pc.EstadoColeccion != EstadoColecciones.Agregado)
                                    pc.EstadoColeccion = EstadoColecciones.Modificado;
                            }
                            Mapeador.SetearEntidadPorfila(dr, pc, pc.EstadoColeccion);
                        }
                        int cantCuotas = ds.Tables[1].Rows.Count;
                        pParametro.PrestamosCuotas.Where(x => x.CuotaNumero > cantCuotas && x.IdPrestamoCuota > 0).ToList().ForEach(x => { x.Estado.IdEstado = (int)EstadosCuotas.Baja; x.EstadoColeccion = EstadoColecciones.Modificado; });
                        pParametro.PrestamosCuotas.RemoveAll(x => x.CuotaNumero > cantCuotas && x.IdPrestamoCuota == 0);
                    }
                }
                if (ds.Tables.Count > 2)
                {
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        TGECampos campo;
                        foreach (DataRow dr in ds.Tables[2].Rows)
                        {
                            if (pParametro.Campos.Exists(x => x.IdCampo == (int)dr["IdCampo"]))
                            {
                                campo = pParametro.Campos.Find(x => x.IdCampo == (int)dr["IdCampo"]);
                                Mapeador.SetearEntidadPorfila(dr, campo, campo.EstadoColeccion);
                            }
                        }
                    }
                }
                if (ds.Tables.Count > 3)
                {
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        PrePrestamosCheques che;
                        foreach (DataRow dr in ds.Tables[3].Rows)
                        {
                            if (pParametro.PrestamosCheques.Exists(x => x.NumeroCheque == dr["NumeroCheque"].ToString()
                                         && x.IdBanco == (int)dr["IdBanco"]))
                            {
                                che = pParametro.PrestamosCheques.Find(x => x.NumeroCheque == dr["NumeroCheque"].ToString()
                                         && x.IdBanco == (int)dr["IdBanco"]);
                                Mapeador.SetearEntidadPorfila(dr, che, che.EstadoColeccion);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                pParametro.CodigoMensaje = ex.Message;
            }
            return resultado;
        }

        public bool ArmarCuponera_BACK(PrePrestamos pParametro)
        {
            int diasAnio = 365;
            //TABLA PARAMETRO
            int diasAdicionalSubPeriodico = 0;
            int diaSolicitud = 0;
            DateTime fechaVtoCuota = pParametro.FechaPrestamo;

            switch (pParametro.Estado.IdEstado)
            {
                case (int)EstadosPrestamos.Activo:
                case (int)EstadosPrestamos.PreAutorizado:
                    diaSolicitud = pParametro.FechaPrestamo.Day;
                    fechaVtoCuota = pParametro.FechaPrestamo;
                    break;
                case (int)EstadosPrestamos.Autorizado:
                    diaSolicitud = pParametro.FechaValidezAutorizado.Day;
                    fechaVtoCuota = pParametro.FechaValidezAutorizado;
                    break;
                case (int)EstadosPrestamos.Confirmado:
                    diaSolicitud = pParametro.FechaConfirmacion.Day;
                    fechaVtoCuota = pParametro.FechaConfirmacion;
                    break;
                default:
                    break;
            }

            //if (AyudaProgramacionLN.ObtenerPeriodo(pParametro.FechaConfirmacion)<pParametro.PeriodoPrimerVencimiento)
            fechaVtoCuota = new DateTime(Convert.ToInt32(pParametro.PeriodoPrimerVencimiento.ToString().Substring(0, 4)), Convert.ToInt32(pParametro.PeriodoPrimerVencimiento.ToString().Substring(4, 2)), 1);

            fechaVtoCuota = this.CalcularFechaUltimoDiaMes(fechaVtoCuota);
            //fechaVtoCuota = this.CalcularFechaVencimientoProcesoCargos(fechaVtoCuota);

            int diasDelMes = DateTime.DaysInMonth(fechaVtoCuota.Year, fechaVtoCuota.Month);
            int diasSubPeriodicos = diasDelMes - diaSolicitud + diasAdicionalSubPeriodico;

            decimal saldoCapital = 0;
            double tasaAnual = Convert.ToDouble(pParametro.PrestamoPlan.PrestamoPlanTasa.Tasa / 100);
            double tasaMensual = tasaAnual / 12;
            int cantidadCuotas = pParametro.CantidadCuotas;
            decimal cuotaFija = 0;
            decimal tasaInteresDiaria = 0;
            PrePrestamosCuotas cuota;
            DateTime fechaInicio;

            TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.PrestamosGastosComoCargo);
            bool gastosComoCargo = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;

            switch (pParametro.TipoOperacion.IdTipoOperacion)
            {
                case (int)EnumTGETiposOperaciones.PrestamosCortoPlazo:
                    #region Adelantos

                    if (pParametro.CantidadCuotas != 1)
                    {
                        pParametro.CodigoMensaje = "ValidarCantidadCuotasPrestamosCortoPlazo";
                        return false;
                    }

                    //El importe Gasto se calcula despues en Anticipos
                    pParametro.ImportePrestamo = pParametro.ObtenerImportePrestamo();

                    saldoCapital = pParametro.ImportePrestamo;
                    diasAnio = 360;
                    tasaInteresDiaria = Convert.ToDecimal(tasaAnual) / diasAnio;
                    fechaInicio = pParametro.FechaPrestamo; //fechaVtoCuota;

                    if (fechaInicio.Date > fechaVtoCuota.Date)
                    {
                        pParametro.CodigoMensaje = "ValidarFechaPrestamoFechaVtoCuotaPrestamosCortoPlazo";
                        return false;
                    }

                    TimeSpan ts = fechaVtoCuota - fechaInicio;
                    int diasAnticipo = ts.Days;

                    decimal interes = Math.Round(saldoCapital * tasaInteresDiaria * diasAnticipo, 2);
                    pParametro.SaldoDeuda = saldoCapital + interes;

                    //pParametro.ImporteCuota = cuotaFija;
                    if (!this.CalcularGastosAnticipos(pParametro))
                        return false;
                    pParametro.SaldoDeuda = pParametro.SaldoDeuda + pParametro.ImporteGastos;
                    pParametro.ImportePrestamo = pParametro.ObtenerImportePrestamoGastos();
                    cuotaFija = pParametro.SaldoDeuda;
                    pParametro.ImporteCuota = cuotaFija;
                    for (int i = 1; i <= pParametro.CantidadCuotas; i++)
                    {
                        if (pParametro.PrestamosCuotas.Exists(x => x.CuotaNumero == i))
                        {
                            cuota = pParametro.PrestamosCuotas.Find(x => x.CuotaNumero == i);
                            cuota.EstadoColeccion = cuota.EstadoColeccion == EstadoColecciones.Agregado ? EstadoColecciones.Agregado : EstadoColecciones.Modificado;
                        }
                        else
                        {
                            cuota = new PrePrestamosCuotas();
                            cuota.CuotaNumero = i;
                            cuota.Estado.IdEstado = (int)EstadosCuotas.Activa;
                            cuota.EstadoColeccion = EstadoColecciones.Agregado;
                            pParametro.PrestamosCuotas.Add(cuota);
                        }

                        cuota.ImporteInteres = interes; // +pParametro.ImporteGastos;
                        cuota.ImporteAmortizacion = cuotaFija - cuota.ImporteInteres;
                        cuota.ImporteCuota = cuotaFija;
                        cuota.ImporteSaldo = 0;
                        cuota.CuotaFechaVencimiento = fechaVtoCuota;
                        saldoCapital = saldoCapital - cuota.ImporteAmortizacion;

                        fechaVtoCuota = this.CalcularFechaUltimoDiaMes(fechaVtoCuota.AddMonths(1));
                    }

                    #endregion
                    break;
                case (int)EnumTGETiposOperaciones.PrestamosFondosPropios:
                    #region Fondos Propios

                    //GASTOS ADMINISTRATIVOS
                    pParametro.ImporteGastos = Math.Round(pParametro.ObtenerImportePrestamo() * pParametro.PrestamoPlan.PorcentajeGasto / 100, 2)
                        + pParametro.PrestamoPlan.PrestamoPlanTasa.TasaGastoAdministrativo;

                    if (!gastosComoCargo)
                    {
                        pParametro.ImportePrestamo = pParametro.ObtenerImportePrestamoGastos();
                        saldoCapital = pParametro.ObtenerImportePrestamo() + pParametro.ImporteGastos;
                    }
                    else
                    {
                        pParametro.ImportePrestamo = pParametro.ObtenerImportePrestamo();
                        saldoCapital = pParametro.ObtenerImportePrestamo();
                    }
                    fechaInicio = fechaVtoCuota;

                    decimal capitalSocial = 0;
                    if (pParametro.PorcentajeCapitalSocial.HasValue)
                    {
                        capitalSocial = Math.Round(saldoCapital * pParametro.PorcentajeCapitalSocial.Value / 100, 2);
                        pParametro.ImporteCapitalSocial = capitalSocial > 0 ? capitalSocial : default(decimal?);
                    }
                    decimal interesfp = Math.Round(saldoCapital * Convert.ToDecimal(tasaMensual) * cantidadCuotas, 2);
                    //saldoCapital += pParametro.ImporteGastos; /* COMENTADO EL 15/04/2020*/
                    saldoCapital += capitalSocial;
                    pParametro.SaldoDeuda = saldoCapital + interesfp;
                    //pParametro.ImporteCuota = cuotaFija;

                    //pParametro.ImportePrestamo = pParametro.ObtenerImportePrestamoGastos();
                    cuotaFija = Math.Round(pParametro.SaldoDeuda / pParametro.CantidadCuotas, 2);
                    pParametro.ImporteCuota = cuotaFija;
                    for (int i = 1; i <= pParametro.CantidadCuotas; i++)
                    {
                        if (pParametro.PrestamosCuotas.Exists(x => x.CuotaNumero == i))
                        {
                            cuota = pParametro.PrestamosCuotas.Find(x => x.CuotaNumero == i);
                            cuota.Estado.IdEstado = cuota.Estado.IdEstado == (int)EstadosCuotas.Baja ? (int)EstadosCuotas.Activa : cuota.Estado.IdEstado;
                            cuota.EstadoColeccion = cuota.EstadoColeccion == EstadoColecciones.Agregado ? EstadoColecciones.Agregado : cuota.EstadoColeccion == EstadoColecciones.AgregadoBorradoMemoria ? EstadoColecciones.Agregado : EstadoColecciones.Modificado;
                        }
                        else
                        {
                            cuota = new PrePrestamosCuotas();
                            cuota.CuotaNumero = i;
                            cuota.Estado.IdEstado = (int)EstadosCuotas.Activa;
                            cuota.EstadoColeccion = EstadoColecciones.Agregado;
                            pParametro.PrestamosCuotas.Add(cuota);
                        }

                        if (i != cantidadCuotas)
                        {
                            cuota.ImporteInteres = Math.Round(interesfp / pParametro.CantidadCuotas, 2);
                            cuota.ImporteAmortizacion = cuotaFija - cuota.ImporteInteres;
                            cuota.ImporteCapitalSocial = Math.Round(capitalSocial / pParametro.CantidadCuotas, 2);
                            cuota.ImporteNetoAmortizacion = cuota.ImporteAmortizacion - cuota.ImporteCapitalSocial;
                            cuota.ImporteCuota = cuotaFija;
                            cuota.ImporteSaldo = saldoCapital - cuota.ImporteAmortizacion;
                            saldoCapital = saldoCapital - cuota.ImporteAmortizacion;
                        }
                        else
                        {
                            cuota.ImporteInteres = Math.Round(interesfp / pParametro.CantidadCuotas, 2);
                            cuota.ImporteAmortizacion = cuotaFija - cuota.ImporteInteres;
                            cuota.ImporteCuota = cuotaFija;
                            cuota.ImporteSaldo = 0;
                            saldoCapital = saldoCapital - cuota.ImporteAmortizacion;
                            //decimal amortizacionTotal = pParametro.PrestamosCuotas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Activa).Sum(x => x.ImporteAmortizacion);
                            if (saldoCapital != 0)
                            {
                                cuota.ImporteAmortizacion = cuota.ImporteAmortizacion + saldoCapital;
                                cuota.ImporteInteres = cuota.ImporteInteres - saldoCapital;
                            }
                            cuota.ImporteCapitalSocial = capitalSocial - pParametro.PrestamosCuotas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Activa).Sum(x => x.ImporteCapitalSocial);
                            cuota.ImporteNetoAmortizacion = cuota.ImporteAmortizacion - cuota.ImporteCapitalSocial;

                        }
                        cuota.CuotaFechaVencimiento = fechaVtoCuota;
                        fechaVtoCuota = this.CalcularFechaUltimoDiaMes(fechaVtoCuota.AddMonths(1));
                    }

                    // Pongo de Baja las Cuotas Mayores a la Cantidad de Cuotas del Prestamo
                    // Cuando se modifica la Cantidad de Cuotas del Plan a un numero menor.
                    if (pParametro.CantidadCuotas < pParametro.PrestamosCuotas.Count)
                    {
                        for (int i = pParametro.CantidadCuotas + 1; i <= pParametro.PrestamosCuotas.Count; i++)
                        {
                            if (pParametro.PrestamosCuotas.Exists(x => x.CuotaNumero == i))
                            {
                                cuota = pParametro.PrestamosCuotas.Find(x => x.CuotaNumero == i);
                                cuota.EstadoColeccion = cuota.EstadoColeccion == EstadoColecciones.Agregado ? EstadoColecciones.AgregadoBorradoMemoria : cuota.EstadoColeccion == EstadoColecciones.AgregadoBorradoMemoria ? EstadoColecciones.AgregadoBorradoMemoria : EstadoColecciones.Modificado;
                                cuota.Estado.IdEstado = (int)EstadosCuotas.Baja;
                            }
                        }
                    }

                    pParametro.SaldoDeuda = pParametro.PrestamosCuotas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Activa).Sum(x => x.ImporteAmortizacion) + pParametro.PrestamosCuotas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Activa).Sum(x => x.ImporteInteres);

                    #endregion
                    break;
                case (int)EnumTGETiposOperaciones.PrestamosLargoPlazo:
                    #region Prestamos

                    TGEEstados estadoCuota = new TGEEstados();
                    estadoCuota.IdEstado = (int)EstadosCuotas.Activa;
                    estadoCuota = TGEGeneralesF.TGEEstadosObtener(estadoCuota);

                    pParametro.ImportePrestamo = pParametro.ObtenerImportePrestamo();

                    if (!this.CalcularGastosPrestamos(pParametro))
                        return false;

                    tasaInteresDiaria = Convert.ToDecimal(tasaAnual) / diasAnio;
                    //pParametro.SubPeriodico = Math.Round(pParametro.ImportePrestamo * tasaInteresDiaria * diasSubPeriodicos, 2);
                    saldoCapital = pParametro.ObtenerImportePrestamoGastos(); //+ pParametro.ImporteGastos; //+ pParametro.ImporteExcedido;

                    //Metodo Frances Cuota Fija
                    if (tasaMensual > 0)
                    {
                        double A = tasaMensual * (Math.Pow(1 + tasaMensual, cantidadCuotas));
                        double B = (Math.Pow(1 + tasaMensual, cantidadCuotas) - 1);
                        cuotaFija = saldoCapital * Convert.ToDecimal(A / B);
                    }
                    else
                    {
                        cuotaFija = saldoCapital / cantidadCuotas;
                    }
                    cuotaFija = Math.Round(cuotaFija, 2);
                    saldoCapital = Math.Round(saldoCapital, 2);

                    pParametro.ImporteCuota = cuotaFija;
                    //fechaVtoCuota = this.CalcularFechaUltimoDiaMes(fechaVtoCuota.AddMonths(1));
                    //Agregado por el corte de Facturacion los dís 25 de cada mes
                    //fechaVtoCuota = this.CalcularFechaVencimientoProcesoCargos(fechaVtoCuota);

                    decimal importeAmortizacion = 0;

                    for (int i = 1; i <= pParametro.CantidadCuotas; i++)
                    {
                        if (pParametro.PrestamosCuotas.Exists(x => x.CuotaNumero == i))
                        {
                            cuota = pParametro.PrestamosCuotas.Find(x => x.CuotaNumero == i);
                            cuota.Estado.IdEstado = cuota.Estado.IdEstado == (int)EstadosCuotas.Baja ? (int)EstadosCuotas.Activa : cuota.Estado.IdEstado;
                            cuota.EstadoColeccion = cuota.EstadoColeccion == EstadoColecciones.Agregado ? EstadoColecciones.Agregado : cuota.EstadoColeccion == EstadoColecciones.AgregadoBorradoMemoria ? EstadoColecciones.Agregado : EstadoColecciones.Modificado;
                        }
                        else
                        {
                            cuota = new PrePrestamosCuotas();
                            cuota.CuotaNumero = i;
                            cuota.Estado = estadoCuota;
                            cuota.EstadoColeccion = EstadoColecciones.Agregado;
                            pParametro.PrestamosCuotas.Add(cuota);
                        }

                        if (i != cantidadCuotas)
                        {
                            cuota.ImporteInteres = Math.Round(saldoCapital * Convert.ToDecimal(tasaMensual), 2);
                            cuota.ImporteAmortizacion = cuotaFija - cuota.ImporteInteres;
                            importeAmortizacion += cuota.ImporteAmortizacion;
                            cuota.ImporteSaldo = saldoCapital - cuota.ImporteAmortizacion;
                        }
                        else
                        {
                            cuota.ImporteAmortizacion = pParametro.ImportePrestamo - importeAmortizacion;
                            cuota.ImporteInteres = cuotaFija - cuota.ImporteAmortizacion;
                            if (cuota.ImporteInteres < 0)
                            {
                                cuota.ImporteInteres = 0;
                            }
                            cuota.ImporteSaldo = 0;
                        }
                        cuota.ImporteCuota = cuotaFija;
                        cuota.CuotaFechaVencimiento = fechaVtoCuota;
                        saldoCapital = saldoCapital - cuota.ImporteAmortizacion;

                        fechaVtoCuota = this.CalcularFechaUltimoDiaMes(fechaVtoCuota.AddMonths(1));
                    }

                    // Pongo de Baja las Cuotas Mayores a la Cantidad de Cuotas del Prestamo
                    // Cuando se modifica la Cantidad de Cuotas del Plan a un numero menor.
                    if (pParametro.CantidadCuotas < pParametro.PrestamosCuotas.Count)
                    {
                        for (int i = pParametro.CantidadCuotas + 1; i <= pParametro.PrestamosCuotas.Count; i++)
                        {
                            if (pParametro.PrestamosCuotas.Exists(x => x.CuotaNumero == i))
                            {
                                cuota = pParametro.PrestamosCuotas.Find(x => x.CuotaNumero == i);
                                cuota.EstadoColeccion = cuota.EstadoColeccion == EstadoColecciones.Agregado ? EstadoColecciones.AgregadoBorradoMemoria : cuota.EstadoColeccion == EstadoColecciones.AgregadoBorradoMemoria ? EstadoColecciones.AgregadoBorradoMemoria : EstadoColecciones.Modificado;
                                cuota.Estado.IdEstado = (int)EstadosCuotas.Baja;
                            }
                        }
                    }


                    pParametro.SaldoDeuda = pParametro.PrestamosCuotas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Activa).Sum(x => x.ImporteAmortizacion) + pParametro.PrestamosCuotas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Activa).Sum(x => x.ImporteInteres);
                    #endregion
                    break;
                case (int)EnumTGETiposOperaciones.PrestamosBancoDelSol:
                    #region Prestamos Banco del Sol

                    pParametro.ImportePrestamo = pParametro.ImporteAutorizado > 0 ? pParametro.ImporteAutorizado : pParametro.ImporteSolicitado;

                    PrePrestamosBancoSolParametros bancoSol = BaseDatos.ObtenerBaseDatos().Obtener<PrePrestamosBancoSolParametros>("PrePrestamosSeleccionarCuotasBancoSol", pParametro);

                    if (bancoSol.IdPrestamoBancoSolParametro == 0)
                    {
                        pParametro.CodigoMensaje = "ValidarPlanBancoSol";
                        pParametro.CodigoMensajeArgs.Add(pParametro.PrestamoPlan.IdPrestamoPlan.ToString());
                        pParametro.CodigoMensajeArgs.Add(pParametro.ImportePrestamo.ToString("C2"));
                        return false;
                    }

                    pParametro.ImportePrestamo = bancoSol.Capital.Value;// bancoSol.Monto.Value;
                    pParametro.ImporteGastos = bancoSol.TasaAdm.Value + bancoSol.Sellado.Value;//bancoSol.ImporteSeguro.Value + bancoSol.TasaAdm.Value + bancoSol.Sellado.Value;
                    pParametro.ImporteSeguro = bancoSol.ImporteSeguro;
                    pParametro.ImporteSellado = bancoSol.Sellado;
                    pParametro.ImporteTasaAdministrativa = bancoSol.TasaAdm;
                    pParametro.ImporteInteres = bancoSol.Monto.Value - bancoSol.Capital.Value;

                    pParametro.SaldoCapital = bancoSol.Capital.Value;
                    saldoCapital = pParametro.SaldoCapital;
                    pParametro.ImporteCuota = bancoSol.TotalCuota.Value;
                    cuotaFija = pParametro.ImporteCuota;
                    pParametro.CantidadCuotas = bancoSol.CantidadCuotas;

                    //fechaVtoCuota = this.CalcularFechaUltimoDiaMes(fechaVtoCuota.AddMonths(1));
                    //Agregado por el corte de Facturacion los dís 25 de cada mes
                    //fechaVtoCuota = this.CalcularFechaVencimientoProcesoCargos(fechaVtoCuota);

                    decimal amortizacionCuotas = 0;
                    for (int i = 1; i <= pParametro.CantidadCuotas; i++)
                    {
                        if (pParametro.PrestamosCuotas.Exists(x => x.CuotaNumero == i))
                        {
                            cuota = pParametro.PrestamosCuotas.Find(x => x.CuotaNumero == i);
                            cuota.Estado.IdEstado = cuota.Estado.IdEstado == (int)EstadosCuotas.Baja ? (int)EstadosCuotas.Activa : cuota.Estado.IdEstado;
                            cuota.EstadoColeccion = cuota.EstadoColeccion == EstadoColecciones.Agregado ? EstadoColecciones.Agregado : cuota.EstadoColeccion == EstadoColecciones.AgregadoBorradoMemoria ? EstadoColecciones.Agregado : EstadoColecciones.Modificado;
                        }
                        else
                        {
                            cuota = new PrePrestamosCuotas();
                            cuota.CuotaNumero = i;
                            cuota.Estado.IdEstado = (int)EstadosCuotas.Activa;
                            cuota.EstadoColeccion = EstadoColecciones.Agregado;
                            pParametro.PrestamosCuotas.Add(cuota);
                        }

                        if (i != cantidadCuotas)
                        {
                            cuota.ImporteInteres = Math.Round(pParametro.ImporteInteres / cantidadCuotas, 2); // bancoSol.ImporteCuota.Value;
                            cuota.ImporteAmortizacion = Math.Round(pParametro.SaldoCapital / cantidadCuotas, 2); // bancoSol.ImporteCuota.Value;
                            cuota.ImporteSaldo = saldoCapital - cuota.ImporteAmortizacion;
                        }
                        else
                        {
                            cuota.ImporteInteres = Math.Round(pParametro.ImporteInteres / cantidadCuotas, 2);
                            cuota.ImporteAmortizacion = Math.Round(pParametro.SaldoCapital / cantidadCuotas, 2);
                            cuota.ImporteSaldo = 0;

                            amortizacionCuotas = pParametro.PrestamosCuotas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Activa).Sum(x => x.ImporteAmortizacion);
                            if (pParametro.ImportePrestamo != amortizacionCuotas)
                            {
                                cuota.ImporteInteres = cuota.ImporteInteres - (pParametro.ImportePrestamo - amortizacionCuotas);
                                cuota.ImporteAmortizacion = cuota.ImporteAmortizacion + (pParametro.ImportePrestamo - amortizacionCuotas);
                            }

                        }
                        cuota.ImporteCuota = cuotaFija;
                        cuota.CuotaFechaVencimiento = fechaVtoCuota;
                        saldoCapital = saldoCapital - cuota.ImporteAmortizacion;

                        fechaVtoCuota = this.CalcularFechaUltimoDiaMes(fechaVtoCuota.AddMonths(1));
                    }

                    // Pongo de Baja las Cuotas Mayores a la Cantidad de Cuotas del Prestamo
                    // Cuando se modifica la Cantidad de Cuotas del Plan a un numero menor.
                    if (pParametro.CantidadCuotas < pParametro.PrestamosCuotas.Count)
                    {
                        for (int i = pParametro.CantidadCuotas + 1; i <= pParametro.PrestamosCuotas.Count; i++)
                        {
                            if (pParametro.PrestamosCuotas.Exists(x => x.CuotaNumero == i))
                            {
                                cuota = pParametro.PrestamosCuotas.Find(x => x.CuotaNumero == i);
                                cuota.EstadoColeccion = cuota.EstadoColeccion == EstadoColecciones.Agregado ? EstadoColecciones.AgregadoBorradoMemoria : cuota.EstadoColeccion == EstadoColecciones.AgregadoBorradoMemoria ? EstadoColecciones.AgregadoBorradoMemoria : EstadoColecciones.Modificado;
                                cuota.Estado.IdEstado = (int)EstadosCuotas.Baja;
                            }
                        }
                    }


                    pParametro.SaldoDeuda = pParametro.PrestamosCuotas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Activa).Sum(x => x.ImporteAmortizacion) + pParametro.PrestamosCuotas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Activa).Sum(x => x.ImporteInteres);
                    #endregion
                    break;
                default:
                    break;
            }

            pParametro.ImporteInteres = pParametro.PrestamosCuotas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Activa).Sum(x => x.ImporteInteres);
            return true;
        }

        /// <summary>
        /// Devuelve la Fecha al ultimo dia del mes
        /// </summary>
        /// <param name="pFecha"></param>
        /// <returns></returns>
        private DateTime CalcularFechaUltimoDiaMes(DateTime pFecha)
        {
            int dias = DateTime.DaysInMonth(pFecha.Year, pFecha.Month);
            pFecha = pFecha.AddDays(-pFecha.Day + dias);
            return pFecha;
        }

        /// <summary>
        /// Si los cargos estan generados para el periodo de la fecha, le suma un mes
        /// </summary>
        /// <param name="pFecha"></param>
        /// <returns></returns>
        private DateTime CalcularFechaVencimientoProcesoCargos(DateTime pFecha)
        {
            int periodo = Convert.ToInt32(string.Concat(pFecha.Year.ToString(), pFecha.Month.ToString().PadLeft(2, '0')));
            if (ProcesosDatosF.ProcesosProcesamientoValidarCargosGenerados(periodo))
            {
                pFecha = pFecha.AddMonths(1);
                pFecha = this.CalcularFechaUltimoDiaMes(pFecha);
            }
            return pFecha;
        }

        /// <summary>
        /// Marca el Estado de la Cuota con el Esado del Prestamo
        /// </summary>
        /// <param name="pParametro"></param>
        private void MarcarCuponera(PrePrestamos pParametro)
        {
            foreach (PrePrestamosCuotas cuota in pParametro.PrestamosCuotas)
            {
                if (cuota.Estado.IdEstado == (int)EstadosCuotas.Activa)
                {
                    cuota.Estado.IdEstado = pParametro.Estado.IdEstado;
                    cuota.EstadoColeccion = EstadoColecciones.Modificado;
                }
                else if (cuota.Estado.IdEstado == (int)EstadosCuotas.PendienteCancelacion
                    && pParametro.Estado.IdEstado == (int)EstadosPrestamos.Confirmado)
                {
                    cuota.Estado.IdEstado = (int)EstadosCuotas.Activa;
                    cuota.EstadoColeccion = EstadoColecciones.Modificado;
                }
            }
        }

        public void CalcularImporteCancelar(PrePrestamos pParametro)
        {
            this.CalcularImporteCancelar(pParametro, false);
        }
        /// <summary>
        /// Calcula el ImporteCancelacion de un Prestamo
        /// </summary>
        /// <param name="pParametro"></param>
        public void CalcularImporteCancelar(PrePrestamos pParametro, bool pValidarIncluir)
        {
            decimal tmpImporteCancelacion = pParametro.ImporteCancelacion;

            if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosCortoPlazo
                || pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosCortoPlazoCancelacion)
            {
                TimeSpan ts = pParametro.FechaCancelacion.Date - pParametro.FechaConfirmacion.Date;
                int diasAnticipoCancela = ts.Days;

                ts = pParametro.PrestamosCuotas[0].CuotaFechaVencimiento.Date - pParametro.FechaConfirmacion.Date;
                int diasAnticipo = ts.Days;

                //decimal interesNuevo = pParametro.ImporteInteres * diasAnticipoCancela / diasAnticipo;
                pParametro.ComisionCancelacion = Math.Round(pParametro.ImporteInteres * diasAnticipoCancela / diasAnticipo, 2);
                pParametro.Bonificacion = pParametro.ImporteInteres - pParametro.ComisionCancelacion;
                pParametro.ImporteCancelacion = pParametro.SaldoDeuda - pParametro.Bonificacion;
            }
            else
            {
                List<PrePrestamosCuotas> cuotasPendientes = pParametro.ObtenerCuotasPendientes(pValidarIncluir).Where(x => !x.IdCuentaCorriente.HasValue).ToList();
                pParametro.AmortizacionCuotasNoCobradas = cuotasPendientes.Sum(x => x.ImporteAmortizacion);
                pParametro.InteresesNoDevengados = cuotasPendientes.Sum(x => x.ImporteInteres);

                List<PrePrestamosCuotas> cuotasFacturadas = pParametro.ObtenerCuotasPendientes(pValidarIncluir).Where(x => x.IdCuentaCorriente.HasValue).ToList();
                //pParametro.ComisionCancelacion = Math.Round(pParametro.AmortizacionCuotasNoCobradas * pParametro.PorcentajeGastos / 100, 2);
                //pParametro.Bonificacion = pParametro.InteresesNoDevengados - pParametro.ComisionCancelacion;
                //pParametro.ImporteCancelacion = pParametro.SaldoDeuda - pParametro.Bonificacion;
                pParametro.ImporteCancelacion = cuotasFacturadas.Sum(x => x.ImporteCuota - (x.ImporteCobrado.HasValue ? x.ImporteCobrado.Value : 0)) + pParametro.AmortizacionCuotasNoCobradas;
                //}
                //else
                //{
                //    pParametro.ImporteCancelacion = pParametro.SaldoDeuda;
                //}

                if (pParametro.ModificaImporteCancelacion)
                {
                    if (tmpImporteCancelacion < pParametro.ImporteCancelacion)
                    {
                        pParametro.Bonificacion = pParametro.ImporteCancelacion - tmpImporteCancelacion;
                        pParametro.ImporteCancelacion = tmpImporteCancelacion;
                    }
                    else
                    {
                        pParametro.ComisionCancelacion = tmpImporteCancelacion - pParametro.ImporteCancelacion;
                        pParametro.ImporteCancelacion = tmpImporteCancelacion;
                    }

                }
            }

            #region Backup Procesos
            //if (pParametro.PrestamosCuotas.Exists(x => x.Estado.IdEstado == (int)EstadosCuotas.Activa))
            //{
            //    //int idPrestamoCuota = pParametro.PrestamosCuotas.Where(x => x.Estado.IdEstado == (int)EstadosCuotas.Activa).Min(x => x.IdPrestamoCuota);
            //    //PrePrestamosCuotas cuotaVencer = pParametro.PrestamosCuotas.Find(x => x.IdPrestamoCuota == idPrestamoCuota);
            //    int diasMes = 30;
            //    int dias = 0;

            //    if (cuotaMes.CuotaNumero == 1)
            //    {
            //        dias = Convert.ToInt32((DateTime.Now - pParametro.FechaConfirmacion).TotalDays);
            //        decimal tasaInteresDiaria = Convert.ToDecimal(pParametro.PrestamoPlan.PrestamoPlanTasa.Tasa) / diasMes;
            //        interes = Math.Round(pParametro.ImporteAutorizado * tasaInteresDiaria, 2);
            //    }
            //    else
            //    {
            //        dias = Convert.ToInt32((cuotaVencer.CuotaFechaVencimiento - DateTime.Now).TotalDays);
            //        interes = cuotaVencer.ImporteInteres * dias / diasMes;
            //    }                
            //}
            //pParametro.ImporteCancelacion += interes;



            //DateTime fechaUltimoDia = this.CalcularFechaUltimoDiaMes(DateTime.Now);
            //if (pParametro.PrestamosCuotas.Exists(x => x.CuotaFechaVencimiento.Date == fechaUltimoDia.Date))
            //{
            //    PrePrestamosCuotas cuotaMes = pParametro.PrestamosCuotas.Find(x => x.CuotaFechaVencimiento.Date == fechaUltimoDia.Date);
            //    int dias = Convert.ToInt32((cuotaMes.CuotaFechaVencimiento - DateTime.Now).TotalDays);
            //    interes = Math.Round(cuotaMes.ImporteInteres * dias / diasMes, 2);
            //}
            //pParametro.ImporteCancelacion -= interes;

            //DateTime fechaUltimoDia =this.CalcularFechaUltimoDiaMes(DateTime.Now);
            //if (pParametro.PrestamosCuotas.Exists(x => x.CuotaFechaVencimiento.Date == fechaUltimoDia.Date))
            //{
            //    PrePrestamosCuotas cuotaMes = pParametro.PrestamosCuotas.Find(x => x.CuotaFechaVencimiento.Date == fechaUltimoDia.Date);
            //    int dias = Convert.ToInt32((cuotaMes.CuotaFechaVencimiento - DateTime.Now).TotalDays);
            //    interes = cuotaMes.ImporteCuota * pParametro.PrestamoPlan.PrestamoPlanTasa.Tasa * dias;
            //    interes = interes / (1 + pParametro.PrestamoPlan.PrestamoPlanTasa.Tasa * dias);
            //    pParametro.ImporteCancelacion -= interes;
            //}            
            #endregion

            if (pParametro.ImporteCancelacion < 0)
                pParametro.ImporteCancelacion = 0;

        }

        /// <summary>
        /// Validaciones para el Alta de un Prestamo
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        private bool ValidarAgregar(PrePrestamos pParametro)
        {
            //if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.Adelantos)
            //{
            //    if (pParametro.ImporteExcedido > 0 && pParametro.ConfirmarExcedido == false)
            //    {
            //        pParametro.ConfirmarAccion = true;
            //        pParametro.CodigoMensaje = "ConfirmarImporteExcedido";
            //        pParametro.CodigoMensajeArgs.Add(pParametro.ImporteExcedido.ToString("C2"));
            //        return false;
            //    }
            //}

            if (pParametro.FechaPrestamo.Date > DateTime.Now.Date)
            {
                pParametro.CodigoMensaje = "ValidarFechaPrestamo";
                return false;
            }

            //VALIDACIONES PARA PRESTAMOS
            //if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosLargoPlazo)
            //{
            //    //Valido que no tenga un Prestamo en tramite
            //    List<PrePrestamos> listaValidar = this.ObtenerPorAfiliado(pParametro.Afiliado);
            //    if (listaValidar.Exists(x => x.IdPrestamo != pParametro.IdPrestamo &&
            //                    (
            //                        x.Estado.IdEstado == (int)EstadosPrestamos.Activo
            //                        || x.Estado.IdEstado == (int)EstadosPrestamos.Autorizado
            //                        || x.Estado.IdEstado == (int)EstadosPrestamos.PendienteCancelacion
            //                        || x.Estado.IdEstado == (int)EstadosPrestamos.PreAutorizado
            //                    )
            //                )
            //        )
            //    {
            //        pParametro.CodigoMensaje = "ValidarCantidad";
            //        //pParametro.CodigoMensajeArgs.Add(pParametro.PrestamoPlan.PrestamoPlanTasa.CantidadCuotas.ToString());
            //        return false;
            //    }
            //}

            //VALIDACIONES PARA PRESTAMOS Y ANTICIPOS
            //Valido las cuotas Minima
            if (pParametro.CantidadCuotas < pParametro.PrestamoPlan.PrestamoPlanTasa.CantidadCuotas)
            {
                pParametro.CodigoMensaje = "PrestamosCuotasMinima";
                pParametro.CodigoMensajeArgs.Add(pParametro.PrestamoPlan.PrestamoPlanTasa.CantidadCuotas.ToString());
                return false;
            }

            //Valido las cuotas Maxima
            if (pParametro.CantidadCuotas > pParametro.PrestamoPlan.PrestamoPlanTasa.CantidadCuotasHasta)
            {
                pParametro.CodigoMensaje = "PrestamosCuotasMaxima";
                pParametro.CodigoMensajeArgs.Add(pParametro.PrestamoPlan.PrestamoPlanTasa.CantidadCuotasHasta.ToString());
                return false;
            }

            decimal importe = 0;
            if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.CompraDeCheque)
            {
                importe = pParametro.PrestamosCheques.Sum(x => x.Importe);
                if (importe <= 0)
                {
                    pParametro.CodigoMensaje = "ValidarImportePrestamoCheques";
                    return false;
                }
                if (pParametro.PrestamosCheques.Exists(x => string.IsNullOrEmpty(x.NumeroCheque)))
                {
                    pParametro.CodigoMensaje = "ValidarPrestamoChequesNumeros";
                    return false;
                }
                if (pParametro.PrestamosCheques.Exists(x => !x.FechaDiferido.HasValue))
                {
                    pParametro.CodigoMensaje = "ValidarPrestamoChequesFechaDiferido";
                    return false;
                }
                if (pParametro.PrestamosCheques.Exists(x => !x.IdBanco.HasValue))
                {
                    pParametro.CodigoMensaje = "ValidarPrestamoChequesBanco";
                    return false;
                }
            }
            else
            {
                
                importe = pParametro.ObtenerImportePrestamo();
                if (importe <= 0)
                {
                    pParametro.CodigoMensaje = "ValidarImportePrestamo";
                    return false;
                }
            }

            if (pParametro.PrestamoPlan.PrestamoPlanTasa.ImporteDesde.HasValue
                && pParametro.PrestamoPlan.PrestamoPlanTasa.ImporteDesde > importe)
            {
                pParametro.CodigoMensaje = "PrestamosImporteDesde";
                pParametro.CodigoMensajeArgs.Add(pParametro.PrestamoPlan.PrestamoPlanTasa.ImporteDesde.Value.ToString("C2"));
                return false;
            }

            if (pParametro.PrestamoPlan.PrestamoPlanTasa.ImporteHasta.HasValue
                && pParametro.PrestamoPlan.PrestamoPlanTasa.ImporteHasta < importe)
            {
                pParametro.CodigoMensaje = "PrestamosImporteHasta";
                pParametro.CodigoMensajeArgs.Add(pParametro.PrestamoPlan.PrestamoPlanTasa.ImporteHasta.Value.ToString("C2"));
                return false;
            }

            if (pParametro.FormaCobroAfiliado.FormaCobro.IdFormaCobro == (int)EnumTGEFormasCobros.IPS
                && pParametro.EstadoColeccion == EstadoColecciones.Agregado)
            {
                PrePrestamosIpsCadAutorizaciones ipsCAD = new PrePrestamosIpsCadAutorizaciones();
                ipsCAD.IdPrestamoIpsCadAutorizacion = pParametro.IdPrestamoIpsCadAutorizacion;
                ipsCAD = new PrePrestamosIpsCadAutorizacionesLN().ObtenerDatosCompletos(ipsCAD);
                if (ipsCAD.Numero == 0)
                {
                    pParametro.CodigoMensaje = "ValidarIPSNumeroCADNoEncontro";
                    return false;
                }
                if (pParametro.ImporteCuota != ipsCAD.Importe)
                {
                    pParametro.CodigoMensaje = "ValidarIPSNumeroCADImporteCuota";
                    pParametro.CodigoMensajeArgs.Add(ipsCAD.Importe.ToString("C2"));
                    return false;
                }
                if (pParametro.CantidadCuotas != ipsCAD.CantidadCuotas)
                {
                    pParametro.CodigoMensaje = "ValidarIPSNumeroCADCantidadCuotas";
                    pParametro.CodigoMensajeArgs.Add(ipsCAD.CantidadCuotas.ToString());
                    return false;
                }
            }
            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "PrePrestamosValidacionesPrevio"))
            {
                return false;
            }

            return true;
        }

        private bool ActualizarCuentaCorriente(PrePrestamos pParametro, Database bd, DbTransaction tran)
        {
            BaseDatos datos = BaseDatos.ObtenerBaseDatos();
            Hashtable param;
            HistorialCambios cambio = new HistorialCambios();
            cambio.CampoCambiado = "Prestamo -> CuentaCorriente";

            foreach (CarCuentasCorrientes cargo in pParametro.CargosExcedidos)
            {
                param = new Hashtable();
                param.Add("IdPrestamo", pParametro.IdPrestamo);
                param.Add("IdCuentaCorriente", cargo.IdCuentaCorriente);
                param.Add("ImporteCobrado", cargo.Importe - cargo.ImporteCobrado);
                cambio.ValorViejo = string.Empty;
                cambio.ValorNuevo = string.Empty;

                switch (cargo.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        if (!datos.Actualizar(param, bd, tran, "PrePrestamosCuentasCorrientesInsertar"))
                            return false;
                        cambio.ValorNuevo = cargo.IdCuentaCorriente.ToString();
                        if (!AuditoriaF.AuditoriaAgregar(pParametro, cambio, Acciones.Insert, bd, tran))
                            return false;
                        break;
                    case EstadoColecciones.Borrado:
                        if (!datos.Actualizar(param, bd, tran, "PrePrestamosCuentasCorrientesBorrar"))
                            return false;
                        cambio.ValorViejo = cargo.IdCuentaCorriente.ToString();
                        if (!AuditoriaF.AuditoriaAgregar(pParametro, cambio, Acciones.Insert, bd, tran))
                            return false;
                        break;
                    default:
                        break;
                }
            }

            return true;
        }

        private bool ActualizarSolicitudesPagos(PrePrestamos pParametro, Database bd, DbTransaction tran)
        {
            //ACTUALIZO LAS SOLICITUDES DE PAGO
            Hashtable parametros;
            foreach (CapSolicitudPago sp in pParametro.SolicitudesPagos)
            {
                parametros = new Hashtable();
                parametros.Add("IdPrestamo", pParametro.IdPrestamo);
                parametros.Add("IdSolicitudPago", sp.IdSolicitudPago);
                if (!BaseDatos.ObtenerBaseDatos().Actualizar(parametros, bd, tran, "PrePrestamosSolicitudesPagosInsertar"))
                {
                    return false;
                }
            }

            return true;
        }

        private bool ActualizarCampoIPS(PrePrestamos pParametro, Database bd, DbTransaction tran)
        {
            if (pParametro.IdPrestamoIpsCadAutorizacion == 0)
                return true;

            PrePrestamosIpsCadAutorizacionesLN ipsCADLN = new PrePrestamosIpsCadAutorizacionesLN();
            PrePrestamosIpsCadAutorizaciones ipsCAD = new PrePrestamosIpsCadAutorizaciones();
            ipsCAD.IdPrestamoIpsCadAutorizacion = pParametro.IdPrestamoIpsCadAutorizacion;
            ipsCAD.SelloTiempo = pParametro.IpsCADSelloTiempo;
            bool actualizar = false;
            if (pParametro.EstadoColeccion == EstadoColecciones.Agregado)
            {
                ipsCAD.Estado.IdEstado = (int)EstadosIPSCAD.Tomado;
                ipsCAD.IdPrestamo = pParametro.IdPrestamo;
                actualizar = true;
            }
            else if (pParametro.EstadoColeccion == EstadoColecciones.Modificado)
            {
                if (pParametro.Estado.IdEstado == (int)EstadosPrestamos.Anulado)
                {
                    ipsCAD.Estado.IdEstado = (int)EstadosIPSCAD.Activo;
                    ipsCAD.IdPrestamo = default(int?);
                    actualizar = true;
                }
            }

            if (actualizar && !new PrePrestamosIpsCadAutorizacionesLN().ModificarEstado(ipsCAD, bd, tran))
            {
                AyudaProgramacionLN.MapearError(ipsCAD, pParametro);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Actualiza el estado de una cuota a Cobrada y Verifica si esta terminado el prestamo.
        /// </summary>
        /// <param name="pIdCuota"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool CuotaActualizar(CarCuentasCorrientes pCuota, Database bd, DbTransaction tran)
        {
            bool resultado = true;

            PrePrestamosCuotas cuota = new PrePrestamosCuotas();
            cuota.IdPrestamoCuota = pCuota.IdReferenciaRegistro.Value;
            cuota = BaseDatos.ObtenerBaseDatos().Obtener<PrePrestamosCuotas>("PrePrestamosCuotasSeleccionarUna", cuota, bd, tran);

            if (cuota.Estado.IdEstado != (int)EstadosCuotas.Activa)
            {
                pCuota.CodigoMensaje = "ValidarCobroCuotaEstado";
                pCuota.CodigoMensajeArgs.Add(cuota.IdPrestamoCuota.ToString());
                pCuota.CodigoMensajeArgs.Add(cuota.IdPrestamo.ToString());
                resultado = false;
            }
            cuota.Estado.IdEstado = (int)EstadosCuotas.Cobrada;
            cuota.UsuarioLogueado.IdUsuarioEvento = pCuota.UsuarioLogueado.IdUsuarioEvento;
            if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(cuota, bd, tran, "PrePrestamosCuotasActualizar"))
            {
                AyudaProgramacionLN.MapearError(cuota, pCuota);
                resultado = false;
            }

            if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(cuota, bd, tran, ""))
            {
                AyudaProgramacionLN.MapearError(cuota, pCuota);
                resultado = false;
            }

            return resultado;
        }

        /// <summary>
        /// Proceso para Marcar Cuotas como Cobradas en estado Cobrado en Cuenta Corriente
        /// Actualiza la Cuota y el Prestamo en finalizado si estan todas cobradas
        /// </summary>
        /// <param name="pObjeto"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool PrestamosCuotasActualizar(Objeto pObjeto, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pObjeto, bd, tran, "CarProcesoLevantamientoCargosActualizacionPrestamos"))
            {
                pObjeto.CodigoMensaje = "ErrorActualizarPrestamos";
                return false;
            }

            return true;
        }

        public bool ArmarMailLinkFirmarDocumento(PrePrestamos pParametro, MailMessage mail)
        {
            bool resultado = true;
            TGEPlantillas plantilla = new TGEPlantillas();
            plantilla.Codigo = "PrestamosMailLinkFirmarDocumento";
            plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

            if (pParametro.Afiliado.CorreoElectronico.Trim() != string.Empty)
            {
                if (pParametro.Afiliado.CorreoElectronico.Trim().Contains(";"))
                {
                    List<string> lista = pParametro.Afiliado.CorreoElectronico.Trim().Split(';').ToList();
                    foreach (string item in lista)
                        mail.To.Add(new MailAddress(item.Trim(), pParametro.Afiliado.ApellidoNombre.Trim()));
                }
                else
                    mail.To.Add(new MailAddress(pParametro.Afiliado.CorreoElectronico.Trim(), pParametro.Afiliado.ApellidoNombre.Trim()));
            }
            //TGEFirmarDocumentos firmarDoc = new TGEFirmarDocumentos();
            //firmarDoc.UsuarioLogueado = pParametro.UsuarioLogueado;
            //firmarDoc.IdRefTabla = pParametro.IdPrestamo;
            //firmarDoc.Tabla = "PrePrestamos";
            //firmarDoc.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            //firmarDoc = TGEGeneralesF.FirmarDocumentosArmarLinkFirmarDocumento(firmarDoc);
            //pParametro.LinkFirmarDocumento = firmarDoc.Link;
            mail.IsBodyHtml = true;
            if (plantilla.HtmlPlantilla.Trim().Length > 0)
            {
                string htmlPlantilla = plantilla.HtmlPlantilla;
                List<StringStartEnd> posiciones = AyudaProgramacionLN.FindAllString(htmlPlantilla, "{", "}");
                List<string> campos = new List<string>();
                string campo;
                foreach (StringStartEnd pos in posiciones)
                {
                    campo = htmlPlantilla.Substring(pos.start, pos.end - pos.start).Replace("{", "").Replace("}", "");
                    if (campo.Length > 0)
                        campos.Add(campo);
                }
                AyudaProgramacionLN.MapearEntidad(ref htmlPlantilla, campos, pParametro);
                mail.Subject = HttpUtility.HtmlDecode(AyudaProgramacionLN.StripHtml(AyudaProgramacionLN.getBetween(htmlPlantilla, "[Evol:Asunto]", "[/Evol:Asunto]")
                    .Replace("[Evol:Asunto]", "").Replace("[/Evol:Asunto]", "")).Trim());
                mail.Body = AyudaProgramacionLN.getBetween(htmlPlantilla, "[Evol:Cuerpo]", "[/Evol:Cuerpo]")
                    .Replace("[Evol:Cuerpo]", "").Replace("[/Evol:Cuerpo]", "").Trim();
            }
            else
            {
                resultado = false;
                pParametro.CodigoMensaje = "Falta Definir la Plantilla para Enviar el Mail.";
            }
            return resultado;
        }

        private bool ChequesActualizar(PrePrestamos pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            foreach (PrePrestamosCheques Cheques in pParametro.PrestamosCheques)
            {
                Cheques.UsuarioLogueado = pParametro.UsuarioLogueado;
                switch (Cheques.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        Cheques.IdPrestamo = pParametro.IdPrestamo;
                        Cheques.Fecha = pParametro.FechaPrestamo;
                        Cheques.IdPrestamoCheque = BaseDatos.ObtenerBaseDatos().Agregar(Cheques, bd, tran, "PrePrestamosChequesInsertar");
                        if (Cheques.IdPrestamoCheque == 0)
                            resultado = false;
                        break;
                    case EstadoColecciones.Modificado:
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(Cheques, bd, tran, "PrePrestamosChequesActualizar"))
                            resultado = false;
                        break;
                    default:
                        break;
                }
                if (!resultado)
                {
                    AyudaProgramacionLN.MapearError(Cheques, pParametro);
                    return false;
                }
            }
            return true;
        }

        public PrePrestamos ObtenerDatosPreCargados(PrePrestamos pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<PrePrestamos>("PrePrestamosPreCargaVendedor", pParametro);

            return pParametro; //BaseDatos.ObtenerBaseDatos().Obtener<PrePrestamos>("PrePrestamosPreCargaVendedor");
        }
        public List<int> ObtenerProximosPeriodosPrestamos(TGEFormasCobrosAfiliados pParametro)//DEPENDIENDO DE LA FORMA DE COBRO LLAMO AL STORE(POR ESO NO ES PREPRESTAMOS)
        {
            DataTable dt = BaseDatos.ObtenerBaseDatos().ObtenerLista("PrePrestamosSeleccionarComboPeriodosVencimientos", pParametro);
            List<int> resultado = new List<int>();
            if (dt.Rows.Count > 0)
                resultado.AddRange(dt.AsEnumerable().Select(x => x.Field<int>("Periodo")));
            return resultado;
        }
    }
}
