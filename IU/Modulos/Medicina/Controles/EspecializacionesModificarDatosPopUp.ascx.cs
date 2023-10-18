using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Medicina.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;

namespace IU.Modulos.Medicina.Controles
{
    public partial class EspecializacionesModificarDatosPopUp : ControlesSeguros
    {
        private MedPrestadoresEspecializaciones MiEspecializacion
        {
            get { return (MedPrestadoresEspecializaciones)Session[this.MiSessionPagina + "EspecializacionesModificarDatosPopUpMiEspecializacion"]; }
            set { Session[this.MiSessionPagina + "EspecializacionesModificarDatosPopUpMiEspecializacion"] = value; }
        }

        public delegate void EspecializacionesModificarDatosEventHandler(MedPrestadoresEspecializaciones e, Gestion pGestion);
        public event EspecializacionesModificarDatosEventHandler EspecializacionesModificarDatosAceptar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
            }
        }

        public void IniciarControl(MedPrestadoresEspecializaciones pParametro, List<MedEspecializaciones> pLista, Gestion pGestion)
        {
            this.CargarCombos(pLista);
            this.MiEspecializacion = pParametro;
            this.GestionControl = pGestion;
            this.ddlEstados.Enabled = true;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //this.txtFe.Text = DateTime.Now.ToShortDateString();
                    pParametro.Estado.IdEstado = (int)Estados.Activo;
                    this.ddlEstados.Enabled = false;
                    this.ctrCamposValores.BorrarControlesParametros();
                    this.ctrCamposValores.IniciarControl(this.MiEspecializacion, new Objeto(), this.GestionControl, null, MiEspecializacion.Campos);
                    break;
                case Gestion.Modificar:
                    this.MapearObjetoAControles(this.MiEspecializacion);
                    this.btnAceptar.Visible = true;
                    break;
                case Gestion.Consultar:
                    this.MapearObjetoAControles(this.MiEspecializacion);
                    ddlEstados.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarEspecializacion();", true);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("EspecializacionesModificarDatos");
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiEspecializacion);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiEspecializacion.Estado.IdEstado = (int)Estados.Activo;
                    this.MiEspecializacion.EstadoColeccion = EstadoColecciones.Agregado;
                    break;
                case Gestion.Modificar:
                    this.MiEspecializacion.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiEspecializacion, this.GestionControl);
                    break;
                default:
                    break;
            }

            if (this.EspecializacionesModificarDatosAceptar != null)
                this.EspecializacionesModificarDatosAceptar(this.MiEspecializacion, this.GestionControl);
            
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalBuscarEspecializacion();", true);
        }

        private void MapearControlesAObjeto(MedPrestadoresEspecializaciones pParametro)
        {
            pParametro.Especializacion.IdEspecializacion = this.ddlEspecialidad.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlEspecialidad.SelectedValue);
            pParametro.Especializacion.Descripcion = this.ddlEspecialidad.SelectedItem.Text;
            pParametro.EspecializacionPorDefecto  = this.chkPredeterminado.Checked;
            pParametro.Estado.IdEstado = this.ddlEstados.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlEstados.SelectedValue);

            pParametro.Campos = this.ctrCamposValores.ObtenerLista();
        }

        private void MapearObjetoAControles(MedPrestadoresEspecializaciones pParametro)
        {
            ListItem item = this.ddlEspecialidad.Items.FindByValue(pParametro.Especializacion.IdEspecializacion.ToString());
            if (item == null)
                this.ddlEspecialidad.Items.Add(new ListItem(pParametro.Especializacion.Descripcion, pParametro.Especializacion.IdEspecializacion.ToString()));

            this.ddlEspecialidad.SelectedValue = pParametro.Especializacion.IdEspecializacion.ToString();
            this.chkPredeterminado.Checked = pParametro.EspecializacionPorDefecto;
            this.ddlEstados.SelectedValue = pParametro.Estado.IdEstado.ToString();

            this.ctrCamposValores.BorrarControlesParametros();
            this.ctrCamposValores.IniciarControl(this.MiEspecializacion, new Objeto(), this.GestionControl, null, MiEspecializacion.Campos);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalBuscarEspecializacion();", true);
        }

        private void CargarCombos(List<MedEspecializaciones> pEspecializaciones)
        {
            List<TGEListasValoresDetalles> lista = TGEGeneralesF.ListasValoresObtenerListaDetalle(Generales.Entidades.EnumTGEListasValoresCodigos.PrestadoresEspecializaciones);
            lista = lista.Where(x => !pEspecializaciones.Any(x2 => x.IdListaValorDetalle == x2.IdEspecializacion)).ToList();
            this.ddlEspecialidad.DataSource = lista;
            this.ddlEspecialidad.DataValueField = "IdListaValorDetalle";
            this.ddlEspecialidad.DataTextField = "Descripcion";
            this.ddlEspecialidad.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEspecialidad, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataBind();
            this.ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();
        }
        
    }
}