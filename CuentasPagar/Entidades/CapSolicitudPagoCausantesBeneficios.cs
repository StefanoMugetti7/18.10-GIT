using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace CuentasPagar.Entidades
{
    public class CapSolicitudPagoCausantesBeneficios : Objeto
    {

        int _idSolicitudPagoCausanteBeneficio;
        int _idSolicitudPago;
        int _idAfiliado;

        public int IdSolicitudPagoCausanteBeneficio
        {
            get { return _idSolicitudPagoCausanteBeneficio; }
            set {_idSolicitudPagoCausanteBeneficio=value;}
        }

        public int IdSolicitudPago
        {
            get { return _idSolicitudPago; }
            set { _idSolicitudPago = value; }
        }

        public int IdAfiliado
        {
            get { return _idAfiliado; }
            set { _idAfiliado = value; }
        }
    }
}
