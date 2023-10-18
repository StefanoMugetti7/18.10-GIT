using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using SKP.ASP.Controls;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Compras.Entidades;
using Comunes.Entidades;
using Compras;
//using Proveedores.Entidades;
//using Proveedores;


namespace IU.Modulos.Compras.Controles
{
    public partial class SolicitudesComprasDatos : ControlesSeguros
    {


        private CmpSolicitudesCompras MiSolicitud
        {
            get { return (CmpSolicitudesCompras)Session[this.MiSessionPagina + "SolicitudDatosMiSolicitud"]; }
            set { Session[this.MiSessionPagina + "SolicitudDatosMiSolicitud"] = value; }
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

        private List<TGEMonedasCotizaciones> MisCotizaciones
        {
            get { return (List<TGEMonedasCotizaciones>)Session[this.MiSessionPagina + "SolicitudesPagosDatosMisCotizaciones"]; }
            set { Session[this.MiSessionPagina + "SolicitudesPagosDatosMisCotizaciones"] = value; }
        }

        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "SolicitudModificarDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "SolicitudModificarDatosMiIndiceDetalleModificar"] = value; }
        }

        public delegate void SolicitudesComprasDatosAceptarEventHandler(object sender, CmpSolicitudesCompras e);
        public event SolicitudesComprasDatosAceptarEventHandler SolicitudesComprasModificarDatosAceptar;

