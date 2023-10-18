
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Contabilidad.Entidades
{
  [Serializable]
    public partial class CtbCuentasContables : CtbCuentasContablesBase
	{

	#region "Private Members"
	int _idCuentaContable;
    
	#endregion
		
	#region "Constructors"
	public CtbCuentasContables()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey()]
      [Auditoria()]
	public int IdCuentaContable
	{
		get{return _idCuentaContable ;}
		set{_idCuentaContable = value;}
	}
		/// <summary>
		/// Prop agregada para filtro de grilla
		/// </summary>
		public int? IdFilial { get; set; }
		public string CuentaContable { get; set; }
		public string CuentaContableCompleta { get; set; }
		#endregion
	}
}
