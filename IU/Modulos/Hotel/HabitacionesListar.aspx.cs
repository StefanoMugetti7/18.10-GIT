using Comunes.Entidades;
using Hoteles;
using Hoteles.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Hotel
{
    public partial class HabitacionesListar : PaginaSegura
    {
        private DataTable MisHabitaciones
        {
            get { return (DataTable)Session[this.MiSessionPagina + "HabitacionesListarMisHabitaciones"]; }
            set { Session[this.MiSessionPagina + "HabitacionesListarMisHabitaciones"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroSocio, this.btnBuscar);

                //this.btnAgregar.Visible = this.ValidarPermiso("ReservasAgregar.aspx");
                this.CargarCombos();

                //this.txtFechaDesde.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                //this.txtFechaHasta.Text = DateTime.Now.ToShortDateString();

                HTLHabitaciones parametros = this.BusquedaParametrosObtenerValor<HTLHabitaciones>();
                if (parametros.BusquedaParametros)
                {
                    //this.txtNumeroSocio.Text = parametros.Afiliado.IdAfiliado.ToString();
                    //this.txtPrefijoNumeroreserva.Text = parametros.PrefijoNumeroreserva;
                    //this.txtNumeroreserva.Text = parametros.Numeroreserva;
                    //this.ddlTiporeserva.SelectedValue = parametros.Tiporeserva.IdTiporeserva.ToString();
                    //this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    //this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    //this.ddlFilial.SelectedValue = parametros.Filial.IdFilial == 0 ? string.Empty : parametros.Filial.IdFilial.ToString();
                    //this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    //this.ddlReservasLotes.SelectedValue = parametros.IdHabitacionLoteEnviado == 0 ? string.Empty : parametros.IdHabitacionLoteEnviado.ToString();
                    // this.txt
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            HTLHabitaciones parametros = this.BusquedaParametrosObtenerValor<HTLHabitaciones>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/HabitacionesAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            HTLHabitaciones reserva = new HTLHabitaciones();
            reserva.IdHabitacion = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdHabitacion"].ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdHabitacion", reserva.IdHabitacion);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/HabitacionesModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/HabitacionesConsultar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                ibtnConsultar.Visible = this.ValidarPermiso("ReservasConsultar.aspx");

                DataRowView dr = (DataRowView)e.Row.DataItem;

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            HTLHabitaciones parametros = this.BusquedaParametrosObtenerValor<HTLHabitaciones>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<HTLHabitaciones>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisHabitaciones;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisHabitaciones = this.OrdenarGrillaDatos<DataTable>(this.MisHabitaciones, e);
            this.gvDatos.DataSource = this.MisHabitaciones;
            this.gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisHabitaciones;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }

        private void CargarCombos()
        {
            HTLHoteles hotel = new HTLHoteles();
            hotel.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.ddlHoteles.DataSource = HotelesF.HotelesObtenerListaActiva(hotel);
            this.ddlHoteles.DataValueField = "IdHotel";
            this.ddlHoteles.DataTextField = "Descripcion";
            this.ddlHoteles.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlHoteles, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void CargarLista(HTLHabitaciones pHabitaciones)
        { HTLHoteles hotel = new HTLHoteles();
            pHabitaciones.IdHotel = this.ddlHoteles.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlHoteles.SelectedValue);
            pHabitaciones.NumeroHabitacion = this.txtNumeroHabitacion.Text;
            pHabitaciones.NombreHabitacion = this.txtNombreHabitacion.Text;
            //pReserva.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            //pReserva.Filial.IdFilial = this.ddlFilial.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlFilial.SelectedValue);
            //pReserva.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            //pReserva.IdHabitacionLoteEnviado = this.ddlReservasLotes.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlReservasLotes.SelectedValue);
            pHabitaciones.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pHabitaciones.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<HTLHabitaciones>(pHabitaciones);
            this.MisHabitaciones = HotelesF.HabitacionesObtenerListaGrilla(pHabitaciones);
            this.gvDatos.DataSource = this.MisHabitaciones;
            this.gvDatos.PageIndex = pHabitaciones.IndiceColeccion;
            this.gvDatos.DataBind();

            if (this.MisHabitaciones.Rows.Count > 0)
            {
                btnExportarExcel.Visible = true;
            }
            else
            {
                btnExportarExcel.Visible = false;
            }
        }
    }
}
