using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Contabilidad;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class SubRubrosDatos : ControlesSeguros
    {
        private CtbSubRubros MiSubRubro
        {
            get { return (CtbSubRubros)Session[this.MiSessionPagina + "MiSubRubro"]; }
            set { Session[this.MiSessionPagina + "MiSubRubro"] = value; }
        }

        public delegate void SubRubroDatosAceptarEventHandler(object sender, CtbSubRubros e);
        public event SubRubroDatosAceptarEventHandler SubRubroDatosAceptar;

        public delegate void SubRubroDatosCancelarEventHandler();
        public event SubRubroDatosCancelarEventHandler SubRubroDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(this.popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
                if (this.MiSubRubro == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }
        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Solicitud Material
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(CtbSubRubros pSubRubro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiSubRubro = pSubRubro;
                    this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
                    break;
                case Gestion.Modificar:
                    this.MiSubRubro = ContabilidadF.SubRubrosObtenerDatosCompletos(pSubRubro);
                    this.MapearObjetoAControles(this.MiSubRubro);
                    break;
                case Gestion.Consultar:
                    this.MiSubRubro = ContabilidadF.SubRubrosObtenerDatosCompletos(pSubRubro);
                    this.MapearObjetoAControles(this.MiSubRubro);
                    this.txtSubRubro.Enabled = false;
                    this.txtCodigoSubRubro.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }
        private void MapearObjetoAControles(CtbSubRubros pSubRubro)
        {
            this.txtSubRubro.Text = pSubRubro.SubRubro;
            this.txtCodigoSubRubro.Text = pSubRubro.CodigoSubRubro;
            this.ddlEstado.SelectedValue = pSubRubro.Estado.IdEstado.ToString();
        }
        private void MapearControlesAObjeto(CtbSubRubros pSubRubro)
        {
            pSubRubro.SubRubro = this.txtSubRubro.Text;
            pSubRubro.CodigoSubRubro = this.txtCodigoSubRubro.Text;
            pSubRubro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiSubRubro);
            this.MiSubRubro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = ContabilidadF.SubRubrosAgregar(this.MiSubRubro);
                    break;
                case Gestion.Modificar:
                    guardo = ContabilidadF.SubRubrosModificar(this.MiSubRubro);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiSubRubro.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiSubRubro.CodigoMensaje, true, this.MiSubRubro.CodigoMensajeArgs);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.SubRubroDatosCancelar != null)
                this.SubRubroDatosCancelar();
        }
        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.SubRubroDatosAceptar != null)
                this.SubRubroDatosAceptar(null, this.MiSubRubro);
        }
    }
}