using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Generales.Entidades;

namespace Bancos.Entidades
{
    [Serializable]
    public partial class TESCobranzasExternasConciliacionesDetalles : Objeto
    {
        #region Private Members
        int _idCobranzaExternaConciliacionDetalle;
        int _idCobranzaExternaConciliacion;
        string _detalle;
        TESTarjetasTransacciones _tarjetaTransaccion;
        bool _checked;
        #endregion

        #region Constructors
        public TESCobranzasExternasConciliacionesDetalles()
        {
        }
        #endregion

        #region Public Properties
        public int IdCobranzaExternaConciliacionDetalle
        {
            get { return _idCobranzaExternaConciliacionDetalle; }
            set { _idCobranzaExternaConciliacionDetalle = value; }
        }

        public int IdCobranzaExternaConciliacion
        {
            get { return _idCobranzaExternaConciliacion; }
            set { _idCobranzaExternaConciliacion = value; }
        }

        public string Detalle
        {
            get { return _detalle; }
            set { _detalle = value; }
        }
        private string numeroLote;

        public string NumeroLote
        {
            get { return numeroLote; }
            set { numeroLote = value; }
        }

        private string fechaTransaccion;

        public string FechaTransaccion
        {
            get { return fechaTransaccion; }
            set { fechaTransaccion = value; }
        }


        public TESTarjetasTransacciones TarjetaTransaccion
        {
            get { return _tarjetaTransaccion == null ? (_tarjetaTransaccion = new TESTarjetasTransacciones()) : _tarjetaTransaccion; }
            set { _tarjetaTransaccion = value; }
        }

        public bool Checked
        {
            get { return _checked; }
            set { _checked = value; }
        }
        #endregion
    }
}
