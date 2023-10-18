using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Contabilidad.Entidades
{
    [Serializable]
    public class CtbClasificadores : TGEListasValoresDetalles
    {
        int _idClasificador;

        [PrimaryKey()]
        public int IdClasificador
        {
            get { return _idClasificador; }
            set { _idClasificador = value; }
        }
    }
}
