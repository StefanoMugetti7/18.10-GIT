using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generales.Entidades;
using Comunes.Entidades;

namespace Contabilidad.Entidades
{
    [Serializable]
    public class CtbEntidadContable : TGEListasValoresDetalles
    {
        int _idEntidadContable;

        [PrimaryKey()]
        public int IdEntidadContable
        {
            get { return _idEntidadContable; }
            set { _idEntidadContable = value; }
        }
    }
}

