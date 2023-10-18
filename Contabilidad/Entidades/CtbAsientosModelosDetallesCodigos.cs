using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Contabilidad.Entidades
{
    [Serializable]
    public class CtbAsientosModelosDetallesCodigos : TGEListasValoresDetalles
    {
        int _idAsientoModeloDetalleCodigo;

        [Auditoria]
        [PrimaryKey]
        public int IdAsientoModeloDetalleCodigo
        {
            get { return _idAsientoModeloDetalleCodigo; }
            set { _idAsientoModeloDetalleCodigo = value; }
        }
    }
}
