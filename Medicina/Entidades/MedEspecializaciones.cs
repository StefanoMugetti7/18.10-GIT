using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generales.Entidades;
using Comunes.Entidades;

namespace Medicina.Entidades
{
    public class MedEspecializaciones : TGEListasValoresDetalles
    {
        int _idEspecializacion;

        [PrimaryKey()]
        public int IdEspecializacion
        {
            get { return _idEspecializacion; }
            set { _idEspecializacion = value; }
        }
    }
}
