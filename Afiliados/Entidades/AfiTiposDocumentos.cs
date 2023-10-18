
using System;
using System.Collections.Generic;
using Comunes.Entidades;
namespace Afiliados.Entidades
{
    [Serializable]
    public partial class AfiTiposDocumentos : Objeto
    {
        #region "Private Members"
        int _idTipoDocumento;
        string _tipoDocumento;
        int _afipCodigo;

        #endregion

        #region "Constructors"
        public AfiTiposDocumentos()
        {
        }
        #endregion

        #region "Public Properties"
        [PrimaryKey]
        public int IdTipoDocumento
        {
            get { return _idTipoDocumento; }
            set { _idTipoDocumento = value; }
        }
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
        #endregion
    }
}
