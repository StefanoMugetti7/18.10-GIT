using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Cobros.Entidades
{
    [Serializable]
    public partial class CobOrdenesCobrosFacturasAnticipos : Objeto
    {
        #region "Private Members"
        int _idOrdenCobroFacturaAnticipo;
        int _idOrdenCobroFactura;
        int _idOrdenCobroAnticipo;
        decimal? _importe;
        #endregion

        #region "Constructors"
        public CobOrdenesCobrosFacturasAnticipos()
        {
        }
        #endregion

        #region "Public Properties"

        public int IdOrdenCobroFacturaAnticipo
        {
            get { return _idOrdenCobroFacturaAnticipo; }
            set { _idOrdenCobroFacturaAnticipo = value; }
        }
        public int IdOrdenCobroFactura
        {
            get { return _idOrdenCobroFactura; }
            set { _idOrdenCobroFactura = value; }
        }

        public int IdOrdenCobroAnticipo
        {
            get { return _idOrdenCobroAnticipo; }
            set { _idOrdenCobroAnticipo = value; }
        }

        public decimal? Importe
        {
            get { return _importe; }
            set { _importe = value; }
        }




        #endregion
    }
}
