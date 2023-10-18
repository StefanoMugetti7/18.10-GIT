using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;

namespace Afiliados.Entidades.Entidades
{
    [Serializable]
    public class AfiAfiliadosDatosUIF : Objeto
    {
        private int _idAfiliadoDatosUIF;
        private int _idAfiliado;
        private int _idPersonaFisicaRelacionadaActTerrorista;
        private int _idPersonaFisicaRelacionadaTripleFrontera;
        private string _cargo;
        private string _pais;
        private string _dependencia;
        private int? _desempenioActual;
        private int _esPEP;
        private int _esCliente;
        private decimal _limitePerfilesAnual;
        private DateTime _limitePerfilesFechaVencimiento;
        private DateTime? _fechaInicio;
        private DateTime? _fechaFin;
        private string _cargoPersonaJuridicaVinculada;
        private int _idPersonaJuridicaVinculada;
        private string _personaJuridicaVinculada;
        private List<AfiAfiliadosMatrizRiesgo> _listaMatrizRiesgo;
        private List<TGEArchivos> _archivos;
        private List<TGEComentarios> _comentarios;
        private AfiAfiliados _esposa;

        [PrimaryKey()]
        public int IdAfiliadoDatosUIF
        {
            get { return _idAfiliadoDatosUIF; }
            set { _idAfiliadoDatosUIF = value; }
        }

        public int IdAfiliado
        {
            get { return _idAfiliado; }
            set { _idAfiliado = value; }
        }
        [Auditoria]
        public int IdPersonaFisicaRelacionadaActTerrorista { get => _idPersonaFisicaRelacionadaActTerrorista; set => _idPersonaFisicaRelacionadaActTerrorista = value; }
        [Auditoria]
        public int IdPersonaFisicaRelacionadaTripleFrontera { get => _idPersonaFisicaRelacionadaTripleFrontera; set => _idPersonaFisicaRelacionadaTripleFrontera = value; }
        [Auditoria]
        public string Cargo { get => _cargo; set => _cargo = value; }
        [Auditoria]
        public string Pais { get => _pais; set => _pais = value; }
        [Auditoria]
        public string Dependencia { get => _dependencia; set => _dependencia = value; }
        [Auditoria]
        public int? DesempenioActual { get => _desempenioActual; set => _desempenioActual = value; }
        [Auditoria]
        public int EsPEP { get => _esPEP; set => _esPEP = value; }
        public int EsCliente { get => _esCliente; set => _esCliente = value; }
        [Auditoria]
        public decimal LimitePerfilesAnual { get => _limitePerfilesAnual; set => _limitePerfilesAnual = value; }
        [Auditoria]
        public DateTime LimitePerfilesFechaVencimiento { get => _limitePerfilesFechaVencimiento; set => _limitePerfilesFechaVencimiento = value; }
        [Auditoria]
        public string CargoPersonaJuridicaVinculada { get => _cargoPersonaJuridicaVinculada; set => _cargoPersonaJuridicaVinculada = value; }
        [Auditoria]
        public int IdPersonaJuridicaVinculada { get => _idPersonaJuridicaVinculada; set => _idPersonaJuridicaVinculada = value; }
        [Auditoria]
        public string PersonaJuridicaVinculada { get => _personaJuridicaVinculada; set => _personaJuridicaVinculada = value; }
        public List<AfiAfiliadosMatrizRiesgo> MatricesDeRiesgo
        {
            get { return _listaMatrizRiesgo == null ? (_listaMatrizRiesgo = new List<AfiAfiliadosMatrizRiesgo>()) : _listaMatrizRiesgo; }
            set { _listaMatrizRiesgo = value; }
        }
        [Auditoria]
        public DateTime? FechaInicio { get => _fechaInicio; set => _fechaInicio = value; }
        [Auditoria]
        public DateTime? FechaFin { get => _fechaFin; set => _fechaFin = value; }
        public List<TGEArchivos> Archivos
        {
            get { return _archivos == null ? (_archivos = new List<TGEArchivos>()) : _archivos; }
            set { _archivos = value; }
        }

        public List<TGEComentarios> Comentarios
        {
            get { return _comentarios == null ? (_comentarios = new List<TGEComentarios>()) : _comentarios; }
            set { _comentarios = value; }
        }

        public AfiAfiliados Esposa
        {
            get { return _esposa == null ? (_esposa = new AfiAfiliados()) : _esposa; }
            set { _esposa = value; }
        }
    }
}
