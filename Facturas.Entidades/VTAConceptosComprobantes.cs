using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Facturas.Entidades
{
    [Serializable]
    public class VTAConceptosComprobantes : TGEListasValoresSistemasDetalles
    {
        int _idConceptoComprobante;

        [PrimaryKey()]
        public int IdConceptoComprobante
        {
            get { return _idConceptoComprobante; }
            set { _idConceptoComprobante = value; }
        }
    }
}
