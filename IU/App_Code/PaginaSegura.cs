﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Xml;
using System.Collections;
using System.Globalization;
using AjaxControlToolkit;
using System.Data;
using System.Security.Principal;
using Seguridad.Entidades;
using Comunes.Entidades;
using Seguridad;
using Seguridad.LogicaNegocio;
using Generales;
using Generales.LogicaNegocio;
using Generales.FachadaNegocio;
using Seguridad.FachadaNegocio;
using Generales.Entidades;
using Expedientes;
using System.Threading;
using System.Web.SessionState;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using Evol.Controls;
using System.Text;
using System.IO;
using Comunes.LogicaNegocio;
using RestSharp.Extensions;
using IU.Modulos.Seguridad.Controles;

namespace IU
{
    public class PaginaSegura : Page
    {

        public string MiSessionPagina
        {
            get
            {
                return Request.QueryString["tabName"]==null? string.Empty : Request.QueryString["tabName"].ToString();
            }
            set
            {
                //this._sessionPaginaHija = value;
            }
        }

        protected Gestion MiGestion
        {
            get
            {
                return this.PropiedadObtenerValor<Gestion>(this.AppRelativeVirtualPath);
            }   
            set { this.PropiedadGuardarValor(this.AppRelativeVirtualPath, value); }
        }
        
        /// <summary>
        /// Devuelve una instancia deel usuario logueado en el Sistema
        /// </summary>
        public Usuarios UsuarioActivo
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

        public TGEEmpresas UsuarioEmpresa
        {
            get
            {
                if (this.Application["UsuarioEmpresa"] == null)
                    this.Application["UsuarioEmpresa"] = TGEGeneralesF.EmpresasSeleccionar();
                return (TGEEmpresas)this.Application["UsuarioEmpresa"];
            }
            set { this.Application["UsuarioEmpresa"] = value; }
        }

        public Version AssemblyVersion
        {
            get
            {
                if (this.Application["AssemblyVersion"] == null)
                {
                    this.Application["AssemblyVersion"] = Assembly.GetExecutingAssembly().GetName().Version;
                }
                return (Version)this.Application["AssemblyVersion"];
            }
            set { this.Application["AssemblyVersion"] = value; }
        }

        /// <summary>
        /// Setea el MenuPadre que sera utilizado para Cargar el Menu Principal
        /// </summary>
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
        public XmlDocument MensajesSistema
        {
            get
            {
                if (this.Application == null)
                { return TGEGeneralesF.TGELeerXMLObtenerMensajesSistema(); }
                else if (this.Application["MensajesSistema"] != null)
                { return (XmlDocument)this.Application["MensajesSistema"]; }
                else
                {
                    XmlDocument xmlDoc = TGEGeneralesF.TGELeerXMLObtenerMensajesSistema();
                    this.Application.Add("MensajesSistema", xmlDoc);
                    return xmlDoc; }
            }
            set { this.Application["MensajesSistema"] = value; }
        }

        /// <summary>
        /// Devuelve una única instancia de los mensajes del sistema.
        /// </summary>
        public string NombreEmpresa
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

        #region "Pagina Maestra"

        /// <summary>
        /// Pagina Maestra Principal del Sistema
        /// </summary>
        public MasterPage MaestraPrincipal
        {
            get
            {
                if (this.Master is Maestra || this.Master is Maestra2)
                    return this.Master;
                else if (this.Master.Master is Maestra || this.Master.Master is Maestra2)
                    return this.Master.Master;
                else
                    return this.Master.Master.Master;
            }
            set { }
        }
        /// <summary>
        /// Devuelve una instancia del control Label Mensaje en la Master para
        /// mostrar un mensaje en el sistema.
        /// </summary>
        public Label Mensaje
        {
            get { return (Label)this.MaestraPrincipal.FindControl("lblMensaje"); }
            set
            {
                Label _mensaje = (Label)this.MaestraPrincipal.FindControl("lblMensaje");
                _mensaje = value;
            }
        }

