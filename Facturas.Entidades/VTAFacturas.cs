using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Proveedores.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml;

namespace Facturas.Entidades
{
    [Serializable]
    public class VTAFacturas : Objeto
    {
        #region "Private Members"

        int _idFactura;
        AfiAfiliados _afiliado;
        TGETiposFacturas _tipoFactura;
        VTAConceptosComprobantes _conceptoComprobante;
        TGEMonedas _moneda;
        string _prefijoNumeroFactura;
        string _numeroFactura;
        DateTime _fechaFactura;
        DateTime _fechaContable;
        string _observacion;
        string _observacionComprobante;
        decimal _importeSinIVA;
        decimal _monedaCotizacionDolar;
        decimal _ivaTotal;
        //decimal _costoFlete;
        decimal _descuentoPorcentual;
        decimal _descuentoImporte;
        decimal _importeTotal;
        decimal _importeNetoNoGravado;
        decimal _importeOperacionesExentas;
        decimal _monedaCotizacion;
        string _CAE;
        string _CAEFechaVencimiento;
        DateTime? _fechaVencimiento;
        TGEFiliales _filial;
        VTAFilialesPuntosVentas _filialPuntoVenta;
        DateTime _fechaAlta;
        UsuariosAlta _usuarioAlta;
        DateTime _fechaAnulacion;
        TGECondicionesFiscales _condicionFiscal;
        TGETiposOperaciones _tipoOperacion;
        List<VTAFacturasDetalles> _facturasDetalles;
        DateTime? _fechaDesde;
        DateTime? _fechaHasta;
        string _appPath;
        string _dirPath;
        decimal _importeParcialCobrado;
        decimal _importeParcial;
        byte[] _facturaPDF;
        int? _idRemitoImportado;
        bool _remitoVentaAutomatico;
        bool _facturaContado;
        List<VTAFacturas> _facturasAsociadas;
        bool _puedeAnular;
        int _cantidadCuotas;
        string _numeroRemitoPrefijo;
        string _numeroRemitoSuFijo;
        string _prefijoNumeroRecibo;
        string _numeroRecibo;
        string _observacionesErrores;
        CapProveedores _proveedor;
        List<VTAFacturasTiposPercepciones> _facturasTiposPercepciones;
        decimal _importePercepciones;
        int _idFacturaLoteEnviado;
        DateTime? _periodoFacturadoDesde;
        DateTime? _periodoFacturadoHasta;
        bool _comprobanteExento;
        List<TGECampos> _campos;
        XmlDocumentSerializationWrapper _loteCamposValores;
        string _tabla;
        Int64? _idRefTabla;
        int _periodo;
        string _clienteCuit;
        string _clienteCondicionFiscal;
        int _clienteIdCondicionFiscal;
        int _clienteIdTipoDocumento;
        string _clienteDomicilio;
        string _clienteLocalidad;
        string _periodoLetras;
        string _clienteRazonSocial;
        XmlDocumentSerializationWrapper _loteFacturasDetalles;
        XmlDocumentSerializationWrapper _loteFacturasAsociadas;
        XmlDocumentSerializationWrapper _loteCuentasCobros;
        int? _idBancoCuenta;
        string _bancoCuenta;
        int? _idListaPrecio;
        string _listaPrecio;
        #endregion

