using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Contabilidad.Entidades
{
    [Serializable]
    public class CtbCuentasContablesGanancias : CtbCuentasContables
    {
        #region "Private Members"
	int? _idCuentaContableGanancia;
    
	#endregion
		
	#region "Constructors"
    public CtbCuentasContablesGanancias()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey()]
      [Auditoria()]
	public int? IdCuentaContableGanancia
	{
		get{return _idCuentaContableGanancia ;}
		set{_idCuentaContableGanancia = value;}
	}
	#endregion
    }
}
