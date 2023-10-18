
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;

namespace Proveedores.Entidades
{
      [Serializable]
	public partial class CapProveedoresTelefonos : Objeto
	{
		
	#region "Private Members"
	int _idTelefono;
    int _idProveedor;
	int _prefijo;
	int _numero;
	int _interno;
    TGETiposTelefonos _tipoTelefono;
    DateTime _fechaAlta;
	#endregion
		
	#region "Constructors"
	public CapProveedoresTelefonos()
	{
	}
	#endregion
		
	#region "Public Properties"
    [PrimaryKey()]
	public int IdTelefono
	{
		get{return _idTelefono ;}
		set{_idTelefono = value;}
	}
	public int IdProveedor
	{
		get{return _idProveedor;}
		set{_idProveedor = value;}
	}

    public TGETiposTelefonos TipoTelefono
    {
        get { return _tipoTelefono == null ? (_tipoTelefono = new TGETiposTelefonos()) : _tipoTelefono; }
        set { _tipoTelefono = value; }
    }

	public int Prefijo
	{
		get{return _prefijo;}
		set{_prefijo = value;}
	}

	public int Numero
	{
		get{return _numero;}
		set{_numero = value;}
	}

	public int Interno
	{
		get{return _interno;}
		set{_interno = value;}
	}

    public DateTime FechaAlta
    {
        get { return _fechaAlta; }
        set { _fechaAlta = value; }
    }

    public string TelefonoCompleto
    {
        get { return string.Concat(this.TipoTelefono.Descripcion, " ", this.Prefijo, " ", this.Numero, " ", this.Interno); }
        set { }
    }

	#endregion
	}
}
