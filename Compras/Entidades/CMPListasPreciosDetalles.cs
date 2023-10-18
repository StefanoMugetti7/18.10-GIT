using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;


namespace Compras.Entidades
{
    class CMPListasPreciosDetalles : Objeto
    {

        #region "Private Members" 

        int _idListaPrecioDetalle;
        int? _idListaPrecio;
        int? _idProducto;
        decimal _precio;

        #endregion


        #region "constructors" 

        public CMPListasPreciosDetalles()
        {
        }

        #endregion


        #region "Public Properties"

        [PrimaryKey()]
        public int IdListaPrecioDetalles
        {
            get { return _idListaPrecioDetalle; }
            set { _idListaPrecioDetalle = value; }
        }


        public int? IdListaPrecio
        {
            get { return _idListaPrecio; }
            set { _idListaPrecio = value; }
        }


        public int? IdProducto
        {
            get { return _idProducto; }
            set { _idProducto = value; }
        }

        public decimal Precio
        {
            get { return _precio; }
            set { _precio = value; }
        }


        #endregion
        

    }
}
