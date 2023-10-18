
using Comunes.Entidades;
using System;
namespace Afiliados.Entidades
{
    [Serializable]
    public partial class AfiGruposSanguineos : Objeto
    {
        #region "Private Members"
        int _idGrupoSanguieno;
        string _grupoSanguineo;

        #endregion

        #region "Constructors"
        public AfiGruposSanguineos()
        {
        }
        #endregion

        #region "Public Properties"
        public int IdGrupoSanguieno
        {
            get { return _idGrupoSanguieno; }
            set { _idGrupoSanguieno = value; }
        }
        public string GrupoSanguineo
        {
            get { return _grupoSanguineo == null ? string.Empty : _grupoSanguineo; }
            set { _grupoSanguineo = value; }
        }


        #endregion
    }
}
