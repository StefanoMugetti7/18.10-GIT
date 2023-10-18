using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Mailing.Entidades;
using Mailing;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace IU.Modulos.TGE.Control
{
    public partial class FilialesDatos : ControlesSeguros
    {
        TGEFiliales MiFilial
        {
            get { return (TGEFiliales)Session[this.MiSessionPagina + "FilialesDatosMiFilial"]; }
            set { Session[this.MiSessionPagina + "FilialesDatosMiFilial"] = value; }
        }

        public delegate void FilialesDatosAceptarEventHandler(object sender, TGEFiliales e);
        public event FilialesDatosAceptarEventHandler FilialModificarDatosAceptar;

        public delegate void FilialesDatosCancelarEventHandler();
        public event FilialesDatosCancelarEventHandler FilialModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (this.IsPostBack)
            {
                if (this.MiFilial == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una operacion
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(TGEFiliales pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiFilial = pParametro;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //this.ctrComentarios.IniciarControl(this.MiFilial, this.GestionControl);
                    this.ctrCamposValores.IniciarControl(pParametro, new Objeto(), this.GestionControl);
                    break;
                case Gestion.Modificar:
                    this.MiFilial = TGEGeneralesF.FilialesObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiFilial);
                    break;
                case Gestion.Consultar:
                    this.MiFilial = TGEGeneralesF.FilialesObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiFilial);
                    this.txtFilial.Enabled = false;
                    this.txtDescripcion.Enabled = false;
                    this.txtDireccion.Enabled = false;
                    this.txtCodigoFilial.Enabled = false;
                    //this.txtAfipPuntoVenta.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.ddlEstiloPlantilla.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        private void MapearObjetoAControles(TGEFiliales pParametro)
        {
            this.txtFilial.Text = pParametro.Filial;
            this.txtDescripcion.Text = pParametro.Descripcion;
            this.txtDireccion.Text = pParametro.Direccion;
            this.txtCodigoFilial.Text = pParametro.CodigoFilial;

            if (!string.IsNullOrEmpty(pParametro.EstiloPlantillaSucursal))
            {
                ListItem item = this.ddlEstiloPlantilla.Items.FindByText(pParametro.EstiloPlantillaSucursal.ToString());
                if (item == null)
                    this.ddlEstiloPlantilla.Items.Add(new ListItem(pParametro.EstiloPlantillaSucursal, "0"));
                this.ddlEstiloPlantilla.SelectedValue = item.Value;
            }
            //this.txtAfipPuntoVenta.Text = pParametro.AfipPuntoVenta.ToString();
            // this.ctrComentarios.IniciarControl(pParametro, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pParametro);
            this.ctrCamposValores.IniciarControl(pParametro, new Objeto(), this.GestionControl);
        }
        private void MapearControlesAObjeto(TGEFiliales pParametro)
        {
            pParametro.Filial = this.txtFilial.Text;
            pParametro.Descripcion = this.txtDescripcion.Text;
            pParametro.Direccion = this.txtDireccion.Text;
            pParametro.CodigoFilial = this.txtCodigoFilial.Text;
            pParametro.Campos = this.ctrCamposValores.ObtenerLista();
            pParametro.EstiloPlantillaSucursal = this.ddlEstiloPlantilla.SelectedValue == string.Empty ? null : this.ddlEstiloPlantilla.SelectedItem.Text;
            //pParametro.AfipPuntoVenta = this.txtAfipPuntoVenta.Text == string.Empty ? 0 : Convert.ToInt32(this.txtAfipPuntoVenta.Text);
        }
        private void CargarCombos()
        {
            this.ddlEstiloPlantilla.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.PlantillaEstiloSucursal);
            this.ddlEstiloPlantilla.DataValueField = "IdListaValorDetalle";
            this.ddlEstiloPlantilla.DataTextField = "Descripcion";
            this.ddlEstiloPlantilla.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlEstiloPlantilla, ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiFilial);
            this.MiFilial.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //this.MiFilial.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    guardo = TGEGeneralesF.FilialesAgregar(this.MiFilial);
                    break;
                case Gestion.Modificar:
                    guardo = TGEGeneralesF.FilialesModificar(this.MiFilial);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiFilial.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiFilial.CodigoMensaje, true, this.MiFilial.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.FilialModificarDatosCancelar != null)
                this.FilialModificarDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.FilialModificarDatosAceptar != null)
                this.FilialModificarDatosAceptar(null, this.MiFilial);
        }
    }
}