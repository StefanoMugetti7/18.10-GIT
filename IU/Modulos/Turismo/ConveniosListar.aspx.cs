using Comunes.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Turismo;
using Turismo.Entidades;

namespace IU.Modulos.Turismo
{
    public partial class ConveniosListar : PaginaSegura
    {
        private DataTable MisConvenios
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ConveniosListarMisConvenios"]; }
            set { Session[this.MiSessionPagina + "ConveniosListarMisConvenios"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.gvDatos.PageSizeEvent += this.GvDatos_PageSizeEvent;
            if (!this.IsPostBack)
            {
                TurConvenios parametros = this.BusquedaParametrosObtenerValor<TurConvenios>();
                this.ddlHotel.DataSource = TurismoF.ConveniosObtenerHoteles(parametros);
                this.ddlHotel.DataValueField = "IdHotel";
                this.ddlHotel.DataTextField = "Hotel";
                this.ddlHotel.DataBind();
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtFechaDesde, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtFechaHasta, this.btnBuscar);
                if (parametros.BusquedaParametros)
                {
                    this.ddlHotel.SelectedValue = parametros.IdHotel.ToString();
                    this.txtFechaDesde.Text = parametros.FechaInicioConvenio.ToString();
                    this.txtFechaHasta.Text = parametros.FechaFinalConvenio.ToString();
                    this.CargarLista(parametros);
                }
                AyudaProgramacion.InsertarItemSeleccione(this.ddlHotel, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
        }
        private void GvDatos_PageSizeEvent(int pageSize)
        {
            TurConvenios parametros = this.BusquedaParametrosObtenerValor<TurConvenios>();
            parametros.PageIndex = 0;
            this.gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            this.CargarLista(parametros);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TurConvenios parametros = this.BusquedaParametrosObtenerValor<TurConvenios>();
            parametros.PageIndex = 0;
            this.gvDatos.PageIndex = 0;
            this.CargarLista(parametros);
            this.UpdatePanel1.Update();
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Turismo/ConveniosAgregar.aspx"), true);
        }
        #region GV
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idMaquina = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable
            {
                { "IdConvenio", idMaquina }
            };

            if (e.CommandName == Gestion.Modificar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Turismo/ConveniosModificar.aspx"), true);
            else if (e.CommandName == Gestion.Consultar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Turismo/ConveniosConsultar.aspx"), true);

        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                modificar.Visible = this.ValidarPermiso("ConveniosModificar.aspx");
                consultar.Visible = this.ValidarPermiso("ConveniosConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisConvenios.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TurConvenios parametros = BusquedaParametrosObtenerValor<TurConvenios>();
            this.gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            this.CargarLista(parametros);
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisConvenios = OrdenarGrillaDatos<DataTable>(this.MisConvenios, e);
            this.gvDatos.DataSource = this.MisConvenios;
            this.gvDatos.DataBind();
        }
        #endregion
        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisConvenios;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }
        private void CargarLista(TurConvenios pParametro)
        {
            pParametro.IdHotel = this.ddlHotel.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlHotel.SelectedValue);
            pParametro.FechaInicioConvenio = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text); 
            pParametro.FechaFinalConvenio = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text); 
            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pParametro.BusquedaParametros = true;
            pParametro.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(this.UsuarioActivo.PageSize);
            this.gvDatos.PageSize = pParametro.PageSize;
            this.BusquedaParametrosGuardarValor<TurConvenios>(pParametro);

            this.MisConvenios = TurismoF.ConveniosObtenerListaGrilla(pParametro);//TurismoF.PaquetesObtenerListaGrilla(pParametro);
            this.gvDatos.DataSource = this.MisConvenios;
            this.gvDatos.VirtualItemCount = this.MisConvenios.Rows.Count > 0 ? Convert.ToInt32(this.MisConvenios.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + this.gvDatos.VirtualItemCount.ToString();
            this.gvDatos.PageIndex = pParametro.PageIndex;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(this.gvDatos);
        }
    }
}