        /// <summary>
        /// Devuelve una instancia del control Literal Menu de la Master para
        /// mostrar los Menues del Sistema.
        /// </summary>
        public Literal ltrMenu
        {
            get { return (Literal)this.MaestraPrincipal.FindControl("ltrMenu"); }
            set
            {
                Literal _ltrmenu = (Literal)this.MaestraPrincipal.FindControl("ltrMenu");
                _ltrmenu = value;
            }
        }

        /// <summary>
        /// Devuelve una instancia del control Label Menu en la Master para especificar 
        /// la úbicacion de la página actual.
        /// </summary>
        public Label UsuarioLogueado
        {
            get { return (Label)MaestraPrincipal.FindControl("lblUsuarioLogueado"); }
            set
            {
                Label _menu = (Label)MaestraPrincipal.FindControl("lblUsuarioLogueado");
                _menu = value;
            }
        }
        public Label LabelFilial
        {
            get { return (Label)MaestraPrincipal.FindControl("lblFilialText"); }
            set
            {
                Label _menu = (Label)MaestraPrincipal.FindControl("lblFilialText");
                _menu = value;
            }
        }     
        public Label LabelSector
        {
            get { return (Label)MaestraPrincipal.FindControl("lblSectorText"); }
            set
            {
                Label _menu = (Label)MaestraPrincipal.FindControl("lblSectorText");
                _menu = value;
            }
        }
        public DropDownList Filial
        {
            get { return (DropDownList)MaestraPrincipal.FindControl("ddlFilialUsuario"); }
            set
            {
                DropDownList _menu = (DropDownList)MaestraPrincipal.FindControl("ddlFilialUsuario");
                _menu = value;
            }
        }

        public DropDownList Sector
        {
            get { return (DropDownList)MaestraPrincipal.FindControl("ddlSectorUsuario"); }
            set
            {
                DropDownList _menu = (DropDownList)MaestraPrincipal.FindControl("ddlSectorUsuario");
                _menu = value;
            }
        }
        /// <summary>
        /// Devuelve una instancia del control Label Mensajes Alertas en la Master
        /// </summary>
        public Label MensajesAlertas
        {
            get { return (Label)MaestraPrincipal.FindControl("lblMensajesAlertas"); }
            set
            {
                Label _menu = (Label)MaestraPrincipal.FindControl("lblMensajesAlertas");
                _menu = value;
            }
        }

        public Image LogoICO
        {
            get { return (Image)MaestraPrincipal.FindControl("imgLogoICO"); }
            set
            {
                Image _menu = (Image)MaestraPrincipal.FindControl("imgLogoICO");
                _menu = value;
            }
        }
        //public SegUsuariosNotificacionesModificarPopUp controlNotificaciones
        //{
        //    get { return (SegUsuariosNotificacionesModificarPopUp)MaestraPrincipal.FindControl("ctrNotificaciones"); }
        //    set
        //    {
        //        SegUsuariosNotificacionesModificarPopUp _menu = (SegUsuariosNotificacionesModificarPopUp)MaestraPrincipal.FindControl("ctrNotificaciones");
        //        _menu = value;
        //    }
        //}
        /// <summary>
        /// Devuelve una instancia del control ScriptManager en la Master
        /// </summary>
        public ScriptManager scriptManager
        {
            get { return (ScriptManager)this.MaestraPrincipal.FindControl("ScriptManager1"); }
            set
            {
                ScriptManager _scriptManager = (ScriptManager)this.MaestraPrincipal.FindControl("ScriptManager1");
                _scriptManager = value;
            }
        }

        public Label lblMenuPosicion
        {
            get { return (Label)MaestraPrincipal.FindControl("lblMenuPosicion"); }
            set
            {
                Label _panel = (Label)MaestraPrincipal.FindControl("lblMenuPosicion");
                _panel = value;
            }
        }

        public HiddenField HdfBrowserTab
        {
            get { return (HiddenField)MaestraPrincipal.FindControl("hdfBrowserTab"); }
            set
            {
                HiddenField hdf = (HiddenField)MaestraPrincipal.FindControl("hdfBrowserTab");
                hdf = value;
            }
        }

