using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generales.Entidades;
using Comunes.Entidades;

namespace Cargos.Entidades
{
    [Serializable]
    public class CarTiposCargosProcesos : TGEListasValoresDetalles
    {
        int _idTipoCargoProceso;

        public int IdTipoCargoProceso
        {
            get { return _idTipoCargoProceso; }
            set { _idTipoCargoProceso = value; }
        }
    }
}
