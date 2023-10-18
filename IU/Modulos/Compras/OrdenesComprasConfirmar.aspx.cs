using Compras;
using Compras.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using ProcesosDatos;
using Proveedores.Entidades;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;

namespace IU.Modulos.Compras
{
    public partial class OrdenesComprasConfirmar : PaginaSegura
    {
        private List<CmpOrdenesCompras> MisOrdenesCompras
        {
            get { return (List<CmpOrdenesCompras>)Session[this.MiSessionPagina + "OrdenesComprasListarMisOrdenesCompras"]; }
            set { Session[this.MiSessionPagina + "OrdenesComprasListarMisOrdenesCompras"] = value; }
        }
        private List<int> MisProximosPeriodos
        {
            get { return (List<int>)Session[this.MiSessionPagina + "OrdenesComprasListarMisProximosPeriodos"]; }
            set { Session[this.MiSessionPagina + "OrdenesComprasListarMisProximosPeriodos"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(this.popUpMensajes_popUpMensajesPostBackAceptar);
            this.ctrBuscarProveedorPopUp.ProveedoresBuscar += new CuentasPagar.Controles.ProveedoresBuscarPopUp.ProveedoresBuscarEventHandler(this.ctrBuscarProveedorPopUp_ProveedoresBuscarSeleccionar);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoOrdenCompra, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigo, this.btnBuscar);
                //this.btnAgregar.Visible = this.ValidarPermiso("OrdenesComprasAgregar.aspx");
                this.CargarCombos();
                this.MisProximosPeriodos = ProcesosDatosF.ProcesosProcesamientoObtenerProximosPeriodosCargos();
                
                CmpOrdenesCompras parametros = this.BusquedaParametrosObtenerValor<CmpOrdenesCompras>();
                if (parametros.BusquedaParametros)
                {
                    this.txtCodigoOrdenCompra.Text = parametros.IdOrdenCompra == 0 ? String.Empty : parametros.IdOrdenCompra.ToString();
                    this.ddlCondicionPago.SelectedValue = parametros.CondicionPago.IdCondicionPago.ToString();
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    //this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.txtCodigo.Text = parametros.Proveedor.IdProveedor.ToString();
                    this.txtProveedor.Text = parametros.Proveedor.RazonSocial;
                    this.CargarLista(parametros);
                }
            }
            else
            {
                this.PersistirDatosGrilla();
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CmpOrdenesCompras parametros = this.BusquedaParametrosObtenerValor<CmpOrdenesCompras>();
            this.CargarLista(parametros);
        }
        //protected void btnAgregar_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/OrdenesComprasAgregar.aspx"), true);
        //}
        private void CargarCombos()
        {
            this.ddlCondicionPago.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.CondicionesPagos);
            this.ddlCondicionPago.DataValueField = "IdListaValorDetalle";
            this.ddlCondicionPago.DataTextField = "Descripcion";
            this.ddlCondicionPago.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCondicionPago, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            //this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosOrdenesCompras));
            //this.ddlEstados.DataValueField = "IdEstado";
            //this.ddlEstados.DataTextField = "Descripcion";
            //this.ddlEstados.DataBind();
            //this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            //this.ddlEstados.SelectedValue = ((int)EstadosTodos.Todos).ToString();
        }
        #region "Grilla"
        private void PersistirDatosGrilla()
        {
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                CheckBox chkIncluir = (CheckBox)fila.FindControl("chkIncluir");
                decimal importeDescontar = ((TextBox)fila.FindControl("txtImporteDescontar")).Text == string.Empty ? 0 : decimal.Parse(((TextBox)fila.FindControl("txtImporteDescontar")).Text, NumberStyles.Currency);
                decimal cuotas = ((TextBox)fila.FindControl("txtCuotas")).Text == string.Empty ? 0 : decimal.Parse(((TextBox)fila.FindControl("txtCuotas")).Text, NumberStyles.Currency);
                DropDownList proximosPeriodos = (DropDownList)fila.FindControl("ddlPeriodoProximosPeriodos");

                if (importeDescontar != 0)
                    this.MisOrdenesCompras[fila.DataItemIndex].ImporteDescontar = importeDescontar;

                if (cuotas != 0)
                    this.MisOrdenesCompras[fila.DataItemIndex].CuotasDescuentoAfiliado = Convert.ToInt32(cuotas);

                this.MisOrdenesCompras[fila.DataItemIndex].PeriodoPrimerVencimiento = proximosPeriodos.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(proximosPeriodos.SelectedValue);
                this.MisOrdenesCompras[fila.DataItemIndex].Check = chkIncluir.Checked;
            }
           
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (!(e.CommandName == Gestion.Consultar.ToString()

            //    ))
            //    return;

            //int index = Convert.ToInt32(e.CommandArgument);
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            //CmpOrdenesCompras pOrdenCompra = this.MisOrdenesCompras[indiceColeccion];

            //this.MisParametrosUrl = new Hashtable();
            //this.MisParametrosUrl.Add("IdOrdenCompra", pOrdenCompra.IdOrdenCompra);

            //if (e.CommandName == Gestion.Anular.ToString())
            //{
            //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/OrdenesComprasAnular.aspx"), true);
            //}

        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CmpOrdenesCompras ordenCompra = (CmpOrdenesCompras)e.Row.DataItem;
                CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
                ibtnConsultar.Visible = true;

                DropDownList proximosPeriodos = (DropDownList)e.Row.FindControl("ddlPeriodoProximosPeriodos");
                foreach (int i in this.MisProximosPeriodos)
                    proximosPeriodos.Items.Add(new ListItem(i.ToString(), i.ToString()));
                if (ordenCompra.PeriodoPrimerVencimiento.HasValue && this.MisProximosPeriodos.Exists(x => x == ordenCompra.PeriodoPrimerVencimiento))
                    proximosPeriodos.SelectedValue = ordenCompra.PeriodoPrimerVencimiento.Value.ToString();
                //if (items.Count != 1)
                //    AyudaProgramacion.AgregarItemSeleccione(this.ddlPeriodoVencimiento, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisOrdenesCompras.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CmpOrdenesCompras parametros = this.BusquedaParametrosObtenerValor<CmpOrdenesCompras>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CmpOrdenesCompras>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisOrdenesCompras;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisOrdenesCompras = this.OrdenarGrillaDatos<CmpOrdenesCompras>(this.MisOrdenesCompras, e);
            this.gvDatos.DataSource = this.MisOrdenesCompras;
            this.gvDatos.DataBind();
        }

        //protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        //{
        //    this.gvDatos.AllowPaging = false;
        //    this.gvDatos.DataSource = this.MisOrdenesCompras;
        //    this.gvDatos.DataBind();
        //    GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        //}
        #endregion
        #region "Proveedores PopUp"
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            CmpCotizaciones parametros = this.BusquedaParametrosObtenerValor<CmpCotizaciones>();
            parametros.Proveedor = new CapProveedores();
            this.txtCodigo.Text = string.Empty;
            this.txtProveedor.Text = string.Empty;
            this.BusquedaParametrosGuardarValor<CmpCotizaciones>(parametros);
            this.UpdatePanel1.Update();
        }
        protected void btnBuscarProveedor_Click(object sender, EventArgs e)
        {
            this.ctrBuscarProveedorPopUp.IniciarControl();
        }
        void ctrBuscarProveedorPopUp_ProveedoresBuscarSeleccionar(CapProveedores pProveedor)
        {
            CmpCotizaciones parametros = this.BusquedaParametrosObtenerValor<CmpCotizaciones>();
            parametros.Proveedor.RazonSocial = pProveedor.RazonSocial;
            parametros.Proveedor.IdProveedor = pProveedor.IdProveedor.Value;
            this.BusquedaParametrosGuardarValor<CmpCotizaciones>(parametros);
            this.MapearObjetoAControlesProveedor(pProveedor);
            this.UpdatePanel1.Update();
        }

