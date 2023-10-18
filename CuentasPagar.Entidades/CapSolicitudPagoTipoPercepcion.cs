using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Generales.Entidades;

namespace CuentasPagar.Entidades
{
    [Serializable]
    public partial class CapSolicitudPagoTipoPercepcion : Objeto
    {
        #region Private Members
        int _idSolicitudPagoTipoPercepcion;
        int _idSolicitudPago;
        TGETiposPercepciones _tipoPercepcion;
        decimal _importe;
        #endregion

        #region Constructors
        public CapSolicitudPagoTipoPercepcion()
        {
        }
        #endregion

        #region Public Properties
        [PrimaryKey()]
        public int IdSolicitudPagoTipoPercepcion
        {
            get { return _idSolicitudPagoTipoPercepcion; }
            set { _idSolicitudPagoTipoPercepcion = value; }
        }

        public int IdSolicitudPago
        {
            get { return _idSolicitudPago; }
            set { _idSolicitudPago = value; }
        }

        public TGETiposPercepciones TipoPercepcion
        {
            get { return _tipoPercepcion == null ? (_tipoPercepcion = new TGETiposPercepciones()): _tipoPercepcion; }
            set { _tipoPercepcion = value; }
        }

        public decimal Importe
        {
            get { return _importe; }
            set { _importe = value; }
        }
        #endregion

    }
}
