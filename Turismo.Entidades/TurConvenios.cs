using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Turismo.Entidades
{
    [Serializable]
    public class TurConvenios : Objeto
    {
        int _idConvenio;
        int _idHotel;
        string _hotel;
        DateTime _fechaInicioTemporadaAlta;
        DateTime _fechaFinalTemporadaAlta;
        int _cantidadPlazas;
        int _cantidadPlazasDia;
        List<TGECampos> _campos;
        List<TurConveniosDetalles> _detalles;
        List<TurConveniosExcepciones> _excepciones;
        XmlDocumentSerializationWrapper _loteDetalles;
        XmlDocumentSerializationWrapper _loteExcepciones;

        [PrimaryKey]
        public int IdConvenio { get => _idConvenio; set => _idConvenio = value; }
        public int IdHotel { get => _idHotel; set => _idHotel = value; }
        public DateTime? FechaInicioConvenio { get; set; }
        public DateTime? FechaFinalConvenio { get; set; }
        public DateTime FechaInicioTemporadaAlta { get => _fechaInicioTemporadaAlta; set => _fechaInicioTemporadaAlta = value; }
        public DateTime FechaFinalTemporadaAlta { get => _fechaFinalTemporadaAlta; set => _fechaFinalTemporadaAlta = value; }
        public int CantidadPlazas { get => _cantidadPlazas; set => _cantidadPlazas = value; }
        public int CantidadPlazasDia { get => _cantidadPlazasDia; set => _cantidadPlazasDia = value; }
        public string Hotel { get => _hotel; set => _hotel = value; }
        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
        }
        public List<TurConveniosDetalles> Detalles
        {
            get { return _detalles == null ? (_detalles = new List<TurConveniosDetalles>()) : _detalles; }
            set { _detalles = value; }
        }
        public List<TurConveniosExcepciones> Excepciones
        {
            get { return _excepciones == null ? (_excepciones = new List<TurConveniosExcepciones>()) : _excepciones; }
            set { _excepciones = value; }
        }
        public XmlDocument LoteDetalles
        {
            get { return _loteDetalles; }
            set { _loteDetalles = value; }
        }
        public XmlDocument LoteExcepciones
        {
            get { return _loteExcepciones; }
            set { _loteExcepciones = value; }
        }
    }
}
