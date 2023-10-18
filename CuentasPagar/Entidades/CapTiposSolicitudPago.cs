using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generales.Entidades;
using Comunes.Entidades;

namespace CuentasPagar.Entidades
{
    public class CapTiposSolicitudPago : TGEListasValoresDetalles
    {
        [PrimaryKey]
        public int IdTipoSolicitudPago { get; set; }
    }
}