        #region "Constructors"
        public VTAFacturas()
        {
            this._monedaCotizacion = 1;
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdFactura
        {
            get { return _idFactura; }
            set { _idFactura = value; }
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

        public string NumeroRemitoPrefijo
        {
            get { return _numeroRemitoPrefijo == null ? string.Empty : _numeroRemitoPrefijo; }
            set { _numeroRemitoPrefijo = value; }
        }

        public string NumeroRemitoSuFijo
        {
            get { return _numeroRemitoSuFijo == null ? string.Empty : _numeroRemitoSuFijo; }
            set { _numeroRemitoSuFijo = value; }
        }

        public string PrefijoNumeroRecibo
        {
            get { return _prefijoNumeroRecibo == null ? string.Empty : _prefijoNumeroRecibo; }
            set { _prefijoNumeroRecibo = value; }
        }

        public string NumeroRecibo
        {
            get { return _numeroRecibo == null ? string.Empty : _numeroRecibo; }
            set { _numeroRecibo = value; }
        }

        public DateTime FechaFactura
        {
            get { return _fechaFactura; }
            set { _fechaFactura = value; }
        }

        public DateTime FechaContable
        {
            get { return _fechaContable; }
            set { _fechaContable = value; }
        }

        public string Observacion
        {
            get { return _observacion == null ? string.Empty : _observacion; }
            set { _observacion = value; }
        }

        public string ObservacionComprobante
        {
            get { return _observacionComprobante == null ? string.Empty : _observacionComprobante; }
            set { _observacionComprobante = value; }
        }

        public int CantidadCuotas
        {
            get { return _cantidadCuotas; }
            set { _cantidadCuotas = value; }
        }

        public decimal ImporteSinIVA
        {
            get { return _importeSinIVA; }
            set { _importeSinIVA = value; }
        }

        public decimal MonedaCotizacionDolar
        {
            get { return _monedaCotizacionDolar; }
            set { _monedaCotizacionDolar = value; }
        }

        public decimal IvaTotal
        {
            get { return _ivaTotal; }
            set { _ivaTotal = value; }
        }

        //public decimal CostoFlete
        //{
        //    get {return _costoFlete;}
        //    set {_costoFlete = value;}
        //}

        public decimal MonedaCotizacion
        {
            get { return _monedaCotizacion; }
            set { _monedaCotizacion = value; }
        }

        public decimal DescuentoPorcentual
        {
            get { return _descuentoPorcentual; }
            set { _descuentoPorcentual = value; }
        }

        public decimal DescuentoImporte
        {
            get { return _descuentoImporte; }
            set { _descuentoImporte = value; }
        }

        public decimal ImporteTotal
        {
            get { return _importeTotal; }
            set { _importeTotal = value; }
        }

        public decimal ImporteNetoNoGravado
        {
            get { return _importeNetoNoGravado; }
            set { _importeNetoNoGravado = value; }
        }

        public decimal ImporteOperacionesExentas
        {
            get { return _importeOperacionesExentas; }
            set { _importeOperacionesExentas = value; }
        }

        public string CAE
        {
            get { return _CAE == null ? string.Empty : _CAE; }
            set { _CAE = value; }
        }

        public string CAEFechaVencimiento
        {
            get { return _CAEFechaVencimiento == null ? string.Empty : _CAEFechaVencimiento; }
            set { _CAEFechaVencimiento = value; }
        }

        public DateTime? FechaVencimiento
        {
            get { return _fechaVencimiento; }
            set { _fechaVencimiento = value; }
        }

        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }



        //public TGECondicionesFiscales CondicionFiscal
        //{
        //    get { return _condicionFiscal == null ? (_condicionFiscal = new TGECondicionesFiscales()) : _condicionFiscal; }
        //    set { _condicionFiscal = value; }
        //}

        public TGETiposOperaciones TipoOperacion
        {
            get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
            set { _tipoOperacion = value; }
        }

        public string NumeroFacturaCompleto
        {
            get { return this.PrefijoNumeroFactura.Length > 0 && this.NumeroFactura.Length > 0 ? string.Concat(this.PrefijoNumeroFactura, "-", this.NumeroFactura) : string.Empty; }
            set { }
        }

        public string TipoFacturaNumeroCompleto
        {
            get { return string.Concat(this.TipoFactura.Descripcion, " ", this.NumeroFacturaCompleto); }
            set { }
        }

        public string AppPath
        {
            get { return _appPath == null ? string.Empty : _appPath; }
            set { _appPath = value; }
        }

        public string DirPath
        {
            get { return _dirPath == null ? string.Empty : _dirPath; }
            set { _dirPath = value; }
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

        public decimal ImporteParcialCobrado
        {
            get { return _importeParcialCobrado; }
            set { _importeParcialCobrado = value; }
        }

        public decimal ImporteParcial
        {
            get { return _importeParcial; }
            set { _importeParcial = value; }
        }

        public byte[] FacturaPDF
        {
            get { return _facturaPDF; }
            set { _facturaPDF = value; }
        }

        public int? IdRemitoImportado
        {
            get { return _idRemitoImportado; }
            set { _idRemitoImportado = value; }
        }

        public bool RemitoVentaAutomatico
        {
            get { return _remitoVentaAutomatico; }
            set { _remitoVentaAutomatico = value; }
        }

        public bool FacturaContado
        {
            get { return _facturaContado; }
            set { _facturaContado = value; }
        }

        public bool ComprobanteExento
        {
            get { return _comprobanteExento; }
            set { _comprobanteExento = value; }
        }

        public List<VTAFacturas> FacturasAsociadas
        {
            get { return _facturasAsociadas == null ? (_facturasAsociadas = new List<VTAFacturas>()) : _facturasAsociadas; }
            set { _facturasAsociadas = value; }
        }

        public bool PuedeAnular
        {
            get { return _puedeAnular; }
            set { _puedeAnular = value; }
        }

        public TGEFiliales Filial
        {
            get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
            set { _filial = value; }
        }

        public TGETiposFacturas TipoFactura
        {
            get { return _tipoFactura == null ? (_tipoFactura = new TGETiposFacturas()) : _tipoFactura; }
            set { _tipoFactura = value; }
        }

        public VTAFilialesPuntosVentas FilialPuntoVenta
        {
            get { return _filialPuntoVenta == null ? (_filialPuntoVenta = new VTAFilialesPuntosVentas()) : _filialPuntoVenta; }
            set { _filialPuntoVenta = value; }
        }

        public AfiAfiliados Afiliado
        {
            get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
            set { _afiliado = value; }
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

        public DateTime FechaAnulacion
        {
            get { return _fechaAnulacion; }
            set { _fechaAnulacion = value; }
        }

        public List<VTAFacturasDetalles> FacturasDetalles
        {
            get { return _facturasDetalles == null ? (_facturasDetalles = new List<VTAFacturasDetalles>()) : _facturasDetalles; }
            set { _facturasDetalles = value; }
        }

        [Auditoria]
        public string ObservacionesErrores
        {
            get { return _observacionesErrores == null ? string.Empty : _observacionesErrores; }
            set { _observacionesErrores = value; }
        }

        public CapProveedores Proveedor
        {
            get { return _proveedor == null ? (_proveedor = new CapProveedores()) : _proveedor; }
            set { _proveedor = value; }
        }

        public List<VTAFacturasTiposPercepciones> FacturasTiposPercepciones
        {
            get { return _facturasTiposPercepciones == null ? (_facturasTiposPercepciones = new List<VTAFacturasTiposPercepciones>()) : _facturasTiposPercepciones; }
            set { _facturasTiposPercepciones = value; }
        }

        public decimal ImportePercepciones
        {
            get { return _importePercepciones; }
            set { _importePercepciones = value; }
        }

        public int IdFacturaLoteEnviado
        {
            get { return _idFacturaLoteEnviado; }
            set { _idFacturaLoteEnviado = value; }
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
        #endregion

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

        public int Periodo
        {
            get { return _periodo; }
            set { _periodo = value; }
        }

        public string PeriodoLetras
        {
            get { return _periodoLetras == null ? string.Empty : _periodoLetras; }
            set { _periodoLetras = value; }
        }

        public string ClienteRazonSocial
        {
            get { return _clienteRazonSocial == null ? string.Empty : _clienteRazonSocial; }
            set { _clienteRazonSocial = value; }
        }
        public string ClienteCondicionFiscal
        {
            get { return _clienteCondicionFiscal == null ? string.Empty : _clienteCondicionFiscal; }
            set { _clienteCondicionFiscal = value; }
        }
        public string ClienteCuit
        {
            get { return _clienteCuit == null ? string.Empty : _clienteCuit; }
            set { _clienteCuit = value; }
        }
        public string ClienteTipoDocumentoAfipCodigo
        {
            get ;
            set ;
        }

        public int ClienteIdCondicionFiscal
        {
            get { return _clienteIdCondicionFiscal; }
            set { _clienteIdCondicionFiscal = value; }
        }
        public int ClienteIdTipoDocumento
        {
            get { return _clienteIdTipoDocumento; }
            set { _clienteIdTipoDocumento = value; }
        }

        public string ClienteDomicilio
        {
            get { return _clienteDomicilio == null ? string.Empty : _clienteDomicilio; }
            set { _clienteDomicilio = value; }
        }

        public string ClienteLocalidad
        {
            get { return _clienteLocalidad == null ? string.Empty : _clienteLocalidad; }
            set { _clienteLocalidad = value; }
        }

        public bool? AcopioFinanciero { get; set; }

        public bool EsFacturaCargos { get; set; }

        public string DescripcionCombo { get; set; }

        public decimal PorcentajeTurismo { get; set; }

        //public DataTable DataTableFacturas { get; set; }

        public XmlDocument LoteFacturasDetalles
        {
            get { return _loteFacturasDetalles; }
            set { _loteFacturasDetalles = value; }
        }

        public void ObtenerListaFacturasDetalles()
        {
            LoteFacturasDetalles = new XmlDocument();

            XmlNode items = LoteFacturasDetalles.CreateElement("FacturasDetalles");
            LoteFacturasDetalles.AppendChild(items);

            XmlNode itemNodo;
            XmlNode ValorNode;

            foreach (VTAFacturasDetalles item in this.FacturasDetalles.Where(x => x.ListaPrecioDetalle.Producto.IdProducto > 0).ToList())
            {
                itemNodo = LoteFacturasDetalles.CreateElement("FacturaDetalle");

                ValorNode = LoteFacturasDetalles.CreateElement("IdFacturaDetalle");
                ValorNode.InnerText = item.IdFacturaDetalle.ToString();
                itemNodo.AppendChild(ValorNode);

                ValorNode = LoteFacturasDetalles.CreateElement("IdFactura");
                ValorNode.InnerText = item.IdFactura.ToString();
                itemNodo.AppendChild(ValorNode);

                ValorNode = LoteFacturasDetalles.CreateElement("Descripcion");
                ValorNode.InnerText = item.Descripcion == string.Empty ? item.DescripcionProducto : item.Descripcion;
                itemNodo.AppendChild(ValorNode);

                ValorNode = LoteFacturasDetalles.CreateElement("Cantidad");
                ValorNode.InnerText = item.Cantidad.ToString();
                itemNodo.AppendChild(ValorNode);

                ValorNode = LoteFacturasDetalles.CreateElement("PrecioUnitarioSinIva");
                ValorNode.InnerText = item.PrecioUnitarioSinIva.ToString();
                itemNodo.AppendChild(ValorNode);

                ValorNode = LoteFacturasDetalles.CreateElement("DescuentoPorcentual");
                ValorNode.InnerText = item.DescuentoPorcentual.ToString();
                itemNodo.AppendChild(ValorNode);

                ValorNode = LoteFacturasDetalles.CreateElement("DescuentoImporte");
                ValorNode.InnerText = item.DescuentoImporte.ToString();
                itemNodo.AppendChild(ValorNode);

                ValorNode = LoteFacturasDetalles.CreateElement("SubTotal");
                ValorNode.InnerText = item.SubTotal.Value.ToString("N2");
                itemNodo.AppendChild(ValorNode);

                ValorNode = LoteFacturasDetalles.CreateElement("SubTotalConIva");
                ValorNode.InnerText = item.SubTotalConIva.Value.ToString("N2");
                itemNodo.AppendChild(ValorNode);

                ValorNode = LoteFacturasDetalles.CreateElement("ImporteIVA");
                ValorNode.InnerText = item.ImporteIVA == null ? (item.SubTotalConIva.Value - item.SubTotal.Value).ToString() : item.ImporteIVA.Value.ToString();
                itemNodo.AppendChild(ValorNode);

                ValorNode = LoteFacturasDetalles.CreateElement("IdIVA");
                ValorNode.InnerText = item.IVA.IdIVA.ToString();
                itemNodo.AppendChild(ValorNode);

                ValorNode = LoteFacturasDetalles.CreateElement("IVADescripcion");
                ValorNode.InnerText = item.IVA.Descripcion.ToString();
                itemNodo.AppendChild(ValorNode);

                ValorNode = LoteFacturasDetalles.CreateElement("Alicuota");
                ValorNode.InnerText = item.IVA.Alicuota.ToString();
                itemNodo.AppendChild(ValorNode);

                ValorNode = LoteFacturasDetalles.CreateElement("AfipCodigo");
                ValorNode.InnerText = item.IVA.AfipCodigo.ToString();
                itemNodo.AppendChild(ValorNode);

                ValorNode = LoteFacturasDetalles.CreateElement("IdIVAAlicuota");
                ValorNode.InnerText = item.IVA.IdIVAAlicuota.ToString();
                itemNodo.AppendChild(ValorNode);

                ValorNode = LoteFacturasDetalles.CreateElement("IdCentroCostoProrrateo");
                ValorNode.InnerText = item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString();
                itemNodo.AppendChild(ValorNode);

                ValorNode = LoteFacturasDetalles.CreateElement("IdRefTabla");
                ValorNode.InnerText = item.IdRefTabla.ToString();
                itemNodo.AppendChild(ValorNode);

                ValorNode = LoteFacturasDetalles.CreateElement("Tabla");
                ValorNode.InnerText = item.Tabla;
                itemNodo.AppendChild(ValorNode);

                items.AppendChild(itemNodo);
            }
            //LoteFacturasDetalles = loteFacturasDet;
            //return LoteFacturasDetalles;
        }

        public XmlDocument LoteFacturasAsociadas
        {
            get { return _loteFacturasAsociadas; }
            set { _loteFacturasAsociadas = value; }
        }
        public int? IdBancoCuenta { get => _idBancoCuenta; set => _idBancoCuenta = value; }
        public string BancoCuenta { get => _bancoCuenta; set => _bancoCuenta = value; }
        public int? IdListaPrecio { get => _idListaPrecio; set => _idListaPrecio = value; }
        public string ListaPrecio { get => _listaPrecio; set => _listaPrecio = value; }
        public int? IdEstadoRemito { get; set; }
        public string EstadoRemito { get; set; }
        public int? IdFilialEntregaRemito { get; set; }
        public string FilialEntregaRemito{ get; set; }
        public XmlDocument CuentasCobrosXML
        {
            get { return _loteCuentasCobros; }
            set { _loteCuentasCobros = value; }
        }
        public void ObtenerListaFacturasAsociadas()
        {
            LoteFacturasAsociadas = new XmlDocument();

            XmlNode items = LoteFacturasAsociadas.CreateElement("FacturasAsociadas");
            LoteFacturasAsociadas.AppendChild(items);

            XmlNode itemNodo;
            XmlNode ValorNode;

            foreach (VTAFacturas item in this.FacturasAsociadas)
            {
                itemNodo = LoteFacturasAsociadas.CreateElement("FacturaAsociada");

                ValorNode = LoteFacturasAsociadas.CreateElement("IdFactura");
                ValorNode.InnerText = item.IdFactura.ToString();
                itemNodo.AppendChild(ValorNode);

                items.AppendChild(itemNodo);
            }
        }
    }
}
