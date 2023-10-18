using Compras.Entidades;
using Comunes.Entidades;
using System;
using System.Collections.Generic;

namespace Turismo.Entidades
{
    [Serializable]
    public class TurPaquetesDetalles : Objeto
    {
        int _idPaqueteDetalle;
        int _idPaquete;
        int _idProveedor;
        string _proveedor;
        CMPProductos _producto;
        decimal _importe;
        decimal _costo;
        List<TGECampos> _campos;

        [PrimaryKey]
        public int IdPaqueteDetalle { get => _idPaqueteDetalle; set => _idPaqueteDetalle = value; }
        public int IdPaquete { get => _idPaquete; set => _idPaquete = value; }
        public int IdProveedor { get => _idProveedor; set => _idProveedor = value; }
        public decimal Importe { get => _importe; set => _importe = value; }
        public decimal Costo { get => _costo; set => _costo = value; }
        public string Proveedor { get => _proveedor; set => _proveedor = value; }
        public CMPProductos Producto
        {
            get { return _producto == null ? (_producto = new CMPProductos()) : _producto; }
            set { _producto = value; }
        }
        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
        }
    }
}
