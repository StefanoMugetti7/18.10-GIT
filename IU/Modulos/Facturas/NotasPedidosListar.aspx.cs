using Comunes.Entidades;
using Facturas;
using Facturas.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Facturas
{
    public partial class NotasPedidosListar : PaginaSegura
    {
        private DataTable MisNotasPedidos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "NotasPedidosListarMisNotasPedidos"]; }
            set { Session[this.MiSessionPagina + "NotasPedidosListarMisNotasPedidos"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("NotasPedidosAgregar.aspx");
                this.CargarCombos();
                VTANotasPedidos parametros = this.BusquedaParametrosObtenerValor<VTANotasPedidos>();
                if (parametros.BusquedaParametros)
                {
                    this.ddlNumeroSocio.SelectedValue = parametros.Afiliado.IdAfiliado.ToString();
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            VTANotasPedidos parametros = this.BusquedaParametrosObtenerValor<VTANotasPedidos>();
            this.CargarLista(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/NotasPedidosAgregar.aspx"), true);
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            VTANotasPedidos NotaPedido = new VTANotasPedidos();
            NotaPedido.IdNotaPedido = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdNotaPedido"].ToString());

            this.MisParametrosUrl = new Hashtable
            {
                { "IdNotaPedido", NotaPedido.IdNotaPedido }
            };

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/NotasPedidosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/NotasPedidosConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Anular.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/NotasPedidosAnular.aspx"), true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                VTANotasPedidos NotaPedidoPDF = new VTANotasPedidos();
                NotaPedidoPDF.IdNotaPedido = NotaPedido.IdNotaPedido;
                NotaPedidoPDF.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
                TGEArchivos archivo = new TGEArchivos();
                archivo.Archivo = NotaPedidoPDF.NotaPedidoPDF;
                if (archivo.Archivo != null)
                    listaArchivos.Add(archivo);
                //TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton anular = (ImageButton)e.Row.FindControl("btnAnular");

                //Permisos btnEliminar
                consultar.Visible = this.ValidarPermiso("NotasPedidosConsultar.aspx");
                modificar.Visible = this.ValidarPermiso("NotasPedidosModificar.aspx");
                anular.Visible = this.ValidarPermiso("NotasPedidosAnular.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisNotasPedidos.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            VTANotasPedidos parametros = this.BusquedaParametrosObtenerValor<VTANotasPedidos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<VTANotasPedidos>(parametros);
            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisNotasPedidos;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisNotasPedidos = this.OrdenarGrillaDatos<DataTable>(this.MisNotasPedidos, e);
            this.gvDatos.DataSource = this.MisNotasPedidos;
            this.gvDatos.DataBind();
        }
        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisNotasPedidos;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }
        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosNotasPedidos));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstados.SelectedValue = ((int)EstadosTodos.Todos).ToString();

            this.ddlFilial.Items.Clear();
            this.ddlFilial.DataSource = TGEGeneralesF.FilialesEntregaObtenerListaActiva();
            this.ddlFilial.DataValueField = "IdFilialEntrega";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void CargarLista(VTANotasPedidos pNotaPedido)
        {
            //VER PARAMETROS QUE RECIBE EL STORE
            pNotaPedido.Afiliado.IdAfiliado = this.hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdAfiliado.Value);
            if (pNotaPedido.Afiliado.IdAfiliado != 0)
            {
                this.ddlNumeroSocio.Items.Add(new ListItem(hdfRazonSocial.Value, hdfIdAfiliado.Value));
                this.ddlNumeroSocio.SelectedValue = hdfIdAfiliado.Value;
            }
            else
            {
                AyudaProgramacion.AgregarItemSeleccione(ddlNumeroSocio, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            pNotaPedido.Filial.IdFilial = ddlFilial.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFilial.SelectedValue);
            pNotaPedido.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pNotaPedido.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pNotaPedido.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pNotaPedido.BusquedaParametros = true;

            this.BusquedaParametrosGuardarValor<VTANotasPedidos>(pNotaPedido);
            this.MisNotasPedidos = FacturasF.NotasPedidosObtenerGrilla(pNotaPedido);
            this.gvDatos.DataSource = this.MisNotasPedidos;
            this.gvDatos.PageIndex = pNotaPedido.IndiceColeccion;
            this.gvDatos.DataBind();

            if (this.MisNotasPedidos.Rows.Count > 0)
                this.btnExportarExcel.Visible = true;
            else
                this.btnExportarExcel.Visible = false;
        }
    }
}