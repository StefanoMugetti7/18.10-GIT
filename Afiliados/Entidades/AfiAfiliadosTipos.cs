using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Afiliados.Entidades
{
    [Serializable]
    public class AfiAfiliadosTipos : Objeto
    {
        int _idAfiliadoTipo;
        string _afiliadoTipo;

        [PrimaryKey()]
        public int IdAfiliadoTipo
        {
            get { return _idAfiliadoTipo; }
            set { _idAfiliadoTipo = value; }
        }
        public string AfiliadoTipo
        {
            get { return _afiliadoTipo == null ? string.Empty : _afiliadoTipo; }
            set { _afiliadoTipo = value; }
        }
    }
}
