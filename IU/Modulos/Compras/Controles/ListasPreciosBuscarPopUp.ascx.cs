using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using Compras;

namespace IU.Modulos.Compras.Controles
{
    public partial class ListasPreciosBuscarPopUp : ControlesSeguros
    {
        private List<CMPListasPrecios> MisListasPrecios
        {
            get { return (List<CMPListasPrecios>)Session[this.MiSessionPagina + "ListasPreciosPopUpMiListaPrecio"]; }
            set { Session[this.MiSessionPagina + "ListasPreciosPopUpMiListaPrecio"] = value; }
        }
        
        public delegate void ListasPreciosBuscarEventHandler(CMPListasPrecios e);
        public event ListasPreciosBuscarEventHandler ListasPreciosBuscarSeleccionar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            
            if (!this.IsPostBack)
            {
                this.CargarCombos();
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoLista, this.btnBuscar);
            }
        }

        public void IniciarControl()
        {
            this.CargarCombos();
            this.txtCodigoLista.Text = string.Empty;
            this.txtDescripcion.Text = string.Empty;
            this.txtFechaDesde.Text = string.Empty;
            this.txtFechaHasta.Text = string.Empty;


            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalPopUp();", true);
        }

        public void IniciarControl(CMPListasPrecios lista)
        {
            this.MisListasPrecios = ComprasF.ListasPreciosObtenerListaFiltro(lista);
            AyudaProgramacion.CargarGrillaListas<CMPListasPrecios>(this.MisListasPrecios, false , gvDatos, true);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalPopUp();", true);
        }

         private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosListasPrecios));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstados.SelectedValue = ((int)EstadosTodos.Todos).ToString();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CMPListasPrecios parametros = this.BusquedaParametrosObtenerValor<CMPListasPrecios>();
            CargarLista(parametros);
        }

        private void CargarLista(CMPListasPrecios pParametros)
        {
            //VER PARAMETROS QUE RECIBE EL STORE
            pParametros.IdListaPrecio = this.txtCodigoLista.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCodigoLista.Text);
            pParametros.Descripcion = this.txtDescripcion.Text;
            pParametros.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pParametros.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pParametros.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pParametros.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pParametros.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CMPListasPrecios>(pParametros);
            this.MisListasPrecios = ComprasF.ListasPreciosObtenerListaFiltro(pParametros);
            this.gvDatos.DataSource = this.MisListasPrecios;
            this.gvDatos.PageIndex = pParametros.IndiceColeccion;
            this.gvDatos.DataBind();
        }

        #region "Grilla"

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CMPListasPrecios miLista = this.MisListasPrecios[indiceColeccion];
            miLista = ComprasF.ListasPreciosObtenerDatosCompletosPopUp(miLista);

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                if (this.ListasPreciosBuscarSeleccionar != null)
                {
                    this.ListasPreciosBuscarSeleccionar(miLista);
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalPopUp();", true);
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalPopUp();", true);
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisListasPrecios.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        //private bool ValidarFecha(CMPListasPrecios listaPrecio)
        //{
        //    if (listaPrecio.FechaInicioVigencia <= DateTime.Now)
        //        return false;
        //    else
        //        return true;
        //}

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CMPListasPrecios parametros = this.BusquedaParametrosObtenerValor<CMPListasPrecios>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CMPListasPrecios>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisListasPrecios;
            gvDatos.DataBind();

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalPopUp();", true);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisListasPrecios = this.OrdenarGrillaDatos<CMPListasPrecios>(this.MisListasPrecios, e);
            this.gvDatos.DataSource = this.MisListasPrecios;
            this.gvDatos.DataBind();

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalPopUp();", true);
        }

        #endregion

    }
}