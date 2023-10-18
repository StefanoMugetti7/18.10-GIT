
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace ProcesosDatos.Entidades
{
  [Serializable]
	public partial class SisProcesosArchivosCampos : Objeto
	{

	#region "Private Members"
	int _idProcesoArchivoCampo;
	string _tableField;
	string _defaultValue;
	string _fileField;
	int _fromChar;
	int _toChar;
	int _fieldsOrder;
	string _type;
    SisProcesosArchivos _procesoArchivo;
	#endregion
		
	#region "Constructors"
	public SisProcesosArchivosCampos()
	{
	}
	#endregion
		
	#region "Public Properties"
    [PrimaryKey()]
	public int IdProcesoArchivoCampo
	{
		get{return _idProcesoArchivoCampo ;}
		set{_idProcesoArchivoCampo = value;}
	}

	public string TableField
	{
		get{return _tableField == null ? string.Empty : _tableField ;}
		set{_tableField = value;}
	}

	public string DefaultValue
	{
		get{return _defaultValue == null ? string.Empty : _defaultValue ;}
		set{_defaultValue = value;}
	}

	public string FileField
	{
		get{return _fileField == null ? string.Empty : _fileField ;}
		set{_fileField = value;}
	}

	public int FromChar
	{
		get{return _fromChar;}
		set{_fromChar = value;}
	}

	public int ToChar
	{
		get{return _toChar;}
		set{_toChar = value;}
	}

	public int FieldsOrder
	{
		get{return _fieldsOrder;}
		set{_fieldsOrder = value;}
	}

	public string Type
	{
		get{return _type == null ? string.Empty : _type ;}
		set{_type = value;}
	}

    public SisProcesosArchivos ProcesoArchivo
    {
        get { return _procesoArchivo == null ? (_procesoArchivo = new SisProcesosArchivos()) : _procesoArchivo; }
        set { _procesoArchivo = value; }
    }

	#endregion
	}
}
