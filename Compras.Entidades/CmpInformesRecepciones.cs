using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Proveedores.Entidades;

namespace Compras.Entidades
{
    [Serializable]
    public partial class CmpInformesRecepciones : Objeto
    {

        #region "Private Members"
        int _idInformeRecepcion;
        int _idEmpresa;
        TGEFiliales _filial;
        string _observacion;
        string _motivoCancelado;
        //int? _idRemito;
        List<CmpInformesRecepcionesDetalles> _informesRecepcionesDetalles;
        CapProveedores _proveedor;
        CmpOrdenesCompras _ordenCompra;
        DateTime _fechaEmision;
        string _numeroRemitoPrefijo;
        string _numeroRemitoSufijo;
        DateTime _fechaAlta;
        int _idUsuarioAlta;
        DateTime? _fechaBaja;
        int? _idUsuarioBaja;
        DateTime? _fechaDesde;
        DateTime? _fechaHasta;
        int _idSolicitudPago;
        decimal _precioTotal;
        #endregion 

        #region "Constructors"
        public CmpInformesRecepciones()
        { }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdInformeRecepcion
        {
            get { return _idInformeRecepcion; }
            set { _idInformeRecepcion = value; }
        }

        public int IdEmpresa
        {
            get { return _idEmpresa; }
            set { _idEmpresa = value; }
        }

        public string Observacion
        {
            get { return _observacion == null ?  String.Empty : _observacion; }
            set { _observacion = value ;} 
        }

        public string MotivoCancelado
        {

            get { return _motivoCancelado == null ?  String.Empty : _motivoCancelado; }
            set { _motivoCancelado = value ;}
        }

        //public int? IdRemito
        //{ 
        //    get {return _idRemito;}
        //    set { _idRemito = value ;}
        //}

        public List<CmpInformesRecepcionesDetalles> InformesRecepcionesDetalles
        {
            get { return _informesRecepcionesDetalles == null ? (_informesRecepcionesDetalles = new List<CmpInformesRecepcionesDetalles>()) : _informesRecepcionesDetalles; }
            set { _informesRecepcionesDetalles = value; }
        }

        public DateTime FechaEmision
        {
            get { return _fechaEmision; }
            set { _fechaEmision = value ;}
        }

        public string NumeroRemitoPrefijo
        {
            get { return _numeroRemitoPrefijo == null ? string.Empty : _numeroRemitoPrefijo; }
            set { _numeroRemitoPrefijo = value; }
        }

        public string NumeroRemitoSufijo
        {
            get { return _numeroRemitoSufijo == null ? string.Empty : _numeroRemitoSufijo; }
            set { _numeroRemitoSufijo = value; }
        }

        public string NumeroRemitoDescripcion
        {
            get { return string.Concat(NumeroRemitoPrefijo, "-", NumeroRemitoSufijo); }
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

        public DateTime? FechaBaja
        {
            get { return _fechaBaja; }
            set { _fechaBaja = value; }
        }

        public int? IdUsuarioBaja
        {
            get { return _idUsuarioBaja; }
            set { _idUsuarioBaja = value; }
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

        public decimal PrecioTotal
        {
            get { return _precioTotal; }
            set { _precioTotal = value; }
        }

        public TGEFiliales Filial
        {
            get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
            set { _filial = value; }
        }

        public CapProveedores Proveedor
        {
            get { return _proveedor == null ? (_proveedor = new CapProveedores()) : _proveedor; }
            set { _proveedor = value; }
        }

        public CmpOrdenesCompras OrdenCompra
        {
            get { return _ordenCompra == null ? (_ordenCompra = new CmpOrdenesCompras()) : _ordenCompra; }
            set { _ordenCompra = value; }
        }

        public int IdSolicitudPago
        {
            get { return _idSolicitudPago; }
            set { _idSolicitudPago = value; }
        }
        public string DetalleAcopio { get; set; }

        public string NumeroFactura { get; set; }

        public decimal? ImporteSolicitudPago { get; set; }
        public decimal? ImportePrevioRecibido { get; set; }
        #endregion
    }
}
