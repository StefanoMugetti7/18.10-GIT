using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generales.Entidades;
using Comunes.Entidades;

namespace Compras.Entidades
{
    public class CmpTiposSolicitudesCompras : TGEListasValoresDetalles 
    {
        [PrimaryKey]
        public int IdTipoSolicitudCompra { get; set; }
    }
}
