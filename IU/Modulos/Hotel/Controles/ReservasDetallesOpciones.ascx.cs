using Comunes.Entidades;
using Hoteles.Entidades;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Hotel.Controles
{
    public partial class ReservasDetallesOpciones : ControlesSeguros
    {

        public HTLReservasDetalles MiReserva
        {
            get { return this.PropiedadObtenerValor<HTLReservasDetalles>("ReservasDetallesOpcionesMiReserva"); }
            set { this.PropiedadGuardarValor("ReservasDetallesOpcionesMiReserva", value); }
        }

        public delegate void ReservasDetallesOpcioneAceptarEventHandler(HTLReservasDetalles e);
        public event ReservasDetallesOpcioneAceptarEventHandler ReservasDetallesOpcioneAceptar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.CargarCombos();
            }
        }

        public void IniciarControl(HTLReservasDetalles pParametro, Gestion pGestion)
        {
            MiReserva = pParametro;
            this.CargarCombos();

            MapearObjetoAControles(pParametro);
            txtCantidad.Attributes.Add("OnChange", "CalcularPersonas();");
            //btnAceptar.Attributes.Add("OnClick", "CalcularPersonas();");
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModalReservasDetallesOpciones", "ShowModalPopUpReservasDetallesOpciones();", true);
            UpdatePanel1.Update();

            //ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "ObtenerOpcionesMoviliario", "ObtenerOpcionesMoviliario();", true);
        }
       
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(txtCantidad.Text) > Convert.ToInt32(hdfCantidadPersonas.Value))
            {
                var mensaje = "La Cantidad De Personas No Puede Ser Mayor a " + hdfCantidadPersonas.Value;
                MostrarMensaje(mensaje, false);
                return;
            }

            MapearControlesAObjeto(MiReserva);
            if (ReservasDetallesOpcioneAceptar != null)
                ReservasDetallesOpcioneAceptar(MiReserva);

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModalReservasDetallesOpciones", "HideModalPopUpReservasDetallesOpciones();", true);
        }

        private void MapearControlesAObjeto(HTLReservasDetalles pParametro)
        {
            pParametro.CantidadPersonasOpciones = txtCantidad.Text == string.Empty ? default(int?) : Convert.ToInt32(txtCantidad.Text);
            pParametro.LateCheckOut = chkChekOut.Checked ;
            pParametro.HabitacionDetalle.IdHabitacionDetalle = ddlMoviliario.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlMoviliario.SelectedValue);
            pParametro.HabitacionDetalle.Descripcion = ddlMoviliario.SelectedValue == string.Empty ? string.Empty : ddlMoviliario.SelectedItem.Text;
            pParametro.Compartida = pParametro.HabitacionDetalle.IdHabitacion > 0;
        }

        private void MapearObjetoAControles(HTLReservasDetalles pParametro)
        {
            hdfCantidadPersonas.Value = pParametro.CantidadPersonas.ToString();
            txtCantidadMaxima.Text = string.Concat("Máximo: " + pParametro.CantidadPersonas.ToString());
            if (string.IsNullOrEmpty(pParametro.CantidadPersonasOpciones.ToString()))
            txtCantidad.Text =  "1";
            else
            txtCantidad.Text =  pParametro.CantidadPersonasOpciones.ToString();
            chkChekOut.Checked = pParametro.LateCheckOut.HasValue ?  pParametro.LateCheckOut.Value : false;            
            this.ddlMoviliario.DataSource = BaseDatos.ObtenerBaseDatos().ObtenerLista<Select2DTO>("HTLReservasSeleccionarAjaxComboDetalleMoviliario", pParametro);
            this.ddlMoviliario.DataValueField = "id";
            this.ddlMoviliario.DataTextField = "text";
            this.ddlMoviliario.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlMoviliario, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            if (pParametro.HabitacionDetalle.IdHabitacionDetalle > 0)
                ddlMoviliario.SelectedValue = pParametro.HabitacionDetalle.IdHabitacionDetalle.ToString();
          
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModalReservasDetallesOpciones", "HideModalPopUpReservasDetallesOpciones();", true);
        }

        private void CargarCombos()
        {
            

        }
    }
}
