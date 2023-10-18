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
using Proveedores.Entidades;
using Proveedores;

namespace IU.Modulos.Compras.Controles
{
    public partial class CotizacionesDatos : ControlesSeguros
    {



        private CmpCotizaciones MiCotizacion
        {
            get { return (CmpCotizaciones)Session[this.MiSessionPagina + "CotizacionesDatosMiCotizacion"]; }
            set { Session[this.MiSessionPagina + "CotizacionesDatosMiCotizacion"] = value; }
        }

        private List<TGEIVA> MisIvas
        {
            get { return (List<TGEIVA>)Session[this.MiSessionPagina + "CotizacionesDatosMisIvas"]; }
            set { Session[this.MiSessionPagina + "CotizacionesDatosMisIvas"] = value; }
        }

        private List<TGEMonedas> MisMonedas
        {
            get { return (List<TGEMonedas>)Session[this.MiSessionPagina + "CotizacionesDatosMisMonedas"]; }
            set { Session[this.MiSessionPagina + "CotizacionesDatosMisMonedas"] = value; }
        }

        private List<TGEMonedasCotizaciones> MisCotizaciones
        {
            get { return (List<TGEMonedasCotizaciones>)Session[this.MiSessionPagina + "CotizacionesDatosMisCotizaciones"]; }
            set { Session[this.MiSessionPagina + "CotizacionesDatosMisCotizaciones"] = value; }
        }

        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "CotizacionModificarDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "CotizacionModificarDatosMiIndiceDetalleModificar"] = value; }
        }

        public delegate void CotizacionesDatosAceptarEventHandler(object sender, CmpCotizaciones e);
        public event CotizacionesDatosAceptarEventHandler CotizacionesModificarDatosAceptar;

