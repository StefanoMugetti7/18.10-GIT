using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Proveedores.Entidades;
using Seguridad.Entidades;
using System;
using System.Collections.Generic;
//using Afiliados.Entidades;
namespace Compras.Entidades
{
    [Serializable]
    public partial class CmpOrdenesCompras : Objeto
    {
        #region "Private Members"
        int _idOrdenCompra;
        string _observacion;
        decimal? _importeTotal;
        decimal? _importeDescontar;
        string _detalle;
        int? _periodoPrimerVencimiento;
        UsuariosAlta _usuarioAlta;
        DateTime? _fechaAutorizacion;
        UsuariosAutorizar _usuarioAutorizar;
        CapProveedores _proveedor;
        CmpCondicionesPagos _condicionPago; //ddl (lista valor)
        DateTime? _fechaEntrega;
        string _direccionDestino;
        CmpTiposOrdenesCompras _tipoOrdenCompra; //ddl (lista valor) --> falta agregar el id en la lista valor
        AfiAfiliados _afiliado;
        //int? _idAfiliado;
        int? _cuotasDescuentoAfiliado;
        TGEFormasCobrosAfiliados _formaCobroAfiliado;
        int? _cuotasPagoProveedor;
        DateTime _fechaAlta;
        //int _idUsuarioAlta;

        DateTime? _fechaBaja;
        int _idUsuarioBaja;
        DateTime _fechaEvento;
        int _idUsuarioEvento;
        bool _check;
        DateTime? _fechaDesde;
        DateTime? _fechaHasta;
        decimal _totalOrden;
        List<CmpSolicitudesComprasDetalles> _solicitudesComprasDetalles;
        List<CmpOrdenesComprasDetalles> _ordenesComprasDetalles;
        List<CapOrdenesComprasValores> _ordenesComprasValores;
        TGEEntidades _entidad;
        DateTime? _fechaPago;
        int? _idFilialDestino;
        string _filialDestino;
        #endregion

        #region "Constructors"
        public CmpOrdenesCompras()
        { }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdOrdenCompra
        {
            get { return _idOrdenCompra; }
            set { _idOrdenCompra = value; }
        }
        [Auditoria()]
        public string Observacion
        {
            get { return _observacion; }
            set { _observacion = value; }
        }

        public decimal? ImporteTotal
        {
            get { return _importeTotal == null ? (_importeTotal = 0) : _importeTotal; }
            set { _importeTotal = value; }
        }

        public decimal? ImporteDescontar
        {
            get { return _importeDescontar == null ? (_importeDescontar = 0) : _importeDescontar; }
            set { _importeDescontar = value; }
        }

        public string Detalle
        {
            get { return _detalle; }
            set { _detalle = value; }
        }

        public bool Check
        {
            get { return _check; }
            set { _check = value; }
        }
        [Auditoria()]
        public int? PeriodoPrimerVencimiento
        {
            get { return _periodoPrimerVencimiento; }
            set { _periodoPrimerVencimiento = value; }
        }

        public UsuariosAlta UsuarioAlta
        {
            get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
            set { _usuarioAlta = value; }
        }

        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }

        [Auditoria()]
        public UsuariosAutorizar UsuarioAutorizar
        {
            get { return _usuarioAutorizar == null ? (_usuarioAutorizar = new UsuariosAutorizar()) : _usuarioAutorizar; }
            set { _usuarioAutorizar = value; }
        }

        public CapProveedores Proveedor
        {
            get { return _proveedor == null ? (_proveedor = new CapProveedores()) : _proveedor; }
            set { _proveedor = value; }
        }

        public CmpCondicionesPagos CondicionPago
        {
            get { return _condicionPago == null ? (_condicionPago = new CmpCondicionesPagos()) : _condicionPago; }
            set { _condicionPago = value; }
        }

        public DateTime? FechaEntrega
        {
            get { return _fechaEntrega; }
            set { _fechaEntrega = value; }
        }

