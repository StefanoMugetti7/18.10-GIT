using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turismo.Entidades
{
    [Serializable]
    public class CtrTurismo : Objeto
    {
        [PrimaryKey]
        public int IdTurismo { get; set; }

        enum TipoControl
        {
            ReservaTurismo,
            ReservaPaquete,
        }
    }
}
