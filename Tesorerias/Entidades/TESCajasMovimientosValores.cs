using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generales.Entidades;
using Comunes.Entidades;
using Bancos.Entidades;

namespace Tesorerias.Entidades
{
    public class TESCajasMovimientosValores : Objeto
    {
        int _idCajaMovimientoValor;
        int _idCajaMovimiento;
        TGETiposValores _tipoValor;
        decimal _importe;
        List<TESCheques> _cheques;
        List<TESBancosCuentasMovimientos> _bancoCunentaMovimientos;

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

        public List<TESBancosCuentasMovimientos> BancosCuentasMovimientos
        {
            get { return _bancoCunentaMovimientos == null ? (_bancoCunentaMovimientos = new List<TESBancosCuentasMovimientos>()) : _bancoCunentaMovimientos; }
            set { _bancoCunentaMovimientos = value; }
        }
    }
}
