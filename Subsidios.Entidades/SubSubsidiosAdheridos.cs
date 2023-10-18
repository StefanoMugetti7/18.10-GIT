using System;
using System.Collections.Generic;
using Comunes.Entidades;

namespace Subsidios.Entidades
{
    [Serializable]

    public class SubSubsidiosAdheridos : Objeto
    {

        #region "Private Members"

        int _idSubsidiosAdheridos;
        int _idAfiliado;
        SubSubsidios _subsidio;
        DateTime _fechaAlta;
        int _idUsuarioAlta;
        DateTime? _fechaAutorizacion;
        int _idUsuarioAutorizacion;
        DateTime? _fechaBaja;
        int _idUsuarioBaja;

        #endregion


        #region "Constructors"
        public SubSubsidiosAdheridos()
        {
        }
        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdSubsidiosAdheridos
        {
            get { return _idSubsidiosAdheridos; }
            set { _idSubsidiosAdheridos = value; }
        }
        public int IdAfiliado
        {
            get { return _idAfiliado; }
            set { _idAfiliado = value; }
        }
        public SubSubsidios Subsidio
        {
            get { return _subsidio == null ? (_subsidio = new SubSubsidios()) : _subsidio; }
            set { _subsidio = value; }
        }

        [Auditoria()]
        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }

        public int IdUsuarioAlta
        {
            get { return _idUsuarioAlta ; }
            set { _idUsuarioAlta = value; }
        }

        [Auditoria()]
        public DateTime? FechaAutorizacion
        {
            get { return _fechaAutorizacion; }
            set { _fechaAutorizacion = value; }
        }

        public int IdUsuarioAutorizacion
        {
            get { return _idUsuarioAutorizacion; }
            set { _idUsuarioAutorizacion = value; }
        }

        [Auditoria()]

        public DateTime? FechaBaja
        {
            get { return _fechaBaja; }
            set { _fechaBaja = value; }
        }

        public int IdUsuarioBaja
	    {
		get{return _idUsuarioBaja   ;}
		set{_idUsuarioBaja = value;}
	    }

        #endregion
    }
}

