using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generales.Entidades;
using Comunes.Entidades;

namespace Subsidios.Entidades
{
    [Serializable]
    public class SubSubsidiosTipos : TGEListasValoresDetalles
    {

        int _idSubsidioTipo;

        [PrimaryKey()]
        public int IdSubsidioTipo
        {
            get { return _idSubsidioTipo; }
            set { _idSubsidioTipo = value; }
        }
    }
}
