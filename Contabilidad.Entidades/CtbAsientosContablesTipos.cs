using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Contabilidad.Entidades
{
    [Serializable]
    public class CtbAsientosContablesTipos : TGEListasValoresDetalles
    {
        int? _idAsientoContableTipo;

        public int? IdAsientoContableTipo
        {
            get { return _idAsientoContableTipo; }
            set { _idAsientoContableTipo = value; }
        }
    }
}
