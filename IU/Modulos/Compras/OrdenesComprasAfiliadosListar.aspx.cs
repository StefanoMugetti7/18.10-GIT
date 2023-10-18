using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Comunes.Entidades;
using System.Collections;
using Proveedores.Entidades;
using Compras;
using Proveedores;
using Afiliados.Entidades;
using Reportes.FachadaNegocio;

namespace IU.Modulos.Compras
{
    public partial class OrdenesComprasAfiliadosListar : PaginaAfiliados
    {
        private List<CmpOrdenesCompras> MisOrdenesCompras
        {
            get { return (List<CmpOrdenesCompras>)Session[this.MiSessionPagina + "OrdenesComprasListarMisOrdenesCompras"]; }
            set { Session[this.MiSessionPagina + "OrdenesComprasListarMisOrdenesCompras"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ctrBuscarProveedorPopUp.ProveedoresBuscar += new CuentasPagar.Controles.ProveedoresBuscarPopUp.ProveedoresBuscarEventHandler(ctrBuscarProveedorPopUp_ProveedoresBuscarSeleccionar);
            if (!this.IsPostBack)
            {

                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoOrdenCompra, this.btnBuscar);
               // BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigo, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("OrdenesComprasAfiliadosAgregar.aspx");
                this.CargarCombos();

                CmpOrdenesCompras parametros = this.BusquedaParametrosObtenerValor<CmpOrdenesCompras>();
                if (parametros.BusquedaParametros)
                {
                    this.txtCodigoOrdenCompra.Text = parametros.IdOrdenCompra == 0 ? String.Empty : parametros.IdOrdenCompra.ToString();

                    this.ddlCondicionPago.SelectedValue = parametros.CondicionPago.IdCondicionPago.ToString();

                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();

                    if (parametros.Proveedor.IdProveedor > 0)
                    {
                        parametros.Proveedor = ProveedoresF.ProveedoresObtenerDatosCompletos(parametros.Proveedor);
                        this.ddlApellido.Items.Add(new ListItem(parametros.Proveedor.RazonSocial, parametros.Proveedor.IdProveedor.ToString()));
                        this.ddlApellido.SelectedValue = parametros.Proveedor.IdProveedor.ToString();
                        hdfIdAfiliado.Value = parametros.Proveedor.IdProveedor.ToString();
                    }
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CmpOrdenesCompras parametros = this.BusquedaParametrosObtenerValor<CmpOrdenesCompras>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/OrdenesComprasAfiliadosAgregar.aspx"), true);
        }

        private void CargarCombos()
        {
            this.ddlCondicionPago.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.CondicionesPagos);
            this.ddlCondicionPago.DataValueField = "IdListaValorDetalle";
            this.ddlCondicionPago.DataTextField = "Descripcion";
            this.ddlCondicionPago.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCondicionPago, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosOrdenesCompras));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstados.SelectedValue = ((int)EstadosTodos.Todos).ToString();

        }

        #region "Grilla"

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Anular.ToString()
                || e.CommandName == Gestion.Autorizar.ToString()
                || e.CommandName == Gestion.Impresion.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CmpOrdenesCompras pOrdenCompra = this.MisOrdenesCompras[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdOrdenCompra", pOrdenCompra.IdOrdenCompra);

            if (e.CommandName == Gestion.Anular.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/OrdenesComprasAfiliadosAnular.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/OrdenesComprasAfiliadosConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Autorizar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/OrdenesComprasAfiliadosAutorizar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CmpOrdenesCompras, "OrdenesCompras", pOrdenCompra, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this, "OrdenesCompras", this.UsuarioActivo);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                CmpOrdenesCompras orden = (CmpOrdenesCompras)e.Row.DataItem;

                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton autorizar = (ImageButton)e.Row.FindControl("btnAutorizar");
                ImageButton anular = (ImageButton)e.Row.FindControl("btnAnular");
                ibtnConsultar.Visible = this.ValidarPermiso("OrdenesComprasAfiliadosConsultar.aspx");

                switch (orden.Estado.IdEstado)
                {
                    case (int)EstadosOrdenesCompras.Activo:
                        autorizar.Visible = this.ValidarPermiso("OrdenesComprasAfiliadosAutorizar.aspx");
                        anular.Visible = this.ValidarPermiso("OrdenesComprasAfiliadosAnular.aspx");
                        break;

                    case (int)EstadosOrdenesCompras.Autorizado:
                        anular.Visible = this.ValidarPermiso("OrdenesComprasAfiliadosAnular.aspx");
                        break;
                    
                }
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

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisOrdenesCompras;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }
        #endregion

