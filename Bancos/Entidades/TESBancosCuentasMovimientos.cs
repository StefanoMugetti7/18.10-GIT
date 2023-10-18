
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
using Contabilidad.Entidades;
using Afiliados.Entidades;
namespace Bancos.Entidades
{
  [Serializable]
	public partial class TESBancosCuentasMovimientos : Objeto
	{

	#region "Private Members"
	int _idBancoCuentaMovimiento;
    string _numeroTipoOperacion;
    decimal _importe;
    string _detalle;
	TESBancosCuentas _bancoCuenta;
    TESBancosCuentas _bancoCuentaDestino;
	DateTime _fechaMovimiento;
	TGETiposOperaciones _tipoOperacion;
    int _idRefTipoOperacion;
	DateTime _fechaConciliacion;
	int _idUsuarioConciliacion;
	DateTime _fechaConfirmacionBanco;
	UsuariosAlta _usuarioAlta;
	DateTime _fechaAlta;
	int _idTesoreriaMovimiento;
    CtbConceptosContables _conceptoContable;
    AfiAfiliados _afiliado;
    List<TGEArchivos> _archivos;
    List<TGEComentarios> _comentarios;
    int _idCajaMovimientoValor;
    string _detalleCheque;
	#endregion
		
	#region "Constructors"
	public TESBancosCuentasMovimientos()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdBancoCuentaMovimiento
	{
		get{return _idBancoCuentaMovimiento ;}
		set{_idBancoCuentaMovimiento = value;}
	}

      [Auditoria()]
      public decimal Importe
      {
          get { return _importe; }
          set { _importe = value; }
      }
      [Auditoria()]
      public string Detalle
      {
          get { return _detalle == null ? string.Empty : _detalle; }
          set { _detalle = value; }
      }

      [Auditoria()]
      public string NumeroTipoOperacion
      {
          get { return _numeroTipoOperacion == null ? string.Empty : _numeroTipoOperacion; }
          set { _numeroTipoOperacion = value; }
      }

      [Auditoria()]
	public TESBancosCuentas BancoCuenta
	{
        get { return _bancoCuenta == null ? (_bancoCuenta = new TESBancosCuentas()) : _bancoCuenta; }
		set{_bancoCuenta = value;}
	}

      [Auditoria()]
      public TESBancosCuentas BancoCuentaDestino
      {
          get { return _bancoCuentaDestino == null ? (_bancoCuentaDestino = new TESBancosCuentas()) : _bancoCuentaDestino; }
          set { _bancoCuentaDestino = value; }
      }

      [Auditoria()]
	public DateTime FechaMovimiento
	{
		get{return _fechaMovimiento;}
		set{_fechaMovimiento = value;}
	}
      [Auditoria()]
	public TGETiposOperaciones TipoOperacion
	{
        get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
		set{_tipoOperacion = value;}
	}
      [Auditoria()]
      public int IdRefTipoOperacion
      {
          get { return _idRefTipoOperacion; }
          set { _idRefTipoOperacion = value; }
      }
      
      [Auditoria()]
	public DateTime FechaConciliacion
	{
		get{return _fechaConciliacion;}
		set{_fechaConciliacion = value;}
	}
      [Auditoria()]
	public int IdUsuarioConciliacion
	{
		get{return _idUsuarioConciliacion;}
		set{_idUsuarioConciliacion = value;}
	}
      [Auditoria()]
	public DateTime FechaConfirmacionBanco
	{
		get{return _fechaConfirmacionBanco;}
		set{_fechaConfirmacionBanco = value;}
	}

	public UsuariosAlta UsuarioAlta
	{
        get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
		set{_usuarioAlta = value;}
	}

	public DateTime FechaAlta
	{
		get{return _fechaAlta;}
		set{_fechaAlta = value;}
	}

	public int IdTesoreriaMovimiento
	{
        get { return _idTesoreriaMovimiento; }
		set{_idTesoreriaMovimiento = value;}
	}

    public int IdCajaMovimientoValor
    {
        get { return _idCajaMovimientoValor; }
        set { _idCajaMovimientoValor = value; }
    }

    public CtbConceptosContables ConceptoContable
    {
        get { return _conceptoContable == null ? (_conceptoContable = new CtbConceptosContables()) : _conceptoContable; }
        set { _conceptoContable = value; }
    }

    public AfiAfiliados Afiliado
    {
        get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
        set { _afiliado = value; }
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

    public string DetalleCheque
    {
        get { return _detalleCheque == null ? string.Empty : _detalleCheque; }
        set { _detalleCheque = value; }
    }
	#endregion
	}
}
