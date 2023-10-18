
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using System.Xml;

namespace Contabilidad.Entidades
{
  [Serializable]
	public partial class CtbAsientosContables : Objeto
	{
		// Class CtbAsientosContables
	#region "Private Members"
	int _idAsientoContable;
    int _idAsientoContableLog;
	string _numeroAsiento;
	string _detalleGeneral;
	DateTime _fechaAsiento;
    int _idTipoOperacion;
    string _tipoOperacion;
	int? _idRefTipoOperacion;
	string _numeroAsientoCopiativo;
	int? _idEjercicioContable;
    string _ejercicioDescripcion;
	DateTime? _fechaRealizado;
    DateTime? _fechaAsientoDesde;
    DateTime? _fechaAsientoHasta;
    TGEFiliales _filial;
    CtbAsientosContablesTipos _asientoContableTipo;
	List<CtbAsientosContablesDetalles> _asientosContablesDetalles;
    int _resultado;
    decimal _totalDebe;
    decimal _totalHaber;
        XmlDocumentSerializationWrapper _loteAsientosContablesDetalles;
        #endregion

        #region "Constructors"
        public CtbAsientosContables()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey]
	public int IdAsientoContable
	{
		get{return _idAsientoContable ;}
		set{_idAsientoContable = value;}
	}
      public int IdAsientoContableLog
      {
          get { return _idAsientoContableLog; }
          set { _idAsientoContableLog = value; }
      }
      [Auditoria]
	public string NumeroAsiento
	{
		get{return _numeroAsiento == null ? string.Empty : _numeroAsiento ;}
		set{_numeroAsiento = value;}
	}

      [Auditoria]
	public string DetalleGeneral
	{
		get{return _detalleGeneral == null ? string.Empty : _detalleGeneral ;}
		set{_detalleGeneral = value;}
	}
      [Auditoria]
	public DateTime FechaAsiento
	{
		get{return _fechaAsiento;}
		set{_fechaAsiento = value;}
	}

    public string TipoOperacion
    {
        get { return _tipoOperacion == null ? string.Empty : _tipoOperacion; }
        set { _tipoOperacion = value; }
    }

    public int IdTipoOperacion
    {
        get { return _idTipoOperacion; }
        set { _idTipoOperacion = value; }
    }

	public int? IdRefTipoOperacion
	{
		get{return _idRefTipoOperacion;}
		set{_idRefTipoOperacion = value;}
	}

	public string NumeroAsientoCopiativo
	{
		get{return _numeroAsientoCopiativo == null ? string.Empty : _numeroAsientoCopiativo ;}
		set{_numeroAsientoCopiativo = value;}
	}

	public int? IdEjercicioContable
	{
		get{return _idEjercicioContable;}
		set{_idEjercicioContable = value;}
	}

    public string EjercicioDescripcion
    {
        get { return _ejercicioDescripcion == null ? string.Empty : _ejercicioDescripcion; }
        set { _ejercicioDescripcion = value; }
    }

	public DateTime? FechaRealizado
	{
		get{return _fechaRealizado;}
		set{_fechaRealizado = value;}
	}

    public DateTime? FechaAsientoDesde
    {
        get { return _fechaAsientoDesde; }
        set { _fechaAsientoDesde = value; }
    }

    public DateTime? FechaAsientoHasta
    {
        get { return _fechaAsientoHasta; }
        set { _fechaAsientoHasta = value; }
    }

	public List<CtbAsientosContablesDetalles> AsientosContablesDetalles
	{
		get{return _asientosContablesDetalles==null ? (_asientosContablesDetalles = new List<CtbAsientosContablesDetalles>()) : _asientosContablesDetalles;}
		set{_asientosContablesDetalles = value;}
	}

    public TGEFiliales Filial
    {
        get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
        set { _filial = value; }
    }

    public CtbAsientosContablesTipos AsientoContableTipo
    {
        get { return _asientoContableTipo == null ? (_asientoContableTipo = new CtbAsientosContablesTipos()) : _asientoContableTipo; }
        set { _asientoContableTipo = value; }
    }

    public decimal TotalDebe
    {
        get
        {
            return _totalDebe;
        }
        set { _totalDebe = value; }
    }

    public decimal TotalHaber
    {
        get
        {
            return _totalHaber;
        }
        set { _totalHaber = value; }
    }

    public int Resultado
    {
        get { return _resultado; }
        set { _resultado = value; }
    }

        public XmlDocument LoteAsientosContablesDetalles
        {
            get { return _loteAsientosContablesDetalles; }
            set { _loteAsientosContablesDetalles = value; }
        }
        #endregion

        public void CargarLoteAsientosContablesDetalles()
        {
            this.LoteAsientosContablesDetalles = new XmlDocument();

            XmlNode items = this.LoteAsientosContablesDetalles.CreateElement("AsientosContablesDetalles");
            this.LoteAsientosContablesDetalles.AppendChild(items);

            XmlNode item;
            XmlAttribute attribute;
            foreach (CtbAsientosContablesDetalles dato in this.AsientosContablesDetalles)
            {
                item = this.LoteAsientosContablesDetalles.CreateElement("AsientoContableDetalle");

                attribute = this.LoteAsientosContablesDetalles.CreateAttribute("IdCuentaContable");
                attribute.Value = dato.CuentaContable.IdCuentaContable.ToString();
                item.Attributes.Append(attribute);
                items.AppendChild(item);

                attribute = this.LoteAsientosContablesDetalles.CreateAttribute("NumeroCuenta");
                attribute.Value = dato.CuentaContable.NumeroCuenta;
                item.Attributes.Append(attribute);
                items.AppendChild(item);

            }
        }
    }
}
