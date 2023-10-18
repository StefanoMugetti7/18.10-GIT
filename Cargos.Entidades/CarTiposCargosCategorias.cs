using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Afiliados.Entidades;

namespace Cargos.Entidades
{
    [Serializable]
    public class CarTiposCargosCategorias : Objeto
    {
        #region "Private Members"
        int _idTipoCargoCategoria;
        AfiCategorias _categoria;
        int _idTipoCargo;
        decimal _importe;
        decimal __importeOriginal;
        bool _importeModificado;
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
        [PrimaryKey()]
        public int IdTipoCargoCategoria
        {
            get { return _idTipoCargoCategoria; }
            set { _idTipoCargoCategoria = value; }
        }
        [Auditoria]
        public AfiCategorias Categoria
        {
            get { return _categoria == null ? (_categoria = new AfiCategorias()) : _categoria; }
            set { _categoria = value; }
        }
        [Auditoria]
        public int IdTipoCargo
        {
            get { return _idTipoCargo; }
            set { _idTipoCargo = value; }
        }

        [Auditoria]
        public decimal Importe
        {
            get { return _importe; }
            set
            {
                if (_importe != __importeOriginal && !this._importeModificado && _importe != value)
                {
                    this._importeModificado = true;
                    this.__importeOriginal = _importe;
                }
                _importe = value;
            }
        }

        public decimal ImporteOriginal
        {
            get { return __importeOriginal; }
            set { }
        }

        public bool ImporteModificado
        {
            get { return _importeModificado; }
            set { }
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
