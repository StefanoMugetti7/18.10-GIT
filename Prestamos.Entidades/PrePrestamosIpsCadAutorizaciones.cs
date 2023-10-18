using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Prestamos.Entidades
{
    [Serializable]
    public class PrePrestamosIpsCadAutorizaciones : Objeto
    {
        int _idPrestamoIpsCadAutorizacion;
        int _tipo;
        Int64 _numero;
        Int64 _dNI;
        string _codigo;
        decimal _importe;
        DateTime _fecha;
        int _cantidadCuotas;
        int _periodo;
        int _idAfiliado;
        int? _idPrestamo;

        [PrimaryKey()]
        public int IdPrestamoIpsCadAutorizacion
        {
            get { return _idPrestamoIpsCadAutorizacion; }
            set { _idPrestamoIpsCadAutorizacion = value; }
        }

        public int Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }

        public Int64 Numero
        {
            get { return _numero; }
            set { _numero = value; }
        }

        public Int64 DNI
        {
            get { return _dNI; }
            set { _dNI = value; }
        }

        public string Codigo
        {
            get { return _codigo == null ? string.Empty : _codigo; }
            set { _codigo = value; }
        }

        public decimal Importe
        {
            get { return _importe; }
            set { _importe = value; }
        }

        public DateTime Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }

        public int CantidadCuotas
        {
            get { return _cantidadCuotas; }
            set { _cantidadCuotas = value; }
        }

        public int Periodo
        {
            get { return _periodo; }
            set { _periodo = value; }
        }

        public int IdAfiliado
        {
            get { return _idAfiliado; }
            set { _idAfiliado = value; }
        }

        public int? IdPrestamo
        {
            get { return _idPrestamo; }
            set { _idPrestamo = value; }
        }
    }
}
