using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;

namespace Afiliados.Entidades
{
    [Serializable]
    public partial class AfiCompensaciones : Objeto
    {
        #region Private Members
        int _idCompensacion;
        string _representacion;
        string _cargo;
        decimal _compensacion;
        decimal _gastosRepresentacion;
        decimal _fondoRepresentacion;
        decimal _totalAAcreditar;
        int _idCuenta;
        int _numeroCuenta;
        int _idAfiliado;
        string _apellidoNombre;
        string _numeroSocio;
        //TGEFiliales _filial;
        int? _idFilial;
        string _codigoFilial;
        string _filial;
        List<TGEArchivos> _archivos;
        List<TGEComentarios> _comentarios;

        #endregion

        #region Constructors
        public AfiCompensaciones()
        {
        }
        #endregion

        #region Public Properties
        [PrimaryKey()]
        public int IdCompensacion
        {
            get { return _idCompensacion; }
            set { _idCompensacion = value; }
        }

        public string Representacion
        {
            get { return _representacion; }
            set { _representacion = value; }
        }

        public string Cargo
        {
            get { return _cargo; }
            set { _cargo = value; }
        }

        [Auditoria()]
        public decimal Compensacion
        {
            get { return _compensacion; }
            set { _compensacion = value; }
        }

        [Auditoria()]
        public decimal GastosRepresentacion
        {
            get { return _gastosRepresentacion; }
            set { _gastosRepresentacion = value; }
        }

        [Auditoria()]
        public decimal FondoRepresentacion
        {
            get { return _fondoRepresentacion; }
            set { _fondoRepresentacion = value; }
        }

        public decimal TotalAAcreditar
        {
            get { return _totalAAcreditar; }
            set { _totalAAcreditar = value; }
        }

        [Auditoria()]
        public int IdCuenta
        {
            get { return _idCuenta; }
            set { _idCuenta = value; }
        }

        public int NumeroCuenta
        {
            get { return _numeroCuenta; }
            set { _numeroCuenta = value; }
        }
        //public TGEFiliales Filial
        //{
        //    get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
        //    set { _filial = value; }
        //}

        public int IdAfiliado
        {
            get { return _idAfiliado; }
            set { _idAfiliado = value; }
        }

        public string ApellidoNombre
        {
            get { return _apellidoNombre; }
            set { _apellidoNombre = value; }
        }

        public string NumeroSocio
        {
            get { return _numeroSocio; }
            set { _numeroSocio = value; }
        }


        public int? IdFilial
        {
            get { return _idFilial; }
            set { _idFilial = value; }
        }
        public string CodigoFilial
        {
            get { return _codigoFilial; }
            set { _codigoFilial = value; }
        }
        public string Filial
        {
            get { return _filial; }
            set { _filial = value; }
        }

        public List<TGEArchivos> Archivos
        {
            get { return _archivos == null ? (_archivos = new List<TGEArchivos>()) : _archivos; }
            set { _archivos = value; }
        }

        public List<TGEComentarios> Comentarios
        {
            get { return _comentarios == null ? (_comentarios = new List<TGEComentarios>()) : _comentarios; }
            set { _comentarios = value; }
        }
        #endregion
    }
}
