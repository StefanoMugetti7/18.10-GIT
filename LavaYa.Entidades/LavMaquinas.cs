using Comunes.Entidades;
using System;

namespace LavaYa.Entidades
{
    [Serializable]
    public class LavMaquinas : Objeto
    {
        [PrimaryKey]
        private int idMaquina { get; set; }

        public int IdMaquina
        {
            get { return idMaquina; }
            set { idMaquina = value; }
        }

        public string CodigoQR { get; set; }
        public byte[] CodigoQRImagen { get; set; }
        public string ManualPDF { get; set; }
        public string TipoMaquina { get; set; }
        public string TipoMaquinaCodigo { get; set; }
        public int IdTipoMaquina { get; set; }

        private string modelo;

        public string Modelo
        {
            get { return modelo; }
            set { modelo = value; }
        }
        private int idModelo;
        public int IdModelo
        {
            get { return idModelo; }
            set { idModelo = value; }
        }

        private string numeroSerie;

        public string NumeroSerie
        {
            get { return numeroSerie; }
            set { numeroSerie = value; }
        }
        private int idMarca;

        public int IdMarca
        {
            get { return idMarca; }
            set { idMarca = value; }
        }
        private string marca;

        public string Marca
        {
            get { return marca; }
            set { marca = value; }
        }

        private DateTime fechaALta;
        private byte[] _pdf;

        public DateTime FechaAlta
        {
            get { return fechaALta; }
            set { fechaALta = value; }
        }
        public int IdEdificio { get; set; }
        public int? IdMaquinaEdificio { get; set; }

        public string InformacionCombo { get; set; }
        public int Numero { get; set; }
        public byte[] Pdf
        {
            get
            {
                return _pdf;
            }
            set
            {
                _pdf = value;
            }
        }
    }
}
