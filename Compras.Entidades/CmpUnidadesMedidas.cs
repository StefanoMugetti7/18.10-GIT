using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Compras.Entidades
{
    [Serializable]
    public class CmpUnidadesMedidas : TGEListasValoresDetalles
    {
        [PrimaryKey]
        [Auditoria()]
        public int IdUnidadMedida { get; set; }
    }
}
