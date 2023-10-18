using Compras.Entidades;
using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Producciones.Entidades
{
    [Serializable]
    public class PrdProduccionesDetalles : Objeto
    {
        UsuariosAlta _usuarioAlta;
        CMPProductos _producto;

        [PrimaryKey]
        public Int64 IdProduccionDetalle { get; set; }
        public Int64 IdProduccion { get; set; }
        public DateTime Fecha { get; set; }
        //public int IdProducto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Valorizacion { get; set; }
        //public int IdUnidadMedida { get; set; }
        public int Sentido { get; set; }
        public DateTime FechaAlta { get; set; }
        public UsuariosAlta UsuarioAlta
        {
            get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
            set { _usuarioAlta = value; }
        }

        public CMPProductos Producto
        {
            get { return _producto == null ? (_producto = new CMPProductos()) : _producto; }
            set { _producto = value; }
        }
    }
}
