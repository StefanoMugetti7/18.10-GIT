using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Facturas.Entidades
{
    [Serializable]
    public class VTATiposLotesEnviados : TGEListasValoresSistemasDetalles
    {
        int _idTipoLoteEnviado;

        [PrimaryKey]
        public int IdTipoLoteEnviado
        {
            get { return _idTipoLoteEnviado; }
            set { _idTipoLoteEnviado = value; }
        }
    }
}
