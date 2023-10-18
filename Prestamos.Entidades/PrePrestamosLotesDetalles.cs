using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prestamos.Entidades
{
    [Serializable]
    public class PrePrestamosLotesDetalles : Objeto
    {
        #region "Private Members"
        #endregion

        #region "Constructors"
        public PrePrestamosLotesDetalles() { }
        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdPrestamoLoteDetalle { get; set; }

        public int IdPrestamoLote { get; set; }

        public int IdPrestamo { get; set; }

        public int IdEstado { get; set; }
        #endregion
    }
}
