
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Seguridad.Entidades;
using Generales.Entidades;
using System.Linq;
namespace Afiliados.Entidades
{
  [Serializable]
	public partial class AfiAfiliados : Objeto
	{
	#region "Private Members"
	int _idAfiliado;
	int _idAfiliadoRef;
    List<AfiAfiliados> _familiares;
    List<AfiAfiliados> _apoderados;
	string _nombre;
	string _apellido;
    DateTime _fechaAlta;
    string _correoElectronico;
	AfiTiposDocumentos _tipoDocumento;
	int _numeroDocumento;
    long _cUIL;
	string _numeroSocio;
	AfiSexos _sexo;
	DateTime _fechaNacimiento;
	DateTime _fechaIngreso;
	AfiEstadoCivil _estadoCivil;
	int _matriculaIAF;
	AfiCategorias _categoria;
	AfiGruposSanguienos _grupoSanguieno;
	AfiGrados _grado;
	DateTime? _fechaRetiro;
	DateTime? _fechaFallecimiento;
    DateTime? _fechaBaja;
	byte[] _foto;
	byte[] _firma;
    TGEFiliales _filial;
    UsuariosAlta _usuarioAlta;
    List<AfiDomicilios> _domicilios;
    List<TGEArchivos> _archivos;
    List<TGEComentarios> _comentarios;
    List<AfiTelefonos> _telefonos;
    AfiParentesco _parentesco;
    AfiAfiliadosTipos _afiliadoTipo;
    List<AfiAlertasTipos> _alertasTipos;
    AfiTiposApoderados _tipoApoderado;
    int _cantidadParticipantes;
    int _idAfiliadoFallecido;
    string _numeroSocioFallecido;
    AfiAlertasTipos _alertaTipo;
    DateTime? _fechaSupervivencia;
    bool _confirmarBajaFamiliares;
    string _codigoZonaGrupo;
    decimal _importeExcedido;
    List<TGECampos> _campos;
    TGECondicionesFiscales _condicionFiscal;
	#endregion
		
	#region "Constructors"
	public AfiAfiliados()
	{
	}
	#endregion
		
	#region "Public Properties"
    [PrimaryKey()]
	public int IdAfiliado
	{
		get{return _idAfiliado ;}
		set{_idAfiliado = value;}
	}
	public int IdAfiliadoRef
	{
		get{return _idAfiliadoRef;}
		set{_idAfiliadoRef = value;}
	}

    public List<AfiAfiliados> Familiares
    {
        get { return _familiares == null ? (_familiares = new List<AfiAfiliados>()) : _familiares; }
        set { _familiares = value; }
    }

    public List<AfiAfiliados> Apoderados
    {
        get { return _apoderados == null ? (_apoderados = new List<AfiAfiliados>()) : _apoderados; }
        set { _apoderados = value; }
    }

    [Auditoria()]
	public string Nombre
	{
		get{return _nombre == null ? string.Empty : _nombre ;}
		set{_nombre = value;}
	}
    [Auditoria()]
	public string Apellido
	{
		get{return _apellido == null ? string.Empty : _apellido ;}
		set{_apellido = value;}
	}

    public string ApellidoNombre
    {
        get { return this.IdAfiliado>0 ? string.Concat(this.Apellido, this.Nombre.Length>0? ", " : string.Empty , this.Nombre) : string.Empty; }
        set { }
    }

    public DateTime FechaAlta
    {
        get { return _fechaAlta; }
        set { _fechaAlta = value; }
    }

    [Auditoria()]
    public string CorreoElectronico
    {
        get { return _correoElectronico == null ? string.Empty : _correoElectronico; }
        set { _correoElectronico = value; }
    }

    [Auditoria()]
    public AfiTiposDocumentos TipoDocumento
	{
        get { return _tipoDocumento == null ? (_tipoDocumento = new AfiTiposDocumentos()) : _tipoDocumento; }
		set{_tipoDocumento = value;}
	}

    [Auditoria()]
	public int NumeroDocumento
	{
		get{return _numeroDocumento;}
		set{_numeroDocumento = value;}
	}
    [Auditoria()]
	public long CUIL
	{
        get { return _cUIL; }
		set{_cUIL = value;}
	}

    [Auditoria()]
    public string CUILFormateado
    {
        get { return this.CUIL.ToString();}// >= 11 ? string.Concat(this.CUIL.ToString().Substring(0, 2), "-", this.CUIL.ToString().Substring(2, 9), "-", this.CUIL.ToString().Substring(10, 1)) : string.Empty; }
        set { }
    }

    [Auditoria()]
	public string NumeroSocio
	{
		get{return _numeroSocio == null ? string.Empty : _numeroSocio ;}
		set{_numeroSocio = value;}
	}
    [Auditoria()]
    public AfiSexos Sexo
	{
        get { return _sexo == null ? (_sexo = new AfiSexos()) : _sexo; }
		set{_sexo = value;}
	}
    [Auditoria()]
	public DateTime FechaNacimiento
	{
		get{return _fechaNacimiento;}
		set{_fechaNacimiento = value;}
	}
    [Auditoria()]
	public DateTime FechaIngreso
	{
		get{return _fechaIngreso;}
		set{_fechaIngreso = value;}
	}
    [Auditoria()]
	public AfiEstadoCivil EstadoCivil
	{
		get{return _estadoCivil==null? (_estadoCivil=new AfiEstadoCivil()):_estadoCivil;}
		set{_estadoCivil = value;}
	}
    [Auditoria()]
	public int MatriculaIAF
	{
		get{return _matriculaIAF;}
		set{_matriculaIAF = value;}
	}
    
