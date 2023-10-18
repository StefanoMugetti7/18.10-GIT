
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Compras.Entidades;
using Subsidios.Entidades;
using Generales.Entidades;
using Contabilidad.Entidades;
namespace CuentasPagar.Entidades
{
    [Serializable]
    public partial class CapSolicitudPagoDetalles : Objeto
    {

        #region "Private Members"
        int _idSolicitudPagoDetalle;
        int _idSolicitudPago;
        string _descripcion;
        string _descripcionProducto;
        decimal _cantidad;
        decimal _precioUnitarioSinIva;
        decimal _precioNoGravado;
        decimal _descuentoImporte;
        decimal _alicuotaIVA;
        TGEIVA _iVA;
        TGEFiliales _filial;
        CMPProductos _producto;
        CMPItemRemitos _itemRemito;
        SubSubsidios _subsidios;
        CmpOrdenesComprasDetalles _ordenCompraDetalle;
        bool _incluirEnSP;
        List<CmpInformesRecepcionesDetalles> _informesDetalles;
        int _idCentroCosto;
        CtbCentrosCostosProrrateos _centroCostoProrrateo;
        int _idComprobanteAsociadoDetalle;
        #endregion

        #region "Constructors"
        public CapSolicitudPagoDetalles()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdSolicitudPagoDetalle
        {
            get { return _idSolicitudPagoDetalle; }
            set { _idSolicitudPagoDetalle = value; }
        }
        public int IdSolicitudPago
        {
            get { return _idSolicitudPago; }
            set { _idSolicitudPago = value; }
        }

        public string Descripcion
        {
            get { return _descripcion == null ? string.Empty : _descripcion; }
            set { _descripcion = value; }
        }

        public decimal Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }

        public decimal PrecioUnitarioSinIva
        {
            get { return _precioUnitarioSinIva; }
            set { _precioUnitarioSinIva = value; }
        }

        public decimal PrecioNoGravado
        {
            get { return _precioNoGravado; }
            set { _precioNoGravado = value; }
        }

        public decimal DescuentoImporte
        {
            get { return _descuentoImporte; }
            set { _descuentoImporte = value; }
        }


        public decimal AlicuotaIVA
        {
            get { return _alicuotaIVA; }
            set { _alicuotaIVA = value; }
        }

        [Auditoria]
        public TGEIVA IVA
        {
            get { return _iVA == null ? (_iVA = new TGEIVA()) : _iVA; }
            set { _iVA = value; }
        }

        [Auditoria]
        public TGEFiliales Filial
        {
            get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
            set { _filial = value; }
        }

        public CtbCentrosCostosProrrateos CentroCostoProrrateo
        {
            get { return _centroCostoProrrateo == null ? (_centroCostoProrrateo = new CtbCentrosCostosProrrateos()) : _centroCostoProrrateo; }
            set { _centroCostoProrrateo = value; }
        }

        public CMPProductos Producto
        {
            get { return _producto == null ? (_producto = new CMPProductos()) : _producto; }
            set { _producto = value; }
        }

        public CMPItemRemitos ItemRemito
        {
            get { return _itemRemito == null ? (_itemRemito = new CMPItemRemitos()) : _itemRemito; }
            set { _itemRemito = value; }
        }

        public SubSubsidios Subsidio
        {
            get { return _subsidios == null ? (_subsidios = new SubSubsidios()) : _subsidios; }
            set { _subsidios = value; }
        }

        public CmpOrdenesComprasDetalles OrdenCompraDetalle
        {
            get { return _ordenCompraDetalle == null ? (_ordenCompraDetalle = new CmpOrdenesComprasDetalles()) : _ordenCompraDetalle; }
            set { _ordenCompraDetalle = value; }
        }

        public bool IncluirEnSP
        {
            get { return _incluirEnSP; }
            set { _incluirEnSP = value; }
        }

        //public decimal ImporteIva
        //{
        //    get { return _precioUnitarioSinIva * _alicuotaIVA / 100; }
        //}

        public decimal ImporteIvaTotal
        {
            get { return Math.Round(this.Subtotal * this.AlicuotaIVA / 100, 2); }
        }

        //public Decimal PrecioUnitario
        //{
        //    get { return this.PrecioUnitarioSinIva + ImporteIva; }
        //}

        /// <summary>
        /// No incluye el PrecioNoGravado
        /// </summary>
        public decimal Subtotal
        {
            get { return Math.Round((this.PrecioUnitarioSinIva * this.Cantidad) - DescuentoImporte, 2); }
        }

        public decimal PrecioTotalItem
        {
            get { return this.Subtotal + this.PrecioNoGravado + this.ImporteIvaTotal; }
        }

        //public decimal ObtenerPrecioSinIVA(decimal precio)
        //{
        //    return Math.Round( precio / (this.Producto.IVA.Alicuota / 100 + 1), 2);
        //}

        //public decimal ObtenerIVA(decimal precio)
        //{
        //    return Math.Round(precio - precio / (this.Producto.IVA.Alicuota / 100 + 1), 2);
        //}

        public List<CmpInformesRecepcionesDetalles> InformesDetalles
        {
            get { return _informesDetalles == null ? (_informesDetalles = new List<CmpInformesRecepcionesDetalles>()) : _informesDetalles; }
            set { _informesDetalles = value; }
        }

        public int IdCentroCosto
        {
            get { return _idCentroCosto; }
            set { _idCentroCosto = value; }
        }

        public int IdComprobanteAsociadoDetalle
        {
            get { return _idComprobanteAsociadoDetalle; }
            set { _idComprobanteAsociadoDetalle = value; }
        }

        public string DescripcionProducto
        {
            get { return _descripcionProducto; }
            set { _descripcionProducto = value; }
        }


        #endregion
    }
}