        #region "Proveedores PopUp"

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            CmpCotizaciones parametros = this.BusquedaParametrosObtenerValor<CmpCotizaciones>();
            parametros.Proveedor = new CapProveedores();
            ddlApellido.SelectedValue = "";
            hdfIdAfiliado.Value = "";
            //this.txtCodigo.Text = string.Empty;
            //this.txtProveedor.Text = string.Empty;
            this.BusquedaParametrosGuardarValor<CmpCotizaciones>(parametros);
            //this.btnBuscarProveedor.Visible = true;
            //this.btnLimpiar.Visible = false;
            this.UpdatePanel1.Update();
        }

        //protected void btnBuscarProveedor_Click(object sender, EventArgs e)
        //{
        //    this.ctrBuscarProveedorPopUp.IniciarControl();
        //}

        void ctrBuscarProveedorPopUp_ProveedoresBuscarSeleccionar(CapProveedores pProveedor)
        {
            CmpCotizaciones parametros = this.BusquedaParametrosObtenerValor<CmpCotizaciones>();
            parametros.Proveedor.RazonSocial = pProveedor.RazonSocial;
            parametros.Proveedor.IdProveedor = pProveedor.IdProveedor.Value;
            this.BusquedaParametrosGuardarValor<CmpCotizaciones>(parametros);
            this.MapearObjetoAControlesProveedor(pProveedor);
            //this.btnLimpiar.Visible = true;
            //this.btnBuscarProveedor.Visible = false; 
            this.UpdatePanel1.Update();
        }

        private void MapearObjetoAControlesProveedor(CapProveedores pProveedor)
        {
            if (pProveedor.IdProveedor > 0)
            {
                pProveedor = ProveedoresF.ProveedoresObtenerDatosCompletos(pProveedor);
                this.ddlApellido.Items.Add(new ListItem(pProveedor.RazonSocial, pProveedor.IdProveedor.ToString()));
                this.ddlApellido.SelectedValue = pProveedor.IdProveedor.ToString();
                hdfIdAfiliado.Value = pProveedor.IdProveedor.ToString();
            }
        }

        //protected void txtCodigo_TextChanged(object sender, EventArgs e)
        //{
        //    string txtCodigo = ((TextBox)sender).Text;
        //    CapProveedores parametro = new CapProveedores();
        //    parametro.IdProveedor = Convert.ToInt32(txtCodigo);
        //    parametro = ProveedoresF.ProveedoresObtenerDatosCompletos(parametro);
           
        //    if (parametro.IdProveedor > 0)
        //        this.ctrBuscarProveedorPopUp_ProveedoresBuscarSeleccionar(parametro);
        //    else
        //        this.ctrBuscarProveedorPopUp.IniciarControl();

        //}
        #endregion

        private void CargarLista(CmpOrdenesCompras pOrdenCompra)
        {
            //VER PARAMETROS QUE RECIBE EL STORE
            pOrdenCompra.IdOrdenCompra = this.txtCodigoOrdenCompra.Text == String.Empty ? 0 : Convert.ToInt32(this.txtCodigoOrdenCompra.Text);

            pOrdenCompra.CondicionPago.IdCondicionPago = this.ddlCondicionPago.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlCondicionPago.SelectedValue);
            pOrdenCompra.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            //VER QUE BUSQUE CON EL POP UP

            pOrdenCompra.Proveedor.IdProveedor = hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(hdfIdAfiliado.Value);
            pOrdenCompra.Proveedor.RazonSocial = hdfAfiliado.Value;
      if(pOrdenCompra.Proveedor.IdProveedor > 0)
            {
                this.ddlApellido.Items.Add(new ListItem(pOrdenCompra.Proveedor.RazonSocial, pOrdenCompra.Proveedor.IdProveedor.ToString()));
                this.ddlApellido.SelectedValue = pOrdenCompra.Proveedor.IdProveedor.ToString();
            }
            pOrdenCompra.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pOrdenCompra.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pOrdenCompra.Afiliado = this.MiAfiliado;
            pOrdenCompra.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CmpOrdenesCompras>(pOrdenCompra);
            this.MisOrdenesCompras = ComprasF.OrdenCompraObtenerLista(pOrdenCompra);
            this.gvDatos.DataSource = this.MisOrdenesCompras;
            this.gvDatos.PageIndex = pOrdenCompra.IndiceColeccion;
            this.gvDatos.DataBind();

            if (this.MisOrdenesCompras.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }
    }
}