using Comunes.Entidades;
using Generales.FachadaNegocio;
using Nichos;
using Nichos.Entidades;
using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;

namespace IU.Modulos.Nichos.Controles
{
    public partial class PanteonesDatos : ControlesSeguros
    {
        public NCHPanteones MiPanteon
        {
            get { return PropiedadObtenerValor<NCHPanteones>("PanteonDatosMiPanteon"); }
            set { PropiedadGuardarValor("PanteonDatosMiPanteon", value); }
        }
        private DataTable MisNichosYUrnas ////IMPLEMENTAR
        {
            get { return (DataTable)Session[this.MiSessionPagina + "PanteonesListarMisPanteones"]; }
            set { Session[this.MiSessionPagina + "PanteonesListarMisPanteones"] = value; }
        }

        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!IsPostBack)
            {
                if (MiPanteon == null && GestionControl != Gestion.Agregar)
                {
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }

        public void IniciarControl(NCHPanteones pParametro, Gestion pGestion)
        {
            MiPanteon = pParametro;
            GestionControl = pGestion;
            CargarCombos();
            switch (GestionControl)
            {
                case Gestion.Agregar:
                    ddlEstado.Enabled = false;
                    ddlEstado.SelectedValue = 1.ToString();
                    break;
                case Gestion.Anular:
                    ddlCementerio.Enabled = false;
                    ddlEstado.Enabled = false;
                    break;
                case Gestion.Modificar:
                    MiPanteon = PanteonesF.PanteonesObtenerDatosCompletos(MiPanteon);
                    this.CargarLista(MiPanteon);
                    ddlEstado.SelectedValue = 1.ToString();
                    MapearObjetoAControles(MiPanteon);
                    break;
                case Gestion.Consultar:
                    MiPanteon = PanteonesF.PanteonesObtenerDatosCompletos(MiPanteon);
                    ddlCementerio.Enabled = false;
                    ddlEstado.Enabled = false;
                    txtCodigo.Enabled = false;
                    txtDescripcion.Enabled = false;
                    MapearObjetoAControles(MiPanteon);
                    this.CargarLista(MiPanteon);
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            ddlEstado.DataValueField = "IdEstado";
            ddlEstado.DataTextField = "Descripcion";
            ddlEstado.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(ddlEstado, ObtenerMensajeSistema("SeleccioneOpcion"));
           
            ddlCementerio.DataSource = CementeriosF.CementeriosObtenerListaActiva(new NCHCementerios());
            ddlCementerio.DataValueField = "IdCementerio";
            ddlCementerio.DataTextField = "Descripcion";
            ddlCementerio.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(ddlCementerio, ObtenerMensajeSistema("SeleccioneOpcion"));
            
        }


        private void MapearObjetoAControles(NCHPanteones pParametros)
        {
            ddlCementerio.Text = pParametros.Descripcion;
            txtCodigo.Text = pParametros.Codigo.ToString();
            txtDescripcion.Text = pParametros.Descripcion.ToString();
            ListItem item = this.ddlCementerio.Items.FindByValue(pParametros.Cementerio.IdCementerio.ToString());
            ListItem item2 = this.ddlEstado.Items.FindByValue(pParametros.Estado.IdEstado.ToString());

            if (item == null)
            {
                 ddlCementerio.Items.Add(new ListItem(pParametros.Cementerio.Descripcion, pParametros.Cementerio.IdCementerio.ToString()));
            }
            if (item2 == null)
            {
                ddlEstado.Items.Add(new ListItem(pParametros.Estado.Descripcion, pParametros.Estado.IdEstado.ToString()));
            }
            ddlEstado.SelectedValue = pParametros.Estado.IdEstado.ToString();
            ddlCementerio.SelectedValue = pParametros.Cementerio.IdCementerio.ToString();
        }
        private void MapearControlesAObjeto(NCHPanteones pParametro)
        {
            pParametro.Descripcion = ddlCementerio.Text;
            pParametro.Estado.IdEstado = ddlCementerio.SelectedValue == string.Empty ? 1  : Convert.ToInt32(ddlEstado.SelectedValue);
            pParametro.Cementerio.IdCementerio = ddlCementerio.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlCementerio.SelectedValue);
            pParametro.Codigo = txtCodigo.Text;
            pParametro.Descripcion = txtDescripcion.Text;
            
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            btnAceptar.Visible = false;
            MapearControlesAObjeto(MiPanteon);

            MiPanteon.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            switch (GestionControl)
            {
                case Gestion.Agregar:
                    guardo = PanteonesF.PanteonesAgregar(MiPanteon);
                    if (guardo)
                        MostrarMensaje(MiPanteon.CodigoMensaje, false, MiPanteon.CodigoMensajeArgs);
                    break;
                case Gestion.Anular:
                    MiPanteon.Estado.IdEstado = (int)Estados.Baja;
                    guardo = PanteonesF.PanteonesModificar(MiPanteon);
                    if (guardo)
                        MostrarMensaje(MiPanteon.CodigoMensaje, false);
                    break;
                case Gestion.Modificar:
                    guardo = PanteonesF.PanteonesModificar(MiPanteon);
                    if (guardo)
                        MostrarMensaje(MiPanteon.CodigoMensaje, false);
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                btnAceptar.Visible = true;
                MostrarMensaje(MiPanteon.CodigoMensaje, true, MiPanteon.CodigoMensajeArgs);
                if (MiPanteon.dsResultado != null)
                    MiPanteon.dsResultado = null;

            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (ControlModificarDatosCancelar != null)
                ControlModificarDatosCancelar();
        }

        protected void btnAgregarPanteon_Click(object sender, EventArgs e)
        {

        }

        protected void btnAgregarHabitacion_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            //this.MisParametrosUrl.Add("IdHotel", MiCementerio.IdHotel);

            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/PanteonesAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            //HTLHabitaciones reserva = new HTLHabitaciones();
            //reserva.IdHabitacion = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdHabitacion"].ToString());

            this.MisParametrosUrl = new Hashtable();
            //this.MisParametrosUrl.Add("IdHabitacion", reserva.IdHabitacion);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/PanteonesModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/PanteonesConsultar.aspx"), true);
            }
        }

        #region gv

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
                //????
            }
        }

        //protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    //HTLHabitaciones parametros = this.BusquedaParametrosObtenerValor<HTLHabitaciones>();
        //    //parametros.IndiceColeccion = e.NewPageIndex;
        //    //this.BusquedaParametrosGuardarValor<HTLHabitaciones>(parametros);

        //    gvDatos.PageIndex = e.NewPageIndex;
        //    gvDatos.DataSource = this.MisPanteones;//resolver linea 25
        //    gvDatos.DataBind();
        //}


        private void CargarLista(NCHPanteones pPanteon)
        {
            //HTLHabitaciones habitacion = new HTLHabitaciones();
            //habitacion.IdHotel = pHoteles.IdHotel;
            //habitacion.NumeroHabitacion = "";
            //habitacion.NombreHabitacion = "";
            //habitacion.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            //habitacion.BusquedaParametros = true;
            //this.BusquedaParametrosGuardarValor(habitacion);
            //this.MisHabitaciones = HotelesF.HabitacionesObtenerListaGrilla(habitacion);
            //this.gvDatos.DataSource = this.MisHabitaciones;
            //this.gvDatos.PageIndex = habitacion.IndiceColeccion;
            //this.gvDatos.DataBind();
        }
        #endregion

    }
}