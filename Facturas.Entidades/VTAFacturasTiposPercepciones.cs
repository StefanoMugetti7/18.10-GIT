using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Generales.Entidades;

namespace Facturas.Entidades
{
    [Serializable]
    public class VTAFacturasTiposPercepciones : Objeto
    {
        #region Private Members
        int _idFacturaTipoPercepcion;
        int _idFactura;
        TGETiposPercepciones _tipoPercepcion;
        decimal _porcentaje;
        decimal _importe;
        #endregion

        #region Constructors
        public VTAFacturasTiposPercepciones()
        {
        }
        #endregion

        #region Public Properties
        [PrimaryKey()]
        public int idFacturaTipoPercepcion
        {
            get { return _idFacturaTipoPercepcion; }
            set { _idFacturaTipoPercepcion = value; }
        }

        public int IdFactura
        {
            get { return _idFactura; }
            set { _idFactura = value; }
        }

        public TGETiposPercepciones TipoPercepcion
        {
            get { return _tipoPercepcion == null ? (_tipoPercepcion = new TGETiposPercepciones()): _tipoPercepcion; }
            set { _tipoPercepcion = value; }
        }

        public decimal Porcentaje
        {
            get { return _porcentaje; }
            set { _porcentaje = value; }
        }

        public decimal Importe
        {
            get { return _importe; }
            set { _importe = value; }
        }

        public int TributoId { get; set; }
        #endregion
    }
}
