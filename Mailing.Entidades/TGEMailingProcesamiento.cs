using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Mailing.Entidades
{
    [Serializable]
    public class TGEMailingProcesamiento : Objeto
    {
        public int IdMailingProcesamiento { get; set; }
        public int IdMailing { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
        public int Cantidad { get; set; }
        TGEMailingProcesos _proceso;
        TGEMailing _mailing;
        public int CantidadEnviada { get; set; }

        public int CantidadError { get; set; }

        public int CantidadPendiente { get; set; }
        XmlDocumentSerializationWrapper _mailsEnvios;

        public XmlDocument MailsEnvios
        {
            get { return _mailsEnvios; }
            set { _mailsEnvios = value; }
        }

        public bool EnvioManual { get; set; }

        public TGEMailingProcesos Proceso
        {
            get { return _proceso == null ? (_proceso = new TGEMailingProcesos()) : _proceso; }
            set { _proceso = value; }
        }

        

    }

}