        #endregion

        public Menues paginaActual
        {
            get
            {
                if (Session[this.MiSessionPagina + "paginaActual"] != null)
                { return (Menues)Session[this.MiSessionPagina + "paginaActual"]; }
                else
                { return new Menues(); }
            }
            set { Session[this.MiSessionPagina + "paginaActual"] = value; }
        }

        /// <summary>
        /// Guarda lo ordenamientos de las grillas seleccionados por el usuario
        /// </summary>
        public Hashtable OrdenamientosGrillas
        {
            get
            {
                if (this.PropiedadObtenerValor<Hashtable>("OrdenamientosGrillas") == null)
                {
                    this.PropiedadGuardarValor("OrdenamientosGrillas", new Hashtable());
                    return this.PropiedadObtenerValor<Hashtable>("OrdenamientosGrillas");
                }
                else
                    return this.PropiedadObtenerValor<Hashtable>("OrdenamientosGrillas");
            }
            set { this.PropiedadGuardarValor("OrdenamientosGrillas", value); }
        }

        public NumberFormatInfo nfi
        {
            get
            {
                if (this.PropiedadObtenerValor<NumberFormatInfo>("nfi") == null)
                {
                    this.PropiedadGuardarValor("nfi", new NumberFormatInfo());
                    return this.PropiedadObtenerValor<NumberFormatInfo>("nfi");
                }
                else
                    return this.PropiedadObtenerValor<NumberFormatInfo>("nfi");
            }
            set { this.PropiedadGuardarValor("nfi", value); }
        }

        public string viewStatePaginaSegura {
            get { return this.ViewState["UrlReferrer"] == null ? string.Empty : this.ViewState["UrlReferrer"].ToString();
            }
            set { this.ViewState["UrlReferrer"] = value; }
        }
                
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            
            ScriptManager.RegisterClientScriptResource(this, typeof(WebResources), "IU.Recursos.accounting.js");

            string estilos = !string.IsNullOrWhiteSpace(UsuarioActivo.FilialPredeterminada.EstiloPlantillaSucursal) ? UsuarioActivo.FilialPredeterminada.EstiloPlantillaSucursal : "com_1";

