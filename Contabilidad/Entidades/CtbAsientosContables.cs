
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Contabilidad.Entidades
{
  [Serializable]
	public partial class CtbAsientosContables : Objeto
	{
		// Class CtbAsientosContables
	#region "Private Members"
	int _idAsientoContable;
	string _numeroAsiento;
	string _detalleGeneral;
	DateTime _fechaAsiento;
	int _idTipoOperacion;
	int? _idRefTipoOperacion;
	string _numeroAsientoCopiativo;
	int? _idEjercicioContable;
	DateTime? _fechaRealizado;
    DateTime? _fechaAsientoDesde;
    DateTime? _fechaAsientoHasta;

	List<CtbAsientosContablesDetalles> _asientosContablesDetalles;
	#endregion
		
	#region "Constructors"
	public CtbAsientosContables()
	{
	}
	#endregion
		
	#region "Public Properties"
	public int IdAsientoContable
	{
		get{return _idAsientoContable ;}
		set{_idAsientoContable = value;}
	}
	public string NumeroAsiento
	{
		get{return _numeroAsiento == null ? string.Empty : _numeroAsiento ;}
		set{_numeroAsiento = value;}
	}

	public string DetalleGeneral
	{
		get{return _detalleGeneral == null ? string.Empty : _detalleGeneral ;}
		set{_detalleGeneral = value;}
	}

	public DateTime FechaAsiento
	{
		get{return _fechaAsiento;}
		set{_fechaAsiento = value;}
	}

	public int IdTipoOperacion
	{
		get{return _idTipoOperacion;}
		set{_idTipoOperacion = value;}
	}

	public int? IdRefTipoOperacion
	{
		get{return _idRefTipoOperacion;}
		set{_idRefTipoOperacion = value;}
	}

	public string NumeroAsientoCopiativo
	{
		get{return _numeroAsientoCopiativo == null ? string.Empty : _numeroAsientoCopiativo ;}
		set{_numeroAsientoCopiativo = value;}
	}

	public int? IdEjercicioContable
	{
		get{return _idEjercicioContable;}
		set{_idEjercicioContable = value;}
	}

	public DateTime? FechaRealizado
	{
		get{return _fechaRealizado;}
		set{_fechaRealizado = value;}
	}

    public DateTime? FechaAsientoDesde
    {
        get { return _fechaAsientoDesde; }
        set { _fechaAsientoDesde = value; }
    }

    public DateTime? FechaAsientoHasta
    {
        get { return _fechaAsientoHasta; }
        set { _fechaAsientoHasta = value; }
    }

	public List<CtbAsientosContablesDetalles> AsientosContablesDetalles
	{
		get{return _asientosContablesDetalles==null ? (_asientosContablesDetalles = new List<CtbAsientosContablesDetalles>()) : _asientosContablesDetalles;}
		set{_asientosContablesDetalles = value;}
	}

    public decimal TotalDebe
    {
        get
        {
            decimal total = 0;
            foreach (var asientoContableDetalle in AsientosContablesDetalles)
            {
                total += Convert.ToDecimal(asientoContableDetalle.Debe);
            }
            return total;
        }
    }

    public decimal TotalHaber
    {
        get
        {
            decimal total = 0;
            foreach (var asientoContableDetalle in AsientosContablesDetalles)
            {
                total += Convert.ToDecimal(asientoContableDetalle.Haber);
            }
            return total;
        }
    }

	#endregion
	}
}
