using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;

namespace IU.Modulos.Comunes
{
    public partial class popUpGrillaGenerica : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public void IniciarControl(Objeto pParametro)
        {
            this.lblDetalleMsg.Text = pParametro.CodigoMensajeArgs.FirstOrDefault();
            //this.lblTitulo.Text = "Sistema de gestión para mutuales";
            
            this.gvDatos.DataSource = pParametro.dsResultado.Tables[0];
            this.gvDatos.DataBind();
            
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModalGrillaGenerica", "ShowModalPopUpGrillaGenerica();", true);

        }
        public void IniciarControl(Objeto pParametro,string titulo,string detalle)
        {
            this.lblDetalleMsg.Text = "Lista: " + detalle;
            //this.lblTitulo.Text = titulo;
            this.h5Titulo.InnerHtml = titulo;
            this.lblDetalle.Visible = false;
            this.gvDatos.DataSource = pParametro.dsResultado.Tables[0];
            this.gvDatos.DataBind();
            
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModalGrillaGenerica", "ShowModalPopUpGrillaGenerica();", true);

        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {

            }
        }

        //protected void btnVolver_Click(object sender, EventArgs e)
        //{
        //    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModalGrillaGenerica", "HideModalPopUpGrillaGenerica();", true);
        //}
    }
}