            if (Page.Theme != estilos)
            {
                if (Directory.Exists(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "\\App_Themes\\", estilos)))
                {
                    Page.Theme = estilos;
                }
            }
        }

        protected void Page_Load(object sender, System.EventArgs e)
        {
            try
            {
                Menues men = new Menues();

                if (this.Request.UrlReferrer != null
                    &&this.Request.Url.AbsolutePath == this.Request.UrlReferrer.AbsolutePath
                    && this.Request.Url.Query != this.Request.UrlReferrer.Query)
                {
                    string newKey = this.Request.QueryString["tabName"].ToString();
                    NameValueCollection nvc = HttpUtility.ParseQueryString(Request.UrlReferrer.Query);
                    string oldKey = nvc["tabName"] != null ? nvc["tabName"].ToString() : string.Empty;
                    Hashtable newSession = new Hashtable();
                    if (oldKey.Length > 0)
                    {                        
                        foreach (string key in this.Session.Keys)
                        {
                            if (key.Contains(oldKey))
                            {
                                newSession.Add(key.Replace(oldKey, newKey), Session[key]);
                            }
                        }
                        foreach (string key in newSession.Keys)
                        {
                            Session.Add(key, newSession[key]);
                        }
                    }
                }
                

                if ( UsuarioActivo == null || UsuarioActivo.IdUsuario == 0)
                {
                    ////if (WindowsIdentity.GetCurrent().IsAuthenticated)
                    //if (HttpContext.Current.User.Identity.IsAuthenticated)
                    //{
                    //    Usuarios usuario = new Usuarios();
                    //    //usuario.Usuario = WindowsIdentity.GetCurrent().Name.Split('\\')[1];
                    //    usuario.Usuario = HttpContext.Current.User.Identity.Name.Split('\\')[1];

                    //    if (SeguridadF.UsuariosObtenerIngresoAutenticado(ref usuario))
                    //    {
                    //        this.UsuarioActivo = usuario;
                    //    }
                    //    else
                    //        Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/IngresoSistema.aspx"), true);
                    //}
                    //else
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/IngresoSistema.aspx"), true);
                    //this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/ControlSesion.aspx"), true);
                }

                if (this.IsPostBack)
                {
                    this.Mensaje.Text = string.Empty;
                    miInitializeCulture(nfi);
                }
                else
                {
                    this.MenuPadre = EnumMenues.General;
                    nfi = new NumberFormatInfo();
                    nfi.CurrencyDecimalDigits = 2;
                    nfi.CurrencyDecimalSeparator = ",";
                    nfi.CurrencyGroupSeparator = ".";
                    nfi.CurrencyGroupSizes = new int[] { 3, 3, 3, 3 };
                    nfi.CurrencySymbol = "$";
                    nfi.CurrencyNegativePattern = 12; // $ -n
                    nfi.NumberDecimalDigits = 2;
                    nfi.NumberDecimalSeparator = ",";
                    nfi.NumberGroupSeparator = ".";
                    nfi.NumberGroupSizes = new int[] { 3, 3, 3, 3 };
                    miInitializeCulture(nfi);
                    this.ViewState["UrlReferrer"] = Request.UrlReferrer;

                    if (UsuarioActivo.IdUsuario > 0)
                    {
                        this.paginaActual = new Menues();
                        string pathAplicacion = this.Page.Request.ApplicationPath.EndsWith("/") ? this.Page.Request.ApplicationPath : string.Concat(this.Page.Request.ApplicationPath, "/");

                        this.paginaActual.URL = this.Page.Request.Path.Substring(pathAplicacion.Length, this.Page.Request.Path.Length - pathAplicacion.Length);
                        string pagina = (string)this.paginaActual.URL.Split('/').GetValue(this.paginaActual.URL.Split('/').Length - 1);
                        //Validación para el cambio de Contraseña
                        if (UsuarioActivo.CambiarContrasenia && pagina != "SegCambiarContrasenia.aspx")
                            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Seguridad/SegCambiarContrasenia.aspx"), true);

                        //Formateo de Números de Moneda
                        //CultureInfo customCulture = new CultureInfo("es-AR", false);
                        //nfi = customCulture.NumberFormat;
                        //nfi.CurrencyDecimalDigits = 2;
                        //nfi.CurrencyDecimalSeparator = ",";
                        //nfi.CurrencyGroupSeparator = ".";
                        //nfi.CurrencyGroupSizes = new int[] {3,3,3,3};
                        //nfi.CurrencySymbol = "$ ";

                        // Valido que el Usuario tenga permisos para ver la página requerida
                        if (!(pagina == "InicioSistema.aspx" ||
                            pagina == "SegCambiarContrasenia.aspx"
                            || pagina == "ReportesExportar.aspx"
                            || pagina == "ControlMensajes.aspx"
                            || pagina == "prueba.aspx"
                            || pagina == "PruebaMenu.aspx"
                            || pagina == "AfiliadosListar2.aspx"
                            ))
                        {
                            men = UsuarioActivo.Menues.Find(delegate(Menues me)
                            {
                                return paginaActual.URL.Equals(me.URL) && me.PaginaVolver == false;
                            });
                            if (men == null)
                            {
                                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                            }
                            else
                                this.paginaActual = men;
                        }
                        //this.UsuarioLogueado.Text = HttpContext.Current.User.Identity.Name;//.Split('\\')[1];
                        //string.Format(this.ObtenerMensajeSistema("UsuarioLogueado")
                       
                        
                        this.UsuarioLogueado.Text = "Usuario: " + this.UsuarioActivo.ApellidoNombre ;// this.UsuarioActivo.FilialPredeterminada.Filial, this.UsuarioActivo.SectorPredeterminado.Sector
                        //this.controlNotificaciones.Visible = true;

                        this.LabelFilial.Visible = true;
                        this.Filial.Visible = true;
                        this.Filial.DataSource = this.UsuarioActivo.Filiales;
                        this.Filial.DataValueField = "IdFilial";
                        this.Filial.DataTextField = "Filial";
                        this.Filial.DataBind();
                        this.Filial.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();

                        if(this.Filial.Items.Count > 0)
                        {
                            this.LabelSector.Visible = true;
                            this.Sector.Visible = true;
                            this.Sector.DataSource = this.UsuarioActivo.Filiales.Find(x => x.IdFilial == this.UsuarioActivo.FilialPredeterminada.IdFilial).Sectores;
                            this.Sector.DataValueField = "IdSector";
                            this.Sector.DataTextField = "Sector";
                            this.Sector.DataBind();
                            AyudaProgramacion.AgregarItemSeleccione(this.Sector,this.ObtenerMensajeSistema("SeleccioneOpcion"));
                            this.Sector.SelectedValue = this.UsuarioActivo.SectorPredeterminado.IdSector.ToString();
                        }
                        if(this.Sector.Items.Count == 0)
                        {
                            AyudaProgramacion.InsertarItemSeleccione(this.Sector, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                        }
                        this.MensajesAlertas.Text = string.Concat(this.UsuarioEmpresa.Descripcion, " - Ver.", AssemblyVersion.ToString(), " RC");

                        if (UsuarioEmpresa.Logo != null)
                        {
                            System.Drawing.Bitmap img = new System.Drawing.Bitmap(new MemoryStream(UsuarioEmpresa.Logo));
                            System.Drawing.Bitmap newImg = AyudaProgramacionLN.ScaleImage(img, Convert.ToInt32(this.LogoICO.Width.Value), Convert.ToInt32(this.LogoICO.Height.Value));

                            string base64String = Convert.ToBase64String((byte[]) new System.Drawing.ImageConverter().ConvertTo(newImg, typeof(byte[])));
                            //string base64String = this.UsuarioEmpresa.Logo == null ? string.Empty : Convert.ToBase64String(this.UsuarioEmpresa.Logo, 0, this.UsuarioEmpresa.Logo.Length);
                            if (base64String != string.Empty)
                            {
                                this.LogoICO.ImageUrl = "data:image/png;base64," + base64String;
                                this.LogoICO.Width = newImg.Width;
                                this.LogoICO.Height = newImg.Height;
                                this.LogoICO.AlternateText = this.UsuarioEmpresa.Empresa;
                                this.LogoICO.Visible = true;
                            }
                        }
                        //if (ExpedientesF.ExpedientesValidarExpedientesPendientes(this.UsuarioActivo))
                        //    this.MensajesAlertas.Text = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.MensajesAlertas).ParametroValor;
                        //else
                        //    this.MensajesAlertas.Text = string.Empty;
                    }
                    else
                    {
                        Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/IngresoSistema.aspx"), true);
                    }
                    //this.Menu.Text = men.Menu;
                    string ubicacion = string.Empty;
                    //if (men.IdMenuPadre> 0 && UsuarioActivo.Menues.Exists(x => x.IdMenu == men.IdMenuPadre))
                    //    ubicacion = string.Concat(this.UsuarioActivo.Menues.Find(x => x.IdMenu == men.IdMenuPadre).Menu, ">>");
                    ubicacion = AyudaProgramacion.ObtenerUbicacion(men, this.UsuarioActivo, string.Empty);
                    this.lblMenuPosicion.Text = ubicacion;
                }
                this.PageLoadEvent(sender, e);
            }
            catch (Exception ex)
            {
                this.Mensaje.Text = ex.Message;
                this.Mensaje.ForeColor = System.Drawing.Color.Red;
            }
            
        }
        
        virtual protected void PageLoadEvent(object sender, System.EventArgs e) { }

        // Override the OnPreRender method to set _message to
        // a default value if it is null.
        protected override void OnPreRender(EventArgs e)
        {            
            base.OnPreRender(e);
            AyudaProgramacion.DeshabilitarControlesSeguros(this, this.paginaActual);
        }

        protected override void InitializeCulture()
        {
            if(nfi !=null)
                miInitializeCulture(nfi);
        }

        protected void miInitializeCulture(NumberFormatInfo nfi)
        {
                var newCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();                
            newCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            newCulture.NumberFormat = nfi;
            System.Threading.Thread.CurrentThread.CurrentCulture = newCulture;
                System.Threading.Thread.CurrentThread.CurrentUICulture = newCulture;            
        }

        public void SetInitializeCulture(string symbol)
        {
            var newCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            var prevSymbol = nfi.CurrencySymbol;
            nfi.CurrencySymbol = symbol;
            newCulture.NumberFormat = nfi;
            System.Threading.Thread.CurrentThread.CurrentCulture = newCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = newCulture;
            List<Control> lstCurrency = new List<Control>();
            lstCurrency = GetAllControls(lstCurrency, typeof(CurrencyTextBox), this);
            CurrencyTextBox ctrl;
            foreach (Control c in lstCurrency)
            {
                ctrl = (CurrencyTextBox)c;
                if (ctrl.Prefix.Trim().Length > 0) {
                    ctrl.Text = ctrl.Text.Replace(prevSymbol, symbol);
                    ctrl.Prefix = symbol;
                }
            }
            List<Control> lstLabel = new List<Control>();
            lstLabel = GetAllControls(lstLabel, typeof(Label), this);
            Label ctrlLabel;
            foreach (Control c in lstLabel)
            {
                ctrlLabel = (Label)c;
                if (ctrlLabel.CssClass.Contains("MonedagblSymbolo"))
                {
                    ctrlLabel.Text = ctrlLabel.Text.Replace(prevSymbol, symbol);
                   // ctrlLabel.Prefix = symbol;
                  
                }
            }
            StringBuilder script = new StringBuilder();
            script.AppendLine("$(document).ready(function () {");
            script.AppendFormat("SetInitializeCulture('{0}');", symbol);
            script.AppendLine("});");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SetInitializeCultureScript", script.ToString(), true);
        }

        /// <summary>
        /// Guarda el valor de una propiedad para poder recuperarlo después de un postback
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pClave"></param>
        /// <param name="pObjeto"></param>
        public void PropiedadGuardarValor(string pClave, object pObjeto)
        {
            Session[this.MiSessionPagina + pClave] = pObjeto;
        }

        /// <summary>
        /// Recupera el valor de una propiedad guardada después de un postback
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pClave"></param>
        /// <returns></returns>
        public T PropiedadObtenerValor<T>(string pClave) where T : new()
        {
            if (Session[this.MiSessionPagina + pClave] != null)
                return (T)Session[this.MiSessionPagina + pClave];
            else
                return new T();
        }

        protected void ControlesSeguros(Menues pPaginaActual)
        {
            Menues menu = this.UsuarioActivo.Menues.Find(delegate(Menues me)
            { return me.URL.Equals(paginaActual.URL);});

            if (menu != null)
            {
                foreach (SegControlesPaginas ctrl in menu.ControlesPaginas)
                {
                    Control control = BuscarControlRecursivo(this, ctrl.ControlesPaginas);
                    if (control != null)
                    {
                        if (control is Button)
                            ((Button)control).Visible = ctrl.TienePermiso;
                        if (control is TextBox)
                            ((TextBox)control).Enabled = ctrl.TienePermiso;
                        if (control is DropDownList)
                            ((DropDownList)control).Enabled = ctrl.TienePermiso;
                        if (control is CheckBox)
                            ((CheckBox)control).Enabled = ctrl.TienePermiso;
                    }
                }
            }
        }

        /// <summary>
        /// Finds a Control recursively. Note finds the first match and exists
        /// </summary>
        /// <param name="ContainerCtl"></param>
        /// <param name="IdToFind"></param>
        /// <returns></returns>
        public Control BuscarControlRecursivo(Control Root, string Id)
        {
            if (Root.ID == Id)
                return Root;

            foreach (Control Ctl in Root.Controls)
            {
                Control FoundCtl = BuscarControlRecursivo(Ctl, Id);
                if (FoundCtl != null)
                    return FoundCtl;
            }
            return null;
        }

        public static List<Control> GetAllControls(List<Control> controls, Type t, Control parent /* can be Page */)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.GetType() == t)
                    controls.Add(c);
                if (c.HasControls())
                    controls = GetAllControls(controls, t, c);
            }
            return controls;
        }

        /// <summary>
        /// Establece el estado de los controles y los controles incluidos en la 
        /// coleccion de controles
        /// </summary>
        /// <param name="pControl"></param>
        /// <param name="pEstado"></param>
        //public void HabilitarControles(Control pControl, bool pEstado)
        //{
        //    bool estado;
        //    Menues menu = this.UsuarioActivo.Menues.Find(delegate(Menues me)
        //    { return me.URL.Equals(paginaActual.URL);});

        //    foreach (Control ctrl in pControl.Controls)
        //    {
        //        SegControlesPaginas ctrlSeguro = menu.ControlesPaginas.Find(delegate(SegControlesPaginas ctrlSeg)
        //        { return ctrlSeg.ControlesPaginas == ctrl.ID;});

        //        if (ctrlSeguro != null && !ctrlSeguro.TienePermiso)
        //            estado = false;
        //        else
        //            estado = pEstado;

        //        if (ctrl is Button && ctrl.ID != "btnCancelar")
        //            ((Button)ctrl).Enabled = estado;
        //        if (ctrl is Image)
        //            ((Image)ctrl).Visible = estado;
        //        if (ctrl is TextBox)
        //            ((TextBox)ctrl).Enabled = estado;
        //        if (ctrl is CheckBox)
        //            ((CheckBox)ctrl).Enabled = estado;
        //        if (ctrl is DropDownList)
        //            ((DropDownList)ctrl).Enabled = estado;
        //        if (ctrl is TreeView)
        //            ((TreeView)ctrl).Enabled = estado;

        //        if (ctrl.Controls != null && ctrl.Controls.Count > 0)
        //            HabilitarControles(ctrl, pEstado);
        //    }
        //}

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
        /// Muestra un mensaje en el Sistema. Si pError=True lo muestra en color rojo.
        /// </summary>
        /// <param name="codigoMensaje"></param>
        /// <param name="pError"></param>
        public void MostrarMensaje(string codigoMensaje, bool pError)
        {
            this.MostrarMensaje(codigoMensaje, pError, new List<string>());
        }

        public void MostrarMensaje(string codigoMensaje, bool pError, List<string> parametrosMensaje)
        {
            string mensaje = this.ObtenerMensajeSistema(codigoMensaje);
            if (parametrosMensaje.Count > 0)
                mensaje = string.Format(mensaje, parametrosMensaje.ToArray());
            Mensajes ctrMensajes = (Mensajes)this.MaestraPrincipal.FindControl("popUpMensajes");
            ctrMensajes.MostrarMensaje(mensaje, pError);
        }

        public void MostrarMensaje(string codigoMensaje, System.Drawing.Color pColor, bool pBold, List<string> parametrosMensaje)
        {
            string mensaje = this.ObtenerMensajeSistema(codigoMensaje);
            if (parametrosMensaje.Count > 0)
                mensaje = string.Format(mensaje, parametrosMensaje.ToArray());
            Mensajes ctrMensajes = (Mensajes)this.MaestraPrincipal.FindControl("popUpMensajes");
            ctrMensajes.MostrarMensaje(mensaje, pColor, pBold);
        }

        public void ProgressBarMensajesOpcionales(string msg)
        {
            ProgressBar ctrMensajes = (ProgressBar)this.MaestraPrincipal.FindControl("poupProgressBar");
            ctrMensajes.MensajesOpcionales(msg);
        }

        /// <summary>
        /// Ordena una lista, guarda los valores del ordenamiento y acomoda los indices
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pLista"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        protected List<T> OrdenarGrillaDatos<T>(List<T> pLista,GridViewSortEventArgs e)
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

        /// <summary>
        /// Validar si un control tiene permisos para ir a una página
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="NombreControl"></param>
        /// <returns></returns>
        public bool ValidarPermiso(string NombrePagina)
        {
            string pagina = string.Empty;
            if (this.UsuarioActivo.Menues.Exists(x => (string)x.URL.Split('/').GetValue(x.URL.Split('/').Length - 1) == NombrePagina))
                return true;
            //foreach (Menues men in this.UsuarioActivo.Menues)
            //{
            //    pagina = (string)men.URL.Split('/').GetValue(men.URL.Split('/').Length - 1);
            //    if (pagina == NombrePagina)
            //        return true;
            //}
            return false;
        }

        /// <summary>
        /// Validar si un control tiene permisos en la pagina actual
        /// </summary>
        /// <param name="NombreControl"></param>
        /// <returns></returns>
        public bool ValidarPermisoControl(string NombreControl)
        {
            SegControlesPaginas control = this.paginaActual.ControlesPaginas.Find(x => x.ControlesPaginas == NombreControl);
            if (control != null && !control.TienePermiso)
                return false;
            else
                return true;
        }

        /// <summary>
        /// Guardar los valores de busqueda seteados por el usuario
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pObjeto"></param>
        public void BusquedaParametrosGuardarValor<T>(T pObjeto)
        {
            BusquedaParametrosGuardarValor<T>(pObjeto, string.Empty);
        }

        public void BusquedaParametrosGuardarValor<T>(T pObjeto, string key)
        {
            this.Session[this.MiSessionPagina + this.paginaActual.URL + key] = pObjeto;
        }

        /// <summary>
        /// Obtiene los valores de busqueda seteados por el usuario
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T BusquedaParametrosObtenerValor<T>(string key) where T : new()
        {
            if (this.Session[this.MiSessionPagina + this.paginaActual.URL + key] != null)
            {
                Objeto parametros = (Objeto)this.Session[this.MiSessionPagina + this.paginaActual.URL + key];
                if (parametros is T)
                    return (T)this.Session[this.MiSessionPagina + this.paginaActual.URL + key];
                else
                    return new T();
            }
            else
                return new T();
        }

        public T BusquedaParametrosObtenerValor<T>() where T : new()
        {
            return BusquedaParametrosObtenerValor<T>(string.Empty);
        }

        //private string MiSessionPagina
        //{
        //    get
        //    {
        //        if (this.ViewState[this.paginaActual.Menu] == null)
        //            this.ViewState[this.paginaActual.Menu] = Guid.NewGuid().ToString();
        //        return (string)this.ViewState[this.paginaActual.Menu];
        //    }
        //    set { this.ViewState[this.paginaActual.Menu] = value; }
        //}

        public Hashtable MisParametrosUrl
        {
            get
            {
                if (Session[this.MiSessionPagina + "PaginaSeguraMisParametrosUrl"] != null)
                { return (Hashtable)Session[this.MiSessionPagina + "PaginaSeguraMisParametrosUrl"]; }
                else
                { return new Hashtable(); }
            }
            set { Session[this.MiSessionPagina + "PaginaSeguraMisParametrosUrl"] = value; }
        }

        protected string ObtenerAppPath()
        {
            return this.Page.Request.ApplicationPath.EndsWith("/") ? this.Page.Request.ApplicationPath : string.Concat(this.Page.Request.ApplicationPath, "/");
        }

        /// <summary>
        /// Save ViewState in Session object
        /// </summary>
        protected override PageStatePersister PageStatePersister
        {
            get
            {
                //return new SessionPageStatePersister(Page);
                return new StreamPageStatePersister(Page);
            }
        }

        protected ScriptManager toolkitScriptManager
        {
            get { return (ScriptManager)this.MaestraPrincipal.FindControl("ScriptManager1"); }
            set
            {
                ScriptManager _tsmanager = (ScriptManager)this.MaestraPrincipal.FindControl("ScriptManager1");
                _tsmanager = value;
            }
        }
    }
}
