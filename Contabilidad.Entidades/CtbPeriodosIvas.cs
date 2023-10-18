using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Contabilidad.Entidades
{
    [Serializable]
    public class CtbPeriodosIvas : Objeto
    {
        // Class CtbPeriodosIvas
        #region "Private Members"
        int _idPeriodoIva;
        int _periodo;
        Decimal _ivaVentas;
        Decimal _ivaCompras;
        Decimal _ivaPosicion;
        Decimal _percepciones;
        Decimal _retenciones;
        Decimal _ivaaPagar;
        Decimal _saldoTecnico;
        CtbEjerciciosContables _ejercicioContable;
        DateTime _fechaCierre;
        DateTime _fechaContable;
        DateTime? _fechaCierreDesde;
        DateTime? _fechaCierreHasta;
        #endregion

        #region "Constructors"
        public CtbPeriodosIvas()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdPeriodoIva
        {
            get { return _idPeriodoIva; }
            set { _idPeriodoIva = value; }
        }
        [Auditoria()]
        public int Periodo
        {
            get { return _periodo; }
            set { _periodo = value; }
        }
        public CtbEjerciciosContables EjercicioContable
        {
            get { return _ejercicioContable == null ? (_ejercicioContable = new CtbEjerciciosContables()) : _ejercicioContable; }
            set { _ejercicioContable = value; }
        }
        [Auditoria()]
        public DateTime FechaCierre
        {
            get { return _fechaCierre; }
            set { _fechaCierre = value; }
        }
        public DateTime FechaContable
        {
            get { return _fechaContable; }
            set { _fechaContable = value; }
        }
        public DateTime? FechaCierreDesde
        {
            get { return _fechaCierreDesde; }
            set { _fechaCierreDesde = value; }
        }
        public DateTime? FechaCierreHasta
        {
            get { return _fechaCierreHasta; }
            set { _fechaCierreHasta = value; }
        }
        public Decimal IVAVentas
        {
            get { return _ivaVentas; }
            set { _ivaVentas = value; }
        }
        public Decimal IVACompras
        {
            get { return _ivaCompras; }
            set { _ivaCompras = value; }
        }
        public Decimal IVAPosicion
        {
            get { return _ivaPosicion; }
            set { _ivaPosicion = value; }
        }
        public Decimal Percepciones
        {
            get { return _percepciones; }
            set { _percepciones = value; }
        }
        public Decimal Retenciones
        {
            get { return _retenciones; }
            set { _retenciones = value; }
        }
        public Decimal IVAAPagar
        {
            get { return _ivaaPagar; }
            set { _ivaaPagar = value; }
        }
        public Decimal SaldoTecnico
        {
            get { return _saldoTecnico; }
            set { _saldoTecnico = value; }
        }
        #endregion
    }
}
