using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seguridad.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;
using System.Text;

namespace IU
{
    public partial class Maestra2 : System.Web.UI.MasterPage
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

        private string MiSessionPagina
        {
            get
            {
                if (this.ViewState[this.AppRelativeVirtualPath] == null)
                    this.ViewState[this.AppRelativeVirtualPath] = Guid.NewGuid().ToString();
                return (string)this.ViewState[this.AppRelativeVirtualPath];
            }
            set { this.ViewState[this.AppRelativeVirtualPath] = value; }
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

        public EnumMenues MenuPadre
        {
            get
            {
                if (Session["MenuPadre"] != null)
                { return (EnumMenues)Session["MenuPadre"]; }
                else
                { return EnumMenues.General; }
            }
            set { Session["MenuPadre"] = value; }
        }

        /// <summary>
        /// Devuelve una única instancia de los mensajes del sistema.
        /// </summary>
        protected string NombreEmpresa
        {
            get
            {
                if (this.Application["NombreEmpresa"] != null)
                { return (string)this.Application["NombreEmpresa"]; }
                else
                {
                    string valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.NombreEmpresa).ParametroValor;
                    this.Application.Add("NombreEmpresa", valor);
                    return valor;
                }
            }
            set { this.Application["NombreEmpresa"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            //string jscript = "";
            base.OnInit(e);
        }

        //==== Method to create dynamic menu.
        public void generateDynamicMenu()
        {
            StringBuilder sbMenu = new StringBuilder();

            //==== Created first ul element of the unordered list to generate menu also assigned id="menu" as our entire css is based on this id.
            sbMenu.Append(" <ul class=\"nav navbar-nav\">");

            //==== Call recursive method to get all child elements according to parents.
            //=== We we pass 0 as argument as 0 is id of default parent.
            string childItems = getMenuItems(0);
            sbMenu.Append(childItems);

            //==== Close dynamic menu unordered list.
            sbMenu.Append(" </ul>");

            //==== Show generated menu inside div.
            navbarContent.InnerHtml = sbMenu.ToString();

        }


        //==== Recrusive method.
        StringBuilder sbMenu = new StringBuilder();
        //int childCount = 0;
        public string getMenuItems(int parentId)
        {
            List<Menues> menues = this.UsuarioActivo.Menues.FindAll(delegate(Menues m) { return m.IdMenuPadre == parentId && m.Mostrar; });
            bool hijos;
                foreach (var obj in menues)
                {
                    hijos = this.UsuarioActivo.Menues.Exists(r => r.IdMenuPadre == obj.IdMenu && r.Mostrar);
                    if (hijos)
                    {
                        //sbMenu.Append("<li><a target=\"_blank\" href='" + Page.ResolveClientUrl(obj.URL) + "'>" + obj.Menu + "</a><ul>");
                        //getMenuItems(obj.IdMenu);
                        if (obj.IdMenuPadre == 0)
                        {
                            sbMenu.Append("<li>");
                            sbMenu.Append("<a id=\"" + obj.IdMenu + "\" href=\"#\" data-toggle=\"dropdown\" class=\"dropdown-toggle\">"+ obj.Menu  +"<b class=\"caret\"></b></a>");
                        }
                        else
                        {
                            sbMenu.Append("<li class=\"dropdown-submenu\">");
                            sbMenu.Append("<a id=\""+ obj.IdMenu + "\" href=\"#\" data-toggle=\"dropdown\" class=\"dropdown-toggle\">" + obj.Menu + "</a>");
                        }
                        sbMenu.Append("<ul class=\"dropdown-menu\">");
                        getMenuItems(obj.IdMenu);
                        sbMenu.Append("</ul>");
                        sbMenu.Append("</li>");
                    }
                    else
                    {
                        if (obj.IdMenuPadre == 0)
                        {
                            sbMenu.Append("<li class=\"nav-item\"><a href=\"" + Page.ResolveUrl("/" + obj.URL) + "\" class=\"nav-link\">" + obj.Menu + "</a></li>");
                        }
                        else
                        {
                            //sbMenu.Append("<li><a target=\"_blank\" href='" + Page.ResolveUrl(obj.URL) + "'>" + obj.Menu + "</a></li>");
                            sbMenu.Append("<li><a href=\"" + Page.ResolveUrl("/" + obj.URL) + "\" >" + obj.Menu + "</a></li>");
                        }
                    }

                }
            return sbMenu.ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string pathAplicacion = this.Page.Request.ApplicationPath.EndsWith("/") ? this.Page.Request.ApplicationPath : string.Concat(this.Page.Request.ApplicationPath, "/");
            string url = this.Page.Request.Path.Substring(pathAplicacion.Length, this.Page.Request.Path.Length - pathAplicacion.Length);
            string pagina = (string)url.Split('/').GetValue(url.Split('/').Length - 1);
            if (!this.IsPostBack
                || pagina=="ReportesSeleccionar.aspx")
            {
                //==== Bind Menu.
                generateDynamicMenu();
            }
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
