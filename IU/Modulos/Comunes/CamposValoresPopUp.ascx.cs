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
using Comunes.Entidades;
using Generales.Entidades;
using SKP.ASP.Controls;

namespace IU.Modulos.Comunes
{
    public partial class CamposValoresPopUp : ControlesSeguros
    {
        private TGECampos MiCampo
        {
            get { return (TGECampos)Session[this.MiSessionPagina + "CamposValoresPopUpMiCampo"]; }
            set { Session[this.MiSessionPagina + "CamposValoresPopUpMiCampo"] = value; }
        }

        public delegate void CamposValoresPopUpAceptarEventHandler(TGECampos e);
        public event CamposValoresPopUpAceptarEventHandler CamposValoresPopUpAceptar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
                this.MiCampo = new TGECampos();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.MiCampo!=null && this.MiCampo.IdCampo > 0)
                this.pnlControlesDinamicos.Controls.Add(this.ObtenerControl(this.MiCampo));
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        public void IniciarControl(TGECampos pParametro)
        {
            this.MiCampo = pParametro;
            this.lblTitulo.Text = pParametro.Titulo;
            this.pnlControlesDinamicos.Controls.Add(this.ObtenerControl(this.MiCampo));
            this.mpePopUp.Show();
        }

        //protected override void LoadViewState(object savedState)
        //{
        //    base.LoadViewState(savedState);                
        //}

        private Control ObtenerControl(TGECampos pParametro)
        {
            Control control;

            switch (pParametro.CampoTipo.IdCampoTipo)
            {   
                case (int)EnumCamposTipos.TextBox:
                    control = new TextBox();
                    ((TextBox)control).Text = pParametro.CampoValor.Valor;
                    break;
                case (int)EnumCamposTipos.IntegerTextBox:
                    control = new NumericTextBox();
                    ((NumericTextBox)control).Text = pParametro.CampoValor.Valor;
                    break;
                case (int)EnumCamposTipos.DropDownList:
                    control = new DropDownList();
                    ((DropDownList)control).SelectedItem.Text = pParametro.CampoValor.Valor;
                    break;
                default:
                    control = new Control();
                    break;
            }
            control.ID = "Control" + pParametro.IdCampo.ToString();
            return control;
        }

        private void ObtenerValor(TGECampos pParametro)
        {
            Control control = this.pnlControlesDinamicos.FindControl("Control" + pParametro.IdCampo.ToString());
            switch (pParametro.CampoTipo.IdCampoTipo)
            {
                case (int)EnumCamposTipos.TextBox:
                    pParametro.CampoValor.Valor = ((TextBox)control).Text;
                    break;
                case (int)EnumCamposTipos.IntegerTextBox:
                    pParametro.CampoValor.Valor = ((NumericTextBox)control).Text;
                    break;
                case (int)EnumCamposTipos.DropDownList:
                    pParametro.CampoValor.Valor = ((DropDownList)control).SelectedItem.Text;
                    break;
                default:
                    break;
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //this.pnlControlesDinamicos.Controls.Add(this.ObtenerControl(this.MiCampo));
            this.ObtenerValor(this.MiCampo);
            this.mpePopUp.Hide();
            if (this.CamposValoresPopUpAceptar != null)
                this.CamposValoresPopUpAceptar(this.MiCampo);
        }
    }
}