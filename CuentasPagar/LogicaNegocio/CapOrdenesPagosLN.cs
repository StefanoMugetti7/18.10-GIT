using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CuentasPagar.Entidades;
using Servicio.AccesoDatos;
using Comunes;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Generales.FachadaNegocio;
using Auditoria;
using Auditoria.Entidades;
using Generales.Entidades;
using System.Xml;
using System.Data;
using ProcesosDatos.Entidades;

namespace CuentasPagar.LogicaNegocio
{
    public class CapOrdenesPagosLN : BaseLN<CapOrdenesPagos>
    {
        private byte[] ObtenerSelloTiempo(CapOrdenesPagos pParametro, Database db, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerSelloTiempo("CapOrdenesPagosSeleccionar", pParametro, db, tran);
        }

        public override CapOrdenesPagos ObtenerDatosCompletos(CapOrdenesPagos pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CapOrdenesPagos>("CapOrdenesPagosSeleccionar", pParametro);
            pParametro.SolicitudesPagos = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapSolicitudPago>("CapSolicitudPagoSeleccionarPorOrdenPago", pParametro);
            pParametro.SolicitudesAnticipos = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapSolicitudPago>("CapOrdenesPagosSolicitudesPagosAnticiposObtenerPorIdOrdenPago", pParametro);
            pParametro.ImporteAnticipos = pParametro.SolicitudesAnticipos.Sum(x => x.ImporteTotal);
            pParametro.OrdenesPagosValore = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapOrdenesPagosValores>("CapOrdenesPagosValoresSeleccionarPorOrdenPago", pParametro);
            CapOrdenesPagosTiposRetenciones tipoRet = new CapOrdenesPagosTiposRetenciones();
            tipoRet.IdOrdenPago=pParametro.IdOrdenPago;
            pParametro.OrdenesPagosTiposRetenciones = new CapOrdenesPagosTiposRetencionesLN().ObtenerListaFiltro(tipoRet);
            pParametro.ImporteRetenciones = pParametro.OrdenesPagosTiposRetenciones.Sum(x => x.ImporteTotalRetencion);
            pParametro.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }

        public CapOrdenesPagos ObtenerDatosCompletos(CapOrdenesPagos pParametro, Database db, DbTransaction tran)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CapOrdenesPagos>("CapOrdenesPagosSeleccionar", pParametro, db, tran);
            pParametro.SolicitudesPagos = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapSolicitudPago>("CapSolicitudPagoSeleccionarPorOrdenPago", pParametro, db, tran);
            pParametro.SolicitudesAnticipos = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapSolicitudPago>("CapOrdenesPagosSolicitudesPagosAnticiposObtenerPorIdOrdenPago", pParametro, db, tran);
            pParametro.ImporteAnticipos = pParametro.SolicitudesAnticipos.Sum(x => x.ImporteTotal);
            pParametro.OrdenesPagosValore = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapOrdenesPagosValores>("CapOrdenesPagosValoresSeleccionarPorOrdenPago", pParametro, db, tran);
            CapOrdenesPagosTiposRetenciones tipoRet = new CapOrdenesPagosTiposRetenciones();
            tipoRet.IdOrdenPago = pParametro.IdOrdenPago;
            pParametro.OrdenesPagosTiposRetenciones = new CapOrdenesPagosTiposRetencionesLN().ObtenerListaFiltro(tipoRet);
            pParametro.ImporteRetenciones = pParametro.OrdenesPagosTiposRetenciones.Sum(x => x.ImporteTotalRetencion);
            return pParametro;
        }

