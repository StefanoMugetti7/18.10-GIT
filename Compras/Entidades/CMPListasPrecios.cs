using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Compras.Entidades
{
    class CMPListasPrecios : Objeto
    {
        #region "Private Members"
        int? _idListaPrecio;
        DateTime _fechaInicioVigencia;
        DateTime _fechaAlta;
        #endregion



        #region "Constructors"

        public CMPListasPrecios()
        {
        }
        #endregion


        #region "Public Properties"

        [PrimaryKey()]
        public int? IdListaPrecio
        {
            get{ return _idListaPrecio; }
            set { _idListaPrecio = value; }
        }

        public DateTime FechaInicioVigencia
        {
            get { return _fechaInicioVigencia; }
            set { _fechaInicioVigencia = value; }
        }

        public DateTime FechaAlta
        {
            get{ return _fechaAlta;}
            set{ _fechaAlta = value;}
        }


        #endregion
        

    }
}