        public delegate void CotizacionesDatosCancelarEventHandler();
        public event CotizacionesDatosCancelarEventHandler CotizacionesModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            ctrBuscarProveedor.BuscarProveedor += new Proveedores.Controles.ProveedoresCabecerasDatos.ProveedoresDatosCabeceraAjaxEventHandler(CtrBuscarProveedor_BuscarProveedor);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.ctrBuscarProductoPopUp.ProductosBuscarSeleccionar += new CuentasPagar.Controles.BuscarProductoPopUp.ProductosBuscarEventHandler(ctrBuscarProductoPopUp_ProductosBuscarSeleccionar);
             if (!this.IsPostBack)
            {
                if (this.MiCotizacion == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);

                }

            }
            else
            {
                this.PersistirDatosGrilla();
            }
        }
        void CtrBuscarProveedor_BuscarProveedor(CapProveedores e)
        {
            this.MapearObjetoAControlesProveedor(e);
            MiCotizacion.Proveedor.IdProveedor = ctrBuscarProveedor.MiProveedor.IdProveedor;
            //this.UpdatePanelProovedor.Update();
        }
        public void IniciarControl(CmpCotizaciones pCotizacion, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MisIvas = TGEGeneralesF.TGEIVAAlicuotaObtenerLista();
            this.MisMonedas = TGEGeneralesF.MonedasObtenerLista();
            this.MisCotizaciones = TGEGeneralesF.MonedasCotizacionesObtenerLista();
            this.CargarCombos();
            this.MiCotizacion = pCotizacion;
            

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.IniciarGrilla();
                 
                    this.txtObservacion.Enabled = true;
                    this.btnAgregarItem.Visible = true;
                    this.ddlCondicionPago.Enabled = true;
                    this.txtMail.Enabled = true;
            
                    this.txtFechaRecibido.Enabled = false;
                    this.txtCostoFlete.Enabled = true;
                    
                    this.txtDescuentoDetalle.Enabled = true;
                    break;
                case Gestion.Modificar:
                    

                    this.MiCotizacion = ComprasF.CotizacionesObtenerDatosCompletos(pCotizacion);
                    MapearObjetoAControlesProveedor(this.MiCotizacion.Proveedor);
                    MapearObjetoAControles(this.MiCotizacion);

                    this.txtObservacion.Enabled = true;
                    this.ddlCondicionPago.Enabled = true;
                    this.txtMail.Enabled = true;
               
                    this.txtCostoFlete.Enabled = true;
                    
                    this.txtDescuentoDetalle.Enabled = true;
                    this.btnAgregarItem.Visible = true;

                    break;

                case Gestion.Consultar:
                    this.MiCotizacion = ComprasF.CotizacionesObtenerDatosCompletos(pCotizacion);
                    MapearObjetoAControlesProveedor(this.MiCotizacion.Proveedor);
                    MapearObjetoAControles(this.MiCotizacion);

                    this.txtObservacion.Enabled = false;
                    this.txtFechaRecibido.Enabled = false;
                    this.btnAgregarItem.Visible = false;
                    this.btnAceptar.Visible = false;
                    break;
                case Gestion.Autorizar:
                    this.MiCotizacion = ComprasF.CotizacionesObtenerDatosCompletos(pCotizacion);
                    MapearObjetoAControlesProveedor(this.MiCotizacion.Proveedor);
                    MapearObjetoAControles(this.MiCotizacion);

                    this.txtObservacion.Enabled = false;
                    this.txtFechaRecibido.Enabled = false;
                    this.btnAgregarItem.Visible = false;
                    
                    break;
                case Gestion.Anular:
                    this.MiCotizacion = ComprasF.CotizacionesObtenerDatosCompletos(pCotizacion);
                    MapearObjetoAControlesProveedor(this.MiCotizacion.Proveedor);
                    MapearObjetoAControles(this.MiCotizacion);

                    this.txtObservacion.Enabled = false;
                    this.txtFechaRecibido.Enabled = false;
                    this.btnAgregarItem.Visible = false;
                    break;
            }
        }

        private void CargarCombos()
        {

            this.ddlCondicionPago.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.CondicionesPagos);
            this.ddlCondicionPago.DataValueField = "IdListaValorDetalle";
            this.ddlCondicionPago.DataTextField = "Descripcion";
            this.ddlCondicionPago.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCondicionPago, this.ObtenerMensajeSistema("SeleccioneOpcion"));

           
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

        protected void MapearControlesAObjeto(CmpCotizaciones pCotizacion)
        {
            // Cotizaciones
            pCotizacion.CondicionPago.IdCondicionPago = Convert.ToInt32(this.ddlCondicionPago.SelectedValue);
            pCotizacion.Mail = this.txtMail.Text;

            pCotizacion.FechaRecibido = this.txtFechaRecibido.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaRecibido.Text);
            pCotizacion.CostoFlete = this.txtCostoFlete.Text == string.Empty ? 0 : Convert.ToDecimal(this.txtCostoFlete.Text);
            pCotizacion.DescuentoTotal = this.txtDescuentoTotal.Text == string.Empty ? 0 : Convert.ToDecimal(this.txtDescuentoTotal.Text);
            pCotizacion.DescuentoDetalle = this.txtDescuentoDetalle.Text;
            pCotizacion.Observaciones = this.txtObservacion.Text;
            pCotizacion.Proveedor = this.ctrBuscarProveedor.MiProveedor;

        }

        private void MapearObjetoAControles(CmpCotizaciones pCotizacion)
        {
            
            this.ddlCondicionPago.SelectedValue = (pCotizacion.CondicionPago.IdCondicionPago).ToString();
            this.txtMail.Text = pCotizacion.Mail;
            this.txtObservacion.Text = pCotizacion.Observaciones;
            this.txtFechaRecibido.Text = pCotizacion.FechaRecibido.HasValue ? pCotizacion.FechaRecibido.Value.ToShortDateString() : string.Empty;
            this.txtCostoFlete.Text = (pCotizacion.CostoFlete).ToString();
            this.txtDescuentoTotal.Text = (pCotizacion.DescuentoTotal).ToString();
            //CalcularTotalDescuento();


            AyudaProgramacion.CargarGrillaListas<CmpCotizacionesDetalles>(this.MiCotizacion.CotizacionesDetalles, false, this.gvItems, true);
            this.items.Update();

        }

        #endregion

        #region "Proveedores PopUp"
        //protected void btnBuscarProveedor_Click(object sender, EventArgs e)
        //{
        //    this.ctrBuscarProveedorPopUp.IniciarControl();
        //}

        void ctrBuscarProveedorPopUp_ProveedoresBuscarSeleccionar(CapProveedores pProveedor)
        {
            this.MiCotizacion.Proveedor.IdProveedor = pProveedor.IdProveedor;
            this.MapearObjetoAControlesProveedor(pProveedor);
            this.UpdatePanelProovedor.Update();
        }

        private void MapearObjetoAControlesProveedor(CapProveedores pProveedor)
        {
            //this.txtCodigo.Text = pProveedor.IdProveedor.Value.ToString();
            //this.txtRazonSocial.Text = pProveedor.RazonSocial;
            //this.txtCUIT.Text = pProveedor.CUIT;
            //this.txtBeneficiario.Text = pProveedor.BeneficiarioDelCheque;

            //ListItem item = this.ddlCondicionFiscal.Items.FindByValue(pProveedor.CondicionFiscal.IdCondicionFiscal.ToString());
            //if (item == null)
            //    this.ddlCondicionFiscal.Items.Add(new ListItem(pProveedor.CondicionFiscal.Descripcion, pProveedor.CondicionFiscal.IdCondicionFiscal.ToString()));
            //this.ddlCondicionFiscal.SelectedValue = pProveedor.CondicionFiscal.IdCondicionFiscal.ToString();

         
            this.items.Update();
        }
        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            string txtCodigo = ((TextBox)sender).Text;
            CapProveedores parametro = new CapProveedores();
            parametro.IdProveedor = Convert.ToInt32(txtCodigo);
            parametro = ProveedoresF.ProveedoresObtenerDatosCompletos(parametro);
            this.MiCotizacion.Proveedor.IdProveedor = parametro.IdProveedor == null ? 0 : Convert.ToInt32(parametro.IdProveedor);
            //mapear todo prov a Entidad
            if (this.MiCotizacion.Proveedor.IdProveedor != 0)
            {
                this.MapearObjetoAControlesProveedor(parametro);
            }
            
        }

        #endregion

        #region Grilla

        private void PersistirDatosGrilla()
        {
            foreach (GridViewRow fila in this.gvItems.Rows)
            {
                DropDownList codigo = ((DropDownList)fila.FindControl("ddlProducto"));

                string cantidad = ((TextBox)fila.FindControl("txtCantidad")).Text;
                DropDownList ddlMonedas = ((DropDownList)fila.FindControl("ddlMonedas"));
                string precioUnitarioSinIva = ((TextBox)fila.FindControl("txtPrecioUnitario")).Text;
                string descuento = ((TextBox)fila.FindControl("txtDescuento")).Text;
                DropDownList ddlAlicuotaIVA = ((DropDownList)fila.FindControl("ddlAlicuotaIVA"));
                int plazoEntrega = ((TextBox)fila.FindControl("txtPlazoEntrega")).Text == string.Empty ? 0 : int.Parse(((TextBox)fila.FindControl("txtPlazoEntrega")).Text);
                string precioCantidad = ((TextBox)fila.FindControl("txtPrecioCantidad")).Text;
                //NumberStyles.Currency para sacar el signo $     

                if (codigo.SelectedValue != "")
                {
                    this.MiCotizacion.CotizacionesDetalles[fila.RowIndex].Producto.IdProducto = Convert.ToInt32(codigo.SelectedValue);
                }

             

                if (cantidad != string.Empty)
                {
                    this.MiCotizacion.CotizacionesDetalles[fila.RowIndex].Cantidad = Convert.ToInt32(cantidad);
                }

                if (ddlMonedas.SelectedValue != "")
                {

                    this.MiCotizacion.CotizacionesDetalles[fila.RowIndex].Moneda.IdMoneda = Convert.ToInt32(ddlMonedas.SelectedValue);
                    
                    this.MiCotizacion.CotizacionesDetalles[fila.RowIndex].CotizacionMoneda = (this.ObtenerCotizacion(this.MiCotizacion.CotizacionesDetalles[fila.RowIndex].Moneda)).MonedaCotizacion;
                }

                if (precioUnitarioSinIva != string.Empty)
                {
                    this.MiCotizacion.CotizacionesDetalles[fila.RowIndex].PrecioUnitario = Convert.ToDecimal(precioUnitarioSinIva);
                }

                if (precioCantidad != string.Empty)
                {
                    this.MiCotizacion.CotizacionesDetalles[fila.RowIndex].PrecioCantidad = Convert.ToDecimal(precioCantidad);
                }

                if (descuento != string.Empty)
                {
                    this.MiCotizacion.CotizacionesDetalles[fila.RowIndex].Descuento = Convert.ToDecimal(descuento);
                }
                if (plazoEntrega != 0)
                {
                    this.MiCotizacion.CotizacionesDetalles[fila.RowIndex].PlazoEntrega = plazoEntrega;
                }

                if (ddlAlicuotaIVA.SelectedValue != "")
                {
                    this.MiCotizacion.CotizacionesDetalles[fila.RowIndex].IdIVA = Convert.ToInt32(ddlAlicuotaIVA.SelectedValue);
                    this.MiCotizacion.CotizacionesDetalles[fila.RowIndex].AlicuotaIVA = Convert.ToDecimal(ddlAlicuotaIVA.SelectedItem.Text);

                }
            }
            this.CalcularTotalDescuento();
        }

        private void CalcularTotalDescuento()
        {
            decimal desc = 0; 
            desc = this.MiCotizacion.CotizacionesDetalles.Sum(x => x.Descuento);
            this.txtDescuentoTotal.Text = desc.ToString();
            this.pnlDesc.Update();
        }

        private void IniciarGrilla()
        {
            CmpCotizacionesDetalles item;
            for (int i = 0; i < 5; i++)
            {
                item = new CmpCotizacionesDetalles();
                this.MiCotizacion.CotizacionesDetalles.Add(item);
                item.IndiceColeccion = this.MiCotizacion.CotizacionesDetalles.IndexOf(item);
            }
            AyudaProgramacion.CargarGrillaListas<CmpCotizacionesDetalles>(this.MiCotizacion.CotizacionesDetalles, false, this.gvItems, true);
        }

        protected void txtCodigoProducto_TextChanged(object sender, EventArgs e)
        {
           
            GridViewRow row = ((GridViewRow)((TextBox)sender).Parent.Parent);
            int IndiceColeccion = row.RowIndex;

            string contenido = ((TextBox)sender).Text;
            this.MiCotizacion.CotizacionesDetalles[IndiceColeccion].Producto.IdProducto = Convert.ToInt32(contenido);
            this.MiCotizacion.CotizacionesDetalles[IndiceColeccion].Producto.Compra = true;
            this.MiCotizacion.CotizacionesDetalles[IndiceColeccion].Producto = ComprasF.ProductosObtenerPorIdProducto(this.MiCotizacion.CotizacionesDetalles[IndiceColeccion].Producto);
            if (this.MiCotizacion.CotizacionesDetalles[IndiceColeccion].Producto.IdProducto == 0)
            {
                return;
            }
            else
            {
                AyudaProgramacion.CargarGrillaListas<CmpCotizacionesDetalles>(this.MiCotizacion.CotizacionesDetalles, false, this.gvItems, true);
            
            }



        }

        void ctrBuscarProductoPopUp_ProductosBuscarSeleccionar(CMPProductos e)
        {

            //AyudaProgramacion.MatchObjectProperties(e, this.MiFactura.FacturasDetalles[this.MiIndiceDetalleModificar].Producto);
            this.MiCotizacion.CotizacionesDetalles[this.MiIndiceDetalleModificar].Producto = e;
            this.MiCotizacion.CotizacionesDetalles[this.MiIndiceDetalleModificar].PrecioUnitario = this.MiCotizacion.CotizacionesDetalles[this.MiIndiceDetalleModificar].Producto.Precio;
            AyudaProgramacion.CargarGrillaListas<CmpCotizacionesDetalles>(this.MiCotizacion.CotizacionesDetalles, false, this.gvItems, true);
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
                this.MiCotizacion.CotizacionesDetalles.RemoveAt(this.MiIndiceDetalleModificar);
                this.MiCotizacion.CotizacionesDetalles = AyudaProgramacion.AcomodarIndices<CmpCotizacionesDetalles>(this.MiCotizacion.CotizacionesDetalles);
                AyudaProgramacion.CargarGrillaListas<CmpCotizacionesDetalles>(this.MiCotizacion.CotizacionesDetalles, false, this.gvItems, true);
                //this.CalcularTotal();
            }
            if (e.CommandName == "BuscarProducto")
            {

                CMPProductos filtro = new CMPProductos();
                filtro.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
            }

        }

        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                CmpCotizacionesDetalles item = (CmpCotizacionesDetalles)e.Row.DataItem;
                DropDownList ddlProducto = ((DropDownList)e.Row.FindControl("ddlProducto"));
                if (item.Producto.IdProducto > 0)
                    ddlProducto.Items.Add(new ListItem(item.Producto.Descripcion, item.Producto.IdProducto.ToString()));

                CurrencyTextBox cantidad = (CurrencyTextBox)e.Row.FindControl("txtCantidad");
        
                DropDownList ddlMonedas = ((DropDownList)e.Row.FindControl("ddlMonedas"));
                CurrencyTextBox PrecioUnitario = (CurrencyTextBox)e.Row.FindControl("txtPrecioUnitario");
                CurrencyTextBox descuento = (CurrencyTextBox)e.Row.FindControl("txtDescuento");
                NumericTextBox plazo = (NumericTextBox)e.Row.FindControl("txtPlazoEntrega");
                CurrencyTextBox precioCantidad = (CurrencyTextBox)e.Row.FindControl("txtPrecioCantidad");
                DropDownList ddlAlicuotaIVA = ((DropDownList)e.Row.FindControl("ddlAlicuotaIVA"));

                ImageButton btnBuscarProducto = (ImageButton)e.Row.FindControl("btnBuscarProducto");
                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");


                switch (GestionControl)
                {


                    case Gestion.Agregar:
                        ddlProducto.Enabled = true;
                        descuento.Attributes.Add("onchange", "CalcularItem();");
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

                        ddlMonedas.SelectedValue = item.Moneda.IdMoneda.ToString();
                        
                        #endregion

                

                        break;

                    case Gestion.Modificar:
                        ddlProducto.Enabled = true;
                        btnBuscarProducto.Visible = true;
                        
                        cantidad.Enabled = true;
                        plazo.Enabled = true;
                        precioCantidad.Enabled = true;
                        PrecioUnitario.Enabled = true;
                        descuento.Enabled = true;
                        
                        
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
                        ddlAlicuotaIVA.Enabled = true;
                        btnEliminar.Visible = true;

                        break;
                    #region Case Autorizar
                    case Gestion.Autorizar:
                      
                        btnBuscarProducto.Visible = false;
                        ddlProducto.Enabled = false;
                        cantidad.Enabled = false;
                        PrecioUnitario.Enabled = false;
                        precioCantidad.Enabled = false;
                        plazo.Enabled = false;
                        descuento.Enabled = false;
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

                    #region Case Anular
                    case Gestion.Anular:

                  
                        btnBuscarProducto.Visible = false;
                        ddlProducto.Enabled = false;
                        cantidad.Enabled = false;
                        PrecioUnitario.Enabled = false;
                        precioCantidad.Enabled = false;
                        plazo.Enabled = false;
                        descuento.Enabled = false;
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

                    #region Case Consultar
                    case Gestion.Consultar:
                      
                        btnBuscarProducto.Visible = false;
                        ddlProducto.Enabled = false;
                        cantidad.Enabled = false;
                        PrecioUnitario.Enabled = false;
                        precioCantidad.Enabled = false;
                        plazo.Enabled = false;
                        descuento.Enabled = false;
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

        //protected void CalcularTotal()
        //{
        //    decimal? totalSinIva = 0;
        //    decimal? totalIva = 0;
        //    decimal? totalConIva = 0;

        //    totalSinIva = this.MiSolicitud.SolicitudCompraDetalles.Sum(x => x.Subtotal);
        //    totalIva = this.MiSolicitud.SolicitudCompraDetalles.Sum(x => x.ImporteIvaItem);
        //    totalConIva = this.MiSolicitud.SolicitudCompraDetalles.Sum(x => x.PrecioTotalItem);


        //    this.txtTotalConIva.Text = totalConIva.ToString();
        //    this.txtTotalSinIva.Text = totalSinIva.ToString();
        //    this.txtTotalIva.Text = totalIva.ToString();
        //    this.pnTotales.Update();
        //}

        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {
            CmpCotizacionesDetalles item;
            item = new CmpCotizacionesDetalles();
            this.MiCotizacion.CotizacionesDetalles.Add(item);
            item.IndiceColeccion = this.MiCotizacion.CotizacionesDetalles.IndexOf(item);
            //this.CalcularTotal();
            AyudaProgramacion.CargarGrillaListas<CmpCotizacionesDetalles>(this.MiCotizacion.CotizacionesDetalles, false, this.gvItems, true);
        }


        #endregion

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.CotizacionesModificarDatosAceptar != null)
                this.CotizacionesModificarDatosAceptar(null, this.MiCotizacion);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiCotizacion);
            this.MiCotizacion.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.PersistirDatosGrilla();
                    this.MiCotizacion.Estado.IdEstado = (int)EstadosCotizaciones.Activo;
                    foreach (CmpCotizacionesDetalles item in this.MiCotizacion.CotizacionesDetalles)
                    {
                        if (item.Producto.IdProducto != 0)
                        {
                            item.EstadoColeccion = EstadoColecciones.Agregado;
                        }
                    }
                    guardo = ComprasF.CotizacionesAgregar(this.MiCotizacion);
                    break;
                case Gestion.Modificar:
                    this.MiCotizacion.Estado.IdEstado = (int)EstadosCotizaciones.Activo; 
                    foreach (CmpCotizacionesDetalles item in this.MiCotizacion.CotizacionesDetalles)
                    {
                        if (item.Producto.IdProducto != 0)
                        {
                            item.EstadoColeccion = EstadoColecciones.Modificado;
                        }
                    }
                    guardo = ComprasF.CotizacionesModificar(this.MiCotizacion);

                    break;
                case Gestion.Autorizar:
                    this.MiCotizacion.Estado.IdEstado = (int)EstadosCotizaciones.Autorizado; 
                    guardo = ComprasF.CotizacionesAutorizar(this.MiCotizacion);

                    break;
                case Gestion.Anular:
                    this.MiCotizacion.Estado.IdEstado = (int)EstadosCotizaciones.Baja; 
                    guardo = ComprasF.CotizacionesAnular(this.MiCotizacion);

                    break;
                default:
                    break;
            }
            if (guardo)
            {

                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiCotizacion.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiCotizacion.CodigoMensaje, true, this.MiCotizacion.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {

            if (this.CotizacionesModificarDatosCancelar != null)
                this.CotizacionesModificarDatosCancelar();
        }
    }
}