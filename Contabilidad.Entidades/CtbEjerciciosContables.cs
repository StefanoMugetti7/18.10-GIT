
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
        int? _idEjercicioContable;
        string _descripcion;
        DateTime _fechaInicio;
        DateTime _fechaFin;
        DateTime? _fechaCierre;
        DateTime? _fechaCopiativo;
        DateTime? _fechaInicioDesde;
        DateTime? _fechaInicioHasta;
        DateTime? _fechaFinDesde;
        DateTime? _fechaFinHasta;
        DateTime? _fechaCierreDesde;
        DateTime? _fechaCierreHasta;
        DateTime? _fechaCopiativoDesde;
        DateTime? _fechaCopiativoHasta;
        int _idEjercicioContableOrigen;
        
        #endregion

        #region "Constructors"
        public CtbEjerciciosContables()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int? IdEjercicioContable
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

        public DateTime? FechaInicioDesde
        {
            get { return _fechaInicioDesde; }
            set { _fechaInicioDesde = value; }
        }

        public DateTime? FechaInicioHasta
        {
            get { return _fechaInicioHasta; }
            set { _fechaInicioHasta = value; }
        }

        public DateTime? FechaFinDesde
        {
            get { return _fechaFinDesde; }
            set { _fechaFinDesde = value; }
        }

        public DateTime? FechaFinHasta
        {
            get { return _fechaFinHasta; }
            set { _fechaFinHasta = value; }
        }

        public DateTime? FechaCierreDesde
        {
            get { return _fechaCierreDesde; }
            set { _fechaCierreDesde = value; }
        }

        public DateTime? FechaCierreHasta
        {
            get { return _fechaCierreHasta; }
            set { _fechaCierreHasta = value; }
        }

        public DateTime? FechaCopiativoDesde
        {
            get { return _fechaCopiativoDesde; }
            set { _fechaCopiativoDesde = value; }
        }

        public DateTime? FechaCopiativoHasta
        {
            get { return _fechaCopiativoHasta; }
            set { _fechaCopiativoHasta = value; }
        }

        public int IdEjercicioContableOrigen
        {
            get { return _idEjercicioContableOrigen; }
            set { _idEjercicioContableOrigen = value; }
        }
        #endregion
    }
}

