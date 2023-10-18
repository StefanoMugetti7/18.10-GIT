
using Comunes.Entidades;
using System;
namespace Afiliados.Entidades
{
    [Serializable]
    public partial class AfiTiposDocumentos : Objeto
    {
        #region "Private Members"
        int? _idTipoDocumento;
        string _tipoDocumento;
        int _afipCodigo;
        int _codigo;

        #endregion

        #region "Constructors"
        public AfiTiposDocumentos()
        {
        }
        #endregion

        #region "Public Properties"
        [PrimaryKey]
        [Auditoria]
        public int? IdTipoDocumento
        {
            get { return _idTipoDocumento; }
            set { _idTipoDocumento = value; }
        }
        [Auditoria]
        public string TipoDocumento
        {
            get { return _tipoDocumento == null ? string.Empty : _tipoDocumento; }
            set { _tipoDocumento = value; }
        }

        public int AfipCodigo
        {
            get { return _afipCodigo; }
            set { _afipCodigo = value; }
        }

        public int Codigo
        {
            get { return _codigo; }
            set { _codigo = value; }
        }
        #endregion
    }
}
