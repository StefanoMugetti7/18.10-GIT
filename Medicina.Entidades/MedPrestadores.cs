
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Afiliados.Entidades;
using Generales.Entidades;
namespace Medicina.Entidades
{
  [Serializable]
	public partial class MedPrestadores : Objeto
	{
		// Class MedPrestadores
	#region "Private Members"
	int _idPrestador;
	int? _idProveedor;
	string _nombre;
	string _apellido;
	AfiTiposDocumentos _tipoDocumento;
	long _numeroDocumento;
	long _cUIL;
	TGESexos _sexo;
	DateTime _fechaNacimiento;
	DateTime _fechaIngreso;
	AfiEstadoCivil _estadoCivil;
	string _matricula;
	string _correoElectronico;
	int? _idGrupoSanguieno;
	DateTime? _fechaBaja;
	byte[] _foto;
	byte[] _firma;
	int? _idConsultorio;
	int _idusuarioAlta;
	DateTime _fechaAlta;
	List<MedPrestaciones> _medPrestaciones;
	List<MedPrestadoresDiasHoras> _medPrestadoresDiasHoras;
	List<MedPrestadoresEspecializaciones> _medPrestadoresEspecializaciones;
	List<MedTurneras> _medTurneras;
    List<TGEArchivos> _archivos;
    List<TGEComentarios> _comentarios;
    List<TGECampos> _campos;
	#endregion
		
	#region "Constructors"
	public MedPrestadores()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdPrestador
	{
		get{return _idPrestador ;}
		set{_idPrestador = value;}
	}
	public int? IdProveedor
	{
		get{return _idProveedor;}
		set{_idProveedor = value;}
	}

	public string Nombre
	{
		get{return _nombre == null ? string.Empty : _nombre ;}
		set{_nombre = value;}
	}

	public string Apellido
	{
		get{return _apellido == null ? string.Empty : _apellido ;}
		set{_apellido = value;}
	}

    public string ApellidoNombre
    {
        get { return string.Concat(this.Apellido, this.Nombre.Length > 0 ?  ", " : " ", this.Nombre); }
        set { }
    }

    public AfiTiposDocumentos TipoDocumento
	{
		get{return _tipoDocumento == null? (_tipoDocumento=new AfiTiposDocumentos()):_tipoDocumento;}
		set{_tipoDocumento = value;}
	}

	public long NumeroDocumento
	{
		get{return _numeroDocumento;}
		set{_numeroDocumento = value;}
	}

	public long CUIL
	{
		get{return _cUIL;}
		set{_cUIL = value;}
	}

    public TGESexos Sexo
	{
        get { return _sexo == null ? (_sexo = new TGESexos()) : _sexo; }
		set{_sexo = value;}
	}

	public DateTime FechaNacimiento
	{
		get{return _fechaNacimiento;}
		set{_fechaNacimiento = value;}
	}

	public DateTime FechaIngreso
	{
		get{return _fechaIngreso;}
		set{_fechaIngreso = value;}
	}

	public AfiEstadoCivil EstadoCivil
	{
        get { return _estadoCivil == null ? (_estadoCivil = new AfiEstadoCivil()) : _estadoCivil; }
		set{_estadoCivil = value;}
	}

	public string Matricula
	{
		get{return _matricula == null ? string.Empty : _matricula ;}
		set{_matricula = value;}
	}

	public string CorreoElectronico
	{
		get{return _correoElectronico == null ? string.Empty : _correoElectronico ;}
		set{_correoElectronico = value;}
	}

	public int? IdGrupoSanguieno
	{
		get{return _idGrupoSanguieno;}
		set{_idGrupoSanguieno = value;}
	}

	public DateTime? FechaBaja
	{
		get{return _fechaBaja;}
		set{_fechaBaja = value;}
	}

	public byte[] Foto
	{
		get{return _foto;}
		set{_foto = value;}
	}

	public byte[] Firma
	{
		get{return _firma;}
		set{_firma = value;}
	}

	public int? IdConsultorio
	{
		get{return _idConsultorio;}
		set{_idConsultorio = value;}
	}

	public int IdusuarioAlta
	{
		get{return _idusuarioAlta;}
		set{_idusuarioAlta = value;}
	}

	public DateTime FechaAlta
	{
		get{return _fechaAlta;}
		set{_fechaAlta = value;}
	}


	public List<MedPrestaciones> Prestaciones
	{
		get{return _medPrestaciones==null ? (_medPrestaciones = new List<MedPrestaciones>()) : _medPrestaciones;}
		set{_medPrestaciones = value;}
	}				
	public List<MedPrestadoresDiasHoras> PrestadoresDiasHoras
	{
		get{return _medPrestadoresDiasHoras==null ? (_medPrestadoresDiasHoras = new List<MedPrestadoresDiasHoras>()) : _medPrestadoresDiasHoras;}
		set{_medPrestadoresDiasHoras = value;}
	}				
	public List<MedPrestadoresEspecializaciones> PrestadoresEspecializaciones
	{
		get{return _medPrestadoresEspecializaciones==null ? (_medPrestadoresEspecializaciones = new List<MedPrestadoresEspecializaciones>()) : _medPrestadoresEspecializaciones;}
		set{_medPrestadoresEspecializaciones = value;}
	}

    public List<MedEspecializaciones> ObtenerEspecializaciones()
    {
        List<MedEspecializaciones> resultado = new List<MedEspecializaciones>();
        this.PrestadoresEspecializaciones.ForEach(x=> resultado.Add(x.Especializacion));
        return resultado;
    }

	public List<MedTurneras> Turneras
	{
		get{return _medTurneras==null ? (_medTurneras = new List<MedTurneras>()) : _medTurneras;}
		set{_medTurneras = value;}
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

    public List<TGECampos> Campos
    {
        get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
        set { _campos = value; }
    }
	#endregion
	}
}
