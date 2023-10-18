using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Contabilidad;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using System.Collections;

namespace IU.Modulos.Contabilidad
{
    public partial class PeriodosIvasListar : PaginaSegura
    {
        private List<CtbPeriodosIvas> MisPeriodosIvas
        {
            get { return (List<CtbPeriodosIvas>)Session[this.MiSessionPagina + "PeriodosIVASMisPeriodosIvas"]; }
            set { Session[this.MiSessionPagina + "PeriodosIVASMisPeriodosIvas"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("PeriodosIvasAgregar.aspx");
                //this.txtFechaCierreDesde.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                //this.txtFechaCierreHasta.Text = DateTime.Now.ToShortDateString();
                this.CargarCombos();
                CtbPeriodosIvas parametros = this.BusquedaParametrosObtenerValor<CtbPeriodosIvas>();
                if (parametros.BusquedaParametros)
                {

                    //this.txtPeriodo.Text = parametros.Periodo == -1 ? string.Empty : parametros.Periodo.ToString();
                    //this.txtFechaCierreDesde.Text = parametros.FechaCierreDesde.HasValue ? parametros.FechaCierreDesde.Value.ToShortDateString() : string.Empty;
                    //this.txtFechaCierreHasta.Text = parametros.FechaCierreHasta.HasValue ? parametros.FechaCierreHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CtbPeriodosIvas parametros = this.BusquedaParametrosObtenerValor<CtbPeriodosIvas>();
            this.CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/PeriodosIvasAgregar.aspx"), true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CtbPeriodosIvas periodoIva = this.MisPeriodosIvas[indiceColeccion];
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdPeriodoIva", periodoIva.IdPeriodoIva);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/PeriodosIvasModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/PeriodosIvasConsultar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CtbPeriodosIvas periodo = (CtbPeriodosIvas)e.Row.DataItem;
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                modificar.Visible = this.ValidarPermiso("PeriodosIvasModificar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPeriodosIvas.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CtbPeriodosIvas parametros = this.BusquedaParametrosObtenerValor<CtbPeriodosIvas>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CtbPeriodosIvas>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisPeriodosIvas;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisPeriodosIvas = this.OrdenarGrillaDatos<CtbPeriodosIvas>(this.MisPeriodosIvas, e);
            this.gvDatos.DataSource = this.MisPeriodosIvas;
            this.gvDatos.DataBind();
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosPeriodosIVAS));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstado.SelectedValue = ((int)EstadosTodos.Todos).ToString();

            //Cargar Ejercicios contables
            List<CtbEjerciciosContables> lista = ContabilidadF.EjerciciosContablesObtenerListar(new CtbEjerciciosContables());
            List<CtbEjerciciosContables> filtro = new List<CtbEjerciciosContables>();
            foreach (CtbEjerciciosContables item in lista)
            {
                if (item.Estado.IdEstado != 0)
                    filtro.Add(item);
            }

            this.ddlEjercicioContable.DataSource = filtro;
            this.ddlEjercicioContable.DataValueField = "IdEjercicioContable";
            this.ddlEjercicioContable.DataTextField = "Descripcion";
            this.ddlEjercicioContable.DataBind();
            if (this.ddlEjercicioContable.Items.Count == 0 || this.ddlEjercicioContable.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlEjercicioContable, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void CargarLista(CtbPeriodosIvas pPeriodoIva)
        {
            //pPeriodoContable.Periodo = this.txtPeriodo.Text.Trim() != String.Empty ? Convert.ToInt32(this.txtPeriodo.Text.Trim()) : -1;
            //pPeriodoContable.FechaCierreDesde = Convert.ToDateTime(this.txtFechaCierreDesde.Text.Trim());
            //pPeriodoContable.FechaCierreHasta = Convert.ToDateTime(this.txtFechaCierreHasta.Text.Trim());
            pPeriodoIva.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pPeriodoIva.EjercicioContable.IdEjercicioContable = this.ddlEjercicioContable.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlEjercicioContable.SelectedValue);
            pPeriodoIva.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CtbPeriodosIvas>(pPeriodoIva);
            this.MisPeriodosIvas = ContabilidadF.PeriodosIvasObtenerListaFiltro(pPeriodoIva);
            this.gvDatos.DataSource = this.MisPeriodosIvas;
            this.gvDatos.PageIndex = pPeriodoIva.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}