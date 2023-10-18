using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Afiliados.Entidades;
using System.Data;
using System.Xml;

namespace Compras.Entidades
{
    [Serializable]
    public class CMPListasPrecios : Objeto
    {
        #region "Private Members"
        int _idListaPrecio;
        string _descripcion;
        decimal _margenPorcentaje;
        decimal _margenImporte;
        decimal _financiacionPorcentaje;
        List<CMPListasPreciosDetalles> _listaPrecioDetalle;
        DateTime _fechaInicioVigencia;
        DateTime _fechaFinVigencia;
        DateTime _fechaAlta;
        DateTime? _fechaDesde;
        DateTime? _fechaHasta;
        bool? _importarLista;
        bool _ultimaAgragada;
        int _idAfiliado;
        string _razonSocial;
        List<AfiAfiliados> _afiliados;
        List<TGEFiliales> _filiales;
        XmlDocumentSerializationWrapper _loteListasPreciosDetalles;
        private List<TGECampos> _campos;
        int? _idFilialEvento;
        #endregion

        #region "Constructors"

        public CMPListasPrecios()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdListaPrecio
        {
            get{ return _idListaPrecio; }
            set { _idListaPrecio = value; }
        }
        [Auditoria]
        public string Descripcion
        {
            get { return _descripcion==null ? string.Empty : _descripcion; }
            set { _descripcion = value; }
        }
        [Auditoria]
        public decimal MargenPorcentaje
        {
            get { return _margenPorcentaje; }
            set { _margenPorcentaje = value; }
        }
        [Auditoria]
        public decimal MargenImporte
        {
            get { return _margenImporte; }
            set { _margenImporte = value; }
        }
        [Auditoria]
        public decimal FinanciacionPorcentaje
        {
            get { return _financiacionPorcentaje; }
            set { _financiacionPorcentaje = value; }
        }

        public List<CMPListasPreciosDetalles> ListaPrecioDetalle
        {
            get { return _listaPrecioDetalle == null ? (_listaPrecioDetalle = new List<CMPListasPreciosDetalles>()) : _listaPrecioDetalle; }
            set { _listaPrecioDetalle = value; }
        }
        [Auditoria]
        public DateTime FechaInicioVigencia
        {
            get { return _fechaInicioVigencia; }
            set { _fechaInicioVigencia = value; }
        }
        [Auditoria]
        public DateTime FechaFinVigencia
        {
            get { return _fechaFinVigencia; }
            set { _fechaFinVigencia = value; }
        }

        public DateTime FechaAlta
        {
            get{ return _fechaAlta;}
            set{ _fechaAlta = value;}
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

        public bool? ImportarLista
        {
            get { return _importarLista; }
            set { _importarLista = value; }
        }

        public bool UltimaAgregada
        {
            get { return _ultimaAgragada; }
            set { _ultimaAgragada = value; }
        }

        public int IdAfiliado
        {
            get { return _idAfiliado; }
            set { _idAfiliado = value; }
        }

        public string RazonSocial
        {
            get { return _razonSocial==null ? string.Empty : _razonSocial; }
            set { _razonSocial = value; }
        }

        public List<AfiAfiliados> Afiliados
        {
            get { return _afiliados == null ? (_afiliados = new List<AfiAfiliados>()) : _afiliados; }
            set { _afiliados = value; }
        }

        public List<TGEFiliales> Filiales
        {
            get { return _filiales == null ? (_filiales = new List<TGEFiliales>()) : _filiales; }
            set { _filiales = value; }
        }

        public DataTable DataTableListasPreciosDetalle { get; set; }

        public XmlDocument LoteListasPreciosDetalles
        {
            get { return _loteListasPreciosDetalles; }
            set { _loteListasPreciosDetalles = value; }
        }

        public DateTime? FechaAcopio { get; set; }

        public bool NoIncluidoEnAcopio { get; set; }

        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
        }

        public int? IdFilialEvento { get => _idFilialEvento; set => _idFilialEvento = value; }
        #endregion


    }
}
