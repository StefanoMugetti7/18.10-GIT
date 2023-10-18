
using Comunes.Entidades;
using System;
using System.Collections.Generic;
namespace Afiliados.Entidades
{
    [Serializable]
    public partial class AfiDomiciliosTipos : Objeto
    {

        #region "Private Members"
        int _idDomicilioTipo;
        string _descripcion;
        int _idEstado;
        int _idUsuarioEvento;
        byte[] _selloTiempo;
        List<AfiDomicilios> _afiDomicilios;
        #endregion

        #region "Constructors"
        public AfiDomiciliosTipos()
        {
        }
        #endregion

        #region "Public Properties"
        public int IdDomicilioTipo
        {
            get { return _idDomicilioTipo; }
            set { _idDomicilioTipo = value; }
        }
        public string Descripcion
        {
            get { return _descripcion == null ? string.Empty : _descripcion; }
            set { _descripcion = value; }
        }

        public int IdEstado
        {
            get { return _idEstado; }
            set { _idEstado = value; }
        }

        public int IdUsuarioEvento
        {
            get { return _idUsuarioEvento; }
            set { _idUsuarioEvento = value; }
        }

        public byte[] SelloTiempo
        {
            get { return _selloTiempo; }
            set { _selloTiempo = value; }
        }



        public List<AfiDomicilios> afiDomicilios
        {
            get { return _afiDomicilios == null ? (_afiDomicilios = new List<AfiDomicilios>()) : _afiDomicilios; }
            set { _afiDomicilios = value; }
        }

        #endregion
    }
}
