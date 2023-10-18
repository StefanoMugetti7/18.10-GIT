
using Comunes.Entidades;
using System;
using System.Collections.Generic;
namespace Medicina.Entidades
{
    [Serializable]
    public partial class MedTurneras : Objeto
    {
        // Class MedTurneras
        #region "Private Members"
        int _idTurnera;
        MedPrestadores _prestador;
        //MedPrestaciones _prestacion;
        MedEspecializaciones _especializacion;
        int _idPrestacion;
        TGEFiliales _filial;
        DateTime _fecha;
        DateTime? _fechaDesde;
        DateTime? _fechaHasta;
        TimeSpan _desdeHora;
        TimeSpan _hastaHora;
        DateTime _fechaAlta;
        int _idUsuarioAlta;
        Guid _guidTurnera;

        List<MedTurnos> _medTurnos;
        #endregion

        #region "Constructors"
        public MedTurneras()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdTurnera
        {
            get { return _idTurnera; }
            set { _idTurnera = value; }
        }
        public MedPrestadores Prestador
        {
            get { return _prestador == null ? (_prestador = new MedPrestadores()) : _prestador; }
            set { _prestador = value; }
        }

        public int IdPrestacion
        {
            get { return _idPrestacion; }
            set { _idPrestacion = value; }
        }

        public MedEspecializaciones Especializacion
        {
            get { return _especializacion == null ? (_especializacion = new MedEspecializaciones()) : _especializacion; }
            set { _especializacion = value; }
        }

        //public MedPrestaciones Prestacion
        //{
        //    get { return _idPrestacion == null ? (_idPrestacion = new MedPrestaciones()) : _idPrestacion; }
        //    set { _idPrestacion = value; }
        //}

        public TGEFiliales Filial
        {
            get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
            set { _filial = value; }
        }

        public DateTime Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }


        public DateTime? FechaDesde
        {
            get { return _fechaDesde; }
            set { _fechaDesde = value; }
        }

        public DateTime? FechaHasta
        {
            get { return _fechaHasta; }
            set { _fechaHasta = value; }
        }


        public TimeSpan DesdeHora
        {
            get { return _desdeHora; }
            set { _desdeHora = value; }
        }

        public TimeSpan HastaHora
        {
            get { return _hastaHora; }
            set { _hastaHora = value; }
        }

        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }

        public int IdUsuarioAlta
        {
            get { return _idUsuarioAlta; }
            set { _idUsuarioAlta = value; }
        }

        public List<MedTurnos> Turnos
        {
            get { return _medTurnos == null ? (_medTurnos = new List<MedTurnos>()) : _medTurnos; }
            set { _medTurnos = value; }
        }

        public Guid GuidTurnera
        {
            get { return _guidTurnera; }
            set { _guidTurnera = value; }
        }

        #endregion
    }
}
