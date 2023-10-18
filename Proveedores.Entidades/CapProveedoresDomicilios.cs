using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Proveedores.Entidades
{
  [Serializable]
    public partial class CapProveedoresDomicilios : Objeto
	{
	#region "Private Members"
	int _idProveedorDomicilio;
	int? _idProveedor;
	bool _predeterminado;
	string _calle;
	int _numero;
	string _piso;
	string _deptoOficina;
    //TGECodigosPostales _codigoPostal;
    TGECodigosPostales _localidad;
    //TGEProvincias _provincias;
    //TGEPaises _paises;
	string _tE;
	string _fAX;
	string _celular;
	string _mail;
	string _contacto;
    //TGEListasValoresDetalles _listasValoresDetalles;
    TGETiposDomicilios _tipoDomicilio;
	#endregion
		
	#region "Constructors"
	public CapProveedoresDomicilios()
	{
	}
	#endregion
		
	#region "Public Properties"
	public int IdProveedorDomicilio
	{
		get{return _idProveedorDomicilio;}
		set{_idProveedorDomicilio = value;}
	}
	public int? IdProveedor
	{
		get{return _idProveedor;}
		set{_idProveedor = value;}
	}

	public bool Predeterminado
	{
		get{return _predeterminado;}
		set{_predeterminado = value;}
	}

	public string Calle
	{
		get{return _calle == null ? string.Empty : _calle;}
		set{_calle = value;}
	}

	public int Numero
	{
		get{return _numero;}
		set{_numero = value;}
	}

	public string Piso
	{
		get{return _piso == null ? string.Empty : _piso;}
		set{_piso = value;}
	}

	public string DeptoOficina
	{
		get{return _deptoOficina==null? string.Empty : _deptoOficina;}
		set{_deptoOficina = value;}
	}

    //VER
    //public TGECodigosPostales CodigoPostal
    //{
    //    get{return _codigoPostal==null? (_codigoPostal=new TGECodigosPostales()):_codigoPostal;}
    //    set{_codigoPostal = value;}
    //}

    public TGECodigosPostales Localidad
    {
        get { return _localidad == null ? (_localidad = new TGECodigosPostales()) : _localidad; }
        set { _localidad = value; }
    }

    //public TGEProvincias Provincias
    //{
    //    get{return _provincias;}
    //    set{_provincias = value;}
    //}

    //public TGEPaises Paises
    //{
    //    get{return _paises;}
    //    set{_paises = value;}
    //}

	public string TE
	{
		get{return _tE == null ? string.Empty : _tE;}
		set{_tE = value;}
	}

	public string FAX
	{
		get{return _fAX == null ? string.Empty : _fAX;}
		set{_fAX = value;}
	}

	public string Celular
	{
		get{return _celular == null ? string.Empty : _celular;}
		set{_celular = value;}
	}

	public string Mail
	{
		get{return _mail == null ? string.Empty : _mail;}
		set{_mail = value;}
	}

	public string Contacto
	{
		get{return _contacto == null ? string.Empty : _contacto;}
		set{_contacto = value;}
	}

    public TGETiposDomicilios TipoDomicilio
    {
        get { return _tipoDomicilio == null ? (_tipoDomicilio = new TGETiposDomicilios()) : _tipoDomicilio; }
        set { _tipoDomicilio = value; }
    }

	#endregion
	}
}
