using Afiliados.Entidades;
using Compras.Entidades;
using Comunes.Entidades;
using System;

namespace Hoteles.Entidades
{
    [Serializable]
    public class HTLListaEspera : Objeto
    {
        [PrimaryKey]
        public int IdListaEspera { get; set; }
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public int IdUsuarioAlta { get; set; }

        AfiAfiliados _afiliado;
        public AfiAfiliados Afiliado
        {
            get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
            set { _afiliado = value; }
        }

        CMPProductos _producto;
        public CMPProductos Producto
        {
            get { return _producto == null ? (_producto = new CMPProductos()) : _producto; }
            set { _producto = value; }
        }
    }
}
