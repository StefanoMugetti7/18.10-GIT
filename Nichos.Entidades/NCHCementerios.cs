using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nichos.Entidades
{
    [Serializable]
    public class NCHCementerios : Objeto
    {
        [PrimaryKey]
        public int IdCementerio { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Domicilio { get; set; }
        TGEFiliales _filial;
        public TGEFiliales Filial { get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; } set { _filial = value; } }
    }
}
