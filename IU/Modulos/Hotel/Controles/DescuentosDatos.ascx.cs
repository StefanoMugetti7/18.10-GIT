using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Hoteles;
using Hoteles.Entidades;
using System;
using System.Collections.Generic;

namespace IU.Modulos.Hotel.Controles
{
    public partial class DescuentosDatos : ControlesSeguros
    {
        public HTLDescuentos MiDescuento
        {
            get { return this.PropiedadObtenerValor<HTLDescuentos>("DescuentosDatosMiDescuento"); }
            set { this.PropiedadGuardarValor("DescuentosDatosMiDescuento", value); }
        }
        public List<TGEListasValoresDetalles> MisMoviliarios
        {
            get { return this.PropiedadObtenerValor<List<TGEListasValoresDetalles>>("DescuentosDatosMisMoviliarios"); }
            set { this.PropiedadGuardarValor("DescuentosDatosMisMoviliarios", value); }
        }
        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }
        public void IniciarControl(HTLDescuentos pParametro, Gestion pGestion)
        {
            this.MiDescuento = pParametro;
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.ctrCamposValores.IniciarControl(this.MiDescuento, new Objeto(), this.GestionControl);
                    ddlEstados.Enabled = false;
                    break;
                case Gestion.Anular:
                    break;
                case Gestion.Modificar:
                    this.MiDescuento = HotelesF.DescuentosObtenerDatosCompletos(this.MiDescuento);
                    this.MapearObjetoAControles(this.MiDescuento);
                    break;
                case Gestion.Consultar:
                    this.MiDescuento = HotelesF.DescuentosObtenerDatosCompletos(this.MiDescuento);
                    this.MapearObjetoAControles(this.MiDescuento);
                    this.ddlHoteles.Enabled = false;
                    this.ddlTiposDescuentos.Enabled = false;
                    this.txtImporte.Enabled = false;
                    this.txtPorcentaje.Enabled = false;
                    this.ddlEstados.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.MisMoviliarios = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.HotelMoviliario);

            HTLHoteles hotel = new HTLHoteles
            {
                UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo)
            };
            this.ddlHoteles.DataSource = HotelesF.HotelesObtenerListaActiva(hotel);
            this.ddlHoteles.DataValueField = "IdHotel";
            this.ddlHoteles.DataTextField = "Descripcion";
            this.ddlHoteles.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlHoteles, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosDescuentos));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();

            this.ddlTiposDescuentos.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.HotelTiposDescuentos);
            this.ddlTiposDescuentos.DataValueField = "IdListaValorDetalle";
            this.ddlTiposDescuentos.DataTextField = "Descripcion";
            this.ddlTiposDescuentos.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposDescuentos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void MapearObjetoAControles(HTLDescuentos pDescuento)
        {
            this.ddlEstados.SelectedValue = pDescuento.Estado.IdEstado.ToString();
            this.ddlHoteles.SelectedValue = pDescuento.IdHotel.ToString();
            this.ddlTiposDescuentos.SelectedValue = pDescuento.IdTipoDescuento.ToString();
            this.txtImporte.Text = pDescuento.DescuentoImporte.ToString();
            this.txtPorcentaje.Text = pDescuento.DescuentoPorcentaje.ToString();
        }
        private void MapearControlesAObjeto(HTLDescuentos pDescuento)
        {
            pDescuento.IdHotel = Convert.ToInt32(this.ddlHoteles.SelectedValue);
            pDescuento.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pDescuento.IdTipoDescuento = Convert.ToInt32(this.ddlTiposDescuentos.SelectedValue);
            pDescuento.DescuentoPorcentaje = Convert.ToDecimal(this.txtPorcentaje.Text);
            pDescuento.DescuentoImporte = Convert.ToDecimal(this.txtImporte.Text);
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptar.Visible = false;
            this.MapearControlesAObjeto(this.MiDescuento);
            this.MiDescuento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = HotelesF.DescuentosAgregar(this.MiDescuento);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiDescuento.CodigoMensaje, false);
                    }
                    break;
                case Gestion.Anular:
                    this.MiDescuento.Estado.IdEstado = (int)Estados.Baja;
                    guardo = HotelesF.DescuentosModificar(this.MiDescuento);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiDescuento.CodigoMensaje, false);
                    }
                    break;
                case Gestion.Modificar:
                    guardo = HotelesF.DescuentosModificar(this.MiDescuento);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiDescuento.CodigoMensaje, false);
                    }
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiDescuento.CodigoMensaje, true, this.MiDescuento.CodigoMensajeArgs);
                if (this.MiDescuento.dsResultado != null)
                {
                    this.MiDescuento.dsResultado = null;
                }
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.ControlModificarDatosCancelar?.Invoke();
        }
    }
}