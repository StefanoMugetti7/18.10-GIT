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
using Generales.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;

namespace IU.Modulos.TGE.Control
{
    public partial class ParametrosDatosPopUp : ControlesSeguros
    {
        private TGEParametrosValores MiParametroValor
        {
            get { return (TGEParametrosValores)Session[this.MiSessionPagina + "ParametrosDatosPopUpMiParametroValor"]; }
            set { Session[this.MiSessionPagina + "ParametrosDatosPopUpMiParametroValor"] = value; }
        }

        public delegate void ParametrosDatosPopUpAceptarEventHandler(TGEParametrosValores e);
        public event ParametrosDatosPopUpAceptarEventHandler ParametrosValoresPopUpAceptar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
                this.MiParametroValor = new TGEParametrosValores();
        }

        public void IniciarControl(TGEParametrosValores pParametro)
        {
            this.CargarCombos();
            this.MiParametroValor = pParametro;
            this.txtFechaAlta.Text = DateTime.Now.ToShortDateString();
            this.txtFechaDesde.Text = DateTime.Now.ToShortDateString();
            this.txtParametroValor.Text = string.Empty;
            //this.mpePopUp.Show();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarTurnos();", true);
        }

        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados)) ;
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.MiParametroValor.FechaDesde = Convert.ToDateTime(this.txtFechaDesde.Text);
            this.MiParametroValor.FechaAlta = DateTime.Now;
            this.MiParametroValor.UsuarioAlta.IdUsuarioAlta = this.UsuarioActivo.IdUsuarioEvento;
            AyudaProgramacion.MatchObjectProperties(this.UsuarioActivo, this.MiParametroValor.UsuarioAlta);
            this.MiParametroValor.ParametroValor = this.txtParametroValor.Text;
            this.MiParametroValor.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            this.MiParametroValor.Estado.Descripcion = this.ddlEstados.SelectedItem.Text;
            this.MiParametroValor.EstadoColeccion = EstadoColecciones.Agregado;

            bool validar = false;
            string codigoMensaje = "Tipo de dato incorrecto";
            switch (this.MiParametroValor.Parametro.CampoTipo.IdCampoTipo)
            {
                case (int)EnumCamposTipos.TextBox:
                    validar = !string.IsNullOrEmpty(this.txtParametroValor.Text);
                    break;
                case (int)EnumCamposTipos.DropDownList:
                    break;
                case (int)EnumCamposTipos.IntegerTextBox:
                    int val = 0;
                    validar = int.TryParse(this.txtParametroValor.Text, out val);
                    break;
                case (int)EnumCamposTipos.CurrencyTextBox:
                case (int)EnumCamposTipos.NumericTextBox:
                case (int)EnumCamposTipos.PercentTextBox:
                    decimal valDeci = 0;
                    validar = decimal.TryParse(this.txtParametroValor.Text, out valDeci);
                    break;
                case (int)EnumCamposTipos.DateTime:
                    //Si es obligatorio
                    if (MiParametroValor.ParametroValor.Trim().Length == 0)
                    {
                        MiParametroValor.CodigoMensaje = "ValidarCampoRequerido";
                        //pObjeto.CodigoMensajeArgs.Add(campo.Titulo);
                       
                    }
                    DateTime dateValor;
                    if (MiParametroValor.ParametroValor.Trim().Length > 0 && !DateTime.TryParse(MiParametroValor.ParametroValor, out dateValor))
                    {
                        MiParametroValor.CodigoMensaje = "ValidarCampoFechaTextBoxTipoDato";
                        //pObjeto.CodigoMensajeArgs.Add(campo.Titulo);
                        MiParametroValor.CodigoMensajeArgs.Add(MiParametroValor.ParametroValor);
                      
                    }
                    break;
                case (int)EnumCamposTipos.CheckBox:
                    bool valBool = false;
                    validar = Boolean.TryParse(this.txtParametroValor.Text, out valBool);
                    break;
                default:
                    break;
            }
            if (!validar)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarTurnos();", true);
                this.MostrarMensaje(codigoMensaje, true);
                
                return;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalBuscarTurnos();", true);
            //this.mpePopUp.Hide();
            if (this.ParametrosValoresPopUpAceptar != null)
                this.ParametrosValoresPopUpAceptar(this.MiParametroValor);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalBuscarTurnos();", true);
        }


    }
}