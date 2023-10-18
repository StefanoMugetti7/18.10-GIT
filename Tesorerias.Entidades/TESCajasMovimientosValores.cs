using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generales.Entidades;
using Comunes.Entidades;
using Bancos.Entidades;
using System.Xml;

namespace Tesorerias.Entidades
{
    [Serializable]
    public class TESCajasMovimientosValores : Objeto
    {
        int _idCajaMovimientoValor;
        int _idCajaMovimiento;
        string _descripcion;
        DateTime? _fecha;
        TGETiposValores _tipoValor;
        decimal _importe;
        List<TESCheques> _cheques;
        //List<TESCheques> _chequesTerceros;
        List<TESBancosCuentasMovimientos> _bancoCunentaMovimientos;
        List<TESTarjetasTransacciones> _tarjetasTransacciones;
        XmlDocumentSerializationWrapper _loteCamposValores;
        [PrimaryKey()]
        public int IdCajaMovimientoValor
        {
            get { return _idCajaMovimientoValor; }
            set { _idCajaMovimientoValor = value; }
        }

        public int IdCajaMovimiento
        {
            get { return _idCajaMovimiento; }
            set { _idCajaMovimiento = value; }
        }

        public DateTime? Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }

        public string Descripcion
        {
            get { return _descripcion == null ? string.Empty :  _descripcion; }
            set { _descripcion = value; }
        }

        public TGETiposValores TipoValor
        {
            get { return _tipoValor == null ? (_tipoValor = new TGETiposValores()) : _tipoValor; }
            set { _tipoValor = value; }
        }

        public decimal Importe
        {
            get { return _importe; }
            set { _importe = value; }
        }

        public List<TESCheques> Cheques
        {
            get { return _cheques == null ? (_cheques = new List<TESCheques>()) : _cheques; }
            set { _cheques = value; }
        }

        //public List<TESCheques> ChequesTerceros
        //{
        //    get { return _chequesTerceros == null ? (_chequesTerceros = new List<TESCheques>()) : _chequesTerceros; }
        //    set { _chequesTerceros = value; }
        //}

        public List<TESBancosCuentasMovimientos> BancosCuentasMovimientos
        {
            get { return _bancoCunentaMovimientos == null ? (_bancoCunentaMovimientos = new List<TESBancosCuentasMovimientos>()) : _bancoCunentaMovimientos; }
            set { _bancoCunentaMovimientos = value; }
        }

        public List<TESTarjetasTransacciones> TarjetasTransacciones
        {
            get { return _tarjetasTransacciones == null ? (_tarjetasTransacciones = new List<TESTarjetasTransacciones>()) : _tarjetasTransacciones; }
            set { _tarjetasTransacciones = value; }
        }

        public XmlDocument LoteCamposValores
        {
            get { return _loteCamposValores; }//  ? (_loteCamposValores = new XmlDocumentSerializationWrapper()) : _loteCamposValores; }
            set { _loteCamposValores = value; }
        }

        public int IdCuenta { get; set; }
    }
}
