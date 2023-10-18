
using Comunes.Entidades;
using System;
namespace Afiliados.Entidades
{
    [Serializable]
    public partial class AfiParentesco : Objeto
    {

        #region "Private Members"
        int? _idParentesco;
        string _parentesco;
        DateTime _fechaAlta;
        UsuariosAlta _usuarioAlta;
        #endregion

        #region "Constructors"
        public AfiParentesco()
        {
        }
        #endregion

        #region "Public Properties"
        public int? IdParentesco
        {
            get { return _idParentesco; }
            set { _idParentesco = value; }
        }
        public string Parentesco
        {
            get { return _parentesco == null ? string.Empty : _parentesco; }
            set { _parentesco = value; }
        }

        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }

        public UsuariosAlta UsuarioAlta
        {
            get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
            set { _usuarioAlta = value; }
        }

        #endregion
    }
}
