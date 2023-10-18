using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Xml;
using System.Collections;
using System.Globalization;
using AjaxControlToolkit;
using Comunes.Entidades;
using Seguridad.Entidades;
using Seguridad.FachadaNegocio;
using Generales;
using Generales.LogicaNegocio;
using Generales.FachadaNegocio;
using Generales.Entidades;
using System.Data;

namespace IU
{
    public class ControlesSeguros : System.Web.UI.UserControl
    {
        PaginaSegura _paginaSegura;
        protected PaginaSegura paginaSegura
        {
            get
            {
                if (_paginaSegura == null)
                    _paginaSegura = (PaginaSegura)this.Page;
                return _paginaSegura;
            }
            set { _paginaSegura = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(this.Page is PaginaSegura))
                return;

            paginaSegura = (PaginaSegura)this.Page;

            Menues menu = new Menues();
            string pathAplicacion = this.Page.Request.ApplicationPath.EndsWith("/") ? this.Page.Request.ApplicationPath : string.Concat(this.Page.Request.ApplicationPath, "/");

            menu.URL = this.Page.Request.Path.Substring(pathAplicacion.Length, this.Page.Request.Path.Length - pathAplicacion.Length);
            string pagina = (string)menu.URL.Split('/').GetValue(menu.URL.Split('/').Length - 1);

            if (!(pagina == "RegistroAsociados.aspx"))
            {
                if (UsuarioActivo == null || UsuarioActivo.IdUsuario == 0)
                {
                    //if (WindowsIdentity.GetCurrent().IsAuthenticated)
                    if (HttpContext.Current.User.Identity.IsAuthenticated)
                    {
                        Usuarios usuario = new Usuarios();
                        //usuario.Usuario = WindowsIdentity.GetCurrent().Name.Split('\\')[1];
                        usuario.Usuario = HttpContext.Current.User.Identity.Name.Split('\\')[1];

                        if (SeguridadF.UsuariosObtenerIngresoAutenticado(ref usuario))
                        {
                            this.UsuarioActivo = usuario;
                        }
                        else
                            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/IngresoSistema.aspx"), true);
                    }
                    else
                        Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/IngresoSistema.aspx"), true);
                }
            }
            this.PageLoadEvent(sender, e);
        }

        protected string MiSessionPagina
        {
            get { return this.paginaSegura.MiSessionPagina; }
            set { this.paginaSegura.MiSessionPagina = value; }
            //get
            //{
            //    if (this.ViewState[this.AppRelativeVirtualPath] == null)
            //        this.ViewState[this.AppRelativeVirtualPath] = Guid.NewGuid().ToString();
            //    return (string)this.ViewState[this.AppRelativeVirtualPath];
            //}
            //set { this.ViewState[this.AppRelativeVirtualPath] = value; }
        }

        [Browsable(true)]
        [Category("Behavior")]
        public Gestion GestionControl
        {
            get
            {
                if (Session[this.MiSessionPagina + this.AppRelativeVirtualPath] == null)
                {
                    Session[this.MiSessionPagina + this.AppRelativeVirtualPath] = Gestion.Consultar;
                    return (Gestion)Session[this.MiSessionPagina + this.AppRelativeVirtualPath];
                }
                else
                    return (Gestion)Session[this.MiSessionPagina + this.AppRelativeVirtualPath];
            }
            set { Session[this.MiSessionPagina + this.AppRelativeVirtualPath] = value; }
        }

        /// <summary>
        /// Devuelve una instancia deel usuario logueado en el Sistema
        /// </summary>
        public Usuarios UsuarioActivo
        {
            get { return this.paginaSegura.UsuarioActivo; }
            set { this.paginaSegura.UsuarioActivo = value; }
        }

        /// <summary>
        /// Devuelve una instancia de Nombre Empresa
        /// </summary>
        public string NombreEmpresa
        {
            get { return this.paginaSegura.NombreEmpresa; }
            set { this.paginaSegura.NombreEmpresa = value; }
        }

        virtual protected void PageLoadEvent(object sender, EventArgs e) { }
        /// <summary>
        /// Devuelve una única instancia de los mensajes del sistema.
        /// </summary>
        protected XmlDocument MensajesSistema
        {
            get { return this.paginaSegura.MensajesSistema; }
            set { this.paginaSegura.MensajesSistema = value; }
        }

