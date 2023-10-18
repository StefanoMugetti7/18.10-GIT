using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Producciones.Entidades
{
    [Serializable]
    public class PrdProduccionesDetallesAjax : Objeto
    {
        public Int64 IdProduccion { get; set; }
        public string Descripcion { get; set; }
        public int IdFilial { get; set; }

    }
}
