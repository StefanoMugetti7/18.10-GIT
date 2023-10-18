
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
using System.Xml;
namespace Compras.Entidades
{
  [Serializable]
	public partial class CMPProductos : Objeto
	{

	#region "Private Members"
	int _idProducto;
	string _descripcion;
	string _codigoBarra;
    private CMPFamilias _familia;
    decimal _precio;
    //int _idUnidadMedida;
    decimal _stockMinimo;
    decimal _stockMaximo;
    decimal _stockRecomendado;
    bool _compra;
    bool _venta;
    string _proveedorCodigoProducto;
    decimal _stockActual;
    int _idFilial;
        //TGEIVA _iVA;
    private CmpTiposProductos _tipoProducto;
    List<TGECampos> _campos;
    XmlDocumentSerializationWrapper _loteCamposValores;
        TGEUnidadesMedidas _unidadMedida;

        decimal _aliCuotaIVA;
        int _idIva;
        //CmpUnidadesMedidas _unidadMedida;

    int _idWooCommerce;
        
        #endregion

        #region "Constructors"
        public CMPProductos()
	{
	}
	#endregion
		
	#region "Public Properties"
    [PrimaryKey()]
	public int IdProducto
	{
		get{return _idProducto ;}
		set{_idProducto = value;}
	}
      [Auditoria()]
	public string Descripcion
	{
		get{return _descripcion == null ? string.Empty : _descripcion ;}
		set{_descripcion = value;}
	}

        public string CodigoBarra
        {
            get { return _codigoBarra == null ? string.Empty : _codigoBarra; }
            set { _codigoBarra = value; }
        }


        public int IdFilial
      {
          get { return _idFilial; }
          set { _idFilial = value; }
      }

      [Auditoria()]
    public CMPFamilias Familia
    {
        get { return _familia == null ? (_familia = new CMPFamilias()) : _familia; }
        set { _familia = value; }
    }
    
      [Auditoria()]
    public decimal Precio
    {
        get { return _precio; }
        set { _precio = value; }
    }

        //[Auditoria()]
        //public CmpUnidadesMedidas UnidadMedida
        //{
        //    get { return _unidadMedida == null ? (_unidadMedida = new CmpUnidadesMedidas()) : _unidadMedida; }
        //    set { _unidadMedida = value; }
        //}
        //public int IdUnidadMedida
        //{
        //    get { return _idUnidadMedida; }
        //    set { _idUnidadMedida = value; }
        //}

        [Auditoria()]
      public decimal StockMinimo
      {
          get { return _stockMinimo; }
          set { _stockMinimo = value; }
      }

      [Auditoria()]
      public decimal StockMaximo
      {
          get { return _stockMaximo; }
          set { _stockMaximo = value; }
      }

      [Auditoria()]
      public decimal StockRecomendado
      {
          get { return _stockRecomendado; }
          set { _stockRecomendado = value; }
      }

      [Auditoria()]
      public bool Compra
      {
          get { return _compra; }
          set { _compra = value; }
      }

      [Auditoria()]
      public bool Venta
      {
          get { return _venta; }
          set { _venta = value; }
      }

      [Auditoria()]
      public string ProveedorCodigoProducto
      {
          get { return _proveedorCodigoProducto == null ? string.Empty : _proveedorCodigoProducto; }
          set { _proveedorCodigoProducto = value; }
      }
      public decimal AliCuotaIva
      {
            get { return _aliCuotaIVA; }
            set { _aliCuotaIVA = value; }
        }
        
      public int IdIva
      {
          get { return _idIva; }
          set { _idIva = value; }
      }
      public decimal StockActual
      {
          get { return _stockActual; }
          set { _stockActual = value; }
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

    //public TGEIVA IVA
    //{
    //    get { return _iVA == null ? (_iVA = new TGEIVA()) : _iVA; }
    //    set { _iVA = value; }
    //}
      [Auditoria()]
      public CmpTiposProductos TipoProducto
      {
          get { return _tipoProducto == null ? (_tipoProducto = new CmpTiposProductos()) : _tipoProducto; }
          set { _tipoProducto = value; }
      }
        //[Auditoria()]
        //public bool Compra
        //{
        //    get { return _compra; }
        //    set { _compra = value; }
        //}
        //[Auditoria()]
        //public bool Venta
        //{
        //    get { return _venta; }
        //    set { _venta = value; }
        //}

        [Auditoria()]
        public TGEUnidadesMedidas UnidadMedida
        {
            get { return _unidadMedida  == null ? (_unidadMedida = new TGEUnidadesMedidas()) : _unidadMedida; }
            set { _unidadMedida = value; }
        }

        public int IdWooCommerce { get; set; }

        public int IdProveedor { get; set; }
        #endregion
    }


    [Serializable]
    public class CMPProductosDTO
    {
        public int IdProducto { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionCombo { get; set; }
        public decimal StockActual { get; set; }
        public bool Stockeable { get; set; }
        public string ProductoDescripcionCompleta { get; set; }
        public decimal PrecioUnitario { get; set; }
    }
}
