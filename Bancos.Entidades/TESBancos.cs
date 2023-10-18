using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generales.Entidades;
using Comunes.Entidades;

namespace Bancos.Entidades
{
    [Serializable]
    public class TESBancos : TGEListasValoresDetalles
    {
        int _idBanco;

        [PrimaryKey()]
        public int IdBanco
        {
            get { return _idBanco; }
            set { _idBanco = value; }
        }
    }
}
