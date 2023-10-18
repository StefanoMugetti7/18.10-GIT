using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Compras.Entidades;
using Generales.Entidades;
using System.Xml;

namespace Facturas.Entidades
{
    [Serializable]
    public partial class VTARemitosDetalles : Objeto
    {
        #region "Private Members"

        int _idRemitoDetalle;
        int _idRemito;
        int? _idFacturaDetalle;
        CMPProductos _producto;
        decimal _cantidad;
        decimal _stockActual;
        decimal _cantidadEntregada;
        string _numeroRemitoCompleto;
        DateTime _fechaRemito;
        bool _incluir;
        bool _listaPrecio;
        int _idListaPrecioDetalle;
        TGEMonedas _Moneda;
        List<VTANotasPedidosDetalles> _notasPedidosDetalles;
        XmlDocumentSerializationWrapper _loteNotasPedidos;
        
        #endregion

        #region "Constructors"
        public VTARemitosDetalles()
        {
        }
        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdRemitoDetalle
        {
            get { return _idRemitoDetalle; }
            set { _idRemitoDetalle = value; }
        }

        public int IdRemito
        {
            get { return _idRemito; }
            set { _idRemito = value; }
        }

        public int? IdFacturaDetalle
        {
            get { return _idFacturaDetalle; }
            set { _idFacturaDetalle = value; }
        }

        public int IdListaPrecioDetalle
        {
            get { return _idListaPrecioDetalle; }
            set { _idListaPrecioDetalle = value; }
        }

        public CMPProductos Producto
        {
            get { return _producto == null ? (_producto = new CMPProductos()) : _producto; }
            set { _producto = value; }
        }

        public decimal Cantidad
        {
            get { return _cantidad; }
            set{ _cantidad = value; }
        }

        public decimal CantidadEntregada
        {
            get { return _cantidadEntregada; }
            set { _cantidadEntregada = value; }
        }

        public decimal StockActual
        {
            get { return _stockActual; }
            set { _stockActual = value; }
        }

        public string NumeroRemitoCompleto
        {
            get { return _numeroRemitoCompleto==null? string.Empty : _numeroRemitoCompleto; }
            set { _numeroRemitoCompleto = value; }
        }

        public DateTime FechaRemito
        {
            get { return _fechaRemito; }
            set { _fechaRemito = value; }
        }

        public bool Incluir
        {
            get { return _incluir; }
            set { _incluir = value; }
        }

        public bool ListaPrecio
        {
            get { return _listaPrecio; }
            set { _listaPrecio = value; }
        }

        string _descripcion;
        public string Descripcion {
            get { return _descripcion == null ? string.Empty : _descripcion; }
            set { _descripcion = value; }
        }
        public TGEMonedas Moneda
        {
            get { return _Moneda == null ? (_Moneda = new TGEMonedas()) : _Moneda; }
            set { _Moneda = value; }
        }

        public XmlDocument LoteNotasPedidos
        {
            get { return _loteNotasPedidos; }
            set { _loteNotasPedidos = value; }
        }
        public List<VTANotasPedidosDetalles> NotasPedidosDetalles
        {
            get { return _notasPedidosDetalles == null ? (_notasPedidosDetalles = new List<VTANotasPedidosDetalles>()) : _notasPedidosDetalles; }
            set { _notasPedidosDetalles = value; }
        }

        public string TipoNumeroFactura { get; set; }
        public decimal? PrecioUnitario { get; set; }
        public decimal? CantidadRestante { get; set; }
        public bool NoIncluidoEnAcopio { get; set; }
        #endregion
    }
}
