using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nichos.Entidades
{
    [Serializable]
    public class NCHPanteones : Objeto
    {
        [PrimaryKey]
        public int IdPanteon { get; set; }
     
        public string Descripcion { get; set; }     
        
        public string Codigo { get; set; }

        NCHCementerios _cementerio;
        public NCHCementerios Cementerio { get { return _cementerio == null ? (_cementerio = new NCHCementerios()) : _cementerio; } set { _cementerio = value; } }
    }
}