    [Auditoria()]
	public AfiCategorias Categoria
	{
		get{return _categoria==null? (_categoria=new AfiCategorias()):_categoria;}
		set{_categoria = value;}
	}
    [Auditoria()]
	public AfiGruposSanguienos GrupoSanguieno
	{
		get{return _grupoSanguieno==null? (_grupoSanguieno=new AfiGruposSanguienos()): _grupoSanguieno;}
		set{_grupoSanguieno = value;}
	}
    [Auditoria()]
	public AfiGrados Grado
	{
		get{return _grado==null? (_grado=new AfiGrados()):_grado;}
		set{_grado = value;}
	}
    [Auditoria()]
	public DateTime? FechaRetiro
	{
		get{return _fechaRetiro;}
		set{_fechaRetiro = value;}
	}
    [Auditoria()]
	public DateTime? FechaFallecimiento
	{
		get{return _fechaFallecimiento;}
		set{_fechaFallecimiento = value;}
	}

    [Auditoria()]
    public DateTime? FechaBaja
    {
        get { return _fechaBaja; }
        set { _fechaBaja = value; }
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

    public TGEFiliales Filial
    {
        get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
        set { _filial = value; }
    }

    public UsuariosAlta UsuarioAlta
    {
        get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
        set { _usuarioAlta = value; }
    }

    public List<AfiDomicilios> Domicilios
    {
        get { return _domicilios == null ? (_domicilios = new List<AfiDomicilios>()) : _domicilios; }
        set { _domicilios = value; }
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

    public List<AfiTelefonos> Telefonos
    {
        get { return _telefonos == null ? (_telefonos = new List<AfiTelefonos>()) : _telefonos; }
        set { _telefonos = value; }
    }

    public AfiParentesco Parentesco
    {
        get { return _parentesco == null ? (_parentesco = new AfiParentesco()) : _parentesco; }
        set { _parentesco = value; }
    }

    public AfiAfiliadosTipos AfiliadoTipo
    {
        get { return _afiliadoTipo == null ? (_afiliadoTipo = new AfiAfiliadosTipos()) : _afiliadoTipo; }
        set { _afiliadoTipo = value; }
    }

    public List<AfiAlertasTipos> AlertasTipos
    {
        get { return _alertasTipos == null ? (_alertasTipos = new List<AfiAlertasTipos>()) : _alertasTipos; }
        set { _alertasTipos = value; }
    }

    public string DomicilioPredeterminado
    {
        get { return this.Domicilios.Exists(x=>x.Predeterminado) ? this.Domicilios.Find(x=>x.Predeterminado).DomicilioCompleto : string.Empty;}
        set { }
    }

    public string TelefonoPredeterminado
    {
        get { return this.Telefonos.Count > 0 ? this.Telefonos[0].TelefonoCompleto : string.Empty; }
        set { }
    }

    [Auditoria()]
    public AfiTiposApoderados TipoApoderado
    {
        get { return _tipoApoderado == null ? (_tipoApoderado = new AfiTiposApoderados()) : _tipoApoderado; }
        set { _tipoApoderado = value; }
    }

    public string DatosAfiliado
    {
        get { return string.Concat(this.NumeroSocio, " - ", this.TipoDocumento.TipoDocumento, " ", this.NumeroDocumento, " ", this.ApellidoNombre); }
        set { }
    }

    public int CantidadParticipantes
    {
        get
        {
            return _cantidadParticipantes;
            //return this.Familiares.Count > 0 ?
            //    this.Familiares.Where(x => x.Estado.IdEstado == (int)EstadosAfiliados.AvisodeFallecido
            //  || x.Estado.IdEstado == (int)EstadosAfiliados.Moroso
            //  || x.Estado.IdEstado == (int)EstadosAfiliados.Normal
            //  || x.Estado.IdEstado == (int)EstadosAfiliados.Suspendido
            //  || x.Estado.IdEstado == (int)EstadosAfiliados.Vitalicio).ToList().Count
            //  : 0;
        }
        set { _cantidadParticipantes = value; }
    }

    public int IdAfiliadoFallecido
    {
        get { return _idAfiliadoFallecido; }
        set { _idAfiliadoFallecido = value; }
    }

    public string NumeroSocioFallecido
    {
        get { return _numeroSocioFallecido == null ? string.Empty : _numeroSocioFallecido; }
        set { _numeroSocioFallecido = value; }
    }

    public AfiAlertasTipos AlertaTipo
    {
        get { return _alertaTipo == null ? (_alertaTipo = new AfiAlertasTipos()) : _alertaTipo; }
        set { _alertaTipo = value; }
    }

      [Auditoria()]
    public DateTime? FechaSupervivencia
    {
        get { return _fechaSupervivencia; }
        set { _fechaSupervivencia = value; }
    }

    public bool ConfirmarBajaFamiliares
    {
        get { return _confirmarBajaFamiliares; }
        set { _confirmarBajaFamiliares = value; }
    }

    [Auditoria()]
    public string CodigoZonaGrupo
    {
        get { return _codigoZonaGrupo == null ? string.Empty : _codigoZonaGrupo; }
        set { _codigoZonaGrupo = value; }
    }

    public decimal ImporteExcedido
    {
        get { return _importeExcedido; }
        set { _importeExcedido = value; }
    }

    public List<TGECampos> Campos
    {
        get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
        set { _campos = value; }
    }

    [Auditoria()]
    public TGECondicionesFiscales CondicionFiscal
    {
        get { return _condicionFiscal == null ? (_condicionFiscal = new TGECondicionesFiscales()) : _condicionFiscal; }
        set { _condicionFiscal = value; }
    }

	#endregion
	}
}
