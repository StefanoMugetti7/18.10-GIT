using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Proveedores.Entidades;
namespace Compras.Entidades
{
    public partial class CmpOrdenesCompras: Objeto
    {
        #region "Private Members"
        int _idOrdenCompra;
        string _observacion;
        CapProveedores _proveedor;
        CmpCondicionesPagos _condicionPago; //ddl (lista valor)
        DateTime? _fechaEntrega;
        string _direccionDestino;
        CmpTiposOrdenesCompras _tipoOrdenCompra; //ddl (lista valor) --> falta agregar el id en la lista valor
        DateTime _fechaAlta;
        int _idUsuarioAlta;
        DateTime? _fechaBaja;
        int _idUsuarioBaja;
        DateTime _fechaEvento;
        int _idUsuarioEvento;

        DateTime? _fechaDesde;
        DateTime? _fechaHasta;
        
        List<CmpSolicitudesComprasDetalles> _solicitudesComprasDetalles;
        List<CmpOrdenesComprasDetalles> _ordenesComprasDetalles;
        #endregion

        #region "Constructors"
        public CmpOrdenesCompras()
        {}
    #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdOrdenCompra
        {
            get { return _idOrdenCompra; }
            set {_idOrdenCompra = value; }
        }
        [Auditoria()]
        public string Observacion
        {
            get { return _observacion; }
            set { _observacion = value; }
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
            get{return _fechaEntrega;}
            set{_fechaEntrega = value;} 
        }

        public string DireccionDestino
        {
            get{return _direccionDestino;}
            set{ _direccionDestino = value;}
        }

        public CmpTiposOrdenesCompras TipoOrdenCompra
        {
            get { return _tipoOrdenCompra == null ? (_tipoOrdenCompra = new CmpTiposOrdenesCompras()) : _tipoOrdenCompra; }
            set {_tipoOrdenCompra = value;}
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
        #endregion
    }
}
