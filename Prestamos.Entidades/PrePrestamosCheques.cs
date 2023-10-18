using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prestamos.Entidades
{
    [Serializable]
    public partial class PrePrestamosCheques : Objeto
    {
        #region "Constructors"
        public PrePrestamosCheques()
        {
        }
        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdPrestamoCheque { get; set; }

        public int IdPrestamo { get; set; }

        public string NumeroCheque { get; set; }
        //string _numeroCheque;
        //public string NumeroCheque
        //{
        //    get { return _numeroCheque == null ? string.Empty : _numeroCheque; }
        //    set { _numeroCheque = value; }
        //}

        public DateTime? Fecha { get; set; }

        public DateTime? FechaDiferido { get; set; }

        public decimal Importe { get; set; }

        public decimal ImporteInteres { get; set; }

        public decimal ImporteGastos { get; set; }

        public string TitularCheque { get; set; }

        public string CUIT { get; set; }

        public int? IdBanco { get; set; }

        public string BancoDescripcion { get; set; }

        public string CodigoPostal { get; set; }
        public string NumeroSucursal { get; set; }
        public int? CantidadDias { get; set; }

        #endregion
    }
}
