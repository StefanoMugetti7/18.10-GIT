using Afiliados;
using Comunes.Entidades;
using Medicina;
using Medicina.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace IU.Modulos.Medicina
{
    public partial class TurnosMedicosListar : PaginaSegura
    {
        private List<MedTurnos> MisPrestaciones
        {
            get { return (List<MedTurnos>)Session[this.MiSessionPagina + "TurnosMedicosListarMisPrestaciones"]; }
            set { Session[this.MiSessionPagina + "TurnosMedicosListarMisPrestaciones"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                this.CargarCombos();
                this.txtFechaDesde.Text = DateTime.Now.ToShortDateString();
                this.txtFechaHasta.Text = DateTime.Now.ToShortDateString();
                MedTurnos parametros = this.BusquedaParametrosObtenerValor<MedTurnos>();
                if (parametros.BusquedaParametros)
                {
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlPrestador.SelectedValue = parametros.Prestador.IdPrestador.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            MedTurnos parametros = this.BusquedaParametrosObtenerValor<MedTurnos>();
            this.CargarLista(parametros);
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            MedTurnos prestacion = this.MisPrestaciones[indiceColeccion];

            PaginaAfiliados pagina = new PaginaAfiliados();
            pagina.Guardar(this.MiSessionPagina, AfiliadosF.AfiliadosObtenerDatosCompletos(prestacion.Afiliado));

            MedHistoriasClinicas historiaClinica = new MedHistoriasClinicas();
            historiaClinica.IdAfiliado = prestacion.Afiliado.IdAfiliado;
            if (!MedicinaF.HistoriasClinicasValidarExiste(historiaClinica))
            {
                historiaClinica.FechaAlta = DateTime.Now;
                historiaClinica.IdUsuarioAlta = this.UsuarioActivo.IdUsuarioEvento;
                historiaClinica.Estado.IdEstado = (int)Estados.Activo;
                if (!MedicinaF.HistoriasClinicasAgregar(historiaClinica))
                {
                    this.MostrarMensaje(historiaClinica.CodigoMensaje, true);
                    return;
                }
            }

            this.MisParametrosUrl = new Hashtable
            {
                { "IdTurno", prestacion.IdTurno }
            };

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/HistoriasClinicasConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PrestacionesAgregar.aspx"), true);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");

                //Permisos btnEliminar
                ibtnConsultar.Visible = false;// this.ValidarPermiso("HistoriasClinicasConsultar.aspx");
                ibtnModificar.Visible = false;//this.ValidarPermiso("HistoriasClinicasModificar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPrestaciones.Count);
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
            this.MisPrestaciones = this.OrdenarGrillaDatos<MedTurnos>(this.MisPrestaciones, e);
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
        private void CargarLista(MedTurnos pPrestacion)
        {
            pPrestacion.Prestador.IdPrestador = this.ddlPrestador.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPrestador.SelectedValue);
            pPrestacion.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pPrestacion.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);

            pPrestacion.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<MedTurnos>(pPrestacion);
            this.MisPrestaciones = MedicinaF.TurnosObtenerListaFiltro(pPrestacion);
            this.gvDatos.DataSource = this.MisPrestaciones;
            this.gvDatos.PageIndex = pPrestacion.IndiceColeccion;
            this.gvDatos.DataBind();
            AyudaProgramacion.CargarGrillaListas<MedTurnos>(this.MisPrestaciones, false, this.gvDatos, true);
        }
    }
}
