using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mailing.Entidades
{
    public class AudMailsEnvios : Objeto
    {
        [PrimaryKey]
        public int IdMailEnvio { get; set; }
        public string Cuerpo { get; set; }
        public string De { get; set; }
        public string DeMostrar { get; set; }
        public string A { get; set; }
        public string AMostrar { get; set; }
        public string Asunto { get; set; }

    }
}
