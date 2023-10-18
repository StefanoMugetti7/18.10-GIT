
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Afiliados.Entidades;
using Generales.Entidades;
using Facturas.Entidades;
using Prestamos.Entidades;
using System.Xml;

namespace Cobros.Entidades
{
    [Serializable]
    public partial class CobOrdenesCobros : Objeto
    {

        #region "Private Members"
        int _idOrdenCobro;
        int? _numeroOrdenCobro;
        DateTime _fechaEmision;
        TGETiposOperaciones _tipoOperacion;
        CobCobrosTipos _cobroTipo;
        string _detalle;
        decimal _importeTotal;
        int _idUsuarioAlta;
        int? _idUsuarioConfirmacion;
        DateTime? _fechaConfirmacion;
        TGEFiliales _filial;
        TGEFilialesCobros _filialCobro;
        AfiAfiliados _afiliado;
        TGEMonedas _moneda;
        DateTime? _fechaDesde;
        DateTime? _fechaHasta;
        List<CobOrdenesCobrosDetalles> _ordenesCobrosDetalles;
        List<VTARemitos> _remitos;
        List<CobOrdenesCobrosTiposRetenciones> _ordenesCobrosTiposRetenciones;
        List<CobOrdenesCobros> _ordenesCobrosAnticipos;
        List<CobOrdenesCobrosValores> _ordenesCobrosValores;
        //List<CobOrdenesCobrosFormasCobros> _ordenesCobrosFormasCobros;
        decimal _importeSubTotal;
        decimal _importeRetenciones;
        int? _cuotasDescuentoAfiliado;
        TGEFormasCobrosAfiliados _formaCobroAfiliado;
        bool _cargoDescuentoAfiliado;
        int? _idRefFacturaOCAutomatica; //guarda ID de la factura a la cual se le esta generando la orden de cobro automatica
        decimal _importeAplicar;
        decimal _importeAplicado;
        bool _incluirEnOC;
        PrePrestamos _prestamo;
        decimal _importeBruto;
        string _prefijoNumeroRecibo;
        string _numeroRecibo;
        string _numeroReciboCompleto;
        decimal _saldoTotalAImputar;
        decimal _importeNoAplicado;
        string _prefijoNumeroFactura;
        string _numeroFactura;
        bool _modifitaTipoOperacion;
        List<TGEArchivos> _archivos;
        List<TGEComentarios> _comentarios;
        string _tabla;
        Int64? _idRefTabla;
        XmlDocumentSerializationWrapper _loteOrdenesCobrosDetalles;
        XmlDocumentSerializationWrapper _loteOrdenesCobrosValores;
        List<TGECampos> _campos;
        XmlDocumentSerializationWrapper _loteCamposValores;
        #endregion

        #region "Constructors"
        public CobOrdenesCobros()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdOrdenCobro
        {
            get { return _idOrdenCobro; }
            set { _idOrdenCobro = value; }
        }

        public int? NumeroOrdenCobro
        {
            get { return _numeroOrdenCobro; }
            set { _numeroOrdenCobro = value; }
        }

        public DateTime FechaEmision
        {
            get { return _fechaEmision; }
            set { _fechaEmision = value; }
        }

        public string Detalle
        {
            get { return _detalle == null ? string.Empty : _detalle; }
            set { _detalle = value; }
        }

        public decimal ImporteTotal
        {
            get { return _importeTotal; }
            set { _importeTotal = value; }
        }

        public int IdUsuarioAlta
        {
            get { return _idUsuarioAlta; }
            set { _idUsuarioAlta = value; }
        }

        public int? IdUsuarioConfirmacion
        {
            get { return _idUsuarioConfirmacion; }
            set { _idUsuarioConfirmacion = value; }
        }

        public DateTime? FechaConfirmacion
        {
            get { return _fechaConfirmacion; }
            set { _fechaConfirmacion = value; }
        }

        public TGETiposOperaciones TipoOperacion
        {
            get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
            set { _tipoOperacion = value; }
        }

