
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Proveedores.Entidades;
namespace Prestamos.Entidades
{
    [Serializable]
    public class PreCesionarios : Objeto
    {
    #region "Private Members"
	int _idCesionario;
    CapProveedores _proveedor;
	string _denominacion;
    DateTime _fechaCorte;
	DateTime _fechaAlta;
	int _idUsuarioAlta;

	#endregion
		
	#region "Constructors"
    public PreCesionarios()
	{
	}
	#endregion
		
	#region "Public Properties"

    [PrimaryKey()]
	public int IdCesionario
	{
		get{return _idCesionario;}
		set{_idCesionario = value;}
	}

        [Auditoria()]
    public CapProveedores Proveedor
    {
        get { return _proveedor == null ? (_proveedor = new CapProveedores()) : _proveedor; }
        set { _proveedor = value; }
    }

        [Auditoria()]
	public string Denominacion
	{
		get{return _denominacion == null ? string.Empty : _denominacion ;}
		set{_denominacion = value;}
	}

        [Auditoria()]
        public DateTime FechaCorte
        {
            get { return _fechaCorte; }
            set { _fechaCorte = value; }
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

    public int? MiProveedorIdProveedor
    {
        get { return this.Proveedor.IdProveedor; }
    }

    public string MiProveedorRazonSocial
    {
        get { return this.Proveedor.RazonSocial; }
    }

	#endregion
	}
}
