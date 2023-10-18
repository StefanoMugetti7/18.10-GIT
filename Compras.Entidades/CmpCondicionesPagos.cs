using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generales.Entidades;
using Comunes.Entidades;

namespace Compras.Entidades
{
    [Serializable]
    public class CmpCondicionesPagos : TGEListasValoresDetalles 
    {
        [PrimaryKey]
        public int IdCondicionPago { get; set; }
    }
}
