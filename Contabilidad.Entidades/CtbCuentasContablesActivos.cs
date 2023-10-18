using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Contabilidad.Entidades
{
    [Serializable]
    public class CtbCuentasContablesActivos : CtbCuentasContables
    {
        #region "Private Members"
	int? _idCuentaContableActivo;
    
	#endregion
		
	#region "Constructors"
    public CtbCuentasContablesActivos()
	{
	}
	#endregion
		
	#region "Public Properties"

      [PrimaryKey()]
      [Auditoria()]
	public int? IdCuentaContableActivo
	{
		get{return _idCuentaContableActivo ;}
		set{_idCuentaContableActivo = value;}
	}
	#endregion
    }
}
