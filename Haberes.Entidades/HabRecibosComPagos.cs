using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Haberes.Entidades
{
    [Serializable]
	public partial class HabRecibosComPagos : Objeto
	{
		// Class HabRecibosCOMPagos
	#region "Private Members"
	int _idReciboCOMPago;
	int? _idReciboCOM;
	int _idCuentaCorriente;
	int _idAfiliado;
	string _apellido;
	string _nombre;
	int? _idTipoDocumento;
	int? _numeroDocumento;
	string _domicilio;
	byte[] _firma;

	#endregion
		
	#region "Constructors"
	public HabRecibosComPagos()
	{
	}
	#endregion
		
	#region "Public Properties"
        [PrimaryKey()]
	public int IdReciboCOMPago
	{
		get{return _idReciboCOMPago ;}
		set{_idReciboCOMPago = value;}
	}
	public int? IdReciboCOM
	{
		get{return _idReciboCOM;}
		set{_idReciboCOM = value;}
	}

	public int IdCuentaCorriente
	{
		get{return _idCuentaCorriente;}
		set{_idCuentaCorriente = value;}
	}

	public int IdAfiliado
	{
		get{return _idAfiliado;}
		set{_idAfiliado = value;}
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

	public int? IdTipoDocumento
	{
		get{return _idTipoDocumento;}
		set{_idTipoDocumento = value;}
	}

	public int? NumeroDocumento
	{
		get{return _numeroDocumento;}
		set{_numeroDocumento = value;}
	}

	public string Domicilio
	{
		get{return _domicilio == null ? string.Empty : _domicilio ;}
		set{_domicilio = value;}
	}

	public byte[] Firma
	{
		get{return _firma;}
		set{_firma = value;}
	}

	#endregion
	}
}