        /// <summary>
        /// Guarda lo ordenamientos de las grillas seleccionados por el usuario
        /// </summary>
        public Hashtable OrdenamientosGrillas
        {
            get
            {
                if (Session[this.MiSessionPagina + "OrdenamientosGrillasControl"] == null)
                {
                    Session[this.MiSessionPagina + "OrdenamientosGrillasControl"] = new Hashtable();
                    return (Hashtable)Session[this.MiSessionPagina + "OrdenamientosGrillasControl"];
                }
                else
                    return (Hashtable)Session[this.MiSessionPagina + "OrdenamientosGrillasControl"];
            }
            set { Session[this.MiSessionPagina + "OrdenamientosGrillasControl"]= value; }
        }

        /// <summary>
        /// Devuevle el mensaje para mostrar del XML según el código de mensaje
        /// </summary>
        /// <param name="codigoMensaje"></param>
        /// <returns></returns>
        public string ObtenerMensajeSistema(string codigoMensaje)
        {
            XmlNode nodo = this.MensajesSistema.GetElementsByTagName(codigoMensaje).Item(0);
            if (nodo != null)
                return nodo.InnerText;
            else
                return codigoMensaje;
        }

        /// <summary>
        /// Devuevle el mensaje para mostrar del XML formateado con los argumentos
        /// </summary>
        /// <param name="codigoMensaje"></param>
        /// <returns></returns>
        public string ObtenerMensajeSistema(string codigoMensaje, List<string> parametrosMensaje)
        {
            string mensaje = this.ObtenerMensajeSistema(codigoMensaje);
            if (parametrosMensaje.Count > 0)
                mensaje = string.Format(mensaje, parametrosMensaje.ToArray());
            return mensaje;
        }

        /// <summary>
        /// Muestra un mensaje en el Sistema. Si pError=True lo muestra en color rojo.
        /// </summary>
        /// <param name="codigoMensaje"></param>
        /// <param name="pError"></param>
        protected void MostrarMensaje(string codigoMensaje, bool pError)
        {
            this.paginaSegura.MostrarMensaje(codigoMensaje, pError);
        }

        /// <summary>
        /// Muestra un mensaje en el Sistema. Si pError=True lo muestra en color rojo.
        /// </summary>
        /// <param name="codigoMensaje"></param>
        /// <param name="pError"></param>
        protected void MostrarMensaje(string codigoMensaje, bool pError, List<string> parametrosMensaje)
        {
            this.paginaSegura.MostrarMensaje(codigoMensaje, pError, parametrosMensaje);
        }

        /// <summary>
        /// Muestra un mensaje en el Sistema. Si pError=True lo muestra en color rojo.
        /// </summary>
        /// <param name="codigoMensaje"></param>
        /// <param name="pError"></param>
        protected void MostrarMensaje(string codigoMensaje, System.Drawing.Color pColor, bool pBold, List<string> parametrosMensaje)
        {
            this.paginaSegura.MostrarMensaje(codigoMensaje, pColor, pBold, parametrosMensaje);
        }

        /// <summary>
        /// Ordena una lista, guarda los valores del ordenamiento y acomoda los indices
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pLista"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected List<T> OrdenarGrillaDatos<T>(List<T> pLista, GridViewSortEventArgs e)
        {
            this.OrdenamientosGrillas = AyudaProgramacion.ObtenerOrdenamientosGrillas(this.OrdenamientosGrillas, e.SortDirection, e.SortExpression);
            SortDirection direccion = (SortDirection)this.OrdenamientosGrillas[e.SortExpression];
            string ordenamiento = string.Concat(e.SortExpression, " ", direccion);
            return AyudaProgramacion.AcomodarIndices<T>(AyudaOrdenamientos.OrderBy<T>(pLista, ordenamiento));
        }

        protected DataTable OrdenarGrillaDatos<T>(DataTable pLista, GridViewSortEventArgs e)
        {
            this.OrdenamientosGrillas = AyudaProgramacion.ObtenerOrdenamientosGrillas(this.OrdenamientosGrillas, e.SortDirection, e.SortExpression);
            SortDirection direccion = (SortDirection)this.OrdenamientosGrillas[e.SortExpression];
            string ordenamiento = string.Concat(e.SortExpression, " ", direccion.ToString().Replace("ending", ""));
            pLista.DefaultView.Sort = ordenamiento;
            return pLista;
        }
        //public NumberFormatInfo nfi
        //{
        //    get
        //    {
        //        if (Session["nfi"] == null)
        //        {
        //            Session["nfi"] = new NumberFormatInfo();
        //            return (NumberFormatInfo)Session["nfi"];
        //        }
        //        else
        //            return (NumberFormatInfo)Session["nfi"];
        //    }
        //    set { Session["nfi"] = value; }
        //}

