using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Contabilidad.Entidades
{
    [Serializable]
    public class CtbTiposAsientos : TGEListasValoresDetalles
    {

        int _idTipoAsiento;

        public int IdTipoAsiento
        {
            get { return _idTipoAsiento; }
            set { _idTipoAsiento = value; }
        }
    }
}
