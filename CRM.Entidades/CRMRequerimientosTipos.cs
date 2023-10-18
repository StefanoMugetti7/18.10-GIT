using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Entidades
{
    [Serializable]
    public class CRMRequerimientosTipos : TGEListasValoresDetalles
    {
        [PrimaryKey]
        public int IdTipoRequerimiento { get; set; }
        //public string TipoRequerimiento { get; set; }
    }
}
