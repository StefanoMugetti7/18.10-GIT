using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.Entidades;

namespace Compras.Entidades
{
    [Serializable]
    public partial class CmpOrdenesComprasDetalles: Objeto
    {

        #region "Private Members"
        int _idOrdenCompraDetalle;
        //int _idOrdenCompra;
        AfiAfiliados _afiliado;
        CmpOrdenesCompras _ordenCompra;
        //int _idCotizacionDetalle;
        
        //int _idProducto;
        decimal _cantidad;
        string _descripcion;
        string _numeroReferencia;
        decimal? _cantidadRecibida;
        decimal? _cantidadPagada;
        bool _incluirEnOP;
        CMPProductos _producto;
        decimal _precio;
        decimal _importe;
        decimal _importeIVA;
        decimal _subtotalConIVA;
        TGEMonedas _moneda;
        int _plazoEntrega;
        TGEIVA _iVA;

        #endregion

        #region "Constructors"
        public CmpOrdenesComprasDetalles()
        {
        }
        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdOrdenCompraDetalle
        {
            get { return _idOrdenCompraDetalle; }
            set { _idOrdenCompraDetalle = value; }
        }

        public AfiAfiliados Afiliado
        {
            get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
            set { _afiliado = value; }
        }

        [Auditoria()]
        public CmpOrdenesCompras OrdenCompra
        {
            get { return _ordenCompra == null ? (_ordenCompra = new CmpOrdenesCompras()) : _ordenCompra; }
            set { _ordenCompra = value; }
        }
        
        //public int IdCotizacionDetalle
        //{
        //    get { return _idCotizacionDetalle;}
        //    set { _idCotizacionDetalle = value;}
        //}

        public CMPProductos Producto
        {
            get { return _producto == null ? (_producto = new CMPProductos()) : _producto; }
            set { _producto = value; }
        }

        //public int IdProducto
        //{
        //    get { return _idProducto; }
        //    set { _idProducto = value; }
        //}
        [Auditoria()]
        public decimal Cantidad
        {
            get { return _cantidad;}
            set { _cantidad = value;}
        }

        [Auditoria()]
        public string Descripcion
        {
            get { return _descripcion;}
            set { _descripcion = value;}
        }

        [Auditoria()]
        public decimal? CantidadRecibida
        {
            get { return _cantidadRecibida;}
            set {  _cantidadRecibida = value;}
        }

        [Auditoria()]
        public decimal? CantidadPagada
        {
            get { return _cantidadPagada;}
            set {  _cantidadPagada = value;}
        }

        public string NumeroReferencia
        {
            get { return _numeroReferencia; }
            set { _numeroReferencia = value; }
        }

        public bool IncluirEnOP
        {
            get { return _incluirEnOP; }
            set { _incluirEnOP = value; }
        }

        [Auditoria()]
        public decimal Precio
        {
            get { return _precio; }
            set { _precio = value; }
        }

        [Auditoria()]
        public decimal Importe
        {
            get { return _importe; }
            set { _importe = value; }
        }

        [Auditoria()]
        public decimal ImporteIVA
        {
            get { return _importeIVA; }
            set { _importeIVA = value; }
        }

        public decimal ImporteConIVA
        {
            get { return this.Importe + this.ImporteIVA; }
            set { }
        }

        [Auditoria()]
        public TGEMonedas Moneda
        {
            get { return _moneda == null ? (_moneda = new TGEMonedas()) : _moneda; }
            set { _moneda = value; }
        }

        [Auditoria()]
        public int PlazoEntrega
        {
            get { return _plazoEntrega; }
            set { _plazoEntrega = value; }
        }

        [Auditoria()]
        public TGEIVA IVA
        {
            get { return _iVA == null ? (_iVA = new TGEIVA()) : _iVA; }
            set { _iVA = value; }
        }
        #endregion
    }
}
