using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cargos.Entidades;
using Cobros.Entidades;
using Comunes.Entidades;
using Cargos;
using Cobros;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System.Globalization;
using System.Net.Mail;
using System.Collections;
using Tesorerias.Entidades;
using Reportes.FachadaNegocio;

namespace IU.Modulos.Cobros.Controles
{
    public partial class OrdenesCobrosAfiliadosDatos : ControlesSeguros
    {
        private List<CarCuentasCorrientes> MisCargosPendientesCobro
        {
            get { return (List<CarCuentasCorrientes>)Session[this.MiSessionPagina + "OrdenesCobrosAfiliadosDatosMisCargosPendientesCobro"]; }
            set { Session[this.MiSessionPagina + "OrdenesCobrosAfiliadosDatosMisCargosPendientesCobro"] = value; }
        }

        private CobOrdenesCobros MiOrdenCobro
        {
            get { return (CobOrdenesCobros)Session[this.MiSessionPagina + "OrdenesCobrosAfiliadosDatosMiOrdenCobro"]; }
            set { Session[this.MiSessionPagina + "OrdenesCobrosAfiliadosDatosMiOrdenCobro"] = value; }
        }

        private List<TGEMonedas> MisMonedas
        {
            get { return (List<TGEMonedas>)Session[this.MiSessionPagina + "TesoreriasMovimientosAgregarMisMonedas"]; }
            set { Session[this.MiSessionPagina + "TesoreriasMovimientosAgregarMisMonedas"] = value; }
        }

        public delegate void OrdenesCobrosDatosAceptarEventHandler(CobOrdenesCobros e);
        public event OrdenesCobrosDatosAceptarEventHandler OrdenesCobrosModificarDatosAceptar;
        public delegate void OrdenesCobrosDatosCancelarEventHandler();
        public event OrdenesCobrosDatosCancelarEventHandler OrdenesCobrosModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrOrdenesCobrosValores.OrdenesCobrosIngresarValor += new OrdenesCobrosValores.OrdenesCobrosValoresIngresarEventHandler(ctrOrdenesCobrosValores_OrdenesCobrosIngresarValor);
        }

        void ctrOrdenesCobrosValores_OrdenesCobrosIngresarValor(List<CobOrdenesCobrosValores> e)
        {
            btnAceptar.Attributes.Remove("OnClick");

            TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.NotificacionEfectivoValidar);
            bool NotificacionEfectivoValidar = valor.ParametroValor == string.Empty ? true : Convert.ToBoolean(valor.ParametroValor);


