﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// Microsoft.VSDesigner generó automáticamente este código fuente, versión=4.0.30319.42000.
// 
#pragma warning disable 1591

namespace Facturas.ar.com.evol.erp {
    using System.Diagnostics;
    using System;
    using System.Xml.Serialization;
    using System.ComponentModel;
    using System.Web.Services.Protocols;
    using System.Web.Services;
    using System.Data;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="WSEvolSoap", Namespace="https://erp.evol.com.ar/ws")]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(object[]))]
    public partial class WSEvol : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback ObtenerAutenticacionOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public WSEvol() {
            this.Url = global::Facturas.Properties.Settings.Default.Facturas_ar_com_evol_erp_WSEvol;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event ObtenerAutenticacionCompletedEventHandler ObtenerAutenticacionCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("https://erp.evol.com.ar/ws/ObtenerAutenticacion", RequestNamespace="https://erp.evol.com.ar/ws", ResponseNamespace="https://erp.evol.com.ar/ws", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public AfipServiciosWebTickets ObtenerAutenticacion(Objeto pParametro) {
            object[] results = this.Invoke("ObtenerAutenticacion", new object[] {
                        pParametro});
            return ((AfipServiciosWebTickets)(results[0]));
        }
        
        /// <remarks/>
        public void ObtenerAutenticacionAsync(Objeto pParametro) {
            this.ObtenerAutenticacionAsync(pParametro, null);
        }
        
        /// <remarks/>
        public void ObtenerAutenticacionAsync(Objeto pParametro, object userState) {
            if ((this.ObtenerAutenticacionOperationCompleted == null)) {
                this.ObtenerAutenticacionOperationCompleted = new System.Threading.SendOrPostCallback(this.OnObtenerAutenticacionOperationCompleted);
            }
            this.InvokeAsync("ObtenerAutenticacion", new object[] {
                        pParametro}, this.ObtenerAutenticacionOperationCompleted, userState);
        }
        
        private void OnObtenerAutenticacionOperationCompleted(object arg) {
            if ((this.ObtenerAutenticacionCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ObtenerAutenticacionCompleted(this, new ObtenerAutenticacionCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(AfipServiciosWebTickets))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(TGETiposFuncionalidades))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://erp.evol.com.ar/ws")]
    public partial class Objeto {
        
        private bool errorAccesoDatosField;
        
        private string errorExceptionField;
        
        private bool bajaLogicaField;
        
        private TGEEstados estadoField;
        
        private System.DateTime fechaEventoField;
        
        private string codigoMensajeField;
        
        private string[] codigoMensajeArgsField;
        
        private int hashTransaccionField;
        
        private string objetoXMLField;
        
        private bool busquedaParametrosField;
        
        private string linkField;
        
        private UsuarioLogueado usuarioLogueadoField;
        
        private bool confirmarAccionField;
        
        private ConfirmarMensajes[] confirmaMensajesField;
        
        private System.Data.DataSet dsResultadoField;
        
        private string filtroField;
        
        /// <remarks/>
        public bool ErrorAccesoDatos {
            get {
                return this.errorAccesoDatosField;
            }
            set {
                this.errorAccesoDatosField = value;
            }
        }
        
        /// <remarks/>
        public string ErrorException {
            get {
                return this.errorExceptionField;
            }
            set {
                this.errorExceptionField = value;
            }
        }
        
        /// <remarks/>
        public bool BajaLogica {
            get {
                return this.bajaLogicaField;
            }
            set {
                this.bajaLogicaField = value;
            }
        }
        
        /// <remarks/>
        public TGEEstados Estado {
            get {
                return this.estadoField;
            }
            set {
                this.estadoField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime FechaEvento {
            get {
                return this.fechaEventoField;
            }
            set {
                this.fechaEventoField = value;
            }
        }
        
        /// <remarks/>
        public string CodigoMensaje {
            get {
                return this.codigoMensajeField;
            }
            set {
                this.codigoMensajeField = value;
            }
        }
        
        /// <remarks/>
        public string[] CodigoMensajeArgs {
            get {
                return this.codigoMensajeArgsField;
            }
            set {
                this.codigoMensajeArgsField = value;
            }
        }
        
        /// <remarks/>
        public int HashTransaccion {
            get {
                return this.hashTransaccionField;
            }
            set {
                this.hashTransaccionField = value;
            }
        }
        
        /// <remarks/>
        public string ObjetoXML {
            get {
                return this.objetoXMLField;
            }
            set {
                this.objetoXMLField = value;
            }
        }
        
        /// <remarks/>
        public bool BusquedaParametros {
            get {
                return this.busquedaParametrosField;
            }
            set {
                this.busquedaParametrosField = value;
            }
        }
        
        /// <remarks/>
        public string Link {
            get {
                return this.linkField;
            }
            set {
                this.linkField = value;
            }
        }
        
        /// <remarks/>
        public UsuarioLogueado UsuarioLogueado {
            get {
                return this.usuarioLogueadoField;
            }
            set {
                this.usuarioLogueadoField = value;
            }
        }
        
        /// <remarks/>
        public bool ConfirmarAccion {
            get {
                return this.confirmarAccionField;
            }
            set {
                this.confirmarAccionField = value;
            }
        }
        
        /// <remarks/>
        public ConfirmarMensajes[] ConfirmaMensajes {
            get {
                return this.confirmaMensajesField;
            }
            set {
                this.confirmaMensajesField = value;
            }
        }
        
        /// <remarks/>
        public System.Data.DataSet dsResultado {
            get {
                return this.dsResultadoField;
            }
            set {
                this.dsResultadoField = value;
            }
        }
        
        /// <remarks/>
        public string Filtro {
            get {
                return this.filtroField;
            }
            set {
                this.filtroField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://erp.evol.com.ar/ws")]
    public partial class TGEEstados {
        
        private int idEstadoField;
        
        private string descripcionField;
        
        private string codigoMensajeField;
        
        private UsuarioLogueado usuarioLogueadoField;
        
        private TGETiposFuncionalidades[] tiposFuncionalidadesField;
        
        /// <remarks/>
        public int IdEstado {
            get {
                return this.idEstadoField;
            }
            set {
                this.idEstadoField = value;
            }
        }
        
        /// <remarks/>
        public string Descripcion {
            get {
                return this.descripcionField;
            }
            set {
                this.descripcionField = value;
            }
        }
        
        /// <remarks/>
        public string CodigoMensaje {
            get {
                return this.codigoMensajeField;
            }
            set {
                this.codigoMensajeField = value;
            }
        }
        
        /// <remarks/>
        public UsuarioLogueado UsuarioLogueado {
            get {
                return this.usuarioLogueadoField;
            }
            set {
                this.usuarioLogueadoField = value;
            }
        }
        
        /// <remarks/>
        public TGETiposFuncionalidades[] TiposFuncionalidades {
            get {
                return this.tiposFuncionalidadesField;
            }
            set {
                this.tiposFuncionalidadesField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://erp.evol.com.ar/ws")]
    public partial class UsuarioLogueado {
        
        private int idUsuarioField;
        
        private int idUsuarioEventoField;
        
        private string usuarioField;
        
        private string usuarioAS400Field;
        
        private string usuarioEventoField;
        
        private string apellidoField;
        
        private string nombreField;
        
        private string apellidoNombreField;
        
        private string correoElectronicoField;
        
        private string codigoMensajeField;
        
        private object[] codigoMensajeArgsField;
        
        private bool consultarAuditoriaField;
        
        private int idFilialEventoField;
        
        /// <remarks/>
        public int IdUsuario {
            get {
                return this.idUsuarioField;
            }
            set {
                this.idUsuarioField = value;
            }
        }
        
        /// <remarks/>
        public int IdUsuarioEvento {
            get {
                return this.idUsuarioEventoField;
            }
            set {
                this.idUsuarioEventoField = value;
            }
        }
        
        /// <remarks/>
        public string Usuario {
            get {
                return this.usuarioField;
            }
            set {
                this.usuarioField = value;
            }
        }
        
        /// <remarks/>
        public string UsuarioAS400 {
            get {
                return this.usuarioAS400Field;
            }
            set {
                this.usuarioAS400Field = value;
            }
        }
        
        /// <remarks/>
        public string UsuarioEvento {
            get {
                return this.usuarioEventoField;
            }
            set {
                this.usuarioEventoField = value;
            }
        }
        
        /// <remarks/>
        public string Apellido {
            get {
                return this.apellidoField;
            }
            set {
                this.apellidoField = value;
            }
        }
        
        /// <remarks/>
        public string Nombre {
            get {
                return this.nombreField;
            }
            set {
                this.nombreField = value;
            }
        }
        
        /// <remarks/>
        public string ApellidoNombre {
            get {
                return this.apellidoNombreField;
            }
            set {
                this.apellidoNombreField = value;
            }
        }
        
        /// <remarks/>
        public string CorreoElectronico {
            get {
                return this.correoElectronicoField;
            }
            set {
                this.correoElectronicoField = value;
            }
        }
        
        /// <remarks/>
        public string CodigoMensaje {
            get {
                return this.codigoMensajeField;
            }
            set {
                this.codigoMensajeField = value;
            }
        }
        
        /// <remarks/>
        public object[] CodigoMensajeArgs {
            get {
                return this.codigoMensajeArgsField;
            }
            set {
                this.codigoMensajeArgsField = value;
            }
        }
        
        /// <remarks/>
        public bool ConsultarAuditoria {
            get {
                return this.consultarAuditoriaField;
            }
            set {
                this.consultarAuditoriaField = value;
            }
        }
        
        /// <remarks/>
        public int IdFilialEvento {
            get {
                return this.idFilialEventoField;
            }
            set {
                this.idFilialEventoField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://erp.evol.com.ar/ws")]
    public partial class ConfirmarMensajes {
        
        private string codigoMensajeField;
        
        private bool confirmarField;
        
        /// <remarks/>
        public string CodigoMensaje {
            get {
                return this.codigoMensajeField;
            }
            set {
                this.codigoMensajeField = value;
            }
        }
        
        /// <remarks/>
        public bool Confirmar {
            get {
                return this.confirmarField;
            }
            set {
                this.confirmarField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://erp.evol.com.ar/ws")]
    public partial class TGETiposFuncionalidades : Objeto {
        
        private int idTipoFuncionalidadField;
        
        private string tipoFuncionalidadField;
        
        /// <remarks/>
        public int IdTipoFuncionalidad {
            get {
                return this.idTipoFuncionalidadField;
            }
            set {
                this.idTipoFuncionalidadField = value;
            }
        }
        
        /// <remarks/>
        public string TipoFuncionalidad {
            get {
                return this.tipoFuncionalidadField;
            }
            set {
                this.tipoFuncionalidadField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.9032.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="https://erp.evol.com.ar/ws")]
    public partial class AfipServiciosWebTickets : Objeto {
        
        private int idLoginTicketField;
        
        private int uniqueIdField;
        
        private System.DateTime generationTimeField;
        
        private System.DateTime expirationTimeField;
        
        private string signField;
        
        private string tokenField;
        
        private string loginTicketResponseField;
        
        private string serviceField;
        
        /// <remarks/>
        public int IdLoginTicket {
            get {
                return this.idLoginTicketField;
            }
            set {
                this.idLoginTicketField = value;
            }
        }
        
        /// <remarks/>
        public int UniqueId {
            get {
                return this.uniqueIdField;
            }
            set {
                this.uniqueIdField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime GenerationTime {
            get {
                return this.generationTimeField;
            }
            set {
                this.generationTimeField = value;
            }
        }
        
        /// <remarks/>
        public System.DateTime ExpirationTime {
            get {
                return this.expirationTimeField;
            }
            set {
                this.expirationTimeField = value;
            }
        }
        
        /// <remarks/>
        public string Sign {
            get {
                return this.signField;
            }
            set {
                this.signField = value;
            }
        }
        
        /// <remarks/>
        public string Token {
            get {
                return this.tokenField;
            }
            set {
                this.tokenField = value;
            }
        }
        
        /// <remarks/>
        public string LoginTicketResponse {
            get {
                return this.loginTicketResponseField;
            }
            set {
                this.loginTicketResponseField = value;
            }
        }
        
        /// <remarks/>
        public string Service {
            get {
                return this.serviceField;
            }
            set {
                this.serviceField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    public delegate void ObtenerAutenticacionCompletedEventHandler(object sender, ObtenerAutenticacionCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.9032.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ObtenerAutenticacionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal ObtenerAutenticacionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public AfipServiciosWebTickets Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((AfipServiciosWebTickets)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591