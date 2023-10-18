
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Afiliados.Entidades;
using Generales.Entidades;
namespace Cobros.Entidades
{
    [Serializable]
    public partial class CobOrdenesCobros : Objeto
    {

        #region "Private Members"
        int _idOrdenCobro;
        DateTime _fechaEmision;
        CobCobrosTipos _cobroTipo;
        string _detalle;
        decimal _importeTotal;
        TGEFiliales _filial;
        TGEFilialesCobros _filialCobro;
        TGETiposOperaciones _tipoOperacion;
        AfiAfiliados _afiliado;
        TGEMonedas _moneda;
        DateTime? _fechaDesde;
        DateTime? _fechaHasta;
        List<CobOrdenesCobrosDetalles> _ordenesCobrosDetalles;
        //List<CobOrdenesCobrosFormasCobros> _ordenesCobrosFormasCobros;
        #endregion

        #region "Constructors"
        public CobOrdenesCobros()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdOrdenCobro
        {
            get { return _idOrdenCobro; }
            set { _idOrdenCobro = value; }
        }
        public DateTime FechaEmision
        {
            get { return _fechaEmision; }
            set { _fechaEmision = value; }
        }

        public string Detalle
        {
            get { return _detalle == null ? string.Empty : _detalle; }
            set { _detalle = value; }
        }

        public decimal ImporteTotal
        {
            get { return _importeTotal; }
            set { _importeTotal = value; }
        }

        public CobCobrosTipos CobroTipo
        {
            get { return _cobroTipo == null ? (_cobroTipo = new CobCobrosTipos()) : _cobroTipo; }
            set { _cobroTipo = value; }
        }

        public TGEFiliales Filial
        {
            get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
            set { _filial = value; }
        }

        public TGEFilialesCobros FilialCobro
        {
            get { return _filialCobro == null ? (_filialCobro = new TGEFilialesCobros()) : _filialCobro; }
            set { _filialCobro = value; }
        }

        public TGETiposOperaciones TipoOperacion
        {
            get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
            set { _tipoOperacion = value; }
        }

        public AfiAfiliados Afiliado
        {
            get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
            set { _afiliado = value; }
        }

        public TGEMonedas Moneda
        {
            get { return _moneda == null ? (_moneda = new TGEMonedas()) : _moneda; }
            set { _moneda = value; }
        }

        public DateTime? FechaDesde
        {
            get { return _fechaDesde; }
            set { _fechaDesde = value; }
        }

        public DateTime? FechaHasta
        {
            get { return _fechaHasta; }
            set { _fechaHasta = value; }
        }

        public List<CobOrdenesCobrosDetalles> OrdenesCobrosDetalles
        {
            get { return _ordenesCobrosDetalles == null ? (_ordenesCobrosDetalles = new List<CobOrdenesCobrosDetalles>()) : _ordenesCobrosDetalles; }
            set { _ordenesCobrosDetalles = value; }
        }
        //public List<CobOrdenesCobrosFormasCobros> OrdenesCobrosFormasCobros
        //{
        //    get { return _ordenesCobrosFormasCobros == null ? (_ordenesCobrosFormasCobros = new List<CobOrdenesCobrosFormasCobros>()) : _ordenesCobrosFormasCobros; }
        //    set { _ordenesCobrosFormasCobros = value; }
        //}

        #endregion
    }
}
