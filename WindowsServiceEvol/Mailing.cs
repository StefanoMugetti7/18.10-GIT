using Mailing;
using Mailing.Entidades;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceProcess;

namespace WindowsServiceEvol
{
    public partial class Mailing : ServiceBase
    {
        bool ejecutando = false;
        const string origen = "Servicios Evol";
        public Mailing()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            //Debug
            //timerMailing.Interval = 5000;
            if (!EventLog.Exists(origen))
            {
                EventLog.CreateEventSource(origen, "Application");
            }
            timerMailing.Start();
            
        }

        protected override void OnStop()
        {
            timerMailing.Stop();
        }

        private void timerMailing_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (ejecutando) return;
                        
            try
            {
                ejecutando = true;
                //if (!EventLog.Exists(origen))
                //    EventLog.CreateEventSource(origen, "Application");

                //EventLog.WriteEntry(origen, "Se inicio el proceso de Mailing", EventLogEntryType.Information);
                List<TGEMailing> lista = MailingF.TGEMailingObtenerGenerarDatos();
                foreach (TGEMailing item in lista)
                {
                    if (!MailingF.TGEMailingGenerarDatosEnvios(item))
                    {
                        //EventLog.WriteEntry(origen, item.CodigoMensaje, EventLogEntryType.Error);
                    }
                }
                //EventLog.WriteEntry(origen, "Se finalizo el proceso de Mailing", EventLogEntryType.Information);
            }
            catch (Exception ex)
            {
                //EventLog.WriteEntry(origen, ex.Message, EventLogEntryType.Error);
            }
            finally {
                ejecutando = false;
            }
        }
    }
}
