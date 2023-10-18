using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Compras.Entidades
{
    [Serializable]
    public partial class CmpInformesRecepcionesDetalles : Objeto
    {
        #region "Private Members"

        int _idInformeRecepcionDetalle;
        int _idInformeRecepcion;
        int? _idOrdenCompraDetalle;
        decimal _cantidadRecibida;
        decimal? _cantidadDevuelta;
        decimal? _cantidadCambio;
        //int _idProducto;
        //string _descripcion;
        decimal _cantidadPedida;
        decimal _cantidadPagada;
        CMPProductos _producto;
        decimal _cantidadPendiente;
        string _numeroRemitoCompleto;
        bool _incluir;
        #endregion
        
        #region "Constructors"
        public CmpInformesRecepcionesDetalles()
        { }
        #endregion

        #region "Public Properties"

        public int IdInformeRecepcionDetalle
        {
            get { return _idInformeRecepcionDetalle; }
            set { _idInformeRecepcionDetalle = value;}
        }

        public int IdInformeRecepcion
        {
            get { return _idInformeRecepcion; }
            set { _idInformeRecepcion = value;}
        }

        public int? IdOrdenCompraDetalle
        {
            get { return _idOrdenCompraDetalle; }
            set { _idOrdenCompraDetalle = value;}
        }

        public decimal CantidadRecibida
        {
            get { return _cantidadRecibida; }
            set { _cantidadRecibida = value;}
        }

        public decimal? CantidadDevuelta
        {
            get { return _cantidadDevuelta; }
            set {_cantidadDevuelta = value;}
        }

        public decimal? CantidadCambio
        {
            get { return _cantidadCambio; }
            set { _cantidadCambio = value;}
        }

        //public string Descripcion
        //{
        //    get { return _descripcion; }
        //    set { _descripcion = value; }
        //}

        public decimal CantidadPedida
        {
            get { return _cantidadPedida; }
            set {_cantidadPedida = value;}
        }

        public decimal CantidadPagada
        {
            get { return _cantidadPagada; }
            set { _cantidadPagada = value; }
        }

        public CMPProductos Producto
        {
            get { return _producto == null ? (_producto = new CMPProductos()) : _producto; }
            set { _producto = value; }
        }

        public decimal CantidadPendiente
        {
            get { return _cantidadPendiente; }
            set { _cantidadPendiente = value; }
        }

        public string NumeroRemitoCompleto
        {
            get { return _numeroRemitoCompleto; }
            set { _numeroRemitoCompleto = value; }
        }

        public bool Incluir
        {
            get { return _incluir; }
            set { _incluir = value; }
        }

        public string TipoNumeroFactura { get; set; }

        public int? IdSolicitudPagoDetalle { get; set; }

        public decimal? PrecioUnitario { get; set; }    
        public bool Importado { get; set; }
        #endregion
    }
}
