
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Medicina.Entidades
{
    [Serializable]
    public partial class MedHistoriasClinicasEvoluciones : Objeto
    {
        // Class MedHistoriasClinicasEvoluciones
        #region "Private Members"
        int _idHistoriaClinicaEvolucion;
        int _idHistoriaClinica;
        string _observaciones;
        int _idUsuarioAlta;
        DateTime _fechaAlta;
        int _idPrestacion;
        #endregion

        #region "Constructors"
        public MedHistoriasClinicasEvoluciones()
        {
        }
        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdHistoriaClinicaEvolucion
        {
            get { return _idHistoriaClinicaEvolucion; }
            set { _idHistoriaClinicaEvolucion = value; }
        }
        public int IdHistoriaClinica
        {
            get { return _idHistoriaClinica; }
            set { _idHistoriaClinica = value; }
        }
        [Auditoria()]
        public string Observaciones
        {
            get { return _observaciones == null ? string.Empty : _observaciones; }
            set { _observaciones = value; }
        }

        public int IdUsuarioAlta
        {
            get { return _idUsuarioAlta; }
            set { _idUsuarioAlta = value; }
        }

        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }

        public int IdPrestacion
        {
            get { return _idPrestacion; }
            set { _idPrestacion = value; }
        }

        #endregion
    }
}
