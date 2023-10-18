using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Generales.Entidades;

namespace Afiliados.Entidades
{
    public partial class AfiArmasDestinos : Objeto
    {

        // Class AfiArmasDestinos

        #region "Private Members"

        int _idArmaDestino;

        AfiArmas _arma;

        string _destino;

        string _calle;

        int _numero;

        int _piso;

        string _departamento;

        TGECodigosPostales _localidad;

        bool _predeterminado;

        DateTime _fechaAlta;

        string _codigoPostal;

        DateTime _fechaEvento;

        int _idUsuarioEvento;

        #endregion


        #region "Constructors"

        public AfiArmasDestinos()
        {

        }

        #endregion


        #region "Public Properties"

        public int IdArmaDestino
        {

            get { return _idArmaDestino; }

            set { _idArmaDestino = value; }

        }

        public AfiArmas Arma
        {

            get { return _arma == null ? (_arma = new AfiArmas()) : _arma; }
            set { _arma = value; }
        }


        public string Destino
        {
            get { return _destino; }
            set { _destino = value; }
        }


        public string Calle
        {

            get { return _calle == null ? string.Empty : _calle; }

            set { _calle = value; }

        }



        public int Numero
        {

            get { return _numero; }

            set { _numero = value; }

        }



        public int Piso
        {

            get { return _piso; }

            set { _piso = value; }

        }



        public string Departamento
        {

            get { return _departamento == null ? string.Empty : _departamento; }

            set { _departamento = value; }

        }



        public TGECodigosPostales Localidad
        {
            get { return _localidad == null ? (_localidad = new TGECodigosPostales()) : _localidad; }
            set { _localidad = value; }
        }



        public bool Predeterminado
        {

            get { return _predeterminado; }

            set { _predeterminado = value; }

        }



        public DateTime FechaAlta
        {

            get { return _fechaAlta; }

            set { _fechaAlta = value; }

        }



        public string CodigoPostal
        {

            get { return _codigoPostal == null ? string.Empty : _codigoPostal; }

            set { _codigoPostal = value; }

        }



        public DateTime FechaEvento
        {

            get { return _fechaEvento; }

            set { _fechaEvento = value; }

        }



        public int IdUsuarioEvento
        {

            get { return _idUsuarioEvento; }

            set { _idUsuarioEvento = value; }

        }



        #endregion
    }
}
