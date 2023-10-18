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
        #region "Private Members"
        int _idCotizacionesUnidades;
        PreTiposUnidades _tipoUnidad;
        #endregion

        #region "Constructors"
        public PrePrestamosCotizacionesUnidades()
        {
        }
        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdCotizacionesUnidades
        {
            get { return _idCotizacionesUnidades; }
            set { _idCotizacionesUnidades = value; }
        }

        public PreTiposUnidades TipoUnidad
        {
            get { return _tipoUnidad == null ? (_tipoUnidad = new PreTiposUnidades()) : _tipoUnidad; }
            set { _tipoUnidad = value; }
        }
        #endregion
    }
}
