using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Generales.Entidades;
using Compras.Entidades;
using System.Xml;

namespace Facturas.Entidades
{
    [Serializable]
    public class VTAFacturasLotesEnviados : Objeto
    {
        int _idFacturaLoteEnviado;
        int _cantidadRegistros;
        DateTime _fechaAlta;
        int _periodo;
        string _tabla;
        Int64? _idRefTabla;
        string _prefijoNumeroFactura;
        DateTime _fechaFactura;
        DateTime _fechaVencimiento;
        string _observacion;
        string _observacionComprobante;
        string _baseDatos;
        DateTime? _periodoFacturadoDesde;
        DateTime? _periodoFacturadoHasta;
        bool _comprobanteExento;
        UsuariosAlta _usuarioAlta;
        TGETiposOperaciones _tipoOperacion;
        VTATiposLotesEnviados _tiposLotesEnviados;
        TGEMonedas _moneda;
        VTAConceptosComprobantes _conceptoComprobante;
        TGEIVA _IVA;
        TGEFiliales _filial;
        VTAFilialesPuntosVentas _filialPuntoVenta;
        CMPProductos _producto;
        XmlDocumentSerializationWrapper _loteFacturas;
        XmlDocumentSerializationWrapper _loteCargos;
        string _nombreArchivo;
        List<TGECampos> _campos;
        XmlDocumentSerializationWrapper _loteCamposValores;

        [PrimaryKey()]
        public int IdFacturaLoteEnviado
        {
            get { return _idFacturaLoteEnviado; }
            set { _idFacturaLoteEnviado = value; }
        }
        public string PrefijoNumeroFactura
        {
            get { return _prefijoNumeroFactura == null ? string.Empty : _prefijoNumeroFactura; }
            set { _prefijoNumeroFactura = value; }
        }

        public DateTime FechaFactura
        {
            get { return _fechaFactura; }
            set { _fechaFactura = value; }
        }

        public string Observacion
        {
            get { return _observacion==string.Empty ? string.Empty : _observacion; }
            set { _observacion = value; }
        }

        public string ObservacionComprobante
        {
            get { return _observacionComprobante == string.Empty ? string.Empty : _observacionComprobante; }
            set { _observacionComprobante = value; }
        }

        public string BaseDatos
        {
            get { return _baseDatos==string.Empty ? string.Empty : _baseDatos; }
            set { _baseDatos = value; }
        }

        public int CantidadRegistros
        {
            get { return _cantidadRegistros; }
            set { _cantidadRegistros = value; }
        }

        public DateTime FechaVencimiento
        {
            get { return _fechaVencimiento; }
            set { _fechaVencimiento = value; }
        }

        public DateTime? PeriodoFacturadoDesde
        {
            get { return _periodoFacturadoDesde; }
            set { _periodoFacturadoDesde = value; }
        }

        public DateTime? PeriodoFacturadoHasta
        {
            get { return _periodoFacturadoHasta; }
            set { _periodoFacturadoHasta = value; }
        }

        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }

        public int Periodo
        {
            get { return _periodo; }
            set { _periodo = value; }
        }

        public string Tabla
        {
            get { return _tabla == null ? string.Empty : _tabla; }
            set { _tabla = value; }
        }

        public Int64? IdRefTabla
        {
            get { return _idRefTabla; }
            set { _idRefTabla = value; }
        }

        public bool ComprobanteExento
        {
            get { return _comprobanteExento; }
            set { _comprobanteExento = value; }
        }

        public string NombreArchivo
        {
            get { return _nombreArchivo == null ? string.Empty : _nombreArchivo; }
            set { _nombreArchivo = value; }
        }

        public VTATiposLotesEnviados TiposLotesEnviados
        {
            get { return _tiposLotesEnviados == null ? (_tiposLotesEnviados = new VTATiposLotesEnviados()) : _tiposLotesEnviados; }
            set { _tiposLotesEnviados = value; }
        }

        public TGETiposOperaciones TipoOperacion
        {
            get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
            set { _tipoOperacion = value; }
        }

        public TGEIVA IVA
        {
            get { return _IVA == null ? (_IVA = new TGEIVA()) : _IVA; }
            set { _IVA = value; }
        }

        public TGEFiliales Filial
        {
            get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
            set { _filial = value; }
        }

        public VTAFilialesPuntosVentas FilialPuntoVenta
        {
            get { return _filialPuntoVenta == null ? (_filialPuntoVenta = new VTAFilialesPuntosVentas()) : _filialPuntoVenta; }
            set { _filialPuntoVenta = value; }
        }

        public VTAConceptosComprobantes ConceptoComprobante
        {
            get { return _conceptoComprobante == null ? (_conceptoComprobante = new VTAConceptosComprobantes()) : _conceptoComprobante; }
            set { _conceptoComprobante = value; }
        }

        public TGEMonedas Moneda
        {
            get { return _moneda == null ? (_moneda = new TGEMonedas()) : _moneda; }
            set { _moneda = value; }
        }

        public UsuariosAlta UsuarioAlta
        {
            get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
            set { _usuarioAlta = value; }
        }

        public CMPProductos Producto
        {
            get { return _producto == null ? (_producto = new CMPProductos()) : _producto; }
            set { _producto = value; }
        }
        public XmlDocument LoteFacturas
        {
            get { return _loteFacturas; ; }
            set { _loteFacturas = value; }
        }

        public XmlDocument LoteCargos
        {
            get { return _loteCargos; ; }
            set { _loteCargos = value; }
        }

        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }

        }

        public XmlDocument LoteCamposValores
        {
            get { return _loteCamposValores; }//  ? (_loteCamposValores = new XmlDocumentSerializationWrapper()) : _loteCamposValores; }
            set { _loteCamposValores = value; }
        }
    }
}
