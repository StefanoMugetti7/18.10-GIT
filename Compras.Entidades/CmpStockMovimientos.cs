using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Generales.Entidades;

namespace Compras.Entidades
{
    [Serializable]
    public partial class CmpStockMovimientos : Objeto
    {
        #region Private Members
        int _idStockMovimiento;
        TGEFiliales _filial;
        int? _idFilialDestino;
        string _filialDestinoFilial;
        CmpTipoStockMovimiento _tipoStockMovimiento;
        string _descripcion;
        DateTime _fechaAlta;
        int _idUsuarioAlta;
        TGETiposOperaciones _tipoOperacion;
        List<CmpStockMovimientosDetalles> _stockMovimientosDetalles;
        List<TGEArchivos> _archivos;
        List<TGEComentarios> _comentarios;
        DateTime? _fechaDesde;
        DateTime? _fechaHasta;
        #endregion

        #region Constructors
        public CmpStockMovimientos()
        {
        }
        #endregion

        #region Public Properties
        [PrimaryKey()]
        public int IdStockMovimiento
        {
            get { return _idStockMovimiento; }
            set { _idStockMovimiento = value; }
        }

        public TGEFiliales Filial
        {
            get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
            set { _filial = value; }
        }

        public int? IdFilialDestino
        {
            get { return _idFilialDestino; }
            set { _idFilialDestino = value; }
        }

        public string FilialDestinoFilial
        {
            get { return _filialDestinoFilial; }
            set { _filialDestinoFilial = value; }
        }
        [Auditoria()]
        public CmpTipoStockMovimiento TipoStockMovimiento
        {
            get { return _tipoStockMovimiento == null ? (_tipoStockMovimiento = new CmpTipoStockMovimiento()) : _tipoStockMovimiento; }
            set { _tipoStockMovimiento = value; }
        }

        [Auditoria()]
        public string Descripcion
        {
            get { return _descripcion == null ? string.Empty : _descripcion; }
            set { _descripcion = value; }
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

        public TGETiposOperaciones TipoOperacion
        {
            get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
            set { _tipoOperacion = value; }
        }

        public List<CmpStockMovimientosDetalles> StockMovimientosDetalles
        {
            get { return _stockMovimientosDetalles == null ? (_stockMovimientosDetalles = new List<CmpStockMovimientosDetalles>()) : _stockMovimientosDetalles; }
            set { _stockMovimientosDetalles = value; }
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
        #endregion
    }
}
