using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LavaYa.Entidades
{
    [Serializable]
    public class LavPuntosVentas : Objeto
    {
        [PrimaryKey]
        private int idPuntoVenta { get; set; }
        private string descripcion;
        private string contacto;
        private string direccion;
        private int? numeroDireccion;
        private string codigoPostal;
        private string localidad;
        private string partido;
        private string provincia;
        public decimal Latitud { get; set; }
        public string Localizacion{ get; set; }
        public decimal Longitud { get; set; }
        public int IdPuntoVenta
        {
            get { return idPuntoVenta; }
            set { idPuntoVenta = value; }
        }
        public string Provincia
        {
            get { return provincia; }
            set { provincia = value; }
        }

        public string Partido
        {
            get { return partido; }
            set { partido = value; }
        }

        public string Localidad
        {
            get { return localidad; }
            set { localidad = value; }
        }

        public string CodigoPostal
        {
            get { return codigoPostal; }
            set { codigoPostal = value; }
        }

        public int? NumeroDireccion
        {
            get { return numeroDireccion; }
            set { numeroDireccion = value; }
        }

        public string Direccion
        {
            get { return direccion; }
            set { direccion = value; }
        }

        public string Contacto
        {
            get { return contacto; }
            set { contacto = value; }
        }

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        List<LavPuntosVentasDetalle> _puntosVentasDetalles;

        public List<LavPuntosVentasDetalle> PuntosVentasDetalles
        {
            get { return _puntosVentasDetalles == null ? (_puntosVentasDetalles = new List<LavPuntosVentasDetalle>()) : _puntosVentasDetalles; }
            set { _puntosVentasDetalles = value; }
        }

    }
}
