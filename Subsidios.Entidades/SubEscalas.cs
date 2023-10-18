using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Subsidios.Entidades
{
    [Serializable]
    public class SubEscalas : Objeto
    {

        #region "Private Members"

        int _idEscala;
        int _idSubsidio;
        DateTime? _fechaIngresoDesde;
        DateTime? _fechaIngresoHasta;
        int? _edadDesde;
        int? _edadHasta;
        int? _antiguedadDesde;
        int? _antiguedadHasta;
        decimal _importeCuotaMensual;
        decimal _importeBeneficio;
        DateTime? _fechaInicioVigencia;
        DateTime? _fechaFinVigencia;
        //TGEFiliales _filial;
        #endregion

        #region "Constructors"
        public SubEscalas()
        {
        }
        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdEscala
        {
            get { return _idEscala; }
            set { _idEscala = value; }
        }
        //public string Descripcion
        //{
        //    get { return _descripcion == null ? string.Empty : _descripcion; }
        //    set { _descripcion = value; }
        //}
        public int IdSubsidio
        {
            get { return _idSubsidio; }
            set { _idSubsidio = value; }
        }

        public DateTime? FechaIngresoDesde
        {
            get { return _fechaIngresoDesde; }
            set { _fechaIngresoDesde = value; }
        }

        public DateTime? FechaIngresoHasta
        {
            get { return _fechaIngresoHasta; }
            set { _fechaIngresoHasta = value; }
        }


        public int? EdadDesde
        {
            get { return _edadDesde; }
            set { _edadDesde = value; }
        }
        public int? EdadHasta
        {
            get { return _edadHasta; }
            set { _edadHasta = value; }
        }
        public int? AntiguedadDesde
        {
            get { return _antiguedadDesde; }
            set { _antiguedadDesde = value; }

        }
        public int? AntiguedadHasta
        {
            get { return _antiguedadHasta; }
            set { _antiguedadHasta = value; }

        } 

        public decimal ImporteCuotaMensual
        {
            get { return _importeCuotaMensual; }
            set { _importeCuotaMensual = value; }
        }

        public decimal ImporteBeneficio
        {
            get { return _importeBeneficio; }
            set { _importeBeneficio = value; }
        }

        public DateTime? FechaInicioVigencia
        {
            get { return _fechaInicioVigencia; }
            set { _fechaInicioVigencia = value; }
        }

        public DateTime? FechaFinVigencia
        {
            get { return _fechaFinVigencia; }
            set { _fechaFinVigencia = value; }
        }

        //public TGEFiliales Filial
        //{
        //    get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
        //    set { _filial = value; }
        //}

        #endregion
    }
}
