using Compras;
using Compras.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Proveedores.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Compras.Controles
{
    public partial class OrdenesComprasDatos : ControlesSeguros
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
        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "SolicitudModificarDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "SolicitudModificarDatosMiIndiceDetalleModificar"] = value; }
        }
        private List<TGEIVA> MisIvas
        {
            get { return (List<TGEIVA>)Session[this.MiSessionPagina + "SolicitudesPagosDatosMisIvas"]; }
            set { Session[this.MiSessionPagina + "SolicitudesPagosDatosMisIvas"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(this.popUpMensajes_popUpMensajesPostBackAceptar);
            //this.ctrBuscarProveedorPopUp.ProveedoresBuscar += new CuentasPagar.Controles.ProveedoresBuscarPopUp.ProveedoresBuscarEventHandler(ctrBuscarProveedorPopUp_ProveedoresBuscarSeleccionar);
            this.ctrBuscarProveedor.BuscarProveedor += new Proveedores.Controles.ProveedoresCabecerasDatos.ProveedoresDatosCabeceraAjaxEventHandler(this.CtrBuscarProveedor_BuscarProveedor);
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
        public void IniciarControl(CmpOrdenesCompras pOrden, Gestion pGestion)
        {
            this.MiOrdenCompra = pOrden;
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.MiOrdenCompra.Entidad.IdEntidad = (int)EnumTGEEntidades.Proveedores;
            TGEIVA pParamaetro = new TGEIVA();
            pParamaetro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            this.MisIvas = TGEGeneralesF.TGEIVAAlicuotaObtenerListaActiva(pParamaetro);

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.txtObservacion.Enabled = true;
                    //this.btnBuscarProveedor.Enabled = true;
                    //this.txtCodigo.Enabled = true;
                    //this.ddlCondicionPago.Enabled = true;
                    this.ddlTipoOrden.Enabled = true;
                    this.txtDireccion.Enabled = true;
                    this.txtFechaEntrega.Enabled = true;
                    this.IniciarGrilla(2);
                    AyudaProgramacion.CargarGrillaListas<CmpOrdenesComprasDetalles>(this.MiOrdenCompra.OrdenesComprasDetalles, false, this.gvDatos, true);
                    this.ctrOrdenesComprasValores.IniciarControl(this.MiOrdenCompra, this.GestionControl);
                    break;
                case Gestion.Autorizar:
                    this.MiOrdenCompra = ComprasF.OrdenCompraObtenerDatosCompletos(this.MiOrdenCompra);
                    this.MapearObjetoControles(this.MiOrdenCompra);
                    //this.MapearObjetoAControlesProveedor(this.MiOrdenCompra.Proveedor);
                    //this.ddlCondicionPago.Enabled = false;
                    this.ddlTipoOrden.Enabled = false;
                    this.txtCantidadAgregar.Visible = false;
                    this.btnAgregarItem.Visible = false;
                    this.lblCantidadAgregar.Visible = false;
                    AyudaProgramacion.CargarGrillaListas<CmpOrdenesComprasDetalles>(this.MiOrdenCompra.OrdenesComprasDetalles, false, this.gvDatos, true);
                    this.upOrdenCompraDetalle.Update();
                    this.txtDiferencia.Visible = false;
                    this.lblDiferencia.Visible = false;
                    break;
                case Gestion.Anular:
                    this.MiOrdenCompra = ComprasF.OrdenCompraObtenerDatosCompletos(this.MiOrdenCompra);
                    this.MapearObjetoControles(this.MiOrdenCompra);
                    //this.MapearObjetoAControlesProveedor(this.MiOrdenCompra.Proveedor);
                    //this.ddlCondicionPago.Enabled = false;
                    this.ddlTipoOrden.Enabled = false;
                    this.txtCantidadAgregar.Visible = false;
                    this.lblCantidadAgregar.Visible = false;
                    this.btnAgregarItem.Visible = false;
                    AyudaProgramacion.CargarGrillaListas<CmpOrdenesComprasDetalles>(this.MiOrdenCompra.OrdenesComprasDetalles, false, this.gvDatos, true);
                    this.upOrdenCompraDetalle.Update();
                    this.txtDiferencia.Visible = false;
                    this.lblDiferencia.Visible = false;
                    break;
                case Gestion.Consultar:
                    this.btnAceptar.Visible = false;
                    this.MiOrdenCompra = ComprasF.OrdenCompraObtenerDatosCompletos(this.MiOrdenCompra);
                    this.MapearObjetoControles(this.MiOrdenCompra);
                    //this.MapearObjetoAControlesProveedor(this.MiOrdenCompra.Proveedor);
                    //this.ddlCondicionPago.Enabled = false;
                    this.ddlTipoOrden.Enabled = false;
                    this.txtCantidadAgregar.Visible = false;
                    this.lblCantidadAgregar.Visible = false;
                    this.btnAgregarItem.Visible = false;
                    AyudaProgramacion.CargarGrillaListas<CmpOrdenesComprasDetalles>(this.MiOrdenCompra.OrdenesComprasDetalles, false, this.gvDatos, true);
                    this.upOrdenCompraDetalle.Update();
                    this.txtDiferencia.Visible = false;
                    this.lblDiferencia.Visible = false;
                    this.ddlFiliales.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            //this.ddlCondicionPago.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.CondicionesPagos);
            //this.ddlCondicionPago.DataValueField = "IdListaValorSistemaDetalle";
            //this.ddlCondicionPago.DataTextField = "Descripcion";
            //this.ddlCondicionPago.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlCondicionPago, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlTipoOrden.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposOrdenesCompras);
            this.ddlTipoOrden.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTipoOrden.DataTextField = "Descripcion";
            this.ddlTipoOrden.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOrden, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlFiliales.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFiliales.DataValueField = "IdFilial";
            this.ddlFiliales.DataTextField = "Filial";
            this.ddlFiliales.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFiliales, this.ObtenerMensajeSistema("SeleccioneOpcion"));

        }
        #region "Proveedores PopUp"
        void CtrBuscarProveedor_BuscarProveedor(CapProveedores e)
        {
            if (!string.IsNullOrEmpty(ctrBuscarProveedor.MiProveedor.IdProveedor.ToString()))
            {
                this.MiOrdenCompra.Entidad.IdEntidad = (int)ctrBuscarProveedor.MiProveedor.IdProveedor;
                //this.ctrOrdenesComprasValores.IniciarControl(this.MiOrdenCompra, this.GestionControl);
            }
        }
        #endregion
        #region Grilla
        //private void IniciarControlAgregar(CmpOrdenesCompras pOrden)
        //{
        //    this.MiOrdenCompra.SolicitudesComprasDetalles = ComprasF.OrdenCompraObtenerListaSCDPorProveedor(pOrden);

        //    //if (this.MiOrdenCompra.OrdenesComprasDetalles.Count() == 0)
        //    //{
        //    //    this.MiOrdenCompra.CodigoMensaje = "NoSeEncontroSolicitudPorProveedor";
        //    //    this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiOrdenCompra.CodigoMensaje));
        //    //}

        //    List<CmpOrdenesComprasDetalles> OCD = new List<CmpOrdenesComprasDetalles>();

        //    foreach (CmpSolicitudesComprasDetalles sc in this.MiOrdenCompra.SolicitudesComprasDetalles)
        //    {
        //        CmpOrdenesComprasDetalles nuevo = new CmpOrdenesComprasDetalles();
        //        nuevo.Producto = sc.Producto;
        //        nuevo.Descripcion = sc.Descripcion;
        //        nuevo.Cantidad = sc.Cantidad;
        //        nuevo.Precio = sc.PrecioUnitario;
        //        nuevo.ImporteIVA = (sc.AlicuotaIVA * sc.PrecioUnitario / 100);
        //        nuevo.Moneda.IdMoneda = sc.Moneda.IdMoneda;
        //        nuevo.Importe = (nuevo.Precio + nuevo.ImporteIVA) * nuevo.Cantidad;
        //        if (OCD.Exists(x => x.Producto.IdProducto == nuevo.Producto.IdProducto) /*ver si necesito revisar el precio*/ )
        //        {
        //            OCD.Find(x => x.Producto.IdProducto == nuevo.Producto.IdProducto).Cantidad += nuevo.Cantidad;
        //        }
        //        else
        //        {
        //            OCD.Add(nuevo);
        //        }
        //    }
        //    this.MiOrdenCompra.OrdenesComprasDetalles = OCD;

        //    AyudaProgramacion.CargarGrillaListas<CmpOrdenesComprasDetalles>(this.MiOrdenCompra.OrdenesComprasDetalles, false, this.gvDatos, true);
        //    this.upOrdenCompraDetalle.Update();
        //}
        private void PersistirDatosGrilla()
        {
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                DataRow dr;
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    var miOrden = this.MiOrdenCompra.OrdenesComprasDetalles[fila.RowIndex];
                    DropDownList ddlAlicuotaIVA = ((DropDownList)fila.FindControl("ddlAlicuotaIVA"));
                    decimal importeIva = ((HiddenField)fila.FindControl("hdfImporteIva")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfImporteIva")).Value.ToString().Replace('.', ','));
                    decimal subTotalConIva = ((HiddenField)fila.FindControl("hdfSubtotalConIva")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfSubtotalConIva")).Value.ToString().Replace('.', ','));

                    decimal cantidad = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtCantidad")).Decimal;
                    HiddenField hdfIdProducto = (HiddenField)fila.FindControl("hdfIdProducto");
                    HiddenField hdfProductoDetalle = (HiddenField)fila.FindControl("hdfProductoDetalle");
                    string NumeroRef = ((TextBox)fila.FindControl("txtNumeroReferencia")).Text;
                    //ImageButton btnBuscarProducto = (ImageButton)e.Row.FindControl("btnBuscarProducto");
                    ImageButton btnEliminar = (ImageButton)fila.FindControl("btnEliminar");
                    decimal PrecioUnitario = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtImporte")).Decimal;
                    DropDownList ddlProducto = ((DropDownList)fila.FindControl("ddlProducto"));
                    DropDownList ddlAfiliado = ((DropDownList)fila.FindControl("ddlAfiliado"));
                    HiddenField hdfIdAfiliado = (HiddenField)fila.FindControl("hdfIdAfiliado");
                    HiddenField hdfIdAfiliadoDescripcion = (HiddenField)fila.FindControl("hdfAfiliadoDescripcion");

                    if (hdfIdProducto.Value != string.Empty && Convert.ToInt32(hdfIdProducto.Value) > 0)
                    {
                        miOrden.Producto.IdProducto = Convert.ToInt32(hdfIdProducto.Value);
                        miOrden.Producto.Descripcion = hdfProductoDetalle.Value.ToString();

                    }
                    if (cantidad > 0) // && precioUnitario != string.Empty)
                    {
                        miOrden.Cantidad = cantidad;
                        miOrden.Importe = PrecioUnitario;
                        miOrden.ImporteIVA = importeIva;

                        miOrden.ImporteConIVA = subTotalConIva;
                        if (ddlAlicuotaIVA.SelectedValue != string.Empty)
                        {
                            miOrden.IVA = this.MisIvas[ddlAlicuotaIVA.SelectedIndex];
                        }
                    }
                    if (hdfIdAfiliado.Value != string.Empty && Convert.ToInt32(hdfIdAfiliado.Value) > 0)
                    {
                        miOrden.Afiliado.IdAfiliado = Convert.ToInt32(hdfIdAfiliado.Value);
                        miOrden.Afiliado.DescripcionAfiliado = hdfIdAfiliadoDescripcion.Value.ToString();

                    }
                    if (!string.IsNullOrEmpty(NumeroRef.ToString()))
                    {
                        miOrden.NumeroReferencia = NumeroRef;
                    }
                }
            }
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "CalcularTotal", "CalcularTotal();", true);
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            this.MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            if (e.CommandName == "Borrar")
            {
                this.PersistirDatosGrilla();
                //ELIMINA FILA
                //int indiceColeccion = Convert.ToInt32(e.CommandArgument);
                this.MiOrdenCompra.OrdenesComprasDetalles.RemoveAt(this.MiIndiceDetalleModificar);
                this.MiOrdenCompra.OrdenesComprasDetalles = AyudaProgramacion.AcomodarIndices<CmpOrdenesComprasDetalles>(this.MiOrdenCompra.OrdenesComprasDetalles);
                AyudaProgramacion.CargarGrillaListas<CmpOrdenesComprasDetalles>(this.MiOrdenCompra.OrdenesComprasDetalles, false, this.gvDatos, true);
            }
        }
        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {
            this.PersistirDatosGrilla();
            CmpOrdenesComprasDetalles item;
            item = new CmpOrdenesComprasDetalles();
            this.MiOrdenCompra.OrdenesComprasDetalles.Add(item);
            item.IndiceColeccion = this.MiOrdenCompra.OrdenesComprasDetalles.IndexOf(item);

            AyudaProgramacion.CargarGrillaListas<CmpOrdenesComprasDetalles>(this.MiOrdenCompra.OrdenesComprasDetalles, false, this.gvDatos, true);
        }
        private void IniciarGrilla(int pCantidad)
        {
            this.MiOrdenCompra.OrdenesComprasDetalles.Clear();
            CmpOrdenesComprasDetalles item;
            for (int i = 0; i < pCantidad; i++)
            {
                item = new CmpOrdenesComprasDetalles();
                this.MiOrdenCompra.OrdenesComprasDetalles.Add(item);
                item.IndiceColeccion = this.MiOrdenCompra.OrdenesComprasDetalles.IndexOf(item);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlAlicuotaIVA = ((DropDownList)e.Row.FindControl("ddlAlicuotaIVA"));
                Evol.Controls.CurrencyTextBox cantidad = (Evol.Controls.CurrencyTextBox)e.Row.FindControl("txtCantidad");

                TextBox NumeroRef = (TextBox)e.Row.FindControl("txtNumeroReferencia");
                //ImageButton btnBuscarProducto = (ImageButton)e.Row.FindControl("btnBuscarProducto");
                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");

                Evol.Controls.CurrencyTextBox PrecioUnitario = (Evol.Controls.CurrencyTextBox)e.Row.FindControl("txtImporte");
                DropDownList ddlProducto = ((DropDownList)e.Row.FindControl("ddlProducto"));
                DropDownList ddlAfiliado = ((DropDownList)e.Row.FindControl("ddlAfiliado"));
                ddlProducto.Enabled = true;
                //CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
                //ibtnConsultar.Visible = true;
                //ibtnConsultar.Attributes.Add("onchange", "CalcularItem();");
                PrecioUnitario.Attributes.Add("onchange", "CalcularItem();");

                //NumericTextBox cantidadPagada = (NumericTextBox)e.Row.FindControl("txtCantidadPagada");
                //cantidadPagada.Enabled = true;
                //cantidadPagada.Attributes.Add("onchange", "CalcularItem();");
                cantidad.Attributes.Add("onchange", "CalcularItem();");
                ddlAlicuotaIVA.Attributes.Add("onchange", "CalcularItem();");

                ddlProducto.Enabled = false;
                //producto.Enabled = false;
                cantidad.Enabled = false;
                PrecioUnitario.Enabled = false;
                ddlAfiliado.Enabled = false;

                ddlAlicuotaIVA.Enabled = false;
                btnEliminar.Visible = false;

                ddlAlicuotaIVA.Items.Clear();
                ddlAlicuotaIVA.SelectedValue = null;
                ddlAlicuotaIVA.DataSource = MisIvas;
                ddlAlicuotaIVA.DataValueField = "IdIVAAlicuota";
                ddlAlicuotaIVA.DataTextField = "Descripcion";
                ddlAlicuotaIVA.DataBind();
                if (ddlAlicuotaIVA.Items.Count == 0)
                    AyudaProgramacion.AgregarItemSeleccione(ddlAlicuotaIVA, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                if (this.GestionControl == Gestion.Agregar)
                {
                    ddlProducto.Enabled = true;
                    //producto.Enabled = false;
                    cantidad.Enabled = true;
                    PrecioUnitario.Enabled = true;
                    ddlAfiliado.Enabled = true;
                    NumeroRef.Enabled = true;
                    ddlAlicuotaIVA.Enabled = true;
                    btnEliminar.Visible = true;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                if (this.GestionControl == Gestion.Consultar
                    || this.GestionControl == Gestion.Autorizar
                    || this.GestionControl == Gestion.Anular)
                {
                    foreach (CmpOrdenesComprasDetalles det in this.MiOrdenCompra.OrdenesComprasDetalles)
                    {
                        this.MiOrdenCompra.TotalOrden += det.ImporteConIVA;
                    }
                    Label lblTotalOrden = (Label)e.Row.FindControl("lblTotalOrden");
                    lblTotalOrden.Text = this.MiOrdenCompra.TotalOrden.ToString("C2");
                }
            }
        }
        #endregion
        #region Mapeo Datos
        private void MapearControlesObjeto(CmpOrdenesCompras pOrden)
        {
            this.MiOrdenCompra.TipoOrdenCompra.IdTipoOrdenCompra = Convert.ToInt32(this.ddlTipoOrden.SelectedValue);
            //this.MiOrdenCompra.CondicionPago.IdCondicionPago = Convert.ToInt32(this.ddlCondicionPago.SelectedValue);
            this.MiOrdenCompra.DireccionDestino = this.txtDireccion.Text;
            this.MiOrdenCompra.FechaEntrega = this.txtFechaEntrega.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaEntrega.Text);
            this.MiOrdenCompra.Observacion = this.txtObservacion.Text;

            /*Proveedor*/
            this.MiOrdenCompra.Proveedor = this.ctrBuscarProveedor.MiProveedor;
            this.MiOrdenCompra.OrdenesComprasValores = this.ctrOrdenesComprasValores.ObtenerOrdenesComprasValores();
        }
        private void MapearObjetoControles(CmpOrdenesCompras pOrden)
        {
            this.txtFechaEntrega.Text = pOrden.FechaEntrega.HasValue ? pOrden.FechaEntrega.Value.ToShortDateString() : string.Empty;
            // this.ddlCondicionPago.SelectedValue = (pOrden.CondicionPago.IdCondicionPago).ToString();
            this.ddlTipoOrden.SelectedValue = (pOrden.TipoOrdenCompra.IdTipoOrdenCompra).ToString();
            this.txtDireccion.Text = pOrden.DireccionDestino;
            this.txtObservacion.Text = pOrden.Observacion;

            if (pOrden.IdFilialDestino.HasValue && pOrden.IdFilialDestino > 0)
            {
                ListItem item3 = this.ddlFiliales.Items.FindByValue(pOrden.IdFilialDestino.ToString());
                if (item3 == null)
                    this.ddlFiliales.Items.Add(new ListItem(pOrden.FilialDestino, pOrden.IdFilialDestino.ToString()));
                this.ddlFiliales.SelectedValue = pOrden.IdFilialDestino.ToString();
            }
            /*Proveedor*/
            this.ctrBuscarProveedor.IniciarControl(pOrden.Proveedor, GestionControl);
            this.ctrOrdenesComprasValores.IniciarControl(this.MiOrdenCompra, this.GestionControl);
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
                    foreach (CmpOrdenesComprasDetalles det in this.MiOrdenCompra.OrdenesComprasDetalles)
                    {
                        det.EstadoColeccion = EstadoColecciones.Agregado;
                    }
                    guardo = ComprasF.OrdenCompraAgregar(this.MiOrdenCompra);
                    break;
                case Gestion.Autorizar:
                    this.MapearControlesObjeto(this.MiOrdenCompra);
                    this.MiOrdenCompra.EstadoColeccion = EstadoColecciones.Modificado;
                    this.MiOrdenCompra.Estado.IdEstado = (int)EstadosOrdenesCompras.Autorizado;
                    foreach (CmpOrdenesComprasDetalles det in this.MiOrdenCompra.OrdenesComprasDetalles)
                    {
                        det.EstadoColeccion = EstadoColecciones.Modificado;
                    }
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