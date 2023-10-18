using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Compras.Entidades
{
    public partial class CmpOrdenesComprasDetalles: Objeto
    {

        #region "Private Members"

        int _idOrdenCompraDetalle;
        int _idOrdenCompra;
        //int _idCotizacionDetalle;
        
        //int _idProducto;
        int _cantidad;
        string _descripcion;
        int? _cantidadRecibida;
        int? _cantidadPagada;
        bool _incluirEnOP;
        CMPProductos _producto;
        DateTime _fechaEvento;
        int _idUsuarioEvento;

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
        [Auditoria()]
        public int IdOrdenCompra
        {
            get { return _idOrdenCompra; }
            set { _idOrdenCompra = value; }
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

        public int Cantidad
        {
            get { return _cantidad;}
            set { _cantidad = value;}
        }


        public string Descripcion
        {
            get { return _descripcion;}
            set { _descripcion = value;}
        }


        public int? CantidadRecibida
        {
            get { return _cantidadRecibida;}
            set {  _cantidadRecibida = value;}
        }


        public int? CantidadPagada
        {
            get { return _cantidadPagada;}
            set {  _cantidadPagada = value;}
        }

        public bool IncluirEnOP
        {
            get { return _incluirEnOP; }
            set { _incluirEnOP = value; }
        }

        [Auditoria()]
        public int IdUsuarioEvento
        {
            get { return _idUsuarioEvento; }
            set { _idUsuarioEvento = value; }
        }
        [Auditoria()]
        public DateTime FechaEvento
        {
            get { return _fechaEvento; }
            set { _fechaEvento = value; }
        }
       
        #endregion
    }
}
