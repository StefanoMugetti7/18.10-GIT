
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;

namespace Medicina.Entidades
{
  [Serializable]
	public partial class MedPrestadoresEspecializaciones : Objeto
	{
		// Class MedPrestadoresEspecializaciones
	#region "Private Members"
	int _idPrestadorEspecializacion;
	int _idPrestador;
    MedEspecializaciones _especializacion;
	bool _especializacionPorDefecto;
        List<TGECampos> _campos;

        #endregion

        #region "Constructors"
        public MedPrestadoresEspecializaciones()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdPrestadorEspecializacion
	{
		get{return _idPrestadorEspecializacion ;}
		set{_idPrestadorEspecializacion = value;}
	}
	public int IdPrestador
	{
		get{return _idPrestador;}
		set{_idPrestador = value;}
	}

	public MedEspecializaciones Especializacion
	{
        get { return _especializacion == null ? (_especializacion = new MedEspecializaciones()) : _especializacion; }
		set{_especializacion = value;}
	}

	public bool EspecializacionPorDefecto
	{
		get{return _especializacionPorDefecto;}
		set{_especializacionPorDefecto = value;}
	}


        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
        }

        #endregion
    }
}
