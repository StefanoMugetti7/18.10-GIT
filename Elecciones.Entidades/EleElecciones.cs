using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Elecciones.Entidades
{
    [Serializable]
    public class EleElecciones : Objeto
    {
        int _idEleccion;
        string _eleccion;
        int _anio;
        List<EleEleccionesEtapas> _etapas;
        XmlDocumentSerializationWrapper _loteEtapas;
        XmlDocumentSerializationWrapper _loteVotos;
        List<TGEArchivos> _archivos;
        List<EleEleccionesVotos> _votos;
        int _idVotante;
        [PrimaryKey]
        [Auditoria()]
        public int IdEleccion { get => _idEleccion; set => _idEleccion = value; }
        public string Eleccion { get => _eleccion; set => _eleccion = value; }
        [Auditoria()]
        public int Anio { get => _anio; set => _anio = value; }
        [Auditoria()]
        public List<EleEleccionesEtapas> Etapas
        {
            get { return _etapas == null ? (_etapas = new List<EleEleccionesEtapas>()) : _etapas; }
            set { _etapas = value; }
        }
        public XmlDocument LoteEtapas
        {
            get { return _loteEtapas; }
            set { _loteEtapas = value; }
        }
        public List<TGEArchivos> Archivos
        {
            get { return _archivos == null ? (_archivos = new List<TGEArchivos>()) : _archivos; }
            set { _archivos = value; }
        }
        public List<EleEleccionesVotos> Votos
        {
            get { return _votos == null ? (_votos= new List<EleEleccionesVotos>()) : _votos; }
            set { _votos = value; }
        }
        public XmlDocument LoteVotos
        {
            get { return _loteVotos; }
            set { _loteVotos = value; }
        }
        /// <summary>
        /// Usado exclusivamente para validacion del votante en combo ajax de la pantalla de votacion
        /// </summary>
        public int IdVotante { get => _idVotante; set => _idVotante = value; }
    }
}
