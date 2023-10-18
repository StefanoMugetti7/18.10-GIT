using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Cobros.Entidades
{
    [Serializable]
    public partial class CobOrdenesCobrosRemitos : Objeto
    {
        // Class CobOrdenesCobrosFacturas
        #region "Private Members"
        int _idOrdenCobroRemito;
        int _idOrdenCobro;
        int _idRemito;
        #endregion

        #region "Constructors"
        public CobOrdenesCobrosRemitos()
        {
        }
        #endregion

        #region "Public Properties"
        [PrimaryKey]
        public int IdOrdenCobroRemtio
        {
            get { return _idOrdenCobroRemito; }
            set { _idOrdenCobroRemito = value; }
        }
        public int IdOrdenCobro
        {
            get { return _idOrdenCobro; }
            set { _idOrdenCobro = value; }
        }

        public int IdRemito
        {
            get { return _idRemito; }
            set { _idRemito = value; }
        }

        #endregion
    }
}
