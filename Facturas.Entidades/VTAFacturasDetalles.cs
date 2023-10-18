using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generales.Entidades;
using Comunes.Entidades;
using Compras.Entidades;
using System.Xml;
using Contabilidad.Entidades;

namespace Facturas.Entidades
{
    [Serializable]
    public class VTAFacturasDetalles : Objeto
    {

        #region "Private Members"
       
        int _idFacturaDetalle;
        int _idFactura;
        //int? _idItemRemito;
        //int? _idProducto;
        //string _descripcion;
        
        //CMPProductos _producto;
        CMPListasPreciosDetalles _listaPrecioDetalle;
        decimal? _cantidad;
        decimal? _precioUnitarioSinIva;
        //decimal? _aliCuotaIVA;
        decimal? _descuentoPorcentual;
        decimal? _descuentoImporte;
        decimal? _subTotal;
        decimal? _subTotalConIva;
        decimal? _importeIVA;
        TGEIVA _IVA;
        string _descripcion;
        string _descripcionProducto;
        bool _detalleImportado;
        decimal? _cantidadEntregada;
        decimal? _cantidadRestante;
        bool _incluir;
        List<VTARemitosDetalles> _remitosDetalles;
        List<VTANotasPedidosDetalles> _notasPedidosDetalles;
        bool _modificaPrecio;
        string _tabla;
        Int64? _idRefTabla;
        int? _idNotaPedido;
        XmlDocumentSerializationWrapper _loteNotasPedidos;
        int _idNotaCreditoAsociadaDetalle;
        CtbCentrosCostosProrrateos _centroCostoProrrateo;
        XmlDocumentSerializationWrapper _loteRemitosDetalles;

        #endregion

        #region "Constructors"
        public VTAFacturasDetalles()
	    {
	    }
	    #endregion
		
	    #region "Public Properties"

        [PrimaryKey()]
        public int IdFacturaDetalle
        {
            get { return _idFacturaDetalle; }
            set {_idFacturaDetalle = value; }
        }

        public int IdFactura
        {
            get { return _idFactura;}
            set { _idFactura = value; }
        }
        public int IdNotaCreditoAsociadaDetalle
        {
            get { return _idNotaCreditoAsociadaDetalle; }
            set { _idNotaCreditoAsociadaDetalle = value; }
        }

        public XmlDocument LoteRemitosDetalles
        {
            get { return _loteRemitosDetalles; }
            set { _loteRemitosDetalles = value; }
        }
        //public int? IdItemRemito
        //{
        //    get { return _idItemRemito; }
        //    set { _idItemRemito = value; }
        //}

        //public int? IdProducto
        //{
        //    get { return _idProducto; }
        //    set { _idProducto = value; }
        //}

        //public string Descripcion
        //{
        //    get { return _descripcion; }
        //    set { _descripcion = value; }
        //}

        public decimal? Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }

        public decimal? PrecioUnitarioSinIva
        {
            get { return _precioUnitarioSinIva; }
            set { _precioUnitarioSinIva = value; }
        }

        //public decimal? AlicuotaIVA
        //{
        //    get { return _aliCuotaIVA; }
        //    set { _aliCuotaIVA = value; }
        //}

        public decimal? DescuentoPorcentual
        {
           // get { return _descuentoPorcentual == null ? 0 : _descuentoPorcentual; }
            get { return _descuentoPorcentual ; }
            set { _descuentoPorcentual = value; }
        }

        public decimal? DescuentoImporte
        {
           // get { return _descuentoImporte == null ? 0 : _descuentoImporte; }
            get { return _descuentoImporte; }
            set { _descuentoImporte = value; }
        }

        public decimal? SubTotal
        {
            get { return _subTotal; }
            set { _subTotal = value; }
        }

        public decimal? SubTotalConIva
        {
            get { return _subTotalConIva; }
            set { _subTotalConIva = value; }
        }

        public decimal? ImporteIVA
        {
            get { return _importeIVA; }
            set { _importeIVA = value; }
        }

        public TGEIVA IVA
        {
            get { return _IVA == null ? (_IVA = new TGEIVA()) : _IVA; }
            set { _IVA = value; }
        }

        //public CMPProductos Producto
        //{
        //    get { return _producto == null ? (_producto = new CMPProductos()) : _producto; }
        //    set { _producto = value; }
        //}

        public CtbCentrosCostosProrrateos CentroCostoProrrateo
        {
            get { return _centroCostoProrrateo == null ? (_centroCostoProrrateo = new CtbCentrosCostosProrrateos()) : _centroCostoProrrateo; }
            set { _centroCostoProrrateo = value; }
        }

        public CMPListasPreciosDetalles ListaPrecioDetalle
        {
            get { return _listaPrecioDetalle == null ? (_listaPrecioDetalle = new CMPListasPreciosDetalles()) : _listaPrecioDetalle; }
            set { _listaPrecioDetalle = value; }
        }

        public string DescripcionProducto
        {
            get { return _descripcionProducto; }
            set { _descripcionProducto = value; }
        }

        public string Descripcion
        {
            get { return _descripcion == null ? string.Empty : _descripcion; }
            set { _descripcion = value; }
        }

        public bool DetalleImportado
        {
            get { return _detalleImportado; }
            set { _detalleImportado = value; }
        }

        public decimal? CantidadEntregada
        {
            get { return _cantidadEntregada; }
            set { _cantidadEntregada = value; }
        }

        public decimal? CantidadRestante
        {
            get { return _cantidadRestante; }
            set { _cantidadRestante = value; }
        }

        public bool Incluir
        {
            get { return _incluir; }
            set { _incluir = value; }
        }

        public List<VTARemitosDetalles> RemitosDetalles
        {
            get { return _remitosDetalles == null ? (_remitosDetalles = new List<VTARemitosDetalles>()) : _remitosDetalles; }
            set { _remitosDetalles = value; }
        }

        public bool ModificaPrecio
        {
            get { return _modificaPrecio; }
            set { _modificaPrecio = value; }
        }

        public string Tabla
        {
            get { return _tabla == null ? string.Empty : _tabla; }
            set { _tabla = value; }
        }

        public Int64? IdRefTabla
        {
            get { return _idRefTabla; }
            set { _idRefTabla = value; }
        }

        public int? IdNotaPedido
        {
            get { return _idNotaPedido; }
            set { _idNotaPedido = value; }
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

        public bool EsFacturaCargos { get; set; }

    
        #endregion

    }
}
