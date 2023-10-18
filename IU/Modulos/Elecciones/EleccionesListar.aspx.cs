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

namespace IU.Modulos.Elecciones
{
    public partial class EleccionesListar : PaginaSegura
    {

        private DataTable MisElecciones
        {
            get { return (DataTable)Session[this.MiSessionPagina + "EleccionesListarMisElecciones"]; }
            set { Session[this.MiSessionPagina + "EleccionesListarMisElecciones"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            gvDatos.PageSizeEvent += GvDatos_PageSizeEvent;
            if (!this.IsPostBack)
            {
                this.CargarCombos();
                EleElecciones parametros = this.BusquedaParametrosObtenerValor<EleElecciones>();
                if (parametros.BusquedaParametros)
                {
                    txtDescripcion.Text = parametros.Filtro;
                    CargarLista(parametros);
                }
            }
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstado, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
            private void GvDatos_PageSizeEvent(int pageSize)
        {
            EleElecciones parametros = this.BusquedaParametrosObtenerValor<EleElecciones>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            UsuarioActivo.PageSize = pageSize;
            CargarLista(parametros);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            EleElecciones parametros = this.BusquedaParametrosObtenerValor<EleElecciones>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            CargarLista(parametros);
            this.UpdatePanel1.Update();
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Elecciones/EleccionesAgregar.aspx"), true);
        }

        #region GV

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Listar.ToString() //GRAFICO
                || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int id = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdEleccion", id);

            if (e.CommandName == Gestion.Modificar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Elecciones/EleccionesModificar.aspx"), true);
            else if (e.CommandName == Gestion.Consultar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Elecciones/EleccionesConsultar.aspx"), true);
            else if (e.CommandName == Gestion.Listar.ToString())//GRAFICO
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Elecciones/EleccionesVotosGraficos.aspx"), true);

        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton detalle = (ImageButton)e.Row.FindControl("btnDetalle");//GRAFICO

                modificar.Visible = this.ValidarPermiso("EleccionesModificar.aspx");
                consultar.Visible = this.ValidarPermiso("EleccionesConsultar.aspx");

                DataRowView dr = (DataRowView)e.Row.DataItem;
                if (Convert.ToInt32(dr["EstadoIdEstado"]) != (int)Estados.Baja)
                {
                    detalle.Visible = this.ValidarPermiso("EleccionesVotosGraficos.aspx");//GRAFICO
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisElecciones.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            EleElecciones parametros = BusquedaParametrosObtenerValor<EleElecciones>();
            gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            CargarLista(parametros);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            MisElecciones = OrdenarGrillaDatos<DataTable>(MisElecciones, e);
            gvDatos.DataSource = MisElecciones;
            gvDatos.DataBind();
        }
        
        #endregion

        private void CargarLista(EleElecciones pParametro)
        {
            pParametro.Filtro = txtDescripcion.Text;
            pParametro.Estado.IdEstado = ddlEstado.SelectedValue == string.Empty ? -1 : Convert.ToInt32(ddlEstado.SelectedValue);
            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pParametro.BusquedaParametros = true;
            pParametro.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize);
            gvDatos.PageSize = pParametro.PageSize;
            this.BusquedaParametrosGuardarValor<EleElecciones>(pParametro);

            this.MisElecciones = EleccionesF.EleccionesObtenerListaGrilla(pParametro);
            this.gvDatos.DataSource = this.MisElecciones;
            this.gvDatos.VirtualItemCount = MisElecciones.Rows.Count > 0 ? Convert.ToInt32(MisElecciones.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + gvDatos.VirtualItemCount.ToString();
            this.gvDatos.PageIndex = pParametro.PageIndex;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);

        }
    }
}