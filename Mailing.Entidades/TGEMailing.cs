using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Mailing.Entidades
{
    [Serializable]
    public partial class TGEMailing : Objeto
    {
        #region "Private Members"
        TGEMailingProcesos _mailingProcesos;
        TGEListasValoresDetalles _listasValoresDetalles;
        TGEListasValores _listasValores;
        TGEPlantillas _plantillas;
        TGEMailingProcesamiento _mailingProcesamiento;
        XmlDocumentSerializationWrapper _loteMailingEnvioManual;
        XmlDocumentSerializationWrapper _loteMailingParametros;
        List<TGEMailingAdjuntos> _adjuntos;
        List<TGEMailingProcesamientosAdjuntos> _procesamientosAdjuntos;
        DataTable _detalleEnvio;
        DataTable _detalleEnvioProcesamiento;
        string _asunto;
        

        #endregion

        #region "Constructors"
        public TGEMailing()
        {
        }

        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdMailing { get; set;}

        public string Codigo { get; set; }
        public string Asunto
        {
            get { return _asunto; }
            set { _asunto = value; }
        }
        public TGEMailingProcesos MailingProcesos
        {
            get { return _mailingProcesos == null ? (_mailingProcesos = new TGEMailingProcesos()) : _mailingProcesos; }
            set { _mailingProcesos = value; }
        }

        public TGEMailingProcesamiento MailingProcesamiento
        {
            get { return _mailingProcesamiento == null ? (_mailingProcesamiento = new TGEMailingProcesamiento()) : _mailingProcesamiento; }
            set { _mailingProcesamiento = value; }
        }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public int IdPeriocididad { get; set; }

        public int DiaEjecucion { get; set; }

        public int IdEstado { get; set; }

        public string Descripcion { get; set; }

        public DateTime FechaAlta { get; set; }

        public TGEListasValoresDetalles ListasValoresDetalles
        {
            get { return _listasValoresDetalles == null ? (_listasValoresDetalles = new TGEListasValoresDetalles()) : _listasValoresDetalles; }
            set { _listasValoresDetalles = value; }
        }

        public TGEListasValores ListasValores
        {
            get { return _listasValores == null ? (_listasValores = new TGEListasValores()) : _listasValores; }
            set { _listasValores = value; }
        }

        public TGEPlantillas Plantillas
        {
            get { return _plantillas == null ? (_plantillas = new TGEPlantillas()) : _plantillas; }
            set { _plantillas = value; }
        }

        public XmlDocument LoteMailingEnvioManual
        {
            get { return _loteMailingEnvioManual; }
            set { _loteMailingEnvioManual = value; }
        }
        public XmlDocument LoteMailingParametros
        {
            get { return _loteMailingParametros; }
            set { _loteMailingParametros = value; }
        }

        public List<TGEMailingAdjuntos> MailingAdjuntos
        {
            get { return _adjuntos == null ? (_adjuntos = new List<TGEMailingAdjuntos>()) : _adjuntos; }
            set { _adjuntos = value; }
        }
        public List<TGEMailingProcesamientosAdjuntos> MailingProcesamientosAdjuntos
        {
            get { return _procesamientosAdjuntos == null ? (_procesamientosAdjuntos = new List<TGEMailingProcesamientosAdjuntos>()) : _procesamientosAdjuntos; }
            set { _procesamientosAdjuntos = value; }
        }


        //public TGEMailingProcesamientosAdjuntos MailingProcesamientosAdjuntos
        //{
        //    get { return _procesamientosAdjuntos == null ? (_procesamientosAdjuntos = new TGEMailingProcesamientosAdjuntos()) : _procesamientosAdjuntos; }
        //    set { _procesamientosAdjuntos = value; }
        //}

        public DataTable DetalleEnvio
        {
            get { return _detalleEnvio == null ? (_detalleEnvio = new DataTable()) : _detalleEnvio; }
            set { _detalleEnvio = value; }
        }
        public DataTable DetalleEnvioProcesamiento
        {
            get { return _detalleEnvioProcesamiento == null ? (_detalleEnvioProcesamiento = new DataTable()) : _detalleEnvioProcesamiento; }
            set { _detalleEnvioProcesamiento = value; }
        }
        #endregion


    }
}