        public string DireccionDestino
        {
            get { return _direccionDestino; }
            set { _direccionDestino = value; }
        }

        public CmpTiposOrdenesCompras TipoOrdenCompra
        {
            get { return _tipoOrdenCompra == null ? (_tipoOrdenCompra = new CmpTiposOrdenesCompras()) : _tipoOrdenCompra; }
            set { _tipoOrdenCompra = value; }
        }

        [Auditoria()]
        public AfiAfiliados Afiliado
        {
            get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
            set { _afiliado = value; }
        }

        //[Auditoria()]
        //public int? IdAfiliado
        //{
        //    get { return _idAfiliado; }
        //    set { _idAfiliado = value; }
        //}

        [Auditoria()]
        public int? CuotasDescuentoAfiliado
        {
            get { return _cuotasDescuentoAfiliado; }
            set { _cuotasDescuentoAfiliado = value; }
        }

        [Auditoria()]
        public TGEFormasCobrosAfiliados FormaCobroAfiliado
        {
            get { return _formaCobroAfiliado == null ? (_formaCobroAfiliado = new TGEFormasCobrosAfiliados()) : _formaCobroAfiliado; }
            set { _formaCobroAfiliado = value; }
        }

        [Auditoria()]
        public int? CuotasPagoProveedor
        {
            get { return _cuotasPagoProveedor; }
            set { _cuotasPagoProveedor = value; }
        }

        //[Auditoria()]
        //public int IdUsuarioAlta
        //{
        //    get { return _idUsuarioAlta; }
        //    set { _idUsuarioAlta = value; }
        //}



        [Auditoria()]
        public DateTime? FechaAutorizacion
        {
            get { return _fechaAutorizacion; }
            set { _fechaAutorizacion = value; }
        }

        [Auditoria()]
        public int IdUsuarioBaja
        {
            get { return _idUsuarioBaja; }
            set { _idUsuarioBaja = value; }
        }

        public DateTime? FechaBaja
        {
            get { return _fechaBaja; }
            set { _fechaBaja = value; }
        }
        [Auditoria()]
        public int IdUsuarioEvento
        {
            get { return _idUsuarioEvento; }
            set { _idUsuarioEvento = value; }
        }
        [Auditoria()]
        public DateTime FechaEvento
        {
            get { return _fechaEvento; }
            set { _fechaEvento = value; }
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

        public decimal TotalOrden
        {
            get { return _totalOrden; }
            set { _totalOrden = value; }
        }

        public List<CmpSolicitudesComprasDetalles> SolicitudesComprasDetalles
        {
            get { return _solicitudesComprasDetalles == null ? (_solicitudesComprasDetalles = new List<CmpSolicitudesComprasDetalles>()) : _solicitudesComprasDetalles; }
            set { _solicitudesComprasDetalles = value; }
        }

        public List<CmpOrdenesComprasDetalles> OrdenesComprasDetalles
        {
            get { return _ordenesComprasDetalles == null ? (_ordenesComprasDetalles = new List<CmpOrdenesComprasDetalles>()) : _ordenesComprasDetalles; }
            set { _ordenesComprasDetalles = value; }
        }

        public List<CapOrdenesComprasValores> OrdenesComprasValores
        {
            get { return _ordenesComprasValores == null ? (_ordenesComprasValores = new List<CapOrdenesComprasValores>()) : _ordenesComprasValores; }
            set { _ordenesComprasValores = value; }
        }

        public TGEEntidades Entidad
        {
            get { return _entidad == null ? (_entidad = new TGEEntidades()) : _entidad; }
            set { _entidad = value; }
        }

        public DateTime? FechaPago
        {
            get { return _fechaPago; }
            set { _fechaPago = value; }
        }

        public int? IdFilialDestino { get => _idFilialDestino; set => _idFilialDestino = value; }
        public string FilialDestino { get => _filialDestino; set => _filialDestino = value; }
        #endregion
    }
}