        public XmlDocument ObtenerMovimientoValoresXML(CapOrdenesPagos pParametro)
        {
            XmlDocument LoteCajasMovimientosValores = new XmlDocument();
            //XmlNode docNode = pParametro.LotePrestamos.CreateXmlDeclaration("1.0", "UTF-8", null);
            //pParametro.LotePrestamos.AppendChild(docNode);

            XmlNode cajasMovimientosValores = LoteCajasMovimientosValores.CreateElement("CajasMovimientosValores");
            LoteCajasMovimientosValores.AppendChild(cajasMovimientosValores);

            XmlNode cajaMovimientoValor;
            XmlAttribute attribute;
            foreach (CapOrdenesPagosValores movValores in pParametro.OrdenesPagosValore)
            {
                switch (movValores.TipoValor.IdTipoValor)
                {
                    case (int)EnumTiposValores.AfipTiposPercepciones:
                    case (int)EnumTiposValores.AfipTiposRetenciones:
                    case (int)EnumTiposValores.Efectivo:
                        #region Efectivo
                        cajaMovimientoValor = LoteCajasMovimientosValores.CreateElement("CajaMovimientoValor");

                        attribute = LoteCajasMovimientosValores.CreateAttribute("IdTipoValor");
                        attribute.Value = movValores.TipoValor.IdTipoValor.ToString();
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("Importe");
                        attribute.Value = movValores.Importe.ToString().Replace(',', '.');
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);
                        #endregion
                        break;
                    case (int)EnumTiposValores.Cargos:
                        break;
                    case (int)EnumTiposValores.Cheque:
                        #region Cheque
                            cajaMovimientoValor = LoteCajasMovimientosValores.CreateElement("CajaMovimientoValor");

                            attribute = LoteCajasMovimientosValores.CreateAttribute("IdTipoValor");
                            attribute.Value = movValores.TipoValor.IdTipoValor.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = LoteCajasMovimientosValores.CreateAttribute("Importe");
                            attribute.Value = movValores.Importe.ToString().Replace(',', '.');
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = LoteCajasMovimientosValores.CreateAttribute("IdBancoCuenta");
                            attribute.Value = movValores.BancoCuenta.IdBancoCuenta.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = LoteCajasMovimientosValores.CreateAttribute("Fecha");
                            attribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(movValores.Fecha.Value);
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = LoteCajasMovimientosValores.CreateAttribute("FechaDiferido");
                            attribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(movValores.FechaDiferido.Value);
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = LoteCajasMovimientosValores.CreateAttribute("NumeroCheque");
                            attribute.Value = movValores.NumeroCheque;
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);
                        #endregion
                        break;
                    case (int)EnumTiposValores.ChequeTercero:
                        #region Cheque Terceros
                            cajaMovimientoValor = LoteCajasMovimientosValores.CreateElement("CajaMovimientoValor");

                            attribute = LoteCajasMovimientosValores.CreateAttribute("IdTipoValor");
                            attribute.Value = movValores.TipoValor.IdTipoValor.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = LoteCajasMovimientosValores.CreateAttribute("Importe");
                            attribute.Value = movValores.Importe.ToString().Replace(',', '.');
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = LoteCajasMovimientosValores.CreateAttribute("IdBanco");
                            attribute.Value = movValores.Cheque.Banco.IdBanco.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = LoteCajasMovimientosValores.CreateAttribute("Fecha");
                            attribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(movValores.Fecha.Value);
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = LoteCajasMovimientosValores.CreateAttribute("FechaDiferido");
                            attribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(movValores.FechaDiferido.Value);
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("IdCheque");
                        attribute.Value = movValores.Cheque.IdCheque.ToString();
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("NumeroCheque");
                            attribute.Value = movValores.NumeroCheque;
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = LoteCajasMovimientosValores.CreateAttribute("TitularCheque");
                            attribute.Value = movValores.Cheque.TitularCheque;
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = LoteCajasMovimientosValores.CreateAttribute("CUIT");
                            attribute.Value = movValores.Cheque.CUIT;
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = LoteCajasMovimientosValores.CreateAttribute("Concepto");
                            attribute.Value = movValores.Cheque.Concepto;
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);
                        #endregion
                        break;
                    case (int)EnumTiposValores.Prestamos:
                        break;
                    case (int)EnumTiposValores.SinMovimientosFondos:
                        break;
                    case (int)EnumTiposValores.TarjetaCredito:
                    case (int)EnumTiposValores.Transferencia:
                    case (int)EnumTiposValores.EfectivoFondoFijo:
                        #region Transferencia / Fondo Fijo
                        cajaMovimientoValor = LoteCajasMovimientosValores.CreateElement("CajaMovimientoValor");

