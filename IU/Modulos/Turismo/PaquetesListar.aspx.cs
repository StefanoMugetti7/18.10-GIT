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
    public partial class PaquetesListar : PaginaSegura
    {
        private DataTable MisPaquetes
        {
            get { return (DataTable)Session[this.MiSessionPagina + "PaquetesListarMisPaquetes"]; }
            set { Session[this.MiSessionPagina + "PaquetesListarMisPaquetes"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.gvDatos.PageSizeEvent += this.GvDatos_PageSizeEvent;
            if (!this.IsPostBack)
            {
                TurPaquetes parametros = this.BusquedaParametrosObtenerValor<TurPaquetes>();
                this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista("TurPaquetes");
                this.ddlEstado.DataValueField = "IdEstado";
                this.ddlEstado.DataTextField = "Descripcion";
                this.ddlEstado.DataBind();
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDescripcion, this.btnBuscar);
                if (parametros.BusquedaParametros)
                {
                    this.txtDescripcion.Text = parametros.Nombre;//aca seria el filtro
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
                AyudaProgramacion.InsertarItemSeleccione(this.ddlEstado, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
        }
        private void GvDatos_PageSizeEvent(int pageSize)
        {
            TurPaquetes parametros = this.BusquedaParametrosObtenerValor<TurPaquetes>();
            parametros.PageIndex = 0;
            this.gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            this.CargarLista(parametros);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TurPaquetes parametros = this.BusquedaParametrosObtenerValor<TurPaquetes>();
            parametros.PageIndex = 0;
            this.gvDatos.PageIndex = 0;
            this.CargarLista(parametros);
            this.UpdatePanel1.Update();
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Turismo/PaquetesAgregar.aspx"), true);
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
                { "IdPaquete", idMaquina }
            };

            if (e.CommandName == Gestion.Modificar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Turismo/PaquetesModificar.aspx"), true);
            else if (e.CommandName == Gestion.Consultar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Turismo/PaquetesConsultar.aspx"), true);

        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                modificar.Visible = this.ValidarPermiso("PaquetesModificar.aspx");
                consultar.Visible = this.ValidarPermiso("PaquetesConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPaquetes.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TurPaquetes parametros = BusquedaParametrosObtenerValor<TurPaquetes>();
            this.gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            this.CargarLista(parametros);
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisPaquetes = OrdenarGrillaDatos<DataTable>(this.MisPaquetes, e);
            this.gvDatos.DataSource = this.MisPaquetes;
            this.gvDatos.DataBind();
        }
        #endregion
        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisPaquetes;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }
        private void CargarLista(TurPaquetes pParametro)
        {
            pParametro.Nombre = txtDescripcion.Text;//aca iria el filtro
            pParametro.Estado.IdEstado = this.ddlEstado.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlEstado.SelectedValue);

            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pParametro.BusquedaParametros = true;
            pParametro.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(this.UsuarioActivo.PageSize);
            this.gvDatos.PageSize = pParametro.PageSize;
            this.BusquedaParametrosGuardarValor<TurPaquetes>(pParametro);

            this.MisPaquetes = TurismoF.PaquetesObtenerListaGrilla(pParametro);
            this.gvDatos.DataSource = this.MisPaquetes;
            this.gvDatos.VirtualItemCount = this.MisPaquetes.Rows.Count > 0 ? Convert.ToInt32(this.MisPaquetes.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + this.gvDatos.VirtualItemCount.ToString();
            this.gvDatos.PageIndex = pParametro.PageIndex;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(this.gvDatos);
        }
    }
}