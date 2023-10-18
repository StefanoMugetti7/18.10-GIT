using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Entidades
{
    [Serializable]
    public class TurTurismo
    {
        public int IdRefTablaValor { get; set; }
        public int NumeroReserva { get; set; }

        UsuarioLogueado _usuarioLogueado;
        public UsuarioLogueado UsuarioLogueado
        {
            get { return _usuarioLogueado == null ? (_usuarioLogueado = new UsuarioLogueado()) : _usuarioLogueado; }
            set { _usuarioLogueado = value; }
        }
    }
}
