using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Facturas.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Afiliados.Entidades;
using Afiliados;
using Compras.Entidades;
using Compras;
using SKP.ASP.Controls;
using System.Globalization;
using Facturas;

namespace IU.Modulos.Facturas.Controles
{
    public partial class PresupuestosDatos : ControlesSeguros
    {
        private VTAPresupuestos MiPresupuesto
        {
            get { return (VTAPresupuestos)Session[this.MiSessionPagina + "PresupuestosDatosMiPresupuesto"]; }
            set { Session[this.MiSessionPagina + "PresupuestosDatosMiPresupuesto"] = value; }
        }

        private List<TGEIVA> MisIvas
        {
            get { return (List<TGEIVA>)Session[this.MiSessionPagina + "PresupuestosDatosMisIvas"]; }
            set { Session[this.MiSessionPagina + "PresupuestosDatosMisIvas"] = value; }
        }

        private List<TGEMonedas> MisMonedas
        {
            get { return (List<TGEMonedas>)Session[this.MiSessionPagina + "PresupuestosDatosMisMonedas"]; }
            set { Session[this.MiSessionPagina + "PresupuestosDatosMisMonedas"] = value; }
        }

        //private List<TGEMonedas> MisMonedas
        //{
        //    get { return (List<TGEMonedas>)Session[this.MiSessionPagina + "PresupuestosDatosMisMonedas"]; }
        //    set { Session[this.MiSessionPagina + "PresupuestosDatosMisMonedas"] = value; }
        //}

        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "PresupuestoModificarDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "PresupuestoModificarDatosMiIndiceDetalleModificar"] = value; }
        }

        //public delegate void PresupuestosDatosAceptarEventHandler(object sender, VTAPresupuestos e);
        //public event PresupuestosDatosAceptarEventHandler PresupuestoModificarDatosAceptar;

        public delegate void PresupuestosDatosCancelarEventHandler();
        public event PresupuestosDatosCancelarEventHandler PresupuestoModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.ctrBuscarCliente.BuscarCliente+= new Afiliados.Controles.ClientesDatosCabeceraAjax.ClientesDatosCabeceraAjaxEventHandler(CtrBuscarCliente_BuscarCliente);
            //this.ctrBuscarProductoPopUp.ProductosBuscarSeleccionar += new BuscarProductoPopUp.ProductosBuscarEventHandler(ctrBuscarProductoPopUp_ProductosBuscarSeleccionar);
            
            if (!this.IsPostBack)
            {
                if (this.MiPresupuesto == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
            else
            {
                if (this.GestionControl == Gestion.Agregar)
                    this.PersistirDatosGrilla();
            }
        }

        public void IniciarControl(VTAPresupuestos pPresupuesto, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiPresupuesto = pPresupuesto;
            this.CargarCombos();
            TGEIVA pParamaetro = new TGEIVA();

            pParamaetro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            this.MisIvas = TGEGeneralesF.TGEIVAAlicuotaObtenerListaActiva(pParamaetro);
            hdfIdFilialPredeterminada.Value = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
            hdfIdUsuarioEvento.Value = this.UsuarioActivo.IdUsuarioEvento.ToString();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.txtFechaPresupuesto.Text = DateTime.Now.ToShortDateString();
                    this.MiPresupuesto.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial; 
                    this.IniciarGrilla();
                    if (this.MisParametrosUrl.Contains("IdAfiliado"))
                    {
                        MiPresupuesto.Afiliado.IdAfiliado = Convert.ToInt32(this.MisParametrosUrl["IdAfiliado"].ToString());
                        ctrBuscarCliente.IniciarControl(MiPresupuesto.Afiliado, this.GestionControl);
                        //this.txtNumeroSocio.Text = this.MisParametrosUrl["IdAfiliado"].ToString();
                        //this.txtNumeroSocio_TextChanged(this.txtNumeroSocio, EventArgs.Empty);
                    }
                    phAplicarPorcentaje.Visible = true;
                    txtAplicarPorcentajeDescuento.Attributes.Add("onchange", "AplicarPorcentaje();");
                    break;
                case Gestion.Copiar:
                    this.MiPresupuesto = FacturasF.PresupuestosObtenerDatosCompletos(pPresupuesto);
                    foreach (VTAPresupuestosDetalles det in MiPresupuesto.PresupuestosDetalles)
                    {
                        det.EstadoColeccion = EstadoColecciones.Agregado;
                        det.IdPresupuesto = 0;
                    }
                    this.MiPresupuesto.FechaAlta = DateTime.Now;
                    this.GestionControl = Gestion.Agregar;
                    this.MiPresupuesto.IdPresupuesto = 0;
                    phAplicarPorcentaje.Visible = true;
                    this.MapearObjetoControles(this.MiPresupuesto);
                    if (this.MisParametrosUrl.Contains("UrlReferrer"))
                        this.paginaSegura.viewStatePaginaSegura = this.MisParametrosUrl["UrlReferrer"].ToString();
                    phAgregarItem.Visible = true;
                    break;
                case Gestion.Modificar:
                    // ANULAR
                    this.MiPresupuesto = FacturasF.PresupuestosObtenerDatosCompletos(pPresupuesto);
                    this.MapearObjetoControles(this.MiPresupuesto);
                    this.btnCopiar.Visible = true;
                    break;
                case Gestion.Consultar:
                    this.MiPresupuesto = FacturasF.PresupuestosObtenerDatosCompletos(pPresupuesto);
                    this.MapearObjetoControles(this.MiPresupuesto);
                    ddlMoneda.Enabled = false;

                    //this.txtNumeroSocio.Enabled = false;
                    //this.btnBuscarSocio.Enabled = false;
                    this.txtFechaPresupuesto.Enabled = false;
                    //this.ddlMonedas.Enabled = false;
                    this.txtObservacion.Enabled = false;
                    this.txtContacto.Enabled = false;
                    this.txtTelefono.Enabled = false;
                    this.txtCorreoElectronico.Enabled = false;
                    this.txtFechaEntrega.Enabled = false;
                    this.txtPlazoEntrega.Enabled = false;
                    this.txtGrantia.Enabled = false;
                    this.txtFormaPago.Enabled = false;
                    //this.btnAgregarItem.Visible = false;
                    phAgregarItem.Visible = false;
                    this.btnAceptar.Visible = false;
                    this.btnCopiar.Visible = true;
                    this.btnImprimir.Visible = true;
                    break;
                default:
                    break;
            }
            this.MiPresupuesto.DirPath = this.Request.PhysicalApplicationPath;
            this.MiPresupuesto.AppPath = this.ObtenerAppPath();
        }

