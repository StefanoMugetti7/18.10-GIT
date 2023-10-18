
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;

namespace Proveedores.Entidades
{
    [Serializable]
    public partial class CapProveedoresPorcentajesComisiones : Objeto
    {

        #region "Private Members"
        int _idProveedorPorcentajeComision;
        CapProveedores _proveedor;
        CapVendedores _vendedor;
        DateTime? _fechaInicioVigencia;
        decimal _porcentaje;
        TGETiposOperaciones _tipoOperacion;
        TGEFormasCobros _formaCobro;
      
        #endregion

        #region "Constructors"
        public CapProveedoresPorcentajesComisiones()
        {
        }
        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdProveedorPorcentajeComision
        {
            get { return _idProveedorPorcentajeComision; }
            set { _idProveedorPorcentajeComision = value; }
        }
        public TGEFormasCobros FormaCobro
        {
            get { return _formaCobro == null ? (_formaCobro = new TGEFormasCobros()) : _formaCobro; }
            set { _formaCobro = value; }
        }
        public CapProveedores Proveedor
        {
            get { return _proveedor==null? (_proveedor=new CapProveedores()) : _proveedor; }
            set { _proveedor = value; }
        }
        public CapVendedores Vendedor
        {
            get { return _vendedor==null? (_vendedor = new CapVendedores()) : _vendedor; }
            set { _vendedor = value; }
        }

        public DateTime? FechaInicioVigencia
        {
            get { return _fechaInicioVigencia; }
            set { _fechaInicioVigencia = value; }
        }

        public decimal Porcentaje
        {
            get { return _porcentaje; }
            set { _porcentaje = value; }
        }

        public TGETiposOperaciones TipoOperacion
        {
            get { return _tipoOperacion==null ? (_tipoOperacion=new TGETiposOperaciones()) : _tipoOperacion; }
            set { _tipoOperacion = value; }
        }

        #endregion
    }
}
