using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Net.Mail;
using Comunes.Entidades;
using System.Threading;
using System.Web.Caching;
using Auditoria;
using Servicio.Conectividad;
using Generales.Entidades;
using Servicio.AccesoDatos;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Generales.FachadaNegocio;

namespace IU.Modulos.Comunes
{
    public partial class EnviarMails : ControlesSeguros
    {
        private string SessionId
        {
            get { return (string)Session["ProcesosDatosModificarSessionId"]; }
            set { Session["ProcesosDatosModificarSessionId"] = value; }
        }

        public delegate void IniciarProcesoEventHandler(ref List<Objeto> listaEnvio);
        public event IniciarProcesoEventHandler IniciarProceso;

        public delegate bool ArmarMailEventHandler(Objeto item, MailMessage mail);
        public event ArmarMailEventHandler ArmarMail;

        public delegate void FinalizarProcesoEventHandler();
        public event FinalizarProcesoEventHandler FinalizarProceso;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this.RegistrarScript();
        }

        private void RegistrarScript()
        {
            // Register a Javascript include file during a partial postback
            ScriptManager.RegisterClientScriptInclude(this, this.GetType(),"EnviarMailsScript",ResolveUrl("~/Modulos/Comunes/EnviarMailsJS.js"));
        }

        protected void btnEnviarMailProceso_Click(object sender, EventArgs e)
        {
            ProgressBarDTO p = new ProgressBarDTO();
            p.number = 1;
            p.text = "";
            p.result = true;
            HttpRuntime.Cache.Insert(Session.SessionID + "CacheProgressBarDTOEnviarMails", p, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            //Thread workerThread = new Thread(new ParameterizedThreadStart(EjecutarProceso));
            //workerThread.CurrentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            //workerThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            //workerThread.Start(Session.SessionID);
            HttpContext ctx = HttpContext.Current;
            Thread t = new Thread(new ThreadStart(() =>
            {
                HttpContext.Current = ctx;
                EjecutarProceso(Session.SessionID);
            }));
            t.Start();
        }

        private void EjecutarProceso(object sesssionID)
        {
            ProgressBarDTO p = new ProgressBarDTO();
            p.result = true;
            try
            {
                this.SessionId = (string)sesssionID;                
                List<Objeto> listaEnvio = new List<Objeto>();
                List<string> errores = new List<string>();
                Int32 cantidadTotal = 0;

                TGEParametrosMails paramMail = new TGEParametrosMails();
                paramMail = TGEGeneralesF.ParametrosMailsObtenerDatosCompletos(paramMail);

                if (this.IniciarProceso != null)
                    this.IniciarProceso(ref listaEnvio);

                cantidadTotal = listaEnvio.Count;
                int contador = 0;
                int porcentajeProgreso = 1;
                string msgProgreso = "Enviando correos {0} de {1}";
                string extraData;
                if (cantidadTotal == 0)
                {
                    p.number = 100;
                    p.text = "No se encontraron mails para enviar";
                    p.result = false;
                    HttpRuntime.Cache.Insert(Session.SessionID + "CacheProgressBarDTOEnviarMails", p, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
                    return;
                }

                MailMessage mail;
                string direcciones = string.Empty;
                string separadorDireccciones = string.Empty;
                Servicio.Conectividad.Email emailLN = new Servicio.Conectividad.Email();
                foreach (Objeto item in listaEnvio)
                {
                    contador++;
                    extraData = string.Format(msgProgreso, contador.ToString(), cantidadTotal.ToString());
                    porcentajeProgreso = (contador * 100 / cantidadTotal);
                    porcentajeProgreso = porcentajeProgreso < 1 ? 1 : porcentajeProgreso > 95 ? 95 : porcentajeProgreso;
                    p.number = porcentajeProgreso;
                    p.text = extraData;
                    HttpRuntime.Cache.Insert(Session.SessionID + "CacheProgressBarDTOEnviarMails", p, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
                    mail = new MailMessage();
                    mail.From = new MailAddress(paramMail.DireccionCorreo, paramMail.Nombre);
                    mail.ReplyTo = new MailAddress(paramMail.DireccionCorreo, paramMail.Nombre);
                    if (this.ArmarMail != null)
                    {
                        if (this.ArmarMail(item, mail))
                        {
                            if (mail.To.Count > 0)
                            {
                                p.text = string.Concat(extraData, "<BR/>", mail.To[0].DisplayName);
                                HttpRuntime.Cache.Insert(Session.SessionID + "CacheProgressBarDTOEnviarMails", p, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
                                //if( !emailLN.Enviar(mail, item))
                                if (!AuditoriaF.MailsEnviosAgregar(mail, item, 13))
                                {
                                    foreach (MailAddress dir in mail.To)
                                    {
                                        direcciones = string.Concat(dir.DisplayName, " ", dir.Address);
                                        separadorDireccciones = " - ";
                                    }
                                    errores.Add(item.CodigoMensaje + " - " + item.ErrorException + " | " + direcciones);
                                }
                            }
                            else
                            {
                                errores.Add(item.Filtro + " - No tiene Correo");
                            }
                        }
                    }
                }
                p.number = 100;

                if (errores.Count > 0)
                {
                    p.text = "<BR/>Se enviaron " + contador + " de " + cantidadTotal + " Correos.<BR/><span style='color: red;'> Los siguientes Correos no se enviaron:</span><BR/>";
                    foreach (string s in errores)
                        p.text = string.Concat(p.text, s, "<BR>");
                }
                else
                {
                    p.text = "Los correos se enviaron de forma correcta.";
                }
                HttpRuntime.Cache.Insert(this.SessionId + "CacheProgressBarDTOEnviarMails", p, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            }
            catch (Exception ex)
            {
                p.result = false;
                p.number = 100;
                p.text = string.Concat( "<BR/><span style='color: red;'>El proceso no pudo completarse.</span><BR/ >", ex.Message, "<BR/ >");
                HttpRuntime.Cache.Insert(this.SessionId + "CacheProgressBarDTOEnviarMails", p, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            }
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            if (this.FinalizarProceso != null)
                this.FinalizarProceso();
        }
    }
}