        private void CargarCombos()
        {
            //this.ddlCondicionFiscal.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(Generales.Entidades.EnumTGEListasValoresSistemas.CondicionesFiscales);
            //this.ddlCondicionFiscal.DataValueField = "IdListaValorSistemaDetalle";
            //this.ddlCondicionFiscal.DataTextField = "Descripcion";
            //this.ddlCondicionFiscal.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlCondicionFiscal, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            //AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion" ));

            //this.MisMonedas = TGEGeneralesF.MonedasObtenerListaActiva();
            //this.ddlMonedas.DataSource = this.MisMonedas;
            //this.ddlMonedas.DataValueField = "IdMoneda";
            //this.ddlMonedas.DataTextField = "miMonedaDescripcion";
            //this.ddlMonedas.DataBind();
            //if (this.ddlMonedas.Items.Count!=1)
            //    AyudaProgramacion.AgregarItemSeleccione(this.ddlMonedas, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            MisMonedas = TGEGeneralesF.MonedasCotizacionesObtenerCombo();
            this.ddlMoneda.DataSource = MisMonedas;
            this.ddlMoneda.DataValueField = "IdMoneda";
            this.ddlMoneda.DataTextField = "MonedaDescripcionCotizacion";
            this.ddlMoneda.DataBind();
            if (this.ddlMoneda.Items.Count != 1)
                AyudaProgramacion.InsertarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            else
                ddlMoneda_SelectedIndexChanged(ddlMoneda, EventArgs.Empty);
        }

        private void IniciarGrilla()
        {
            VTAPresupuestosDetalles item;
            for (int i = 0; i < 2; i++)
            {
                item = new VTAPresupuestosDetalles();
                item.EstadoColeccion = EstadoColecciones.AgregadoPrevio;
                this.MiPresupuesto.PresupuestosDetalles.Add(item);
                item.IndiceColeccion = this.MiPresupuesto.PresupuestosDetalles.IndexOf(item);
            }
            AyudaProgramacion.CargarGrillaListas<VTAPresupuestosDetalles>(this.MiPresupuesto.PresupuestosDetalles, false, this.gvItems, true);
        }
        protected void ddlMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            MiPresupuesto.PresupuestosDetalles = new List<VTAPresupuestosDetalles>();
            IniciarGrilla();

            if (!string.IsNullOrWhiteSpace(this.ddlMoneda.SelectedValue))
            {
                TGEMonedas moneda = MisMonedas.FirstOrDefault(x => x.IdMoneda == Convert.ToInt32(this.ddlMoneda.SelectedValue));
                SetInitializeCulture(moneda.Moneda);
            }

