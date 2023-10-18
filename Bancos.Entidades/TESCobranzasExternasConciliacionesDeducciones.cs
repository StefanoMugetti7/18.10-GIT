using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Generales.Entidades;

namespace Bancos.Entidades
{
     [Serializable]
    public partial class TESCobranzasExternasConciliacionesDeducciones : Objeto
    {
        int _idCobranzaExternaConciliacionDeduccion;
        int _idCobranzaExternaConciliacion;
        TGETiposDeducciones _tipoDeduccion;
        decimal _importeDeduccion;

        public TESCobranzasExternasConciliacionesDeducciones()
        {
        }

        public int IdCobranzaExternaConciliacionDeduccion
        {
            get { return _idCobranzaExternaConciliacionDeduccion; }
            set { _idCobranzaExternaConciliacionDeduccion = value; }
        }

        public int IdCobranzaExternaConciliacion
        {
            get { return _idCobranzaExternaConciliacion; }
            set { _idCobranzaExternaConciliacion = value; }
        }

        public TGETiposDeducciones TipoDeduccion
        {
            get { return _tipoDeduccion == null ? (_tipoDeduccion = new TGETiposDeducciones()) : _tipoDeduccion; }
            set { _tipoDeduccion = value; }
        }

        public decimal ImporteDeduccion
        {
            get { return _importeDeduccion; }
            set { _importeDeduccion = value; }
        }
    }
}
