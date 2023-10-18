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
    public partial class CementeriosDatos : ControlesSeguros
    {
        public NCHCementerios MiCementerio
        {
            get { return PropiedadObtenerValor<NCHCementerios>("CementerioDatosMiCementerio"); }
            set { PropiedadGuardarValor("CementerioDatosMiCementerio", value); }
        }
        private DataTable MisPanteones ////IMPLEMENTAR
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
                if (MiCementerio == null && GestionControl != Gestion.Agregar)
                {
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }

        public void IniciarControl(NCHCementerios pParametro, Gestion pGestion)
        {
            MiCementerio = pParametro;
            GestionControl = pGestion;
            CargarCombos();
            switch (GestionControl)
            {
                case Gestion.Agregar:
                    ddlEstado.Enabled = false;
                    ddlEstado.SelectedValue = 1.ToString();
                    break;
                case Gestion.Anular:
                    txtDescripcion.Enabled = false;
                    ddlEstado.Enabled = false;
                    ddlFilial.Enabled = false;
                    break;
                case Gestion.Modificar:
                    MiCementerio = CementeriosF.CementeriosObtenerDatosCompletos(MiCementerio);
                    this.CargarLista(MiCementerio);
                    MapearObjetoAControles(MiCementerio);
                    break;
                case Gestion.Consultar:
                    MiCementerio = CementeriosF.CementeriosObtenerDatosCompletos(MiCementerio);
                    txtDescripcion.Enabled = false;
                    ddlEstado.Enabled = false;
                    ddlFilial.Enabled = false;
                    txtCodigo.Enabled = false;
                    txtDomicilio.Enabled = false;
                    btnAceptar.Visible = false;
                    this.CargarLista(MiCementerio);
                    MapearObjetoAControles(MiCementerio);
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

            ddlFilial.DataSource = UsuarioActivo.Filiales;
            ddlFilial.DataValueField = "IdFilial";
            ddlFilial.DataTextField = "Filial";
            ddlFilial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(ddlFilial, ObtenerMensajeSistema("SeleccioneOpcion"));
            ddlFilial.SelectedValue = UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
        }

        private void MapearObjetoAControles(NCHCementerios pCementerio)
        {
            txtDescripcion.Text = pCementerio.Descripcion;
            txtCodigo.Text = pCementerio.Codigo.ToString();
            txtDomicilio.Text = pCementerio.Domicilio.ToString();

            var item = ddlEstado.Items.FindByValue(pCementerio.Estado.ToString());
            if (item == null)
                ddlEstado.Items.Add(new ListItem(pCementerio.Estado.Descripcion, pCementerio.Estado.IdEstado.ToString()));

            ddlEstado.SelectedValue = pCementerio.Estado.IdEstado.ToString();
            ListItem filial = ddlFilial.Items.FindByValue(pCementerio.Filial.IdFilial.ToString());

            if (filial == null && pCementerio.Filial.IdFilial > 0)
                ddlFilial.Items.Add(new ListItem(pCementerio.Filial.Filial, pCementerio.Filial.IdFilial.ToString()));

            ddlFilial.SelectedValue = pCementerio.Filial.IdFilial == 0 ? string.Empty : pCementerio.Filial.IdFilial.ToString();
        }
        private void MapearControlesAObjeto(NCHCementerios pCementerios)
        {
            pCementerios.Descripcion = txtDescripcion.Text;
            pCementerios.Estado.IdEstado = Convert.ToInt32(ddlEstado.SelectedValue);
            pCementerios.Filial.IdFilial = Convert.ToInt32(ddlFilial.SelectedValue);
            pCementerios.Codigo = txtCodigo.Text;
            pCementerios.Domicilio = txtDomicilio.Text;
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            btnAceptar.Visible = false;
            MapearControlesAObjeto(MiCementerio);

            MiCementerio.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            switch (GestionControl)
            {
                case Gestion.Agregar:
                    guardo = CementeriosF.CementeriosAgregar(MiCementerio);
                    if (guardo)
                        MostrarMensaje(MiCementerio.CodigoMensaje, false, MiCementerio.CodigoMensajeArgs);
                    break;
                case Gestion.Anular:
                    MiCementerio.Estado.IdEstado = (int)Estados.Baja;
                    guardo = CementeriosF.CementeriosModificar(MiCementerio);
                    if (guardo)
                        MostrarMensaje(MiCementerio.CodigoMensaje, false);
                    break;
                case Gestion.Modificar:
                    guardo = CementeriosF.CementeriosModificar(MiCementerio);
                    if (guardo)
                        MostrarMensaje(MiCementerio.CodigoMensaje, false);
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                btnAceptar.Visible = true;
                MostrarMensaje(MiCementerio.CodigoMensaje, true, MiCementerio.CodigoMensajeArgs);
                if (MiCementerio.dsResultado != null)
                    MiCementerio.dsResultado = null;

            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (ControlModificarDatosCancelar != null)
                ControlModificarDatosCancelar();
        }

        //protected void btnAgregarPanteon_Click(object sender, EventArgs e)
        //{
        //    this.MisParametrosUrl = new Hashtable();
        //    this.MisParametrosUrl.Add("IdCementerio", MiCementerio.IdCementerio);
        //    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/PanteonesAgregar.aspx"), true);
        //}



        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            this.MisParametrosUrl = new Hashtable();

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/CementeriosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/CementeriosConsultar.aspx"), true);
                
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


        private void CargarLista(NCHCementerios pCementerios)
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