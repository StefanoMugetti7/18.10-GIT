using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Afiliados.Entidades;
using Generales.Entidades;

namespace Facturas.Entidades
{
    [Serializable]
    public partial class VTARemitos : Objeto
    {
        #region "Private Members"
        int _idRemito;
        DateTime _fechaRemito;
        string _numeroRemitoPrefijo;
        string _numeroRemitoSuFijo;
        DateTime _fechaAlta;
        DateTime? _fechaEntrega;
        int _idUsuarioAlta;
        DateTime? _fechaDesde;
        DateTime? _fechaHasta;
        byte[] _remitoPDF;
        string _appPath;
        string _dirPath;
        string _domicilioEntrega;
        string _numeroRemitoCompleto;
        TGEMonedas _Moneda;
        TGETiposOperaciones _tipoOperacion;
        TGETiposFacturas _tipoFactura;
        TGEFiliales _filial;
        AfiAfiliados _afiliado;
        VTAFilialesPuntosVentas _filialPuntoVenta;
        VTAFacturas _factura;
        List<VTARemitosDetalles> _remitosDetalles;
        List<TGEArchivos> _archivos;
        List<TGEComentarios> _comentarios;
        TGEFilialesEntregas _filialEntrega;
        List<TGECampos> _campos;
        string _descripcionCombo;
        #endregion

        #region "Constructors"
        public VTARemitos()
        {
        }
        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdRemito
        {
            get { return _idRemito; }
            set { _idRemito = value; }
        }

        [Auditoria()]
        public DateTime FechaRemito
        {
            get { return _fechaRemito; }
            set { _fechaRemito = value; }
        }

        [Auditoria()]
        public DateTime? FechaEntrega
        {
            get { return _fechaEntrega; }
            set { _fechaEntrega = value; }
        }

        [Auditoria()]
        public string NumeroRemitoPrefijo
        {
            get { return _numeroRemitoPrefijo == null ? string.Empty :_numeroRemitoPrefijo; }
            set { _numeroRemitoPrefijo = value; }
        }
        [Auditoria()]
        public string NumeroRemitoSuFijo
        {
            get { return _numeroRemitoSuFijo == null ? string.Empty : _numeroRemitoSuFijo; }
            set { _numeroRemitoSuFijo = value; }
        }

        public string NumeroRemitoCompleto
        {
            get { return _numeroRemitoCompleto == null ? string.Empty : _numeroRemitoCompleto; }
            set { _numeroRemitoCompleto = value; }
        }

        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }

        public int IdUsuarioAlta
        {
            get { return _idUsuarioAlta; }
            set { _idUsuarioAlta = value; }
        }

        public List<VTARemitosDetalles> RemitosDetalles
        {
            get { return _remitosDetalles == null ? (_remitosDetalles = new List<VTARemitosDetalles>()) : _remitosDetalles; }
            set { _remitosDetalles = value; }
                
        }

        public List<TGEArchivos> Archivos
        {
            get { return _archivos == null ? (_archivos = new List<TGEArchivos>()) : _archivos; }
            set { _archivos = value; }
        }

        public List<TGEComentarios> Comentarios
        {
            get { return _comentarios == null ? (_comentarios = new List<TGEComentarios>()) : _comentarios; }
            set { _comentarios = value; }
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

        public byte[] RemitoPDF
        {
            get { return _remitoPDF; }
            set { _remitoPDF = value; }
        }

        public string AppPath
        {
            get { return _appPath == null ? string.Empty : _appPath; }
            set { _appPath = value; }
        }

        public string DirPath
        {
            get { return _dirPath == null ? string.Empty : _dirPath; }
            set { _dirPath = value; }
        }
        [Auditoria()]
        public string DomicilioEntrega
        {
            get { return _domicilioEntrega == null ? string.Empty : _domicilioEntrega; }
            set { _domicilioEntrega = value; }
        }

        public TGETiposOperaciones TipoOperacion
        {
            get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
            set { _tipoOperacion = value; }
        }

        public TGEFiliales Filial
        {
            get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
            set { _filial = value; }
        }

        public TGEFilialesEntregas FilialEntrega
        {
            get { return _filialEntrega == null ? (_filialEntrega = new TGEFilialesEntregas()) : _filialEntrega; }
            set { _filialEntrega = value; }
        }

        public TGETiposFacturas TipoFactura
        {
            get { return _tipoFactura == null ? (_tipoFactura = new TGETiposFacturas()) : _tipoFactura; }
            set { _tipoFactura = value; }
        }

        public VTAFilialesPuntosVentas FilialPuntoVenta
        {
            get { return _filialPuntoVenta == null ? (_filialPuntoVenta = new VTAFilialesPuntosVentas()) : _filialPuntoVenta; }
            set { _filialPuntoVenta = value; }
        }

        public AfiAfiliados Afiliado
        {
            get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
            set { _afiliado = value; }
        }
        public TGEMonedas Moneda
        {
            get { return _Moneda == null ? (_Moneda = new TGEMonedas()) : _Moneda; }
            set { _Moneda = value; }
        }
        public VTAFacturas Factura
        {
            get { return _factura == null ? (_factura = new VTAFacturas()) : _factura; }
            set { _factura = value; }
        }

        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }

        }

        public string DescripcionCombo
        {
            get { return _descripcionCombo == null ? string.Empty : _descripcionCombo; }
            set { _descripcionCombo = value; }
        }

    

        public int? IdDomicilio { get; set; }

        public string DetalleAcopio { get; set; }

        public decimal? ImportePrevioEntregado { get; set; }
        #endregion

        [Auditoria()]
        public string ObservacionComprobante { get; set; }
        [Auditoria()]
        public string ObservacionInterna { get; set; }
    }
}
