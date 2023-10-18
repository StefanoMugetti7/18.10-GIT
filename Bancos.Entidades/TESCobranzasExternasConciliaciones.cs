using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Generales.Entidades;

namespace Bancos.Entidades
{
    [Serializable]
    public partial class TESCobranzasExternasConciliaciones : Objeto
    {
        #region Private Members
        int _idCobranzaExternaConciliacion;
        TGEFormasCobros _formaCobro;
        int _idRefFormaCobro;
        string _refFormaCobroDescripcion;
        DateTime? _liquidacionFechaDesde;
        DateTime? _liquidacionFechaHasta;
        int _cantidadRegistros;
        decimal _importePresentado;
        decimal _descuentos;
        decimal _importeNeto;
        DateTime _fechaAlta;
        int _idUsuarioAlta;
        List<TESCobranzasExternasConciliacionesDetalles> _cobranzaExternaConciliacionDetalles;
        List<TESCobranzasExternasConciliacionesDeducciones> _cobranzaExternaConciliacionDeducciones;
        DateTime? _fechaDesde;
        DateTime? _fechaHasta;
        int _idBancoCuenta;
        DateTime _fechaConfirmacion;
        TGEFiliales _filial;
        TGETiposOperaciones _tipoOperacion;
        #endregion 

        #region Constructors
        public TESCobranzasExternasConciliaciones()
        {
        }
        #endregion

        #region Public Properties
        [PrimaryKey()]
        public int IdCobranzaExternaConciliacion
        {
            get { return _idCobranzaExternaConciliacion; }
            set { _idCobranzaExternaConciliacion = value; }
        }

        public TGEFiliales Filial
        {
            get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
            set { _filial = value; }
        }
        public TGETiposOperaciones TipoOperacion
        {
            get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
            set { _tipoOperacion = value; }
        }

        public TGEFormasCobros FormaCobro
        {
            get { return _formaCobro == null ? (_formaCobro = new TGEFormasCobros()) : _formaCobro; }
            set { _formaCobro = value; }
        }

        private string lote;

        public string Lote
        {
            get { return lote; }
            set { lote = value; }
        }

        public int IdRefFormaCobro
        {
            get { return _idRefFormaCobro; }
            set { _idRefFormaCobro = value; }
        }

        public DateTime? LiquidacionFechaDesde
        {
            get { return _liquidacionFechaDesde; }
            set { _liquidacionFechaDesde = value; }
        }

        public DateTime? LiquidacionFechaHasta
        {
            get { return _liquidacionFechaHasta; }
            set { _liquidacionFechaHasta = value; }
        }

        public int CantidadRegistros
        {
            get { return _cantidadRegistros; }
            set { _cantidadRegistros = value; }
        }

        public decimal ImportePresentado
        {
            get { return _importePresentado; }
            set { _importePresentado = value; }
        }

        public decimal Descuentos
        {
            get { return _descuentos; }
            set { _descuentos = value; }
        }

        public decimal ImporteNeto
        {
            get { return _importeNeto; }
            set { _importeNeto = value; }
        }

        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }

        public int IdUsuarioAlta
        {
            get { return _idUsuarioAlta; }
            set { _idUsuarioAlta = value;}
        }

        public List<TESCobranzasExternasConciliacionesDetalles> CobranzaExternaConciliacionDetalles
        {
            get { return _cobranzaExternaConciliacionDetalles == null ? (_cobranzaExternaConciliacionDetalles = new List<TESCobranzasExternasConciliacionesDetalles>()) : _cobranzaExternaConciliacionDetalles; }
            set { _cobranzaExternaConciliacionDetalles = value; }
        }

        public List<TESCobranzasExternasConciliacionesDeducciones> CobranzaExternaConciliacionDeducciones
        {
            get { return _cobranzaExternaConciliacionDeducciones == null ? (_cobranzaExternaConciliacionDeducciones = new List<TESCobranzasExternasConciliacionesDeducciones>()) : _cobranzaExternaConciliacionDeducciones; }
            set { _cobranzaExternaConciliacionDeducciones = value; }
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

        public int IdBancoCuenta
        {
            get { return _idBancoCuenta; }
            set { _idBancoCuenta = value; }
        }

        public DateTime FechaConfirmacion
        {
            get { return _fechaConfirmacion; }
            set { _fechaConfirmacion = value; }
        }

        public string RefFormaCobroDescripcion
        {
            get { return _refFormaCobroDescripcion; }
            set { _refFormaCobroDescripcion = value; }
        }
        #endregion
    }
}