                            attribute = LoteCajasMovimientosValores.CreateAttribute("IdTipoValor");
                            attribute.Value = movValores.TipoValor.IdTipoValor.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = LoteCajasMovimientosValores.CreateAttribute("Importe");
                            attribute.Value = movValores.Importe.ToString().Replace(',', '.');
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = LoteCajasMovimientosValores.CreateAttribute("IdBancoCuenta");
                            attribute.Value = movValores.BancoCuenta.IdBancoCuenta.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = LoteCajasMovimientosValores.CreateAttribute("FechaConfirmacionBanco");
                            attribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(movValores.Fecha.Value);
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("CantidadCuotas");
                        attribute.Value = movValores.CantidadCuotas.ToString();
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("ImporteCuota");
                        attribute.Value = movValores.ImporteCuota.ToString().Replace(',', '.');
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("IdOrdenPagoValor");
                        attribute.Value = movValores.IdOrdenPagoValor.ToString();
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        #endregion
                        break;
                    default:
                        break;
                }

            }
            return LoteCajasMovimientosValores;
        }

        public override List<CapOrdenesPagos> ObtenerListaFiltro(CapOrdenesPagos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CapOrdenesPagos>("CapOrdenesPagosSeleccionarPorFiltro", pParametro);
        }

        public DataTable ObtenerListaFiltroGrilla(CapOrdenesPagos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CapOrdenesPagosSeleccionarPorFiltro", pParametro);
        }

        public DataTable ObtenerListaFiltroGrillaTurismo(CapOrdenesPagos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CapOrdenesPagosSeleccionarPorFiltroTurismo", pParametro);
        }
        public DataTable ObtenerListaFiltroPagoTurismo(CapOrdenesPagos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CapOrdenesPagosSeleccionarPagoPorFiltroTurismo", pParametro);
        }

        public override bool Agregar(CapOrdenesPagos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            if (!this.Validar(pParametro))
                return false;

            pParametro.FechaEvento = DateTime.Now;
            pParametro.Estado.IdEstado = (int)Estados.Activo;

            if (pParametro.SolicitudesPagos.Where(x => x.IncluirEnOP).ToList()[0].TipoOperacion.Contabiliza)
                pParametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.OrdenesPagos;
            else
                pParametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.OrdenesPagosInterno;

            TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.OrdenesPagosCircuitoAprobacion);
            bool bvalor = false;
            if (!string.IsNullOrEmpty(valor.ParametroValor) && bool.TryParse(valor.ParametroValor, out bvalor))
            {
                if (!bvalor)
                {
                    pParametro.Estado.IdEstado = (int)EstadosOrdenesPago.Autorizado;
                    pParametro.FechaAutorizacion = pParametro.FechaPago.HasValue ? pParametro.FechaPago : DateTime.Now;
                    pParametro.IdUsuarioAutorizacion = pParametro.IdUsuarioAlta;
                }
            }

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (pParametro.OrdenesPagosValore.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque))
                    {
                        OperacionConfirmar opValidar = new OperacionConfirmar();
                        opValidar.IdRefTipoOperacion = pParametro.IdOrdenPago;
                        opValidar.LoteCajasMovimientosValores = this.ObtenerMovimientoValoresXML(pParametro);
                        if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(opValidar, bd, tran, "TesChequesValidarNumero"))
                        {
                            AyudaProgramacionLN.MapearError(opValidar, pParametro);
                            return false;
                        }
                    }

                    pParametro.IdOrdenPago = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CapOrdenesPagosInsertar");
                    if (pParametro.IdOrdenPago == 0)
                        resultado = false;

                    if (resultado && !this.ActualizarDetallesAgregar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarOrdenesPagosValores(pParametro, new CapOrdenesPagos(), bd, tran))
                        resultado = false;

                    if (resultado && !new CapOrdenesPagosTiposRetencionesLN().AgregarOrdenesPagosTiposRetenciones(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;
                    //if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    //valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.CircuitoDiarioCajasAutomatico,bd, tran);
                    //bvalor = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;

                    if (resultado && pParametro.Estado.IdEstado == (int)EstadosOrdenesPago.Autorizado && pParametro.ImporteTotal == 0)
                    {
                        if(!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CapOrdenesPagosActualizarConfirmar"))
                        {
                            resultado = false;
                        }
                        if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CapOrdenesPagosActualizarConfirmarContabilizar"))
                        {
                            resultado = false;
                        }
                    }
                    else
                    {
                        OperacionConfirmar paramValor = new OperacionConfirmar();
                        paramValor.IdRefTipoOperacion = pParametro.IdOrdenPago;
                        paramValor.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
                        paramValor.UsuarioLogueado = pParametro.UsuarioLogueado;
                        bvalor = BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(paramValor, bd, tran, "TesCajasMovimientosCircuitoDiarioCajasAutomatico");

                        if (resultado && bvalor && pParametro.Estado.IdEstado == (int)EstadosOrdenesPago.Autorizado)
                        {
                            OperacionConfirmar opConfirmar = new OperacionConfirmar();
                            opConfirmar.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
                            opConfirmar.IdRefTipoOperacion = pParametro.IdOrdenPago;
                            opConfirmar.Fecha = pParametro.FechaPago.Value;
                            opConfirmar.IdFilial = pParametro.FilialPago.IdFilialPago.Value;
                            opConfirmar.Importe = pParametro.ImporteTotal;
                            opConfirmar.Descripcion = pParametro.Observacion;
                            opConfirmar.UsuarioLogueado = pParametro.UsuarioLogueado;
                            opConfirmar.LoteCajasMovimientosValores = this.ObtenerMovimientoValoresXML(pParametro);
                            opConfirmar.SelloTiempo = this.ObtenerSelloTiempo(pParametro, bd, tran);
                            resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(opConfirmar, bd, tran, "TesCajasMovimientosConfirmacionAutomaticaProceso");
                            if (!resultado)
                            {
                                resultado = false;
                                AyudaProgramacionLN.MapearError(opConfirmar, pParametro);
                            }
                        }
                    }
                    //if (resultado && pParametro.Estado.IdEstado==(int)EstadosOrdenesPago.Autorizado && pParametro.ImporteTotal==0)
                    //{
                    //    CapOrdenesPagos opConfirmar = new CapOrdenesPagos();
                    //    opConfirmar.IdOrdenPago = pParametro.IdOrdenPago;
                    //    opConfirmar = this.ObtenerDatosCompletos(opConfirmar, bd, tran);
                    //    opConfirmar.UsuarioLogueado = pParametro.UsuarioLogueado;
                    //    if (!this.ConfirmarMovimiento(opConfirmar, pParametro.FechaAutorizacion.Value, bd, tran))
                    //    {
                    //        resultado = false;
                    //        AyudaProgramacionLN.MapearError(opConfirmar, pParametro);
                    //    }
                    //}

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

        public CapOrdenesPagosValores ObtenerCbu(CapOrdenesPagos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CapOrdenesPagosValores>("CapOrdenesPagosValoresObtenerCbu", pParametro);
        }
        public CapOrdenesPagosValores ObtenerDatosCbu(CapOrdenesPagosValores pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CapOrdenesPagosValores>("CapOrdenesPagosValoresObtenerDatosCbu", pParametro);
        }


        /// <summary>
        /// Metodo para Autorizar una Orden de Pago
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool Autorizar(CapOrdenesPagos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //if (!this.Validar(pParametro))
            //    return false;

            pParametro.FechaEvento = DateTime.Now;
            pParametro.Estado.IdEstado = (int)EstadosOrdenesPago.Autorizado;
            pParametro.Estado = TGEGeneralesF.TGEEstadosObtener(pParametro.Estado);

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CapOrdenesPagos valorViejo = new CapOrdenesPagos();
            valorViejo.IdOrdenPago = pParametro.IdOrdenPago;
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
                    if(!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CapOrdenesPagosActualizar"))
                        resultado = false;

                    if (resultado && !this.ActualizarOrdenesPagosValores(pParametro, valorViejo, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    //if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;


                    if (resultado && pParametro.Estado.IdEstado == (int)EstadosOrdenesPago.Autorizado && pParametro.ImporteTotal == 0)
                    {
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CapOrdenesPagosActualizarConfirmar"))
                        {
                            resultado = false;
                        }
                        if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CapOrdenesPagosActualizarConfirmarContabilizar"))
                        {
                            resultado = false;
                        }
                    }
                    else
                    {
                        //GEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.CircuitoDiarioCajasAutomatico);
                        OperacionConfirmar paramValor = new OperacionConfirmar();
                        paramValor.IdRefTipoOperacion = pParametro.IdOrdenPago;
                        paramValor.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
                        paramValor.UsuarioLogueado = pParametro.UsuarioLogueado;
                        bool bvalor = BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(paramValor, bd, tran, "TesCajasMovimientosCircuitoDiarioCajasAutomatico");
                        if (resultado && bvalor && pParametro.Estado.IdEstado == (int)EstadosOrdenesPago.Autorizado)
                        {
                            OperacionConfirmar opConfirmar = new OperacionConfirmar();
                            opConfirmar.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
                            opConfirmar.IdRefTipoOperacion = pParametro.IdOrdenPago;
                            opConfirmar.Fecha = pParametro.FechaPago.Value;
                            opConfirmar.IdFilial = pParametro.FilialPago.IdFilialPago.Value;
                            opConfirmar.Importe = pParametro.ImporteTotal;
                            opConfirmar.Descripcion = pParametro.Observacion;
                            opConfirmar.UsuarioLogueado = pParametro.UsuarioLogueado;
                            opConfirmar.LoteCajasMovimientosValores = this.ObtenerMovimientoValoresXML(pParametro);
                            opConfirmar.SelloTiempo = this.ObtenerSelloTiempo(pParametro, bd, tran);
                            resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(opConfirmar, bd, tran, "TesCajasMovimientosConfirmacionAutomaticaProceso");
                            if (!resultado)
                            {
                                resultado = false;
                                AyudaProgramacionLN.MapearError(opConfirmar, pParametro);
                            }
                        }
                        //else if (resultado && pParametro.Estado.IdEstado == (int)EstadosOrdenesPago.Autorizado && pParametro.ImporteTotal == 0)
                        //{
                        //    CapOrdenesPagos opConfirmar = new CapOrdenesPagos();
                        //    opConfirmar.IdOrdenPago = pParametro.IdOrdenPago;
                        //    opConfirmar = this.ObtenerDatosCompletos(opConfirmar, bd, tran);
                        //    opConfirmar.UsuarioLogueado = pParametro.UsuarioLogueado;
                        //    if (!this.ConfirmarMovimiento(opConfirmar, pParametro.FechaAutorizacion.Value, bd, tran))
                        //    {
                        //        resultado = false;
                        //        AyudaProgramacionLN.MapearError(opConfirmar, pParametro);
                        //    }
                        //}
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

        /// <summary>
        /// Metodo para Anular/Baja de Orden de Pago
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool Anular(CapOrdenesPagos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //if (!this.Validar(pParametro))
            //    return false;

            pParametro.FechaEvento = DateTime.Now;
            pParametro.Estado.IdEstado = (int)EstadosOrdenesPago.Baja;
            pParametro.Estado = TGEGeneralesF.TGEEstadosObtener(pParametro.Estado);

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CapOrdenesPagos valorViejo = new CapOrdenesPagos();
            valorViejo.IdOrdenPago = pParametro.IdOrdenPago;
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
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CapOrdenesPagosActualizar"))
                        resultado = false;

                    if (resultado && !this.ActualizarDetallesAnular(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    //if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
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

        /// <summary>
        /// Metodo para Anular una Orden de Pago Pagada
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public bool AnularPagada(CapOrdenesPagos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CapOrdenesPagosValidaciones"))
            {
                pParametro.CodigoMensaje = pParametro.CodigoMensaje;
                return false;
            }

            pParametro.FechaEvento = DateTime.Now;
            pParametro.Estado.IdEstado = (int)EstadosOrdenesPago.Baja;
            pParametro.Estado = TGEGeneralesF.TGEEstadosObtener(pParametro.Estado);

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CapOrdenesPagos valorViejo = new CapOrdenesPagos();
            valorViejo.IdOrdenPago = pParametro.IdOrdenPago;
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
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CapOrdenesPagosAnularPagadaProceso"))
                        resultado = false;

                    //if (resultado && !this.ActualizarDetallesAnular(pParametro, bd, tran))
                    //    resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;


                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
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

        private bool ActualizarDetallesAgregar(CapOrdenesPagos pParametro, Database db, DbTransaction tran)
        {
            CapOrdenesPagosSolicitudesPagos item;
            CapSolicitudPagoLN solPagoLN = new CapSolicitudPagoLN();
            foreach (CapSolicitudPago Detalle in pParametro.SolicitudesPagos.Where(x=>x.IncluirEnOP).ToList())
            {
                item = new CapOrdenesPagosSolicitudesPagos();
                item.IdSolicitudPago = Detalle.IdSolicitudPago;
                item.IdOrdenPago = pParametro.IdOrdenPago;
                item.ImportePago = Detalle.ImporteParcial;
                item.IdOrdenPagoSolicitudPago = BaseDatos.ObtenerBaseDatos().Agregar(item, db, tran, "CapOrdenesPagosSolicitudesPagosInsertar");
                if (item.IdOrdenPagoSolicitudPago == 0)
                {
                    AyudaProgramacionLN.MapearError(item, pParametro);
                    return false;
                }

                if (Detalle.ImporteTotal == Detalle.ImporteParcial + Detalle.ImporteParcialPagado)
                    Detalle.Estado.IdEstado = (int)EstadosSolicitudesPagos.EnOrdenPago;
                else
                    Detalle.Estado.IdEstado = (int)EstadosSolicitudesPagos.EnOrdenPagoParcial;

                if (!solPagoLN.ModificarEstado(Detalle, db, tran))
                {
                    AyudaProgramacionLN.MapearError(Detalle, pParametro);
                    return false;
                }

            }
            CapOrdenesPagosSolicitudesPagosAnticipos anti;
            foreach (CapSolicitudPago Detalle in pParametro.SolicitudesAnticipos.Where(x => x.IncluirEnOP).ToList())
            {
                anti = new CapOrdenesPagosSolicitudesPagosAnticipos();
                anti.IdSolicitudPagoAnticipo = Detalle.IdSolicitudPago;
                anti.IdOrdenPago = pParametro.IdOrdenPago;
                anti.Importe = Detalle.ImporteTotal;
                anti.IdOrdenPagoSolicitudPagoAnticipo = BaseDatos.ObtenerBaseDatos().Agregar(anti, db, tran, "CapOrdenesPagosSolicitudesPagosAnticipoInsertar");
                if (anti.IdOrdenPagoSolicitudPagoAnticipo == 0)
                {
                    AyudaProgramacionLN.MapearError(anti, pParametro);
                    return false;
                }
            }


            return true;
        }

        private bool ActualizarDetallesAnular(CapOrdenesPagos pParametro, Database db, DbTransaction tran)
        {
            CapSolicitudPagoLN solPagoLN = new CapSolicitudPagoLN();
            foreach (CapSolicitudPago Detalle in pParametro.SolicitudesPagos)
            {
                if (!BaseDatos.ObtenerBaseDatos().Actualizar(Detalle, db, tran, "CapSolicitudPagoActualizarEstadoDetalles"))
                    return false;
                //if (Detalle.ImporteTotal == Detalle.ImporteParcial)
                //    Detalle.Estado.IdEstado = (int)EstadosSolicitudesPagos.Autorizado;
                //else
                //    Detalle.Estado.IdEstado = (int)EstadosSolicitudesPagos.EnOrdenPagoParcial;

                //if (!solPagoLN.ModificarEstado(Detalle, db, tran))
                //{
                //    AyudaProgramacionLN.MapearError(Detalle, pParametro);
                //    return false;
                //}
            }

            //ELIMINO TODAS LAS RELACIONES DE LA TABLA CapOrdenesPagosSolicitudesPagosAnticipos pasando IdOrdenPago, y asi libero las solicitudes.
            if (pParametro.SolicitudesAnticipos.Count > 0)
            {
                if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, db, tran, "CapOrdenesPagosSolicitudesPagosAnticiposBorrarPorOrdenPago"))
                    return false;
            }

            return true;
        }

        private bool ActualizarOrdenesPagosValores(CapOrdenesPagos pParametro, CapOrdenesPagos pValorViejo, Database db, DbTransaction tran)
        {
            foreach (CapOrdenesPagosValores opValor in pParametro.OrdenesPagosValore)
            {
                switch (opValor.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        opValor.IdOrdenPago = pParametro.IdOrdenPago;
                        opValor.Estado.IdEstado=(int)Estados.Activo;
                        opValor.IdOrdenPagoValor = BaseDatos.ObtenerBaseDatos().Agregar(opValor, db, tran, "CapOrdenesPagosValoresInsertar");
                        if (opValor.IdOrdenPagoValor == 0)
                        {
                            AyudaProgramacionLN.MapearError(opValor, pParametro);
                            return false;
                        }
                        break;
                    case EstadoColecciones.Modificado:
                        opValor.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(opValor, db, tran, "CapOrdenesPagosValoresActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(opValor, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.OrdenesPagosValore.Find(x => x.IdOrdenPagoValor == opValor.IdOrdenPagoValor), Acciones.Update, opValor, db, tran))
                        {
                            AyudaProgramacionLN.MapearError(opValor, pParametro);
                            return false;
                        }
                        break;
                    default:
                        break;
                }
                
            }
            return true;
        }

        private bool Validar(CapOrdenesPagos pParametro)
        {
            List<CapSolicitudPago> itemsInlcuidos = pParametro.SolicitudesPagos.Where(x => x.IncluirEnOP).ToList();
            if (itemsInlcuidos.Count == 0)
            {
                pParametro.CodigoMensaje = "ValidarCantidadSolicitudesPagos";
                    return false;
            }

            foreach (CapSolicitudPago item in itemsInlcuidos)
            {
                if (item.ImporteParcial == 0)
                {
                    pParametro.CodigoMensaje = "ValidarImporteSolicitudPago";
                    pParametro.CodigoMensajeArgs.Add(item.IdSolicitudPago.ToString());
                    return false;
                }
                if (item.ImporteTotal > 0 && item.ImporteParcial + item.ImporteParcialPagado > item.ImporteTotal)
                {
                    pParametro.CodigoMensaje = "ValidarImporteMayorSolicitudPago";
                    pParametro.CodigoMensajeArgs.Add(item.IdSolicitudPago.ToString());
                    pParametro.CodigoMensajeArgs.Add((item.ImporteTotal - item.ImporteParcialPagado).ToString("C2"));
                    return false;
                }
                if (item.ImporteTotal < 0 && item.ImporteParcial + item.ImporteParcialPagado < item.ImporteTotal)
                {
                    pParametro.CodigoMensaje = "ValidarImporteMayorSolicitudPago";
                    pParametro.CodigoMensajeArgs.Add(item.IdSolicitudPago.ToString());
                    pParametro.CodigoMensajeArgs.Add((item.ImporteTotal - item.ImporteParcialPagado).ToString("C2"));
                    return false;
                }
            }

            if (pParametro.ImporteTotal != pParametro.OrdenesPagosValore.Sum(x => x.Importe))
            {
                pParametro.CodigoMensaje = "ValidarImporteTotalImporteValores";
                return false;
            }

            if (itemsInlcuidos.Select(x => x.TipoOperacion.Contabiliza).Distinct().Count() > 1)
            {
                pParametro.CodigoMensaje = "ValidarTiposComprobantesContabilizar";
                return false;
            }

            //if (pParametro.SolicitudesPagos.Where(x=>x.IncluirEnOP).ToList().Exists(x => x.TiposFacturas.Contabilizar)
            //    && pParametro.SolicitudesPagos.Where(x => x.IncluirEnOP).ToList().Exists(x => !x.TiposFacturas.Contabilizar)
            //    )
            //{
            //    pParametro.CodigoMensaje = "ValidarTiposComprobantesContabilizar";
            //    return false;
            //}
            
            return true;
        }



        /// <summary>
        /// Confirma un movimiento de Pagos
        /// Metodo para los movimientos de Caja
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool ConfirmarMovimiento(Objeto pParametro, DateTime pFecha, Database bd, DbTransaction tran)
        {
            bool resultado = true;

            CapOrdenesPagos ordenPago = (CapOrdenesPagos)pParametro;
            ordenPago.Estado.IdEstado = (int)EstadosOrdenesPago.Pagado;
            ordenPago.FechaConfirmacion = pFecha;// DateTime.Now;
            ordenPago.FechaPago = pFecha; // DateTime.Now;
            ordenPago.IdUsuarioConfirmacion = ordenPago.UsuarioLogueado.IdUsuario;

            bool contabilizar = ordenPago.SolicitudesPagos.Exists(
                x => x.TiposFacturas.Contabilizar) ? true : false;

            if (contabilizar)
                ordenPago.NumeroOrdenPago = BaseDatos.ObtenerBaseDatos().Obtener<CapOrdenesPagos>("CapOrdenesPagosSeleccionarProximoNumero", new Objeto(), bd, tran).NumeroOrdenPago;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CapOrdenesPagos valorViejo = new CapOrdenesPagos();
            valorViejo.IdOrdenPago = ordenPago.IdOrdenPago;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo, bd, tran);


            if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(ordenPago, bd, tran, "CapOrdenesPagosActualizar"))
                resultado = false;

            if (resultado && !this.ActualizarOrdenesPagosValores(ordenPago, valorViejo, bd, tran))
                resultado = false;

            CapSolicitudPagoLN solPagoLN = new CapSolicitudPagoLN();
            foreach (CapSolicitudPago sp in ordenPago.SolicitudesPagos)
            {
                sp.UsuarioLogueado = ordenPago.UsuarioLogueado;
                if (sp.ImporteParcial + sp.ImporteParcialPagado == sp.ImporteTotal)
                    sp.Estado.IdEstado = (int)EstadosSolicitudesPagos.Pagado;
                else
                    sp.Estado.IdEstado = (int)EstadosSolicitudesPagos.PagadoParcial;

                if (!solPagoLN.ModificarEstado(sp, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(sp, ordenPago);
                    resultado = false;
                    break;
                }
            }

            if (resultado && contabilizar && !new InterfazContableLN().OrdenesPagos(ordenPago, bd, tran))
                resultado = false;

            return resultado;
        }

        public CapOrdenesPagos ObtenerUltimaPorSolicitudPago(CapSolicitudPago pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CapOrdenesPagos>("CapOrdenPagoObtenerUltimaPorIdSolicitudPago", pParametro);
        }

        public override bool Modificar(CapOrdenesPagos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CapOrdenesPagos valorViejo = new CapOrdenesPagos();
            valorViejo.IdOrdenPago = pParametro.IdOrdenPago;
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
                    //if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CapSolicitudPagoActualizar"))
                    //    resultado = false;

                    //if (resultado && !this.ActualizarDetalles(pParametro, valorViejo, bd, tran))
                    //    resultado = false;

                    //if (resultado && !this.ActualizarTiposPercepciones(pParametro, valorViejo, bd, tran))
                    //    resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    //if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
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

        public DataTable ImportarArchivo(SisProcesosProcesamiento pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CapSolicitudPagoImportarArchivoCargaMasiva", pParametro);
        }
    }
}
