
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Afiliados.Entidades
{
  [Serializable]
	public partial class AfiTelefonos : Objeto
	{
		
	#region "Private Members"
	int _idTelefono;
	int _idAfiliado;
	AfiTelefonosTipos _telefonoTipo;
	int _prefijo;
	int _numero;
	int _interno;
    DateTime _fechaAlta;
	#endregion
		
	#region "Constructors"
	public AfiTelefonos()
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
	public int IdAfiliado
	{
		get{return _idAfiliado;}
		set{_idAfiliado = value;}
	}

	public AfiTelefonosTipos TelefonoTipo
	{
		get{return _telefonoTipo == null ? (_telefonoTipo = new AfiTelefonosTipos()):_telefonoTipo;}
		set{_telefonoTipo = value;}
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
        get { return string.Concat(this.TelefonoTipo.Descripcion, " ", this.Prefijo, " ", this.Numero, " ", this.Interno); }
        set { }
    }

	#endregion
	}
}
