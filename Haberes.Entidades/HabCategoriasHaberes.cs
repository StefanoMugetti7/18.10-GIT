using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberes.Entidades
{
    [Serializable]
    public class HabCategoriasHaberes : TGEListasValoresDetalles
    {
        int _idCategoriaHaber;

        [PrimaryKey()]
        public int IdCategoriaHaber
        {
            get { return _idCategoriaHaber; }
            set { _idCategoriaHaber = value; }
        }
    }
}

