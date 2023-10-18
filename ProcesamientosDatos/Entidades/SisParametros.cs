
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace ProcesosDatos.Entidades
{
  [Serializable]
	public partial class SisParametros : Objeto
	{
	#region "Private Members"
	int _idParametro;
	int _idProceso;
	string _parametro;
	int _orden;
	string _cnn;
	string _storedProcedure;
	string _nombreParametro;
	SisTiposParametros _tipoParametro;
	string _paramDependiente;
    object _valorParametro;
    bool _parametroDibujado;
	#endregion
		
	#region "Constructors"
	public SisParametros()
	{
	}
	#endregion
		
	#region "Public Properties"
      
    [PrimaryKey()]
	public int IdParametro
	{
		get{return _idParametro ;}
		set{_idParametro = value;}
	}
	public int IdProceso
	{
		get{return _idProceso;}
		set{_idProceso = value;}
	}
    [Auditoria()]
	public string Parametro
	{
		get{return _parametro == null ? string.Empty : _parametro ;}
		set{_parametro = value;}
	}

    [Auditoria()]
	public int Orden
	{
		get{return _orden;}
		set{_orden = value;}
	}

	public string Cnn
	{
		get{return _cnn == null ? string.Empty : _cnn ;}
		set{_cnn = value;}
	}
    [Auditoria()]
	public string StoredProcedure
	{
		get{return _storedProcedure == null ? string.Empty : _storedProcedure ;}
		set{_storedProcedure = value;}
	}
    [Auditoria()]
	public string NombreParametro
	{
		get{return _nombreParametro == null ? string.Empty : _nombreParametro ;}
		set{_nombreParametro = value;}
	}
    [Auditoria()]
    public SisTiposParametros TipoParametro
	{
        get { return _tipoParametro == null ? (_tipoParametro = new SisTiposParametros()) : _tipoParametro; }
		set{_tipoParametro = value;}
	}
    [Auditoria()]
	public string ParamDependiente
	{
		get{return _paramDependiente == null ? string.Empty : _paramDependiente ;}
		set{_paramDependiente = value;}
	}

    public bool ParametroDibujado
    {
        get { return _parametroDibujado; }
        set { _parametroDibujado = value; }
    }

    public object ValorParametro
    {
        get { return _valorParametro == null ? string.Empty : _valorParametro; }
        set { _valorParametro = value; }
    }

	#endregion
	}
}
