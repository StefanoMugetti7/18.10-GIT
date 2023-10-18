using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
using Subsidios.Entidades;
using Afiliados.Entidades;
using System.Xml;
namespace CuentasPagar.Entidades
{
    [Serializable]
    public partial class CapSolicitudPago : Objeto
    {
        #region "Private Members"
        int _idSolicitudPago;
        TGEEntidades _entidad;
        string _nombreGrilla;
        string _cuitGrilla;
        string _prefijoNumeroFactura;
        string _numeroFactura;
        DateTime? _fechaFactura;
        DateTime? _fechaContable;
        DateTime? _fechaEventoSubsidio;
        string _observacion;
        decimal _importeSinIVA;
        decimal _importeConIVA; // Se utiliza para que no de error el mapeador de presentación
        decimal _ivaTotal;
        decimal _costoFlete;
        decimal _descuento;
        decimal _redondeo;
        decimal _importeTotal;
        decimal _importeParcial;
        decimal _importeParcialOriginal;
        bool _importeParcialModificado;
        decimal _importeParcialPagado;
        DateTime? _fechaVencimiento;
        DateTime? _fechaVencimientoDesde;
        DateTime? _fechaVencimientoHasta;
        //List<CapItemOrdenPago> _capItemOrdenPago;
        List<CapSolicitudPagoDetalles> _SolicitudPagoDetalles;
        //List<CapSolicitudPagoTipoPercepcion> _SolicitudPagoTipoPercepciones;
        //CapProveedores _proveedor;
        bool _incluirEnOP; //ver si tengo que poner la opcion para dar de alta el prov en la db
        bool _incluirTodosEnOP;
        bool _pagoParcial;
        TGETiposFacturas _tipoFactura;
        CapTiposSolicitudPago _tipoSolicitudPago;
        TGETiposOperaciones _tipoOperacion;
        SubSubsidios _subsidio;
        int _idRefTipoSolicitudPago;
        //CapTipoDeActividad _tipoDeActividad;
        int? _IdOrdenPago;
        int? _idUsuarioAnulacion;
        int _idFilial;
        DateTime _fechaAlta;
        int _idUsuarioEmision;
        DateTime? _fechaAutorizacion;
        int? _idUsuarioAutorizacion;
        string _usuarioAutorizar;
        int? _idUsuarioPreAutorizar;
        DateTime? _fechaPreAutorizar;
        int? _idUsuarioAutorizar2;
        DateTime? _fechaUsuarioAutorizar2;
        DateTime? _fechaAnulacion;
        DateTime? _fechaDesde;
        DateTime? _fechaHasta;
        //CTBAsientosCabeceras _asientoContable;
        //CTBAsientosCabeceras _asientoContableAnulacion;
        int? _idOrdenPagoDescuentoAnticipo;
        //List<CapOrdenesPagoSolicitudPago> _ordenPagoSolicitudPago;
        //JURVales _vale;
        List<TGEArchivos> _archivos;
        List<TGECampos> _campos;
        List<TGEComentarios> _comentarios;
        AfiAfiliados _afiliado;
        CapSolicitudPagoCausantesBeneficios _solicitudPagoCausanteBeneficio;
        TGEFormasCobrosAfiliados _formaCobroAfiliado;
        int? _cuotasDescuentoAfiliado;
        int? _cuotasPagoProveedor;
        decimal _importePercepciones;
        List<CapSolicitudPagoTipoPercepcion> _solicitudPagoTiposPercepciones;
        TGEFilialesPagos _filialPago;
        int? _cantidadSolicitada;
        bool _remitoAutomatico;
        string _numeroRemitoPrefijo;
        string _numeroRemito;
        XmlDocumentSerializationWrapper _loteSolicitudPagoCargaMasiva;
        XmlDocumentSerializationWrapper _idsSolicitudPagoNotaCredito;
        XmlDocumentSerializationWrapper _solicitudPagoDetallesXml;
        List<CapSolicitudPago> _comprobantesAsociados;
        int _idRefTabla;
        string _tabla;
        XmlDocumentSerializationWrapper _loteAnticiposReimputar;
        List<CapSolicitudPago> _anticiposReimputar;
        #endregion

        #region "Constructors"
        public CapSolicitudPago()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdSolicitudPago
        {
            get { return _idSolicitudPago; }
            set { _idSolicitudPago = value; }
        }

        public TGEEntidades Entidad
        {
            get { return _entidad == null ? (_entidad = new TGEEntidades()) : _entidad; }
            set { _entidad = value; }
        }

        public string NombreGrilla
        {
            get { return _nombreGrilla == null ? string.Empty : _nombreGrilla; }
            set { _nombreGrilla = value; }
        }

