using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Afiliados;

namespace IU.Modulos.Afiliados.Controles
{
    public partial class CategoriasModificarDatos : ControlesSeguros
    {
        private AfiCategorias MiAfiCategorias
        {
            get { return (AfiCategorias)Session[this.MiSessionPagina + "AfiliadoMiAfiCategorias"]; }
            set { Session[this.MiSessionPagina + "AfiliadoMiAfiCategorias"] = value; }
        }

        public delegate void CategoriaAfiliadoDatosAceptarEventHandler(object sender, AfiCategorias e);
        public event CategoriaAfiliadoDatosAceptarEventHandler CategoriaAfiliadoModificarDatosAceptar;
        public delegate void CategoriaAfiliadoDatosCancelarEventHandler();
        public event CategoriaAfiliadoDatosCancelarEventHandler CategoriaAfiliadoModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (this.IsPostBack)
            {
                if (this.MiAfiCategorias == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }
        public void IniciarControl(AfiCategorias pCategorias, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiAfiCategorias = pCategorias;
                    this.ctrComentarios.IniciarControl(MiAfiCategorias, this.GestionControl);
                    this.ctrArchivos.IniciarControl(MiAfiCategorias, this.GestionControl);
                    this.ctrCamposValores.IniciarControl(this.MiAfiCategorias, new Objeto(), this.GestionControl);
                    break;
                case Gestion.Modificar:
                    this.MiAfiCategorias = AfiliadosF.CategoriaObtenerDatosCompletos(pCategorias);
                    this.MapearObjetoAControles(this.MiAfiCategorias);
                    break;
                case Gestion.Consultar:
                    this.MiAfiCategorias = AfiliadosF.CategoriaObtenerDatosCompletos(pCategorias);
                    this.MapearObjetoAControles(this.MiAfiCategorias);
                    this.txtCategoria.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.ddlTipoCategoria.Enabled = false;
                    this.txtCodigo.Enabled = false;
                    this.txtNumeroSocioDesde.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        private void MapearObjetoAControles(AfiCategorias pCategorias)
        {
            this.txtCategoria.Text = pCategorias.Categoria;
            this.txtCodigo.Text = pCategorias.Codigo;
            this.txtNumeroSocioDesde.Text = pCategorias.NumeroSocioDesde;
            this.ddlEstado.SelectedValue = pCategorias.Estado.IdEstado.ToString();
            this.ddlTipoCategoria.SelectedValue = pCategorias.IdTipoCategoria.ToString();

            this.ctrComentarios.IniciarControl(pCategorias, this.GestionControl);
            this.ctrArchivos.IniciarControl(pCategorias, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pCategorias);
            this.ctrCamposValores.IniciarControl(pCategorias, new Objeto(), this.GestionControl);
        }
        private void MapearControlesAObjeto(AfiCategorias pCategorias)
        {
            pCategorias.Categoria = this.txtCategoria.Text;
            pCategorias.Codigo = this.txtCodigo.Text;
            pCategorias.NumeroSocioDesde = this.txtNumeroSocioDesde.Text;
            pCategorias.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pCategorias.IdTipoCategoria = this.ddlTipoCategoria.SelectedValue == string.Empty ? (Nullable<int>)null : Convert.ToInt32(this.ddlTipoCategoria.SelectedValue);
            pCategorias.Comentarios = ctrComentarios.ObtenerLista();
            pCategorias.Archivos = ctrArchivos.ObtenerLista();
            pCategorias.Campos = ctrCamposValores.ObtenerLista();
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();

            this.ddlTipoCategoria.DataSource = AfiliadosF.CategoriasObtenerTiposCategoria();
            this.ddlTipoCategoria.DataValueField = "IdTipoCategoria";
            this.ddlTipoCategoria.DataTextField = "TipoCategoria";
            this.ddlTipoCategoria.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlTipoCategoria, ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiAfiCategorias);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = AfiliadosF.CategoriasAgregar(this.MiAfiCategorias);
                    break;
                case Gestion.Modificar:
                    guardo = AfiliadosF.CategoriasModificar(this.MiAfiCategorias);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAfiCategorias.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiAfiCategorias.CodigoMensaje, true, this.MiAfiCategorias.CodigoMensajeArgs);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.CategoriaAfiliadoModificarDatosCancelar?.Invoke();
        }
        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            this.CategoriaAfiliadoModificarDatosAceptar?.Invoke(null, this.MiAfiCategorias);
        }
    }
}