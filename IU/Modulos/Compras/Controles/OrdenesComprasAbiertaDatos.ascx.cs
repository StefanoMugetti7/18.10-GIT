using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Compras.Entidades;
using Comunes.Entidades;
using Proveedores.Entidades;
using Proveedores;
using Compras;
using Afiliados.Entidades;
using Cargos.Entidades;
using Evol.Controls;

namespace IU.Modulos.Compras.Controles
{
    public partial class OrdenesComprasAbiertaDatos : ControlesSeguros
    {
        public delegate void OrdenesComprasDatosAceptarEventHandler(object sender, CmpOrdenesCompras e);
        public event OrdenesComprasDatosAceptarEventHandler OrdenesComprasModificarDatosAceptar;

        public delegate void OrdenesComprasDatosCancelarEventHandler();
        public event OrdenesComprasDatosCancelarEventHandler OrdenesComprasModificarDatosCancelar;

        private CmpOrdenesCompras MiOrdenCompra
        {
            get { return (CmpOrdenesCompras)Session[this.MiSessionPagina + "OrdenCompraDatosMiOrden"]; }
            set { Session[this.MiSessionPagina + "OrdenCompraDatosMiOrden"] = value; }
        }

        private List<TGEIVA> MisIvas
        {
            get { return (List<TGEIVA>)Session[this.MiSessionPagina + "SolicitudesPagosDatosMisIvas"]; }
            set { Session[this.MiSessionPagina + "SolicitudesPagosDatosMisIvas"] = value; }
        }

