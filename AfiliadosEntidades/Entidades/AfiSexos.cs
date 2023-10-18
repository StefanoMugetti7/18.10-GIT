
using Comunes.Entidades;
using System;
namespace Afiliados.Entidades
{
    [Serializable]
    public partial class AfiSexos : Objeto
    {
        #region "Private Members"
        int? _idSexo;
        string _sexo;

        #endregion

        #region "Constructors"
        public AfiSexos()
        {
        }
        #endregion

        #region "Public Properties"
        public int? IdSexo
        {
            get { return _idSexo; }
            set { _idSexo = value; }
        }
        public string Sexo
        {
            get { return _sexo == null ? string.Empty : _sexo; }
            set { _sexo = value; }
        }

        #endregion
    }
}
