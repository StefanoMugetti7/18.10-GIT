using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Entidades
{
    [Serializable]
    public class CRMSeguimientos:Objeto
    {
        [PrimaryKey]
        public int IdSeguimiento{ get; set; }
        public int IdRequerimiento{ get; set; }
        public int IdEstado { get; set; }
        public int IdUsuarioAlta{ get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaAlta { get; set; }
    }
}