        public string CuitGrilla
        {
            get { return _cuitGrilla == null ? string.Empty : _cuitGrilla; }
            set { _cuitGrilla = value; }
        }

        public string PrefijoNumeroFactura
        {
            get { return _prefijoNumeroFactura == null ? string.Empty : _prefijoNumeroFactura; }
            set { _prefijoNumeroFactura = value; }
        }

        public string NumeroFactura
        {
            get { return _numeroFactura == null ? string.Empty : _numeroFactura; }
            set { _numeroFactura = value; }
        }

        public string NumeroFacturaCompleto
        {
            get { return string.Concat(this.PrefijoNumeroFactura, this.NumeroFactura.Length>0? "-": string.Empty, this.NumeroFactura); }
            set { }
        }

        public string TipoNumeroFacturaCompleto
        {
            get { return string.Concat(this.TiposFacturas.Descripcion, " ", this.NumeroFacturaCompleto); }
            set { }
        }

        public DateTime? FechaFactura
        {
            get { return _fechaFactura; }
            set { _fechaFactura = value; }
        }

        public DateTime? FechaEventoSubsidio
        {
            get { return _fechaEventoSubsidio; }
            set { _fechaEventoSubsidio = value; }
        }

        [Auditoria()]
        public DateTime? FechaContable
        {
            get { return _fechaContable; }
            set { _fechaContable = value; }
        }

        [Auditoria()]
        public string Observacion
        {
            get { return _observacion == null ? string.Empty : _observacion; }
            set { _observacion = value; }
        }


        public decimal ImporteSinIVA
        {
            get { return _importeSinIVA; }
            set { _importeSinIVA = value; }
        }

        public decimal IvaTotal
        {
            get { return _ivaTotal; }
            set { _ivaTotal = value; }
        }

        public decimal ImporteConIVA
        {
            get { return _importeSinIVA + _ivaTotal; }
            //Esta propiedad no se utiliza nunca
            set { _importeConIVA = value; }
        }

        public decimal CostoFlete
        {
            get { return _costoFlete; }
            set { _costoFlete = value; }
        }

        public decimal Descuento
        {
            get { return _descuento; }
            set { _descuento = value; }
        }

        public decimal Redondeo
        {
            get { return _redondeo; }
            set { _redondeo = value; }
        }

        public decimal ImporteTotal
        {
            get { return _importeTotal; }
            set { _importeTotal = value; }
        }

        public decimal ImporteParcial
        {
            get { return _importeParcial; }
            set
            {
                //if (_importeParcial != _importeParcialOriginal && !this._importeParcialModificado && _importeParcial != value)
                //{
                //    this._importeParcialModificado = true;
                //    this._importeParcialOriginal = _importeParcial;
                //}
                _importeParcial = value;
            }
        }

        public decimal ImporteParcialOriginal
        {
            get { return _importeParcialOriginal; }
            set { _importeParcialOriginal = value; }
        }

        public bool ImporteParcialModificado
        {
            get { return _importeParcialModificado; }
            set { }
        }

        public decimal ImporteParcialPagado
        {
            get { return _importeParcialPagado; }
            set { _importeParcialPagado = value; }
        }

        public DateTime? FechaVencimiento
        {
            get { return _fechaVencimiento; }
            set { _fechaVencimiento = value; }
        }
        public DateTime? FechaVencimientoDesde
        {
            get { return _fechaVencimientoDesde; }
            set { _fechaVencimientoDesde = value; }
        }
        public DateTime? FechaVencimientoHasta
        {
            get { return _fechaVencimientoHasta; }
            set { _fechaVencimientoHasta = value; }
        }

        //public List<CapItemOrdenPago> capItemOrdenPago
        //{
        //    get{return _capItemOrdenPago==null ? (_capItemOrdenPago = new List<CapItemOrdenPago>()) : _capItemOrdenPago;}
        //    set{_capItemOrdenPago = value;}
        //}				

        public List<CapSolicitudPagoDetalles> SolicitudPagoDetalles
        {
            get { return _SolicitudPagoDetalles == null ? (_SolicitudPagoDetalles = new List<CapSolicitudPagoDetalles>()) : _SolicitudPagoDetalles; }
            set { _SolicitudPagoDetalles = value; }
        }

        //public List<CapSolicitudPagoTipoPercepcion> SolicitudPagoTipoPercepciones
        //{
        //    get { return _SolicitudPagoTipoPercepciones == null ? (_SolicitudPagoTipoPercepciones = new List<CapSolicitudPagoTipoPercepcion>()) : _SolicitudPagoTipoPercepciones; }
        //    set { _SolicitudPagoTipoPercepciones = value; }
        //}

