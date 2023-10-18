
using System;
using System.Collections.Generic;
using Afiliados.Entidades;
using Comunes.Entidades;
namespace Afiliados.Entidades
{
    [Serializable]
    public partial class AfiEmpresasTelefonicas : Objeto
    {

        #region "Private Members"
        int? _idEmpresaTelefonica;
        string _descripcion;
        #endregion

        #region "Constructors"
        public AfiEmpresasTelefonicas()
        {
        }
        #endregion

        #region "Public Properties"
        public int? IdEmpresaTelefonica
        {
            get { return _idEmpresaTelefonica; }
            set { _idEmpresaTelefonica = value; }
        }
        public string Descripcion
        {
            get { return _descripcion == null ? string.Empty : _descripcion; }
            set { _descripcion = value; }
        }

        #endregion
    }
}