using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Generales.Entidades;

namespace Compras.Entidades
{
    [Serializable]
    public class CMPListasPreciosDetalles : Objeto
    {

        #region "Private Members" 

        int _idListaPrecioDetalle;
        //int? _idListaPrecio;
        //int? _idProducto;
        CMPProductos _producto;
        CMPListasPrecios _listaPrecio;
        int _idMoneda;
        string _descripcion;
        decimal _precio;
        DateTime? _fecha;
        int? _idFilial;
        decimal? _importeContado;
        decimal? _importe6Cuotas;
        decimal? _importe12Cuotas;
        decimal? _importe18Cuotas;
        decimal _stockActual;
        bool _precioEditable;
        decimal? _margenPorcentaje;
        decimal? _margenImporte;
        #endregion


        #region "constructors" 

        public CMPListasPreciosDetalles()
        {
        }

        #endregion


        #region "Public Properties"

        [PrimaryKey()]
        public int IdListaPrecioDetalle
        {
            get { return _idListaPrecioDetalle; }
            set { _idListaPrecioDetalle = value; }
        }

        //public int? IdListaPrecio
        //{
        //    get { return _idListaPrecio; }
        //    set { _idListaPrecio = value; }
        //}


        //public int? IdProducto
        //{
        //    get { return _idProducto; }
        //    set { _idProducto = value; }
        //}

        public int IdMoneda
        {
            get { return _idMoneda; }
            set { _idMoneda = value; }
        }

      
        [Auditoria]
        public decimal Precio
        {
            get { return _precio; }
            set { _precio = value; }
        }

        public decimal? ImporteContado
        {
            get { return CalcularPrecio(1); }
            set { _importeContado = value; }
        }

        public decimal? Importe6Cuotas
        {
            get { return CalcularPrecio(6); }
            set { _importe6Cuotas = value; }
        }

        public decimal? Importe12Cuotas
        {
            get { return CalcularPrecio(12); }
            set { _importe12Cuotas = value; }
        }

        public decimal? Importe18Cuotas
        {
            get { return CalcularPrecio(18); }
            set { _importe18Cuotas = value; }
        }

        public DateTime? Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }

        public int? IdFilial
        {
            get { return _idFilial; }
            set { _idFilial = value; }
        }

        public decimal StockActual
        {
            get { return _stockActual; }
            set
            {
                _stockActual = value;
            }
        }
        [Auditoria]
        public bool PrecioEditable
        {
            get { return _precioEditable; }
            set { _precioEditable = value; }
        }

        [Auditoria]
        public decimal? MargenImporte
        {
            get { return _margenImporte; }
            set { _margenImporte = value; }
        }
        [Auditoria]
        public decimal? MargenPorcentaje
        {
            get { return _margenPorcentaje; }
            set { _margenPorcentaje = value; }
        }
        #endregion

        public decimal PrecioConMargen
        {
            get { return Math.Round(this.Precio * (1 + (MargenPorcentaje.HasValue ? MargenPorcentaje.Value : 0) / 100) + (MargenImporte.HasValue ? MargenImporte.Value : 0), 2, MidpointRounding.AwayFromZero); }
            set { }
        }

        public decimal CalcularPrecio(int pCantidadCuotas)
        {
            //return Math.Round((this.Precio * (1 + this.ListaPrecio.MargenPorcentaje / 100) + this.ListaPrecio.MargenImporte) * (1 + this.ListaPrecio.FinanciacionPorcentaje * (pCantidadCuotas == 1 ? 0 : pCantidadCuotas) / 100) + this.ListaPrecio.MargenImporte, 2);
            return Math.Round((this.Precio * (1 + this.ListaPrecio.MargenPorcentaje / 100) + this.ListaPrecio.MargenImporte) * (1 + this.ListaPrecio.FinanciacionPorcentaje * (pCantidadCuotas == 1 ? 0 : pCantidadCuotas) / 100) + this.ListaPrecio.MargenImporte * pCantidadCuotas, 2, MidpointRounding.AwayFromZero);
        }

        public CMPProductos Producto
        {
            get { return _producto == null ? (_producto = new CMPProductos()) : _producto; }
            set { _producto = value; }
        }



        public CMPListasPrecios ListaPrecio
        {
            get { return _listaPrecio == null ? (_listaPrecio = new CMPListasPrecios()) : _listaPrecio; }
            set { _listaPrecio = value; }
        }
        TGEIVA _IVAFiltro;
        public TGEIVA IVAFiltro
        {
            get { return _IVAFiltro == null ? (_IVAFiltro = new TGEIVA()) : _IVAFiltro; }
            set { _IVAFiltro = value; }
        }

        public bool NoIncluidoEnAcopio { get; set; }

        public int CantidadCuotas { get; set; }
        public int? IdListaPrecioFiltro { get; set; }
    }
    [Serializable]
    public class CMPListasPreciosDetallesDTO
    {
        public int IdListaPrecioDetalle { get; set; }
        public int IdFilial { get; set; }
        public int ProductoIdProducto { get; set; }
        public string ProductoDescripcion { get; set; }
        public string ProductoDescripcionCombo { get; set; }
        //public int ProductoCantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Subtotal { get; set; }
        public decimal ImporteIVA { get; set; }
        public decimal SubtotalIVA { get; set; }
        public bool PrecioEditable { get; set; }
        public decimal PrecioUnitarioSinIva { get; set; }
        public bool NoIncluidoEnAcopio { get; set; }
        public decimal StockActual { get; set; }
        public bool Stockeable { get; set; }
        public int? IdIva { get; set; }
        public decimal? AlicuotaIva { get; set; }
        
    }
}
