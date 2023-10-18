using Comunes.Entidades;
using DayPilot.Web.Ui.Events.Scheduler;
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
    public partial class ReservasAgenda : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.CargarCombos();

                HTLReservas parametros = this.BusquedaParametrosObtenerValor<HTLReservas>();
                if (parametros.BusquedaParametros)
                {
                    this.ddlHoteles.SelectedValue = parametros.IdHotel == 0 ? string.Empty : parametros.IdHotel.ToString(); ;
                    DayPilotScheduler1.StartDate = parametros.FechaIngreso.HasValue ? parametros.FechaIngreso.Value : DateTime.Today;
                    DayPilotScheduler1.Days = parametros.FechaEgreso.HasValue ? (parametros.FechaEgreso.Value - parametros.FechaIngreso.Value).Days : 31;
                    if (DayPilotScheduler1.Days < 31)
                        DayPilotScheduler1.Days = 31;
                    this.txtFecha.Text = DayPilotScheduler1.StartDate.ToShortDateString(); //DayPilotScheduler1.StartDate.ToString("yyyy-MMMM");
                    this.CargarRecursos();
                }
                else
                {
                    this.CargarRecursos();
                    DayPilotScheduler1.StartDate = DateTime.Today;
                    DayPilotScheduler1.Days = 31;
                    this.txtFecha.Text = DateTime.Today.ToShortDateString();// DateTime.Today.ToString("yyyy-MMMM");
                }
                this.CargarEventos();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/ReservasAgregar.aspx"), true);
        }

        protected void DayPilotScheduler1_BeforeEventRender(object sender, BeforeEventRenderEventArgs e)
        {
            string color = e.DataItem["color"] as string;
            if (!String.IsNullOrEmpty(color))
            {
                e.DurationBarColor = color;
            }
        }

        protected void DayPilotScheduler1_OnEventClick(object sender, DayPilot.Web.Ui.Events.EventClickEventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdReserva", e.Id);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/ReservasModificar.aspx"), true);
        }

        private void CargarCombos()
        {
            HTLHoteles hotel = new HTLHoteles();
            hotel.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            List<HTLHoteles> hoteles = HotelesF.HotelesObtenerListaActiva(hotel);
            this.ddlHoteles.DataSource = hoteles;
            this.ddlHoteles.DataValueField = "IdHotel";
            this.ddlHoteles.DataTextField = "Descripcion";
            this.ddlHoteles.DataBind();
            if (hoteles.Exists(x => x.Filial.IdFilial == this.UsuarioActivo.FilialPredeterminada.IdFilial))
                this.ddlHoteles.SelectedValue = hoteles.Find(x => x.Filial.IdFilial == this.UsuarioActivo.FilialPredeterminada.IdFilial).Filial.IdFilial.ToString();
        }

        private void CargarRecursos()
        {
            this.DayPilotScheduler1.Resources.Clear();
            HTLHabitaciones filtro = new HTLHabitaciones();
            filtro.IdHotel = this.ddlHoteles.SelectedValue == string.Empty ? default(int) : Convert.ToInt32(this.ddlHoteles.SelectedValue);
            filtro.Estado.IdEstado = (int)Estados.Activo;
            List<HTLHabitaciones> habitaciones = HotelesF.HabitacionesObtenerAgenda(filtro);
            foreach(HTLHabitaciones r in habitaciones)
                this.DayPilotScheduler1.Resources.Add(r.NombreHabitacion, r.IdHabitacion.ToString());

        }

        private void CargarEventos()
        {
            HTLReservas filtro = new HTLReservas();
            filtro.IdHotel = this.ddlHoteles.SelectedValue==string.Empty ? default(int) : Convert.ToInt32( this.ddlHoteles.SelectedValue);
            filtro.FechaIngreso = DayPilotScheduler1.StartDate;
            filtro.FechaEgreso = DayPilotScheduler1.EndDate;
            filtro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<HTLReservas>(filtro);
            DayPilotScheduler1.DataSource = HotelesF.ReservasObtenerAgenda(filtro);
            DayPilotScheduler1.DataBind();
        }

        protected void ddlHoteles_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CargarRecursos();
            this.CargarEventos();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtFecha.Text))
            {
                DateTime fecha = Convert.ToDateTime(this.txtFecha.Text);
                int days = 0;
                if (fecha.Date == DateTime.Now.Date)
                    days = 31;
                else
                    days = DateTime.DaysInMonth(fecha.Year, fecha.Month);
                DayPilotScheduler1.StartDate = fecha;
                DayPilotScheduler1.Days = days;
                this.txtFecha.Text = fecha.ToShortDateString(); //fecha.ToString("yyyy-MMMM");
                this.CargarEventos();
            }
        }
        //protected void txtFecha_TextChanged(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(this.txtFecha.Text))
        //    {
        //        DateTime fecha = Convert.ToDateTime(this.txtFecha.Text);
        //        int days = 0;
        //        if (fecha.Date == DateTime.Now.Date)
        //            days = 31;
        //        else
        //            days = DateTime.DaysInMonth(fecha.Year, fecha.Month);
        //        DayPilotScheduler1.StartDate = fecha;
        //        DayPilotScheduler1.Days = days;
        //        this.txtFecha.Text = fecha.ToShortDateString(); //fecha.ToString("yyyy-MMMM");
        //        this.CargarEventos();
        //    }
        //}

        protected void DayPilotScheduler1_TimeRangeSelected(object sender, DayPilot.Web.Ui.Events.TimeRangeSelectedEventArgs e)
        {
            HTLReservas reserva = new HTLReservas();
            reserva.IdHotel = Convert.ToInt32(ddlHoteles.SelectedValue);
            reserva.FechaIngreso = e.Start;
            reserva.FechaEgreso = e.End;
            HTLReservasDetalles detalle = new HTLReservasDetalles();
            detalle.IdHabitacion = Convert.ToInt64(e.Resource);
            detalle = HotelesF.HabitacionesSeleccionarHabitacionesDetallesAgenda(detalle);
            detalle.FechaIngreso = reserva.FechaIngreso;
            detalle.FechaEgreso = reserva.FechaEgreso;
            TimeSpan t = detalle.FechaEgreso.Value - detalle.FechaIngreso.Value;
            detalle.Cantidad = Convert.ToInt32( t.TotalDays);
            detalle.Estado.IdEstado = (int)Estados.Activo;
            detalle.EstadoColeccion = EstadoColecciones.Agregado;
            reserva.ReservasDetalles.Add(detalle);
            detalle.IndiceColeccion = reserva.ReservasDetalles.IndexOf(detalle);

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("HTLReservas", reserva);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/ReservasAgregar.aspx"), true);
        }
    }
}