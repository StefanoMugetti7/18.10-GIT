using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.UI.HtmlControls;
using System.Text;
using Comunes.Entidades;

namespace IU
{
    public partial class Maestra : System.Web.UI.MasterPage
    {
        //private bool CargarMenu
        //{
        //    get
        //    {
        //        if (Session[this.MiSessionPagina + "MaestraCargarMenu"] != null)
        //        { return (bool)Session[this.MiSessionPagina + "MaestraCargarMenu"]; }
        //        else
        //        { return false; }
        //    }
        //    set { Session[this.MiSessionPagina + "MaestraCargarMenu"] = value; }
        //}

        public string MiSessionPagina
        {
            get
            {
                return Request.QueryString["tabName"] == null ? string.Empty : Request.QueryString["tabName"].ToString();
            }
            set
            {
                //this.ViewState["UniqueTabGuid"] = value;
            }
            //get
            //{
            //    if (this.ViewState[this.AppRelativeVirtualPath] == null)
            //        this.ViewState[this.AppRelativeVirtualPath] = Guid.NewGuid().ToString();
            //    return (string)this.ViewState[this.AppRelativeVirtualPath];
            //}
            //set { this.ViewState[this.AppRelativeVirtualPath] = value; }
        }

        private Usuarios UsuarioActivo
        {
            get
            {
                if (Session["UsuarioActivo"] != null)
                { return (Usuarios)Session["UsuarioActivo"]; }
                else
                { return new Usuarios(); }
            }
            set { Session["UsuarioActivo"] = value; }
        }


        private StringBuilder MenuBootStrap;
        protected override void OnInit(EventArgs e)
        {
            //string jscript = "";
            base.OnInit(e);
        }

        private string MenuHtml
        {
            get
            {
                if (Session["MaestraMenuHtml"] != null)
                { return (string)Session["MaestraMenuHtml"]; }
                else
                { return string.Empty; }
            }
            set { Session["MaestraMenuHtml"] = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            HtmlLink link = null;
            foreach (Control c in this.Page.Header.Controls)
            {
                if (c is HtmlLink)
                {
                    link = c as HtmlLink;

                    if (link.Href.IndexOf("App_Themes/", StringComparison.InvariantCultureIgnoreCase) >= 0 &&
                        link.Href.EndsWith(".css", StringComparison.InvariantCultureIgnoreCase))
                    {
                        link.Href += string.Format("?t={0}", "20211214");
                    }
                }
            }
            base.OnPreRender(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                navbarNavDropdown.Visible = UsuarioActivo.IdUsuario > 0;
            }
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Session.Clear();
            //this.UsuarioActivo = new Usuarios();
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/IngresoSistema.aspx"), true);
        }

        protected void ScriptManager1_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            //string debugMsg = string.Concat("Msg: ", e.Exception.Message, e.Exception.InnerException.ToString(), e.Exception.Source, e.Exception.StackTrace);
            ScriptManager1.AsyncPostBackErrorMessage = e.Exception.Message;
            //ScriptManager1.AsyncPostBackErrorMessage = debugMsg;
            //this.lblMensaje.Text = e.Exception.Message;
            //this.popUpMensajes.MostrarMensaje(debugMsg, true);
            this.popUpMensajes.MostrarMensaje(e.Exception.Message, true);
        }
    }
}
