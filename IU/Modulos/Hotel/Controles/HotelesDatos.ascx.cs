using Afiliados;
using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
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

namespace IU.Modulos.Hotel.Controles
{
    public partial class HotelesDatos : ControlesSeguros
    {
        public HTLHoteles MiHotel
        {
            get { return PropiedadObtenerValor<HTLHoteles>("HotelesDatosMiHotel"); }
            set { PropiedadGuardarValor("HotelesDatosMiHotel", value); }
        }
        private DataTable MisHabitaciones
        {
            get { return (DataTable)Session[this.MiSessionPagina + "HabitacionesListarMisHabitaciones"]; }
            set { Session[this.MiSessionPagina + "HabitacionesListarMisHabitaciones"] = value; }
        }

        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            
            if (!IsPostBack)
            {
                if (MiHotel == null && GestionControl != Gestion.Agregar)
                {
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }

        public void IniciarControl(HTLHoteles pParametro, Gestion pGestion)
        {
            MiHotel = pParametro;
            GestionControl = pGestion;
            CargarCombos();
            switch (GestionControl)
            {
                case Gestion.Agregar:
                    btnAgregarHabitacion.Visible = false;
                    break;
                case Gestion.Anular:
                    txtDescripcion.Enabled = false;
                    //ddlCondicionFiscal.Enabled = false;
                    ddlEstado.Enabled = false;
                    ddlFilial.Enabled = false;
                    ddlHoraIngreso.Enabled = false;
                    ddlHoraEgreso.Enabled = false;
                    break;
                case Gestion.Modificar:
                    MiHotel = HotelesF.HotelesObtenerDatosCompletos(MiHotel);
                    this.CargarLista(MiHotel);
                    MapearObjetoAControles(MiHotel);
                    break;
                case Gestion.Consultar:
                    txtDescripcion.Enabled = false;
                    //ddlCondicionFiscal.Enabled = false;
                    ddlEstado.Enabled = false;
                    ddlFilial.Enabled = false;
                    ddlHoraIngreso.Enabled = false;
                    ddlHoraEgreso.Enabled = false;
                    this.CargarLista(MiHotel);
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            //ddlCondicionFiscal.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(Generales.Entidades.EnumTGEListasValoresSistemas.CondicionesFiscales);
            //ddlCondicionFiscal.DataValueField = "IdListaValorSistemaDetalle";
            //ddlCondicionFiscal.DataTextField = "Descripcion";
            //ddlCondicionFiscal.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(ddlCondicionFiscal, ObtenerMensajeSistema("SeleccioneOpcion"));

            ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            ddlEstado.DataValueField = "IdEstado";
            ddlEstado.DataTextField = "Descripcion";
            ddlEstado.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(ddlEstado, ObtenerMensajeSistema("SeleccioneOpcion"));

            ddlFilial.DataSource = UsuarioActivo.Filiales; //TGEGeneralesF.FilialesObenerLista();
            ddlFilial.DataValueField = "IdFilial";
            ddlFilial.DataTextField = "Filial";
            ddlFilial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(ddlFilial, ObtenerMensajeSistema("SeleccioneOpcion"));
            ddlFilial.SelectedValue = UsuarioActivo.FilialPredeterminada.IdFilial.ToString();

            DateTime StartTime = DateTime.ParseExact("00:00", "HH:mm", null);
            DateTime EndTime = DateTime.ParseExact("23:55", "HH:mm", null);
            TimeSpan Interval = new TimeSpan(0, 30, 0);
            ddlHoraIngreso.Items.Clear();
            ddlHoraEgreso.Items.Clear();
            while (StartTime <= EndTime)
            {
                ddlHoraIngreso.Items.Add(new ListItem(StartTime.ToString("HH:mm"), StartTime.ToString("HH:mm")));
                ddlHoraEgreso.Items.Add(new ListItem(StartTime.ToString("HH:mm"), StartTime.ToString("HH:mm")));
                StartTime = StartTime.Add(Interval);
            }
            AyudaProgramacion.InsertarItemSeleccione(ddlHoraIngreso, ObtenerMensajeSistema("SeleccioneOpcion"));
            AyudaProgramacion.InsertarItemSeleccione(ddlHoraEgreso, ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void MapearObjetoAControles(HTLHoteles pHoteles)
        {
            txtDescripcion.Text = pHoteles.Descripcion;

            //ListItem condicionFiscal = ddlCondicionFiscal.Items.FindByValue(pHoteles.Afiliado.CondicionFiscal.IdCondicionFiscal.ToString());
            //if (condicionFiscal == null && pHoteles.Afiliado.CondicionFiscal.IdCondicionFiscal > 0)
            //{
            //    ddlCondicionFiscal.Items.Add(new ListItem(pHoteles.Afiliado.CondicionFiscal.Descripcion, pHoteles.Afiliado.CondicionFiscal.IdCondicionFiscal.ToString()));
            //}
            //ddlCondicionFiscal.SelectedValue = pHoteles.Afiliado.CondicionFiscal.IdCondicionFiscal == 0 ? string.Empty : pHoteles.Afiliado.CondicionFiscal.IdCondicionFiscal.ToString();

            var item = ddlEstado.Items.FindByValue(pHoteles.Estado.ToString());
            if (item == null)
            {
                ddlEstado.Items.Add(new ListItem(pHoteles.Estado.Descripcion, pHoteles.Estado.IdEstado.ToString()));
            }
            ddlEstado.SelectedValue = pHoteles.Estado.IdEstado.ToString();

            ListItem filial = ddlFilial.Items.FindByValue(pHoteles.Filial.IdFilial.ToString());
            if (filial == null && pHoteles.Filial.IdFilial > 0)
            {
                ddlFilial.Items.Add(new ListItem(pHoteles.Filial.Filial, pHoteles.Filial.IdFilial.ToString()));
            }
            ddlFilial.SelectedValue = pHoteles.Filial.IdFilial == 0 ? string.Empty : pHoteles.Filial.IdFilial.ToString();

            ddlHoraIngreso.SelectedValue = (pHoteles.HorarioIngreso.ToString()).Substring(0, 5);
            ddlHoraEgreso.SelectedValue = (pHoteles.HorarioEgreso.ToString()).Substring(0, 5);
        }

        private void MapearControlesAObjeto(HTLHoteles pHoteles)
        {
            pHoteles.Descripcion = txtDescripcion.Text;
            //pHoteles.Afiliado.CondicionFiscal.IdCondicionFiscal = Convert.ToInt32(ddlCondicionFiscal.SelectedValue);
            pHoteles.Estado.IdEstado = Convert.ToInt32(ddlEstado.SelectedValue);
            pHoteles.Filial.IdFilial = Convert.ToInt32(ddlFilial.SelectedValue);
            pHoteles.HoraIngresoStr  = ddlHoraIngreso.SelectedValue;
            pHoteles.HoraEgresoStr  = ddlHoraEgreso.SelectedValue;
            pHoteles.HorarioIngreso = TimeSpan.Parse(pHoteles.HoraIngresoStr);
            pHoteles.HorarioEgreso = TimeSpan.Parse(pHoteles.HoraEgresoStr);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            btnAceptar.Visible = false;
            MapearControlesAObjeto(MiHotel);

            MiHotel.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            switch (GestionControl)
            {
                case Gestion.Agregar:
                    guardo = HotelesF.HotelesAgregar(MiHotel);
                    if (guardo)
                    {
                        MostrarMensaje(MiHotel.CodigoMensaje, false, MiHotel.CodigoMensajeArgs);
                    }
                    break;
                case Gestion.Anular:
                    MiHotel.Estado.IdEstado = (int)Estados.Baja;
                    guardo = HotelesF.HotelesModificar(MiHotel);
                    if (guardo)
                    {
                        MostrarMensaje(MiHotel.CodigoMensaje, false);
                    }
                    break;
                case Gestion.Modificar:
                    guardo = HotelesF.HotelesModificar(MiHotel);
                    if (guardo)
                    {
                        MostrarMensaje(MiHotel.CodigoMensaje, false);
                    }
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                btnAceptar.Visible = true;
                MostrarMensaje(MiHotel.CodigoMensaje, true, MiHotel.CodigoMensajeArgs);
                if (MiHotel.dsResultado != null)
                {
                    MiHotel.dsResultado = null;
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (ControlModificarDatosCancelar != null)
                ControlModificarDatosCancelar();
        }

        protected void btnAgregarHabitacion_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdHotel", MiHotel.IdHotel);

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
            this.MisHabitaciones = this.OrdenarGrillaDatos<DataTable>(MisHabitaciones, e);
            this.gvDatos.DataSource = this.MisHabitaciones;
            this.gvDatos.DataBind();
        }

        private void CargarLista(HTLHoteles pHoteles)
        {
            HTLHabitaciones habitacion = new HTLHabitaciones();
            habitacion.IdHotel = pHoteles.IdHotel;
            habitacion.NumeroHabitacion = "";
            habitacion.NombreHabitacion = "";
            habitacion.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            habitacion.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor(habitacion);
            this.MisHabitaciones = HotelesF.HabitacionesObtenerListaGrilla(habitacion);
            this.gvDatos.DataSource = this.MisHabitaciones;
            this.gvDatos.PageIndex = habitacion.IndiceColeccion;
            this.gvDatos.DataBind();
        }

    }
}