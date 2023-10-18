using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Compras.Entidades
{
    [Serializable]
    public partial class CmpStockMovimientosDetalles : Objeto
    {
        #region Private Members
        int _idStockMovimientoDetalle;
        int _idStockMovimiento;
        CMPProductos _producto;
        decimal _cantidad;
        decimal _stockActual;
        decimal _stockFinal;
        #endregion

        #region Constructors
        public CmpStockMovimientosDetalles()
        {

        }
        #endregion

        #region Public Properties
        [PrimaryKey()]
        public int IdStockMovimientoDetalle
        {
            get { return _idStockMovimientoDetalle; }
            set { _idStockMovimientoDetalle = value; }
        }

        public int IdStockMovimiento
        {
            get { return _idStockMovimiento; }
            set { _idStockMovimiento = value; }
        }

        public CMPProductos Producto
        {
            get { return _producto == null ? (_producto = new CMPProductos()) : _producto; }
            set { _producto = value; }
        }

        public decimal Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; }
        }

        public decimal StockActual
        {
            get { return _stockActual ; }
            set { _stockActual = value; }
        }

        public decimal StockFinal
        {
            get { return _stockFinal; }
            set { _stockFinal = value; }
        }

        public decimal? PrecioUnitario { get; set; }
        #endregion

    }
}
