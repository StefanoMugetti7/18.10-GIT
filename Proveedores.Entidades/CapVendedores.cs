using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proveedores.Entidades
{
    [Serializable]
    public class CapVendedores : CapProveedores
    {

        [PrimaryKey]
        public int IdVendedor { get; set; }
    }
}
