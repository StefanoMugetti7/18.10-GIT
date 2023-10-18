using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Cobros.Entidades
{
    [Serializable]
    public partial class CobOrdenesCobrosPrestamos : Objeto
    {
        // Class CobOrdenesCobrosPrestamos
        #region "Private Members"
        int _idOrdenCobroPrestamo;
        int _idOrdenCobro;
        int _idPrestamo;
        #endregion

        #region "Constructors"
        public CobOrdenesCobrosPrestamos()
        {
        }
        #endregion

        #region "Public Properties"
        
        [PrimaryKey]
        public int IdOrdenCobroPrestamo
        {
            get { return _idOrdenCobroPrestamo; }
            set { _idOrdenCobroPrestamo = value; }
        }
        public int IdOrdenCobro
        {
            get { return _idOrdenCobro; }
            set { _idOrdenCobro = value; }
        }

        public int IdPrestamo
        {
            get { return _idPrestamo; }
            set { _idPrestamo = value; }
        }

        #endregion
    }
}