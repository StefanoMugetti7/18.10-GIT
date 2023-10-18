using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicina.Entidades
{
    [Serializable]
    public partial class MedPrestadoresEspecializacionesDiasHoras : Objeto
    {

        int _idPrestadorDiaHora;
        int _idEspecializacion;

        public int IdPrestadorDiaHora
        {
            get { return _idPrestadorDiaHora; }
            set { _idPrestadorDiaHora = value; }
        }

        public int IdEspecializacion
        {
            get { return _idEspecializacion; }
            set { _idEspecializacion = value; }
        }
    }
}