        protected Label lblMenuPosicion
        {
            get { return (Label)this.paginaSegura.lblMenuPosicion; }
            set { this.paginaSegura.lblMenuPosicion = value;}
        }

        protected Label Mensaje
        {
            get { return (Label)this.paginaSegura.MaestraPrincipal.FindControl("lblMensaje"); }
            set
            {
                Label _mensaje = (Label)this.paginaSegura.MaestraPrincipal.FindControl("lblMensaje");
                _mensaje = value;
            }
        }

        /// <summary>
        /// Guardar los valores de busqueda seteados por el usuario
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pObjeto"></param>
        protected void BusquedaParametrosGuardarValor<T>(T pObjeto)
        {
            this.paginaSegura.BusquedaParametrosGuardarValor(pObjeto);
        }

        protected void BusquedaParametrosGuardarValor<T>(T pObjeto, string key)
        {
            this.paginaSegura.BusquedaParametrosGuardarValor(pObjeto, key);
        }

            /// <summary>
            /// Obtiene los valores de busqueda seteados por el usuario
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
        protected T BusquedaParametrosObtenerValor<T>() where T : new()
        {
            return this.paginaSegura.BusquedaParametrosObtenerValor<T>();
        }

        protected T BusquedaParametrosObtenerValor<T>(string key) where T : new()
        {
            return this.paginaSegura.BusquedaParametrosObtenerValor<T>(key);
        }

        protected ScriptManager toolkitScriptManager
        {
            get { return (ScriptManager)this.paginaSegura.MaestraPrincipal.FindControl("ScriptManager1"); }
            set
            {
                ScriptManager _tsmanager = (ScriptManager)this.paginaSegura.MaestraPrincipal.FindControl("ScriptManager1");
                _tsmanager = value;
            }
        }

        /// <summary>
        /// Validar si el usuario tiene permisos para la página
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="NombreControl"></param>
        /// <returns></returns>
        protected bool ValidarPermiso(string NombrePagina)
        {
            string pagina = string.Empty;
            foreach (Menues men in this.UsuarioActivo.Menues)
            {
                pagina = (string)men.URL.Split('/').GetValue(men.URL.Split('/').Length - 1);
                if (pagina == NombrePagina)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Validar si un control tiene permisos en la pagina actual
        /// </summary>
        /// <param name="NombreControl"></param>
        /// <returns></returns>
        public bool ValidarPermisoControl(string NombreControl)
        {
            SegControlesPaginas control = this.paginaSegura.paginaActual.ControlesPaginas.Find(x => x.ControlesPaginas == NombreControl);
            if (control != null && !control.TienePermiso)
                return false;
            else
                return true;
        }

        protected string ObtenerAppPath()
        {
            return this.Page.Request.ApplicationPath.EndsWith("/") ? this.Page.Request.ApplicationPath : string.Concat(this.Page.Request.ApplicationPath, "/");
        }

        /// <summary>
        /// Finds a Control recursively. Note finds the first match and exists
        /// </summary>
        /// <param name="ContainerCtl"></param>
        /// <param name="IdToFind"></param>
        /// <returns></returns>
        public Control BuscarControlRecursivo(Control Root, string Id)
        {
            return this.paginaSegura.BuscarControlRecursivo(Root, Id);
        }

        protected Hashtable MisParametrosUrl
        {
            get {return this.paginaSegura.MisParametrosUrl;}
            set { this.paginaSegura.MisParametrosUrl = value; }
        }

        /// <summary>
        /// Guarda el valor de una propiedad para poder recuperarlo después de un postback
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pClave"></param>
        /// <param name="pObjeto"></param>
        protected void PropiedadGuardarValor(string pClave, object pObjeto)
        {
            this.paginaSegura.PropiedadGuardarValor(pClave, pObjeto);
        }

        /// <summary>
        /// Recupera el valor de una propiedad guardada después de un postback
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pClave"></param>
        /// <returns></returns>
        protected T PropiedadObtenerValor<T>(string pClave) where T : new()
        {
            return this.paginaSegura.PropiedadObtenerValor<T>(pClave);
        }


        protected void SetInitializeCulture(string simbolo)
        {
            this.paginaSegura.SetInitializeCulture(simbolo);
        }
    }
}
