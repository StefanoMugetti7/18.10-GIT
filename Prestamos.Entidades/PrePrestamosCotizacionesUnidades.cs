using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prestamos.Entidades 
{
    [Serializable]
    public class PrePrestamosCotizacionesUnidades : Objeto
    {
        public int IdCotizacionUnidad { get; set; }
        public decimal Coeficiente { get; set; }
        public DateTime FechaDesdeAplica { get; set; }

        private PreTiposUnidades _tipoUnidad;

        public PreTiposUnidades TipoUnidad
        {
            get { return _tipoUnidad == null ? (_tipoUnidad = new PreTiposUnidades()) : _tipoUnidad; }
            set { _tipoUnidad = value; }
        }


    }
}
