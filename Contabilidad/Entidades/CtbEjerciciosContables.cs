
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Contabilidad.Entidades
{
    [Serializable]
    public partial class CtbEjerciciosContables : Objeto
    {
        // Class CtbEjerciciosContables
        #region "Private Members"
        int _idEjercicioContable;
        string _descripcion;
        DateTime _fechaInicio;
        DateTime _fechaFin;
        DateTime? _fechaCierre;
        DateTime? _fechaCopiativo;
        
        #endregion

        #region "Constructors"
        public CtbEjerciciosContables()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdEjercicioContable
        {
            get { return _idEjercicioContable; }
            set { _idEjercicioContable = value; }
        }

        [Auditoria()]
        public string Descripcion
        {
            get { return _descripcion == null ? string.Empty : _descripcion; }
            set { _descripcion = value; }
        }

        [Auditoria()]
        public DateTime FechaInicio
        {
            get { return _fechaInicio; }
            set { _fechaInicio = value; }
        }

        [Auditoria()]
        public DateTime FechaFin
        {
            get { return _fechaFin; }
            set { _fechaFin = value; }
        }

        [Auditoria()]
        public DateTime? FechaCierre
        {
            get { return _fechaCierre; }
            set { _fechaCierre = value; }
        }

        [Auditoria()]
        public DateTime? FechaCopiativo
        {
            get { return _fechaCopiativo; }
            set { _fechaCopiativo = value; }
        }

        #endregion
    }
}