        private List<TGEMonedas> MisMonedas
        {
            get { return (List<TGEMonedas>)Session[this.MiSessionPagina + "SolicitudesPagosDatosMisMonedas"]; }
            set { Session[this.MiSessionPagina + "SolicitudesPagosDatosMisMonedas"] = value; }
        }

        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "SolicitudModificarDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "SolicitudModificarDatosMiIndiceDetalleModificar"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            ctrBuscarProveedor.BuscarProveedor += new Proveedores.Controles.ProveedoresCabecerasDatos.ProveedoresDatosCabeceraAjaxEventHandler(CtrBuscarProveedor_BuscarProveedor);
            //this.ctrBuscarProductoPopUp.ProductosBuscarSeleccionar +=new CuentasPagar.Controles.BuscarProductoPopUp.ProductosBuscarEventHandler(ctrBuscarProductoPopUp_ProductosBuscarSeleccionar);
            if (!this.IsPostBack)
            {
                if (this.MiOrdenCompra == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);

                }

            }
            //else
            //{
            //    this.PersistirDatosGrilla();
            //}

        }

        void CtrBuscarProveedor_BuscarProveedor(CapProveedores e)
        {
            MiOrdenCompra.Proveedor.IdProveedor = ctrBuscarProveedor.MiProveedor.IdProveedor;
            //this.UpdatePanelProovedor.Update();
        }

        public void IniciarControl(CmpOrdenesCompras pOrden, Gestion pGestion)
        {
            this.MiOrdenCompra = pOrden;
            this.MisIvas = TGEGeneralesF.TGEIVAAlicuotaObtenerLista();
            this.MisMonedas = TGEGeneralesF.MonedasObtenerLista();
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.txtObservacion.Enabled = true;

                    //this.rfvFechaEntrega.Enabled = false;
                    this.pnlCuotas.Visible = true;
                    this.ddlTipoOrden.SelectedValue = ((int)EnumTiposOrdenesCompras.Terceros).ToString();
                    this.ddlTipoOrden.Enabled = false;
                    this.IniciarGrilla();
                    
                    break;
                case Gestion.Autorizar:
                    //this.ddlCondicionPago.Enabled = false;
                    this.ddlTipoOrden.Enabled = false;
                    //this.btnAgregarItem.Visible = false;
                    //this.rfvFechaEntrega.Enabled = false;
                    
                    this.txtCuotasDescuentoAfiliado.Enabled = false;
                    this.txtCuotasDescuentoProveedor.Enabled = false;

                    this.MiOrdenCompra= ComprasF.OrdenCompraObtenerDatosCompletos(this.MiOrdenCompra);
                    this.MapearObjetoControles(this.MiOrdenCompra);
                    break;
                case Gestion.Anular:
                    //this.ddlCondicionPago.Enabled = false;
                    this.ddlTipoOrden.Enabled = false;
                    //this.rfvFechaEntrega.Enabled = false;
                    //this.btnAgregarItem.Visible = false;
                    this.ddlFormasCobros.Enabled = false;
                    
                    this.txtCuotasDescuentoAfiliado.Enabled = false;
                    this.txtCuotasDescuentoProveedor.Enabled = false;

                    this.MiOrdenCompra= ComprasF.OrdenCompraObtenerDatosCompletos(this.MiOrdenCompra);
                    this.MapearObjetoControles(this.MiOrdenCompra);
                    break;
                case Gestion.Consultar:
                    //this.ddlCondicionPago.Enabled = false;
                    this.ddlTipoOrden.Enabled = false;
                    //this.rfvFechaEntrega.Enabled = false;
                    //this.btnAgregarItem.Visible = false;
                    this.ddlFormasCobros.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.txtCuotasDescuentoAfiliado.Enabled = false;
                    this.txtCuotasDescuentoProveedor.Enabled = false;
                    this.MiOrdenCompra= ComprasF.OrdenCompraObtenerDatosCompletos(this.MiOrdenCompra);
                    this.MapearObjetoControles(this.MiOrdenCompra);
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            //this.ddlCondicionPago.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValores.CondicionesPagos);
            //this.ddlCondicionPago.DataValueField = "IdListaValorDetalle";
            //this.ddlCondicionPago.DataTextField = "Descripcion";
            //this.ddlCondicionPago.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlCondicionPago, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTipoOrden.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposOrdenesCompras);
            this.ddlTipoOrden.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTipoOrden.DataTextField = "Descripcion";
            this.ddlTipoOrden.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOrden, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            TGEFormasCobrosAfiliados formasCobroAfi = new TGEFormasCobrosAfiliados();
            formasCobroAfi.IdAfiliado = this.MiOrdenCompra.Afiliado.IdAfiliado;
            formasCobroAfi.IdTipoCargo = (int)EnumTiposCargos.OrdenesComprasTerceros;
            this.ddlFormasCobros.DataSource = TGEGeneralesF.FormasCobrosAfiliadosObtenerPorAfiliadoTipoCargo(formasCobroAfi);
            this.ddlFormasCobros.DataValueField = "IdFormaCobroAfiliado";
            this.ddlFormasCobros.DataTextField = "FormaCobroDescripcion";
            this.ddlFormasCobros.DataBind();
            
        }

        #region Grilla

        private void PersistirDatosGrilla()
        {
            string codigo, cantidad;// descripcion;
            decimal precioUnitarioSinIva;
            DropDownList ddlAlicuotaIVA, ddlMonedas;
            int plazoEntrega;
            //decimal subtotalConIVA = 0;
            foreach (GridViewRow fila in this.gvItems.Rows)
            {
    
                codigo = ((HiddenField)fila.FindControl("hdfIdProducto")).Value;

                cantidad = ((TextBox)fila.FindControl("txtCantidad")).Text;
                //descripcion = fila..FindControl("txtProducto")).Text;
                precioUnitarioSinIva = ((CurrencyTextBox)fila.FindControl("txtPrecioUnitario")).Decimal;
                //subTotal = ((Label)fila.FindControl("lblSubtotal")).Text == string.Empty ? "0" : Convert.ToString(((Label)fila.FindControl("lblSubtotal")).Text);
                ddlAlicuotaIVA = ((DropDownList)fila.FindControl("ddlAlicuotaIVA"));
                ddlMonedas = ((DropDownList)fila.FindControl("ddlMonedas"));
                //importeIva = ((Label)fila.FindControl("lblImporteIva")).Text == string.Empty ? "0" : Convert.ToString(((Label)fila.FindControl("lblImporteIva")).Text);
                //string subTotalIva = ((Label)fila.FindControl("lblSubtotalConIva")).Text == string.Empty ? "0" : Convert.ToString(((Label)fila.FindControl("lblSubtotalConIva")).Text);
                plazoEntrega = ((TextBox)fila.FindControl("txtPlazoEntrega")).Text == string.Empty ? 0 : int.Parse(((TextBox)fila.FindControl("txtPlazoEntrega")).Text);
                //int plazoEntrega = ((TextBox)fila.FindControl("txtPlazoEntrega")).Text == string.Empty ? 0 : int.Parse(((TextBox)fila.FindControl("txtPlazoEntrega")).Text, NumberStyles.Currency);
                //NumberStyles.Currency para sacar el signo $     

                if (codigo != string.Empty && Convert.ToInt32(codigo)>0)
                {
                    this.MiOrdenCompra.OrdenesComprasDetalles[fila.RowIndex].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiOrdenCompra.OrdenesComprasDetalles[fila.RowIndex], this.GestionControl);
                    this.MiOrdenCompra.OrdenesComprasDetalles[fila.RowIndex].Producto.IdProducto = Convert.ToInt32(codigo);
                }

                if (cantidad != string.Empty)
                {
                    this.MiOrdenCompra.OrdenesComprasDetalles[fila.RowIndex].Cantidad = Convert.ToDecimal(cantidad);
                }
                //if (descripcion != string.Empty)
                //{
                //    this.MiOrdenCompra.OrdenesComprasDetalles[fila.RowIndex].Descripcion = descripcion;
                //}
                if (ddlMonedas.SelectedValue != "")
                {
                    this.MiOrdenCompra.OrdenesComprasDetalles[fila.RowIndex].Moneda = this.MisMonedas[ddlMonedas.SelectedIndex];
                }
                //if (precioUnitarioSinIva != string.Empty)
                //{
                this.MiOrdenCompra.OrdenesComprasDetalles[fila.RowIndex].Precio = precioUnitarioSinIva;//Convert.ToDecimal(precioUnitarioSinIva);
                //}
                if (plazoEntrega != 0)
                {
                    this.MiOrdenCompra.OrdenesComprasDetalles[fila.RowIndex].PlazoEntrega = plazoEntrega;
                }

                if (ddlAlicuotaIVA.SelectedValue != "")
                {
                    this.MiOrdenCompra.OrdenesComprasDetalles[fila.RowIndex].IVA = this.MisIvas[ddlAlicuotaIVA.SelectedIndex];
                }
                this.MiOrdenCompra.OrdenesComprasDetalles[fila.RowIndex].Importe = this.MiOrdenCompra.OrdenesComprasDetalles[fila.RowIndex].Precio * this.MiOrdenCompra.OrdenesComprasDetalles[fila.RowIndex].Cantidad;
                this.MiOrdenCompra.OrdenesComprasDetalles[fila.RowIndex].ImporteIVA = Math.Round( this.MiOrdenCompra.OrdenesComprasDetalles[fila.RowIndex].Importe * this.MiOrdenCompra.OrdenesComprasDetalles[fila.RowIndex].IVA.Alicuota / 100, 2);

                ((Label)fila.FindControl("lblImporteIva")).Text = (this.MiOrdenCompra.OrdenesComprasDetalles[fila.RowIndex].ImporteIVA).ToString("C2");
                ((Label)fila.FindControl("lblImporte")).Text = (this.MiOrdenCompra.OrdenesComprasDetalles[fila.RowIndex].Importe).ToString("C2");
                ((Label)fila.FindControl("lblImporteConIva")).Text = (this.MiOrdenCompra.OrdenesComprasDetalles[fila.RowIndex].ImporteConIVA).ToString("C2");

            }
            this.CalcularTotal();
        }

        private void IniciarGrilla()
        {
            CmpOrdenesComprasDetalles item;
            for (int i = 0; i < 1; i++)
            {
                item = new CmpOrdenesComprasDetalles();
                this.MiOrdenCompra.OrdenesComprasDetalles.Add(item);
                item.IndiceColeccion = this.MiOrdenCompra.OrdenesComprasDetalles.IndexOf(item);
                item.Producto.IdProducto = (int)EnumProductos.VariosGenerico;
                item.Cantidad = 1;
                item.Producto = ComprasF.ProductosObtenerPorIdProducto(item.Producto);
                item.Descripcion = item.Producto.Descripcion;
            }
            AyudaProgramacion.CargarGrillaListas<CmpOrdenesComprasDetalles>(this.MiOrdenCompra.OrdenesComprasDetalles, false, this.gvItems, true);
        }

        protected void txtCodigoProducto_TextChanged(object sender, EventArgs e)
        {
            //GridViewRow row = ((GridViewRow)((TextBox)sender).Parent.Parent);
            //int IndiceColeccion = row.RowIndex;

            //string contenido = ((TextBox)sender).Text;
            //this.MiOrdenCompra.OrdenesComprasDetalles[IndiceColeccion].Producto.IdProducto = Convert.ToInt32(contenido);
            //this.MiOrdenCompra.OrdenesComprasDetalles[IndiceColeccion].Producto = ComprasF.ProductosObtenerPorIdProducto(this.MiOrdenCompra.OrdenesComprasDetalles[IndiceColeccion].Producto);
            //if (this.MiOrdenCompra.OrdenesComprasDetalles[IndiceColeccion].Producto.IdProducto == 0)
            //{
            //    this.MiIndiceDetalleModificar = IndiceColeccion;
            //    this.ctrBuscarProductoPopUp.IniciarControl();
            //}
            //else
            //{
            //    this.MiOrdenCompra.OrdenesComprasDetalles[IndiceColeccion].Descripcion = this.MiOrdenCompra.OrdenesComprasDetalles[IndiceColeccion].Producto.Descripcion;
            //    AyudaProgramacion.CargarGrillaListas<CmpOrdenesComprasDetalles>(this.MiOrdenCompra.OrdenesComprasDetalles, false, this.gvItems, true);
            //}

        }

        //void ctrBuscarProductoPopUp_ProductosBuscarSeleccionar(CMPProductos e)
        //{

        //    //AyudaProgramacion.MatchObjectProperties(e, this.MiFactura.FacturasDetalles[this.MiIndiceDetalleModificar].Producto);
        //    this.MiOrdenCompra.OrdenesComprasDetalles[this.MiIndiceDetalleModificar].Producto = e;
        //    this.MiOrdenCompra.OrdenesComprasDetalles[this.MiIndiceDetalleModificar].Descripcion = e.Descripcion;
        //    AyudaProgramacion.CargarGrillaListas<CmpOrdenesComprasDetalles>(this.MiOrdenCompra.OrdenesComprasDetalles, false, this.gvItems, true);
        //    this.upOrdenCompraDetalle.Update();
        //}

        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ////int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            //this.MiIndiceDetalleModificar = Convert.ToInt32(e.CommandArgument);
            //if (e.CommandName == "Borrar")
            //{
            //    //ELIMINA FILA
            //    //int indiceColeccion = Convert.ToInt32(e.CommandArgument);
            //    this.MiOrdenCompra.OrdenesComprasDetalles.RemoveAt(this.MiIndiceDetalleModificar);
            //    this.MiOrdenCompra.OrdenesComprasDetalles = AyudaProgramacion.AcomodarIndices<CmpOrdenesComprasDetalles>(this.MiOrdenCompra.OrdenesComprasDetalles);
            //    AyudaProgramacion.CargarGrillaListas<CmpOrdenesComprasDetalles>(this.MiOrdenCompra.OrdenesComprasDetalles, false, this.gvItems, true);
            //    this.CalcularTotal();
            //}
            //if (e.CommandName == "BuscarProducto")
            //{

            //    this.ctrBuscarProductoPopUp.IniciarControl();
            //}
        }

        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                CmpOrdenesComprasDetalles item = (CmpOrdenesComprasDetalles)e.Row.DataItem;
                //SKP.ASP.Controls.NumericTextBox codigo = (SKP.ASP.Controls.NumericTextBox)e.Row.FindControl("txtCodigoProducto");
                //NumericTextBox descuentoImporte = (NumericTextBox)e.Row.FindControl("txtDescuentoImporte");
                DropDownList ddlMonedas = ((DropDownList)e.Row.FindControl("ddlMonedas"));
                CurrencyTextBox PrecioUnitario = (CurrencyTextBox)e.Row.FindControl("txtPrecioUnitario");
                DropDownList ddlAlicuotaIVA = ((DropDownList)e.Row.FindControl("ddlAlicuotaIVA"));
                SKP.ASP.Controls.NumericTextBox cantidad = (SKP.ASP.Controls.NumericTextBox)e.Row.FindControl("txtCantidad");
                SKP.ASP.Controls.NumericTextBox plazo = (SKP.ASP.Controls.NumericTextBox)e.Row.FindControl("txtPlazoEntrega");
                //TextBox producto = (TextBox)e.Row.FindControl("txtProducto");
                //ImageButton btnBuscarProducto = (ImageButton)e.Row.FindControl("btnBuscarProducto");
                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                DropDownList ddlProducto = ((DropDownList)e.Row.FindControl("ddlProducto"));
                if (item.Producto.IdProducto > 0)
                    ddlProducto.Items.Add(new ListItem(item.Producto.Descripcion, item.Producto.IdProducto.ToString()));


                switch (GestionControl)
                {
                    case Gestion.Agregar:
                        PrecioUnitario.Attributes.Add("onchange", "CalcularItem();");
                        cantidad.Attributes.Add("onchange", "CalcularItem();");
                        #region IVA
                        ddlAlicuotaIVA.DataSource = this.MisIvas;
                        ddlAlicuotaIVA.DataValueField = "Alicuota";
                        ddlAlicuotaIVA.DataTextField = "Descripcion";
                        ddlAlicuotaIVA.DataBind();
                        if (ddlAlicuotaIVA.Items.Count != 1)
                            AyudaProgramacion.AgregarItemSeleccione(ddlAlicuotaIVA, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                        //ddlAlicuotaIVA.SelectedValue = item.IdIVA.ToString();
                        ddlAlicuotaIVA.Attributes.Add("onchange", "CalcularItem();");
                        #endregion

                        #region Moneda
                        ddlMonedas.DataSource = this.MisMonedas;
                        ddlMonedas.DataValueField = "IdMoneda";
                        ddlMonedas.DataTextField = "Moneda";
                        ddlMonedas.DataBind();
                        if (ddlMonedas.Items.Count > 1)
                            AyudaProgramacion.AgregarItemSeleccione(ddlMonedas, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                        //ddlMonedas.SelectedValue = item.Moneda.miMonedaDescripcion.ToString();
                        ddlMonedas.Attributes.Add("onchange", "CalcularItem();");
                        #endregion

                        //descuentoImporte.Attributes.Add("onchange", "CalcularItem();");
                        string mensaje = this.ObtenerMensajeSistema("ConfirmarEliminar");
                        string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                        btnEliminar.Attributes.Add("OnClick", funcion);

                        break;

                    #region Case Autorizar
                    case Gestion.Autorizar:

                        ddlProducto.Enabled = false;
                        //codigo.Enabled = false;
                        //btnBuscarProducto.Visible = false;
                        //producto.Enabled = false;
                        cantidad.Enabled = false;
                        PrecioUnitario.Enabled = false;
                        plazo.Enabled = false;
                        //descuentoImporte.Enabled = false;
                        #region Monedas
                        ddlMonedas.DataSource = this.MisMonedas;
                        ddlMonedas.DataValueField = "IdMoneda";
                        ddlMonedas.DataTextField = "Moneda";
                        ddlMonedas.DataBind();

                        //ddlMonedas.SelectedValue = item.Moneda.Moneda.ToString();
                        ddlMonedas.Enabled = false;
                        #endregion
                        //ddlAlicuotaIVA.Items.Add(new ListItem(item.AlicuotaIVA.ToString(), item.IdIVA.ToString()));
                        //ddlAlicuotaIVA.SelectedValue = item.AlicuotaIVA.ToString();
                        ddlAlicuotaIVA.Enabled = false;
                        btnEliminar.Visible = false;
                        break;
                    #endregion

                    #region Case Anular
                    case Gestion.Anular:

                        //codigo.Enabled = false;
                        //btnBuscarProducto.Visible = false;
                        ddlProducto.Enabled = false;
                        //producto.Enabled = false;
                        cantidad.Enabled = false;
                        PrecioUnitario.Enabled = false;
                        plazo.Enabled = false;
                        //descuentoImporte.Enabled = false;
                        #region Monedas
                        ddlMonedas.DataSource = this.MisMonedas;
                        ddlMonedas.DataValueField = "IdMoneda";
                        ddlMonedas.DataTextField = "Moneda";
                        ddlMonedas.DataBind();

                        //ddlMonedas.SelectedValue = item.Moneda.Moneda.ToString();
                        ddlMonedas.Enabled = false;
                        #endregion
                        //ddlAlicuotaIVA.Items.Add(new ListItem(item.AlicuotaIVA.ToString(), item.IdIVA.ToString()));
                        //ddlAlicuotaIVA.SelectedValue = item.AlicuotaIVA.ToString();
                        ddlAlicuotaIVA.Enabled = false;
                        btnEliminar.Visible = false;
                        break;
                    #endregion

                    #region Case Consultar
                    case Gestion.Consultar:
                        ddlProducto.Enabled = false;
                        //codigo.Enabled = false;
                        //btnBuscarProducto.Visible = false;
                        //producto.Enabled = false;
                        cantidad.Enabled = false;
                        PrecioUnitario.Enabled = false;
                        plazo.Enabled = false;
                        //descuentoImporte.Enabled = false;
                        #region Monedas
                        ddlMonedas.DataSource = this.MisMonedas;
                        ddlMonedas.DataValueField = "IdMoneda";
                        ddlMonedas.DataTextField = "Moneda";
                        ddlMonedas.DataBind();
                        //ddlMonedas.SelectedValue = item.Moneda.Moneda.ToString();
                        ddlMonedas.Enabled = false;
                        #endregion
                        //ddlAlicuotaIVA.Items.Add(new ListItem(item.AlicuotaIVA.ToString(), item.IdIVA.ToString()));
                        //ddlAlicuotaIVA.SelectedValue = item.AlicuotaIVA.ToString();
                        ddlAlicuotaIVA.Enabled = false;
                        btnEliminar.Visible = false;
                        break;
                    #endregion

                    default:
                        break;
                }

            }
        }

        protected void CalcularTotal()
        {
            //decimal? totalSinIva = 0;
            //decimal? totalIva = 0;
            //decimal? totalConIva = 0;

            //totalSinIva = this.MiOrdenCompra.OrdenesComprasDetalles.Sum(x => x.Importe);
            //totalIva = this.MiOrdenCompra.OrdenesComprasDetalles.Sum(x => x.ImporteIVA);
            //totalConIva = totalSinIva + totalIva;


            //this.txtTotalConIva.Text = totalConIva.Value.ToString("N2");
            //this.txtTotalSinIva.Text = totalSinIva.Value.ToString("N2");
            //this.txtTotalIva.Text = totalIva.Value.ToString("N2");
            //this.pnTotales.Update();
        }

        //protected void btnAgregarItem_Click(object sender, EventArgs e)
        //{
        //    CmpOrdenesComprasDetalles item;
        //    item = new CmpOrdenesComprasDetalles();
        //    this.MiOrdenCompra.OrdenesComprasDetalles.Add(item);
        //    item.IndiceColeccion = this.MiOrdenCompra.OrdenesComprasDetalles.IndexOf(item);
        //    //this.CalcularTotal();
        //    AyudaProgramacion.CargarGrillaListas<CmpOrdenesComprasDetalles>(this.MiOrdenCompra.OrdenesComprasDetalles, false, this.gvItems, true);
        //}

        #endregion

        #region Mapeo Datos
        private void MapearControlesObjeto(CmpOrdenesCompras pOrden)
        {
            this.MiOrdenCompra.TipoOrdenCompra.IdTipoOrdenCompra = Convert.ToInt32(this.ddlTipoOrden.SelectedValue);
            //this.MiOrdenCompra.CondicionPago.IdCondicionPago = (int)EnumTiposValores.Efectivo;// Convert.ToInt32(this.ddlCondicionPago.SelectedValue);
            //this.MiOrdenCompra.DireccionDestino = this.txtDireccion.Text;
            //this.MiOrdenCompra.FechaEntrega = this.txtFechaEntrega.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaEntrega.Text);
            this.MiOrdenCompra.Observacion = this.txtObservacion.Text;
            this.MiOrdenCompra.CuotasDescuentoAfiliado = this.txtCuotasDescuentoAfiliado.Text == string.Empty ? default(int) : Convert.ToInt32(this.txtCuotasDescuentoAfiliado.Text);
            this.MiOrdenCompra.FormaCobroAfiliado.IdFormaCobroAfiliado = this.ddlFormasCobros.SelectedValue == string.Empty ? default(int) : Convert.ToInt32(this.ddlFormasCobros.SelectedValue);
            this.MiOrdenCompra.FormaCobroAfiliado.FormaCobroDescripcion = this.ddlFormasCobros.SelectedValue == string.Empty ? string.Empty : this.ddlFormasCobros.SelectedItem.Text;
            this.MiOrdenCompra.CuotasPagoProveedor = this.txtCuotasDescuentoProveedor.Text == string.Empty ? default(int) : Convert.ToInt32(this.txtCuotasDescuentoProveedor.Text);
            MiOrdenCompra.Proveedor = this.ctrBuscarProveedor.MiProveedor;
        }

        private void MapearObjetoControles(CmpOrdenesCompras pOrden)
        {

            //this.txtFechaEntrega.Text = pOrden.FechaEntrega.HasValue ? pOrden.FechaEntrega.Value.ToShortDateString() : string.Empty;
            //this.ddlCondicionPago.SelectedValue = (pOrden.CondicionPago.IdCondicionPago).ToString();
            this.ddlTipoOrden.SelectedValue = (pOrden.TipoOrdenCompra.IdTipoOrdenCompra).ToString();
            //this.txtDireccion.Text = pOrden.DireccionDestino;
            this.txtObservacion.Text = pOrden.Observacion;
            this.txtCuotasDescuentoAfiliado.Text = pOrden.CuotasDescuentoAfiliado.HasValue ? pOrden.CuotasDescuentoAfiliado.Value.ToString() : string.Empty;
            if (pOrden.FormaCobroAfiliado.IdFormaCobroAfiliado>0)
            {
                ListItem item = this.ddlFormasCobros.Items.FindByValue(pOrden.FormaCobroAfiliado.IdFormaCobroAfiliado.ToString());
                if (item == null)
                    this.ddlFormasCobros.Items.Add(new ListItem(pOrden.FormaCobroAfiliado.FormaCobroDescripcion, pOrden.FormaCobroAfiliado.IdFormaCobroAfiliado.ToString()));
                this.ddlFormasCobros.SelectedValue = pOrden.FormaCobroAfiliado.IdFormaCobroAfiliado.ToString();
            }
            this.txtCuotasDescuentoProveedor.Text = pOrden.CuotasPagoProveedor.HasValue ? pOrden.CuotasPagoProveedor.Value.ToString() : string.Empty;

            decimal total = pOrden.OrdenesComprasDetalles.Sum(x => x.Importe);
            decimal totalIVA = pOrden.OrdenesComprasDetalles.Sum(x => x.ImporteIVA);
            this.txtTotalSinIva.Text = total.ToString("C2");
            this.txtTotalIva.Text = totalIVA.ToString("C2");
            this.txtTotalConIva.Text = (total + totalIVA).ToString("C2");
            this.ctrBuscarProveedor.IniciarControl(pOrden.Proveedor, this.GestionControl);
            AyudaProgramacion.CargarGrillaListas<CmpOrdenesComprasDetalles>(this.MiOrdenCompra.OrdenesComprasDetalles, false, this.gvItems, true);
        }

        #endregion
       
        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.OrdenesComprasModificarDatosAceptar != null)
                this.OrdenesComprasModificarDatosAceptar(null, this.MiOrdenCompra);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //this.Page.Validate("OrdenesComprasDatos");
            if (!this.Page.IsValid)
                return;
            bool guardo = true;

            this.MiOrdenCompra.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.PersistirDatosGrilla();
                    this.MapearControlesObjeto(this.MiOrdenCompra);
                    this.MiOrdenCompra.EstadoColeccion = EstadoColecciones.Agregado;
                    guardo = ComprasF.OrdenCompraAgregar(this.MiOrdenCompra);
                    break;
                case Gestion.Autorizar:
                    this.MapearControlesObjeto(this.MiOrdenCompra);
                    this.MiOrdenCompra.EstadoColeccion = EstadoColecciones.Modificado;
                    //this.MiOrdenCompra.usu = this.UsuarioActivo.IdUsuarioEvento;
                    //this.MiOrdenCompra.FechaAutorizacion = DateTime.Now;
                    this.MiOrdenCompra.Estado.IdEstado = (int)EstadosOrdenesCompras.Autorizado;
                    guardo = ComprasF.OrdenCompraAutorizar(this.MiOrdenCompra);
                    break;
                case Gestion.Anular:
                    this.MapearControlesObjeto(this.MiOrdenCompra);
                    this.MiOrdenCompra.EstadoColeccion = EstadoColecciones.Borrado;
                    this.MiOrdenCompra.Estado.IdEstado = (int)EstadosOrdenesCompras.Baja;
                    foreach (CmpOrdenesComprasDetalles det in this.MiOrdenCompra.OrdenesComprasDetalles)
                    {
                        det.EstadoColeccion = EstadoColecciones.Borrado;
                    }
                    guardo = ComprasF.OrdenCompraAnular(this.MiOrdenCompra);
                    break;
                default:
                    break;
            }

            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiOrdenCompra.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiOrdenCompra.CodigoMensaje, true, this.MiOrdenCompra.CodigoMensajeArgs);
            }

        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.OrdenesComprasModificarDatosCancelar != null)
                this.OrdenesComprasModificarDatosCancelar();
        }
    }
}