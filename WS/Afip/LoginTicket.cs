using System;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Text;
//using Facturas.ar.gov.afip.wsaahomo;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using WS.ar.gov.afip.wsaa;

/// <summary> 
/// Clase para crear objetos Login Tickets 
/// </summary> 
/// <remarks> 
/// Ver documentacion: 
/// Especificacion Tecnica del Webservice de Autenticacion y Autorizacion 
/// Version 1.0 
/// Departamento de Seguridad Informatica - AFIP 
/// </remarks> 

namespace WS.Afip
{
    class LoginTicket
    {
        // Entero de 32 bits sin signo que identifica el requerimiento 
        public UInt32 UniqueId;
        // Momento en que fue generado el requerimiento 
        public DateTime GenerationTime;
        // Momento en el que exoira la solicitud 
        public DateTime ExpirationTime;
        // Identificacion del WSN para el cual se solicita el TA 
        public string Service;
        // Firma de seguridad recibida en la respuesta 
        public string Sign;
        // Token de seguridad recibido en la respuesta 
        public string Token;

        public string LoginTicketResponse;

        public XmlDocument XmlLoginTicketRequest = null;
        public XmlDocument XmlLoginTicketResponse = null;
        public string RutaDelCertificadoFirmante;
        public string XmlStrLoginTicketRequestTemplate = "<loginTicketRequest><header><uniqueId></uniqueId><generationTime></generationTime><expirationTime></expirationTime></header><service></service></loginTicketRequest>";

        private bool _verboseMode = false;

        // OJO! NO ES THREAD-SAFE 
        private static UInt32 _globalUniqueID = 0;

        public bool ObtenerLoginTicketResponse(Objeto pObjeto, string pathCertificado)
        {
            string argUrlWsaa = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.WSLogin).ParametroValor;
            if (string.IsNullOrEmpty(argUrlWsaa))
            {
                pObjeto.CodigoMensaje = "ErrorValidarWSLogin";
                return false;
            }
            //string argRutaCertX509Firmante = pFactura.AppPath + "Modulos\\Facturas\\FacturaElectronica\\CertificadoFELogin.pfx";
            string argRutaCertX509Firmante = pathCertificado;
            //string argRutaCertX509Firmante = pFactura.AppPath + "Modulos\\Facturas\\FacturaElectronica\\EvolCert.p12";
            bool argVerbose = false;
            return this.ObtenerLoginTicketResponse(pObjeto.Filtro, argUrlWsaa, argRutaCertX509Firmante, argVerbose);
        }

