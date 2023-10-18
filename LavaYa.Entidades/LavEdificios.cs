using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LavaYa.Entidades
{
    [Serializable]
    public class LavEdificios : Objeto
    {
        [PrimaryKey]
        public int IdEdificio { get; set; }
        private string descripcion;
        private string contacto;
        private string direccion;
        private int? numeroDireccion;
        private string codigoPostal;
        private string localidad;
        private string partido;
        private string provincia;
        private List<TGEArchivos> _archivos;
        private List<LavMaquinas> _maquinas;
        public int IdHorario { get; set; }
        public string Horario { get; set; }

        public int CantidadMaquinasSecado { get; set; }
        public int CantidadMaquinasLavado { get; set; }
        public int FrecuenciaRecaudacion { get; set; }
        public int FrecuenciaAspiracion { get; set; }
    
        public DateTime FechaPEM { get; set; }
        public int IdContrato { get; set; }
        public string Localizacion { get; set; }

        public string Contrato { get; set; }
        public string CodigoQR { get; set; }
        public byte[] CodigoQRImagen { get; set; }
        public decimal Latitud { get; set; }
        public decimal Longitud { get; set; }
        public int? UnidadesFuncionales { get; set; }
        public string Servicios { get; set; }
        public int? IdSistemaPago { get; set; }
        public string SistemaPago { get; set; }
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
        public List<TGEArchivos> Archivos
        {
            get { return _archivos == null ? (_archivos = new List<TGEArchivos>()) : _archivos; }
            set { _archivos = value; }
        }

        public List<LavMaquinas> Maquinas
        {
            get { return _maquinas == null ? (_maquinas = new List<LavMaquinas>()) : _maquinas; }
            set { _maquinas = value; }
        }
    }
}
