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
using Proveedores.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;

namespace IU.Modulos.Proveedores.Controles
{
    public partial class ProveedorModificarDatosTelefonoPopUp : ControlesSeguros
    {
        private CapProveedoresTelefonos MiTelefono
        {
            get { return (CapProveedoresTelefonos)Session[this.MiSessionPagina + "ProveedorModificarDatosTelefonoMiTelefono"]; }
            set { Session[this.MiSessionPagina + "ProveedorModificarDatosTelefonoMiTelefono"] = value; }
        }

        public delegate void ProveedorModificarDatosTelefonoEventHandler(object sender, CapProveedoresTelefonos e, Gestion pGestion);
        public event ProveedorModificarDatosTelefonoEventHandler ProveedoresModificarDatosAceptar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void IniciarControl(CapProveedoresTelefonos pTelefono, Gestion pGestion)
        {
            this.CargarCombos();
            this.MiTelefono = pTelefono;
            this.GestionControl = pGestion;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    pTelefono.Estado.IdEstado = (int)Estados.Activo;
                    break;
                case Gestion.Modificar:
                    this.MapearObjetoAControles(this.MiTelefono);
                    break;
                case Gestion.Consultar:
                    this.MapearObjetoAControles(this.MiTelefono);
                    this.btnAceptar.Visible = false;
                    this.ddlTipoTelefono.Enabled = false;
                    this.txtPrefijo.Enabled = false;
                    this.txtNumero.Enabled = false;
                    this.txtInterno.Enabled = false;
                    break;
                default:
                    break;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarTurnos();", true);
        }

        private void CargarCombos()
        {
            this.ddlTipoTelefono.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposTelefonos);
            this.ddlTipoTelefono.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTipoTelefono.DataTextField = "Descripcion";
            this.ddlTipoTelefono.DataBind();
        }

        private void MapearControlesAObjeto(CapProveedoresTelefonos pTelefonos)
        {
            pTelefonos.TipoTelefono.IdTipoTelefono = Convert.ToInt32(this.ddlTipoTelefono.SelectedValue);
            pTelefonos.TipoTelefono.Descripcion = this.ddlTipoTelefono.SelectedItem.Text;
            pTelefonos.Prefijo = this.txtPrefijo.Text == string.Empty ? 0 : Convert.ToInt32(this.txtPrefijo.Text);
            pTelefonos.Numero = Convert.ToInt32(this.txtNumero.Text);
            pTelefonos.Interno = this.txtInterno.Text == string.Empty ? 0 : Convert.ToInt32(this.txtInterno.Text);
        }

        private void MapearObjetoAControles(CapProveedoresTelefonos pTelefonos)
        {
            this.ddlTipoTelefono.SelectedValue = pTelefonos.TipoTelefono.IdTipoTelefono.ToString();
            this.txtPrefijo.Text = pTelefonos.Prefijo.ToString();
            this.txtNumero.Text = pTelefonos.Numero.ToString();
            this.txtInterno.Text = pTelefonos.Interno.ToString();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("ProveedoresDatosTelefonos");
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiTelefono);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiTelefono.Estado.IdEstado = (int)Estados.Activo;
                    this.MiTelefono.EstadoColeccion = EstadoColecciones.Agregado;
                    break;
                case Gestion.Modificar:
                    this.MiTelefono.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiTelefono, this.GestionControl);
                    break;
                default:
                    break;
            }

            if (this.ProveedoresModificarDatosAceptar != null)
                this.ProveedoresModificarDatosAceptar(sender, this.MiTelefono, this.GestionControl);
       
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalBuscarTurnos();", true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalBuscarTurnos();", true);
        }
    }
}