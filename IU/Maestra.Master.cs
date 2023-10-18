using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;
using System.Web.Services;
using System.Web.Script.Services;
using System.Web.UI.HtmlControls;
using System.Text;
using Seguridad.FachadaNegocio;

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

        public EnumMenues MenuPadre
        {
            get
            {
                if (Session[this.MiSessionPagina + "MenuPadre"] != null)
                { return (EnumMenues)Session[this.MiSessionPagina + "MenuPadre"]; }
                else
                { return EnumMenues.General; }
            }
            set { Session[this.MiSessionPagina + "MenuPadre"] = value; }
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
                    string valor = TGEGeneralesF.EmpresasSeleccionar().Empresa;
                    this.Application.Add("NombreEmpresa", valor);
                    return valor;
                }
            }
            set { this.Application["NombreEmpresa"] = value; }
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
                        link.Href += string.Format("?t={0}", "20230807");
                    }
                }
            }
            base.OnPreRender(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string pathAplicacion = this.Page.Request.ApplicationPath.EndsWith("/") ? this.Page.Request.ApplicationPath : string.Concat(this.Page.Request.ApplicationPath, "/");
            string url = this.Page.Request.Path.Substring(pathAplicacion.Length, this.Page.Request.Path.Length - pathAplicacion.Length);
            string pagina = (string)url.Split('/').GetValue(url.Split('/').Length - 1);
            if (!this.IsPostBack
                || pagina=="ReportesSeleccionar.aspx")
            {
                this.Page.Title = this.NombreEmpresa == string.Empty ? "EVOL - SIM" : this.NombreEmpresa;
                this.ctrNotificaciones.Visible = UsuarioActivo.IdUsuario > 0;
                this.MenuBootStrap = new StringBuilder();
                this.CargarMenuBootStrap(((int)MenuPadre));
                this.MenuHtml = this.MenuBootStrap==null ? string.Empty : this.MenuBootStrap.ToString();
                this.ltrMenu.Text = this.MenuHtml;
            }
        }

        private void CargarMenu(int IdPadre, MenuItem mi)
        {
            //string texto = string.Empty;
            //string accion = string.Empty;
            //string appPath = string.Empty;
            //string jsEventClick = string.Empty;
            //bool nuevaPestana = false;
            //List<Menues> menues = this.UsuarioActivo.Menues.FindAll(delegate(Menues m) { return m.IdMenuPadre == IdPadre; });

            //string queryString = this.Request.QueryString.ToString();

            //foreach (Menues men in menues)
            //{
            //    if (!men.Mostrar)
            //        continue;

            //    texto = men.Menu;
                
            //    if (men.URL.Length > 0)
            //    {
            //        if (men.URL.StartsWith("http"))
            //        {
            //            appPath = string.Empty;
            //            nuevaPestana = true;
            //        }
            //        else
            //        {
            //            //URL DEL SISTEMA
            //            nuevaPestana = false;
            //        if (this.Page.Request.ApplicationPath.EndsWith("/"))
            //            appPath = this.Page.Request.ApplicationPath;
            //        else
            //        appPath = string.Concat(this.Page.Request.ApplicationPath, "/");
                        
            //            if (accion.Contains(".pdf"))
            //                nuevaPestana = true;
            //        }
            //        //string sessionCookie = string.Empty;
            //        //if (this.Session.IsCookieless)
            //        //{
            //        //    sessionCookie = this.Page.Request.RawUrl.Split('/')[1] + "/";
            //        //}
            //        accion = AyudaProgramacion.ObtenerUrlParametros(appPath + men.URL);// +"?" + queryString;
            //        //accion = Response.ApplyAppPathModifier(appPath + men.URL);
            //        texto = string.Concat("<span style='cursor:pointer;' onclick='javascript:fnMenuItemClick();'>", texto, "</span>");
            //    }
            //    else
            //    {
            //        accion = "javascript: void(0);";
            //        jsEventClick = string.Empty;
            //    }
            //    //if (Request.FilePath == accion)
            //    //    lblUbicacion.Text = men.Descripcion;

            //    bool bHijos = men.Hijos;
            //    int idPadre = men.IdMenu;

            //    MenuItem miNuevo = new MenuItem();
            //    miNuevo.Text = texto;
            //    miNuevo.NavigateUrl = accion;
            //    miNuevo.Target = nuevaPestana==true? "_blank" : string.Empty;

            //    if (mi == null)
            //    {
            //        this.mnMenuPrincipal.Items.Add(miNuevo);
            //    }
            //    else
            //    {
            //        miNuevo.NavigateUrl = accion;
            //        mi.ChildItems.Add(miNuevo);
            //    }
            //    if (bHijos)
            //    {
            //        CargarMenu(idPadre, miNuevo);
            //        if (miNuevo.ChildItems.Count == 0)
            //            this.mnMenuPrincipal.Items.Remove(miNuevo);
            //    }
            //}
        }

        protected void btnSalir_Click(object sender, EventArgs e)
        {
            Session.Clear();
            this.UsuarioActivo = new Usuarios();
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/IngresoSistema.aspx"),true);
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

        private void CargarMenuBootStrap(int IdPadre)
        {
            string accion = string.Empty;
            string appPath = string.Empty;
            string target = string.Empty;

            List<Menues> menues = this.UsuarioActivo.Menues.FindAll(x => x.IdMenuPadre == IdPadre && x.Mostrar);

            foreach (Menues men in menues)
            {
                string dddnId = string.Concat("dropdown", men.IdMenu.ToString());

                //Primer Nivel del Menu
                if (this.MenuTieneHijos(men))
                {
                    //Menu dinamico
                    this.MenuBootStrap.AppendLine("<li class=\"dropdown\">");
                    this.MenuBootStrap.AppendFormat("<a class=\"nav-link dropdown-toggle\" href=\"#\" id=\"{0}\" role=\"button\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">{1}</a>", dddnId, men.Menu);
                    this.MenuBootStrap.AppendFormat("<ul class=\"dropdown-menu border-0 \" aria-labelledby=\"{0}\">", dddnId);
                    this.DropDownItemDinamico(men);
                    this.MenuBootStrap.AppendLine("</ul>");
                    this.MenuBootStrap.AppendLine("</li>");
                }
                else
                {
                    //Menu statico
                    if (men.URL.Length > 0)
                    {
                        if (men.URL.StartsWith("http"))
                        {
                            appPath = string.Empty;
                            target = "_blank";
                        }
                        else
                        {
                            //URL DEL SISTEMA
                            if (this.Page.Request.ApplicationPath.EndsWith("/"))
                                appPath = this.Page.Request.ApplicationPath;
                            else
                                appPath = string.Concat(this.Page.Request.ApplicationPath, "/");

                            if (accion.Contains(".pdf"))
                                target = "_blank";
                        }
                        accion = AyudaProgramacion.ObtenerUrlParametros(appPath + men.URL);// +"?" + queryString;
                    }
                    else
                        accion = "#";

                    this.MenuBootStrap.AppendLine("<li class=\"\">");
                    this.MenuBootStrap.AppendFormat("<a class=\"nav-link\" href=\"{0}\" target=\"{1}\">{2}</a>", accion, target, men.Menu);
                    this.MenuBootStrap.AppendLine("</li>");
                }

            }
        }

        private void DropDownItemDinamico(Menues menu)
        {
            if (this.MenuTieneHijos(menu))
            {
                List<Menues> menuesHijos = this.UsuarioActivo.Menues.FindAll(x => x.IdMenuPadre == menu.IdMenu && x.Mostrar);

                foreach (Menues mh in menuesHijos)
                {
                    if (this.MenuTieneHijos(mh))
                    {
                        string dddnId = string.Concat("dropdown", mh.IdMenu.ToString());
                        this.MenuBootStrap.AppendLine("<li class=\"dropdown-submenu\">");
                        this.MenuBootStrap.AppendFormat("<a class=\"dropdown-item dropdown-toggle\" id=\"{0}\" href=\"#\" data-toggle=\"dropdown\" aria-haspopup=\"true\" aria-expanded=\"false\">{1}</a>", dddnId, mh.Menu);
                        this.MenuBootStrap.AppendFormat("<ul class=\"dropdown-menu border-0 \" aria-labelledby=\"{0}\">", dddnId);

                        this.DropDownItemDinamico(mh);

                        this.MenuBootStrap.AppendLine("</ul>");
                        this.MenuBootStrap.AppendLine("</li>");
                    }
                    else
                        this.MenuBootStrap.AppendFormat(this.ItemDinamico(mh));
                }
            }
            else
            {
                this.MenuBootStrap.AppendFormat(this.ItemDinamico(menu));
            }

        }

        private string ItemDinamico(Menues men)
        {
            string target = string.Empty;
            string appPath = string.Empty;
            string accion = string.Empty;

            if (men.URL.Length > 0)
            {
                if (men.URL.StartsWith("http"))
                {
                    appPath = string.Empty;
                    target = "_blank";
                }
                else
                {
                    //URL DEL SISTEMA
                    if (this.Page.Request.ApplicationPath.EndsWith("/"))
                        appPath = this.Page.Request.ApplicationPath;
                    else
                        appPath = string.Concat(this.Page.Request.ApplicationPath, "/");

                    if (accion.Contains(".pdf"))
                        target = "_blank";
                }

                accion = AyudaProgramacion.ObtenerUrlParametros(appPath + men.URL);// +"?" + queryString;
            }
            else
                accion = "#";

            //return string.Format("<a class=\"dropdown-item\" href=\"{0}\" target=\"{1}\">{2}</a>", accion, target, men.Menu);
            return string.Format("<li><a class=\"dropdown-item\" href=\"{0}\" target=\"{1}\">{2}</a></li>", accion, target, men.Menu);
            //return string.Format("<li class=\"dropdown-item\" ><a href=\"{0}\" target=\"{1}\">{2}</a></li>", accion, target, men.Menu);
        }

        private bool MenuTieneHijos(Menues men)
        {
            return this.UsuarioActivo.Menues.Exists(x => x.IdMenuPadre == men.IdMenu && x.Mostrar);
        }

        protected void ddlFilialUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.ddlFilialUsuario.SelectedValue))
            {
                this.UsuarioActivo.FilialPredeterminada = this.UsuarioActivo.Filiales.Find(x => x.IdFilial == Convert.ToInt32(this.ddlFilialUsuario.SelectedValue));
                this.UsuarioActivo.NoValidarSector = true;
                this.UsuarioActivo.SectorPredeterminado.IdSector = 0;
                if (SeguridadF.UsuariosModificarFilial(this.UsuarioActivo))
                {
                    this.Response.Redirect(this.Request.Url.ToString());
                }
            }
        }

        protected void ddlSectorUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UsuarioActivo.SectorPredeterminado.IdSector = this.ddlSectorUsuario.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlSectorUsuario.SelectedValue);

            if (SeguridadF.UsuariosModificarFilial(this.UsuarioActivo))
            {
                this.Response.Redirect(this.Request.Url.ToString());
            }
        }

        
    }
}
