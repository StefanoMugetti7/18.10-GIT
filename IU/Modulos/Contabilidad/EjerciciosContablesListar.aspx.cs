using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using Contabilidad;
using System.Collections;

namespace IU.Modulos.Contabilidad
{
    public partial class EjerciciosContablesListar : PaginaSegura
    {
        private List<CtbEjerciciosContables> MisEjerciciosContables
        {
            get { return (List<CtbEjerciciosContables>)Session[this.MiSessionPagina + "MisEjerciciosContables"]; }
            set { Session[this.MiSessionPagina + "MisEjerciciosContables"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDescripcion, this.btnBuscar);
                this.btnAgregar.Visible = this.ValidarPermiso("EjerciciosContablesAgregar.aspx");
                //this.txtFechaInicioDesde.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                //this.txtFechaInicioHasta.Text = DateTime.Now.ToShortDateString();
                //this.txtFechaFinDesde.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                //this.txtFechaFinHasta.Text = DateTime.Now.ToShortDateString();
                //this.txtFechaCierreDesde.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                //this.txtFechaCierreHasta.Text = DateTime.Now.ToShortDateString();
                //this.txtFechaCopiativoDesde.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                //this.txtFechaCopiativoHasta.Text = DateTime.Now.ToShortDateString();
                this.CargarCombos();
                CtbEjerciciosContables parametros = this.BusquedaParametrosObtenerValor<CtbEjerciciosContables>();
                if (parametros.BusquedaParametros)
                {
                    this.txtDescripcion.Text = parametros.Descripcion;
                    //this.txtFechaInicioDesde.Text = parametros.FechaInicioDesde.HasValue ? parametros.FechaInicioDesde.Value.ToShortDateString() : string.Empty;
                    //this.txtFechaInicioHasta.Text = parametros.FechaInicioHasta.HasValue ? parametros.FechaInicioHasta.Value.ToShortDateString() : string.Empty;
                    //this.txtFechaFinDesde.Text = parametros.FechaFinDesde.HasValue ? parametros.FechaFinDesde.Value.ToShortDateString() : string.Empty;
                    //this.txtFechaFinHasta.Text = parametros.FechaFinHasta.HasValue ? parametros.FechaFinHasta.Value.ToShortDateString() : string.Empty;
                    //this.txtFechaCierreDesde.Text = parametros.FechaCierreDesde.HasValue ? parametros.FechaCierreDesde.Value.ToShortDateString() : string.Empty;
                    //this.txtFechaCierreHasta.Text = parametros.FechaCierreHasta.HasValue ? parametros.FechaCierreHasta.Value.ToShortDateString() : string.Empty;
                    //this.txtFechaCopiativoDesde.Text = parametros.FechaCopiativoDesde.HasValue ? parametros.FechaCopiativoDesde.Value.ToShortDateString() : string.Empty;
                    //this.txtFechaCopiativoHasta.Text = parametros.FechaCopiativoHasta.HasValue ? parametros.FechaCopiativoHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CtbEjerciciosContables parametros = this.BusquedaParametrosObtenerValor<CtbEjerciciosContables>();
            this.CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/EjerciciosContablesAgregar.aspx"), true);
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
            CtbEjerciciosContables ejercicioContable = this.MisEjerciciosContables[indiceColeccion];
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdEjercicioContable", ejercicioContable.IdEjercicioContable);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/EjerciciosContablesModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/EjerciciosContablesConsultar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                //modificar.Visible = this.ValidarPermiso("AsientosModificar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisEjerciciosContables.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CtbEjerciciosContables parametros = this.BusquedaParametrosObtenerValor<CtbEjerciciosContables>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CtbEjerciciosContables>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisEjerciciosContables;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisEjerciciosContables = this.OrdenarGrillaDatos<CtbEjerciciosContables>(this.MisEjerciciosContables, e);
            this.gvDatos.DataSource = this.MisEjerciciosContables;
            this.gvDatos.DataBind();
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstado.SelectedValue = ((int)EstadosTodos.Todos).ToString();
        }

        private void CargarLista(CtbEjerciciosContables pEjercicioContable)
        {
            pEjercicioContable.Descripcion = this.txtDescripcion.Text.Trim();
            //pEjercicioContable.FechaInicioDesde = Convert.ToDateTime(this.txtFechaInicioDesde.Text.Trim());
            //pEjercicioContable.FechaInicioHasta = Convert.ToDateTime(this.txtFechaInicioHasta.Text.Trim());
            //pEjercicioContable.FechaFinDesde = Convert.ToDateTime(this.txtFechaFinDesde.Text.Trim());
            //pEjercicioContable.FechaFinHasta = Convert.ToDateTime(this.txtFechaFinHasta.Text.Trim());
            //pEjercicioContable.FechaCierreDesde = Convert.ToDateTime(this.txtFechaCierreDesde.Text.Trim());
            //pEjercicioContable.FechaCierreHasta = Convert.ToDateTime(this.txtFechaCierreHasta.Text.Trim());
            //pEjercicioContable.FechaCopiativoDesde = Convert.ToDateTime(this.txtFechaCopiativoDesde.Text.Trim());
            //pEjercicioContable.FechaCopiativoHasta = Convert.ToDateTime(this.txtFechaCopiativoHasta.Text.Trim());
            pEjercicioContable.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pEjercicioContable.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CtbEjerciciosContables>(pEjercicioContable);
            this.MisEjerciciosContables = ContabilidadF.EjerciciosContablesObtenerListaFiltro(pEjercicioContable);
            this.gvDatos.DataSource = this.MisEjerciciosContables;
            this.gvDatos.PageIndex = pEjercicioContable.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}