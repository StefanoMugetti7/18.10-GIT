
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Expedientes.Entidades
{
  [Serializable]
	public partial class ExpExpedientesTracking : Objeto
	{

	#region "Private Members"
	int _idExpedienteTracking;
	int _idExpediente;
	TGESectores _sector;
	DateTime _fecha;
	#endregion
		
	#region "Constructors"
	public ExpExpedientesTracking()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdExpedienteTracking
	{
		get{return _idExpedienteTracking ;}
		set{_idExpedienteTracking = value;}
	}
	public int IdExpediente
	{
		get{return _idExpediente;}
		set{_idExpediente = value;}
	}

    public TGESectores Sector
	{
        get { return _sector == null ? (_sector = new TGESectores()) : _sector; }
		set{_sector = value;}
	}

	public DateTime Fecha
	{
		get{return _fecha;}
		set{_fecha = value;}
	}

	#endregion
	}
}
