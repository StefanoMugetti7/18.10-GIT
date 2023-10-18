
using System;
using System.Collections.Generic;
using System.Xml;
using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.Entidades;

namespace Facturas.Entidades
{
    [Serializable]
    public partial class VTANotasPedidos : Objeto
    {
        #region "Private Members"
        int _idNotaPedido;
        //int _idAfiliado;
        AfiAfiliados _afiliado;
        TGEMonedas _moneda;
        string _descripcion;
        DateTime _fechaAlta;
        decimal _importeSinIVA;
        decimal _ivaTotal;
        decimal _importeTotal;
        byte[] _notaPedidoPDF;
        bool? _firmaDigital;
        string _domicilioEntrega;
        DateTime? _fechaEntrega;
        List<VTANotasPedidosDetalles> _notasPedidosDetalles;
        string _appPath;
        string _dirPath;
        DateTime? _fechaDesde;
        DateTime? _fechaHasta;
        XmlDocumentSerializationWrapper _loteCamposValores;
        TGEFiliales _filial;
        List<TGEComentarios> _comentarios;
        List<TGECampos> _campos;
        decimal _monedaCotizacion;

        #endregion

        #region "Constructors"
        public VTANotasPedidos()
        {
            this._monedaCotizacion = 1;
        }
        #endregion



        #region "Public Properties"
        [PrimaryKey()]
        public int IdNotaPedido
        {
            get { return _idNotaPedido; }
            set { _idNotaPedido = value; }
        }
        //public int IdAfiliado
        //{
        // get{return _idAfiliado;}
        // set{_idAfiliado = value;}
        //}
        public TGEFiliales Filial
        {
            get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
            set { _filial = value; }
        }

        public TGEMonedas Moneda
        {
            get { return _moneda == null ? (_moneda = new TGEMonedas()) : _moneda; }
            set { _moneda = value; }
        }

        public AfiAfiliados Afiliado
        {
            get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
            set { _afiliado = value; }
        }

        public string Descripcion
	    {
		    get{return _descripcion == null ? string.Empty : _descripcion ;}
		    set{_descripcion = value;}
	    }

	    public DateTime FechaAlta
	    {
		    get{return _fechaAlta;}
		    set{_fechaAlta = value;}
	    }

	

        public decimal ImporteSinIVA
        {
            get { return _importeSinIVA; }
            set { _importeSinIVA = value; }
        }

        public decimal IvaTotal
        {
            get { return _ivaTotal; }
            set { _ivaTotal = value; }
        }

        public decimal ImporteTotal
        {
            get { return _importeTotal; }
            set { _importeTotal = value; }
        }

	    public byte[] NotaPedidoPDF
	    {
		    get{return _notaPedidoPDF;}
		    set{_notaPedidoPDF = value;}
	    }

	    public bool? FirmaDigital
	    {
		    get{return _firmaDigital;}
		    set{_firmaDigital = value;}
	    }

	    public string DomicilioEntrega
	    {
		    get{return _domicilioEntrega == null ? string.Empty : _domicilioEntrega ;}
		    set{_domicilioEntrega = value;}
	    }

	    public DateTime? FechaEntrega
	    {
		    get{return _fechaEntrega;}
		    set{_fechaEntrega = value;}
	    }

        public List<VTANotasPedidosDetalles> NotasPedidosDetalles
        {
            get { return _notasPedidosDetalles == null ? (_notasPedidosDetalles = new List<VTANotasPedidosDetalles>()) : _notasPedidosDetalles; }
            set { _notasPedidosDetalles = value; }
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

	    public XmlDocument LoteCamposValores
	    {
		    get { return _loteCamposValores; }// == null ? (_loteCamposValores = new XmlDocumentSerializationWrapper()) : _loteCamposValores; }
		    set { _loteCamposValores = value; }
	    }
       
        public int? IdDomicilio { get; set; }

        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
        }
        public List<TGEComentarios> Comentarios
        {
            get { return _comentarios == null ? (_comentarios = new List<TGEComentarios>()) : _comentarios; }
            set { _comentarios = value; }
        }

        public decimal MonedaCotizacion
        {
            get { return _monedaCotizacion; }
            set { _monedaCotizacion = value; }
        }

      
        #endregion
    }
}
