﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AFIP.WebServices.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.4.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("https://servicios1.afip.gov.ar/wsfev1/service.asmx")]
        public string AFIP_WebServices_ar_gov_afip_servicios1_Service {
            get {
                return ((string)(this["AFIP_WebServices_ar_gov_afip_servicios1_Service"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("https://wsaa.afip.gov.ar/ws/services/LoginCms")]
        public string AFIP_WebServices_ar_gov_afip_wsaa_LoginCMSService {
            get {
                return ((string)(this["AFIP_WebServices_ar_gov_afip_wsaa_LoginCMSService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("https://aws.afip.gov.ar/sr-padron/webservices/personaServiceA5")]
        public string AFIP_WebServices_ar_gov_afip_aws_PersonaServiceA5 {
            get {
                return ((string)(this["AFIP_WebServices_ar_gov_afip_aws_PersonaServiceA5"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("https://erp.evol.com.ar/ws2/WSEvol.asmx")]
        public string AFIP_WebServices_ar_com_evol_erp_WSEvol {
            get {
                return ((string)(this["AFIP_WebServices_ar_com_evol_erp_WSEvol"]));
            }
        }
    }
}
