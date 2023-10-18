using Comunes.Entidades;
using CRM;
using CRM.Entidades;
using Generales.FachadaNegocio;
using Prestamos;
using Prestamos.Entidades;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using static CRM.Entidades.CRMRequerimientos;

namespace IU.Modulos.CRM
{
    public partial class RequerimientosListar : PaginaSegura
    {
        private DataTable CardsBootStrapDataTable
        {
            get { return (DataTable)Session[this.MiSessionPagina + "RequerimientosListarMisRequerimientos"]; }
            set { Session[this.MiSessionPagina + "RequerimientosListarMisRequerimientos"] = value; }
        }
        private DataTable MisRequerimientos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "RequerimientosListarMisRequerimientos"]; }
            set { Session[this.MiSessionPagina + "RequerimientosListarMisRequerimientos"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            gvDatos.PageSizeEvent += GvDatos_PageSizeEvent;
            if (!this.IsPostBack)
            {
                this.CardsBootStrap = new StringBuilder();
                this.CargarCardsBootStrap();
                this.MenuCards = this.CardsBootStrap == null ? string.Empty : this.CardsBootStrap.ToString();
                this.ltrCards.Text = this.MenuCards;
                this.CargarCombos();
                CRMRequerimientos parametros = this.BusquedaParametrosObtenerValor<CRMRequerimientos>();
                if (parametros.BusquedaParametros)
                {
                    txtDescripcion.Text = parametros.Descripcion;
                    CargarLista(parametros);
                }
            }
        }
        private string MenuCards
        {
            get
            {
                if (Session["MaestraMenuHtml"] != null)
                { return (string)Session["MaestraMenuHtml"]; }
                else
                { return string.Empty; }
            }
            set { Session["MaestraMenuHtml"] = value; }
        }
        private StringBuilder CardsBootStrap;

        private void CargarCardsBootStrap()
        {
            CRMRequerimientos filtro = new CRMRequerimientos();
            DataTable cards = RequerimientosF.RequerimientosCargarCardsBootStrapListar(filtro);
            if (cards.Rows.Count > 0)
            {
                foreach (DataRow fila in cards.Rows)
                {
                    this.CardsBootStrap.AppendFormat("     <a href=\"#\" class=\"link-prestamos\" onclick=\"javascript:EjecutarFiltro({0});\">", fila["IdEstado"]);
                    this.CardsBootStrap.AppendLine(" <div class=\"card card-prestamos\">");
                    this.CardsBootStrap.AppendFormat(" <div class=\"card-body {0} \">", fila["Color"]);
                    this.CardsBootStrap.AppendFormat(" <h5 class=\"card-title text-left card-prestamosh5\" >{0}</h5>", fila["Cantidad"]);
                    this.CardsBootStrap.AppendFormat("<p class=\"card-prestamosp card-text text-left\">{0}</p>", fila["EstadoDescripcion"]);
                    this.CardsBootStrap.AppendLine("</div>");
                    this.CardsBootStrap.AppendLine("</div>");
                    this.CardsBootStrap.AppendLine("</a>");

                }
            }
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosRequerimientos));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlEstado, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlPrioridad.DataSource = RequerimientosF.RequerimientosObtenerPrioridades();
            this.ddlPrioridad.DataValueField = "IdPrioridad";
            this.ddlPrioridad.DataTextField = "Prioridad";
            this.ddlPrioridad.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlPrioridad, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void GvDatos_PageSizeEvent(int pageSize)
        {
            CRMRequerimientos parametros = this.BusquedaParametrosObtenerValor<CRMRequerimientos>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            UsuarioActivo.PageSize = pageSize;
            CargarLista(parametros);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CRMRequerimientos parametros = this.BusquedaParametrosObtenerValor<CRMRequerimientos>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CRM/RequerimientosAgregar.aspx"), true);
        }

        #region GV
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int id = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable
            {
                { "IdRequerimiento", id }
            };

            if (e.CommandName == Gestion.Modificar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CRM/RequerimientosModificar.aspx"), true);
            else if (e.CommandName == Gestion.Consultar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CRM/RequerimientosConsultar.aspx"), true);
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                var valor = ((DataRowView)e.Row.DataItem).Row["EstadoIdEstado"].ToString();
                int? idEstado = Convert.ToInt32(valor == string.Empty ? "0" : valor.ToString());
                if (idEstado.HasValue)
                {
                    modificar.Visible = this.ValidarPermiso("RequerimientosModificar.aspx");
                    consultar.Visible = this.ValidarPermiso("RequerimientosConsultar.aspx");
                    if (idEstado == (int)EstadosRequerimientos.Solucionado)
                    {
                        modificar.Visible = false;
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisRequerimientos.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CRMRequerimientos parametros = BusquedaParametrosObtenerValor<CRMRequerimientos>();
            gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            CargarLista(parametros);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            MisRequerimientos = OrdenarGrillaDatos<DataTable>(MisRequerimientos, e);
            gvDatos.DataSource = MisRequerimientos;
            gvDatos.DataBind();
        }

        #endregion
        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            gvDatos.AllowPaging = false;
            gvDatos.DataSource = MisRequerimientos;
            gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", gvDatos);
        }
        private void CargarLista(CRMRequerimientos pParametro)
        {

            pParametro.Tabla = null;
            pParametro.IdRefTabla = (Nullable<int>)null;

            pParametro.Descripcion = txtDescripcion.Text;
            pParametro.IdPrioridad = this.ddlPrioridad.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPrioridad.SelectedValue);
            pParametro.Estado.IdEstado = this.ddlEstado.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.Tecnico = this.txtAsignadoA.Text == string.Empty ? null : this.txtAsignadoA.Text;
            pParametro.FechaCierre = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);//COMODIN PARA SP -> evito crear otra propiedad
            pParametro.FechaInternaResolucion = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);//COMODIN PARA SP -> evito crear otra propiedad

            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pParametro.BusquedaParametros = true;
            pParametro.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize);
            gvDatos.PageSize = pParametro.PageSize;
            this.BusquedaParametrosGuardarValor<CRMRequerimientos>(pParametro);

            this.MisRequerimientos = RequerimientosF.RequerimientosObtenerListaGrilla(pParametro);
            this.gvDatos.DataSource = this.MisRequerimientos;
            this.gvDatos.VirtualItemCount = MisRequerimientos.Rows.Count > 0 ? Convert.ToInt32(MisRequerimientos.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + gvDatos.VirtualItemCount.ToString();
            this.gvDatos.PageIndex = pParametro.PageIndex;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);

            if (this.MisRequerimientos.Rows.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }
    }
}