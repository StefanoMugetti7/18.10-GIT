using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Generales.Entidades;

namespace Bancos.Entidades
{
    public class TESBancosCuentasTipos : TGEListasValoresDetalles
    {
        int _idBancoCuentaTipo;

        [PrimaryKey()]
        public int IdBancoCuentaTipo
        {
            get { return _idBancoCuentaTipo; }
            set { _idBancoCuentaTipo = value; }
        }
    }
}
