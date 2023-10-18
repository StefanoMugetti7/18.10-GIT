using Comunes.Entidades;
using CuentasPagar.Entidades;
using CuentasPagar.FachadaNegocio;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Compras
{
    public partial class AnticipoReservaListar : PaginaSegura
    {
        private DataTable MisSolicitudes
        {
            get { return (DataTable)Session[this.MiSessionPagina + "SolicitudesPagosListarMisSolicitudesAnticipos"]; }
            set { Session[this.MiSessionPagina + "SolicitudesPagosListarMisSolicitudesAnticipos"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.gvDatos.PageSizeEvent += GvDatos_PageSizeEvent;
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtFechaDesde, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtFechaHasta, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtFechaVencimientoDesde, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtFechaHasta, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroSolicitud, this.btnBuscar);
                CapSolicitudPago parametros = this.BusquedaParametrosObtenerValor<CapSolicitudPago>();
                this.CargarCombos();
                if (parametros.BusquedaParametros)
                {
                    this.txtNumeroSolicitud.Text = parametros.IdSolicitudPago == 0 ? String.Empty : parametros.IdSolicitudPago.ToString();
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.txtFechaVencimientoDesde.Text = parametros.FechaVencimientoDesde.HasValue ? parametros.FechaVencimientoDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaVencimientoHasta.Text = parametros.FechaVencimientoHasta.HasValue ? parametros.FechaVencimientoHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlFilial.SelectedValue = parametros.IdFilial > 0 ? parametros.IdFilial.ToString() : string.Empty;
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado > 0 ? parametros.Estado.IdEstado.ToString() : string.Empty;
                    this.CargarLista(parametros);
                }
            }
        }
        private void GvDatos_PageSizeEvent(int pageSize)
        {
            CapSolicitudPago parametros = this.BusquedaParametrosObtenerValor<CapSolicitudPago>();
            parametros.PageIndex = 0;
            this.gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            this.CargarLista(parametros);
        }
        private void CargarCombos()
        {
            this.ddlFilial.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFilial.DataValueField = "IdFilial";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlFilial.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.SelectedValue = ((int)Estados.Baja).ToString();
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CapSolicitudPago parametros = this.BusquedaParametrosObtenerValor<CapSolicitudPago>();
            parametros.PageIndex = 0;
            this.gvDatos.PageIndex = 0;
            this.CargarLista(parametros);
            this.UpdatePanel1.Update();
        }
        #region GV
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idMaquina = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable
            {
                { "IdSolicitudPago", idMaquina }
            };

            if (e.CommandName == Gestion.Modificar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/AnticipoReservaModificar.aspx"), true);
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                modificar.Visible = this.ValidarPermiso("AnticipoReservaModificar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisSolicitudes.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CapSolicitudPago parametros = BusquedaParametrosObtenerValor<CapSolicitudPago>();
            this.gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            this.CargarLista(parametros);
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisSolicitudes = OrdenarGrillaDatos<DataTable>(this.MisSolicitudes, e);
            this.gvDatos.DataSource = this.MisSolicitudes;
            this.gvDatos.DataBind();
        }
        #endregion
        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisSolicitudes;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }
        private void CargarLista(CapSolicitudPago pParametro)
        {
            pParametro.IdSolicitudPago = this.txtNumeroSolicitud.Text == String.Empty ? 0 : Convert.ToInt32(this.txtNumeroSolicitud.Text);
            pParametro.Entidad.IdEntidad = (int)EnumTGEEntidades.Proveedores;
            pParametro.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pParametro.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pParametro.FechaVencimientoDesde = this.txtFechaVencimientoDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaVencimientoDesde.Text);
            pParametro.FechaVencimientoHasta = this.txtFechaVencimientoHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaVencimientoHasta.Text);
            pParametro.IdFilial = this.ddlFilial.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlFilial.SelectedValue);
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pParametro.BusquedaParametros = true;
            pParametro.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(this.UsuarioActivo.PageSize);
            this.gvDatos.PageSize = pParametro.PageSize;
            this.BusquedaParametrosGuardarValor<CapSolicitudPago>(pParametro);

            this.MisSolicitudes = CuentasPagarF.SolicitudPagoObtenerGrillaAnticipoReservas(pParametro);
            this.gvDatos.DataSource = this.MisSolicitudes;
            this.gvDatos.VirtualItemCount = this.MisSolicitudes.Rows.Count > 0 ? Convert.ToInt32(this.MisSolicitudes.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + this.gvDatos.VirtualItemCount.ToString();
            this.gvDatos.PageIndex = pParametro.PageIndex;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(this.gvDatos);
        }
    }
}