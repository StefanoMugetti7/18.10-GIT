
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Expedientes.Entidades
{
  [Serializable]
	public partial class ExpExpedientes : Objeto
	{

	#region "Private Members"
	int _idExpediente;
    string _titulo;
    string _descripcion;
    int _idEstadoExpedienteTracking;
    DateTime _fechaExpediente;
    ExpExpedientesTipos _expedienteTipo;
	List<ExpExpedientesTracking> _expedientesTracking;
    List<TGEArchivos> _archivos;
    List<TGEComentarios> _comentarios;
    ExpExpedientesTracking _expedienteTracking;
    ExpExpedientesTracking _expedienteDerivado;
    bool _incluir;
	#endregion
		
	#region "Constructors"
	public ExpExpedientes()
	{
	}
	#endregion
		
	#region "Public Properties"
    [PrimaryKey()]
	public int IdExpediente
	{
		get{return _idExpediente ;}
		set{_idExpediente = value;}
	}
    
    [Auditoria()]
	public string Titulo
	{
		get{return _titulo == null ? string.Empty : _titulo ;}
		set{_titulo = value;}
	}
    [Auditoria()]
	public string Descripcion
	{
		get{return _descripcion == null ? string.Empty : _descripcion ;}
		set{_descripcion = value;}
	}

      /// <summary>
      /// Se utiliza para el filtro de Listar Expedientes
      /// </summary>
    public int IdEstadoExpedienteTracking
    {
        get { return _idEstadoExpedienteTracking; }
        set { _idEstadoExpedienteTracking = value; }
    }

    public DateTime FechaExpediente
    {
        get { return _fechaExpediente; }
        set { _fechaExpediente = value; }
    }

    [Auditoria()]
    public ExpExpedientesTipos ExpedienteTipo
    {
        get { return _expedienteTipo == null ? (_expedienteTipo = new ExpExpedientesTipos()) : _expedienteTipo; }
        set { _expedienteTipo = value; }
    }

	public List<ExpExpedientesTracking> ExpedientesTracking
	{
		get{return _expedientesTracking==null ? (_expedientesTracking = new List<ExpExpedientesTracking>()) : _expedientesTracking;}
		set{_expedientesTracking = value;}
	}

    public List<TGEArchivos> Archivos
    {
        get { return _archivos == null ? (_archivos = new List<TGEArchivos>()) : _archivos; }
        set { _archivos = value; }
    }

    public List<TGEComentarios> Comentarios
    {
        get { return _comentarios == null ? (_comentarios = new List<TGEComentarios>()) : _comentarios; }
        set { _comentarios = value; }
    }

      /// <summary>
      /// Ultimo Tracking de Expediente en estado Activo
      /// </summary>
    public ExpExpedientesTracking ExpedienteTracking
    {
        get { return _expedienteTracking == null ? (_expedienteTracking = new ExpExpedientesTracking()) : _expedienteTracking; }
        set { _expedienteTracking = value; }
    }
      /// <summary>
      /// Ultimo Tracking de Expediente SI esta en estado Derivado
      /// </summary>
    public ExpExpedientesTracking ExpedienteDerivado
    {
        get { return _expedienteDerivado == null ? (_expedienteDerivado = new ExpExpedientesTracking()) : _expedienteDerivado; }
        set { _expedienteDerivado = value; }
    }

    public bool Incluir
    {
        get { return _incluir; }
        set { _incluir = value; }
    }

	#endregion
	}
}
