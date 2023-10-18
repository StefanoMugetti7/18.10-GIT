using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace CuentasPagar.Entidades
{
    [Serializable()]
    public partial class CapOrdenesPagosSolicitudesPagosAnticipos : Objeto
    {
        #region Private Members
        int _idOrdenPagoSolicitudPagoAnticipo;
        int _idOrdenPago;
        int _idSolicitudPagoAnticipo;
        decimal _importe;
        #endregion
        #region Constructors
        public CapOrdenesPagosSolicitudesPagosAnticipos()
        {
        }
        #endregion
        #region Public Properties
        public int IdOrdenPagoSolicitudPagoAnticipo
        {
            get { return _idOrdenPagoSolicitudPagoAnticipo; }
            set { _idOrdenPagoSolicitudPagoAnticipo = value; }
        }

        public int IdOrdenPago
        {
            get { return _idOrdenPago; }
            set { _idOrdenPago = value; }
        }

        public int IdSolicitudPagoAnticipo
        {
            get { return _idSolicitudPagoAnticipo; }
            set { _idSolicitudPagoAnticipo = value; }
        }

        public decimal Importe
        {
            get { return _importe; }
            set { _importe = value; }
        }
        #endregion
    }
}
