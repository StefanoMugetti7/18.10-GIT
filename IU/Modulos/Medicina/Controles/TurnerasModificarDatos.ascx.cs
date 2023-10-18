using Comunes.Entidades;
using CrystalDecisions.ReportAppServer.Prompting;
using DayPilot.Web.Ui.Events.Calendar;
using Generales.FachadaNegocio;
using Medicina;
using Medicina.Entidades;
using System;
using System.Collections.Generic;

namespace IU.Modulos.Medicina.Controles
{
    public partial class TurnerasModificarDatos : ControlesSeguros
    {
        private List<MedTurneras> MisTurneras
        {
            get { return (List<MedTurneras>)Session[this.MiSessionPagina + "TurnerasModificarDatosMisTurneras"]; }
            set { Session[this.MiSessionPagina + "TurnerasModificarDatosMisTurneras"] = value; }
        }
        private MedTurnos MiTurno
        {
            get { return (MedTurnos)Session[this.MiSessionPagina + "TurnerasModificarDatosMiTurno"]; }
            set { Session[this.MiSessionPagina + "TurnerasModificarDatosMiTurno"] = value; }
        }
        private MedPrestadores MiPrestador
        {
            get { return (MedPrestadores)Session[this.MiSessionPagina + "TurnerasModificarDatosMiPrestador"]; }
            set { Session[this.MiSessionPagina + "TurnerasModificarDatosMiPrestador"] = value; }
        }
        //public delegate void ModificarDatosEventHandler(MedTurneras e, Gestion pGestion);
        //public event ModificarDatosEventHandler ModificarDatosAceptar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrTurnos.ModificarDatosAceptar += new TurnosModificarDatosPopUp.ModificarDatosAceptarEventHandler(ctrTurnos_ModificarDatosAceptar);
            //this.ctrTurnos.ModificarDatosCancelar += new TurnosModificarDatosPopUp.ModificarDatosAceptarEventHandler(ctrTurnos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                MedTurneras parametros = this.BusquedaParametrosObtenerValor<MedTurneras>();
                if (parametros.BusquedaParametros)
                {
                    this.ddlPrestador.SelectedValue = parametros.Prestador.IdPrestador.ToString();//aca seria el filtro
                    this.ddlFilial.SelectedValue = parametros.Filial.IdFilial.ToString();//aca seria el filtro
                }
            }
        }
        //void ctrTurnos_ModificarDatosCancelar(MedTurnos e, Gestion pGestion)
        //{
        //    this.MostrarTurneras();
        //}
        void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            this.CargarTurnera(null);
        }
        void ctrTurnos_ModificarDatosAceptar(MedTurnos e, Gestion pGestion)
        {
            //this.MiTurno.EstadoColeccion = EstadoColecciones.Agregado;
            //this.MiTurno.Estado.IdEstado = (int)EstadosTurnos.Reservado;
            MedTurneras turnera = this.MisTurneras.Find(x => x.GuidTurnera == this.MiTurno.GuidTurnera);
            turnera.Prestador = this.MiTurno.Prestador;
            turnera.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(turnera, pGestion);
            //if (pGestion==Gestion.Agregar)
            //    turnera.Turnos.Add(this.MiTurno);
            //if (pGestion == Gestion.Modificar)
            turnera.Turnos[turnera.Turnos.FindIndex(x => x.GuidTurno == e.GuidTurno)] = e;
            this.GuardarTurno(turnera);
            this.CargarTurnera(null);
        }
        public void IniciarControl(MedTurneras pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            //this.MiTurnera = pParametro;
            this.GestionControl = pGestion;
            this.txtFechaDesde.Text = DateTime.Now.ToShortDateString();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //this.txtFe.Text = DateTime.Now.ToShortDateString();
                    pParametro.Estado.IdEstado = (int)Estados.Activo;
                    break;
                case Gestion.Modificar:
                    //this.MapearObjetoAControles(this.MiTurnera);
                    break;
                case Gestion.Consultar:
                    //this.MapearObjetoAControles(this.MiTurnera);
                    break;
                default:
                    break;
            }
        }
        private void MapearControlesAObjeto(MedTurneras pParametro)
        {
            pParametro.Prestador.IdPrestador = this.ddlPrestador.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPrestador.SelectedValue);
            pParametro.IdPrestacion = this.ddlFilial.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFilial.SelectedValue);
            //pParametro.Especializacion.IdEspecializacion = this.ddlEspecialidad.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlEspecialidad.SelectedValue);
            //pParametro.Especializacion.Descripcion = this.ddlEspecialidad.SelectedItem.Text;
            //pParametro.EspecializacionPorDefecto  = this.chkPredeterminado.Checked;
            //pParametro.Estado.IdEstado = this.ddlEstados.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlEstados.SelectedValue);
        }
        private void MapearObjetoAControles(MedTurneras pParametro)
        {
            this.ddlFilial.SelectedValue = Convert.ToString(pParametro.IdPrestacion);
            this.ddlPrestador.SelectedValue = Convert.ToString(pParametro.Prestador.IdPrestador);
            //this.ddlEspecialidad.SelectedValue = pParametro.Especializacion.IdEspecializacion.ToString();
            //this.chkPredeterminado.Checked = pParametro.EspecializacionPorDefecto;
            //this.ddlEstados.SelectedValue = pParametro.Estado.IdEstado.ToString();
        }
        private void GuardarTurno(MedTurneras pTurnera)
        {
            bool guardo = false;
            pTurnera.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            guardo = MedicinaF.TurnerasAgregarActualizar(pTurnera);
            if (guardo)
                this.MostrarMensaje(pTurnera.CodigoMensaje, false);
            else
                this.MostrarMensaje(pTurnera.CodigoMensaje, true, pTurnera.CodigoMensajeArgs);
            this.upTurneras.Update();
        }
        protected void btnCancelar_Click(object sender, EventArgs e) { }
        protected void ddlPrestadores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlPrestador.SelectedValue))
            {
                MiPrestador = new MedPrestadores();
                this.MiPrestador.IdPrestador = Convert.ToInt32(this.ddlPrestador.SelectedValue);
                this.MiPrestador = MedicinaF.PrestadoresObtenerDatosCompletos(MiPrestador);
                this.ddlEspecializacion.DataSource = MiPrestador.ObtenerEspecializaciones();
                this.ddlEspecializacion.DataValueField = "IdEspecializacion";
                this.ddlEspecializacion.DataTextField = "Descripcion";
                this.ddlEspecializacion.DataBind();
                if (!string.IsNullOrEmpty(ddlFilial.SelectedValue))
                    this.CargarTurnera(null);
            }
            else
            {
                this.ddlEspecializacion.SelectedIndex = -1;
                this.ddlEspecializacion.SelectedValue = null;
                this.ddlEspecializacion.Items.Clear();
                this.ddlEspecializacion.ClearSelection();
            }
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEspecializacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void CargarCombos()
        {
            this.ddlFilial.DataSource = TGEGeneralesF.FilialesObenerLista();
            this.ddlFilial.DataValueField = "IdFilial";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            MedPrestadores prestador = new MedPrestadores();
            prestador.Estado.IdEstado = (int)Estados.Activo;
            this.ddlPrestador.DataSource = MedicinaF.PrestadoresObtenerListaFiltro(prestador);
            this.ddlPrestador.DataTextField = "ApellidoNombre";
            this.ddlPrestador.DataValueField = "IdPrestador";
            this.ddlPrestador.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlPrestador, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEspecializacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void CargarTurnera(MedTurneras pParametro)
        {
            MedTurneras filtro;
            if (pParametro == null)
                filtro = new MedTurneras();
            else
                filtro = pParametro;

            //DayPilotCalendar1.DataSource = _appointments;
            //DayPilotCalendar1.DataStartField = "AppointmentStart";
            //DayPilotCalendar1.DataEndField = "AppointmentEnd";
            //DayPilotCalendar1.DataIdField = "AppointmentId";
            //DayPilotCalendar1.DataTextField = "AppointmentPatientName";
            //DayPilotCalendar1.DataTagFields = "AppointmentStatus";
            //DayPilotCalendar1.DataBind();
            //DayPilotCalendar1.Update();
            //this.MiPrestador = new MedPrestadores();
            //this.MiPrestador.IdPrestador = this.ddlPrestador.SelectedValue==string.Empty ? 0 : Convert.ToInt32(this.ddlPrestador.SelectedValue);
            filtro.Prestador.IdPrestador = this.MiPrestador.IdPrestador;
            filtro.Especializacion.IdEspecializacion = ddlEspecializacion.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlEspecializacion.SelectedValue);
            filtro.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            filtro.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            filtro.Fecha = (DateTime)(this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text));
            filtro.Filial.IdFilial = this.ddlFilial.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFilial.SelectedValue);
            filtro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<MedTurneras>(filtro);
            this.MisTurneras = MedicinaF.TurnerasObtenerTurneras(filtro, this.dpcDias.Days);
            this.MostrarTurneras();
            this.upTurneras.Update();
        }
        private void MostrarTurneras()
        {
            List<MedTurnos> lis = new List<MedTurnos>();
            foreach (MedTurneras i in this.MisTurneras)
                lis.AddRange(i.Turnos);
            DateTime fecha = this.txtFechaDesde.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFechaDesde.Text);
            this.dpcDias.StartDate = fecha;
            this.dpcDias.DataSource = lis;
            this.dpcDias.DataStartField = "FechaHoraDesde";
            this.dpcDias.DataEndField = "FechaHoraHasta";
            this.dpcDias.DataIdField = "GuidTurno";
            this.dpcDias.DataTextField = "ApellidoNombre";
            //dpcDias.DataTagFields = "AppointmentStatus";
            this.dpcDias.DataBind();
            this.dpcDias.Visible = true;
            this.dpcDias.Update();
        }
        //protected void ddlPrestador_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(this.ddlPrestador.SelectedValue))
        //    {
        //        this.MiPrestador = new MedPrestadores();
        //        this.MiPrestador.IdPrestador = Convert.ToInt32(this.ddlPrestador.SelectedValue);
        //        //this.MiPrestador = MedicinaF.PrestadoresObtenerDatosCompletos(this.MiPrestador);
        //        this.CargarTurnera();
        //    }
        //}
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.CargarTurnera(null);
        }
        protected void dpcDias_OnBeforeEventRender(object sender, BeforeEventRenderEventArgs e)
        {
            MedTurnos turno = (MedTurnos)e.DataItem.Source;
            if (turno == null)
                return;
            switch (turno.Estado.IdEstado)
            {
                case (int)EstadosTurnos.Disponible:
                    e.DurationBarColor = "#688E26"; // Verde
                    e.EventClickEnabled = true;
                    break;
                case (int)EstadosTurnos.Reservado:
                    e.DurationBarColor = "#FAA613"; // Amarillo
                    e.EventClickEnabled = false;
                    break;
                case (int)EstadosTurnos.SalaDeEspera:
                    e.DurationBarColor = "#F44708";  // Naranja            
                    e.EventClickEnabled = false;
                    break;
                case (int)EstadosTurnos.Atendido:
                    e.DurationBarColor = "#083D77";  // Azul  
                    e.EventClickEnabled = false;
                    break;
            }
        }
        protected void dpcDias_OnEventClick(object sender, DayPilot.Web.Ui.Events.EventClickEventArgs e)
        {
            this.MiTurno = new MedTurnos();
            this.MiTurno.GuidTurno = new Guid(e.Id);
            //MedTurneras turnera = this.MisTurneras.Find(x=> x.GuidTurnera==x.Turnos.Find(y=>y.GuidTurno==this.MiTurno.GuidTurno).GuidTurnera);
            MedTurneras turnera = new MedTurneras();
            MedTurnos tmpTurno;
            foreach (MedTurneras item in this.MisTurneras)
            {
                tmpTurno = item.Turnos.Find(x => x.GuidTurno == this.MiTurno.GuidTurno);
                if (tmpTurno != null)
                {
                    this.MiTurno = tmpTurno;
                    this.MiTurno.Prestador = item.Prestador;
                    this.MiTurno.HashTransaccion = item.HashTransaccion;
                    turnera = item;
                    break;
                }
            }
            this.MiTurno = turnera.Turnos.Find(x => x.GuidTurno == this.MiTurno.GuidTurno);
            Gestion gestion = this.GestionControl;
            if (this.GestionControl == Gestion.Modificar && this.MiTurno.IdTurno == 0)
            {
                gestion = Gestion.Agregar;
                this.MiTurno.FechaHoraDesde = e.Start;
                this.MiTurno.FechaHoraHasta = e.End;
            }
            this.ctrTurnos.IniciarControl(this.MiTurno, gestion);
        }
        protected void dpcDias_Command(object sender, DayPilot.Web.Ui.Events.CommandEventArgs e) { }
        protected void dpcDias_TimeRangeSelected(object sender, DayPilot.Web.Ui.Events.TimeRangeSelectedEventArgs e) { }
    }
}