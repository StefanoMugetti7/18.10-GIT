
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Facturas.Entidades
{
  [Serializable]
	public partial class AfipServiciosWebTickets : Objeto
	{
		// Class AfipServiciosWebTickets
	#region "Private Members"
	int _idLoginTicket;
	int _uniqueId;
	DateTime _generationTime;
	DateTime _expirationTime;
	string _sign;
	string _token;
	string _loginTicketResponse;

	#endregion
		
	#region "Constructors"
	public AfipServiciosWebTickets()
	{
	}
	#endregion
		
	#region "Public Properties"
	public int IdLoginTicket
	{
		get{return _idLoginTicket ;}
		set{_idLoginTicket = value;}
	}
	public int UniqueId
	{
		get{return _uniqueId;}
		set{_uniqueId = value;}
	}

	public DateTime GenerationTime
	{
		get{return _generationTime;}
		set{_generationTime = value;}
	}

	public DateTime ExpirationTime
	{
		get{return _expirationTime;}
		set{_expirationTime = value;}
	}

	public string Sign
	{
		get{return _sign == null ? string.Empty : _sign ;}
		set{_sign = value;}
	}

	public string Token
	{
		get{return _token == null ? string.Empty : _token ;}
		set{_token = value;}
	}

	public string LoginTicketResponse
	{
		get{return _loginTicketResponse == null ? string.Empty : _loginTicketResponse ;}
		set{_loginTicketResponse = value;}
	}

	#endregion
	}
}
