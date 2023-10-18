using Compras;
using Compras.Entidades;
using Comunes.Entidades;
using Evol.Controls;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Proveedores;
using Proveedores.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Compras.Controles
{
    public partial class InformesRecepcionesAbiertoDatos : ControlesSeguros
    {
        const int CantidadItems = 5;
        private CmpInformesRecepciones MiInformeRecepcion
        {
            get { return (CmpInformesRecepciones)Session[this.MiSessionPagina + "InformesRecepcionesAbiertoDatosMiInformeRecepcion"]; }
            set { Session[this.MiSessionPagina + "InformesRecepcionesAbiertoDatosMiInformeRecepcion"] = value; }
        }
        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "InformesRecepcionesAbiertoDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "InformesRecepcionesAbiertoDatosMiIndiceDetalleModificar"] = value; }
        }
        public int MiCantidadDecimales
        {
            get { return (int)Session[this.MiSessionPagina + "SolicitudDatosCantidadDecimales"]; }
            set { Session[this.MiSessionPagina + "SolicitudDatosCantidadDecimales"] = value; }
        }

        public delegate void InformeRecepcionDatosAceptarEventHandler(object sender, CmpInformesRecepciones e);
        public event InformeRecepcionDatosAceptarEventHandler InformesRecepcionesModificarDatosAceptar;
        public delegate void InformeRecepcionDatosCancelarEventHandler();
        public event InformeRecepcionDatosCancelarEventHandler InformesRecepcionesModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(this.popUpMensajes_popUpMensajesPostBackAceptar);
            this.ctrBuscarProveedor.BuscarProveedor += new Proveedores.Controles.ProveedoresCabecerasDatos.ProveedoresDatosCabeceraAjaxEventHandler(this.CtrBuscarProveedor_BuscarProveedor);
            //this.ctrBuscarProductoPopUp.ProductosBuscarSeleccionar += new CuentasPagar.Controles.BuscarProductoPopUp.ProductosBuscarEventHandler(ctrBuscarProductoPopUp_ProductosBuscarSeleccionar);
            this.ctrImportarSolPagosDetalles.ControlAceptar += this.CtrImportarSolPagosDetalles_ControlAceptar;
            this.ctrImportarSolPagosDetalles.ControlAceptarAcopio += this.CtrImportarSolPagosDetalles_ControlAceptarAcopio;
            this.ctrImportarOrdenesComprasDetalles.ControlAceptar += this.CtrImportarOrdenesComprasDetalles_ControlAceptar;
            if (!this.IsPostBack)
            {
                if (this.MiInformeRecepcion == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
                //if (this.paginaSegura.UsuarioEmpresa.CondicionFiscal.IdCondicionFiscal==(int)EnumTGECondicionesFiscales.IVASujetoExcento
            }
            else
            {
                this.PersistirDatosGrilla();
            }
        }
        public void IniciarControl(CmpInformesRecepciones pInformeRecepcion, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiInformeRecepcion = pInformeRecepcion;
            this.MiIndiceDetalleModificar = 0;
            this.CargarCombos();

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.IniciarGrilla(CantidadItems);
                    AyudaProgramacion.CargarGrillaListas<CmpInformesRecepcionesDetalles>(this.MiInformeRecepcion.InformesRecepcionesDetalles, false, this.gvItems, true);
                    //this.txtCodigo.Enabled = true;
                    //this.btnBuscarProveedor.Enabled = true;
                    this.ddlCantidadDecimales.Enabled = true;
                    this.txtFechaRemito.Enabled = true;
                    this.txtObservacion.Enabled = true;
                    this.txtNumeroRemito.Enabled = true;
                    this.txtPreNumeroRemito.Enabled = true;
                    this.btnImportarFactura.Visible = true;

                    this.ctrComentarios.IniciarControl(this.MiInformeRecepcion, this.GestionControl);
                    this.ctrArchivos.IniciarControl(this.MiInformeRecepcion, this.GestionControl);
                    if (this.MiInformeRecepcion.Proveedor.IdProveedor.HasValue)
                    {
                        //this.txtCodigo.Text = this.MiInformeRecepcion.Proveedor.IdProveedor.Value.ToString();
                        //this.txtCodigo_TextChanged(this.txtCodigo, EventArgs.Empty);
                    }

                    if (this.MisParametrosUrl.Contains("IdProveedor"))
                    {
                        CapProveedores prov = new CapProveedores();
                        //this.hdfIdAfiliado.Value = this.MisParametrosUrl["IdAfiliado"].ToString();
                        //MiProveedor.IdProveedor = 0;
                        this.ctrBuscarProveedor.BuscarProveedor += new Proveedores.Controles.ProveedoresCabecerasDatos.ProveedoresDatosCabeceraAjaxEventHandler(this.CtrBuscarProveedor_BuscarProveedor);
                        prov.IdProveedor = Convert.ToInt32(this.MisParametrosUrl["IdProveedor"].ToString());
                        this.ctrBuscarProveedor.IniciarControl(prov, this.GestionControl);
                        //button_Click(null, EventArgs.Empty);
                        //this.txtNumeroSocio.Text = this.MisParametrosUrl["IdAfiliado"].ToString();
                        //this.txtNumeroSocio_TextChanged(this.txtNumeroSocio, EventArgs.Empty);
                    }
                    break;
                case Gestion.Anular:
                    this.MiInformeRecepcion = ComprasF.InformesRecepcionesObtenerDatosCompletos(this.MiInformeRecepcion);
                    //prov.IdProveedor = this.MiInformeRecepcion.Entidad.IdRefEntidad;
                    //MapearObjetoAControlesProveedor(ProveedoresF.ProveedoresObtenerDatosCompletos(prov));
                    this.MapearObjetoAControles(this.MiInformeRecepcion);
                    this.ddlCantidadDecimales.Visible = false;
                    this.lblCantidadDecimales.Visible = false;
                    this.txtObservacion.Enabled = false;
                    this.btnAgregarItem.Visible = false;
                    this.btnImportarOC.Visible = false;
                    break;
                case Gestion.Consultar:
                    this.MiInformeRecepcion = ComprasF.InformesRecepcionesObtenerDatosCompletos(this.MiInformeRecepcion);
                    this.ddlCantidadDecimales.Visible = false;
                    this.lblCantidadDecimales.Visible = false;
                    this.btnImportarOC.Visible = false;
                    //prov.IdProveedor = this.MiInformeRecepcion.Entidad.IdRefEntidad;
                    //if (this.MiInformeRecepcion.Proveedor.IdProveedor > 0)
                    //{
                    //    CapProveedores prov = new CapProveedores();
                    //    //this.hdfIdAfiliado.Value = this.MisParametrosUrl["IdAfiliado"].ToString();
                    //    //MiProveedor.IdProveedor = 0;

                    //    ctrBuscarProveedor.BuscarProveedor += new Proveedores.Controles.ProveedoresCabecerasDatos.ProveedoresDatosCabeceraAjaxEventHandler(CtrBuscarProveedor_BuscarProveedor);
                    //    prov.IdProveedor = Convert.ToInt32(this.MiInformeRecepcion.Proveedor.IdProveedor.ToString());
                    //    ctrBuscarProveedor.IniciarControl(prov, this.GestionControl);
                    //    //button_Click(null, EventArgs.Empty);
                    //    //this.txtNumeroSocio.Text = this.MisParametrosUrl["IdAfiliado"].ToString();
                    //    //this.txtNumeroSocio_TextChanged(this.txtNumeroSocio, EventArgs.Empty);
                    //}
                    //MapearObjetoAControlesProveedor(ProveedoresF.ProveedoresObtenerDatosCompletos(prov));
                    this.MapearObjetoAControles(this.MiInformeRecepcion);
                    this.btnImportarFactura.Visible = false;
                    this.txtObservacion.Enabled = false;
                    //this.ddlCondicionFiscal.Enabled = false;
                    this.btnAgregarItem.Visible = false;
                    this.btnAceptar.Visible = false;
                    this.ddlFiliales.Enabled = false;
                    this.gvItems.Columns[this.gvItems.Columns.Count - 1].Visible = false;
                    break;
            }
        }
        private void CargarCombos()
        {
            this.ddlFiliales.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFiliales.DataValueField = "IdFilial";
            this.ddlFiliales.DataTextField = "Filial";
            this.ddlFiliales.DataBind();
            this.ddlFiliales.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();

            this.ddlCantidadDecimales.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.CantidadDecimales);
            this.ddlCantidadDecimales.DataValueField = "CodigoValor";
            this.ddlCantidadDecimales.DataTextField = "Descripcion";
            this.ddlCantidadDecimales.DataBind();
        }
        protected void ddlCantidadDecimales_OnClick(object sender, EventArgs e)
        {
            this.MiCantidadDecimales = Convert.ToInt32(this.ddlCantidadDecimales.SelectedValue);
            string numberSymbol = string.Empty;
            foreach (GridViewRow fila in this.gvItems.Rows)
            {
                CurrencyTextBox PrecioUnitario = (CurrencyTextBox)fila.FindControl("txtPrecioUnitario");
                PrecioUnitario.NumberOfDecimals = this.MiCantidadDecimales;
                numberSymbol = PrecioUnitario.Prefix == string.Empty ? "N" : "C";
                PrecioUnitario.Text = PrecioUnitario.Decimal.ToString(string.Concat(numberSymbol, this.MiCantidadDecimales));
            }
            this.items.Update();
        }
        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.InformesRecepcionesModificarDatosAceptar != null)
                this.InformesRecepcionesModificarDatosAceptar(null, this.MiInformeRecepcion);
        }
        #region "Datos Solicitud"
        protected void MapearControlesAObjeto(CmpInformesRecepciones pInformeRecepcion)
        {
            //pInformeRecepcion.IdUsuarioAlta = this.UsuarioActivo.IdUsuarioEvento;
            pInformeRecepcion.NumeroRemitoPrefijo = txtPreNumeroRemito.Text.ToString().PadLeft(4, '0');
            pInformeRecepcion.NumeroRemitoSufijo = txtNumeroRemito.Text.ToString().PadLeft(8, '0');
            pInformeRecepcion.FechaEmision = this.txtFechaRemito.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFechaRemito.Text);
            pInformeRecepcion.Observacion = this.txtObservacion.Text;
            pInformeRecepcion.Filial.IdFilial = this.ddlFiliales.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFiliales.SelectedValue);
            pInformeRecepcion.PrecioTotal = hdfPrecioTotal.Value == string.Empty ? 0 : Convert.ToDecimal(hdfPrecioTotal.Value.Replace('.', ','));
            //pInformeRecepcion.ImporteSolicitudPago = txtImporteRecibido.Text == string.Empty ? 0 : Convert.ToDecimal(txtImporte.Text);
            //pInformeRecepcion.ImportePrevioRecibido = txtImporteRecibido.Text == string.Empty ? 0 : Convert.ToDecimal(txtImporteRecibido.Text);

            // DATOS PROVEEDOR
            pInformeRecepcion.Proveedor = this.ctrBuscarProveedor.MiProveedor;
            // pInformeRecepcion.Proveedor = ;
        }
        private void MapearObjetoAControles(CmpInformesRecepciones pInformeRecepcion)
        {
            this.txtPreNumeroRemito.Text = pInformeRecepcion.NumeroRemitoPrefijo.PadLeft(4, '0');
            this.txtNumeroRemito.Text = pInformeRecepcion.NumeroRemitoSufijo.PadLeft(8, '0');
            this.txtFechaRemito.Text = pInformeRecepcion.FechaEmision.ToShortDateString();
            this.txtObservacion.Text = pInformeRecepcion.Observacion;
            this.ddlFiliales.SelectedValue = pInformeRecepcion.Filial.IdFilial.ToString();
            this.MapearObjetoAControlesProveedor(pInformeRecepcion.Proveedor);

            AyudaProgramacion.CargarGrillaListas<CmpInformesRecepcionesDetalles>(this.MiInformeRecepcion.InformesRecepcionesDetalles, false, this.gvItems, true);

            this.ctrComentarios.IniciarControl(pInformeRecepcion, this.GestionControl);
            this.ctrArchivos.IniciarControl(pInformeRecepcion, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pInformeRecepcion);
            if (pInformeRecepcion.IdSolicitudPago > 0)
            {
                this.phDetalleAcopio.Visible = true;
                this.hdfIdSolicitudPago.Value = pInformeRecepcion.IdSolicitudPago.ToString();
                this.txtDetalleAcopio.Text = pInformeRecepcion.DetalleAcopio;
                this.txtImporte.Text = pInformeRecepcion.ImporteSolicitudPago.HasValue ? pInformeRecepcion.ImporteSolicitudPago.Value.ToString("C2") : string.Empty;
                this.txtImporteRecibido.Text = pInformeRecepcion.ImportePrevioRecibido.HasValue ? pInformeRecepcion.ImportePrevioRecibido.Value.ToString("C2") : string.Empty;
                this.gvItems.Columns[3].Visible = true;
                this.gvItems.Columns[4].Visible = true;
                ScriptManager.RegisterStartupScript(this.items, this.items.GetType(), "CalcularItemScript", "CalcularItem();", true);
                this.btnAceptar.Attributes.Add("onclick", "CalcularItem();");
            }
            this.ctrBuscarProveedor.IniciarControl(pInformeRecepcion.Proveedor, this.GestionControl);
        }
        void CtrBuscarProveedor_BuscarProveedor(CapProveedores e)
        {
            this.MapearObjetoAControlesProveedor(e);
            this.MiInformeRecepcion.Proveedor.IdProveedor = this.ctrBuscarProveedor.MiProveedor.IdProveedor;
            //this.UpdatePanelProovedor.Update();
            if (this.MiInformeRecepcion.IdSolicitudPago > 0)
                this.btnEliminarAcopio_Click(this.btnEliminarAcopio, EventArgs.Empty);

            if (this.MiInformeRecepcion.InformesRecepcionesDetalles.Exists(x => x.IdOrdenCompraDetalle > 0))
            {
                this.MiInformeRecepcion.InformesRecepcionesDetalles = new List<CmpInformesRecepcionesDetalles>();
                this.IniciarGrilla(CantidadItems);
                AyudaProgramacion.CargarGrillaListas<CmpInformesRecepcionesDetalles>(this.MiInformeRecepcion.InformesRecepcionesDetalles, false, this.gvItems, true);
            }
        }
        #endregion
        #region "Proveedores PopUp"
        //protected void btnBuscarProveedor_Click(object sender, EventArgs e)
        //{
        //    this.ctrBuscarProveedorPopUp.IniciarControl();
        //}

        //void ctrBuscarProveedorPopUp_ProveedoresBuscarSeleccionar(CapProveedores pProveedor)
        //{
        //    this.MiInformeRecepcion.Proveedor = pProveedor;
        //    this.MapearObjetoAControlesProveedor(this.MiInformeRecepcion.Proveedor);
        //    this.UpdatePanelProovedor.Update();
        //}
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
            this.btnImportarFactura.Visible = pProveedor.IdProveedor.HasValue;
            this.items.Update();
        }
        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            string txtCodigo = ((TextBox)sender).Text;
            CapProveedores parametro = new CapProveedores();
            parametro.IdProveedor = Convert.ToInt32(txtCodigo);
            parametro = ProveedoresF.ProveedoresObtenerDatosCompletos(parametro);
            if (parametro.IdProveedor.HasValue)
            {
                this.MiInformeRecepcion.Proveedor = parametro;
                this.MapearObjetoAControlesProveedor(parametro);
            }
            else
            {
                parametro.CodigoMensaje = "ProveedorCodigoNoExiste";
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(parametro.CodigoMensaje));
            }
        }
        #endregion
        #region "Grilla Solicitud Detalles"
        private void PersistirDatosGrilla()
        {
            bool modifica;
            CmpInformesRecepcionesDetalles det;
            foreach (GridViewRow fila in this.gvItems.Rows)
            {
                modifica = false;
                det = this.MiInformeRecepcion.InformesRecepcionesDetalles[fila.DataItemIndex];
                // string cantidad = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtCantidad")).Text;
                //string cantidad = ((TextBox)fila.FindControl("txtCantidad")).Text;
                decimal cantidad = ((CurrencyTextBox)fila.FindControl("txtCantidad")).Decimal;

                DropDownList ddlProducto = ((DropDownList)fila.FindControl("ddlProducto"));
                HiddenField hdfIdProducto = (HiddenField)fila.FindControl("hdfIdProducto");
                HiddenField hdfProductoDetalle = (HiddenField)fila.FindControl("hdfProductoDetalle");

                bool hdfStockeable = ((HiddenField)fila.FindControl("hdfStockeable")).Value == string.Empty ? false : Convert.ToBoolean(((HiddenField)fila.FindControl("hdfStockeable")).Value);

                if (det.CantidadRecibida != Convert.ToDecimal(cantidad))
                {
                    det.CantidadRecibida = Convert.ToDecimal(cantidad);
                    modifica = true;
                }
                if (hdfIdProducto.Value != string.Empty && Convert.ToInt32(hdfIdProducto.Value) > 0)
                {
                    det.Producto.IdProducto = Convert.ToInt32(hdfIdProducto.Value);
                    det.Producto.Descripcion = hdfProductoDetalle.Value;
                    det.Producto.Venta = true;
                    det.Producto.Familia.Stockeable = hdfStockeable;
                }
                if (cantidad > 0)
                {
                    this.MiInformeRecepcion.InformesRecepcionesDetalles[fila.RowIndex].CantidadRecibida = cantidad;//decimal.Parse(cantidad.Replace(".", ","), NumberStyles.AllowDecimalPoint); //Convert.ToDecimal(cantidad);
                }

                if (this.MiInformeRecepcion.IdSolicitudPago > 0)
                {
                    CurrencyTextBox txtPrecioUnitario = (CurrencyTextBox)fila.FindControl("txtPrecioUnitario");
                    if (!det.PrecioUnitario.HasValue || (det.PrecioUnitario.HasValue && det.PrecioUnitario != txtPrecioUnitario.Decimal))
                    {
                        det.PrecioUnitario = txtPrecioUnitario.Decimal;
                        modifica = true;
                    }
                }
                if (modifica)
                    this.MiInformeRecepcion.InformesRecepcionesDetalles[fila.DataItemIndex].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiInformeRecepcion.InformesRecepcionesDetalles[fila.DataItemIndex], this.GestionControl);
            }
        }
        private void IniciarGrilla(int pCantidad)
        {
            CmpInformesRecepcionesDetalles item;
            for (int i = 0; i < pCantidad; i++)
            {
                item = new CmpInformesRecepcionesDetalles();
                item.EstadoColeccion = EstadoColecciones.AgregadoPrevio;
                this.MiInformeRecepcion.InformesRecepcionesDetalles.Add(item);
                item.IndiceColeccion = this.MiInformeRecepcion.InformesRecepcionesDetalles.IndexOf(item);
            }
        }
        protected void txtCodigoProducto_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).Parent.Parent);
            int IndiceColeccion = row.RowIndex;

            decimal contenido = ((CurrencyTextBox)sender).Decimal;
            if (contenido == 0)
                return;
            this.MiInformeRecepcion.InformesRecepcionesDetalles[IndiceColeccion].Producto.IdProducto = Convert.ToInt32(contenido);
            this.MiInformeRecepcion.InformesRecepcionesDetalles[IndiceColeccion].Producto.Compra = true;
            this.MiInformeRecepcion.InformesRecepcionesDetalles[IndiceColeccion].Producto = ComprasF.ProductosObtenerPorIdProducto(this.MiInformeRecepcion.InformesRecepcionesDetalles[IndiceColeccion].Producto);
            AyudaProgramacion.CargarGrillaListas<CmpInformesRecepcionesDetalles>(this.MiInformeRecepcion.InformesRecepcionesDetalles, false, this.gvItems, true);
            CurrencyTextBox txtCantidad = (CurrencyTextBox)row.FindControl("txtCantidad");
            txtCantidad.Focus();
        }
        //void ctrBuscarProductoPopUp_ProductosBuscarSeleccionar(CMPProductos e)
        //{
        //    //AyudaProgramacion.MatchObjectProperties(e, this.MiFactura.FacturasDetalles[this.MiIndiceDetalleModificar].Producto);
        //    this.MiInformeRecepcion.InformesRecepcionesDetalles[this.MiIndiceDetalleModificar].Producto = e;
        //    AyudaProgramacion.CargarGrillaListas<CmpInformesRecepcionesDetalles>(this.MiInformeRecepcion.InformesRecepcionesDetalles, false, this.gvItems, true);
        //    this.items.Update();
        //}
        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            this.MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            if (e.CommandName == "Borrar")
            {
                //ELIMINA FILA
                //int indiceColeccion = Convert.ToInt32(e.CommandArgument);
                this.MiInformeRecepcion.InformesRecepcionesDetalles.RemoveAt(index);
                this.MiInformeRecepcion.InformesRecepcionesDetalles = AyudaProgramacion.AcomodarIndices<CmpInformesRecepcionesDetalles>(this.MiInformeRecepcion.InformesRecepcionesDetalles);
                AyudaProgramacion.CargarGrillaListas<CmpInformesRecepcionesDetalles>(this.MiInformeRecepcion.InformesRecepcionesDetalles, false, this.gvItems, true);
            }
            if (e.CommandName == "BuscarProducto")
            {
                //CMPProductos filtro = new CMPProductos
                //{
                //    IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial
                //};
                //this.ctrBuscarProductoPopUp.IniciarControl(EnumTiposProductos.Compras, filtro);
            }
        }
        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CmpInformesRecepcionesDetalles item = (CmpInformesRecepcionesDetalles)e.Row.DataItem;
                CurrencyTextBox Precio = (CurrencyTextBox)e.Row.FindControl("txtPrecioUnitario");
                CurrencyTextBox cantidad = (CurrencyTextBox)e.Row.FindControl("txtCantidad");
                //ImageButton btnBuscarProducto = (ImageButton)e.Row.FindControl("btnBuscarProducto");
                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");

                DropDownList ddlProducto = ((DropDownList)e.Row.FindControl("ddlProducto"));
                if (item.Producto.IdProducto > 0)
                    ddlProducto.Items.Add(new ListItem(item.Producto.Descripcion, item.Producto.IdProducto.ToString()));

                //btnBuscarProducto.Visible = false;
                Precio.Enabled = false;
                cantidad.Enabled = false;
                //btnEliminar.Visible = false;

                switch (this.GestionControl)
                {
                    case Gestion.Agregar:
                        ddlProducto.Enabled = true;
                        //btnBuscarProducto.Visible = !item.IdSolicitudPagoDetalle.HasValue;
                        btnEliminar.Visible = true;
                        if (!item.Importado)
                        {
                            ddlProducto.Enabled = true;
                            cantidad.Enabled = true;
                        }
                        else
                        {
                            ddlProducto.Enabled = false;
                            cantidad.Enabled = false;
                        }
                        cantidad.Attributes.Add("onchange", "CalcularItem();");
                        CurrencyTextBox txtPrecioUnitario = (CurrencyTextBox)e.Row.FindControl("txtPrecioUnitario");
                        txtPrecioUnitario.Attributes.Add("onchange", "CalcularItem();");
                        txtPrecioUnitario.Enabled = true;
                        break;
                    case Gestion.Anular:
                    case Gestion.Consultar:
                        break;
                    default:
                        break;
                }
            }
        }
        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {
            CmpInformesRecepcionesDetalles item;
            item = new CmpInformesRecepcionesDetalles();
            this.MiInformeRecepcion.InformesRecepcionesDetalles.Add(item);
            item.IndiceColeccion = this.MiInformeRecepcion.InformesRecepcionesDetalles.IndexOf(item);
            AyudaProgramacion.CargarGrillaListas<CmpInformesRecepcionesDetalles>(this.MiInformeRecepcion.InformesRecepcionesDetalles, false, this.gvItems, true);
            ScriptManager.RegisterStartupScript(this.items, this.items.GetType(), "CalcularItemScript", "CalcularItem();", true);
        }
        protected void btnImportarFactura_Click(object sender, EventArgs e)
        {
            this.ctrImportarSolPagosDetalles.IniciarControl(this.MiInformeRecepcion.Proveedor);
            this.btnImportarOC.Visible = false;
        }
        protected void btnImportarOC_Click(object sender, EventArgs e)
        {
            if (this.MiInformeRecepcion.Proveedor.IdProveedor.HasValue && this.MiInformeRecepcion.Proveedor.IdProveedor > 0)
            {
                this.ctrImportarOrdenesComprasDetalles.IniciarControl(this.MiInformeRecepcion.Proveedor);
                this.btnImportarFactura.Visible = false;
            }
            else
            {
                this.MostrarMensaje(this.ObtenerMensajeSistema("ValidarProveedorSeleccionado"), true);
                return;
            }
        }
        private void CtrImportarOrdenesComprasDetalles_ControlAceptar(List<CmpInformesRecepcionesDetalles> e)
        {
            foreach (CmpInformesRecepcionesDetalles item in e)
            {
                item.EstadoColeccion = EstadoColecciones.Agregado;
                item.CantidadPedida = item.CantidadRecibida;
                item.Importado = true;
            }
            //Agrego a la lista los que no estan en la lista
            this.MiInformeRecepcion.InformesRecepcionesDetalles.RemoveAll(x => x.Producto.IdProducto == 0);
            this.MiInformeRecepcion.InformesRecepcionesDetalles.AddRange(e.Where(x => !this.MiInformeRecepcion.InformesRecepcionesDetalles.Any(y => x.IdOrdenCompraDetalle == y.IdOrdenCompraDetalle)));
            AyudaProgramacion.CargarGrillaListas<CmpInformesRecepcionesDetalles>(this.MiInformeRecepcion.InformesRecepcionesDetalles, false, this.gvItems, true);
            this.MiInformeRecepcion.OrdenCompra.IdOrdenCompra = 0;
            //this.hdfIdOrdenCompra .Value = "0";
            this.phDetalleAcopio.Visible = false;
            //this.gvItems.Columns[5].Visible = false;
            this.gvItems.Columns[4].Visible = false;
            this.items.Update();
        }
        private void CtrImportarSolPagosDetalles_ControlAceptar(List<CmpInformesRecepcionesDetalles> e)
        {
            foreach (CmpInformesRecepcionesDetalles item in e)
            {
                item.EstadoColeccion = EstadoColecciones.Agregado;
                item.Importado = true;
            }
            //Agrego a la lista los que no estan en la lista
            this.MiInformeRecepcion.InformesRecepcionesDetalles.RemoveAll(x => x.Producto.IdProducto == 0);
            this.MiInformeRecepcion.InformesRecepcionesDetalles.AddRange(e.Where(x => !this.MiInformeRecepcion.InformesRecepcionesDetalles.Any(y => x.IdSolicitudPagoDetalle == y.IdSolicitudPagoDetalle)));
            AyudaProgramacion.CargarGrillaListas<CmpInformesRecepcionesDetalles>(this.MiInformeRecepcion.InformesRecepcionesDetalles, false, this.gvItems, true);
            this.MiInformeRecepcion.IdSolicitudPago = 0;
            this.hdfIdSolicitudPago.Value = "0";
            this.phDetalleAcopio.Visible = false;
            //this.gvItems.Columns[5].Visible = false;
            this.gvItems.Columns[4].Visible = false;
            this.items.Update();
        }
        private void CtrImportarSolPagosDetalles_ControlAceptarAcopio(global::CuentasPagar.Entidades.CapSolicitudPago e)
        {
            if (e.IdSolicitudPago > 0)
            {
                this.MiInformeRecepcion.IdSolicitudPago = e.IdSolicitudPago;
                this.MiInformeRecepcion.DetalleAcopio = e.Filtro;
                this.hdfIdSolicitudPago.Value = e.IdSolicitudPago.ToString();
                this.ddlCantidadDecimales.Visible = true;
                this.lblCantidadDecimales.Visible = true;
                this.phDetalleAcopio.Visible = true;
                this.btnEliminarAcopio.Visible = true;

                this.gvItems.Columns[3].Visible = true;
                this.gvItems.Columns[4].Visible = true;
                this.txtDetalleAcopio.Text = e.Filtro;
                this.MiInformeRecepcion.ImporteSolicitudPago = e.ImporteSinIVA;
                this.txtImporte.Text = e.ImporteSinIVA.ToString("C2");
                this.MiInformeRecepcion.ImportePrevioRecibido = e.ImporteParcial;
                this.txtImporteRecibido.Text = e.ImporteParcial.ToString("C2");

                AyudaProgramacion.CargarGrillaListas<CmpInformesRecepcionesDetalles>(this.MiInformeRecepcion.InformesRecepcionesDetalles, false, this.gvItems, true);
                this.items.Update();
            }
            else
            {
                this.MiInformeRecepcion.IdSolicitudPago = 0;
                this.hdfIdSolicitudPago.Value = "0";
                this.phDetalleAcopio.Visible = false;
                this.gvItems.Columns[5].Visible = false;
                this.gvItems.Columns[6].Visible = false;
                this.txtDetalleAcopio.Text = string.Empty;
                this.txtImporte.Text = string.Empty;
                this.MiInformeRecepcion.ImportePrevioRecibido = default(decimal?);
                this.txtImporteRecibido.Text = string.Empty;
            }
            ScriptManager.RegisterStartupScript(this.items, this.items.GetType(), "CalcularItemScript", "CalcularItem();", true);
            this.items.Update();
        }
        protected void btnEliminarAcopio_Click(object sender, EventArgs e)
        {
            this.btnAceptar.Attributes.Remove("onclick");
            this.MiInformeRecepcion.IdSolicitudPago = 0;
            this.hdfIdSolicitudPago.Value = "0";
            this.phDetalleAcopio.Visible = false;
            this.ddlCantidadDecimales.Visible = false;
            this.lblCantidadDecimales.Visible = false;
            this.gvItems.Columns[3].Visible = false;
            this.gvItems.Columns[4].Visible = false;
            this.txtDetalleAcopio.Text = string.Empty;
            this.items.Update();
        }
        #endregion
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this.items, this.items.GetType(), "CalcularTotalScript", "CalcularTotal();", true);
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiInformeRecepcion);

            this.MiInformeRecepcion.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiInformeRecepcion.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    this.MiInformeRecepcion.FechaAlta = DateTime.Now;
                    this.MiInformeRecepcion.Estado.IdEstado = (int)Estados.Activo;
                    this.MiInformeRecepcion.InformesRecepcionesDetalles = this.MiInformeRecepcion.InformesRecepcionesDetalles.Where(x => x.EstadoColeccion == EstadoColecciones.Agregado && x.Producto.IdProducto > 0).ToList();
                    guardo = ComprasF.InformesRecepcionesAgregar(this.MiInformeRecepcion);
                    if (!guardo && this.MiInformeRecepcion.InformesRecepcionesDetalles.Count == 0)
                        this.IniciarGrilla(5);
                    AyudaProgramacion.CargarGrillaListas(this.MiInformeRecepcion.InformesRecepcionesDetalles, false, this.gvItems, true);
                    this.items.Update();
                    break;
                case Gestion.Anular:
                    //this.MiSolicitud.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    this.MiInformeRecepcion.Estado.IdEstado = (int)Estados.Baja;
                    this.MiInformeRecepcion.FechaBaja = DateTime.Now;
                    this.MiInformeRecepcion.IdUsuarioBaja = this.UsuarioActivo.IdUsuario;
                    guardo = ComprasF.InformesRecepcionesAnular(this.MiInformeRecepcion);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiInformeRecepcion.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiInformeRecepcion.CodigoMensaje, true, this.MiInformeRecepcion.CodigoMensajeArgs);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {

            if (this.InformesRecepcionesModificarDatosCancelar != null)
                this.InformesRecepcionesModificarDatosCancelar();
        }

    }
}
