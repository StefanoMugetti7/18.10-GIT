
using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
namespace Afiliados.Entidades
{
    [Serializable]
    public partial class AfiCertificadosSupervivencia : Objeto
    {
        #region "Private Members"
        int _idCertificadoSupervivencia;
        int _idAfiliado;
        DateTime _fechaAlta;
        DateTime _fechaCertificacion;
        string _detalle;
        UsuariosAlta _usuarioAlta;
        List<TGEArchivos> _archivos;

        #endregion

        #region "Constructors"
        public AfiCertificadosSupervivencia()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdCertificadoSupervivencia
        {
            get { return _idCertificadoSupervivencia; }
            set { _idCertificadoSupervivencia = value; }
        }
        public int IdAfiliado
        {
            get { return _idAfiliado; }
            set { _idAfiliado = value; }
        }

        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }
        [Auditoria()]
        public DateTime FechaCertificacion
        {
            get { return _fechaCertificacion; }
            set { _fechaCertificacion = value; }
        }
        [Auditoria()]
        public string Detalle
        {
            get { return _detalle == null ? string.Empty : _detalle; }
            set { _detalle = value; }
        }

        public UsuariosAlta UsuarioAlta
        {
            get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
            set { _usuarioAlta = value; }
        }

        public List<TGEArchivos> Archivos
        {
            get { return _archivos == null ? (_archivos = new List<TGEArchivos>()) : _archivos; }
            set { _archivos = value; }
        }

        #endregion
    }
}
