using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Afiliados.Entidades
{
    [Serializable]
    public class AfiTiposApoderados : Objeto
    {
        int _idTipoApoderado;
        string _tipoApoderado;

        [PrimaryKey()]
        public int IdTipoApoderado
        {
            get { return _idTipoApoderado; }
            set { _idTipoApoderado = value; }
        }

        [Auditoria()]
        public string TipoApoderado
        {
            get { return _tipoApoderado == null ? string.Empty : _tipoApoderado; }
            set { _tipoApoderado = value; }
        }
    }
}