
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Cobros.Entidades
{
    [Serializable]
    public partial class CobOrdenesCobrosFacturas : Objeto
    {
        // Class CobOrdenesCobrosFacturas
        #region "Private Members"
        int _idOrdenCobroFactura;
        int _idOrdenCobro;
        int _idFactura;
        decimal _importeCobroParcial;
        #endregion

        #region "Constructors"
        public CobOrdenesCobrosFacturas()
        {
        }
        #endregion

        #region "Public Properties"
        [PrimaryKey]
        public int IdOrdenCobroFactura
        {
            get { return _idOrdenCobroFactura; }
            set { _idOrdenCobroFactura = value; }
        }
        public int IdOrdenCobro
        {
            get { return _idOrdenCobro; }
            set { _idOrdenCobro = value; }
        }

        public int IdFactura
        {
            get { return _idFactura; }
            set { _idFactura = value; }
        }

        public decimal ImporteCobroParcial
        {
            get { return _importeCobroParcial; }
            set { _importeCobroParcial = value; }
        }

        #endregion
    }
}
