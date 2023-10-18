
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Contabilidad.Entidades
{
    [Serializable]
    public partial class CtbPeriodosContables : Objeto
    {
        // Class CtbPeriodosContables
        #region "Private Members"
        int _idPeriodoContable;
        int _periodo;
        CtbEjerciciosContables _ejercicioContable;
        DateTime _fechaCierre;
        DateTime? _fechaCierreDesde;
        DateTime? _fechaCierreHasta;
        #endregion

        #region "Constructors"
        public CtbPeriodosContables()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdPeriodoContable
        {
            get { return _idPeriodoContable; }
            set { _idPeriodoContable = value; }
        }

        [Auditoria()]
        public int Periodo
        {
            get { return _periodo; }
            set { _periodo = value; }
        }

        public CtbEjerciciosContables EjercicioContable
        {
            get { return _ejercicioContable == null ? (_ejercicioContable = new CtbEjerciciosContables()) : _ejercicioContable; }
            set { _ejercicioContable = value; }
        }

        [Auditoria()]
        public DateTime FechaCierre
        {
            get { return _fechaCierre; }
            set { _fechaCierre = value; }
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

        #endregion
    }
}
