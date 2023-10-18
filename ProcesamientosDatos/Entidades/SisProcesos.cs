
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace ProcesosDatos.Entidades
{
  [Serializable]
	public partial class SisProcesos : Objeto
	{

	#region "Private Members"
	int _idProceso;
    string _descripcion;
	SisProcesosArchivos _procesoArchivo;
    bool _tieneArchivo;
    List<SisParametros> _parametros;
	#endregion
		
	#region "Constructors"
	public SisProcesos()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdProceso
	{
		get{return _idProceso ;}
		set{_idProceso = value;}
	}

	public string Descripcion
	{
		get{return _descripcion == null ? string.Empty : _descripcion ;}
		set{_descripcion = value;}
	}

    public bool TieneArchivo
    {
        get { return _tieneArchivo; }
        set { _tieneArchivo = value; }
    }

	public SisProcesosArchivos ProcesoArchivo
	{
        get { return _procesoArchivo == null ? (_procesoArchivo = new SisProcesosArchivos()) : _procesoArchivo; }
		set{_procesoArchivo = value;}
	}

    public List<SisParametros> Parametros
    {
        get { return _parametros == null ? (_parametros = new List<SisParametros>()) : _parametros; }
        set { _parametros = value; }
    }

	#endregion
	}
}