        //public CapProveedores Proveedor
        //{
        //    get { return _proveedor == null ? (_proveedor = new CapProveedores()) : _proveedor; }
        //    set { _proveedor = value; }
        //}

        //public TGETipoProducto TipoProducto
        //{
        //    get { return _tipoProducto == null ? (_tipoProducto = new TGETipoProducto()) : _tipoProducto; }
        //    set { _tipoProducto = value; }
        //}

        public TGETiposFacturas TiposFacturas
        {
            get { return _tipoFactura == null ? (_tipoFactura = new TGETiposFacturas()) : _tipoFactura; }
            set { _tipoFactura = value; }
        }

        public CapTiposSolicitudPago TipoSolicitudPago
        {
            get { return _tipoSolicitudPago == null ? (_tipoSolicitudPago = new CapTiposSolicitudPago()) : _tipoSolicitudPago; }
            set { _tipoSolicitudPago = value; }
        }

        public int IdRefTipoSolicitudPago
        {
            get { return _idRefTipoSolicitudPago; }
            set { _idRefTipoSolicitudPago = value; }
        }

        public TGETiposOperaciones TipoOperacion
        {
            get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
            set { _tipoOperacion = value; }
        }

        public SubSubsidios Subsidio
        {
            get { return _subsidio == null ? (_subsidio = new SubSubsidios()) : _subsidio; }
            set { _subsidio = value; }
        }

        //public CapTipoDeActividad TipoDeActividad
        //{
        //    get { return _tipoDeActividad == null ? (_tipoDeActividad = new CapTipoDeActividad()) : _tipoDeActividad; }
        //    set { _tipoDeActividad = value; }
        //}
        //AVISAR QUE LO AGREGUE
        [Auditoria()]
        public int? IdUsuarioAnulacion
        {
            get { return _idUsuarioAnulacion; }
            set {_idUsuarioAnulacion = value;}
        }

        public bool IncluirEnOP
        {
            get { return _incluirEnOP; }
            set { _incluirEnOP = value; }
        }

        public bool IncluirTodosEnOP
        {
            get { return _incluirTodosEnOP; }
            set { _incluirTodosEnOP = value; }
        }

        public bool PagoParcial
        {
            get { return _pagoParcial; }
            set { _pagoParcial = value; }
        }

        public int IdFilial
        {
            get { return _idFilial; }
            set { _idFilial = value; }
        }

        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }

        public int IdUsuarioAlta
        {
            get { return _idUsuarioEmision; }
            set { _idUsuarioEmision = value; }
        }

        public int? IdUsuarioAutorizacion
        {
            get { return _idUsuarioAutorizacion; }
            set { _idUsuarioAutorizacion = value; }
        }

        public string UsuarioAutorizar
        {
            get { return _usuarioAutorizar == null ? string.Empty : _usuarioAutorizar; }
            set { _usuarioAutorizar = value; }
        }

        public DateTime? FechaAutorizacion
        {
            get { return _fechaAutorizacion; }
            set { _fechaAutorizacion = value; }
        }

        public int? IdUsuarioPreAutorizar 
        {
            get { return _idUsuarioPreAutorizar; }
            set { _idUsuarioPreAutorizar = value; }
        }

        public DateTime? FechaPreAutorizar
        {
            get { return _fechaPreAutorizar; }
            set { _fechaPreAutorizar = value; }
        }

        public int? IdUsuarioAutorizar2
        {
            get { return _idUsuarioAutorizar2; }
            set { _idUsuarioAutorizar2 = value; }
        }

        public DateTime? FechaUsuarioAutorizar2
        {
            get { return _fechaUsuarioAutorizar2; }
            set { _fechaUsuarioAutorizar2 = value; }
        }

        public DateTime? FechaAnulacion
        {
            get { return _fechaAnulacion; }
            set { _fechaAnulacion = value; }
        }

        //public CTBAsientosCabeceras AsientoContable
        //{
        //    get { return _asientoContable == null ? (_asientoContable = new CTBAsientosCabeceras()) : _asientoContable; }
        //    set { _asientoContable = value; }
        //}

        //public CTBAsientosCabeceras AsientoContableAnulacion
        //{
        //    get { return _asientoContableAnulacion == null ? (_asientoContableAnulacion = new CTBAsientosCabeceras()) : _asientoContableAnulacion; }
        //    set { _asientoContableAnulacion = value; }
        //}

        public int? IdOrdenPagoDescuentoAnticipo
        {
            get { return _idOrdenPagoDescuentoAnticipo; }
            set { _idOrdenPagoDescuentoAnticipo = value; }
        }