        public delegate void SolicitudesComprasDatosCancelarEventHandler();
        public event SolicitudesComprasDatosCancelarEventHandler SolicitudesComprasModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.ctrBuscarProductoPopUp.ProductosBuscarSeleccionar += new CuentasPagar.Controles.BuscarProductoPopUp.ProductosBuscarEventHandler(ctrBuscarProductoPopUp_ProductosBuscarSeleccionar);
            //this.ctrBuscarProveedorPopUp.ProveedoresBuscar += new CuentasPagar.Controles.ProveedoresBuscarPopUp.ProveedoresBuscarEventHandler(ctrBuscarProveedorPopUp_ProveedoresBuscarSeleccionar);
            this.ctrBuscarCotizacionesPopUp.CotizacionesBuscarSeleccionar += new Compras.Controles.CotizacionesBuscarPopUp.CotizacionesBuscarEventHandler(ctrBuscarCotizacionesPopUp_CotizacionesBuscarSeleccionar);
            if (!this.IsPostBack)
            {
                if (this.MiSolicitud == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);

                }

            }
            else
            {
                this.PersistirDatosGrilla();
            }
        }
        
        public void IniciarControl(CmpSolicitudesCompras pSolicitud, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MisIvas = TGEGeneralesF.TGEIVAAlicuotaObtenerLista();
            this.MisMonedas = TGEGeneralesF.MonedasObtenerLista();
            this.MisCotizaciones = TGEGeneralesF.MonedasCotizacionesObtenerLista();
            this.CargarCombos();
            this.MiSolicitud = pSolicitud;
            this.MiSolicitud = ComprasF.ComprasObtenerDatosCompletos(pSolicitud);
            
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.IniciarGrilla();
                   // this.txtCodigo.Enabled = true;
                   // this.btnBuscarProveedor.Visible = true;

                    this.ddlTipoSolicitudCompra.Enabled = true;
                    this.txtPlazoEntrega.Enabled = true;
                    this.txtObservacion.Enabled = true;
                    //this.btnAgregarItem.Visible = true;
                    
                    break;
                case Gestion.Modificar:
                    MapearObjetoAControles(this.MiSolicitud);
                    this.ddlTipoSolicitudCompra.Enabled = false;
                    this.txtObservacion.Enabled = false;
                    //this.ddlCondicionFiscal.Enabled = false;
                    this.btnAgregarItem.Visible = false;
                    this.txtPlazoEntrega.Enabled = false;

                    break;

                case Gestion.Consultar:
                     //MapearObjetoAControlesProveedor(this.MiSolicitud.Proveedor);
                    MapearObjetoAControles(this.MiSolicitud);
                    
                    this.ddlTipoSolicitudCompra.Enabled = false;
                    this.txtObservacion.Enabled = false;
                    //this.ddlCondicionFiscal.Enabled = false;
                    this.btnAgregarItem.Visible = false;
                    this.txtPlazoEntrega.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                case Gestion.Autorizar:
                    //MapearObjetoAControlesProveedor(this.MiSolicitud.Proveedor);
                    MapearObjetoAControles(this.MiSolicitud);
                    
                    this.ddlTipoSolicitudCompra.Enabled = false;
                    this.txtObservacion.Enabled = false;
                    //this.ddlCondicionFiscal.Enabled = false;
                    this.btnAgregarItem.Visible = false;
                    this.txtPlazoEntrega.Enabled = false;
                    break;
                case Gestion.Anular:
                    //MapearObjetoAControlesProveedor(this.MiSolicitud.Proveedor);
                    MapearObjetoAControles(this.MiSolicitud);
                    
                    this.ddlTipoSolicitudCompra.Enabled = false;
                    this.txtObservacion.Enabled = false;
                    //this.ddlCondicionFiscal.Enabled = false;
                    this.btnAgregarItem.Visible = false;
                    this.txtPlazoEntrega.Enabled = false;
                    break;
            }
        }

        private void CargarCombos()
        {
           
            //this.ddlCondicionFiscal.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValores.CondicionesFiscales);
            //this.ddlCondicionFiscal.DataValueField = "IdListaValorDetalle";
            //this.ddlCondicionFiscal.DataTextField = "Descripcion";
            //this.ddlCondicionFiscal.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlCondicionFiscal, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTipoSolicitudCompra.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposSolicitudesCompras);//TiposSolicitudesCompras
            this.ddlTipoSolicitudCompra.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTipoSolicitudCompra.DataTextField = "Descripcion";
            this.ddlTipoSolicitudCompra.DataBind();
            if(this.ddlTipoSolicitudCompra.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoSolicitudCompra, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private TGEMonedasCotizaciones ObtenerCotizacion(TGEMonedas pMoneda)
        {
            List<TGEMonedasCotizaciones> pCotizacion = this.MisCotizaciones;
            TGEMonedasCotizaciones result = new TGEMonedasCotizaciones();
            foreach (TGEMonedasCotizaciones cotiz in pCotizacion)
            {
                if (cotiz.IdMoneda == pMoneda.IdMoneda)
                {
                    result = cotiz;
                }
                
            }
            return result;
        }

        #region Mapeo De Datos

        protected void MapearControlesAObjeto(CmpSolicitudesCompras pSolicitud)
        {
            // Solicitud Compra
            pSolicitud.TipoSolicitudCompra.IdTipoSolicitudCompra = Convert.ToInt32(this.ddlTipoSolicitudCompra.SelectedValue);
            pSolicitud.PlazoEntrega = this.txtPlazoEntrega.Text == string.Empty ? 0 : Convert.ToInt32(this.txtPlazoEntrega.Text);
            pSolicitud.Observacion = this.txtObservacion.Text;
            
            // Detalles
            pSolicitud.Subtotal = Convert.ToDecimal(this.txtTotalSinIva.Text);
            pSolicitud.ImporteIVA = Convert.ToDecimal(this.txtTotalIva.Text);
            pSolicitud.Total = Convert.ToDecimal(this.txtTotalConIva.Text);


            
        }

        private void MapearObjetoAControles(CmpSolicitudesCompras pSolicitud)
        {
            // Solicitud Compra
            this.ddlTipoSolicitudCompra.SelectedValue = (pSolicitud.TipoSolicitudCompra.IdTipoSolicitudCompra).ToString();
            this.txtPlazoEntrega.Text = pSolicitud.PlazoEntrega.ToString();
            this.txtObservacion.Text = pSolicitud.Observacion.ToString();
            //Detalles
            
            
            this.txtTotalSinIva.Text = pSolicitud.Subtotal.ToString();
            this.txtTotalIva.Text = pSolicitud.ImporteIVA.ToString();
            this.txtTotalConIva.Text = pSolicitud.Total.ToString();

            AyudaProgramacion.CargarGrillaListas<CmpSolicitudesComprasDetalles>(this.MiSolicitud.SolicitudCompraDetalles, false, this.gvItems, true);
            //this.items.Update();
        }

        #endregion

        //#region "Proveedores PopUp"
        //protected void btnBuscarProveedor_Click(object sender, EventArgs e)
        //{
        //    this.ctrBuscarProveedorPopUp.IniciarControl();
        //}

        //void ctrBuscarProveedorPopUp_ProveedoresBuscarSeleccionar(CapProveedores pProveedor)
        //{
        //    this.MapearObjetoAControlesProveedor(pProveedor);
        //    this.UpdatePanelProovedor.Update();
        //}


        //private void MapearObjetoAControlesProveedor(CapProveedores pProveedor)
        //{
        //    this.txtCodigo.Text = pProveedor.IdProveedor.ToString();
        //    this.txtRazonSocial.Text = pProveedor.RazonSocial;

        //    this.txtCUIT.Text = pProveedor.CUIT;

        //    this.txtBeneficiario.Text = pProveedor.BeneficiarioDelCheque;

        //    ListItem item = this.ddlCondicionFiscal.Items.FindByValue(pProveedor.CondicionesFiscales.IdCondicionFiscal.ToString());
        //    if (item == null)
        //        this.ddlCondicionFiscal.Items.Add(new ListItem(pProveedor.CondicionesFiscales.Descripcion, pProveedor.CondicionesFiscales.IdCondicionFiscal.ToString()));
        //    this.ddlCondicionFiscal.SelectedValue = pProveedor.CondicionesFiscales.IdCondicionFiscal.ToString();

        //}

        //protected void txtCodigo_TextChanged(object sender, EventArgs e)
        //{
        //    string txtCodigo = ((TextBox)sender).Text;
        //    CapProveedores parametro = new CapProveedores();
        //    parametro.IdProveedor = Convert.ToInt32(txtCodigo);
        //    parametro = ProveedoresF.ProveedoresObtenerDatosCompletos(parametro);
        //    this.MiSolicitud.Proveedor.IdProveedor= parametro.IdProveedor == null ? 0 : Convert.ToInt32(parametro.IdProveedor);
        //    //mapear todo prov a Entidad
        //    if (this.MiSolicitud.Proveedor.IdProveedor != 0)
        //    {
        //        this.MapearObjetoAControlesProveedor(parametro);
        //    }
        //    else
        //    {
        //        parametro.CodigoMensaje = "ProveedorCodigoNoExiste";
        //        this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(parametro.CodigoMensaje));
        //    }
        //}

        //#endregion

        #region Grilla

        private void PersistirDatosGrilla()
        {
            foreach (GridViewRow fila in this.gvItems.Rows)
            {
                string codigo = ((TextBox)fila.FindControl("txtCodigoProducto")).Text;
                string cantidad = ((TextBox)fila.FindControl("txtCantidad")).Text;
                string descripcion = ((TextBox)fila.FindControl("txtProducto")).Text;
                string precioUnitarioSinIva = ((TextBox)fila.FindControl("txtPrecioUnitario")).Text;
                string subTotal = ((Label)fila.FindControl("lblSubtotal")).Text == string.Empty ? "0" : Convert.ToString(((Label)fila.FindControl("lblSubtotal")).Text);
                DropDownList ddlAlicuotaIVA = ((DropDownList)fila.FindControl("ddlAlicuotaIVA"));
                DropDownList ddlMonedas = ((DropDownList)fila.FindControl("ddlMonedas"));
                //string importeIvaTotal = ((Label)fila.FindControl("lblImporteIva")).Text == string.Empty ? "0" : Convert.ToString(((Label)fila.FindControl("lblImporteIva")).Text);
                //string subTotalIva = ((Label)fila.FindControl("lblSubtotalConIva")).Text == string.Empty ? "0" : Convert.ToString(((Label)fila.FindControl("lblSubtotalConIva")).Text);
                int plazoEntrega = ((TextBox)fila.FindControl("txtPlazoEntrega")).Text == string.Empty ? 0 : int.Parse(((TextBox)fila.FindControl("txtPlazoEntrega")).Text);
                //int plazoEntrega = ((TextBox)fila.FindControl("txtPlazoEntrega")).Text == string.Empty ? 0 : int.Parse(((TextBox)fila.FindControl("txtPlazoEntrega")).Text, NumberStyles.Currency);
                //NumberStyles.Currency para sacar el signo $     

                if (codigo != string.Empty)
                {
                    this.MiSolicitud.SolicitudCompraDetalles[fila.RowIndex].Producto.IdProducto = Convert.ToInt32(codigo);
                }

                if (cantidad != string.Empty)
                {
                    this.MiSolicitud.SolicitudCompraDetalles[fila.RowIndex].Cantidad = Convert.ToInt32(cantidad);
                }

                if (descripcion != string.Empty)
                {
                    this.MiSolicitud.SolicitudCompraDetalles[fila.RowIndex].Descripcion = descripcion;
                }

                if (ddlMonedas.SelectedValue != "")
                {
                    
                    this.MiSolicitud.SolicitudCompraDetalles[fila.RowIndex].Moneda.IdMoneda = Convert.ToInt32(ddlMonedas.SelectedValue);
                    //Ver como hacer para no ir tantas veces a la DB
                    this.MiSolicitud.SolicitudCompraDetalles[fila.RowIndex].CotizacionMoneda = (this.ObtenerCotizacion(this.MiSolicitud.SolicitudCompraDetalles[fila.RowIndex].Moneda)).MonedaCotizacion;
                }

                if (precioUnitarioSinIva != string.Empty)
                {
                    this.MiSolicitud.SolicitudCompraDetalles[fila.RowIndex].PrecioUnitario = Convert.ToDecimal(precioUnitarioSinIva);
                }
                //if (importeIvaTotal != string.Empty)
                //{
                //    this.MiFactura.FacturasDetalles[fila.RowIndex].ImporteIvaTotal = Convert.ToInt32(importeIvaTotal);
                //}
                if (plazoEntrega != 0)
                {
                    this.MiSolicitud.SolicitudCompraDetalles[fila.RowIndex].PlazoEntrega = plazoEntrega;
                }

                if (ddlAlicuotaIVA.SelectedValue != "")
                {
                    this.MiSolicitud.SolicitudCompraDetalles[fila.RowIndex].IdIVA = Convert.ToInt32(ddlAlicuotaIVA.SelectedValue);
                    this.MiSolicitud.SolicitudCompraDetalles[fila.RowIndex].AlicuotaIVA = Convert.ToDecimal(ddlAlicuotaIVA.SelectedItem.Text);
                 
                }
                
                ((Label)fila.FindControl("lblImporteIva")).Text = (this.MiSolicitud.SolicitudCompraDetalles[fila.RowIndex].ImporteIvaItem).ToString();
                ((Label)fila.FindControl("lblSubtotal")).Text = (this.MiSolicitud.SolicitudCompraDetalles[fila.RowIndex].Subtotal).ToString();
                ((Label)fila.FindControl("lblSubtotalConIva")).Text = (this.MiSolicitud.SolicitudCompraDetalles[fila.RowIndex].PrecioTotalItem).ToString();
                
            }
            this.CalcularTotal();
            //AyudaProgramacion.CargarGrillaListas<VTAItemFacturas>(this.MiFactura.FacturasDetalles, false, this.gvItems, true);
        }

        private void IniciarGrilla()
        {
            CmpSolicitudesComprasDetalles item;
            for (int i = 0; i < 5; i++)
            {
                item = new CmpSolicitudesComprasDetalles();
                this.MiSolicitud.SolicitudCompraDetalles.Add(item);
                item.IndiceColeccion = this.MiSolicitud.SolicitudCompraDetalles.IndexOf(item);
            }
            AyudaProgramacion.CargarGrillaListas<CmpSolicitudesComprasDetalles>(this.MiSolicitud.SolicitudCompraDetalles, false, this.gvItems, true);
        }

        protected void txtCodigoProducto_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).Parent.Parent);
            int IndiceColeccion = row.RowIndex;

            string contenido = ((TextBox)sender).Text;
            this.MiSolicitud.SolicitudCompraDetalles[IndiceColeccion].Producto.IdProducto = Convert.ToInt32(contenido);
            this.MiSolicitud.SolicitudCompraDetalles[IndiceColeccion].Producto.Compra = true;
            this.MiSolicitud.SolicitudCompraDetalles[IndiceColeccion].Producto = ComprasF.ProductosObtenerPorIdProducto(this.MiSolicitud.SolicitudCompraDetalles[IndiceColeccion].Producto);
            if (this.MiSolicitud.SolicitudCompraDetalles[IndiceColeccion].Producto.IdProducto == 0)
            {
                this.MiIndiceDetalleModificar = IndiceColeccion;
                this.ctrBuscarProductoPopUp.IniciarControl(EnumTiposProductos.Compras, new CMPProductos());
            }
            else
            {
                AyudaProgramacion.CargarGrillaListas<CmpSolicitudesComprasDetalles>(this.MiSolicitud.SolicitudCompraDetalles, false, this.gvItems, true);
            }

        }

        void ctrBuscarProductoPopUp_ProductosBuscarSeleccionar(CMPProductos e)
        {

            //AyudaProgramacion.MatchObjectProperties(e, this.MiFactura.FacturasDetalles[this.MiIndiceDetalleModificar].Producto);
            this.MiSolicitud.SolicitudCompraDetalles[this.MiIndiceDetalleModificar].Producto = e;
            AyudaProgramacion.CargarGrillaListas<CmpSolicitudesComprasDetalles>(this.MiSolicitud.SolicitudCompraDetalles, false, this.gvItems, true);
            this.items.Update();
        }

        void ctrBuscarCotizacionesPopUp_CotizacionesBuscarSeleccionar(CmpSolicitudesComprasDetalles e)
        {
            this.MiSolicitud.SolicitudCompraDetalles[this.MiIndiceDetalleModificar].IdCotizacionDetalle = e.IdCotizacionDetalle;
            this.MiSolicitud.SolicitudCompraDetalles[this.MiIndiceDetalleModificar].PrecioUnitario = e.PrecioUnitario;
            AyudaProgramacion.CargarGrillaListas<CmpSolicitudesComprasDetalles>(this.MiSolicitud.SolicitudCompraDetalles, false, this.gvItems, true);
            this.items.Update();
        }

        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            this.MiIndiceDetalleModificar = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Borrar")
            {
                //ELIMINA FILA
                //int indiceColeccion = Convert.ToInt32(e.CommandArgument);
                this.MiSolicitud.SolicitudCompraDetalles.RemoveAt(this.MiIndiceDetalleModificar);
                this.MiSolicitud.SolicitudCompraDetalles = AyudaProgramacion.AcomodarIndices<CmpSolicitudesComprasDetalles>(this.MiSolicitud.SolicitudCompraDetalles);
                AyudaProgramacion.CargarGrillaListas<CmpSolicitudesComprasDetalles>(this.MiSolicitud.SolicitudCompraDetalles, false, this.gvItems, true);
                this.CalcularTotal();
            }
            if (e.CommandName == "BuscarProducto")
            {
                this.ctrBuscarProductoPopUp.IniciarControl(EnumTiposProductos.Compras, new CMPProductos());
            }
            if (e.CommandName == "BuscarCotizacion")
            {
                CmpSolicitudesComprasDetalles sol = new CmpSolicitudesComprasDetalles();
                sol.Producto.IdProducto = this.MiSolicitud.SolicitudCompraDetalles[this.MiIndiceDetalleModificar].Producto.IdProducto;
                this.ctrBuscarCotizacionesPopUp.IniciarControl(sol);
            }


        }

        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                CmpSolicitudesComprasDetalles item = (CmpSolicitudesComprasDetalles)e.Row.DataItem;
                NumericTextBox codigo = (NumericTextBox)e.Row.FindControl("txtCodigoProducto");
                //NumericTextBox descuentoImporte = (NumericTextBox)e.Row.FindControl("txtDescuentoImporte");
                DropDownList ddlMonedas = ((DropDownList)e.Row.FindControl("ddlMonedas"));
                CurrencyTextBox PrecioUnitario = (CurrencyTextBox)e.Row.FindControl("txtPrecioUnitario");
                DropDownList ddlAlicuotaIVA = ((DropDownList)e.Row.FindControl("ddlAlicuotaIVA"));
                NumericTextBox cantidad = (NumericTextBox)e.Row.FindControl("txtCantidad");
                NumericTextBox plazo = (NumericTextBox)e.Row.FindControl("txtPlazoEntrega");
                TextBox producto = (TextBox)e.Row.FindControl("txtProducto");
                ImageButton btnBuscarProducto = (ImageButton)e.Row.FindControl("btnBuscarProducto");
                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                ImageButton btnCotizar = (ImageButton)e.Row.FindControl("btnCotizar");

                switch (GestionControl)
                {
                              
                    
                    case Gestion.Agregar:
                        
                        PrecioUnitario.Attributes.Add("onchange", "CalcularItem();");
                        cantidad.Attributes.Add("onchange", "CalcularItem();");    
                    
                        #region IVA
                        ddlAlicuotaIVA.DataSource = this.MisIvas;
                        ddlAlicuotaIVA.DataValueField = "IdIVA";
                        ddlAlicuotaIVA.DataTextField = "Alicuota";
                        ddlAlicuotaIVA.DataBind();
                        AyudaProgramacion.AgregarItemSeleccione(ddlAlicuotaIVA, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                        ddlAlicuotaIVA.SelectedValue = item.IdIVA.ToString();
                        ddlAlicuotaIVA.Attributes.Add("onchange", "CalcularItem();");
                        #endregion             

                        #region Moneda
                        ddlMonedas.DataSource = this.MisMonedas;
                        ddlMonedas.DataValueField = "IdMoneda";
                        ddlMonedas.DataTextField = "Moneda";
                        ddlMonedas.DataBind();
                        if (ddlMonedas.Items.Count > 1)
                        AyudaProgramacion.AgregarItemSeleccione(ddlMonedas, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                        ddlMonedas.SelectedValue = item.Moneda.miMonedaDescripcion.ToString();
                        ddlMonedas.Attributes.Add("onchange", "CalcularItem();");
                        #endregion   
                        
                        //descuentoImporte.Attributes.Add("onchange", "CalcularItem();");
                        string mensaje = this.ObtenerMensajeSistema("ConfirmarEliminar");
                        string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                        btnEliminar.Attributes.Add("OnClick", funcion);

                        break;

                    #region Case Cotizar
                    case Gestion.Modificar:
                        codigo.Enabled = false;
                        btnBuscarProducto.Visible = false;
                        producto.Enabled = false;
                        cantidad.Enabled = false;
                        PrecioUnitario.Enabled = true;
                        plazo.Enabled = false;
                        //descuentoImporte.Enabled = false;
                        #region Monedas
                        ddlMonedas.DataSource = this.MisMonedas;
                        ddlMonedas.DataValueField = "IdMoneda";
                        ddlMonedas.DataTextField = "Moneda";
                        ddlMonedas.DataBind();
                        //ddlMonedas.Items.Add(new ListItem(item.Moneda.Moneda.ToString(), item.Moneda.IdMoneda.ToString()));
                        ddlMonedas.SelectedValue = item.Moneda.Moneda.ToString();
                        ddlMonedas.Enabled = false;
                        #endregion
                        ddlAlicuotaIVA.Items.Add(new ListItem(item.AlicuotaIVA.ToString(), item.IdIVA.ToString()));
                        ddlAlicuotaIVA.SelectedValue = item.AlicuotaIVA.ToString();
                        ddlAlicuotaIVA.Enabled = false;
                        btnEliminar.Visible = false;
                        btnCotizar.Visible = true;
                    break;
                    #endregion

                    #region Case Autorizar
                    case Gestion.Autorizar:
                        
                        codigo.Enabled = false;
                        btnBuscarProducto.Visible = false;
                        producto.Enabled = false;
                        cantidad.Enabled = false;
                        PrecioUnitario.Enabled = false;
                        plazo.Enabled = false;
                        //descuentoImporte.Enabled = false;
                        #region Monedas
                        ddlMonedas.DataSource = this.MisMonedas;
                        ddlMonedas.DataValueField = "IdMoneda";
                        ddlMonedas.DataTextField = "Moneda";
                        ddlMonedas.DataBind();

                        ddlMonedas.SelectedValue = item.Moneda.Moneda.ToString();
                        ddlMonedas.Enabled = false;
                        #endregion
                        ddlAlicuotaIVA.Items.Add(new ListItem(item.AlicuotaIVA.ToString(), item.IdIVA.ToString()));
                        ddlAlicuotaIVA.SelectedValue = item.AlicuotaIVA.ToString();
                        ddlAlicuotaIVA.Enabled = false;
                        btnEliminar.Visible = false;
                        break;
                    #endregion

                    #region Case Anular
                    case Gestion.Anular:
                        
                        codigo.Enabled = false;
                        btnBuscarProducto.Visible = false;
                        producto.Enabled = false;
                        cantidad.Enabled = false;
                        PrecioUnitario.Enabled = false;
                        plazo.Enabled = false;
                        //descuentoImporte.Enabled = false;
                        #region Monedas
                        ddlMonedas.DataSource = this.MisMonedas;
                        ddlMonedas.DataValueField = "IdMoneda";
                        ddlMonedas.DataTextField = "Moneda";
                        ddlMonedas.DataBind();

                        ddlMonedas.SelectedValue = item.Moneda.Moneda.ToString();
                        ddlMonedas.Enabled = false;
                        #endregion
                        ddlAlicuotaIVA.Items.Add(new ListItem(item.AlicuotaIVA.ToString(), item.IdIVA.ToString()));
                        ddlAlicuotaIVA.SelectedValue = item.AlicuotaIVA.ToString();
                        ddlAlicuotaIVA.Enabled = false;
                        btnEliminar.Visible = false;
                        break;
                    #endregion

                    #region Case Consultar
                    case Gestion.Consultar:
                        
                        codigo.Enabled = false;
                        btnBuscarProducto.Visible = false;
                        producto.Enabled = false;
                        cantidad.Enabled = false;
                        PrecioUnitario.Enabled = false;
                        plazo.Enabled = false;
                        //descuentoImporte.Enabled = false;
                        #region Monedas
                        ddlMonedas.DataSource = this.MisMonedas;
                        ddlMonedas.DataValueField = "IdMoneda";
                        ddlMonedas.DataTextField = "Moneda";
                        ddlMonedas.DataBind();
                        //ddlMonedas.Items.Add(new ListItem(item.Moneda.Moneda.ToString(), item.Moneda.IdMoneda.ToString()));
                        ddlMonedas.SelectedValue = item.Moneda.Moneda.ToString();
                        ddlMonedas.Enabled = false;
                        #endregion
                        ddlAlicuotaIVA.Items.Add(new ListItem(item.AlicuotaIVA.ToString(), item.IdIVA.ToString()));
                        ddlAlicuotaIVA.SelectedValue = item.AlicuotaIVA.ToString();
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
            decimal? totalSinIva = 0;
            decimal? totalIva = 0;
            decimal? totalConIva = 0;

            totalSinIva = this.MiSolicitud.SolicitudCompraDetalles.Sum(x => x.Subtotal);
            totalIva = this.MiSolicitud.SolicitudCompraDetalles.Sum(x => x.ImporteIvaItem);
            totalConIva = this.MiSolicitud.SolicitudCompraDetalles.Sum(x => x.PrecioTotalItem);

            
            this.txtTotalConIva.Text = totalConIva.Value.ToString("N2");
            this.txtTotalSinIva.Text = totalSinIva.Value.ToString("N2");
            this.txtTotalIva.Text = totalIva.Value.ToString("N2");
            this.pnTotales.Update();
        }

        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {
            CmpSolicitudesComprasDetalles item;
            item = new CmpSolicitudesComprasDetalles();
            this.MiSolicitud.SolicitudCompraDetalles.Add(item);
            item.IndiceColeccion = this.MiSolicitud.SolicitudCompraDetalles.IndexOf(item);
            this.CalcularTotal();
            AyudaProgramacion.CargarGrillaListas<CmpSolicitudesComprasDetalles>(this.MiSolicitud.SolicitudCompraDetalles, false, this.gvItems, true);
        }


        #endregion

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.SolicitudesComprasModificarDatosAceptar != null)
                this.SolicitudesComprasModificarDatosAceptar(null, this.MiSolicitud);
        }
        
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiSolicitud);
            this.MiSolicitud.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.PersistirDatosGrilla();
                    this.MiSolicitud.Estado.IdEstado = (int)EstadosSolicitudesCompras.Activo;
                    foreach (CmpSolicitudesComprasDetalles item in this.MiSolicitud.SolicitudCompraDetalles)
                    {
                        if (item.Producto.IdProducto != 0)
                        {
                            item.EstadoColeccion = EstadoColecciones.Agregado;
                        }
                    }
                    guardo = ComprasF.ComprasAgregar(this.MiSolicitud);
                    break;
                case Gestion.Modificar: // COTIZAR
                     this.MiSolicitud.Estado.IdEstado = (int)EstadosSolicitudesCompras.Cotizado;
                    foreach (CmpSolicitudesComprasDetalles item in this.MiSolicitud.SolicitudCompraDetalles)
                    {
                        if (item.Producto.IdProducto != 0)
                        {
                            item.EstadoColeccion = EstadoColecciones.Modificado;
                        }
                    }
                    guardo = ComprasF.ComprasCotizar(this.MiSolicitud);

                    break;
                case Gestion.Autorizar:
                    //this.MiSolicitud.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    this.MiSolicitud.Estado.IdEstado = (int)EstadosSolicitudesCompras.Autorizado; //modificado?
                    foreach (CmpSolicitudesComprasDetalles item in this.MiSolicitud.SolicitudCompraDetalles)
                    {
                        if (item.Producto.IdProducto != 0)
                        {
                            item.EstadoColeccion = EstadoColecciones.SinCambio;
                        }
                    }
                    guardo = ComprasF.ComprasAutorizar(this.MiSolicitud);
                    break;

                case Gestion.Anular:
                    //this.MiSolicitud.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    this.MiSolicitud.Estado.IdEstado = (int)EstadosSolicitudesCompras.Baja;
                    foreach (CmpSolicitudesComprasDetalles item in this.MiSolicitud.SolicitudCompraDetalles)
                    {
                        if (item.Producto.IdProducto != 0)
                        {
                            item.EstadoColeccion = EstadoColecciones.Borrado;
                        }
                    }
                    guardo = ComprasF.ComprasAnular(this.MiSolicitud);
                    break;
                default:
                    break;
            }
            if (guardo)
            {

                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiSolicitud.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiSolicitud.CodigoMensaje, true, this.MiSolicitud.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {

            if (this.SolicitudesComprasModificarDatosCancelar != null)
                this.SolicitudesComprasModificarDatosCancelar();
        }

    }
}