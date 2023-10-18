
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Facturas.Entidades
{
  [Serializable]
	public partial class VTAFacturacionesHabituales : Objeto
	{
		// Class VTAFacturacionesHabituales
	#region "Private Members"
	int _idFacturacionHabitual;
	int _idAfiliado;
	DateTime _fechaAlta;
	string _descripcion;
	VTATiposFacturacionesHabituales _tipoFacturacionHabitual;
	decimal _incrementoPorcentaje;
	int _incrementoPeriodoMeses;
	int _periodoInicio;
	int? _periodoFin;
    TGETiposFacturas _tipoFactura;
	int? _facturaDiaVencimiento;
    string _correoElectronico;
    List<VTAFacturacionesHabitualesDetalles> _facturacionesHabitualesDetalles;
	private List<TGECampos> _campos;
		#endregion

		#region "Constructors"
		public VTAFacturacionesHabituales()
	{
	}
	#endregion
		
	#region "Public Properties"
      [PrimaryKey]
	public int IdFacturacionHabitual
	{
		get{return _idFacturacionHabitual ;}
		set{_idFacturacionHabitual = value;}
	}
	public int IdAfiliado
	{
		get{return _idAfiliado;}
		set{_idAfiliado = value;}
	}

	public DateTime FechaAlta
	{
		get{return _fechaAlta;}
		set{_fechaAlta = value;}
	}
      [Auditoria]
	public string Descripcion
	{
		get{return _descripcion == null ? string.Empty : _descripcion ;}
		set{_descripcion = value;}
	}
      [Auditoria]
      public VTATiposFacturacionesHabituales TipoFacturacionHabitual
	{
        get { return _tipoFacturacionHabitual == null ? (_tipoFacturacionHabitual = new VTATiposFacturacionesHabituales()) : _tipoFacturacionHabitual; }
		set{_tipoFacturacionHabitual = value;}
	}
      [Auditoria]
	public decimal IncrementoPorcentaje
	{
		get{return _incrementoPorcentaje;}
		set{_incrementoPorcentaje = value;}
	}
      [Auditoria]
	public int IncrementoPeriodoMeses
	{
		get{return _incrementoPeriodoMeses;}
		set{_incrementoPeriodoMeses = value;}
	}
      [Auditoria]
	public int PeriodoInicio
	{
		get{return _periodoInicio;}
		set{_periodoInicio = value;}
	}
      [Auditoria]
	public int? PeriodoFin
	{
		get{return _periodoFin;}
		set{_periodoFin = value;}
	}

      [Auditoria]
      public TGETiposFacturas TipoFactura
      {
          get { return _tipoFactura == null ? (_tipoFactura = new TGETiposFacturas()) : _tipoFactura; }
          set { _tipoFactura = value; }
      }

      [Auditoria]
	public int? FacturaDiaVencimiento
	{
		get{return _facturaDiaVencimiento;}
		set{_facturaDiaVencimiento = value;}
	}

      [Auditoria]
      public string CorreoElectronico
      {
          get { return _correoElectronico == null ? string.Empty : _correoElectronico; }
          set { _correoElectronico = value; }
      }

      public List<VTAFacturacionesHabitualesDetalles> FacturacionesHabitualesDetalles
      {
          get { return _facturacionesHabitualesDetalles == null ? (_facturacionesHabitualesDetalles = new List<VTAFacturacionesHabitualesDetalles>()) : _facturacionesHabitualesDetalles; }
          set { _facturacionesHabitualesDetalles = value; }
      }

		public List<TGECampos> Campos
		{
			get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
			set { _campos = value; }
		}
		#endregion
	}
}
