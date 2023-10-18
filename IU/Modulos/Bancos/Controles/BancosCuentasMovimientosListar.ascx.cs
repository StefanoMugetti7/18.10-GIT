using Afiliados;
using Afiliados.Entidades;
using Ahorros.Entidades;
using Bancos;
using Bancos.Entidades;
using Cobros.Entidades;
using Comunes.Entidades;
using CuentasPagar.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Prestamos.Entidades;
using Reportes.Entidades;
using Reportes.FachadaNegocio;
using Seguridad.FachadaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace IU.Modulos.Bancos.Controles
{
    public partial class BancosCuentasMovimientosListar : ControlesSeguros
    {
        private TESBancosCuentas MiBancoCuenta
        {
            get { return (TESBancosCuentas)Session[this.MiSessionPagina + "BancosCuentasMovimientosListarMiBancoCuenta"]; }
            set { Session[this.MiSessionPagina + "BancosCuentasMovimientosListarMiBancoCuenta"] = value; }
        }
        private TGEEstados MiEstadoConfirmado
        {
            get { return (TGEEstados)Session[this.MiSessionPagina + "BancosCuentasMovimientosListarMiEstadoConfirmado"]; }
            set { Session[this.MiSessionPagina + "BancosCuentasMovimientosListarMiEstadoConfirmado"] = value; }
        }
        private List<TGEEstados> MisEstados
        {
            get { return (List<TGEEstados>)Session[this.MiSessionPagina + "ChequesModificarListarMisEstados"]; }
            set { Session[this.MiSessionPagina + "ChequesModificarListarMisEstados"] = value; }
        }
        private bool MiPermisoConciliar
        {
            get { return (bool)Session[this.MiSessionPagina + "BancosCuentasMovimientosListarMiPermisoConciliar"]; }
            set { Session[this.MiSessionPagina + "BancosCuentasMovimientosListarMiPermisoConciliar"] = value; }
        }
        private DataTable MisDatosGrilla
        {
            get { return (DataTable)Session[this.MiSessionPagina + "BancosCuentasMovimientosListarMisDatosGrilla"]; }
            set { Session[this.MiSessionPagina + "BancosCuentasMovimientosListarMisDatosGrilla"] = value; }
        }
        private DataTable MisDatosGrillaPendientes
        {
            get { return (DataTable)Session[this.MiSessionPagina + "BancosCuentasMovimientosListarMisDatosGrillaPendientes"]; }
            set { Session[this.MiSessionPagina + "BancosCuentasMovimientosListarMisDatosGrillaPendientes"] = value; }
        }
        private DataTable MisDatosGrillaRechazados
        {
            get { return (DataTable)Session[this.MiSessionPagina + "BancosCuentasMovimientosListarMisDatosGrillaRechazados"]; }
            set { Session[this.MiSessionPagina + "BancosCuentasMovimientosListarMisDatosGrillaRechazados"] = value; }
        }
        //private DataTable MisDatosPendientesGrilla
        //{
        //    get { return (DataTable)Session[this.MiSessionPagina + "BancosCuentasMovimientosListarMisDatosPendientesGrilla"]; }
        //    set { Session[this.MiSessionPagina + "BancosCuentasMovimientosListarMisDatosPendientesGrilla"] = value; }
        //}
        //public delegate void BancosCuentasDatosAceptarEventHandler(object sender, TESBancosCuentas e);
        //public event BancosCuentasDatosAceptarEventHandler BancoCuentaModificarDatosAceptar;
        public delegate void BancosCuentasDatosCancelarEventHandler();
        public event BancosCuentasDatosCancelarEventHandler BancoCuentaModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            this.gvDatos.PageSizeEvent += this.GvDatos_PageSizeEvent;
            this.gvRechazados.PageSizeEvent += this.GvRechazados_PageSizeEvent;
            this.gvPendientes.PageSizeEvent += this.GvPendientes_PageSizeEvent;
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtFechaDesde, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtFechaHasta, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDetalle, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDetallePendiente, this.btnBuscarPendientes);
                this.btnAgregar.Visible = this.ValidarPermiso("BancosCuentasMovimientosAgregar.aspx");

                this.SetInitializeCulture(this.MiBancoCuenta.Moneda.Moneda);

                TESBancosCuentas parametros = this.BusquedaParametrosObtenerValor<TESBancosCuentas>();
                if (parametros.BusquedaParametros)
                {
                    this.txtFechaDesde.Text = parametros.FechaDesde.ToShortDateString();
                    this.txtFechaHasta.Text = parametros.FechaHasta.ToShortDateString();
                    this.gvDatos.PageIndex = parametros.IndiceColeccion;
                    this.gvPendientes.PageIndex = parametros.Filial.IndiceColeccion;

                    if (parametros.HashTransaccion == 0)
                    {
                        this.tcDatos.ActiveTab = this.tpMovimientos;
                        this.txtDetalle.Text = parametros.Detalle;
                    }
                    else if (parametros.HashTransaccion == 1)
                    {
                        this.tcDatos.ActiveTab = this.tpPendientes;
                        this.txtDetallePendiente.Text = parametros.Detalle;
                    }
                }
                else
                {
                    this.txtFechaDesde.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                    this.txtFechaHasta.Text = DateTime.Now.ToShortDateString();
                    parametros.IdBancoCuenta = this.MiBancoCuenta.IdBancoCuenta;
                }
                this.btnBuscar_Click(null, EventArgs.Empty);
                this.btnBuscarPendientes_Click(null, EventArgs.Empty);
            }
        }
        private void GvDatos_PageSizeEvent(int pageSize)
        {
            this.UsuarioActivo.PageSize = pageSize;
            this.gvDatos.PageSize = pageSize;
            this.gvDatos.PageIndex = 0;
            this.MiBancoCuenta.PageIndex = 0;

            this.btnBuscar_Click(null, EventArgs.Empty);
        }
        private void GvRechazados_PageSizeEvent(int pageSize)
        {
            this.UsuarioActivo.PageSize = pageSize;
            this.gvRechazados.PageSize = pageSize;
            this.gvRechazados.PageIndex = 0;
            this.MiBancoCuenta.PageIndex = 0;

            this.btnBuscar_Click(null, EventArgs.Empty);
        }
        private void GvPendientes_PageSizeEvent(int pageSize)
        {
            this.UsuarioActivo.PageSize = pageSize;
            this.gvPendientes.PageSize = pageSize;
            this.gvPendientes.PageIndex = 0;
            this.MiBancoCuenta.PageIndex = 0;

            this.btnBuscarPendientes_Click(null, EventArgs.Empty);
        }
        /// <summary>
        /// Inicializa el control de Detalle y Conciliacion Bancaria
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(TESBancosCuentas pBancoCuenta, Gestion pGestion)
        {
            //OJO CON ESTO!
            TESBancosCuentas parametros = this.BusquedaParametrosObtenerValor<TESBancosCuentas>();
            if (pBancoCuenta.IdBancoCuenta != parametros.IdBancoCuenta)
            {
                parametros = new TESBancosCuentas();
                this.BusquedaParametrosGuardarValor<TESBancosCuentas>(parametros);
            }

            this.GestionControl = pGestion;
            this.MiBancoCuenta = pBancoCuenta;
            this.MiBancoCuenta = BancosF.BancosCuentasObtenerDatosCompletos(this.MiBancoCuenta);

            this.MisEstados = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosBancosCuentasMovimientos));
            if (this.MisEstados.Exists(x => x.IdEstado == (int)EstadosBancosCuentasMovimientos.Baja))
                this.MisEstados.Remove(this.MisEstados.Find(x => x.IdEstado == (int)EstadosBancosCuentasMovimientos.Baja));
            if (this.MisEstados.Exists(x => x.IdEstado == (int)EstadosBancosCuentasMovimientos.PendienteConfirmacion))
                this.MisEstados.Remove(this.MisEstados.Find(x => x.IdEstado == (int)EstadosBancosCuentasMovimientos.PendienteConfirmacion));
            if (this.MisEstados.Exists(x => x.IdEstado == (int)EstadosBancosCuentasMovimientos.Pendiente))
                this.MisEstados.Remove(this.MisEstados.Find(x => x.IdEstado == (int)EstadosBancosCuentasMovimientos.Pendiente));

            this.MiEstadoConfirmado = new TGEEstados();
            this.MiEstadoConfirmado.IdEstado = (int)EstadosBancosCuentasMovimientos.Confirmado;
            this.MiEstadoConfirmado = TGEGeneralesF.TGEEstadosObtener(this.MiEstadoConfirmado);
            this.MiPermisoConciliar = this.ValidarPermiso("BancosCuentasMovimientosConciliar.aspx");
            this.MapearObjetoAControles(this.MiBancoCuenta);

            this.btnAgregar.Visible = this.ValidarPermiso("BancosCuentasMovimientosAgregar.aspx");
            this.btnAgregarMultiplesMovimientos.Visible = this.ValidarPermiso("BancosCuentasMovimientosMultiplesAgregar.aspx");
            this.botoneraAgregar.Visible = this.btnAgregar.Visible || this.btnAgregarMultiplesMovimientos.Visible;
        }
        private void MapearObjetoAControles(TESBancosCuentas pBancoCuenta)
        {
            this.txtDenominacion.Text = pBancoCuenta.Denominacion;
            this.txtImporteDescubierto.Text = pBancoCuenta.ImporteDescubierto.ToString();
            this.txtNumeroBancoSucursal.Text = pBancoCuenta.NumeroBancoSucursal;
            this.txtNumeroCuenta.Text = pBancoCuenta.NumeroCuenta;
            this.txtBanco.Text = pBancoCuenta.Banco.Descripcion;
            this.txtTipoCuenta.Text = pBancoCuenta.BancoCuentaTipo.Descripcion;
            this.txtFilial.Text = pBancoCuenta.Filial.Filial;
            this.txtEstado.Text = pBancoCuenta.Estado.Descripcion;
            this.txtMoneda.Text = pBancoCuenta.Moneda.Moneda;
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                DataRow movimiento = ((DataRowView)e.Row.DataItem).Row;
                ImageButton btnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton imprimir = (ImageButton)e.Row.FindControl("btnImprimir");
                imprimir.Visible = false;
                btnConsultar.Visible = this.ValidarPermiso("BancosCuentasMovimientosConsultar.aspx");
                //Muestro Comprobantes de Orden Pago y OrdenCobro
                if (Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.OrdenesPagos
                    || Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.AhorroDepositos
                    || Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.OrdenesCobrosVarios
                    || Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas
                    || Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.OrdenesCobrosAfiliados
                    || Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.DepositosCuentasBancarias
                    || Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.PrestamosLargoPlazo
                    || Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.PrestamosCortoPlazo
                    || Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.PrestamosLargoPlazoCancelacion
                    || Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.PrestamosCortoPlazoCancelacion
                    || Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.PrestamosFondosPropios
                    || Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.PrestamosFondosPropiosCancelacion
                    || Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.CompraDeCheque
                    || Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.PrestamosManual
                    //AGREGO LOS MOVIMIENTOS INTERNOS
                    || Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.IngresosBancos
                    || Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.EgresosBancos
                    || Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasDebito
                    || Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasCredito
                    || Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.TransferenciaTesoreriaCredito
                    || Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]) == (int)EnumTGETiposOperaciones.TransferenciaTesoreriaDebito
                    )
                    imprimir.Visible = true;
                //if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasCredito)
                //{
                //    btnConsultar.Visible = false;
                //}

                //Label fechaBanco = (Label)e.Row.FindControl("lblFechaBanco");
                //if (Convert.ToInt32(movimiento["EstadoIdEstado"]) == (int)EstadosBancosCuentasMovimientos.PendienteConciliacion)
                //    fechaBanco.Visible = false;
                //else
                //    fechaBanco.Text = Convert.ToDateTime(movimiento["FechaConfirmacionBanco"]).ToShortDateString();

                if (Convert.ToInt32(movimiento["EstadoIdEstado"]) == (int)EstadosBancosCuentasMovimientos.Confirmado)
                {
                    ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                    btnEliminar.Visible = this.ValidarPermiso("AnularMovimientoConfirmado.aspx");

                    string codigoMsg = "BancosCuentasMovimientosConfirmarBaja";
                    if (movimiento["TESCajasMovimientosIdRefTipoOperacion"] != DBNull.Value)
                        codigoMsg = "BancosCuentasMovimientosConfirmarDesconciliar";

                    string mensaje = this.ObtenerMensajeSistema(codigoMsg);
                    mensaje = string.Format(mensaje, movimiento["IdBancoCuentaMovimiento"]);
                    string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                    btnEliminar.Attributes.Add("OnClick", funcion);
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotalImporte = (Label)e.Row.FindControl("lblTotalImporte");
                lblTotalImporte.Text = this.MisDatosGrilla.Rows.Count > 1 ? Convert.ToDecimal(this.MisDatosGrilla.Rows[0]["Saldo"]).ToString("C2") : (0).ToString("C2");
                Label lblCantidadRegistros = (Label)e.Row.FindControl("lblCantidadRegistros");
                lblCantidadRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisDatosGrilla.Rows.Count);
            }
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Impresion.ToString()
                || e.CommandName == "Borrar"))
                return;

            TESBancosCuentas parametros = this.BusquedaParametrosObtenerValor<TESBancosCuentas>();
            parametros.HashTransaccion = 0;
            this.BusquedaParametrosGuardarValor<TESBancosCuentas>(parametros);

            int index = Convert.ToInt32(e.CommandArgument);
            int indice = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TESBancosCuentasMovimientos movimiento = new TESBancosCuentasMovimientos();
            DataRow row = this.MisDatosGrilla.Select("IdBancoCuentaMovimiento=" + indice)[0];
            Servicio.AccesoDatos.Mapeador.SetearEntidadPorFila(row, movimiento);

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                TGETiposOperacionesFiltros tipoOp = new TGETiposOperacionesFiltros();
                tipoOp.IdTipoOperacion = movimiento.TipoOperacion.IdTipoOperacion;// Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdTipoOperacion")).Value);
                tipoOp.IdRefTipoOperacion = movimiento.TESCajasMovimientosIdRefTipoOperacion;// Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdRefTipoOperacion")).Value);
                int IdAfiliado = AfiliadosF.AfiliadosObtenerPorTipoOperacionRefTipoOperacion(tipoOp).IdAfiliado; //movimiento.Afiliado.IdAfiliado;// Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdAfiliado")).Value);
                //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

                this.MisParametrosUrl = new Hashtable();
                this.MisParametrosUrl.Add("IdBancoCuenta", movimiento.BancoCuenta.IdBancoCuenta);
                //Filtro para Obtener URL y NombreParametro
                Menues filtroMenu = new Menues();
                filtroMenu.IdTipoOperacion = tipoOp.IdTipoOperacion;
                this.MisParametrosUrl.Add("IdTipoOperacion", filtroMenu.IdTipoOperacion);
                //Control de Tipo de Menues (SOLO CONSULTA)
                if (e.CommandName == Gestion.Consultar.ToString())
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Consulta;

                //Guardo Menu devuelto de la DB
                filtroMenu = SeguridadF.MenuesObtenerPorOperacionTipoMenu(filtroMenu);

                if (filtroMenu.IdMenu > 0)
                {
                    this.MisParametrosUrl.Add(filtroMenu.NombreParametro, tipoOp.IdRefTipoOperacion);
                    //Si devuelve una URL Redirecciona si no muestra mensaje error
                    if (filtroMenu.URL.Length != 0)
                    {
                        if (IdAfiliado > 0)
                        {
                            AfiAfiliados afi = new AfiAfiliados();
                            afi.IdAfiliado = IdAfiliado;
                            PaginaAfiliados paginaAfi = new PaginaAfiliados();
                            paginaAfi.Guardar(this.MiSessionPagina, AfiliadosF.AfiliadosObtenerDatos(afi));
                        }
                        this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(string.Concat("~/", filtroMenu.URL)), true);
                    }
                    else
                    {
                        this.MostrarMensaje("ErrorURLNoValida", true);
                    }
                }
                else
                {
                    if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasCredito)
                        this.MisParametrosUrl.Add("IdBancoCuentaMovimiento", movimiento.IdRefTipoOperacion);
                    else
                        this.MisParametrosUrl.Add("IdBancoCuentaMovimiento", movimiento.IdBancoCuentaMovimiento);

                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasMovimientosConsultar.aspx"), true);
                }
            }
            else if (e.CommandName == "Borrar")
            {
                movimiento.Estado = TGEGeneralesF.TGEEstadosObtener(new TGEEstados() { IdEstado = (int)EstadosBancosCuentasMovimientos.Baja });
                movimiento.EstadoColeccion = EstadoColecciones.Modificado;
                movimiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

                if (BancosF.BancosCuentasMovimientosAnularConfirmado(movimiento))
                {
                    this.MostrarMensaje(movimiento.CodigoMensaje, false);
                    this.btnBuscar_Click(null, EventArgs.Empty);
                    this.btnBuscarPendientes_Click(null, EventArgs.Empty);
                }
                else
                {
                    this.MostrarMensaje(movimiento.CodigoMensaje, true, movimiento.CodigoMensajeArgs);
                }
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                 #region Comprobante Ordenes Cobros
                if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosVarios
                    || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas
                    )
                {
                    CobOrdenesCobros pReporte = new CobOrdenesCobros();
                    pReporte.IdOrdenCobro = movimiento.TESCajasMovimientosIdRefTipoOperacion;

                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CobOrdenesCobros, "OrdenesCobros", pReporte, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.Page, "BancoCuentaMovimiento_" + movimiento.IdBancoCuentaMovimiento.ToString() + "_" + this.UsuarioActivo.ApellidoNombre, this.UsuarioActivo);

                    this.upMovimientos.Update();
                }
                if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosAfiliados)
                {
                    CobOrdenesCobros pReporte = new CobOrdenesCobros();
                    pReporte.IdOrdenCobro = movimiento.TESCajasMovimientosIdRefTipoOperacion;

                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CobOrdenesCobrosAfiliados, "OrdenesCobrosAfiliados", pReporte, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.Page, "BancoCuentaMovimiento_" + movimiento.IdBancoCuentaMovimiento.ToString() + "_" + this.UsuarioActivo.ApellidoNombre, this.UsuarioActivo);

                    this.upMovimientos.Update();
                }
                #endregion
                #region Comprobante Ordenes Pagos
                else if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesPagos
                    || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesPagosInterno)
                {
                    CapOrdenesPagos pReporte = new CapOrdenesPagos();
                    pReporte.IdOrdenPago = movimiento.TESCajasMovimientosIdRefTipoOperacion;
                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CapOrdenesPagos, "OrdenesPago", pReporte, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.Page, "BancoCuentaMovimiento_" + movimiento.IdBancoCuentaMovimiento.ToString() + "_" + this.UsuarioActivo.ApellidoNombre, this.UsuarioActivo);

                    this.upMovimientos.Update();
                }
                #endregion
                #region Comprobante Ahorros Depositos
                else if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.AhorroDepositos
                        || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.DepositosCuentasBancarias)
                {
                    //Preguntar por Ahorro Depositos ver que entidad y comprobante usar (?) AhoCuentasMovimientos, AhoPlazosFijos, AhoCuentasDetallesMovimientos
                    AhoCuentasMovimientos pReporte = new AhoCuentasMovimientos();
                    pReporte.IdCuentaMovimiento = movimiento.TESCajasMovimientosIdRefTipoOperacion;
                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.AhoCuentasMovimientos, "AhorroCuentasMovimientos", pReporte, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.Page, "BancoCuentaMovimiento_" + movimiento.IdBancoCuentaMovimiento.ToString() + "_" + this.UsuarioActivo.ApellidoNombre, this.UsuarioActivo);

                    this.upMovimientos.Update();

                }
                #endregion
                #region Comprobante Prestamos
                else if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosCortoPlazo
                        || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosCortoPlazoCancelacion
                        || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosLargoPlazo
                        || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosLargoPlazoCancelacion
                        || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosFondosPropios
                        || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosFondosPropiosCancelacion
                        || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.CompraDeCheque
                        || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosManual
                    )
                {
                    //Preguntar por Ahorro Depositos ver que entidad y comprobante usar (?) AhoCuentasMovimientos, AhoPlazosFijos, AhoCuentasDetallesMovimientos
                    PrePrestamos pReporte = new PrePrestamos();
                    pReporte.IdPrestamo = movimiento.TESCajasMovimientosIdRefTipoOperacion;
                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.PrePrestamos, "PrestamosSolicitudOtorgamiento", pReporte, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.Page, "BancoCuentaMovimiento_" + movimiento.IdBancoCuentaMovimiento.ToString() + "_" + this.UsuarioActivo.ApellidoNombre, this.UsuarioActivo);

                    this.upMovimientos.Update();

                }
                #endregion
                #region Movimientos Internos
                else if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.IngresosBancos
                    || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.EgresosBancos
                    || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasDebito
                    || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasCredito
                    || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaTesoreriaDebito
                    || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaTesoreriaCredito
                    )
                {

                    //this.ctrPopUpComprobantes.CargarReporte(movimiento, EnumTGEComprobantes.TesBancosMovimientosInternos);
                    //this.upMovimientos.Update();
                    try
                    {
                        RepReportes reporte = new RepReportes();
                        string parametro = "";
                        if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasCredito
                            || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaTesoreriaCredito)
                            parametro = movimiento.IdRefTipoOperacion.ToString();
                        else
                            parametro = movimiento.IdBancoCuentaMovimiento.ToString();


                        RepParametros param = new RepParametros();
                        param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.TextBox;
                        param.Parametro = "IdBancoCuentaMovimiento";
                        param.ValorParametro = parametro;
                        reporte.Parametros.Add(param);

                        param = new RepParametros();
                        param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.TextBox;
                        param.Parametro = "IdTipoOperacion";
                        param.ValorParametro = movimiento.TipoOperacion.IdTipoOperacion;
                        reporte.Parametros.Add(param);

                        param = new RepParametros();
                        param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.TextBox;
                        param.Parametro = "IdRefTipoOperacion";
                        param.ValorParametro = movimiento.IdRefTipoOperacion.ToString();
                        reporte.Parametros.Add(param);

                        param = new RepParametros();
                        param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
                        param.Parametro = "FechaDesde";
                        param.ValorParametro = Convert.ToDateTime(this.txtFechaDesde.Text);
                        reporte.Parametros.Add(param);
                        param = new RepParametros();
                        param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
                        param.Parametro = "FechaHasta";
                        param.ValorParametro = Convert.ToDateTime(this.txtFechaHasta.Text);
                        reporte.Parametros.Add(param);
                        param = new RepParametros();
                        param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                        param.Parametro = "IdEstado";
                        param.ValorParametro = movimiento.Estado.IdEstado;
                        reporte.Parametros.Add(param);

                        TGEPlantillas plantilla = new TGEPlantillas();
                        plantilla.Codigo = "TESBancosCuentasMovimientos";
                        plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

                        reporte.StoredProcedure = plantilla.NombreSP;


                        DataSet ds = ReportesF.ReportesObtenerDatos(reporte);


                        byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(plantilla, ds, "IdBancoCuentaMovimiento", AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                        ExportPDF.ExportarPDF(pdf, this.Page, "BancoCuentaMovimiento_" + movimiento.IdBancoCuentaMovimiento.ToString() + "_" + this.UsuarioActivo.ApellidoNombre, this.UsuarioActivo);
                        this.upMovimientos.Update();
                    }
                    catch (Exception ex)
                    {
                        this.MostrarMensaje("No se pudo imprimir el comprobante.", true);
                    }
                }
                //else if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasCredito
                //    || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaTesoreriaCredito
                //    )
                //{
                //    TESBancosCuentasMovimientos reporte = new TESBancosCuentasMovimientos();
                //    reporte.IdBancoCuentaMovimiento = movimiento.IdRefTipoOperacion;
                //    this.ctrPopUpComprobantes.CargarReporte(reporte, EnumTGEComprobantes.TesBancosMovimientosInternos);
                //    this.upMovimientos.Update();
                //}
                #endregion
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //this.gvDatos.PageIndex = e.NewPageIndex;
            //TESBancosCuentas parametros = this.BusquedaParametrosObtenerValor<TESBancosCuentas>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //parametros.HashTransaccion = 0;
            //this.BusquedaParametrosGuardarValor<TESBancosCuentas>(parametros);
            //this.gvDatos.DataSource = this.MisDatosGrilla;
            //this.gvDatos.DataBind();
            ////AyudaProgramacion.CargarGrillaListas<TESBancosCuentasMovimientos>(this.MiBancoCuenta.BancosCuentasMovimientos, false, this.gvDatos, true);
            this.MiBancoCuenta = this.BusquedaParametrosObtenerValor<TESBancosCuentas>();
            this.gvDatos.PageIndex = e.NewPageIndex;
            this.MiBancoCuenta.PageIndex = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor(this.MiBancoCuenta);
            this.btnBuscar_Click(null, EventArgs.Empty);
        }
        protected void gvPendientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                //TESBancosCuentasMovimientos movimiento = (TESBancosCuentasMovimientos)e.Row.DataItem;
                DataRowView movimiento = (DataRowView)e.Row.DataItem;
                bool permisoConciliar = this.ValidarPermiso("BancosCuentasMovimientosConciliar.aspx");
                DropDownList estado = (DropDownList)e.Row.FindControl("ddlEstados");
                ImageButton imprimir = (ImageButton)e.Row.FindControl("btnImprimir");
                TextBox txtFecha = (TextBox)e.Row.FindControl("txtFechaBanco");
                imprimir.Visible = false;

                int idTipoOperacion = Convert.ToInt32(movimiento["TipoOperacionIdTipoOperacion"]);
                int idEstado = Convert.ToInt32(movimiento["EstadoIdEstado"]);
                int idBancoCuentaMovimiento = Convert.ToInt32(movimiento["IdBancoCuentaMovimiento"]);
                //Muestro Comprobantes de Orden Pago y OrdenCobro
                if (idTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesPagos
                    || idTipoOperacion == (int)EnumTGETiposOperaciones.AhorroDepositos
                    || idTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosVarios
                    || idTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas
                    || idTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosAfiliados
                    || idTipoOperacion == (int)EnumTGETiposOperaciones.DepositosCuentasBancarias
                    || idTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosLargoPlazo
                    || idTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosCortoPlazo
                    || idTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosLargoPlazoCancelacion
                    || idTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosCortoPlazoCancelacion
                    || idTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosFondosPropios
                    || idTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosFondosPropiosCancelacion
                    || idTipoOperacion == (int)EnumTGETiposOperaciones.CompraDeCheque
                    || idTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosManual
                    //AGREGO LOS MOVIMIENTOS INTERNOS
                    || idTipoOperacion == (int)EnumTGETiposOperaciones.IngresosBancos
                    || idTipoOperacion == (int)EnumTGETiposOperaciones.EgresosBancos
                    || idTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasDebito
                    || idTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasCredito
                    || idTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaTesoreriaCredito
                    || idTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaTesoreriaDebito
                    )
                    imprimir.Visible = true;
                else
                {
                    imprimir.Visible = false;
                }
                //Anulación de Movimiento Pendiente de Conciliación (Transferencia)

                if (
                    (idTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasDebito
                    && idEstado == (int)EstadosBancosCuentasMovimientos.PendienteConciliacion
                    && this.ValidarPermiso("BancosCuentasMovimientosAgregar.aspx"))
                    ||
                    (idTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaTesoreriaDebito
                    && idEstado == (int)EstadosBancosCuentasMovimientos.PendienteConfirmacion)
                    )
                {
                    ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                    btnEliminar.Visible = true;

                    string mensaje = this.ObtenerMensajeSistema("BancosCuentasMovimientosConfirmarBaja");
                    mensaje = string.Format(mensaje, idBancoCuentaMovimiento);
                    string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                    btnEliminar.Attributes.Add("OnClick", funcion);
                    permisoConciliar = false;

                    estado.DataSource = this.MisEstados;
                    estado.DataValueField = "IdEstado";
                    estado.DataTextField = "Descripcion";
                    estado.DataBind();
                    ListItem item = estado.Items.FindByValue(((int)EstadosBancosCuentasMovimientos.PendienteConfirmacion).ToString());
                    if (item != null)
                        estado.Items.Remove(item);
                    item = estado.Items.FindByValue(idEstado.ToString());
                    if (item == null)
                        estado.Items.Add(new ListItem(movimiento["EstadoDescripcion"].ToString(), idEstado.ToString()));
                    estado.SelectedValue = idEstado.ToString();
                    estado.Visible = permisoConciliar;
                    txtFecha.Visible = permisoConciliar;
                    estado.Enabled = permisoConciliar;

                }
                else if (
                    (idTipoOperacion == (int)EnumTGETiposOperaciones.DepositosCuentasBancarias
                    && idEstado == (int)EstadosBancosCuentasMovimientos.PendienteConciliacion)
                    )
                {
                    ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                    btnEliminar.Visible = true;
                    string mensaje = this.ObtenerMensajeSistema("BancosCuentasMovimientosConfirmarBaja");
                    mensaje = string.Format(mensaje, idBancoCuentaMovimiento);
                    string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                    btnEliminar.Attributes.Add("OnClick", funcion);

                    estado.DataSource = this.MisEstados;
                    estado.DataValueField = "IdEstado";
                    estado.DataTextField = "Descripcion";
                    estado.DataBind();
                    ListItem item = estado.Items.FindByValue(((int)EstadosBancosCuentasMovimientos.PendienteConfirmacion).ToString());
                    if (item != null)
                        estado.Items.Remove(item);
                    item = estado.Items.FindByValue(idEstado.ToString());
                    if (item == null)
                        estado.Items.Add(new ListItem(movimiento["EstadoDescripcion"].ToString(), idEstado.ToString()));
                    estado.SelectedValue = idEstado.ToString();
                    estado.Enabled = permisoConciliar;
                }
                else
                {
                    estado.DataSource = this.MisEstados;
                    estado.DataValueField = "IdEstado";
                    estado.DataTextField = "Descripcion";
                    estado.DataBind();
                    ListItem item = estado.Items.FindByValue(((int)EstadosBancosCuentasMovimientos.PendienteConciliacion).ToString());
                    if (item != null)
                        estado.Items.Remove(item);
                    item = estado.Items.FindByValue(idEstado.ToString());
                    if (item == null)
                        estado.Items.Add(new ListItem(movimiento["EstadoDescripcion"].ToString(), idEstado.ToString()));
                    estado.SelectedValue = idEstado.ToString();
                    estado.Enabled = permisoConciliar;
                }

                //Conciliación de Movimientos Pendientes
                //CheckBox chkConciliar = (CheckBox)e.Row.FindControl("chkConciliar");
                //chkConciliar.Checked = (int)EstadosBancosCuentasMovimientos.Confirmado == movimiento.Estado.IdEstado;
                TextBox txtFechaBanco = (TextBox)e.Row.FindControl("txtFechaBanco");
                string funcionFechaBanco = " ModificarFechaBancoEstado(this);";
                txtFechaBanco.Attributes.Add("OnChange", funcionFechaBanco);

                //HtmlControl dvFechaBanco = (HtmlControl)e.Row.FindControl("dvFechaBanco");
                //dvFechaBanco.Visible = this.MiPermisoConciliar;

                txtFechaBanco.Enabled = this.MiPermisoConciliar;
                //txtFechaBanco.Text = DateTime.Now.ToShortDateString();
                //chkConciliar.Enabled = this.MiPermisoConciliar;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblCantidadRegistros = (Label)e.Row.FindControl("lblCantidadRegistros");
                lblCantidadRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisDatosGrillaPendientes.Rows.Count);
                Label lblImporte = (Label)e.Row.FindControl("lblImporte");
                lblImporte.Text = Convert.ToDecimal(this.MisDatosGrillaPendientes.Compute("Sum(Importe)", string.Empty)).ToString("C2");
            }
        }
        protected void gvPendientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            this.PersistirDatosGrillaPendiente();

            if (!(e.CommandName == "Borrar"
                || e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Impresion.ToString()))
                return;

            TESBancosCuentas parametros = this.BusquedaParametrosObtenerValor<TESBancosCuentas>();
            parametros.HashTransaccion = 1;
            this.BusquedaParametrosGuardarValor<TESBancosCuentas>(parametros);

            int index = Convert.ToInt32(e.CommandArgument);
            int idBancoCuentaMovimiento = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            DataRow row = this.MisDatosGrillaPendientes.Select(" IdBancoCuentaMovimiento = " + idBancoCuentaMovimiento)[0];
            TESBancosCuentasMovimientos movimiento = new TESBancosCuentasMovimientos();
            Servicio.AccesoDatos.Mapeador.SetearEntidadPorfila(row, movimiento, EstadoColecciones.SinCambio);
            //TESBancosCuentasMovimientos movimiento = this.MiBancoCuenta.BancosCuentasMovimientosPendientes[indice];

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                int IdTipoOperacion = movimiento.TipoOperacion.IdTipoOperacion;// Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdTipoOperacion")).Value);
                int IdRefTipoOperacion = movimiento.TESCajasMovimientosIdRefTipoOperacion;// Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdRefTipoOperacion")).Value);
                int IdAfiliado = movimiento.Afiliado.IdAfiliado;// Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdAfiliado")).Value);
                //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

                this.MisParametrosUrl = new Hashtable();
                this.MisParametrosUrl.Add("IdBancoCuenta", movimiento.BancoCuenta.IdBancoCuenta);
                //Filtro para Obtener URL y NombreParametro
                Menues filtroMenu = new Menues();
                filtroMenu.IdTipoOperacion = IdTipoOperacion;
                //Control de Tipo de Menues (SOLO CONSULTA)
                if (e.CommandName == Gestion.Consultar.ToString())
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Consulta;

                //Guardo Menu devuelto de la DB
                filtroMenu = SeguridadF.MenuesObtenerPorOperacionTipoMenu(filtroMenu);

                if (filtroMenu.IdMenu > 0)
                {
                    this.MisParametrosUrl.Add(filtroMenu.NombreParametro, IdRefTipoOperacion);
                    //Si devuelve una URL Redirecciona si no muestra mensaje error
                    if (filtroMenu.URL.Length != 0)
                    {
                        if (IdAfiliado > 0)
                        {
                            AfiAfiliados afi = new AfiAfiliados();
                            afi.IdAfiliado = IdAfiliado;
                            PaginaAfiliados paginaAfi = new PaginaAfiliados();
                            paginaAfi.Guardar(this.MiSessionPagina, AfiliadosF.AfiliadosObtenerDatos(afi));
                        }
                        this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(string.Concat("~/", filtroMenu.URL)), true);
                    }
                    else
                    {
                        this.MostrarMensaje("ErrorURLNoValida", true);
                    }
                }
                else
                {
                    if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasCredito)
                        this.MisParametrosUrl.Add("IdBancoCuentaMovimiento", movimiento.IdRefTipoOperacion);
                    else
                        this.MisParametrosUrl.Add("IdBancoCuentaMovimiento", movimiento.IdBancoCuentaMovimiento);

                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasMovimientosConsultar.aspx"), true);
                }
            }
            else if (e.CommandName == "Borrar")
            {
                movimiento.Estado = TGEGeneralesF.TGEEstadosObtener(new TGEEstados() { IdEstado = (int)EstadosBancosCuentasMovimientos.Baja });
                movimiento.EstadoColeccion = EstadoColecciones.Modificado;
                movimiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

                if (BancosF.BancosCuentasMovimientosAnular(movimiento))
                {
                    this.MostrarMensaje(movimiento.CodigoMensaje, false);
                    this.btnBuscarPendientes_Click(null, EventArgs.Empty);
                }
                else
                {
                    this.MostrarMensaje(movimiento.CodigoMensaje, true);
                }
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                #region Comprobante Ordenes Cobros
                if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosVarios
                    || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas
                    || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosAfiliados)
                {
                    CobOrdenesCobros pReporte = new CobOrdenesCobros();
                    pReporte.IdOrdenCobro = movimiento.TESCajasMovimientosIdRefTipoOperacion;
                    this.ctrPopUpComprobantes.CargarReporte(pReporte, EnumTGEComprobantes.CobOrdenesCobros);
                    this.upMovimientos.Update();
                }
                #endregion
                #region Comprobante Ordenes Pagos
                else if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesPagos
                    || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesPagosInterno)
                {
                    CapOrdenesPagos pReporte = new CapOrdenesPagos();
                    pReporte.IdOrdenPago = movimiento.TESCajasMovimientosIdRefTipoOperacion;
                    this.ctrPopUpComprobantes.CargarReporte(pReporte, EnumTGEComprobantes.CapOrdenesPagos);
                    this.upMovimientos.Update();
                }
                #endregion
                #region Comprobante Ahorros Depositos
                else if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.AhorroDepositos
                        || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.DepositosCuentasBancarias)
                {
                    //Preguntar por Ahorro Depositos ver que entidad y comprobante usar (?) AhoCuentasMovimientos, AhoPlazosFijos, AhoCuentasDetallesMovimientos
                    AhoCuentasMovimientos pReporte = new AhoCuentasMovimientos();
                    pReporte.IdCuentaMovimiento = movimiento.TESCajasMovimientosIdRefTipoOperacion;
                    this.ctrPopUpComprobantes.CargarReporte(pReporte, EnumTGEComprobantes.AhoCuentasMovimientos);
                    this.upMovimientos.Update();

                }
                #endregion
                #region Comprobante Prestamos
                else if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosCortoPlazo
                        || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosCortoPlazoCancelacion
                        || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosLargoPlazo
                        || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosLargoPlazoCancelacion
                        || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosFondosPropios
                        || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosFondosPropiosCancelacion
                        || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.CompraDeCheque
                        || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosManual
                    )
                {
                    //Preguntar por Ahorro Depositos ver que entidad y comprobante usar (?) AhoCuentasMovimientos, AhoPlazosFijos, AhoCuentasDetallesMovimientos
                    PrePrestamos pReporte = new PrePrestamos();
                    pReporte.IdPrestamo = movimiento.TESCajasMovimientosIdRefTipoOperacion;
                    this.ctrPopUpComprobantes.CargarReporte(pReporte, EnumTGEComprobantes.PrePrestamos);
                    this.upMovimientos.Update();

                }
                #endregion
                #region Movimientos Internos
                else if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.IngresosBancos
                    || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.EgresosBancos
                    || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasDebito
                    || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaTesoreriaDebito
                    )
                {
                    this.ctrPopUpComprobantes.CargarReporte(movimiento, EnumTGEComprobantes.TesBancosMovimientosInternos);
                    this.upMovimientos.Update();
                }
                else if (movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasCredito
                    || movimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaTesoreriaCredito
                    )
                {
                    TESBancosCuentasMovimientos reporte = new TESBancosCuentasMovimientos();
                    reporte.IdBancoCuentaMovimiento = movimiento.IdRefTipoOperacion;
                    this.ctrPopUpComprobantes.CargarReporte(reporte, EnumTGEComprobantes.TesBancosMovimientosInternos);
                    this.upMovimientos.Update();
                }
                #endregion
            }
        }
        protected void gvPendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //this.PersistirDatosGrillaPendiente();
            //TESBancosCuentas parametros = this.BusquedaParametrosObtenerValor<TESBancosCuentas>();
            //parametros.Filial.IndiceColeccion = e.NewPageIndex;
            //parametros.HashTransaccion = 1;
            //this.BusquedaParametrosGuardarValor<TESBancosCuentas>(parametros);
            //this.gvPendientes.PageIndex = e.NewPageIndex;
            //this.gvPendientes.DataSource = this.MisDatosGrillaPendientes;
            //this.gvPendientes.DataBind();
            this.MiBancoCuenta = this.BusquedaParametrosObtenerValor<TESBancosCuentas>();
            this.gvPendientes.PageIndex = e.NewPageIndex;
            this.MiBancoCuenta.PageIndex = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor(this.MiBancoCuenta);
            this.btnBuscarPendientes_Click(null, EventArgs.Empty);
        }

        protected void gvRechazados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ////ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                //TESBancosCuentasMovimientos movimiento = (TESBancosCuentasMovimientos)e.Row.DataItem;

                //Label fechaBanco = (Label)e.Row.FindControl("lblFechaBanco");
                //if (movimiento.Estado.IdEstado == (int)EstadosBancosCuentasMovimientos.PendienteConciliacion)
                //    fechaBanco.Visible = false;
                //else
                //    fechaBanco.Text = movimiento.FechaConfirmacionBanco.ToShortDateString();

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //Label lblOperativo = (Label)e.Row.FindControl("lblSaldoOperativoValor");
                //lblOperativo.Text = this.MiBancoCuenta.BancosCuentasMovimientos.Where(x => x.Estado.IdEstado == (int)EstadosBancosCuentasMovimientos.Confirmado).ToList().Sum(x => x.Importe).ToString("C2");
                //Label lblFinanciero = (Label)e.Row.FindControl("lblSaldoFinancieroValor");
                //lblFinanciero.Text = this.MiBancoCuenta.BancosCuentasMovimientos.Sum(x => x.Importe).ToString("C2");
            }
        }

        protected void gvRechazados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //TESBancosCuentas parametros = this.BusquedaParametrosObtenerValor<TESBancosCuentas>();
            //parametros.Filial.IndiceColeccion = e.NewPageIndex;
            //parametros.HashTransaccion = 1;
            //this.BusquedaParametrosGuardarValor<TESBancosCuentas>(parametros);
            //this.gvRechazados.PageIndex = e.NewPageIndex;
            //this.gvRechazados.DataSource = this.MisDatosGrillaRechazados;
            //this.gvRechazados.DataBind();

            this.gvRechazados.PageIndex = e.NewPageIndex;
            this.MiBancoCuenta.PageIndex = e.NewPageIndex;
            this.btnBuscar_Click(null, EventArgs.Empty);
        }
        private void PersistirDatosGrillaPendiente()
        {
            TESBancosCuentasMovimientos movimiento;
            DataRow rowMovimiento;
            //CheckBox chkConciliar;
            DateTime fechaBanco;
            //bool validarItems = false;
            foreach (GridViewRow fila in this.gvPendientes.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    //chkConciliar = (CheckBox)fila.FindControl("chkConciliar");
                    //if (chkConciliar.Enabled && chkConciliar.Checked)
                    DropDownList estado = (DropDownList)fila.FindControl("ddlEstados");
                    //movimiento = this.MiBancoCuenta.BancosCuentasMovimientosPendientes[fila.DataItemIndex];
                    rowMovimiento = this.MisDatosGrillaPendientes.Rows[fila.DataItemIndex];
                    if (estado.SelectedValue != rowMovimiento["EstadoIdEstado"].ToString())// movimiento.Estado.IdEstado.ToString())
                    {
                        //validarItems = true;
                        TextBox txtFechaBanco = (TextBox)fila.FindControl("txtFechaBanco");
                        DateTime.TryParse(txtFechaBanco.Text, out fechaBanco);
                        //if (txtFechaBanco.Text == string.Empty || !DateTime.TryParse(txtFechaBanco.Text, out fechaBanco))
                        //{
                        //    List<string> listaMsg = new List<string>();
                        //    listaMsg.Add(rowMovimiento["NumeroTipoOperacion"].ToString());
                        //    this.MostrarMensaje(this.ObtenerMensajeSistema("ValidarFechaBanco"), true, listaMsg);
                        //    return;
                        //}
                        if (this.MiBancoCuenta.BancosCuentasMovimientosPendientes.Exists(x => x.IdBancoCuentaMovimiento == Convert.ToInt32(rowMovimiento["IdBancoCuentaMovimiento"])))
                            movimiento = this.MiBancoCuenta.BancosCuentasMovimientosPendientes.FirstOrDefault(x => x.IdBancoCuentaMovimiento == Convert.ToInt32(rowMovimiento["IdBancoCuentaMovimiento"]));
                        else
                        {
                            movimiento = new TESBancosCuentasMovimientos();
                            this.MiBancoCuenta.BancosCuentasMovimientosPendientes.Add(movimiento);
                        }
                        Servicio.AccesoDatos.Mapeador.SetearEntidadPorfila(rowMovimiento, movimiento, EstadoColecciones.SinCambio);
                        movimiento.EstadoColeccion = EstadoColecciones.Modificado;
                        movimiento.Estado.IdEstado = Convert.ToInt32(estado.SelectedValue); //this.MiEstadoConfirmado;
                        movimiento.Estado.Descripcion = estado.SelectedItem.Text;
                        movimiento.FechaConfirmacionBanco = fechaBanco;
                        movimiento.FechaConciliacion = DateTime.Now;
                        if (rowMovimiento["MonedaCotizacion"].ToString() != string.Empty)
                        {
                            movimiento.BancoCuenta.Moneda.MonedeaCotizacion.MonedaCotizacion = Convert.ToDecimal(rowMovimiento["MonedaCotizacion"]);
                        }
                    }
                }
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.MiBancoCuenta = this.BusquedaParametrosObtenerValor<TESBancosCuentas>();
            //Lo uso para setear el MultiTab
            this.MiBancoCuenta.HashTransaccion = 0;
            this.MiBancoCuenta.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<TESBancosCuentas>(this.MiBancoCuenta);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasMovimientosAgregar.aspx"), true);
        }
        protected void btnAgregarMultiplesMovimientos_Click(object sender, EventArgs e)
        {
            this.MiBancoCuenta = this.BusquedaParametrosObtenerValor<TESBancosCuentas>();
            //Lo uso para setear el MultiTab
            this.MiBancoCuenta.HashTransaccion = 0;
            this.MiBancoCuenta.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<TESBancosCuentas>(this.MiBancoCuenta);

            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Bancos/BancosCuentasMovimientosMultiplesAgregar.aspx"), true);
        }
        protected void btnConciliar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            //if (!this.Page.IsValid)
            //    return;
            this.PersistirDatosGrillaPendiente();

            if (this.MiBancoCuenta.BancosCuentasMovimientosPendientes.Exists(x => x.Estado.IdEstado == (int)EstadosBancosCuentasMovimientos.Confirmado
                && x.FechaConfirmacionBanco == default(DateTime)))
            {

                List<string> listaMsg = new List<string>();
                listaMsg.Add(this.MiBancoCuenta.BancosCuentasMovimientosPendientes.First(x => x.Estado.IdEstado == (int)EstadosBancosCuentasMovimientos.Confirmado
                && x.FechaConfirmacionBanco == default(DateTime)).Detalle);
                this.MostrarMensaje(this.ObtenerMensajeSistema("ValidarFechaBanco"), true, listaMsg);
                return;
            }

            this.MiBancoCuenta.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            guardo = BancosF.BancosCuentasActualizarMovimientosPendientes(this.MiBancoCuenta);

            if (guardo)
            {
                this.MostrarMensaje(this.ObtenerMensajeSistema(this.MiBancoCuenta.CodigoMensaje), false);
                this.btnBuscar_Click(null, EventArgs.Empty);
                this.btnBuscarPendientes_Click(null, EventArgs.Empty);
                this.upMovimientos.Update();
            }
            else
            {
                this.MostrarMensaje(this.MiBancoCuenta.CodigoMensaje, true, this.MiBancoCuenta.CodigoMensajeArgs);
                if (this.MiBancoCuenta.dsResultado != null)
                {
                    this.ctrPopUpGrilla.IniciarControl(this.MiBancoCuenta);
                    this.MiBancoCuenta.dsResultado = null;
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.MiBancoCuenta = this.BusquedaParametrosObtenerValor<TESBancosCuentas>();
            this.MiBancoCuenta.FechaDesde = Convert.ToDateTime(this.txtFechaDesde.Text);
            this.MiBancoCuenta.FechaHasta = Convert.ToDateTime(this.txtFechaHasta.Text);
            this.MiBancoCuenta.Detalle = this.txtDetalle.Text.Trim();
            this.MiBancoCuenta.HashTransaccion = 0; //Lo uso para setear el MultiTab
            this.MiBancoCuenta.BusquedaParametros = true;
            this.MiBancoCuenta.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize);

            this.BusquedaParametrosGuardarValor<TESBancosCuentas>(this.MiBancoCuenta);
            this.MisDatosGrilla = BancosF.BancosCuentasObtenerMovimientos(this.MiBancoCuenta);
            this.gvDatos.DataSource = this.MisDatosGrilla;
            this.gvDatos.VirtualItemCount = MisDatosGrilla.Rows.Count > 0 ? Convert.ToInt32(MisDatosGrilla.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + gvDatos.VirtualItemCount.ToString();
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);

            this.MisDatosGrillaRechazados = BancosF.BancosCuentasObtenerMovimientosRechazados(this.MiBancoCuenta);
            this.gvRechazados.DataSource = this.MisDatosGrillaRechazados;
            this.gvRechazados.VirtualItemCount = MisDatosGrillaRechazados.Rows.Count > 0 ? Convert.ToInt32(MisDatosGrillaRechazados.Rows[0]["Cantidad"]) : 0;
            this.gvRechazados.deTantos.Text = "de " + gvRechazados.VirtualItemCount.ToString();
            this.gvRechazados.DataBind();
            AyudaProgramacion.FixGridView(gvRechazados);

            if (this.MisDatosGrilla.Rows.Count > 0)
                this.btnExportarExcel.Visible = true;
            else
                this.btnExportarExcel.Visible = false;
        }
        protected void btnBuscarPendientes_Click(object sender, EventArgs e)
        {
            this.MiBancoCuenta.Detalle = this.txtDetallePendiente.Text.Trim();
            this.MiBancoCuenta.HashTransaccion = 1;
            this.MiBancoCuenta.BusquedaParametros = true;

            this.BusquedaParametrosGuardarValor<TESBancosCuentas>(this.MiBancoCuenta);
            this.MisDatosGrillaPendientes = BancosF.BancosCuentasObtenerMovimientosPendientes(this.MiBancoCuenta);
            this.gvPendientes.DataSource = this.MisDatosGrillaPendientes;
            this.gvPendientes.DataBind();
            AyudaProgramacion.FixGridView(gvPendientes);

            if (this.MiPermisoConciliar && this.MisDatosGrillaPendientes.Rows.Count > 0)
                this.btnConciliar.Visible = true;
            else
                this.btnConciliar.Visible = false;
        }
        protected void btnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            RepReportes reporte = new RepReportes();
            RepParametros param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdBancoCuenta";
            param.ValorParametro = this.MiBancoCuenta.IdBancoCuenta;
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdEstado";
            param.ValorParametro = (int)EstadosBancosCuentasMovimientos.Confirmado;
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
            param.Parametro = "FechaDesde";
            param.ValorParametro = Convert.ToDateTime(this.txtFechaDesde.Text);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
            param.Parametro = "FechaHasta";
            param.ValorParametro = Convert.ToDateTime(this.txtFechaHasta.Text);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "PageSize";
            param.ValorParametro = int.MaxValue;
            reporte.Parametros.Add(param);
            // this.ctrPopUpComprobantes.CargarReporte(reporte, EnumTGEComprobantes.BcoCuentasMovimientos);
            TGEComprobantes comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.BcoCuentasMovimientos);

            reporte.StoredProcedure = comprobante.NombreSP;
            DataSet ds = ReportesF.ReportesObtenerDatos(reporte);
            TGEPlantillas plantilla = new TGEPlantillas();
            plantilla.Codigo = "TesBancosCuentasMovimientosReporte";
            plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);
            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(comprobante, plantilla, ds, "BancoCuentaIdBancoCuenta", AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            ExportPDF.ExportarPDF(pdf, this.upMovimientos, "TesBancosCuentasMovimientosReporte", this.UsuarioActivo);

            this.upMovimientos.Update();
        }
        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisDatosGrilla;
            this.gvDatos.DataBind();
            //GridViewExportUtil.Export("BancosMovimientos.xls", this.gvDatos);
            ExportData exportData = new ExportData();
            exportData.ExportExcel(this.Page, this.MisDatosGrilla);
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.BancoCuentaModificarDatosCancelar != null)
                this.BancoCuentaModificarDatosCancelar();
        }
    }
}