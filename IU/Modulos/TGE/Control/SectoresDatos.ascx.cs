using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Generales.FachadaNegocio;

namespace IU.Modulos.TGE.Control
{
    public partial class SectoresDatos : ControlesSeguros
    {
        TGESectores MiSector
        {
            get { return (TGESectores)Session[this.MiSessionPagina + "SectoresDatosMiSector"]; }
            set { Session[this.MiSessionPagina + "SectoresDatosMiSector"] = value; }
        }

        public delegate void SectoresDatosAceptarEventHandler(object sender, TGESectores e);
        public event SectoresDatosAceptarEventHandler SectorModificarDatosAceptar;

        public delegate void SectoresDatosCancelarEventHandler();
        public event SectoresDatosCancelarEventHandler SectorModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (this.IsPostBack)
            {
                if (this.MiSector == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una operacion
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(TGESectores pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.MiSector = pParametro;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //this.ctrComentarios.IniciarControl(this.MiSector, this.GestionControl);
                    break;
                case Gestion.Modificar:
                    this.MiSector = TGEGeneralesF.SectoresObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiSector);
                    break;
                case Gestion.Consultar:
                    this.MiSector = TGEGeneralesF.SectoresObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiSector);
                    this.txtSector.Enabled = false;
                    this.ddlFiliales.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            this.ddlFiliales.DataSource = TGEGeneralesF.FilialesObenerLista();
            this.ddlFiliales.DataValueField = "IdFilial";
            this.ddlFiliales.DataTextField = "Filial";
            this.ddlFiliales.DataBind();
            if (this.ddlFiliales.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlFiliales, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void MapearObjetoAControles(TGESectores pParametro)
        {
            this.txtSector.Text = pParametro.Sector;
            this.ddlFiliales.SelectedValue = pParametro.Filial.IdFilial.ToString();

            //this.ctrComentarios.IniciarControl(pParametro, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pParametro);
        }

        private void MapearControlesAObjeto(TGESectores pParametro)
        {
            pParametro.Sector = this.txtSector.Text;
            pParametro.Filial.IdFilial = this.ddlFiliales.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFiliales.SelectedValue);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiSector);
            this.MiSector.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //this.MiFilial.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    guardo = TGEGeneralesF.SectoresAgregar(this.MiSector);
                    break;
                case Gestion.Modificar:
                    guardo = TGEGeneralesF.SectoresModificar(this.MiSector);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiSector.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiSector.CodigoMensaje, true, this.MiSector.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.SectorModificarDatosCancelar != null)
                this.SectorModificarDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.SectorModificarDatosAceptar != null)
                this.SectorModificarDatosAceptar(null, this.MiSector);
        }
    }
}