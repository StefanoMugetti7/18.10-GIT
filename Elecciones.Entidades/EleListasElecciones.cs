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
    public class EleListasElecciones : Objeto
    {
        int _idListaEleccion;
        EleElecciones _eleccion;
        int _idTipoRegion;
        int _idTipoLista;
        string _lista;
        decimal? _porcentajeAval;
        int? _idListaRef;
        string _listaRef;
        List<EleListasEleccionesPostulantes> _postulantes;
        List<EleListasEleccionesApoderados> _apoderados;
        List<EleListasEleccionesAvales> _avales;
        XmlDocumentSerializationWrapper _lotePostulantes;
        XmlDocumentSerializationWrapper _loteAvales;
        XmlDocumentSerializationWrapper _loteApoderados;
        List<TGEComentarios> _comentarios;
        XmlDocumentSerializationWrapper _loteComentarios;
        List<TGEArchivos> _archivos;

        int _idAfiliado;

        [PrimaryKey]
        [Auditoria()]
        public int IdListaEleccion { get => _idListaEleccion; set => _idListaEleccion = value; }
        public EleElecciones Eleccion { get => _eleccion == null ? (_eleccion = new EleElecciones()) : _eleccion; set => _eleccion = value; }
        [Auditoria()]
        public int IdTipoRegion { get => _idTipoRegion; set => _idTipoRegion = value; }
        [Auditoria()]
        public int IdTipoLista { get => _idTipoLista; set => _idTipoLista = value; }
        public string Lista { get => _lista; set => _lista = value; }
        [Auditoria()]
        public decimal? PorcentajeAval { get => _porcentajeAval; set => _porcentajeAval = value; }
        public List<EleListasEleccionesPostulantes> Postulantes
        {
            get { return _postulantes == null ? (_postulantes = new List<EleListasEleccionesPostulantes>()) : _postulantes; }
            set { _postulantes = value; }
        }

        public List<EleListasEleccionesApoderados> Apoderados
        {
            get { return _apoderados == null ? (_apoderados = new List<EleListasEleccionesApoderados>()) : _apoderados; }
            set { _apoderados = value; }
        }

        public List<EleListasEleccionesAvales> Avales
        {
            get { return _avales == null ? (_avales = new List<EleListasEleccionesAvales>()) : _avales; }
            set { _avales = value; }
        }

        public XmlDocument LotePostulantes
        {
            get { return _lotePostulantes; }
            set { _lotePostulantes = value; }
        }

        public XmlDocument LoteApoderados
        {
            get { return _loteApoderados; }
            set { _loteApoderados = value; }
        }

        public XmlDocument LoteAvales
        {
            get { return _loteAvales; }
            set { _loteAvales = value; }
        }
        public string TipoLista{ get; set; }
        public string TipoRegion { get; set; }
        public List<TGEComentarios> Comentarios
        {
            get { return _comentarios == null ? (_comentarios = new List<TGEComentarios>()) : _comentarios; }
            set { _comentarios = value; }
        }
        public XmlDocument LoteComentarios
        {
            get { return _loteComentarios; }
            set { _loteComentarios = value; }
        }
        public List<TGEArchivos> Archivos
        {
            get { return _archivos == null ? (_archivos = new List<TGEArchivos>()) : _archivos; }
            set { _archivos = value; }
        }
        /// <summary>
        /// NECESARIO PARA LA FUNCIONALIDAD DE LA VOTACION
        /// </summary>
        public int IdAfiliado { get => _idAfiliado; set => _idAfiliado = value; }
        public int? IdListaRef { get => _idListaRef; set => _idListaRef = value; }
        public string ListaRef { get => _listaRef; set => _listaRef = value; }
    }
}