        private void MapearObjetoAControlesProveedor(CapProveedores pProveedor)
        {
            this.txtCodigo.Text = pProveedor.IdProveedor.Value.ToString();
            this.txtProveedor.Text = pProveedor.RazonSocial;
        }

        #endregion
        private void CargarLista(CmpOrdenesCompras pOrdenCompra)
        {
            //VER PARAMETROS QUE RECIBE EL STORE
            pOrdenCompra.IdOrdenCompra = this.txtCodigoOrdenCompra.Text == String.Empty ? 0 : Convert.ToInt32(this.txtCodigoOrdenCompra.Text);
            pOrdenCompra.CondicionPago.IdCondicionPago = this.ddlCondicionPago.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlCondicionPago.SelectedValue);
            pOrdenCompra.Estado.IdEstado = (int)EstadosOrdenesCompras.Autorizado;
            pOrdenCompra.TipoOrdenCompra.IdTipoOrdenCompra = (int)EnumTiposOrdenesCompras.Terceros;
            //VER QUE BUSQUE CON EL POP UP

            pOrdenCompra.Proveedor.IdProveedor = this.txtCodigo.Text == String.Empty ? 0 : Convert.ToInt32(this.txtCodigo.Text);
            pOrdenCompra.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pOrdenCompra.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pOrdenCompra.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CmpOrdenesCompras>(pOrdenCompra);
            this.MisOrdenesCompras = ComprasF.OrdenCompraObtenerTercerosAutorizadas(pOrdenCompra);
            this.gvDatos.DataSource = this.MisOrdenesCompras;
            this.gvDatos.PageIndex = pOrdenCompra.IndiceColeccion;
            this.gvDatos.DataBind();

            if (this.MisOrdenesCompras.Count > 0)
            {
                this.btnAceptar.Visible = true;
                this.UpdatePanel3.Update();
            }
            //if (this.MisOrdenesCompras.Count > 0)
            //    btnExportarExcel.Visible = true;
            //else
            //    btnExportarExcel.Visible = false;
        }
        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            CmpOrdenesCompras parametros = this.BusquedaParametrosObtenerValor<CmpOrdenesCompras>();
            this.CargarLista(parametros);
            this.UpdatePanel1.Update();

        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            
            //this.Page.Validate("OrdenesComprasDatos");
            if (!this.Page.IsValid)
                return;
            bool guardo = true;
            this.PersistirDatosGrilla();
            Objeto pObjeto = new Objeto();
            foreach (CmpOrdenesCompras orden in this.MisOrdenesCompras)
            {
                orden.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                orden.EstadoColeccion = EstadoColecciones.Modificado;
            }
            guardo = ComprasF.OrdenesComprasConfirmarLista(this.MisOrdenesCompras, pObjeto);

            if (guardo)
            {
                CmpOrdenesCompras parametros = this.BusquedaParametrosObtenerValor<CmpOrdenesCompras>();
                this.CargarLista(parametros);
                this.UpdatePanel1.Update();
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(pObjeto.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(pObjeto.CodigoMensaje, true, pObjeto.CodigoMensajeArgs);
            }

        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/OrdenesComprasListar.aspx"), true);
        }
    }
}