            if (e.Count == 1
                && e.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.CajaAhorro))
            {
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarAccion"));
                this.btnAceptar.Attributes.Add("OnClick", funcion);
            }
            //else if(e.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Efectivo) && NotificacionEfectivoValidar)
            //{
            //    TGEParametrosValores valor1 = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.NotificacionEfectivoTope);
            //    decimal NotificacionEfectivoTope = valor1.ParametroValor == string.Empty ? 0 : Convert.ToDecimal(valor1.ParametroValor);

            //   if(e.Exists(x => x.Importe > NotificacionEfectivoTope))
            //    {
            //        string funcion = string.Format("showConfirm(this,'{0}'); return false;", string.Format( this.ObtenerMensajeSistema("ValidarEfectivoTope"), NotificacionEfectivoTope));
            //        this.btnAceptar.Attributes.Add("OnClick", funcion);
            //    }
               
            //}

            UpdatePanel1.Update();
        }

        public void IniciarControl(CobOrdenesCobros pOrdenCobro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiOrdenCobro = pOrdenCobro;
            this.CargarCombos();


            switch (this.GestionControl)
            {
                case Gestion.Agregar:

                    this.MiOrdenCobro.FechaEmision = DateTime.Now;
                    this.txtFecha.Text = DateTime.Now.ToShortDateString();
                    this.txtTipoOperacion.Text = this.MiOrdenCobro.TipoOperacion.TipoOperacion;
                    CarCuentasCorrientes cuenta = new CarCuentasCorrientes();
                    cuenta.IdAfiliado = this.MiOrdenCobro.Afiliado.IdAfiliado;
                    cuenta.TipoOperacion = MiOrdenCobro.TipoOperacion;
                    cuenta.UsuarioLogueado.IdUsuarioEvento = this.UsuarioActivo.IdUsuario;

                    if (this.MisParametrosUrl.Contains("IdTipoCargoAfiliadoFormaCobro"))
                    {
                        cuenta.IdReferenciaRegistro = Convert.ToInt32(this.MisParametrosUrl["IdTipoCargoAfiliadoFormaCobro"].ToString());
                        //  cuenta.IdRefTipoOperacion = Convert.ToInt32(this.MisParametrosUrl["IdTipoCargo"].ToString());
                        MiOrdenCobro.IdRefTabla = Convert.ToInt64(this.MisParametrosUrl["IdTipoCargoAfiliadoFormaCobro"].ToString());
                        MiOrdenCobro.Tabla = typeof(CarTiposCargosAfiliadosFormasCobros).Name;

                    }
                    this.ddlMoneda_OnSelectedIndexChanged(null, EventArgs.Empty);
                    this.MisCargosPendientesCobro = CargosF.CuentasCorrientesObtenerPendientesCobro(cuenta);
                    //this.MisCargosPendientesCobro = this.MisCargosPendientesCobro.Where(x => !this.MiOrdenCobro.OrdenesCobrosDetalles.Any(x2 => x.IdCuentaCorriente == x2.CuentaCorriente.IdCuentaCorriente)).ToList();
                    //this.MisCargosPendientesCobro = AyudaProgramacion.AcomodarIndices<CarCuentasCorrientes>(this.MisCargosPendientesCobro);
                    AyudaProgramacion.CargarGrillaListas<CarCuentasCorrientes>(this.MisCargosPendientesCobro, false, this.gvCuentaCorriente, true);
                    this.pnlCuentaCorriente.Visible = true;
                    //this.btnIncluirCobro.Visible = true;
                    this.ddlFilialCobro.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
                    if (this.MiOrdenCobro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.NotaCreditoCargos)
                    {
                        string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarNotaCreditoCargos"));
                        this.btnAceptar.Attributes.Add("OnClick", funcion);
                    }

                    this.ctrFechaCajaContable.IniciarControl(Gestion.Agregar, DateTime.Now, DateTime.Now);
                    this.ctrCampos.IniciarControl(MiOrdenCobro, new Objeto(), GestionControl);
                    break;
                case Gestion.Anular:
                case Gestion.AnularConfirmar:
                    this.MiOrdenCobro = CobrosF.OrdenesCobrosObtenerDatosCompletos(this.MiOrdenCobro);
                    this.MapearObjetoAControles();
                    SetInitializeCulture(MiOrdenCobro.Moneda.Moneda);
                    this.txtDetalle.Enabled = false;
                    ddlMoneda.Enabled = false;
                    this.ddlFilialCobro.Enabled = false;
                    this.btnAceptar.Visible = true;
                    this.btnEnviarMail.Visible = true;
                    this.OcultarColumnas();
                    break;
                case Gestion.Consultar:
                    this.MiOrdenCobro = CobrosF.OrdenesCobrosObtenerDatosCompletos(this.MiOrdenCobro);
                    SetInitializeCulture(MiOrdenCobro.Moneda.Moneda);
                    this.MapearObjetoAControles();
                    ddlMoneda.Enabled = false;
                    this.txtDetalle.Enabled = false;
                    this.ddlFilialCobro.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.btnImprimir.Visible = true;
                    this.btnEnviarMail.Visible = true;
                    this.OcultarColumnas();
                    break;
                default:
                    break;
            }
        }
        private void OcultarColumnas()
        {
            this.gvCuentaCorriente.Columns[this.gvCuentaCorriente.Columns.Count - 1].Visible = false; //COLUMNA CHECK
            this.gvCuentaCorriente.Columns[this.gvCuentaCorriente.Columns.Count - 2].Visible = false; //COLUMNA A COBRAR
        }

        private void MapearObjetoAControles()
        {
            this.txtTipoOperacion.Text = this.MiOrdenCobro.TipoOperacion.TipoOperacion;
            this.txtFecha.Text = this.MiOrdenCobro.FechaEmision.ToShortDateString();
            this.txtOrdenCobro.Text = this.MiOrdenCobro.IdOrdenCobro.ToString();
            this.txtDetalle.Text = this.MiOrdenCobro.Detalle;
            this.ddlFilialCobro.SelectedValue = this.MiOrdenCobro.FilialCobro.IdFilialCobro.ToString();
            //AyudaProgramacion.CargarGrillaListas<CobOrdenesCobrosDetalles>(this.MiOrdenCobro.OrdenesCobrosDetalles, false, this.gvOrdenesCobrosDetalles, true);
            this.MisCargosPendientesCobro = CobrosF.CuentasCorrientesObtenerPorIdOrdenCobro(this.MiOrdenCobro);
            AyudaProgramacion.CargarGrillaListas<CarCuentasCorrientes>(this.MisCargosPendientesCobro, false, this.gvCuentaCorriente, true);
            this.pnlCuentaCorriente.Visible = true;
            this.ctrAsientoMostrar.IniciarControl(this.MiOrdenCobro);
            if (MiOrdenCobro.TipoOperacion.IdTipoOperacion != (int)EnumTGETiposOperaciones.NotaCreditoCargos)
                this.ctrOrdenesCobrosValores.IniciarControl(this.MiOrdenCobro, this.GestionControl);
            this.ctrFechaCajaContable.IniciarControl(Gestion.Consultar, this.MiOrdenCobro.FechaConfirmacion);
            this.ddlMoneda.SelectedValue = MiOrdenCobro.Moneda.IdMoneda.ToString();
            this.ctrCampos.IniciarControl(MiOrdenCobro, new Objeto(), GestionControl);
        }

        private void MapearControlesAObjeto()
        {
            this.MiOrdenCobro.Detalle = this.txtDetalle.Text;
            this.MiOrdenCobro.FilialCobro.IdFilialCobro = Convert.ToInt32(this.ddlFilialCobro.SelectedValue);
            this.MiOrdenCobro.FilialCobro.Filial = this.ddlFilialCobro.SelectedItem.Text;
            this.MiOrdenCobro.FechaConfirmacion = this.ctrFechaCajaContable.dFechaCajaContable;
            this.MiOrdenCobro.Moneda.IdMoneda = ddlMoneda.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlMoneda.SelectedValue);
            TGEMonedas mon = MisMonedas.Find(x => x.IdMoneda == Convert.ToInt32(this.ddlMoneda.SelectedValue));
            MiOrdenCobro.MonedaCotizacion = mon.MonedeaCotizacion.MonedaCotizacion;
            this.MiOrdenCobro.Campos = this.ctrCampos.ObtenerLista();
            this.MiOrdenCobro.LoteCamposValores = this.ctrCampos.ObtenerListaCamposValores();
        }

        private void CargarCombos()
        {
            this.ddlFilialCobro.DataSource = TGEGeneralesF.FilialesObenerLista();
            this.ddlFilialCobro.DataValueField = "IdFilial";
            this.ddlFilialCobro.DataTextField = "Filial";
            this.ddlFilialCobro.DataBind();

            MisMonedas = TGEGeneralesF.MonedasCotizacionesObtenerVentaCombo();
            this.ddlMoneda.DataSource = MisMonedas; // TGEGeneralesF.MonedasObtenerListaActiva();
            this.ddlMoneda.DataValueField = "IdMoneda";
            this.ddlMoneda.DataTextField = "Descripcion";
            this.ddlMoneda.DataBind();
           
        }

        protected void gvCuentaCorriente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
            ibtnConsultar.Visible = true;
            ibtnConsultar.Attributes.Add("onchange", "CalcularItem();");

            TextBox aCobrar = (TextBox)e.Row.FindControl("txtACobrar");
            aCobrar.Attributes.Add("onchange", "CalcularItem();");

            if ((this.GestionControl == Gestion.Anular)
                || (this.GestionControl == Gestion.Consultar))
            {
                ibtnConsultar.Enabled = false;
                ibtnConsultar.Checked = true;
                aCobrar.Enabled = false;

                if (this.MiOrdenCobro.Estado.IdEstado == (int)EstadosOrdenesCobro.Cobrado)
                    aCobrar.Text = "0";
            }
            }
            //if (e.Row.RowType == DataControlRowType.Footer
            //    && this.MisCargosPendientesCobro.Count > 0)
            //{
            //    int cellCount = e.Row.Cells.Count;
            //    e.Row.Cells.Clear();
            //    TableCell tableCell = new TableCell();
            //    tableCell.ColumnSpan = cellCount - 2;
            //    tableCell.HorizontalAlign = HorizontalAlign.Right;
            //    tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalImporte"), this.MisCargosPendientesCobro.Sum(x => x.Importe).ToString("C2"));
            //    e.Row.Cells.Add(tableCell);
            //    e.Row.Cells.Add(new TableCell());
            //    e.Row.Cells.Add(new TableCell());
            //}
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if ((this.GestionControl == Gestion.Consultar || this.GestionControl == Gestion.Anular) && (this.MiOrdenCobro.Estado.IdEstado == (int)EstadosOrdenesCobro.Activo))
                {
                    Label lblImporteTotal = (Label)e.Row.FindControl("lblTotalACobrar");
                    lblImporteTotal.Text = this.MisCargosPendientesCobro.Sum(x => x.ImporteEnviar).ToString("C2");
                }
            }
        }

        protected void gvCuentaCorriente_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //AhoCuentasMovimientos parametros = this.BusquedaParametrosObtenerValor<AhoCuentasMovimientos>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<AhoCuentasMovimientos>(parametros);

            this.gvCuentaCorriente.PageIndex = e.NewPageIndex;
            this.gvCuentaCorriente.DataSource = this.MisCargosPendientesCobro;
            this.gvCuentaCorriente.DataBind();
        }

        protected bool MapearCuentasAOrdenDetalle()
        {
            CobOrdenesCobrosDetalles detalle;
            CarCuentasCorrientes cuentaCte;
            CheckBox incluir;
            TextBox aCobrar;
            decimal importeACobrar;
            this.MiOrdenCobro.OrdenesCobrosDetalles = new List<CobOrdenesCobrosDetalles>();
            foreach (GridViewRow fila in this.gvCuentaCorriente.Rows)
            {
                
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    //REEMPLAZO IMPORTE POR IMPORTEENVIAR
                    cuentaCte = this.MisCargosPendientesCobro[fila.DataItemIndex];
                    //cuentaCte.Incluir = true;
                    incluir = (CheckBox)fila.FindControl("chkIncluir");
                    aCobrar = (TextBox)fila.FindControl("txtACobrar");
                    importeACobrar = Decimal.Parse(aCobrar.Text, NumberStyles.Currency);
                    if (incluir.Checked)
                    {
                        detalle = new CobOrdenesCobrosDetalles();
                        detalle.EstadoColeccion = EstadoColecciones.Agregado;
                        detalle.CuentaCorriente = cuentaCte;
                        //AyudaProgramacion.MatchObjectProperties(cuentaCte.TipoCargo.CuentaContable, detalle.CuentaContable);
                        //detalle.ConceptoContable = cuentaCte.TipoCargo.ConceptoContable;

                        // EmporteEnviar  =  Importe - ImporteCobrado
                        if (cuentaCte.ImporteEnviar < importeACobrar)
                        {
                            this.MiOrdenCobro.CodigoMensaje = "ValidarImporteCobrar";
                            return false;
                        }
                        detalle.Detalle = cuentaCte.Concepto;
                        detalle.IncluirEnOP = true;
                        detalle.Importe = importeACobrar;
                        this.MiOrdenCobro.OrdenesCobrosDetalles.Add(detalle);
                        detalle.IndiceColeccion = this.MiOrdenCobro.OrdenesCobrosDetalles.IndexOf(detalle);
                    }
                }
            }
            return true;
        }

        protected void ddlMoneda_OnSelectedIndexChanged(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(ddlMoneda.SelectedValue))
            {
                //List<TESCajasMonedas> monedas = this.MiCaja.CajasMonedas;
                var idMoneda = Convert.ToInt32(this.ddlMoneda.SelectedValue);
                MiOrdenCobro.Moneda = MisMonedas.Find(delegate (TGEMonedas m) { return m.IdMoneda == idMoneda; });
                SetInitializeCulture(MiOrdenCobro.Moneda.Moneda);

                CarCuentasCorrientes cuenta = new CarCuentasCorrientes();
                cuenta.IdAfiliado = this.MiOrdenCobro.Afiliado.IdAfiliado;
                cuenta.Moneda = MisMonedas.Find(delegate (TGEMonedas m) { return m.IdMoneda == idMoneda; });
                cuenta.TipoOperacion = MiOrdenCobro.TipoOperacion;
                if (this.MisParametrosUrl.Contains("IdTipoCargoAfiliadoFormaCobro"))
                {
                    cuenta.IdReferenciaRegistro = Convert.ToInt32(this.MisParametrosUrl["IdTipoCargoAfiliadoFormaCobro"].ToString());
                    cuenta.IdRefTipoOperacion = Convert.ToInt32(this.MisParametrosUrl["IdTipoCargo"].ToString());

                }
                this.MisCargosPendientesCobro = CargosF.CuentasCorrientesObtenerPendientesCobro(cuenta);
                //this.MisCargosPendientesCobro = this.MisCargosPendientesCobro.Where(x => !this.MiOrdenCobro.OrdenesCobrosDetalles.Any(x2 => x.IdCuentaCorriente == x2.CuentaCorriente.IdCuentaCorriente)).ToList();
                //this.MisCargosPendientesCobro = AyudaProgramacion.AcomodarIndices<CarCuentasCorrientes>(this.MisCargosPendientesCobro);
                AyudaProgramacion.CargarGrillaListas<CarCuentasCorrientes>(this.MisCargosPendientesCobro, false, this.gvCuentaCorriente, true);
                this.pnlCuentaCorriente.Visible = true;
                if (MiOrdenCobro.TipoOperacion.IdTipoOperacion != (int)EnumTGETiposOperaciones.NotaCreditoCargos)
                    this.ctrOrdenesCobrosValores.IniciarControl(this.MiOrdenCobro, this.GestionControl, true);
                
            }
            this.ctrOrdenesCobrosValores.ActualizarUpdatePanel();
            this.upArmarCobros.Update();
            //this.IniciarControlAgregar(this.MiOrdenCobro);
            //upMovimientos.Update();



        }


        //private void IniciarControlAgregar(CobOrdenesCobros pOrdenCobro)
        //{
           
        //        if (!string.IsNullOrEmpty(ddlMoneda.SelectedValue))
        //        {
        //            pOrdenCobro.Moneda.IdMoneda = Convert.ToInt32(ddlMoneda.SelectedValue);
        //            this.MiOrdenCobro.OrdenesCobrosDetalles = CobrosF.FacturaObtenerPendientePago(pOrdenCobro);
        //            this.MiOrdenCobro.OrdenesCobrosAnticipos = CobrosF.OrdenesCobrosAnticiposPendientesAplicar(pOrdenCobro);
        //            //si es factura automatica .... 
        //            if (Convert.ToInt32(pOrdenCobro.IdRefFacturaOCAutomatica) != 0)
        //            {
        //                foreach (CobOrdenesCobrosDetalles detalle in this.MiOrdenCobro.OrdenesCobrosDetalles)
        //                {
        //                    if (detalle.Factura.IdFactura != Convert.ToInt32(pOrdenCobro.IdRefFacturaOCAutomatica))
        //                    {
        //                        detalle.IncluirEnOP = false;
        //                    }
        //                }
        //            }

        //            //this.MiOrdenCobro.ImporteSubTotal = this.MiOrdenCobro.OrdenesCobrosDetalles.Where(x => x.IncluirEnOP).Sum(x => x.Importe);
        //            //this.txtSubTotal.Text = (0).ToString("C2"); // this.MiOrdenCobro.ImporteSubTotal.ToString("C2");
        //            //this.txtImporteRetenciones.Text = (0).ToString("C2");
        //            //this.txtTotalCobrar.Text = (0).ToString("C2");// this.MiOrdenCobro.ImporteSubTotal.ToString("C2");
        //                                                          //this.txtFechaAlta.Text = DateTime.Now.ToShortDateString();
        //            AyudaProgramacion.CargarGrillaListas<CobOrdenesCobrosDetalles>(this.MiOrdenCobro.OrdenesCobrosDetalles, false, this.gvCuentaCorriente, true);
        //            //if (this.MiOrdenCobro.OrdenesCobrosTiposRetenciones.Count == 0)
        //            //    this.btnAgregarRetencion_Click(null, EventArgs.Empty);
        //            this.upArmarCobros.Update();
                  

        //            //AyudaProgramacion.CargarGrillaListas<CobOrdenesCobros>(this.MiOrdenCobro.OrdenesCobrosAnticipos, false, this.gvCuentaCorriente, true);

        //            //if (this.MiOrdenCobro.OrdenesCobrosAnticipos.Count > 0)
        //            //{
        //            //    this.pnlOrdenesCobrosAnticipos.Visible = true;
        //            //}
        //            //else
        //            //{
        //            //    this.pnlOrdenesCobrosAnticipos.Visible = false;
        //            //}
        //            //this.upOrdenesCobrosAnticipos.Update();
        //            //this.upTotales.Update();
        //        }
            
        //    //ScriptManager.RegisterStartupScript(this.upOrdenPagoDetalle, this.upOrdenPagoDetalle.GetType(), "gvDetallesCalcularItem", "CalcularItem();", true);
        //}

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto();
            this.MiOrdenCobro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    if (this.MapearCuentasAOrdenDetalle())
                    {
                        this.MiOrdenCobro.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                   
                        this.MiOrdenCobro.ImporteTotal = this.MiOrdenCobro.OrdenesCobrosDetalles.Sum(x => x.Importe);

                        if (MiOrdenCobro.TipoOperacion.IdTipoOperacion != (int)EnumTGETiposOperaciones.NotaCreditoCargos)
                            this.MiOrdenCobro = this.ctrOrdenesCobrosValores.ObtenerOrdenesCobrosValores(this.MiOrdenCobro);

                        guardo = CobrosF.OrdenesCobrosAgregar(this.MiOrdenCobro);
                    }
                    else
                        guardo = false;
                    break;
                case Gestion.Anular:
                    this.MiOrdenCobro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    guardo = CobrosF.OrdenesCobrosAnular(this.MiOrdenCobro);
                    break;
                case Gestion.AnularConfirmar:
                    this.MiOrdenCobro.EstadoColeccion = EstadoColecciones.Agregado;
                    this.MiOrdenCobro.Estado.IdEstado = (int)EstadosOrdenesCobro.Baja;
                    guardo = CobrosF.OrdenesCobrosAnularAfiliadoCobrada(this.MiOrdenCobro);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.btnAceptar.Visible = false;
                this.btnEnviarMail.Visible = true;

                if (this.OrdenesCobrosModificarDatosAceptar != null)
                    this.OrdenesCobrosModificarDatosAceptar(MiOrdenCobro);

                this.MostrarMensaje(this.MiOrdenCobro.CodigoMensaje, false, this.MiOrdenCobro.CodigoMensajeArgs);
                if (this.MiOrdenCobro.Estado.IdEstado == (int)EstadosOrdenesCobro.Cobrado)
                {
                    this.btnImprimir.Visible = true;
                    this.ctrAsientoMostrar.IniciarControl(this.MiOrdenCobro);
                    //  this.ctrPopUpComprobantes.CargarReporte(this.MiOrdenCobro, EnumTGEComprobantes.CobOrdenesCobros, true);
                    //byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CobOrdenesCobrosAfiliados, "OrdenesCobrosAfiliados", MiOrdenCobro, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    //ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, string.Concat("Comprobante_", MiOrdenCobro.IdOrdenCobro.ToString().PadLeft(10, '0')), this.UsuarioActivo);

                }

            }
            else
            {
                this.MostrarMensaje(this.MiOrdenCobro.CodigoMensaje, true, this.MiOrdenCobro.CodigoMensajeArgs);
                if (this.MiOrdenCobro.dsResultado != null)
                {
                    this.ctrPopUpGrilla.IniciarControl(this.MiOrdenCobro);
                    this.MiOrdenCobro.dsResultado = null;
                }
            }
        }

        protected void btnAnticipos_Click(object sender, EventArgs e)
        {
            PaginaAfiliados paginaAfi = new PaginaAfiliados();
            paginaAfi.Guardar(this.MiSessionPagina, this.MiOrdenCobro.Afiliado);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosFacturacionAnticipada.aspx"), true);

        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.OrdenesCobrosModificarDatosCancelar != null)
            {
                this.MiOrdenCobro = new CobOrdenesCobros();
                this.OrdenesCobrosModificarDatosCancelar();
            }
        }
        protected void btnEnviarMail_Click(object sender, EventArgs e)
        {
            MailMessage mail = new MailMessage();
            if (CobrosF.OrdenesCobroArmarMail(this.MiOrdenCobro, mail))
            {
                this.popUpMail.IniciarControl(mail, this.MiOrdenCobro);
            }
        }

        protected void btnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            CobOrdenesCobros obj = new CobOrdenesCobros();
            obj.Filtro = obj.IdOrdenCobro.ToString();
            obj.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            obj.TipoOperacion.IdTipoOperacion = MiOrdenCobro.TipoOperacion.IdTipoOperacion;
            TGEPlantillas miPlantilla = TGEGeneralesF.PlantillasObtenerDatosPorTipoOperacion(obj);
            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CobOrdenesCobros, miPlantilla.Codigo, MiOrdenCobro, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, string.Concat("Comprobante_", MiOrdenCobro.IdOrdenCobro.ToString().PadLeft(10, '0')), this.UsuarioActivo);
        }
    }
}