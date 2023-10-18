using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberes.Entidades
{
    [Serializable]
    public class HabSueldosAportesRangos : Objeto
    {
        #region "Private Members"
        int _idSueldoAporteRango;
        DateTime? _fechaIngresoDesde;
        DateTime? _fechaIngresoHasta;
        int _aniosMinimos;
        int _aniosMaximos;
        decimal _porcentajeMinimo;
        decimal _porcentajeMaximo;

        #endregion

        #region "Constructors"
        
        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdSueldoAporteRango
        {
            get { return _idSueldoAporteRango; }
            set { _idSueldoAporteRango = value; }
        }
        public int AniosMinimos
        {
            get { return _aniosMinimos; }
            set { _aniosMinimos = value; }
        }
        public int AniosMaximos
        {
            get { return _aniosMaximos; }
            set { _aniosMaximos = value; }
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

        public decimal PorcentajeAporteMinimo
        {
            get { return _porcentajeMinimo; }
            set { _porcentajeMinimo = value; }
        }
        public decimal PorcentajeAporteMaximo
        {
            get { return _porcentajeMaximo; }
            set { _porcentajeMaximo = value; }
        }
        #endregion
    }
}
