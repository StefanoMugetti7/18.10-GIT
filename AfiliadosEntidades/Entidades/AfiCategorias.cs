
using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Xml;
namespace Afiliados.Entidades
{
    [Serializable]
    public partial class AfiCategorias : Objeto
    {
        #region "Private Members"
        int? _idCategoria;
        string _categoria;
        decimal _importeCuota;
        decimal _importeGasto;
        List<TGEArchivos> _archivos;
        List<TGEComentarios> _comentarios;
        int? _idTipoCategoria;
        #endregion

        #region "Constructors"
        public AfiCategorias()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        [Auditoria()]
        public int? IdCategoria
        {
            get { return _idCategoria; }
            set { _idCategoria = value; }
        }
        [Auditoria()]
        public string Categoria
        {
            get { return _categoria == null ? string.Empty : _categoria; }
            set { _categoria = value; }
        }
        [Auditoria()]
        public string Codigo { get; set; }

        [Auditoria()]
        public string NumeroSocioDesde { get; set; }

        public decimal ImporteCuota
        {
            get { return _importeCuota; }
            set { _importeCuota = value; }
        }

        public decimal ImporteGasto
        {
            get { return _importeGasto; }
            set { _importeGasto = value; }
        }

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

        public int? IdTipoCategoria
        {
            get { return _idTipoCategoria; }
            set { _idTipoCategoria = value; }
        }
        public string TipoCategoria { get; set; }

        #endregion

        List<TGECampos> _campos;
        XmlDocumentSerializationWrapper _loteCamposValores;
        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
        }

        public XmlDocument LoteCamposValores
        {
            get { return _loteCamposValores; }// == null ? (_loteCamposValores = new XmlDocumentSerializationWrapper()) : _loteCamposValores; }
            set { _loteCamposValores = value; }
        }
    }
}
