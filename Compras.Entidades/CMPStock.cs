using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Compras.Entidades
{
    [Serializable]
    public partial class CMPStock : Objeto
    {
        #region Private Members
        int _idStock;
        decimal _stockActual;
        decimal _stockActualOriginal;
        bool _stockActualModificado;
        DateTime _fechaAlta;
        int _idUsuarioAlta;
        int _idFilial;
        CMPProductos _producto;
        //los siguientes atributos se agregaron para la consulta de stock!
        string _filial;
        decimal _valorizacion;
        decimal _precio;
        bool _validarStock;
        #endregion

        #region Constructors
        public CMPStock()
        {
        }
        #endregion

        #region Public Properties
        [PrimaryKey()]
        public int IdStock
        {
            get { return _idStock; }
            set { _idStock = value; }
        }

        public decimal StockActual
        {
            get { return _stockActual; }
            set {
                if (_stockActual != _stockActualOriginal && !this._stockActualModificado && _stockActual != value)
                {
                    this._stockActualModificado = true;
                    this._stockActualOriginal = _stockActual;
                }
                _stockActual = value; }
        }

        public decimal StockActualOriginal
        {
            get { return _stockActualOriginal; }
            set { }
        }

        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }
        [Auditoria()]
        public int IdUsuarioAlta
        {
            get { return _idUsuarioAlta; }
            set { _idUsuarioAlta = value; }
        }

        public int IdFilial
        {
            get { return _idFilial; }
            set { _idFilial = value; }
        }

        [Auditoria()]
        public CMPProductos Producto
        {
            get { return _producto == null ? (_producto = new CMPProductos()) : _producto; }
            set { _producto = value; }
        }

        public string Filial
        {
            get { return _filial; }
            set { _filial = value; }
        }

        public decimal Valorizacion
        {
            get { return _valorizacion; }
            set { _valorizacion = value; }
        }

        public decimal Precio
        {
            get { return _precio; }
            set { _precio = value; }
        }

        public bool ValidarStock
        {
            get { return _validarStock; }
            set { _validarStock = value; }
        }
        #endregion

    }
}
