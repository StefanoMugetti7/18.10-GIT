using Afiliados.Entidades;
using Compras.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Entidades
{
    [Serializable]
    public class TurPaquetes : Objeto
    {
        int _idPaquete;
        string _nombre;
        int _idTipoCargo;
        int _cantidad;
        List<TurPaquetesDetalles> _detalles;
        decimal _importe;
        TGEMonedas _moneda;
        List<TGECampos> _campos;

        [PrimaryKey]
        public int IdPaquete { get => _idPaquete; set => _idPaquete = value; }
        public string Nombre { get => _nombre; set => _nombre = value; }
        /// <summary>
        /// Cantidad total de plazas
        /// </summary>
        public int Cantidad { get => _cantidad; set => _cantidad = value; }
        /// <summary>
        /// Cantidad total de plazas disp
        /// </summary>
        public int CantidadDisponible { get; set; }
        public decimal Importe { get => _importe; set => _importe = value; }
        public decimal Costo { get => _importe; set => _importe = value; }
        public int IdTipoCargo { get => _idTipoCargo; set => _idTipoCargo = value; }
        public DateTime? FechaSalida { get; set; }
        public DateTime? FechaRegreso { get; set; }
        public List<TurPaquetesDetalles> Detalles
        {
            get { return _detalles == null ? (_detalles = new List<TurPaquetesDetalles>()) : _detalles; }
            set { _detalles = value; }
        }
        public TGEMonedas Moneda
        {
            get { return _moneda == null ? (_moneda = new TGEMonedas()) : _moneda; }
            set { _moneda = value; }
        }
        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
        }
    }
    public enum EstadosPaquetesTurismo
    {
        Baja = 0,
        Autorizado = 28,
        Pendiente = 38,
        Presentado = 112,
        Rechazado = 15,
        Activo = 1,
    }
}
