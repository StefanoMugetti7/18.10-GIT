
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace ProcesosDatos.Entidades
{
  [Serializable]
	public partial class SisProcesosProcesamiento : Objeto
	{
	#region "Private Members"
	int _idProcesoProcesamiento;
	string _procesado;
	string _archivo;
	string _resultado;
	int _registrosProcesados;
	string _procesamientoEjecutado;
    int _periodo;
    SisProcesos _proceso;
	#endregion
		
	#region "Constructors"
	public SisProcesosProcesamiento()
	{
	}
	#endregion
		
	#region "Public Properties"
    [PrimaryKey()]
	public int IdProcesoProcesamiento
	{
		get{return _idProcesoProcesamiento ;}
		set{_idProcesoProcesamiento = value;}
	}

	public string Procesado
	{
		get{return _procesado == null ? string.Empty : _procesado ;}
		set{_procesado = value;}
	}

	public string Archivo
	{
		get{return _archivo == null ? string.Empty : _archivo ;}
		set{_archivo = value;}
	}

	public string Resultado
	{
		get{return _resultado == null ? string.Empty : _resultado ;}
		set{_resultado = value;}
	}

	public int RegistrosProcesados
	{
		get{return _registrosProcesados;}
		set{_registrosProcesados = value;}
	}

	public string ProcesamientoEjecutado
	{
		get{return _procesamientoEjecutado == null ? string.Empty : _procesamientoEjecutado ;}
		set{_procesamientoEjecutado = value;}
	}

    public int Periodo
    {
        get { return _periodo; }
        set { _periodo = value; }
    }

    public SisProcesos Proceso
    {
        get { return _proceso == null ? (_proceso = new SisProcesos()) : _proceso; }
        set { _proceso = value; }
    }


	#endregion
	}
}
