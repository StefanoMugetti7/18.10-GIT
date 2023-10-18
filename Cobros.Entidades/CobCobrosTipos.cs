using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Generales.Entidades;

namespace Cobros.Entidades
{
    [Serializable]
    public class CobCobrosTipos : TGEListasValoresDetalles
    {
        int _idCobroConcepto;

        [PrimaryKey()]
        public int IdCobroConcepto
        {
            get { return _idCobroConcepto; }
            set { _idCobroConcepto = value; }
        }
        int hola;
    }
}