        public CobCobrosTipos CobroTipo
        {
            get { return _cobroTipo == null ? (_cobroTipo = new CobCobrosTipos()) : _cobroTipo; }
            set { _cobroTipo = value; }
        }

        public TGEFiliales Filial
        {
            get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
            set { _filial = value; }
        }

        public TGEFilialesCobros FilialCobro
        {
            get { return _filialCobro == null ? (_filialCobro = new TGEFilialesCobros()) : _filialCobro; }
            set { _filialCobro = value; }
        }

        public AfiAfiliados Afiliado
        {
            get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
            set { _afiliado = value; }
        }

        public TGEMonedas Moneda
        {
            get { return _moneda == null ? (_moneda = new TGEMonedas()) : _moneda; }
            set { _moneda = value; }
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

        public List<CobOrdenesCobrosDetalles> OrdenesCobrosDetalles
        {
            get { return _ordenesCobrosDetalles == null ? (_ordenesCobrosDetalles = new List<CobOrdenesCobrosDetalles>()) : _ordenesCobrosDetalles; }
            set { _ordenesCobrosDetalles = value; }
        }

        public List<VTARemitos> Remitos
        {
            get { return _remitos == null ? (_remitos = new List<VTARemitos>()) : _remitos; }
            set { _remitos = value; }
        }

        public List<CobOrdenesCobrosTiposRetenciones> OrdenesCobrosTiposRetenciones
        {
            get { return _ordenesCobrosTiposRetenciones == null ? (_ordenesCobrosTiposRetenciones = new List<CobOrdenesCobrosTiposRetenciones>()) : _ordenesCobrosTiposRetenciones; }
            set { _ordenesCobrosTiposRetenciones = value; }
        }

        public List<CobOrdenesCobros> OrdenesCobrosAnticipos
        {
            get { return _ordenesCobrosAnticipos == null ? (_ordenesCobrosAnticipos = new List<CobOrdenesCobros>()) : _ordenesCobrosAnticipos; }
            set { _ordenesCobrosAnticipos = value; }
        }

        public List<CobOrdenesCobrosValores> OrdenesCobrosValores
        {
            get { return _ordenesCobrosValores == null ? (_ordenesCobrosValores = new List<CobOrdenesCobrosValores>()) : _ordenesCobrosValores; }
            set { _ordenesCobrosValores = value; }
        }

        //public List<CobOrdenesCobrosFormasCobros> OrdenesCobrosFormasCobros
        //{
        //    get { return _ordenesCobrosFormasCobros == null ? (_ordenesCobrosFormasCobros = new List<CobOrdenesCobrosFormasCobros>()) : _ordenesCobrosFormasCobros; }
        //    set { _ordenesCobrosFormasCobros = value; }
        //}

        public decimal ImporteSubTotal
        {
            get { return _importeSubTotal; }
            set { _importeSubTotal = value; }
        }

        public decimal ImporteRetenciones
        {
            get { return _importeRetenciones; }
            set { _importeRetenciones = value; }
        }

        public bool CargoDescuentoAfiliado
        {
            get { return _cargoDescuentoAfiliado; }
            set { _cargoDescuentoAfiliado = value; }
        }

        public decimal ImporteBruto
        {
            get { return _importeBruto; }
            set { _importeBruto = value; }
        }

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

        public int? IdRefFacturaOCAutomatica
        {
            get { return _idRefFacturaOCAutomatica; }
            set { _idRefFacturaOCAutomatica = value; }
        }

        public decimal ImporteAplicar
        {
            get { return _importeAplicar; }
            set { _importeAplicar = value; }
        }

        public decimal ImporteAplicado
        {
            get { return _importeAplicado; }
            set { _importeAplicado = value; }
        }

        public bool IncluirEnOC
        {
            get { return _incluirEnOC; }
            set { _incluirEnOC = value; }
        }

        public PrePrestamos Prestamo
        {
            get { return _prestamo == null ? (_prestamo = new PrePrestamos()) : _prestamo; }
            set { _prestamo = value; }
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

        public string NumeroReciboCompleto
        {
            get { return _numeroReciboCompleto == null ? string.Empty : _numeroReciboCompleto; }
            set { _numeroReciboCompleto = value; }
        }

        public decimal SaldoTotalAImputar
        {
            get { return _saldoTotalAImputar; }
            set { _saldoTotalAImputar = value; }
        }

        public decimal ImporteNoAplicado
        {
            get { return _importeNoAplicado; }
            set { _importeNoAplicado = value; }
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

        public bool ModificaTipoOperacion
        {
            get { return _modifitaTipoOperacion; }
            set { _modifitaTipoOperacion = value; }
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

        public decimal MonedaCotizacion { get; set; }

        public bool ModuloTesoreriaCajas { get; set; }

        public XmlDocument LoteOrdenesCobrosDetalles
        {
            get { return _loteOrdenesCobrosDetalles; }
            set { _loteOrdenesCobrosDetalles = value; }
        }

        public void CargarLoteOrdenesCobrosDetallesDetalles()
        {
            this.LoteOrdenesCobrosDetalles = new XmlDocument();

            XmlNode items = this.LoteOrdenesCobrosDetalles.CreateElement("OrdenesCobrosDetalles");
            this.LoteOrdenesCobrosDetalles.AppendChild(items);

            XmlNode item;
            XmlAttribute attribute;
            foreach (CobOrdenesCobrosDetalles dato in this.OrdenesCobrosDetalles)
            {
                item = this.LoteOrdenesCobrosDetalles.CreateElement("OrdeneCobroDetalle");

                attribute = this.LoteOrdenesCobrosDetalles.CreateAttribute("IdCuentaCorriente");
                attribute.Value = dato.CuentaCorriente.IdCuentaCorriente.ToString();
                item.Attributes.Append(attribute);
                items.AppendChild(item);

                attribute = this.LoteOrdenesCobrosDetalles.CreateAttribute("IdFactura");
                attribute.Value = dato.Factura.IdFactura.ToString();
                item.Attributes.Append(attribute);
                items.AppendChild(item);

                attribute = this.LoteOrdenesCobrosDetalles.CreateAttribute("Importe");
                attribute.Value = dato.Importe.ToString().Replace(',', '.'); ;
                item.Attributes.Append(attribute);
                items.AppendChild(item);
            }
        }

        public XmlDocument LoteOrdenesCobrosValores
        {
            get { return _loteOrdenesCobrosValores; }
            set { _loteOrdenesCobrosValores = value; }
        }

        public void CargarLoteOrdenesCobrosValores()
        {
            this.LoteOrdenesCobrosValores = new XmlDocument();

            XmlNode items = this.LoteOrdenesCobrosValores.CreateElement("OrdenesCobrosValores");
            this.LoteOrdenesCobrosValores.AppendChild(items);

            XmlNode item;
            XmlAttribute attribute;
            foreach (CobOrdenesCobrosValores dato in this.OrdenesCobrosValores)
            {
                item = this.LoteOrdenesCobrosValores.CreateElement("OrdeneCobroValor");

                attribute = this.LoteOrdenesCobrosValores.CreateAttribute("IdTipoValor");
                attribute.Value = dato.TipoValor.IdTipoValor.ToString();
                item.Attributes.Append(attribute);
                items.AppendChild(item);

                attribute = this.LoteOrdenesCobrosValores.CreateAttribute("IdCuenta");
                attribute.Value = dato.IdCuenta.ToString();
                item.Attributes.Append(attribute);
                items.AppendChild(item);

                attribute = this.LoteOrdenesCobrosValores.CreateAttribute("Importe");
                attribute.Value = dato.Importe.ToString().Replace(',', '.'); ;
                item.Attributes.Append(attribute);
                items.AppendChild(item);
            }
    }
        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
        }

        public XmlDocument LoteCamposValores
        {
            get { return _loteCamposValores; }// == null ? (_loteCamposValores = new XmlDocumentSerializationWrapper()) : _loteCamposValores; }
            set { _loteCamposValores = value; }
        }

        #endregion
    }
}
