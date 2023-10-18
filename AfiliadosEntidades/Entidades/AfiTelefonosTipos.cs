
using Comunes.Entidades;
using System;
namespace Afiliados.Entidades
{
    [Serializable]
    public partial class AfiTelefonosTipos : Objeto
    {

        #region "Private Members"
        int _idTelefonoTipo;
        string _descripcion;
        #endregion

        #region "Constructors"
        public AfiTelefonosTipos()
        {
        }
        #endregion

        #region "Public Properties"
        public int IdTelefonoTipo
        {
            get { return _idTelefonoTipo; }
            set { _idTelefonoTipo = value; }
        }
        public string Descripcion
        {
            get { return _descripcion == null ? string.Empty : _descripcion; }
            set { _descripcion = value; }
        }

        #endregion
    }
}
