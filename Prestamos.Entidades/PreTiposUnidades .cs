using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prestamos.Entidades
{
    [Serializable]
    public class PreTiposUnidades : TGEListasValoresDetalles
    {
        int _idTiposUnidades;

        [PrimaryKey()]
        public int IdTiposUnidades
        {
            get { return _idTiposUnidades; }
            set { _idTiposUnidades = value; }
        }
    }
}
