using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acopios.Entidades
{
    [Serializable]
    public class AcpAcopiosImportes : Objeto
    {
        [PrimaryKey]
        public int IdAcopioImporte { get; set; }
        public int IdAcopio { get; set; }
        public string Tabla { get; set; }
        public Int64 IdRefTabla { get; set; }
        public decimal Importe { get; set; }
        public DateTime? Fecha { get; set; }
        public String FechaDDMMAAAA { get; set; }
        public string Detalle { get; set; }
    }
}
