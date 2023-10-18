using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Afiliados.Entidades;

namespace CuentasPagar.Entidades
{
    [Serializable]
    public class CapSolicitudPagoCausantesBeneficios : Objeto
    {

        int _idSolicitudPagoCausanteBeneficio;
        int _idSolicitudPago;
        int _idAfiliado;
        AfiTiposDocumentos _tipoDocumento;
        long _numeroDocumento;
        string _nombre;
        string _apellido;

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

        public AfiTiposDocumentos TipoDocumento
        {
            get { return _tipoDocumento == null ? (_tipoDocumento = new AfiTiposDocumentos()) : _tipoDocumento; }
            set { _tipoDocumento = value; }
        }

        public long NumeroDocumento
        {
            get { return _numeroDocumento; }
            set { _numeroDocumento = value; }
        }

        public string Nombre
        {
            get { return _nombre == null ? string.Empty : _nombre; }
            set { _nombre = value; }
        }
        [Auditoria()]
        public string Apellido
        {
            get { return _apellido == null ? string.Empty : _apellido; }
            set { _apellido = value; }
        }

        public string ApellidoNombre
        {
            get { return this.IdAfiliado > 0 ? string.Concat(this.Apellido, this.Nombre.Length > 0 ? ", " : string.Empty, this.Nombre) : string.Empty; }
            set { }
        }
    }
}
