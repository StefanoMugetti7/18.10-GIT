using Bancos;
using Bancos.Entidades;
using Comunes.Entidades;
using Evol.Controls;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Reportes.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;

namespace IU.Modulos.Bancos.Controles
{
    public partial class CobranzasExternasConciliacionesDatos : ControlesSeguros
    {
        private TESCobranzasExternasConciliaciones MiCobranza
        {
            get { return (TESCobranzasExternasConciliaciones)Session[this.MiSessionPagina + "CobranzaModificarDatosMiCobranza"]; }
            set { Session[this.MiSessionPagina + "CobranzaModificarDatosMiCobranza"] = value; }
        }

        private List<TGEListasValoresSistemasDetalles> MisDeducciones
        {
            get { return (List<TGEListasValoresSistemasDetalles>)Session[this.MiSessionPagina + "CobranzasExternasConciliacionesDatosMisDeducciones"]; }
            set { Session[this.MiSessionPagina + "CobranzasExternasConciliacionesDatosMisDeducciones"] = value; }
        }

        public delegate void CobranzasModificarDatosAceptarEventHandler(object sender, TESCobranzasExternasConciliaciones e);
        public event CobranzasModificarDatosAceptarEventHandler CobranzasModificarDatosAceptar;

        public delegate void CobranzasModificarDatosCancelarEventHandler();
        public event CobranzasModificarDatosCancelarEventHandler CobranzasModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                if (this.MiCobranza == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }

                string mensaje = "";
                if (this.GestionControl == Gestion.Anular)
                    mensaje = this.ObtenerMensajeSistema("CobranzasExternasAnularMovimiento");
                else
                    mensaje = this.ObtenerMensajeSistema("CobranzasExternasConfirmarMovimiento");

