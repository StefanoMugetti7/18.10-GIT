using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Haberes.Entidades
{
  [Serializable]
	public partial class HabRecibosCom : Objeto
	{
	#region "Private Members"
	int _idReciboCom;
	int _periodo;
	int _idAfiliado;
	string _numeroSocio;
	string _zonaGrupo;
	int _idTipoDocumento;
	string _tipoDocumento;
	int _numeroDocumento;
	string _tipoDocumentoApoderado;
	int _numeroDocumentoApoderado;
	long _cUIL;
	string _apellido;
	string _nombre;
	string _apellidoApoderado;
	string _nombreApoderado;
	string _domicilio;
	string _codigoPostal;
	string _localidad;
	string _provincia;
	decimal _haberes;
	decimal _descuentos;
	decimal _netoPagar;
	int _idRemesaDetalle;
    int _idTipoRecibo;
    string _netoPagarLetras;
	#endregion
		
	#region "Constructors"
	public HabRecibosCom()
	{
	}
	#endregion
		
	#region "Public Properties"
	public int IdReciboCom
	{
		get{return _idReciboCom ;}
		set{_idReciboCom = value;}
	}
	public int Periodo
	{
		get{return _periodo;}
		set{_periodo = value;}
	}

	public int IdAfiliado
	{
		get{return _idAfiliado;}
		set{_idAfiliado = value;}
	}

	public string NumeroSocio
	{
		get{return _numeroSocio == null ? string.Empty : _numeroSocio ;}
		set{_numeroSocio = value;}
	}

	public string ZonaGrupo
	{
		get{return _zonaGrupo == null ? string.Empty : _zonaGrupo ;}
		set{_zonaGrupo = value;}
	}

	public int IdTipoDocumento
	{
		get{return _idTipoDocumento;}
		set{_idTipoDocumento = value;}
	}

	public string TipoDocumento
	{
		get{return _tipoDocumento == null ? string.Empty : _tipoDocumento ;}
		set{_tipoDocumento = value;}
	}

	public int NumeroDocumento
	{
		get{return _numeroDocumento;}
		set{_numeroDocumento = value;}
	}

	public string TipoDocumentoApoderado
	{
		get{return _tipoDocumentoApoderado == null ? string.Empty : _tipoDocumentoApoderado ;}
		set{_tipoDocumentoApoderado = value;}
	}

	public int NumeroDocumentoApoderado
	{
		get{return _numeroDocumentoApoderado;}
		set{_numeroDocumentoApoderado = value;}
	}

	public long CUIL
	{
		get{return _cUIL;}
		set{_cUIL = value;}
	}

	public string Apellido
	{
		get{return _apellido == null ? string.Empty : _apellido ;}
		set{_apellido = value;}
	}

	public string Nombre
	{
		get{return _nombre == null ? string.Empty : _nombre ;}
		set{_nombre = value;}
	}

	public string ApellidoApoderado
	{
		get{return _apellidoApoderado == null ? string.Empty : _apellidoApoderado ;}
		set{_apellidoApoderado = value;}
	}

	public string NombreApoderado
	{
		get{return _nombreApoderado == null ? string.Empty : _nombreApoderado ;}
		set{_nombreApoderado = value;}
	}

	public string Domicilio
	{
		get{return _domicilio == null ? string.Empty : _domicilio ;}
		set{_domicilio = value;}
	}

	public string CodigoPostal
	{
		get{return _codigoPostal == null ? string.Empty : _codigoPostal ;}
		set{_codigoPostal = value;}
	}

	public string Localidad
	{
		get{return _localidad == null ? string.Empty : _localidad ;}
		set{_localidad = value;}
	}

	public string Provincia
	{
		get{return _provincia == null ? string.Empty : _provincia ;}
		set{_provincia = value;}
	}

	public decimal Haberes
	{
		get{return _haberes;}
		set{_haberes = value;}
	}

	public decimal Descuentos
	{
		get{return _descuentos;}
		set{_descuentos = value;}
	}

	public decimal NetoPagar
	{
		get{return _netoPagar;}
		set{_netoPagar = value;}
	}

	public int IdRemesaDetalle
	{
		get{return _idRemesaDetalle;}
		set{_idRemesaDetalle = value;}
	}

    public int IdTipoRecibo
    {
        get { return _idTipoRecibo; }
        set { _idTipoRecibo = value; }
    }

    public string NetoPagarLetras
    {
        get { return _netoPagarLetras == null ? string.Empty : _netoPagarLetras; }
        set { _netoPagarLetras = value; }
    }

	#endregion
	}
}

