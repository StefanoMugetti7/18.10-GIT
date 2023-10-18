using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Generales.Entidades;

namespace Contabilidad.Entidades
{
    [Serializable]
    public class CtbCuentasContablesBase : Objeto
    {
        #region "Private Members"
	int _imputacion;
	string _descripcion;
    string _numeroCuenta;
	DateTime _fechaAlta;
	int _idUsuarioAlta;
    int _idCuentaContableRama;
    string _descripcionRama;
    bool _imputable;
    int _idRefTipoOperacionCuentaContable;
    DateTime _fechaDesde;
    DateTime _fechaHasta;
    int _idEjercicioContable;
    List<TGECampos> _campos;
    int _nivel;
    bool _tieneMovimientos;
    bool _centroCostoObligatorio;
    CtbCentrosCostosProrrateos _centroCostoProrrateo;
    CtbCapitulos _capitulo;
    CtbRubros _rubro;
    CtbMonedas _moneda;
    CtbDepartamentos _departamento;
    CtbSubRubros _subRubro;
    bool _monetaria;
	#endregion
		
	#region "Constructors"
    public CtbCuentasContablesBase()
	{
	}
	#endregion
		
	#region "Public Properties"

      [Auditoria()]
	public int Imputacion
	{
		get{return _imputacion;}
		set{_imputacion = value;}
	}

      [Auditoria()]
	public string Descripcion
	{
		get{return _descripcion == null ? string.Empty : _descripcion ;}
		set{_descripcion = value;}
	}
      [Auditoria()]
    public string NumeroCuenta
    {
        get { return _numeroCuenta == null ? string.Empty : _numeroCuenta; }
        set { _numeroCuenta = value; }
    }
      [Auditoria()]
    public int IdCuentaContableRama
    {
        get { return _idCuentaContableRama; }
        set { _idCuentaContableRama = value; }
    }
      [Auditoria()]
    public string DescripcionRama
    {
        get { return _descripcionRama == null ? string.Empty : _descripcionRama; }
        set { _descripcionRama = value; }
    }
      [Auditoria()]
    public bool Imputable
    {
        get { return _imputable; }
        set { _imputable = value; }
    }

	public DateTime FechaAlta
	{
		get{return _fechaAlta;}
		set{_fechaAlta = value;}
	}

	public int IdUsuarioAlta
	{
		get{return _idUsuarioAlta;}
		set{_idUsuarioAlta = value;}
	}

    public int IdRefTipoOperacionCuentaContable
    {
        get { return _idRefTipoOperacionCuentaContable; }
        set { _idRefTipoOperacionCuentaContable = value; }
    }

    public DateTime FechaDesde
    {
        get { return _fechaDesde; }
        set { _fechaDesde = value; }
    }

public List<TGECampos> Campos
    {
        get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
        set { _campos = value; }
    }
    [Auditoria()]
    public int Nivel
    {
        get { return _nivel; }
        set { _nivel = value; }
    }

    public bool TieneMovimientos
    {
        get { return _tieneMovimientos; }
        set { _tieneMovimientos = value; }
    }

    public DateTime FechaHasta
    {
        get { return _fechaHasta; }
        set { _fechaHasta = value; }
    }

    public int IdEjercicioContable
    {
        get { return _idEjercicioContable; }
        set { _idEjercicioContable = value; }
    }

    [Auditoria()]
    public bool CentroCostoObligatorio
    {
        get { return _centroCostoObligatorio; }
        set { _centroCostoObligatorio = value; }
    }

    [Auditoria()]
    public bool Monetaria
    {
        get { return _monetaria; }
        set { _monetaria = value; }
    }

       [Auditoria()]
    public CtbCentrosCostosProrrateos CentroCostoProrrateo
    {
        get { return _centroCostoProrrateo == null ? (_centroCostoProrrateo = new CtbCentrosCostosProrrateos()) : _centroCostoProrrateo; }
        set { _centroCostoProrrateo = value; }
    }

    [Auditoria()]
    public CtbCapitulos Capitulo
    {
        get { return _capitulo == null ? (_capitulo = new CtbCapitulos()) : _capitulo; }
        set { _capitulo = value; }
    }
    [Auditoria()]
    public CtbRubros Rubro
    {
        get { return _rubro == null ? (_rubro = new CtbRubros()) : _rubro; }
        set { _rubro = value; }
    }
    [Auditoria()]
    public CtbMonedas Moneda
    {
        get { return _moneda == null ? (_moneda = new CtbMonedas()) : _moneda; }
        set { _moneda = value; }
    }
    [Auditoria()]
    public CtbDepartamentos Departamento
    {
        get { return _departamento == null ? (_departamento = new CtbDepartamentos()) : _departamento; }
        set { _departamento = value; }
    }
    [Auditoria()]
    public CtbSubRubros SubRubro
    {
        get { return _subRubro == null ? (_subRubro = new CtbSubRubros()) : _subRubro; }
        set { _subRubro = value; }
    }
	#endregion
    }

    [Serializable]
    public class CtbCuentasContablesDTO
        {
        public int id { get; set; }
        public int idPadre { get; set; }
        public string text { get; set; }
        public string numeroCuenta { get; set; }
        public bool imputable { get; set; }
        public List<CtbCuentasContablesDTO> children { get; set; }
    }
}
