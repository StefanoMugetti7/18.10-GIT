using Comunes.Entidades;
using Elecciones;
using Elecciones.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EnumElecciones = Elecciones.Entidades.EnumElecciones;

namespace IU.Modulos.Elecciones
{
    public partial class ListasEleccionesListar : PaginaSegura
    {
        private DataTable MisListasElecciones
        {
            get { return (DataTable)Session[this.MiSessionPagina + "EleccionesListarMisListasElecciones"]; }
            set { Session[this.MiSessionPagina + "EleccionesListarMisListasElecciones"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            gvDatos.PageSizeEvent += GvDatos_PageSizeEvent;
            if (!this.IsPostBack)
            {
                this.CargarCombos();
                EleListasElecciones parametros = this.BusquedaParametrosObtenerValor<EleListasElecciones>();
                if (parametros.BusquedaParametros)
                {
                    txtDescripcion.Text = parametros.Filtro;
                    CargarLista(parametros);
                }
            }
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista("ELEListasElecciones");
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstado, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void GvDatos_PageSizeEvent(int pageSize)
        {
            EleListasElecciones parametros = this.BusquedaParametrosObtenerValor<EleListasElecciones>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            UsuarioActivo.PageSize = pageSize;
            CargarLista(parametros);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            EleListasElecciones parametros = this.BusquedaParametrosObtenerValor<EleListasElecciones>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Elecciones/ListasEleccionesAgregar.aspx"), true);
        }

        #region GV

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()
                || e.CommandName == Gestion.Autorizar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int id = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdListaEleccion", id);

            if (e.CommandName == Gestion.Modificar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Elecciones/ListasEleccionesModificar.aspx"), true);
            else if (e.CommandName == Gestion.Consultar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Elecciones/ListasEleccionesConsultar.aspx"), true);
            else if (e.CommandName == Gestion.Autorizar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Elecciones/ListasEleccionesAutorizar.aspx"), true);

        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton autorizar = (ImageButton)e.Row.FindControl("btnAutorizar");

                consultar.Visible = this.ValidarPermiso("ListasEleccionesConsultar.aspx");
                modificar.Visible = this.ValidarPermiso("ListasEleccionesModificar.aspx");
                DataRowView dr = (DataRowView)e.Row.DataItem;
                if (Convert.ToInt32(dr["EstadoIdEstado"]) == (int)EnumElecciones.Presentado)
                {
                    autorizar.Visible = this.ValidarPermiso("ListasEleccionesAutorizar.aspx");
                } 
                if (Convert.ToInt32(dr["EstadoIdEstado"]) == (int)EnumElecciones.Autorizado)
                {
                    modificar.Visible = false;
                }



            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisListasElecciones.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            EleListasElecciones parametros = BusquedaParametrosObtenerValor<EleListasElecciones>();
            gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            CargarLista(parametros);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            MisListasElecciones = OrdenarGrillaDatos<DataTable>(MisListasElecciones, e);
            gvDatos.DataSource = MisListasElecciones;
            gvDatos.DataBind();
        }

        #endregion

        private void CargarLista(EleListasElecciones pParametro)
        {
            pParametro.Filtro = txtDescripcion.Text;
            pParametro.Estado.IdEstado = ddlEstado.SelectedValue == string.Empty ? -1 : Convert.ToInt32(ddlEstado.SelectedValue);
            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pParametro.BusquedaParametros = true;
            pParametro.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize);
            gvDatos.PageSize = pParametro.PageSize;
            this.BusquedaParametrosGuardarValor<EleListasElecciones>(pParametro);

            this.MisListasElecciones = EleccionesF.ListasEleccionesObtenerListaGrilla(pParametro);
            this.gvDatos.DataSource = this.MisListasElecciones;
            this.gvDatos.VirtualItemCount = MisListasElecciones.Rows.Count > 0 ? Convert.ToInt32(MisListasElecciones.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + gvDatos.VirtualItemCount.ToString();
            this.gvDatos.PageIndex = pParametro.PageIndex;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);

        }
    }
}