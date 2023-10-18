
using Comunes.Entidades;
using System;
namespace Afiliados.Entidades
{
    [Serializable]
    public partial class AfiMensajesAlertas : Objeto
    {
        // Class AfiMensajesAlertas
        #region "Private Members"
        int _idMensajeAlerta;
        int _idAfiliado;
        string _mensaje;
        DateTime _fechaAlta;
        int _idUsuarioAlta;
        DateTime? _fechaEvento;
        int? _idUsuarioEvento;
        #endregion

        #region "Constructors"
        public AfiMensajesAlertas()
        {
        }
        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdMensajeAlerta
        {
            get { return _idMensajeAlerta; }
            set { _idMensajeAlerta = value; }
        }

        public int IdAfiliado
        {
            get { return _idAfiliado; }
            set { _idAfiliado = value; }
        }
        [Auditoria()]
        public string Mensaje
        {
            get { return _mensaje == null ? string.Empty : _mensaje; }
            set { _mensaje = value; }
        }

        public DateTime FechaAlta
        {
            get { return _fechaAlta; }
            set { _fechaAlta = value; }
        }

        public int IdUsuarioAlta
        {
            get { return _idUsuarioAlta; }
            set { _idUsuarioAlta = value; }
        }

        public DateTime? FechaEvento
        {
            get { return _fechaEvento; }
            set { _fechaEvento = value; }
        }
        [Auditoria()]
        public int? IdUsuarioEvento
        {
            get { return _idUsuarioEvento; }
            set { _idUsuarioEvento = value; }
        }
        #endregion
    }
}
