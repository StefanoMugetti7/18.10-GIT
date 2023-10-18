using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Prestamos.Entidades
{
    [Serializable]
    public partial class PrePrestamosBancoSolParametros : Objeto
    {
        #region "Private Members"
        int _idPrestamoBancoSolParametro;
        int _idPrestamoPlan;
        int _cantidadCuotas;
        decimal? _neto;
        decimal? _capital;
        decimal? _monto;
        decimal? _importeCuota;
        decimal? _importeSeguro;
        decimal? _importeQuebrantoCuota;
        decimal? _totalCuota;
        decimal? _fondoQuebrantoNeto;
        decimal? _tasaAdm;
        decimal? _sellado;
        decimal? _sueldoMinimo;
        decimal? _cFT;
        decimal? _tEA;
        decimal? _tNA;
        #endregion

        #region "Constructors"
        public PrePrestamosBancoSolParametros()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        [SQLBulkCopy()]
        public int IdPrestamoBancoSolParametro
        {
            get { return _idPrestamoBancoSolParametro; }
            set { _idPrestamoBancoSolParametro = value; }
        }
        [SQLBulkCopy()]
        public int IdPrestamoPlan
        {
            get { return _idPrestamoPlan; }
            set { _idPrestamoPlan = value; }
        }
        [SQLBulkCopy()]
        public int CantidadCuotas
        {
            get { return _cantidadCuotas; }
            set { _cantidadCuotas = value; }
        }
        [SQLBulkCopy()]
        public decimal? Neto
        {
            get { return _neto; }
            set { _neto = value; }
        }
        [SQLBulkCopy()]
        public decimal? Capital
        {
            get { return _capital; }
            set { _capital = value; }
        }

        [SQLBulkCopy()]
        public decimal? Monto
        {
            get { return _monto; }
            set { _monto = value; }
        }

        [SQLBulkCopy()]
        public decimal? ImporteCuota
        {
            get { return _importeCuota; }
            set { _importeCuota = value; }
        }

        [SQLBulkCopy()]
        public decimal? ImporteSeguro
        {
            get { return _importeSeguro; }
            set { _importeSeguro = value; }
        }

        [SQLBulkCopy()]
        public decimal? ImporteQuebrantoCuota
        {
            get { return _importeQuebrantoCuota; }
            set { _importeQuebrantoCuota = value; }
        }

        [SQLBulkCopy()]
        public decimal? TotalCuota
        {
            get { return _totalCuota; }
            set { _totalCuota = value; }
        }

        [SQLBulkCopy()]
        public decimal? FondoQuebrantoNeto
        {
            get { return _fondoQuebrantoNeto; }
            set { _fondoQuebrantoNeto = value; }
        }

        [SQLBulkCopy()]
        public decimal? TasaAdm
        {
            get { return _tasaAdm; }
            set { _tasaAdm = value; }
        }

        [SQLBulkCopy()]
        public decimal? Sellado
        {
            get { return _sellado; }
            set { _sellado = value; }
        }

        [SQLBulkCopy()]
        public decimal? SueldoMinimo
        {
            get { return _sueldoMinimo; }
            set { _sueldoMinimo = value; }
        }

        [SQLBulkCopy()]
        public decimal? CFT
        {
            get { return _cFT; }
            set { _cFT = value; }
        }

        [SQLBulkCopy()]
        public decimal? TEA
        {
            get { return _tEA; }
            set { _tEA = value; }
        }

        public decimal? TNA
        {
            get { return _tNA; }
            set { _tNA = value; }
        }

        #endregion
    }
}
