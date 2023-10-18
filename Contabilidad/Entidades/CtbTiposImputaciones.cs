using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Contabilidad.Entidades
{
    [Serializable]
    public class CtbTiposImputaciones : TGEListasValoresDetalles
    {

        int? _idTipoImputacion;

        public int? IdTipoImputacion
        {
            get { return _idTipoImputacion; }
            set { _idTipoImputacion = value; }
        }
    }
}