        //public List<CapOrdenPagoSolicitudPago> OrdenPagoSolicitudPago
        //{
        //    get { return _ordenPagoSolicitudPago == null ? (_ordenPagoSolicitudPago = new List<CapOrdenPagoSolicitudPago>()) : _ordenPagoSolicitudPago; }
        //    set { _ordenPagoSolicitudPago = value; }
        //}

        //public JURVales Vale
        //{
        //    get { return _vale == null ? (_vale = new JURVales()) : _vale; }
        //    set { _vale = value; }
        //}
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

        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
        }

        [Auditoria()]
        public AfiAfiliados Afiliado
        {
            get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
            set { _afiliado = value; } 
        }

        public CapSolicitudPagoCausantesBeneficios SolicitudPagoCausanteBeneficio
        {
            get { return _solicitudPagoCausanteBeneficio == null ? (_solicitudPagoCausanteBeneficio = new CapSolicitudPagoCausantesBeneficios()) : _solicitudPagoCausanteBeneficio; }
            set { _solicitudPagoCausanteBeneficio = value; }
        }

        [Auditoria()]
        public TGEFormasCobrosAfiliados FormaCobroAfiliado
        {
            get { return _formaCobroAfiliado == null ? (_formaCobroAfiliado = new TGEFormasCobrosAfiliados()) : _formaCobroAfiliado; }
            set { _formaCobroAfiliado = value; } 
        }

        [Auditoria()]
        public int? CuotasDescuentoAfiliado
        {
            get { return _cuotasDescuentoAfiliado; }
            set { _cuotasDescuentoAfiliado = value; }
        }

        [Auditoria()]
        public int? CuotasPagoProveedor
        {
            get { return _cuotasPagoProveedor; }
            set { _cuotasPagoProveedor = value; }
        }

        public decimal ImportePercepciones
        {
            get { return _importePercepciones; }
            set { _importePercepciones = value; }
        }

        public List<CapSolicitudPagoTipoPercepcion> SolicitudPagoTiposPercepciones
        {
            get { return _solicitudPagoTiposPercepciones == null ? (_solicitudPagoTiposPercepciones = new List<CapSolicitudPagoTipoPercepcion>()) : _solicitudPagoTiposPercepciones; }
            set { _solicitudPagoTiposPercepciones = value; }
        }

        public TGEFilialesPagos FilialPago
        {
            get { return _filialPago == null ? (_filialPago = new TGEFilialesPagos()) : _filialPago; }
            set { _filialPago = value; }
        }

        public int? CantidadSolicitada
        {
            get { return _cantidadSolicitada; }
            set { _cantidadSolicitada = value; }
        }

        public bool RemitoAutomatico
        {
            get { return _remitoAutomatico; }
            set { _remitoAutomatico = value; }
        }

        public string NumeroRemitoPrefijo
        {
            get { return _numeroRemitoPrefijo == null ? string.Empty : _numeroRemitoPrefijo; }
            set { _numeroRemitoPrefijo = value; }
        }

        public string NumeroRemito
        {
            get { return _numeroRemito == null ? string.Empty : _numeroRemito; }
            set { _numeroRemito = value; }
        }

        public bool? AcopioFinanciero { get; set; }

        public XmlDocument LoteSolicitudPagoCargaMasiva
        {
            get { return _loteSolicitudPagoCargaMasiva; }
            set { _loteSolicitudPagoCargaMasiva = value; }
        }

        public XmlDocument IdsSolicitudPagoNotaCredito
        {
            get { return _idsSolicitudPagoNotaCredito; }
            set { _idsSolicitudPagoNotaCredito = value; }
        }

        public XmlDocument SolicitudPagoDetallesXml
        {
            get { return _solicitudPagoDetallesXml; }
            set { _solicitudPagoDetallesXml = value; }
        }

        public List<CapSolicitudPago> ComprobantesAsociados
        {
            get { return _comprobantesAsociados == null ? (_comprobantesAsociados = new List<CapSolicitudPago>()) : _comprobantesAsociados; }
            set { _comprobantesAsociados = value; }
        }

        public string DescripcionCombo { get; set; }
        public int IdRefTabla { get => _idRefTabla; set => _idRefTabla = value; }

        public XmlDocument AnticiposReimputarXML
        {
            get { return _loteAnticiposReimputar; }
            set { _loteAnticiposReimputar = value; }
        }

        public List<CapSolicitudPago> AnticiposReimputar
        {
            get { return _anticiposReimputar == null ? (_anticiposReimputar = new List<CapSolicitudPago>()) : _anticiposReimputar; }
            set { _anticiposReimputar = value; }
        }

        public string Tabla { get => _tabla; set => _tabla = value; }
        #endregion
    }
}
