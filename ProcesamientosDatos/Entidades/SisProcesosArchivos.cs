
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace ProcesosDatos.Entidades
{
  [Serializable]
	public partial class SisProcesosArchivos : Objeto
	{

	#region "Private Members"
	int _idProcesoArchivo;
	string _rowDelimitator;
	string _fieldDelimitator;
	int _cabLines;
	int _trailLines;
	string _nombreArchivo;
	byte _type;
	string _path;
	string _sheetName;
	string _storedProcedure;
    bool _procesaArchivo;
    List<SisProcesosArchivosCampos> _procesosArchivosCampos;
	#endregion
		
	#region "Constructors"
	public SisProcesosArchivos()
	{
	}
	#endregion
		
	#region "Public Properties"
    [PrimaryKey()]
	public int IdProcesoArchivo
	{
		get{return _idProcesoArchivo ;}
		set{_idProcesoArchivo = value;}
	}
	public string RowDelimitator
	{
		get{return _rowDelimitator == null ? string.Empty : _rowDelimitator ;}
		set{_rowDelimitator = value;}
	}

	public string FieldDelimitator
	{
		get{return _fieldDelimitator == null ? string.Empty : _fieldDelimitator ;}
		set{_fieldDelimitator = value;}
	}

	public int CabLines
	{
		get{return _cabLines;}
		set{_cabLines = value;}
	}

	public int TrailLines
	{
		get{return _trailLines;}
		set{_trailLines = value;}
	}

	public string NombreArchivo
	{
		get{return _nombreArchivo == null ? string.Empty : _nombreArchivo ;}
		set{_nombreArchivo = value;}
	}

	public byte Type
	{
		get{return _type;}
		set{_type = value;}
	}

	public string Path
	{
		get{return _path == null ? string.Empty : _path ;}
		set{_path = value;}
	}

	public string SheetName
	{
		get{return _sheetName == null ? string.Empty : _sheetName ;}
		set{_sheetName = value;}
	}

	public string StoredProcedure
	{
		get{return _storedProcedure == null ? string.Empty : _storedProcedure ;}
		set{_storedProcedure = value;}
	}

    public bool ProcesaArchivo
    {
        get { return _procesaArchivo;}
        set { _procesaArchivo = value; }
    }

    public List<SisProcesosArchivosCampos> ProcesosArchivosCampos
    {
        get { return _procesosArchivosCampos == null ? (_procesosArchivosCampos = new List<SisProcesosArchivosCampos>()) : _procesosArchivosCampos; }
        set { _procesosArchivosCampos = value; }
    }

	#endregion
	}
}
