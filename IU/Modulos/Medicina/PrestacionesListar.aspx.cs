using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Medicina.Entidades;
using Comunes.Entidades;
using System.Collections;
using Medicina;
using Afiliados;

namespace IU.Modulos.Medicina
{
    public partial class PrestacionesListar : PaginaSegura
    {
        private List<MedPrestaciones> MisPrestaciones
        {
            get { return (List<MedPrestaciones>)Session[this.MiSessionPagina + "PrestacionesListarMisPrestaciones"]; }
            set { Session[this.MiSessionPagina + "PrestacionesListarMisPrestaciones"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("PrestacionesAgregar.aspx");
                this.CargarCombos();
                this.txtFechaDesde.Text = DateTime.Now.ToShortDateString();
                this.txtFechaHasta.Text = DateTime.Now.AddDays(7).ToShortDateString();
                MedPrestaciones parametros = this.BusquedaParametrosObtenerValor<MedPrestaciones>();
                if (parametros.BusquedaParametros)
                {
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    if (parametros.Afiliado.IdAfiliado > 0)
                    {
                        parametros.Afiliado = AfiliadosF.AfiliadosObtenerDatosCompletos(parametros.Afiliado);

                        this.ddlApellido.Items.Add(new ListItem(parametros.Afiliado.DescripcionAfiliado.ToString(), parametros.Afiliado.IdAfiliado.ToString()));
                        this.ddlApellido.SelectedValue = parametros.Afiliado.IdAfiliado.ToString();
                        this.hdfIdAfiliado.Value = parametros.Afiliado.IdAfiliado.ToString();
                    }
                    if(parametros.Prestador.IdPrestador > 0)
                    {
                        this.ddlPrestador.SelectedValue = parametros.Prestador.IdPrestador.ToString();
                    }
                    this.CargarLista(parametros);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            MedPrestaciones parametros = this.BusquedaParametrosObtenerValor<MedPrestaciones>();
            this.CargarLista(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PrestacionesAgregar.aspx"), true);
        }
        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = (this.MisPrestaciones);
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            MedPrestaciones prestador = this.MisPrestaciones[indiceColeccion];

            this.MisParametrosUrl = new Hashtable
            {
                { "IdPrestacion", prestador.IdPrestacion }
            };

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PrestacionesConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PrestacionesModificar.aspx"), true);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");

                ibtnConsultar.Visible = this.ValidarPermiso("PrestacionesConsultar.aspx");
                ibtnModificar.Visible = this.ValidarPermiso("PrestacionesModificar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell
                {
                    ColumnSpan = cellCount,
                    HorizontalAlign = HorizontalAlign.Right,
                    Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPrestaciones.Count)
                };
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            MedPrestadores parametros = this.BusquedaParametrosObtenerValor<MedPrestadores>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<MedPrestadores>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisPrestaciones;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisPrestaciones = this.OrdenarGrillaDatos<MedPrestaciones>(this.MisPrestaciones, e);
            this.gvDatos.DataSource = this.MisPrestaciones;
            this.gvDatos.DataBind();
        }
        private void CargarCombos()
        {
            MedPrestadores prestador = new MedPrestadores();
            prestador.Estado.IdEstado = (int)Estados.Activo;
            this.ddlPrestador.DataSource = MedicinaF.PrestadoresObtenerListaFiltro(prestador);
            this.ddlPrestador.DataTextField = "ApellidoNombre";
            this.ddlPrestador.DataValueField = "IdPrestador";
            this.ddlPrestador.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlPrestador, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void CargarLista(MedPrestaciones pPrestacion)
        {
            pPrestacion.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pPrestacion.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pPrestacion.Afiliado.IdAfiliado = this.hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdAfiliado.Value);
            pPrestacion.Prestador.IdPrestador = this.ddlPrestador.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPrestador.SelectedValue);
            this.ddlApellido.Items.Add(new ListItem(this.hdfAfiliado.Value, this.hdfIdAfiliado.Value.ToString()));
            this.ddlApellido.SelectedValue = hdfIdAfiliado.Value.ToString();
            pPrestacion.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<MedPrestaciones>(pPrestacion);
            this.MisPrestaciones = MedicinaF.PrestacionesObtenerListaFiltro(pPrestacion);
            this.gvDatos.PageIndex = pPrestacion.IndiceColeccion;
            AyudaProgramacion.CargarGrillaListas<MedPrestaciones>(this.MisPrestaciones, false, this.gvDatos, true);
        }
    }
}