                string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                this.btnAceptar.Attributes.Add("OnClick", funcion);
            }
            else
            {
                if (this.MiCobranza.CobranzaExternaConciliacionDeducciones.Count() > 0)
                {
                    this.PersistirDeducciones();
                }
                if (this.MiCobranza.CobranzaExternaConciliacionDetalles.Count() > 0)
                {
                    this.PersistirDatosGrilla();
                }
            }


        }

        public void IniciarControl(TESCobranzasExternasConciliaciones pCobranza, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.MiCobranza = pCobranza;
            this.MisDeducciones = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.DeduccionesCobranzaExterna);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.ddlBancoCuenta.Enabled = true;
                    break;
                case Gestion.Modificar:
                    this.MiCobranza = BancosF.CobranzasExternasConciliacionesObtenerDatosCompletos(this.MiCobranza);
                    this.MapearObjetoAControles(this.MiCobranza);
                    break;
                case Gestion.Consultar:
                    this.MiCobranza = BancosF.CobranzasExternasConciliacionesObtenerDatosCompletos(this.MiCobranza);
                    this.txtFechaDesde.Visible = false;
                    this.txtFechaHasta.Visible = false;
                    this.lblFechaDesde.Visible = false;
                    this.lblLote.Visible = false;
                    this.txtLote.Visible = false;
                    this.MapearObjetoAControles(this.MiCobranza);
                    this.pnlCobranzaDetalles.Visible = true;
                    this.btnImprimir.Visible = true;
                    this.btnAceptar.Visible = false;
                    AyudaProgramacion.HabilitarControles(this, false, this.paginaSegura);
                    this.btnCancelar.Visible = true;
                    AyudaProgramacion.CargarGrillaListas<TESCobranzasExternasConciliacionesDetalles>(this.MiCobranza.CobranzaExternaConciliacionDetalles, false, this.gvCobranzaDetalles, true);
                    break;
                case Gestion.Anular:
                    this.MiCobranza = BancosF.CobranzasExternasConciliacionesObtenerDatosCompletos(this.MiCobranza);
                    this.txtFechaDesde.Visible = false;
                    this.txtFechaHasta.Visible = false;
                    this.lblFechaDesde.Visible = false;
                    this.lblLote.Visible = false;
                    this.txtLote.Visible = false;
                    this.MapearObjetoAControles(this.MiCobranza);
                    this.pnlCobranzaDetalles.Visible = true;
                    this.btnImprimir.Visible = false;
                    this.btnAceptar.Visible = true;
                    AyudaProgramacion.HabilitarControles(this, false, this.paginaSegura);
                    this.btnAceptar.Enabled = true;
                    this.btnCancelar.Visible = true;
                    AyudaProgramacion.CargarGrillaListas<TESCobranzasExternasConciliacionesDetalles>(this.MiCobranza.CobranzaExternaConciliacionDetalles, false, this.gvCobranzaDetalles, true);
                    this.UpdatePanel1.Update();
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            this.ddlFiliales.DataSource = UsuarioActivo.Filiales;
            this.ddlFiliales.DataValueField = "IdFilial";
            this.ddlFiliales.DataTextField = "Filial";
            this.ddlFiliales.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFiliales, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlFormaCobro.DataSource = TGEGeneralesF.FormasCobrosObtenerLista();
            this.ddlFormaCobro.DataValueField = "IdFormaCobro";
            this.ddlFormaCobro.DataTextField = "FormaCobro";
            this.ddlFormaCobro.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFormaCobro, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            TESBancosCuentas bancoCuenta = new TESBancosCuentas();
            bancoCuenta.Estado.IdEstado = (int)Estados.Activo;
            bancoCuenta.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
            bancoCuenta.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

            this.ddlBancoCuenta.DataSource = BancosF.BancosCuentasObtenerListaFiltro(bancoCuenta);
            this.ddlBancoCuenta.DataValueField = "IdBancoCuenta";
            this.ddlBancoCuenta.DataTextField = "DescripcionFilialBancoTipoCuentaNumero";
            this.ddlBancoCuenta.DataBind();

            ////REF FORMA COBRO
            AyudaProgramacion.AgregarItemSeleccione(this.ddlRefFormaCobro, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.SinComprobante, "CobranzasExternas", MiCobranza, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            ExportPDF.ExportarPDF(pdf, this.Page, "CobranzasExternas", this.UsuarioActivo);
        }
        protected void ddlFormaCobro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ddlFormaCobro.SelectedValue))
                return;

            if (Convert.ToInt32(ddlFormaCobro.SelectedValue) == (int)EnumTGEFormasCobros.TarjetaCredito)
            {
                this.ddlRefFormaCobro.Enabled = true;
                this.btnBuscar.Visible = true;
                this.rfvRefFormaCobro.Enabled = true;
                //CARGAR COMBO TARJETAS CREDITO
                this.ddlRefFormaCobro.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TarjetasCredito);
                this.ddlRefFormaCobro.DataValueField = "IdListaValorSistemaDetalle";
                this.ddlRefFormaCobro.DataTextField = "Descripcion";
                this.ddlRefFormaCobro.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlRefFormaCobro, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                this.upRefFormaCobro.Update();
            }
            else
            {
                //Limpio el DropDownList
                this.ddlRefFormaCobro.Items.Clear();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlRefFormaCobro, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                this.rfvRefFormaCobro.Enabled = false;
                this.LimpiarControles();
            }

        }
        protected void ddlRefFormaCobro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ddlRefFormaCobro.SelectedValue))
                return;

            this.LimpiarControles();
        }
        private void LimpiarControles()
        {
            this.txtTotal.Text = string.Empty;
            //Campos Fechas
            this.txtFechaDesde.Text = String.Empty;
            this.txtFechaHasta.Text = String.Empty;
            //this.LimpiarDeducciones();
            //Limpio grilla
            this.pnlCobranzaDetalles.Visible = false;
            this.MiCobranza.CobranzaExternaConciliacionDetalles = new List<TESCobranzasExternasConciliacionesDetalles>();
            AyudaProgramacion.CargarGrillaListas<TESCobranzasExternasConciliacionesDetalles>(this.MiCobranza.CobranzaExternaConciliacionDetalles, false, this.gvCobranzaDetalles, true);
            this.upGrilla.Update();
        }

        public void MapearFiltros()
        {
            this.MiCobranza.FormaCobro.IdFormaCobro = Convert.ToInt32(this.ddlFormaCobro.SelectedValue);
            this.MiCobranza.IdRefFormaCobro = this.ddlRefFormaCobro.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlRefFormaCobro.SelectedValue);
            this.MiCobranza.LiquidacionFechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            this.MiCobranza.LiquidacionFechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            this.MiCobranza.Filial.IdFilial = this.ddlFiliales.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFiliales.SelectedValue);
            this.MiCobranza.Lote = txtLote.Text;
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("Buscar");
            this.pnlCobranzaDetalles.Visible = true;
            this.btnAgregarDeduccion.Visible = true;
            this.MiCobranza = new TESCobranzasExternasConciliaciones(); //Vacio los detalles por si se carga con nuevos filtros

            this.MapearFiltros();

            if (this.MiCobranza.FormaCobro.IdFormaCobro == (int)EnumTGEFormasCobros.TarjetaCredito)
            {
                List<TESTarjetasTransacciones> tarjetasActivas = BancosF.TarjetasTransaccionesObtenerActivoEntreFechas(this.MiCobranza);

                this.MapearTarjetasACobranzaDetalles(tarjetasActivas);
            }
            this.pnlCobranzaDetalles.Visible = true;
            AyudaProgramacion.CargarGrillaListas<TESCobranzasExternasConciliacionesDetalles>(this.MiCobranza.CobranzaExternaConciliacionDetalles, false, this.gvCobranzaDetalles, true);
            this.upDeducciones.Update();
            this.upGrilla.Update();
        }

        private void MapearTarjetasACobranzaDetalles(List<TESTarjetasTransacciones> pTarjetas)
        {
            foreach (TESTarjetasTransacciones tarjeta in pTarjetas)
            {
                TESCobranzasExternasConciliacionesDetalles detalle = new TESCobranzasExternasConciliacionesDetalles();
                detalle.TarjetaTransaccion = tarjeta;
                detalle.FechaTransaccion = tarjeta.FechaTransaccion.ToShortDateString();
                detalle.NumeroLote = tarjeta.NumeroLote;
                detalle.Detalle = tarjeta.Detalle;
                detalle.Checked = false;
                this.MiCobranza.CobranzaExternaConciliacionDetalles.Add(detalle);
            }
        }

        private void CalcularTotal()
        {
            decimal totalPresentado = 0;
            decimal totalDescuentos = 0;
            decimal totalNeto = 0;

            if (this.MiCobranza.CobranzaExternaConciliacionDetalles.Where(x => x.Checked).Count() > 0)
            {
                totalPresentado = this.MiCobranza.CobranzaExternaConciliacionDetalles.Where(x => x.Checked).Sum(x => x.TarjetaTransaccion.Importe);
                this.MiCobranza.ImportePresentado = totalPresentado;
            }
            if (this.MiCobranza.CobranzaExternaConciliacionDeducciones.Count() > 0)
            {
                totalDescuentos = this.MiCobranza.CobranzaExternaConciliacionDeducciones.Sum(x => x.ImporteDeduccion);
                this.MiCobranza.Descuentos = totalDescuentos;
            }
            totalNeto = totalPresentado - totalDescuentos;

            this.MiCobranza.ImporteNeto = totalNeto;
            this.txtTotal.Text = totalNeto.ToString("C2");
            this.upTotal.Update();
        }

        #region Grilla Tarjetas Credito

        //protected void gvFormasCobros_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (!(e.CommandName == "Incluir"))
        //        return;

        //    int index = Convert.ToInt32(e.CommandArgument);
        //    int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
        //    TESCobranzasExternasConciliacionesDetalles detalle = MiCobranza.CobranzaExternaConciliacionDetalles[indiceColeccion];
        //    this.MiCobranza.CobranzaExternaConciliacionDetalles.Find(detalle).Checked = false;
        //   // MiCobranza.CobranzaExternaConciliacionDetalles = AyudaProgramacion.AcomodarIndices<CobranzaExternaConciliacionDetalles>(this.MiCobranza.CobranzaExternaConciliacionDetalles);
        //    AyudaProgramacion.CargarGrillaListas<TESCajasMovimientosValores>(MiCajaMovimiento.CajasMovimientosValores, false, this.gvFormasCobros, true); ;
        //    this.upGrilla.Update();
        //}

        protected void gvCobranzaDetalles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (this.GestionControl == Gestion.Agregar)
                {
                    CheckBox ibtnCheck = (CheckBox)e.Row.FindControl("chkIncluir");
                    ibtnCheck.Visible = true;
                    ibtnCheck.Attributes.Add("onchange", "CalcularItem();");

                }
                if (this.GestionControl == Gestion.Consultar)
                {
                    CheckBox ibtnCheck = (CheckBox)e.Row.FindControl("chkIncluir");
                    ibtnCheck.Visible = true;
                    ibtnCheck.Checked = true;
                    ibtnCheck.Enabled = false;
                }


            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

                Label lblTotalDetalles = (Label)e.Row.FindControl("lblTotalDetalles");

                if (this.GestionControl == Gestion.Agregar)
                {

                    lblTotalDetalles.Text = this.MiCobranza.CobranzaExternaConciliacionDetalles.Where(x => x.Checked).Sum(x => x.TarjetaTransaccion.Importe).ToString("C2");
                    this.MiCobranza.ImportePresentado = decimal.Parse(lblTotalDetalles.Text, NumberStyles.Currency);
                }
                else
                {

                    lblTotalDetalles.Text = this.MiCobranza.ImportePresentado.ToString("C2");
                }
            }
        }

        protected void gvCobranzaDetalles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvCobranzaDetalles.PageIndex = e.NewPageIndex;
            this.gvCobranzaDetalles.DataSource = this.MiCobranza.CobranzaExternaConciliacionDetalles;
            this.gvCobranzaDetalles.DataBind();
        }

        private void PersistirDatosGrilla()
        {
            foreach (GridViewRow fila in this.gvCobranzaDetalles.Rows)
            {
                CheckBox chkIncluir = (CheckBox)fila.FindControl("chkIncluir");
                this.MiCobranza.CobranzaExternaConciliacionDetalles[fila.DataItemIndex].Checked = chkIncluir.Checked;
            }
            this.CalcularTotal();
        }
        #endregion

        #region Grilla Deducciones
        private void PersistirDeducciones()
        {
            foreach (GridViewRow fila in this.gvDeducciones.Rows)
            {
                DropDownList ddlDeducciones = ((DropDownList)fila.FindControl("ddlDeducciones"));
                decimal importeDeduccion = ((TextBox)fila.FindControl("txtImporteDeduccion")).Text == string.Empty ? 0 : decimal.Parse(((TextBox)fila.FindControl("txtImporteDeduccion")).Text, NumberStyles.Currency);

                if (ddlDeducciones.SelectedValue != "")
                {
                    this.MiCobranza.CobranzaExternaConciliacionDeducciones[fila.DataItemIndex].TipoDeduccion.IdTipoDeduccion = Convert.ToInt32(ddlDeducciones.SelectedValue);
                }
                if (importeDeduccion != 0)
                {
                    this.MiCobranza.CobranzaExternaConciliacionDeducciones[fila.DataItemIndex].ImporteDeduccion = importeDeduccion;
                }

                //((Label)fila.FindControl("lblImporteIva")).Text = (this.MiSolicitud.SolicitudPagoDetalles[fila.RowIndex].ImporteIvaTotal).ToString();
            }
            this.CalcularTotal();

        }

        protected void gvDeducciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            int index = Convert.ToInt32(e.CommandArgument);
            int IndiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            if (e.CommandName == "Borrar")
            {
                //ELIMINA FILA

                this.MiCobranza.CobranzaExternaConciliacionDeducciones.RemoveAt(IndiceColeccion);
                if (this.MiCobranza.CobranzaExternaConciliacionDeducciones.Count() > 0)
                {
                    this.MiCobranza.CobranzaExternaConciliacionDeducciones = AyudaProgramacion.AcomodarIndices<TESCobranzasExternasConciliacionesDeducciones>(this.MiCobranza.CobranzaExternaConciliacionDeducciones);

                }
                else
                {
                    this.MiCobranza.CobranzaExternaConciliacionDeducciones = new List<TESCobranzasExternasConciliacionesDeducciones>();

                }
                this.MiCobranza.Descuentos = this.MiCobranza.CobranzaExternaConciliacionDeducciones.Sum(x => x.ImporteDeduccion);
                AyudaProgramacion.CargarGrillaListas<TESCobranzasExternasConciliacionesDeducciones>(this.MiCobranza.CobranzaExternaConciliacionDeducciones, false, this.gvDeducciones, true);
                this.CalcularTotal();
            }

        }

        protected void gvDeducciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                TESCobranzasExternasConciliacionesDeducciones item = (TESCobranzasExternasConciliacionesDeducciones)e.Row.DataItem;
                CurrencyTextBox importePercepcion = (CurrencyTextBox)e.Row.FindControl("txtImporteDeduccion");
                DropDownList ddlDeducciones = ((DropDownList)e.Row.FindControl("ddlDeducciones"));
                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");

                ddlDeducciones.Enabled = false;
                importePercepcion.Enabled = false;
                btnEliminar.Visible = false;

                ddlDeducciones.DataSource = this.MisDeducciones;//TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.AFIPTiposPercepciones);
                ddlDeducciones.DataValueField = "IdListaValorSistemaDetalle";
                ddlDeducciones.DataTextField = "Descripcion";
                ddlDeducciones.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(ddlDeducciones, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                ListItem itemCombo;

                switch (GestionControl)
                {
                    case Gestion.Agregar:
                        ddlDeducciones.SelectedValue = item.TipoDeduccion.IdTipoDeduccion.ToString();
                        ddlDeducciones.Enabled = true;
                        importePercepcion.Attributes.Add("onchange", "CalcularDeduccion();"); ;
                        importePercepcion.Enabled = true;
                        string mensaje = this.ObtenerMensajeSistema("ConfirmarEliminar");
                        string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                        btnEliminar.Attributes.Add("OnClick", funcion);
                        btnEliminar.Visible = true;
                        break;
                    case Gestion.Consultar:
                        itemCombo = ddlDeducciones.Items.FindByValue(item.TipoDeduccion.IdTipoDeduccion.ToString());
                        if (itemCombo == null)
                            ddlDeducciones.Items.Add(new ListItem(item.TipoDeduccion.IdTipoDeduccion.ToString(), item.TipoDeduccion.Descripcion));
                        ddlDeducciones.SelectedValue = item.TipoDeduccion.IdTipoDeduccion.ToString();
                        ddlDeducciones.Enabled = false;
                        break;
                    case Gestion.Autorizar:
                        itemCombo = ddlDeducciones.Items.FindByValue(item.TipoDeduccion.IdTipoDeduccion.ToString());
                        if (itemCombo == null)
                            ddlDeducciones.Items.Add(new ListItem(item.TipoDeduccion.IdTipoDeduccion.ToString(), item.TipoDeduccion.Descripcion));
                        ddlDeducciones.SelectedValue = item.TipoDeduccion.IdTipoDeduccion.ToString();
                        ddlDeducciones.Enabled = false;
                        break;
                    default:
                        break;

                }

            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {

                Label lblImporteDeducciones = (Label)e.Row.FindControl("lblImporteDeducciones");

                lblImporteDeducciones.Text = this.MiCobranza.Descuentos.ToString("C2");

            }
        }

        protected void btnAgregarDeduccion_Click(object sender, EventArgs e)
        {
            TESCobranzasExternasConciliacionesDeducciones item;
            item = new TESCobranzasExternasConciliacionesDeducciones();
            this.MiCobranza.CobranzaExternaConciliacionDeducciones.Add(item);
            item.IndiceColeccion = this.MiCobranza.CobranzaExternaConciliacionDeducciones.IndexOf(item);
            AyudaProgramacion.CargarGrillaListas<TESCobranzasExternasConciliacionesDeducciones>(this.MiCobranza.CobranzaExternaConciliacionDeducciones, false, this.gvDeducciones, true);
            this.upDeducciones.Update();
        }

        //protected void LimpiarDeducciones()
        //{
        //    this.MiCobranza.CobranzaExternaConciliacionDeducciones.Clear();
        //    AyudaProgramacion.CargarGrillaListas<TESCobranzasExternasConciliacionesDeducciones>(this.MiCobranza.CobranzaExternaConciliacionDeducciones, false, this.gvDeducciones, true);
        //    this.upDeducciones.Update();
        //}
        #endregion

        private void MapearControlesAObjeto(TESCobranzasExternasConciliaciones pCobranza)
        {
            pCobranza.CantidadRegistros = pCobranza.CobranzaExternaConciliacionDetalles.Count();
            //pCobranza.ImporteNeto = decimal.Parse(this.txtTotal.Text, NumberStyles.Currency);
            pCobranza.IdBancoCuenta = Convert.ToInt32(this.ddlBancoCuenta.SelectedValue);
            pCobranza.FechaConfirmacion = Convert.ToDateTime(this.txtFechaConfirmacion.Text);
            pCobranza.Filial.IdFilial = Convert.ToInt32(this.ddlFiliales.SelectedValue);

        }

        private void MapearObjetoAControles(TESCobranzasExternasConciliaciones pCobranza)
        {
            this.ddlFormaCobro.SelectedValue = pCobranza.FormaCobro.IdFormaCobro.ToString();
            // CARGO LOS DDL DE LA FORMA DE COBRO PARA PODER MOSTRAR
            if (Convert.ToInt32(ddlFormaCobro.SelectedValue) == (int)EnumTGEFormasCobros.TarjetaCredito)
            {
                this.ddlRefFormaCobro.Items.Clear();
                this.ddlRefFormaCobro.SelectedIndex = -1;
                this.ddlRefFormaCobro.SelectedValue = null;
                this.ddlRefFormaCobro.ClearSelection();
                //CARGAR COMBO TARJETAS CREDITO
                this.ddlRefFormaCobro.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TarjetasCredito);
                this.ddlRefFormaCobro.DataValueField = "IdListaValorSistemaDetalle";
                this.ddlRefFormaCobro.DataTextField = "Descripcion";
                this.ddlRefFormaCobro.DataBind();
            }

            this.ddlRefFormaCobro.SelectedValue = pCobranza.IdRefFormaCobro.ToString();
            this.txtFechaConfirmacion.Text = pCobranza.FechaConfirmacion.ToShortDateString();
            this.txtFechaDesde.Text = pCobranza.LiquidacionFechaDesde == null ? string.Empty : Convert.ToDateTime(pCobranza.LiquidacionFechaDesde).ToShortDateString();
            this.txtFechaHasta.Text = pCobranza.LiquidacionFechaHasta == null ? string.Empty : Convert.ToDateTime(pCobranza.LiquidacionFechaHasta).ToShortDateString();
            this.txtLote.Text = pCobranza.Lote == null ? string.Empty : pCobranza.ToString();
            this.txtTotal.Text = pCobranza.ImporteNeto.ToString("C2");
            this.ddlBancoCuenta.SelectedValue = pCobranza.IdBancoCuenta.ToString();
            this.ddlFiliales.SelectedValue = pCobranza.Filial.IdFilial == 0 ? string.Empty : pCobranza.Filial.IdFilial.ToString();
            AyudaProgramacion.CargarGrillaListas<TESCobranzasExternasConciliacionesDetalles>(pCobranza.CobranzaExternaConciliacionDetalles, false, gvCobranzaDetalles, true);
            AyudaProgramacion.CargarGrillaListas<TESCobranzasExternasConciliacionesDeducciones>(pCobranza.CobranzaExternaConciliacionDeducciones, false, gvDeducciones, true);

        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptar.Visible = false;
            if (!this.Page.IsValid)
            {
                this.upFechaConfirmacion.Update();
                return;
            }
            this.PersistirDeducciones();
            this.PersistirDatosGrilla();

            if ((this.MiCobranza.CobranzaExternaConciliacionDetalles.Count() == 0 || !this.MiCobranza.CobranzaExternaConciliacionDetalles.Exists(x => x.Checked)) && GestionControl == Gestion.Agregar)
            {
                this.MiCobranza.CodigoMensaje = "CobranzasExternasGrillaVacia";
                guardo = false;
            }
            else if (this.MiCobranza.CobranzaExternaConciliacionDeducciones.Exists(x => x.TipoDeduccion.IdTipoDeduccion == 0) && GestionControl == Gestion.Agregar)
            {
                this.MiCobranza.CodigoMensaje = "SeleccioneDeduccion";
                guardo = false;
            }
            else
            {
                this.MapearControlesAObjeto(this.MiCobranza);
                this.MiCobranza.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                switch (this.GestionControl)
                {
                    case Gestion.Agregar:

                        guardo = BancosF.CobranzasExternasConciliacionesAgregar(this.MiCobranza);

                        break;
                    case Gestion.Modificar:

                        break;
                    case Gestion.Anular:
                        guardo = BancosF.CobranzasExternasConciliacionesAnular(this.MiCobranza);
                        break;
                    default:
                        break;
                }
            }
            if (guardo)
            {
                this.MostrarMensaje(this.ObtenerMensajeSistema(this.MiCobranza.CodigoMensaje), false);
                this.btnImprimir.Visible = true;
            }
            else
            {
                this.MostrarMensaje(this.MiCobranza.CodigoMensaje, true, this.MiCobranza.CodigoMensajeArgs);
                this.btnAceptar.Visible = true;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.CobranzasModificarDatosCancelar != null)
                this.CobranzasModificarDatosCancelar();
        }
    }
}