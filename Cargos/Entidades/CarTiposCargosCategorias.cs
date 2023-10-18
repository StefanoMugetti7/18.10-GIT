using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Afiliados.Entidades;

namespace Cargos.Entidades
{
    public class CarTiposCargosCategorias : Objeto
    {
        #region "Private Members"
        int _idTipoCargoCategoria;
        AfiCategorias _categoria;
        int _idTipoCargo;
        decimal _importe;
        DateTime _fechaAlta;
        DateTime _fechaVigenciaDesde;
        UsuariosAlta _usuarioAlta;

        #endregion

        #region "Constructors"
        public CarTiposCargosCategorias()
        {
        }
        #endregion

        #region "Public Properties"
        public int IdTipoCargoCategoria
        {
            get { return _idTipoCargoCategoria; }
            set { _idTipoCargoCategoria = value; }
        }
        public AfiCategorias Categoria
        {
            get { return _categoria == null ? (_categoria = new AfiCategorias()) : _categoria; }
            set { _categoria = value; }
        }

        public int IdTipoCargo
        {
            get { return _idTipoCargo; }
            set { _idTipoCargo = value; }
        }

        public decimal Importe
        {
            get { return _importe; }
            set { _importe = value; }
        }

        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }

        public DateTime FechaVigenciaDesde
        {
            get { return _fechaVigenciaDesde; }
            set { _fechaVigenciaDesde = value; }
        }

        public UsuariosAlta UsuarioAlta
        {
            get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
            set { _usuarioAlta = value; }
        }

        #endregion
    }
}
