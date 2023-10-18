using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Arba.WebServices
{
    public class ConsultarPadronEntidad : Objeto
    {
        public int IdPadronPorSujeto { get; set; }
        public Int64 NumeroCUIT { get; set; }
        public DateTime Fecha { get; set; }
        public XmlDocument Respuesta { get; set; }

        public decimal AlicuotaPercepcion { get; set; }
        public decimal AlicuotaRetencion { get; set; }

        public string GrupoPercepcion { get; set; }
        public string GrupoRetencion { get; set; }

        public DateTime FechaVigenciaDesde
        {
            get { return new DateTime(Fecha.Year, Fecha.Month, 1); }
            set { }
        }
        public DateTime FechaVigenciaHasta
        {
            get { return FechaVigenciaDesde.AddMonths(1).AddDays(-1) ; }
            set { }
        }


    }
}
