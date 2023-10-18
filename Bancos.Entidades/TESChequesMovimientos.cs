
using System;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
namespace Bancos.Entidades
{
    [Serializable]
    public partial class TESChequesMovimientos : Objeto
    {
        #region "Private Members"
        int _idChequeMovimiento;
        DateTime _fecha;
        string _descripcion;
        int _idRefChequeMovimiento;
        int _idRefBancoCuentaMovimiento;
        int _idRefTipoOperacion;
        TGETiposOperaciones _tipoOperacion;
        TGEFiliales _filial;
        TGEFilialesDestinos _filialDestino;
        TESCheques _cheque;
        TESBancosCuentas _bancoCuenta;
        #endregion

        #region "Constructors"
        public TESChequesMovimientos()
        {
        }
        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdChequeMovimiento
        {
            get { return _idChequeMovimiento; }
            set { _idChequeMovimiento = value; }
        }

        public DateTime Fecha
        {
            get { return _fecha; }
            set { _fecha = value; }
        }

        public string Descripcion
        {
            get { return _descripcion == null ? string.Empty : _descripcion; }
            set { _descripcion = value; }
        }

        public int IdRefChequeMovimiento
        {
            get { return _idRefChequeMovimiento; }
            set { _idRefChequeMovimiento = value; }
        }

        public int IdRefBancoCuentaMovimiento
        {
            get { return _idRefBancoCuentaMovimiento; }
            set { _idRefBancoCuentaMovimiento = value; }
        }

        public int IdRefTipoOperacion
        {
            get { return _idRefTipoOperacion; }
            set { _idRefTipoOperacion = value; }
        }

        public TGETiposOperaciones TipoOperacion
        {
            get { return _tipoOperacion == null ? (_tipoOperacion = new TGETiposOperaciones()) : _tipoOperacion; }
            set { _tipoOperacion = value; }
        }

        public TGEFiliales Filial
        {
            get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
            set { _filial = value; }
        }

        public TGEFilialesDestinos FilialDestino
        {
            get { return _filialDestino == null ? (_filialDestino = new TGEFilialesDestinos()) : _filialDestino; }
            set { _filialDestino = value; }
        }

        public TESCheques Cheque
        {
            get { return _cheque == null ? (_cheque = new TESCheques()) : _cheque; }
            set { _cheque = value; }
        }

        public TESBancosCuentas BancoCuenta
        {
            get { return _bancoCuenta == null ? (_bancoCuenta = new TESBancosCuentas()) : _bancoCuenta; }
            set { _bancoCuenta = value; }
        }

        #endregion
    }
}
