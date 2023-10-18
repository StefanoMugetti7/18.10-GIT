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
using Afiliados;
using Afiliados.LogicaNegocio;
using Generales.FachadaNegocio;
using EO.Web.Internal;

namespace IU.Modulos.Afiliados.Controles
{
    public partial class AfiliadoModificarDatosTelefonoPopUp : ControlesSeguros
    {
        private AfiTelefonos MiTelefono
        {
            get { return (AfiTelefonos)Session[this.MiSessionPagina + "AfiliadoModificarDatosTelefonoMiTelefono"]; }
            set { Session[this.MiSessionPagina + "AfiliadoModificarDatosTelefonoMiTelefono"] = value; }
        }
        public delegate void AfiliadoModificarDatosTelefonoEventHandler(object sender, AfiTelefonos e, Gestion pGestion);
        public event AfiliadoModificarDatosTelefonoEventHandler AfiliadosModificarDatosAceptar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }
        public void IniciarControl(AfiTelefonos pTelefono, Gestion pGestion)
        {
            AyudaProgramacion.LimpiarControles(this, true);
            this.CargarCombos();
            this.MiTelefono = pTelefono;
            this.GestionControl = pGestion;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    pTelefono.Estado.IdEstado = (int)Estados.Activo;
                    this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
                    this.lblEmpresaTelefonica.Visible = false;
                    this.ddlEmpresaTelefonica.Visible = false;
                    break;
                case Gestion.Modificar:
                    this.MapearObjetoAControles(this.MiTelefono);
                    if (pTelefono.EmpresaTelefonica.IdEmpresaTelefonica == null)
                    {
                        this.lblEmpresaTelefonica.Visible = false;
                        this.ddlEmpresaTelefonica.Visible = false;
                    }
                    break;
                case Gestion.Consultar:
                    this.MapearObjetoAControles(this.MiTelefono);
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModal", "ShowModalTelefonos();", true);
        }
        private void CargarCombos()
        {
            this.ddlTipoTelefono.DataSource = AfiliadosF.TelefonosTiposObtenerLista();
            this.ddlTipoTelefono.DataValueField = "IdTelefonoTipo";
            this.ddlTipoTelefono.DataTextField = "Descripcion";
            this.ddlTipoTelefono.DataBind();

            this.ddlEmpresaTelefonica.DataSource = AfiliadosF.EmpresasTelefonicasObtenerLista();
            this.ddlEmpresaTelefonica.DataValueField = "IdEmpresaTelefonica";
            this.ddlEmpresaTelefonica.DataTextField = "Descripcion";
            this.ddlEmpresaTelefonica.DataBind();

            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
        }

        private void MapearControlesAObjeto(AfiTelefonos pTelefonos)
        {
            pTelefonos.TelefonoTipo.IdTelefonoTipo = Convert.ToInt32(this.ddlTipoTelefono.SelectedValue);
            pTelefonos.TelefonoTipo.Descripcion = this.ddlTipoTelefono.SelectedItem.Text;

            pTelefonos.EmpresaTelefonica.IdEmpresaTelefonica = this.ddlEmpresaTelefonica.SelectedValue == string.Empty ? default(int) : Convert.ToInt32(this.ddlEmpresaTelefonica.SelectedValue);
            //pTelefonos.EmpresaTelefonica.IdEmpresaTelefonica = this.ddlEmpresaTelefonica.Visible == false ? (Nullable<int>)null : Convert.ToInt32(this.ddlEmpresaTelefonica.SelectedValue);
            pTelefonos.EmpresaTelefonica.Descripcion = this.ddlEmpresaTelefonica.SelectedValue == string.Empty ? string.Empty : this.ddlEmpresaTelefonica.SelectedItem.Text;
            
            //pTelefonos.Prefijo = this.txtPrefijo.Text == string.Empty ? 0 : Convert.ToInt32(this.txtPrefijo.Text);
            pTelefonos.Numero = Convert.ToInt64(this.txtNumero.Text);
            pTelefonos.Interno = this.txtInterno.Text == string.Empty ? 0 : Convert.ToInt32(this.txtInterno.Text);
            pTelefonos.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModal", "ShowModalTelefonos();", true);
        }

        private void MapearObjetoAControles(AfiTelefonos pTelefonos)
        {
            this.ddlTipoTelefono.SelectedValue = pTelefonos.TelefonoTipo.IdTelefonoTipo.ToString();
            if (pTelefonos.EmpresaTelefonica.IdEmpresaTelefonica.HasValue)
            {
                this.ddlEmpresaTelefonica.Visible = true;
                this.lblEmpresaTelefonica.Visible = true;
                this.ddlEmpresaTelefonica.SelectedValue = pTelefonos.EmpresaTelefonica.IdEmpresaTelefonica.HasValue ?  pTelefonos.EmpresaTelefonica.IdEmpresaTelefonica.ToString() : string.Empty;
            }
            else
            {
                this.ddlEmpresaTelefonica.Visible = false;
                this.lblEmpresaTelefonica.Visible = false;
            }
            //this.txtPrefijo.Text = pTelefonos.Prefijo.ToString();
            this.txtNumero.Text = pTelefonos.Numero.ToString();
            this.txtInterno.Text = pTelefonos.Interno.ToString();
            this.ddlEstado.SelectedValue = pTelefonos.Estado.IdEstado.ToString();
        } 

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("AfiliadosDatosTelefonos");
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

            if (this.AfiliadosModificarDatosAceptar != null)
                this.AfiliadosModificarDatosAceptar(sender, this.MiTelefono, this.GestionControl);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModalTelefonos", "HideModaTelefonos();", true);
        }

        protected void ddlTipoTelefono_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlTipoTelefono.SelectedValue))
            {
                if (ddlTipoTelefono.SelectedValue == ((int)EnumTiposTelefonos.Celular).ToString())
                {
                    this.ddlEmpresaTelefonica.Visible = true;
                    this.lblEmpresaTelefonica.Visible = true;
                }
                else
                {
                    this.ddlEmpresaTelefonica.Visible = false;
                    this.lblEmpresaTelefonica.Visible = false;
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModalTelefonos", "HideModaTelefonos();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModal", "ShowModalTelefonos();", true);
        }
    }
}