        /// <summary> 
        /// Construye un Login Ticket obtenido del WSAA 
        /// </summary> 
        /// <param name="argServicio">Servicio al que se desea acceder</param> 
        /// <param name="argUrlWsaa">URL del WSAA</param> 
        /// <param name="argRutaCertX509Firmante">Ruta del certificado X509 (con clave privada) usado para firmar</param> 
        /// <param name="argVerbose">Nivel detallado de descripcion? true/false</param> 
        /// <remarks></remarks> 
        public bool ObtenerLoginTicketResponse(string argServicio, string argUrlWsaa, string argRutaCertX509Firmante, bool argVerbose)
        {

            this.RutaDelCertificadoFirmante = argRutaCertX509Firmante;
            this._verboseMode = argVerbose;
            CertificadosX509Lib.VerboseMode = argVerbose;

            string cmsFirmadoBase64;
            string loginTicketResponse;

            XmlNode xmlNodoUniqueId;
            XmlNode xmlNodoGenerationTime;
            XmlNode xmlNodoExpirationTime;
            XmlNode xmlNodoService;

            // PASO 1: Genero el Login Ticket Request 
            try
            {
                XmlLoginTicketRequest = new XmlDocument();
                XmlLoginTicketRequest.LoadXml(XmlStrLoginTicketRequestTemplate);
                //XmlLoginTicketRequest.Load("~\\Templates\\TemplateFELogin.xml");

                xmlNodoUniqueId = XmlLoginTicketRequest.SelectSingleNode("//uniqueId");
                xmlNodoGenerationTime = XmlLoginTicketRequest.SelectSingleNode("//generationTime");
                xmlNodoExpirationTime = XmlLoginTicketRequest.SelectSingleNode("//expirationTime");
                xmlNodoService = XmlLoginTicketRequest.SelectSingleNode("//service");

                xmlNodoGenerationTime.InnerText = DateTime.Now.AddMinutes(-10).ToString("s");
                xmlNodoExpirationTime.InnerText = DateTime.Now.AddMinutes(+10).ToString("s");
                xmlNodoUniqueId.InnerText = Convert.ToString(_globalUniqueID);
                xmlNodoService.InnerText = argServicio;
                this.Service = argServicio;

                _globalUniqueID += 1;

                if (this._verboseMode)
                {
                    Console.WriteLine(XmlLoginTicketRequest.OuterXml);
                }
            }

            catch (Exception excepcionAlGenerarLoginTicketRequest)
            {
                throw new Exception("***Error GENERANDO el LoginTicketRequest : " + excepcionAlGenerarLoginTicketRequest.Message);
            }

            // PASO 2: Firmo el Login Ticket Request 
            try
            {
                if (this._verboseMode)
                {
                    Console.WriteLine("***Leyendo certificado: {0}", RutaDelCertificadoFirmante);
                }

                //X509Certificate2 certFirmante = CertificadosX509Lib.ObtieneCertificadoDesdeArchivo(RutaDelCertificadoFirmante);
                TGEParametrosValores contrasenia = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.WSCertificadoContrasenia);
                
                X509Certificate2 certFirmante = CertificadosX509Lib.ObtieneCertificadoDesdeArchivo(this.RutaDelCertificadoFirmante, contrasenia.ParametroValor.Trim());

                if (this._verboseMode)
                {
                    Console.WriteLine("***Firmando: ");
                    Console.WriteLine(XmlLoginTicketRequest.OuterXml);
                }

                // Convierto el login ticket request a bytes, para firmar 
                Encoding EncodedMsg = Encoding.UTF8;
                byte[] msgBytes = EncodedMsg.GetBytes(XmlLoginTicketRequest.OuterXml);

                // Firmo el msg y paso a Base64 
                byte[] encodedSignedCms = CertificadosX509Lib.FirmaBytesMensaje(msgBytes, certFirmante);
                cmsFirmadoBase64 = Convert.ToBase64String(encodedSignedCms);
            }

            catch (Exception excepcionAlFirmar)
            {
                throw new Exception("***Error FIRMANDO el LoginTicketRequest : " + excepcionAlFirmar.Message);
            }

            // PASO 3: Invoco al WSAA para obtener el Login Ticket Response 
            try
            {
                if (this._verboseMode)
                {
                    Console.WriteLine("***Llamando al WSAA en URL: {0}", argUrlWsaa);
                    Console.WriteLine("***Argumento en el request:");
                    Console.WriteLine(cmsFirmadoBase64);
                }

                //ClienteLoginCms_CS.Wsaa.LoginCMSService servicioWsaa = new ClienteLoginCms_CS.Wsaa.LoginCMSService();
                LoginCMSService servicioWsaa = new LoginCMSService();
                
                //servicioWsaa.Url = argUrlWsaa;
                loginTicketResponse = servicioWsaa.loginCms(cmsFirmadoBase64);

                if (this._verboseMode)
                {
                    Console.WriteLine("***LoguinTicketResponse: ");
                    Console.WriteLine(loginTicketResponse);
                }
            }

            catch (Exception excepcionAlInvocarWsaa)
            {
                throw new Exception("***Error INVOCANDO al servicio WSAA : " + excepcionAlInvocarWsaa.Message);
            }


            // PASO 4: Analizo el Login Ticket Response recibido del WSAA 
            try
            {
                XmlLoginTicketResponse = new XmlDocument();
                XmlLoginTicketResponse.LoadXml(loginTicketResponse);

                this.UniqueId = UInt32.Parse(XmlLoginTicketResponse.SelectSingleNode("//uniqueId").InnerText);
                this.GenerationTime = DateTime.Parse(XmlLoginTicketResponse.SelectSingleNode("//generationTime").InnerText);
                this.ExpirationTime = DateTime.Parse(XmlLoginTicketResponse.SelectSingleNode("//expirationTime").InnerText);
                this.Sign = XmlLoginTicketResponse.SelectSingleNode("//sign").InnerText;
                this.Token = XmlLoginTicketResponse.SelectSingleNode("//token").InnerText;
            }
            catch (Exception excepcionAlAnalizarLoginTicketResponse)
            {
                throw new Exception("***Error ANALIZANDO el LoginTicketResponse : " + excepcionAlAnalizarLoginTicketResponse.Message);
            }

            this.LoginTicketResponse = loginTicketResponse;

            return true;
        }
    }
}