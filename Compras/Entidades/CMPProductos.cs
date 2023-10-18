
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Compras.Entidades
{
  [Serializable]
	public partial class CMPProductos : Objeto
	{

	#region "Private Members"
	int _idProducto;
	string _descripcion;
    private CMPFamilias _familia;
    decimal _precio;
    int _idUnidadMedida;
    int _stockMinimo;
    int _stockMaximo;
    int _stockRecomendado;
    //TGEIVA _iVA;
    //private TGETipoProducto _tipoProducto;

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

      [Auditoria()]
      public int IdUnidadMedida
      {
          get { return _idUnidadMedida; }
          set { _idUnidadMedida = value; }
      }

      [Auditoria()]
      public int StockMinimo
      {
          get { return _stockMinimo; }
          set { _stockMinimo = value; }
      }

      [Auditoria()]
      public int StockMaximo
      {
          get { return _stockMaximo; }
          set { _stockMaximo = value; }
      }

      [Auditoria()]
      public int StockRecomendado
      {
          get { return _stockRecomendado; }
          set { _stockRecomendado = value; }
      }

    //public TGEIVA IVA
    //{
    //    get { return _iVA == null ? (_iVA = new TGEIVA()) : _iVA; }
    //    set { _iVA = value; }
    //}

    //public TGETipoProducto TipoProducto
    //{
    //    get { return _tipoProducto == null ? (_tipoProducto = new TGETipoProducto()) : _tipoProducto; }
    //    set { _tipoProducto = value; }
    //}

	#endregion
	}
}
