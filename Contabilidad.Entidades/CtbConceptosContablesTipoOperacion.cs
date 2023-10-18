using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Contabilidad.Entidades
{
    [Serializable]
    public class CtbConceptosContablesTipoOperacion
    {
        	#region "Private Members"
        int _idConceptosContablesTipoOperacion;
        int? _idConceptoContable;
        int _idTipoOperacion;
	    #endregion
		
	    #region "Constructors"
        public CtbConceptosContablesTipoOperacion()
	    {
	    }
	    #endregion
		
	    #region "Public Properties"
        [Auditoria()]
        public int IdConceptosContablesTipoOperacion
        {
            get { return _idConceptosContablesTipoOperacion; }
            set { _idConceptosContablesTipoOperacion = value; }
        }
        [Auditoria()]
        public int? IdConceptoContable
	    {
            get { return _idConceptoContable; }
            set { _idConceptoContable = value; }
	    }

        [Auditoria()]
        public int IdTipoOperacion
	    {
            get { return _idTipoOperacion; }
            set { _idTipoOperacion = value; }
	    }
	    #endregion
    }
}
