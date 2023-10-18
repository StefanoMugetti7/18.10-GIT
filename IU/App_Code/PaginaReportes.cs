using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Reportes.Entidades;

namespace IU
{
    public class PaginaReportes : PaginaSegura
    {
        protected DataTable MisDatos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ReportesMisDatos"]; }
            set { Session[this.MiSessionPagina + "ReportesMisDatos"] = value; }
        }

        protected RepReportes MiReporte
        {
            get { return this.PropiedadObtenerValor<RepReportes>("RepReportesMiReporte"); }
            set { this.PropiedadGuardarValor("RepReportesMiReporte", value); }
        }
    }
}
