using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Prestamos.Entidades
{
    [Serializable]
    public class PrePrestamosIpsPlanes : Objeto
    {
        int _idPrestamoIpsPlan;
        int _idPlan;
        decimal _importeTotal;
        int _cantidadCuotas;
        decimal _importeCuota;
        DateTime _fechaAlta;
        int _idUsuarioAlta;
        int _idPrestamoIpsCadAutorizacion;
        DateTime _fechaDesde;
        [PrimaryKey()]
        public int IdPrestamoIpsPlan
        {
            get { return _idPrestamoIpsPlan; }
            set { _idPrestamoIpsPlan = value; }
        }

        public int IdPlan
        {
            get { return _idPlan; }
            set { _idPlan = value; }
        }
        [Auditoria()]
        public decimal ImporteTotal
        {
            get { return _importeTotal; }
            set { _importeTotal = value; }
        }

        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }

        public DateTime FechaDesde
        {
            get { return _fechaDesde; }
            set { _fechaDesde = value; }
        }

        public int CantidadCuotas
        {
            get { return _cantidadCuotas; }
            set { _cantidadCuotas = value; }
        }

        public decimal ImporteCuota
        {
            get { return _importeCuota; }
            set { _importeCuota = value; }
        }

        public int IdUsuarioAlta
        {
            get { return _idUsuarioAlta; }
            set { _idUsuarioAlta = value; }
        }

        public int IdPrestamoIpsCadAutorizacion
        {
            get { return _idPrestamoIpsCadAutorizacion; }
            set { _idPrestamoIpsCadAutorizacion = value; }
        }
    }
}