            upItems.Update();



        }
        private void PersistirDatosGrilla()
        {
            foreach (GridViewRow fila in this.gvItems.Rows)
            {
                if (fila.RowIndex + 1 > this.MiPresupuesto.PresupuestosDetalles.Count)
                    return;

                //string codigo = ((TextBox)fila.FindControl("txtCodigo")).Text;
                DropDownList ddlProducto = ((DropDownList)fila.FindControl("ddlProducto"));
                HiddenField hdfIdProducto = (HiddenField)fila.FindControl("hdfIdProducto");
                HiddenField hdfProductoDetalle = (HiddenField)fila.FindControl("hdfProductoDetalle");
                HiddenField hdfIdListaPrecioDetalle = (HiddenField)fila.FindControl("hdfIdListaPrecioDetalle");
                decimal MargenPorcentaje = ((HiddenField)fila.FindControl("hdfMargenPorcentaje")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfMargenPorcentaje")).Value.ToString().Replace('.', ','));
                decimal Costo = ((HiddenField)fila.FindControl("hdfCosto")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfCosto")).Value.ToString().Replace('.', ','));
                Label lblProductoDescripcion = (Label)fila.FindControl("lblProductoDescripcion");
    
                string descripcion = ((TextBox)fila.FindControl("txtDescripcion")).Text;
                decimal cantidad = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtCantidad")).Decimal;
                decimal precioUnitario = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtPrecioUnitario")).Decimal;
                decimal hdfPrecio = ((HiddenField)fila.FindControl("hdfPrecio")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfPrecio")).Value.ToString().Replace('.', ','));
                //string subTotal = ((Label)fila.FindControl("lblSubtotal")).Text == string.Empty ? "0" : Convert.ToString(((Label)fila.FindControl("lblSubtotal")).Text);
                decimal subTotal = ((HiddenField)fila.FindControl("hdfSubtotal")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfSubtotal")).Value.ToString().Replace('.', ','));
                DropDownList ddlAlicuotaIVA = ((DropDownList)fila.FindControl("ddlAlicuotaIVA"));
                //string importeIva = ((Label)fila.FindControl("lblImporteIva")).Text == string.Empty ? "0" : Convert.ToString(((Label)fila.FindControl("lblImporteIva")).Text);
                //string subTotalIva = ((Label)fila.FindControl("lblSubtotalConIva")).Text == string.Empty ? "0" : Convert.ToString(((Label)fila.FindControl("lblSubtotalConIva")).Text);
                decimal importeIva = ((HiddenField)fila.FindControl("hdfMargenImporte")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfMargenImporte")).Value.ToString().Replace('.', ','));
                decimal subTotalConIva = ((HiddenField)fila.FindControl("hdfSubtotalConIva")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfSubtotalConIva")).Value.ToString().Replace('.', ','));
                decimal porcDesc = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtDescuentoPorcentual")).Decimal;// == string.Empty ? 0 : decimal.Parse(((TextBox)fila.FindControl("txtDescuentoPorcentual")).Text, NumberStyles.Currency);
                Label lblCosto = (Label)fila.FindControl("lblCosto");
                //string importeDesc = ((Label)fila.FindControl("lblDescuentoImporte")).Text == string.Empty ? 0 : Convert.ToString(((Label)fila.FindControl("lblDescuentoImporte")).Text);

                if (hdfIdProducto.Value != string.Empty && Convert.ToInt32(hdfIdProducto.Value)>0)
                {
                    this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex], GestionControl);
                    this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].Estado.IdEstado = (int)Estados.Activo;
                    this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].ListaPrecioDetalle.Producto.IdProducto = Convert.ToInt32(hdfIdProducto.Value);
                    this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].ListaPrecioDetalle.Producto.Descripcion = hdfProductoDetalle.Value;
                    
                    this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].Costo = Costo;
                    this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].MargenImporte = MargenPorcentaje;
                }
                this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].Descripcion = descripcion;
                this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].DescripcionProducto = hdfProductoDetalle.Value;

                if (hdfIdListaPrecioDetalle.Value != string.Empty && Convert.ToInt32(hdfIdListaPrecioDetalle.Value) > 0)
                {
                    this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].ListaPrecioDetalle.IdListaPrecioDetalle = Convert.ToInt32(hdfIdListaPrecioDetalle.Value);
                }
                    //this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].Cantidad = cantidad;
                    if (cantidad > 0)
                {
                    this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].Cantidad = cantidad; //Convert.ToDecimal(cantidad);
                }
        

                this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].PrecioUnitarioSinIva = precioUnitario;

                decimal subTotalParcial = 0;
                decimal descuento=0;
                if (cantidad > 0)
                {
                    subTotalParcial = cantidad * precioUnitario;
                    descuento = Math.Round(subTotalParcial * porcDesc / 100, 2);
                    this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].DescuentoPorcentual = porcDesc;
                    this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].DescuentoImporte = descuento;
                    this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].SubTotal = subTotalParcial - descuento;

                    if (importeIva.ToString() != string.Empty)
                    {
                        this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].ImporteIVA = decimal.Parse(importeIva.ToString(), NumberStyles.Currency);
                    }
                    if (subTotalConIva.ToString() != string.Empty)
                    {
                        this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].SubTotalConIva = decimal.Parse(subTotalConIva.ToString(), NumberStyles.Currency);
                    }
                    if (ddlAlicuotaIVA.SelectedValue != string.Empty)
                    {
                        this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].IVA = this.MisIvas[ddlAlicuotaIVA.SelectedIndex];
                        decimal adicionalIva = Math.Round(((this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].SubTotal.Value * this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].IVA.Alicuota) / 100), 2);
                        this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].ImporteIVA = adicionalIva;
                        this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].SubTotalConIva = this.MiPresupuesto.PresupuestosDetalles[fila.RowIndex].SubTotal.Value + adicionalIva;
                    }
                }
            }
            this.CalcularTotal();
        }

        protected void gvItems_PreRender(object sender, EventArgs e)
        {
            GridView gv = (GridView)sender;

            if ((gv.ShowHeader == true && gv.Rows.Count > 0)
                || (gv.ShowHeaderWhenEmpty == true))
            {
                //Force GridView to use <thead> instead of <tbody> - 11/03/2013 - MCR.
                gv.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            this.MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            if (e.CommandName == "Borrar")
            {
                this.MiPresupuesto.PresupuestosDetalles.RemoveAt(this.MiIndiceDetalleModificar);
                this.MiPresupuesto.PresupuestosDetalles = AyudaProgramacion.AcomodarIndices<VTAPresupuestosDetalles>(this.MiPresupuesto.PresupuestosDetalles);
                AyudaProgramacion.CargarGrillaListas<VTAPresupuestosDetalles>(this.MiPresupuesto.PresupuestosDetalles, false, this.gvItems, true);
                this.CalcularTotal();
            }
            //else if (e.CommandName == "BuscarProducto")
            //{
            //    CMPListasPrecios listaFiltro = new CMPListasPrecios();
            //    listaFiltro.IdAfiliado = this.MiPresupuesto.Afiliado.IdAfiliado;
            //    this.ctrBuscarProductoPopUp.IniciarControl(listaFiltro);
            //}
            //else if (e.CommandName == "AgregarDetalle")
            //{
            //    TextBox desc = (TextBox)this.gvItems.Rows[this.MiIndiceDetalleModificar].FindControl("txtDescripcion");
            //    desc.Visible = !desc.Visible;
            //}
        }

        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                VTAPresupuestosDetalles item = (VTAPresupuestosDetalles)e.Row.DataItem;

                DropDownList ddlAlicuotaIVA = ((DropDownList)e.Row.FindControl("ddlAlicuotaIVA"));
                ddlAlicuotaIVA.DataSource = this.MisIvas;
                ddlAlicuotaIVA.DataValueField = "Alicuota";
                ddlAlicuotaIVA.DataTextField = "Descripcion";
                ddlAlicuotaIVA.DataBind();

                ListItem lstItem;
                if (ddlAlicuotaIVA.Items.Count == 0)
                    AyudaProgramacion.AgregarItemSeleccione(ddlAlicuotaIVA, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                else
                {
                    ddlAlicuotaIVA.SelectedValue = item.IVA.Alicuota.ToString();
                    ddlAlicuotaIVA.Attributes.Add("onchange", "CalcularItem();");
                    ddlAlicuotaIVA.Enabled = true;
                }

                DropDownList ddlProducto = ((DropDownList)e.Row.FindControl("ddlProducto"));
                if (item.ListaPrecioDetalle.Producto.IdProducto > 0)
                    ddlProducto.Items.Add(new ListItem(item.ListaPrecioDetalle.Producto.Descripcion, item.ListaPrecioDetalle.Producto.IdProducto.ToString()));


                if (this.GestionControl == Gestion.Agregar)
                {

                    //ImageButton ibtnProducto = (ImageButton)e.Row.FindControl("btnBuscarProducto");
                    //ibtnProducto.Visible = true;
                    Label lblCosto = (Label)e.Row.FindControl("lblCosto");
                    lblCosto.Visible = true;
                    ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                    btnEliminar.Visible = true;
                    //NumericTextBox codigo = (NumericTextBox)e.Row.FindControl("txtCodigo");
                    //codigo.Enabled = true;
                    ddlProducto.Enabled = true;
                    TextBox txtDescripcion = (TextBox)e.Row.FindControl("txtDescripcion");
                    txtDescripcion.Visible = true;
                    txtDescripcion.Enabled = true;
                    Evol.Controls.CurrencyTextBox cantidad = (Evol.Controls.CurrencyTextBox)e.Row.FindControl("txtCantidad");
                    cantidad.Attributes.Add("onchange", "CalcularItem();");
                    cantidad.Enabled = true;
                    //CurrencyTextBox precioUnitario = (CurrencyTextBox)e.Row.FindControl("txtPrecioUnitario");
                    //precioUnitario.Attributes.Add("onchange", "CalcularItem();");
                    //precioUnitario.Enabled = true;

                    Evol.Controls.CurrencyTextBox descuentoPorcentual = (Evol.Controls.CurrencyTextBox)e.Row.FindControl("txtDescuentoPorcentual");
                    descuentoPorcentual.Attributes.Add("onchange", "CalcularItem();");
                    descuentoPorcentual.Enabled = true;

                    Evol.Controls.CurrencyTextBox importe = (Evol.Controls.CurrencyTextBox)e.Row.FindControl("txtPrecioUnitario");
                    importe.Attributes.Add("onchange", "CalcularItem();");
                    importe.Enabled = true;

                    string numberSymbol = importe.Prefix == string.Empty ? "N" : "C";
                    decimal precioUni = item.PrecioUnitarioSinIva.HasValue ? item.PrecioUnitarioSinIva.Value : 0;
                    importe.Text = precioUni.ToString(string.Concat(numberSymbol, "2"));


                    //AyudaProgramacion.AgregarItemSeleccione(ddlAlicuotaIVA, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                    ////ddlAlicuotaIVA.SelectedValue = item.AlicuotaIVA == null? "0" : item.AlicuotaIVA.Value.ToString();
                    //ddlAlicuotaIVA.Attributes.Add("onchange", "CalcularItem();");
                    //ddlAlicuotaIVA.Enabled = true;

                    //if (item.IVA.IdIVA > 0)
                    //    ddlAlicuotaIVA.SelectedValue = item.IVA.IdIVA.ToString();

                }
                else if (this.GestionControl == Gestion.Consultar)
                {
                    //TextBox txtProductoDescripcion = (TextBox)e.Row.FindControl("txtProductoDescripcion");
                    //txtProductoDescripcion.Visible = false;
                    Label lblCosto = (Label)e.Row.FindControl("lblCosto");
                    lblCosto.Visible = true;
                    TextBox txtDescripcion = (TextBox)e.Row.FindControl("txtDescripcion");
                    txtDescripcion.Visible = false;
                    Label lblProductoDescripcion = (Label)e.Row.FindControl("lblProductoDescripcion");
                    lblProductoDescripcion.Visible = true;
                    Label lblProducto = (Label)e.Row.FindControl("lblProducto");
                    lblProducto.Visible = true;
                    lblProducto.Enabled = true;
                    ddlProducto.Visible = false;

                    lstItem = ddlAlicuotaIVA.Items.FindByValue(item.IVA.Alicuota.ToString());
                    if (lstItem == null)
                        ddlAlicuotaIVA.Items.Add(new ListItem(item.IVA.Descripcion, item.IVA.Alicuota.ToString()));
                    ddlAlicuotaIVA.SelectedValue = item.IVA.Alicuota.ToString();
                    ddlAlicuotaIVA.Enabled = false;
                }
            }
        }

        //void ctrBuscarProductoPopUp_ProductosBuscarSeleccionar(CMPListasPreciosDetalles e)
        //{
        //    this.MiPresupuesto.PresupuestosDetalles[this.MiIndiceDetalleModificar].ListaPrecioDetalle = e;
        //    this.MiPresupuesto.PresupuestosDetalles[this.MiIndiceDetalleModificar].DescripcionProducto = e.Producto.Descripcion;
        //    this.MiPresupuesto.PresupuestosDetalles[this.MiIndiceDetalleModificar].PrecioUnitarioSinIva = this.MiPresupuesto.PresupuestosDetalles[this.MiIndiceDetalleModificar].ListaPrecioDetalle.CalcularPrecio(Convert.ToInt32(1));
        //    this.MiPresupuesto.PresupuestosDetalles[this.MiIndiceDetalleModificar].Costo = e.Precio;
        //    this.MiPresupuesto.PresupuestosDetalles[this.MiIndiceDetalleModificar].MargenPorcentaje = e.MargenPorcentaje.HasValue? e.MargenPorcentaje.Value : default(decimal?);
        //    AyudaProgramacion.CargarGrillaListas<VTAPresupuestosDetalles>(this.MiPresupuesto.PresupuestosDetalles, false, this.gvItems, true);
        //    this.upItems.Update();
        //}


        //protected void txtCodigo_TextChanged(object sender, EventArgs e)
        //{
        //    GridViewRow row = ((GridViewRow)((TextBox)sender).Parent.Parent);
        //    int IndiceColeccion = row.RowIndex;

        //    string contenido = ((TextBox)sender).Text;
        //    this.MiPresupuesto.PresupuestosDetalles[IndiceColeccion].ListaPrecioDetalle.Producto.IdProducto = Convert.ToInt32(contenido);
        //    this.MiPresupuesto.PresupuestosDetalles[IndiceColeccion].ListaPrecioDetalle.Fecha = DateTime.Now;
        //    this.MiPresupuesto.PresupuestosDetalles[IndiceColeccion].ListaPrecioDetalle.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;

        //    this.MiPresupuesto.PresupuestosDetalles[IndiceColeccion].ListaPrecioDetalle = ComprasF.ListasPreciosDetallesObtenerDatosPorProducto(this.MiPresupuesto.PresupuestosDetalles[IndiceColeccion].ListaPrecioDetalle);
        //    this.MiPresupuesto.PresupuestosDetalles[IndiceColeccion].DescripcionProducto = this.MiPresupuesto.PresupuestosDetalles[IndiceColeccion].ListaPrecioDetalle.Producto.Descripcion;
        //    this.MiPresupuesto.PresupuestosDetalles[IndiceColeccion].PrecioUnitarioSinIva = this.MiPresupuesto.PresupuestosDetalles[IndiceColeccion].ListaPrecioDetalle.CalcularPrecio(Convert.ToInt32(1));
        //    this.MiPresupuesto.PresupuestosDetalles[IndiceColeccion].Costo = this.MiPresupuesto.PresupuestosDetalles[IndiceColeccion].ListaPrecioDetalle.Precio;
        //    this.MiPresupuesto.PresupuestosDetalles[IndiceColeccion].MargenPorcentaje = this.MiPresupuesto.PresupuestosDetalles[IndiceColeccion].ListaPrecioDetalle.MargenPorcentaje.HasValue ? this.MiPresupuesto.PresupuestosDetalles[IndiceColeccion].ListaPrecioDetalle.MargenPorcentaje.Value : default(decimal?);
        //    AyudaProgramacion.CargarGrillaListas<VTAPresupuestosDetalles>(this.MiPresupuesto.PresupuestosDetalles, false, this.gvItems, true);
        //}


        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {
            this.PersistirDatosGrilla();
            this.AgregarItem();
            txtCantidadAgregar.Text = string.Empty;
            //VTAPresupuestosDetalles item;
            //item = new VTAPresupuestosDetalles();
            //this.MiPresupuesto.PresupuestosDetalles.Add(item);
            //item.IndiceColeccion = this.MiPresupuesto.PresupuestosDetalles.IndexOf(item);
            //AyudaProgramacion.CargarGrillaListas<VTAPresupuestosDetalles>(this.MiPresupuesto.PresupuestosDetalles, false, this.gvItems, true);
        }

        private void AgregarItem()
        {
            VTAPresupuestosDetalles item;
            if (this.txtCantidadAgregar.Text == string.Empty || txtCantidadAgregar.Text == "0")
            {
                txtCantidadAgregar.Text = "1";
            }
            int cantidad = Convert.ToInt32(this.txtCantidadAgregar.Text);
            for (int i = 0; i < cantidad; i++)
            {
                item = new VTAPresupuestosDetalles();
                this.MiPresupuesto.PresupuestosDetalles.Add(item);
                item.IndiceColeccion = this.MiPresupuesto.PresupuestosDetalles.IndexOf(item);
                item.DescuentoPorcentual = txtAplicarPorcentajeDescuento.Decimal;
                item.IdPresupuestoDetalle = item.IndiceColeccion * -1;
                //this.gvItems.Rows[item.IndiceColeccion].FindControl("ddlProducto").Focus();

            }
            AyudaProgramacion.CargarGrillaListas<VTAPresupuestosDetalles>(this.MiPresupuesto.PresupuestosDetalles, false, this.gvItems, true);
            //ScriptManager.RegisterStartupScript(this.upDetalleGastos, this.upDetalleGastos.GetType(), "CalcularPrecioScript", "CalcularPrecio();", true);
        }

        #region Control Clientes
        void CtrBuscarCliente_BuscarCliente(AfiAfiliados pAfiliado)
        {
            if (pAfiliado.IdAfiliado > 0)
            {
                pAfiliado = AfiliadosF.AfiliadosObtenerDatosCompletos(pAfiliado);
                txtTelefono.Text = pAfiliado.TelefonoPredeterminado;
                txtCorreoElectronico.Text = pAfiliado.CorreoElectronico;
                UpdatePanel1.Update();
                MiPresupuesto.Afiliado = ctrBuscarCliente.MiAfiliado;
                MiPresupuesto.CondicionFiscal.IdCondicionFiscal = ctrBuscarCliente.MiAfiliado.CondicionFiscal.IdCondicionFiscal;
                MiPresupuesto.RazonSocial = ctrBuscarCliente.MiAfiliado.RazonSocial;
            }
            else
            { }
        }

        //private void MapearObjetoAControlesAfiliado(VTAPresupuestos pPresupuesto)
        //{
        //    this.txtNumeroSocio.Text = pPresupuesto.Afiliado.IdAfiliado.ToString();
        //    this.txtSocio.Text = pPresupuesto.RazonSocial;
        //    this.txtTelefono.Text = pPresupuesto.Telefono;
        //    this.txtCorreoElectronico.Text = pPresupuesto.CorreoElectronico;
        //    this.ddlCondicionFiscal.SelectedValue = pPresupuesto.CondicionFiscal.IdCondicionFiscal == 0 ? string.Empty : pPresupuesto.CondicionFiscal.IdCondicionFiscal.ToString();
        //}
        
        //protected void txtNumeroSocio_TextChanged(object sender, EventArgs e)
        //{
        //    string txtNumeroSocio = ((TextBox)sender).Text;
        //    AfiAfiliados parametro = new AfiAfiliados();
        //    parametro.IdAfiliado = Convert.ToInt32(txtNumeroSocio);
        //    AfiAfiliados pAfiliado = AfiliadosF.AfiliadosObtenerDatosCompletos(parametro);
        //    if (pAfiliado.IdAfiliado != 0)
        //    {
        //        this.MiPresupuesto.Afiliado.IdAfiliado = pAfiliado.IdAfiliado;
        //        this.MiPresupuesto.RazonSocial = pAfiliado.Apellido;
        //        this.MiPresupuesto.CondicionFiscal = pAfiliado.CondicionFiscal;
        //        this.MiPresupuesto.Telefono = pAfiliado.TelefonoPredeterminado;
        //        this.MiPresupuesto.CorreoElectronico = pAfiliado.CorreoElectronico;
        //        this.MapearObjetoAControlesAfiliado(this.MiPresupuesto);
        //        this.btnBuscarSocio.Visible = false;
        //    }
        //    else
        //    {
        //        this.txtNumeroSocio.Text = string.Empty;
        //        parametro.CodigoMensaje = "ValidarNumeroClienteNoExiste";
        //        this.MostrarMensaje(parametro.CodigoMensaje, true);
        //    }
        //    this.UpdatePanel2.Update();
        //}

        //protected void btnBuscarCliente_Click(object sender, EventArgs e)
        //{
        //    this.ctrBuscarClientePopUp.IniciarControl(true);
        //}

        //protected void btnLimpiar_Click(object sender, EventArgs e)
        //{
        //    this.MiPresupuesto.Afiliado.IdAfiliado = 0;
        //    this.MiPresupuesto.RazonSocial = string.Empty;
        //    this.MiPresupuesto.Telefono = string.Empty;
        //    this.MiPresupuesto.CorreoElectronico = string.Empty;
        //    this.MapearObjetoAControlesAfiliado(this.MiPresupuesto);
        //    this.btnLimpiar.Visible = false;
        //    this.btnBuscarSocio.Visible = true;
        //    this.UpdatePanel1.Update();
        //}
        #endregion
        protected void MapearControlesAObjeto(VTAPresupuestos pPresupuesto)
        {
            pPresupuesto.FechaAlta = Convert.ToDateTime(this.txtFechaPresupuesto.Text);
            //pFactura.Moneda = this.MisMonedas[this.ddlMonedas.SelectedIndex];
            pPresupuesto.CondicionFiscal.IdCondicionFiscal = ctrBuscarCliente.MiAfiliado.CondicionFiscal.IdCondicionFiscal; //this.ddlCondicionFiscal.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlCondicionFiscal.SelectedValue);
            pPresupuesto.Descripcion = this.txtObservacion.Text;
            pPresupuesto.RazonSocial = ctrBuscarCliente.MiAfiliado.RazonSocial;
            pPresupuesto.Contacto = this.txtContacto.Text;
            pPresupuesto.Telefono = this.txtTelefono.Text;
            pPresupuesto.CorreoElectronico = this.txtCorreoElectronico.Text;
            pPresupuesto.FechaEntrega = this.txtFechaEntrega.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaEntrega.Text);
            pPresupuesto.PlazoEntrega = this.txtPlazoEntrega.Text;
            pPresupuesto.Garantia = this.txtGrantia.Text;
            pPresupuesto.FormaPago = this.txtFormaPago.Text;
            MiPresupuesto.Moneda.IdMoneda = Convert.ToInt32(this.ddlMoneda.SelectedValue);
            TGEMonedas moneda = MisMonedas.FirstOrDefault(x => x.IdMoneda == Convert.ToInt32(this.ddlMoneda.SelectedValue));
            if (moneda.IdMoneda > 0)
            {
                MiPresupuesto.Moneda = moneda;
                MiPresupuesto.MonedaCotizacion = moneda.MonedeaCotizacion.MonedaCotizacion;
            }

            this.PersistirDatosGrilla();
        }

        private void MapearObjetoControles(VTAPresupuestos pPresupuesto)
        {
            //this.MapearObjetoAControlesAfiliado(pPresupuesto);
            //this.txtFilialDescripcion.Text = pFactura.Filial.Filial;
            this.txtFechaPresupuesto.Text = pPresupuesto.FechaAlta.ToShortDateString();
            this.txtContacto.Text = pPresupuesto.Contacto;
            //ListItem item = this.ddlCondicionFiscal.Items.FindByValue(pPresupuesto.CondicionFiscal.IdCondicionFiscal.ToString());
            //if (item != null)
            //    this.ddlCondicionFiscal.SelectedValue = pPresupuesto.CondicionFiscal.IdCondicionFiscal.ToString();

            //this.ddlMonedas.SelectedValue = pPresupuesto.Moneda.IdMoneda.ToString();
            this.txtTelefono.Text = pPresupuesto.Telefono;
            this.txtCorreoElectronico.Text = pPresupuesto.CorreoElectronico;
            this.txtObservacion.Text = pPresupuesto.Descripcion;
            this.txtFechaEntrega.Text = pPresupuesto.FechaEntrega.HasValue ? pPresupuesto.FechaEntrega.Value.ToShortDateString() : string.Empty;
            this.txtPlazoEntrega.Text = pPresupuesto.PlazoEntrega;
            this.txtGrantia.Text = pPresupuesto.Garantia;
            this.txtFormaPago.Text = pPresupuesto.FormaPago;

            if (MisMonedas.Exists(x => x.IdMoneda == MiPresupuesto.Moneda.IdMoneda))
                MisMonedas.First(x => x.IdMoneda == MiPresupuesto.Moneda.IdMoneda).MonedeaCotizacion.MonedaCotizacion = MiPresupuesto.MonedaCotizacion;
            else if (MiPresupuesto.Moneda.IdMoneda > 0)
                MisMonedas.Add(MiPresupuesto.Moneda);
            this.ddlMoneda.Items.Clear();
            this.ddlMoneda.SelectedIndex = -1;
            this.ddlMoneda.SelectedValue = null;
            this.ddlMoneda.ClearSelection();
            this.ddlMoneda.DataSource = MisMonedas;
            this.ddlMoneda.DataValueField = "IdMoneda";
            this.ddlMoneda.DataTextField = "MonedaDescripcionCotizacion";
            this.ddlMoneda.DataBind();
            if (this.ddlMoneda.Items.Count != 1)
                AyudaProgramacion.InsertarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlMoneda.SelectedValue = MiPresupuesto.Moneda.IdMoneda > 0 ? MiPresupuesto.Moneda.IdMoneda.ToString() : string.Empty;
            TGEMonedas moneda = MisMonedas.FirstOrDefault(x => x.IdMoneda == Convert.ToInt32(this.ddlMoneda.SelectedValue));
            SetInitializeCulture(moneda.Moneda);


            AyudaProgramacion.CargarGrillaListas<VTAPresupuestosDetalles>(pPresupuesto.PresupuestosDetalles, false, this.gvItems, true);
            this.CalcularTotal();
            ctrBuscarCliente.IniciarControl(pPresupuesto.Afiliado, this.GestionControl);
        }

        protected void CalcularTotal()
        {
            decimal? totalSinIva = 0;
            decimal? totalIva = 0;
            decimal? totalConIva = 0;

            totalSinIva = this.MiPresupuesto.PresupuestosDetalles.Sum(x => x.SubTotal == null ? 0 : x.SubTotal);
            totalIva = this.MiPresupuesto.PresupuestosDetalles.Sum(x => x.ImporteIVA == null ? 0 : x.ImporteIVA);
            totalConIva = totalSinIva + totalIva;

            this.MiPresupuesto.ImporteSinIVA = totalSinIva.Value;
            this.MiPresupuesto.IvaTotal = totalIva.Value;
            this.MiPresupuesto.ImporteTotal = totalConIva.Value;

            this.txtTotalConIva.Text = totalConIva.Value.ToString("C2");
            this.txtTotalSinIva.Text = totalSinIva.Value.ToString("C2");
            this.txtTotalIva.Text = totalIva.Value.ToString("C2");
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.btnAceptar.Visible = false;

            this.MapearControlesAObjeto(this.MiPresupuesto);

            this.MiPresupuesto.DirPath = this.Request.PhysicalApplicationPath;
            this.MiPresupuesto.AppPath = this.ObtenerAppPath();
            this.MiPresupuesto.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiPresupuesto.PresupuestosDetalles = this.MiPresupuesto.PresupuestosDetalles.Where(x => x.EstadoColeccion == EstadoColecciones.Agregado).ToList();
                    AyudaProgramacion.AcomodarIndices<VTAPresupuestosDetalles>(this.MiPresupuesto.PresupuestosDetalles);
                    AyudaProgramacion.CargarGrillaListas(this.MiPresupuesto.PresupuestosDetalles, false, this.gvItems, true);
                    this.upItems.Update();
                    //this.MiPresupuesto.IdUsuarioAlta = this.MiPresupuesto.UsuarioLogueado.IdUsuario;
                    this.MiPresupuesto.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    guardo = FacturasF.PresupuestosAgregar(this.MiPresupuesto);
                    break;
                case Gestion.Modificar:
                    guardo = FacturasF.PresupuestosModificar(this.MiPresupuesto);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.btnImprimir.Visible = true;
                this.MostrarArchivo();
            }
            else
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiPresupuesto.CodigoMensaje, true, this.MiPresupuesto.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.PresupuestoModificarDatosCancelar != null)
                this.PresupuestoModificarDatosCancelar();
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            this.MostrarArchivo();
        }

        protected void btnCopiar_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", Gestion.Copiar);
            this.MisParametrosUrl.Add("IdPresupuesto", this.MiPresupuesto.IdPresupuesto);
            this.MisParametrosUrl.Add("IdAfiliado", this.MiPresupuesto.Afiliado.IdAfiliado);
            if (!string.IsNullOrEmpty(this.paginaSegura.viewStatePaginaSegura))
                this.MisParametrosUrl.Add("UrlReferrer", this.paginaSegura.viewStatePaginaSegura.ToString());
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/PresupuestosAgregar.aspx"), true);
        }

        private void MostrarArchivo()
        {
            List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
            TGEArchivos archivo = new TGEArchivos();
            VTAPresupuestos presupuestoPDF = new VTAPresupuestos();
            presupuestoPDF = FacturasF.PresupuestosObtenerArchivo(this.MiPresupuesto);
            archivo.Archivo = presupuestoPDF.PresupuestoPDF;
            listaArchivos.Add(archivo);
            TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();
            string nombreArchivo = string.Concat(empresa.CUIT, "_Presupuesto_", this.MiPresupuesto.IdPresupuesto.ToString().PadLeft(10, '0'), ".pdf");
            ExportPDF.ConvertirArchivoEnPdf(this.UpdatePanel3, listaArchivos, nombreArchivo);
        }  
    }
}