using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Bancos.Entidades
{
    [Serializable]
    public partial class TESTarjetasTransacciones : Objeto
    {
        #region "Private Members"
        int _idTarjetaTransaccion;
        int _idCajaMovimientoValor;
        DateTime _fecha;
        int _idTarjetaCredito;
        string _numeroTarjetaCredito;
        string _titular;
        int _vencimientoMes;
        int _vencimientoAnio;
        string _numeroTransaccionPosnet;
        string _numeroLote;
        DateTime _fechaTransaccion;
        decimal _importe;
        int _cantidadCuotas;
        string _observaciones;
        string _tarjetaDescripcion;
        string _detalle;
        #endregion

        #region "Constructors"
        public TESTarjetasTransacciones()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdTarjetaTransaccion
        {
            get { return _idTarjetaTransaccion; }
            set { _idTarjetaTransaccion = value; }
        }

        public int IdCajaMovimientoValor
        {
            get { return _idCajaMovimientoValor; }
            set { _idCajaMovimientoValor = value; }
        }

        public DateTime Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }

        public int IdTarjetaCredito
        {
            get { return _idTarjetaCredito; }
            set { _idTarjetaCredito = value; }
        }

        public string NumeroTarjetaCredito
        {
            get { return _numeroTarjetaCredito; }
            set { _numeroTarjetaCredito = value; }
        }

        public string Titular
        {
            get { return _titular; }
            set { _titular = value; }
        }

        public int VencimientoMes
        {
            get { return _vencimientoMes; }
            set { _vencimientoMes = value; }
        }

        public int VencimientoAnio
        {
            get { return _vencimientoAnio; }
            set { _vencimientoAnio = value; }
        }

        public string NumeroTransaccionPosnet
        {
            get { return _numeroTransaccionPosnet; }
            set { _numeroTransaccionPosnet = value; }
        }

        public string NumeroLote
        {
            get { return _numeroLote; }
            set { _numeroLote = value; }
        }

        public DateTime FechaTransaccion
        {
            get { return _fechaTransaccion; }
            set { _fechaTransaccion = value; }
        }

        public decimal Importe
        {
            get { return _importe; }
            set { _importe = value; }
        }

        public int CantidadCuotas
        {
            get { return _cantidadCuotas; }
            set { _cantidadCuotas = value; }
        }

        public string Observaciones
        {
            get { return _observaciones; }
            set { _observaciones = value; }
        }

        public string TarjetaDescripcion
        {
            get { return _tarjetaDescripcion; }
            set { _tarjetaDescripcion = value; }
        }

        public string Detalle
        {
            get { return _detalle; }
            set { _detalle = value; }
        }
        #endregion
    }
}
