
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Contabilidad.Entidades;
using Generales.Entidades;

namespace Compras.Entidades
{
  [Serializable]
	public partial class CMPFamilias : Objeto
	{

	#region "Private Members"
	int _idFamilia;
    bool _stockeable;
	string _descripcion;
    TGERegimenesRetencionesIIGG _regimenRetencionIIGG;
    bool _retieneSUSS;
    int _idUsuarioAlta;
    DateTime _fechaAlta;
        List<TGECampos> _campos;

        CtbCuentasContables _cuentacontable;
   CtbCuentasContablesGanancias _cuentaContableGanancia;
   CtbCuentasContablesActivos _cuentaContableActivo;
   CtbCuentasContablesCostoMercaderiaVendida _cuentaContableCostoMercaderiaVendida;
	#endregion
		
	#region "Constructors"
	public CMPFamilias()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey()]
	public int IdFamilia
	{
		get{return _idFamilia ;}
		set{_idFamilia = value;}
	}
      [Auditoria()]
    public bool Stockeable
	{
		get{return _stockeable  ;}
        set { _stockeable = value; }
	}
      [Auditoria()]
	public string Descripcion
	{
		get{return _descripcion == null ? string.Empty : _descripcion ;}
		set{_descripcion = value;}
	}

      [Auditoria()]
    public CtbCuentasContables CuentaContable
    {
        get { return _cuentacontable == null ? (_cuentacontable = new CtbCuentasContables()) : _cuentacontable; }
        set { _cuentacontable = value; }
    }

      [Auditoria()]
      public CtbCuentasContablesActivos CuentaContableActivo
      {
          get { return _cuentaContableActivo == null ? (_cuentaContableActivo = new CtbCuentasContablesActivos()) : _cuentaContableActivo; }
          set { _cuentaContableActivo = value; }
      }

      [Auditoria()]
      public CtbCuentasContablesGanancias CuentaContableGanancia
      {
          get { return _cuentaContableGanancia == null ? (_cuentaContableGanancia = new CtbCuentasContablesGanancias()) : _cuentaContableGanancia; }
          set { _cuentaContableGanancia = value; }
      }

        [Auditoria()]
        public CtbCuentasContablesCostoMercaderiaVendida CuentaContableCostoMercaderiaVendida
        {
            get { return _cuentaContableCostoMercaderiaVendida == null ? (_cuentaContableCostoMercaderiaVendida = new CtbCuentasContablesCostoMercaderiaVendida()) : _cuentaContableCostoMercaderiaVendida; }
            set { _cuentaContableCostoMercaderiaVendida = value; }
        }

        public TGERegimenesRetencionesIIGG RegimenRetencionIIGG
      {
          get { return _regimenRetencionIIGG == null ? (_regimenRetencionIIGG = new TGERegimenesRetencionesIIGG()): _regimenRetencionIIGG; }
          set { _regimenRetencionIIGG = value; }
      }

      public bool RetieneSUSS
      {
          get { return _retieneSUSS; }
          set { _retieneSUSS = value; }
      }

    public int IdUsuarioAlta
    {
        get { return _idUsuarioAlta; }
        set { _idUsuarioAlta = value; }
    }

    public DateTime FechaAlta
    {
        get { return _fechaAlta; }
        set { _fechaAlta = value; }
    }
        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
        }

        #endregion
    }
}
