using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using System.ComponentModel;

namespace IU.Modulos.Comunes
{
    public partial class Impresion : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public delegate void ImpresionAceptarEventHandler(object sender, string e, bool resultado);
        public event ImpresionAceptarEventHandler ImpresionAceptar;

        public void IniciarControl()
        {
            this.mpePopUp.Show();
            //this.ddlImpresoras.DataSource = AyudaImpresion.ObtenerImpresoras();
            //this.ddlImpresoras.DataBind();
            this.seleccionarimpresora.Visible = true;
            this.procesoimpresion.Visible = false;
            this.pnlPopUp.GroupingText = "Seleccionar Impresora";
        }


        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            if (ImpresionAceptar != null)
                ImpresionAceptar(sender, this.ddlImpresoras.SelectedValue.ToString (), true);
            this.mpePopUp.Show();
            this.procesoimpresion.Visible = true;
            this.seleccionarimpresora.Visible = false;
            this.pnlPopUp.GroupingText = "Imprimiendo...";
            this.tmrImpresion.Enabled = true;
  
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.mpePopUp.Hide();
        }

        protected void tmrImpresion_Tick(object sender, EventArgs e)
        {
            this.tmrImpresion.Enabled = false;
            this.mpePopUp.Hide();
        }
    }
}