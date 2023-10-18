
using Comunes.Entidades;
using System;
namespace Afiliados.Entidades
{
    [Serializable]
    public partial class AfiEstadoCivil : Objeto
    {
        #region "Private Members"
        int? _idEstadoCivil;
        string _estadoCivil;

        #endregion

        #region "Constructors"
        public AfiEstadoCivil()
        {
        }
        #endregion

        #region "Public Properties"
        public int? IdEstadoCivil
        {
            get { return _idEstadoCivil; }
            set { _idEstadoCivil = value; }
        }
        public string EstadoCivil
        {
            get { return _estadoCivil == null ? string.Empty : _estadoCivil; }
            set { _estadoCivil = value; }
        }

        #endregion
    }
}
