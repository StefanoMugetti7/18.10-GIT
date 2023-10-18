using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace CuentasPagar.Entidades
{
    [Serializable]
    public partial class CapSolicitudPagoDetalleInformesRecpecionesDetalles : Objeto
    {
        #region Private Members
        int _idSolicitudPagoDetalleInformeRecpecionDetalle;
        int _idSolicitudPagoDetalle;
        int _idInformeRecepcionDetalle;
        decimal _cantidad;
        #endregion

        #region Constructors
        public CapSolicitudPagoDetalleInformesRecpecionesDetalles()
        {
        }
        #endregion

        #region Public Properties
        [PrimaryKey()]
        public int IdSolicitudPagoDetalleInformeRecpecionDetalle
        {
            get { return _idSolicitudPagoDetalleInformeRecpecionDetalle; }
            set { _idSolicitudPagoDetalleInformeRecpecionDetalle = value; }
        }
        [Auditoria()]
        public int IdSolicitudPagoDetalle
        {
            get { return _idSolicitudPagoDetalle; }
            set { _idSolicitudPagoDetalle = value; }
        }
        [Auditoria()]
        public int IdInformeRecepcionDetalle
        {
            get { return _idInformeRecepcionDetalle; }
            set { _idInformeRecepcionDetalle = value; }
        }
        [Auditoria()]
        public decimal Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }
        #endregion
    }
}
