using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Proveedores.Entidades;

namespace Compras.Entidades
{
    public partial class CmpSolicitudesCompras : Objeto
    {

        #region "Private Members"

        int _idSolicitudCompra;
        //CapProveedores _proveedor;
        decimal _subtotal;
        decimal _importeIVA;
        decimal _total;
        int _plazoEntrega;
        List<CmpSolicitudesComprasDetalles> _solicitudCompraDetalles;
        CmpTiposSolicitudesCompras _tipoSolicitudCompra;
        string _observacion;
        int? _idUsuarioAutorizacion;
        DateTime? _fechaAutorizacion;
        int _idUsuarioAlta;
        int? _idUsuarioBaja;
        DateTime _fechaAlta;
        DateTime? _fechaBaja;
        int _idUsuarioEvento;
        DateTime? _fechaDesde;
        DateTime? _fechaHasta;
        
        #endregion

        #region "Constructors"
        public CmpSolicitudesCompras()
        {
        }
        
        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdSolicitudCompra
        {
            get { return _idSolicitudCompra; }
            set { _idSolicitudCompra = value; }
        }

        //public CapProveedores Proveedor
        //{
        //    get { return _proveedor == null ? (_proveedor = new CapProveedores()) : _proveedor; }
        //    set { _proveedor = value; }
        //}

        public decimal Subtotal
        {
            get { return _subtotal;}
            set { _subtotal = value; }
        }

        public decimal ImporteIVA
        {
            get { return _importeIVA; }
            set { _importeIVA = value; }
        }

        public decimal Total
        {
            get { return _total; }
            set { _total = value; }
        }

        public int PlazoEntrega
        {
            get { return _plazoEntrega; }
            set { _plazoEntrega = value; }
        }

        public List<CmpSolicitudesComprasDetalles> SolicitudCompraDetalles
        {
            get { return _solicitudCompraDetalles == null ? (_solicitudCompraDetalles = new List<CmpSolicitudesComprasDetalles>()) : _solicitudCompraDetalles; }
            set { _solicitudCompraDetalles = value; }
        }

        public CmpTiposSolicitudesCompras TipoSolicitudCompra
        {
            get { return _tipoSolicitudCompra == null ? (_tipoSolicitudCompra = new CmpTiposSolicitudesCompras()) : _tipoSolicitudCompra; }
            set { _tipoSolicitudCompra = value; }
        }

        public string Observacion
        {
            get { return _observacion; }
            set { _observacion = value; }
        }

        [Auditoria()]
        public int? IdUsuarioAutorizacion
        {
            get { return _idUsuarioAutorizacion; }
            set { _idUsuarioAutorizacion = value; }
        }

        public DateTime? FechaAutorizacion
        {
            get { return _fechaAutorizacion; }
            set { _fechaAutorizacion = value; }
        }

        [Auditoria()]
        public int IdUsuarioAlta
        {
            get { return _idUsuarioAlta; }
            set { _idUsuarioAlta = value; }
        }

        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }

        public int? IdUsuarioBaja
        {
            get { return _idUsuarioBaja; }
            set { _idUsuarioBaja = value; }
        }

        public DateTime? FechaBaja
        {
            get { return _fechaBaja; }
            set { _fechaBaja = value; }
        }

        public int IdUsuarioEvento
        {
            get { return _idUsuarioEvento; }
            set {_idUsuarioEvento = value;}
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

        #endregion


    }
}
