
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace CuentasPagar.Entidades
{
  [Serializable]
	public partial class CapOrdenesPagosSolicitudesPagos : Objeto
	{
		// Class CapOrdenesPagosSolicitudesPagos
	#region "Private Members"
	int _idOrdenPagoSolicitudPago;
	int _idOrdenPago;
	int _idSolicitudPago;
	decimal _importePago;
	#endregion
		
	#region "Constructors"
	public CapOrdenesPagosSolicitudesPagos()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdOrdenPagoSolicitudPago
	{
		get{return _idOrdenPagoSolicitudPago ;}
		set{_idOrdenPagoSolicitudPago = value;}
	}
	public int IdOrdenPago
	{
		get{return _idOrdenPago;}
		set{_idOrdenPago = value;}
	}

	public int IdSolicitudPago
	{
		get{return _idSolicitudPago;}
		set{_idSolicitudPago = value;}
	}

	public decimal ImportePago
	{
		get{return _importePago;}
		set{_importePago = value;}
	}


	#endregion
	}
}
