using Ahorros.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bancos.Entidades
{
    [Serializable]
    public partial class TESPlazosFijos : Objeto
    {
        #region "Private Members"
        int _idPlazoFijo;
        int _idPlazoFijoAnterior;
  
        TESBancosCuentas _bancoCuenta;
        AhoTiposRenovaciones _tipoRenovacion;
        TGETiposOperaciones _tipoOperacion;
        TGETiposValores _tipoValor;
        TGEMonedas _moneda;
        UsuariosAlta _usuarioAlta;
        DateTime _fechaVencimiento;
        DateTime _fechaInicioVigencia;
        string _descripcion;
        List<TGEArchivos> _archivos;
        #endregion

        #region "Constructors"
        public TESPlazosFijos()
        {
        }
        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdPlazoFijo
        {
            get { return _idPlazoFijo; }
            set { _idPlazoFijo = value; }
        }

        [Auditoria()]
        public int IdPlazoFijoAnterior
        {
            get { return _idPlazoFijoAnterior; }
            set { _idPlazoFijoAnterior = value; }
        }

        

        public TESBancosCuentas BancoCuenta
        {
            get { return _bancoCuenta == null ? (_bancoCuenta = new TESBancosCuentas()) : _bancoCuenta; }
            set { _bancoCuenta = value; }
        }

        public decimal ImporteCapital { get; set; }

        public decimal TasaInteres { get; set; }

        public decimal ImporteInteres { get; set; }
        public decimal ImporteTotal { get; set; }

        //[Auditoria()]
        //public AhoTiposRenovaciones TipoRenovacion
        //{
        //    get { return _tipoRenovacion == null ? (_tipoRenovacion = new AhoTiposRenovaciones()) : _tipoRenovacion; }
        //    set { _tipoRenovacion = value; }
        //}

        public string Descripcion
        {
            get { return _descripcion == null ? string.Empty : _descripcion; }
            set { _descripcion = value; }
        }

        public TGETiposOperaciones TipoOperacion
        {
            get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
            set { _tipoOperacion = value; }
        }

        [Auditoria()]
        public decimal MonedaCotizacion { get; set; }

        public DateTime FechaAlta { get; set; }

        public UsuariosAlta UsuarioAlta
        {
            get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
            set { _usuarioAlta = value; }
        }

        public int IdUsuarioEvento { get; set; }

        public int IdEstado { get; set; }

        public DateTime FechaVencimiento
        {
            get { return _fechaVencimiento; }
            set { _fechaVencimiento = value; }
        }

        public DateTime FechaInicioVigencia
        {
            get { return _fechaInicioVigencia; }
            set { _fechaInicioVigencia = value; }
        }


        public List<TGEArchivos> Archivos
        {
            get { return _archivos == null ? (_archivos = new List<TGEArchivos>()) : _archivos; }
            set { _archivos = value; }
        }

        #endregion
    }
}
