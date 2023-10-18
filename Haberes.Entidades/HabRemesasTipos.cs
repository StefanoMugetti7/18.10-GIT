using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generales.Entidades;
using Comunes.Entidades;

namespace Haberes.Entidades
{
    [Serializable]
	public class HabRemesasTipos : TGEListasValoresDetalles
    {
        int _idRemesaTipo;

        public int IdRemesaTipo
        {
            get { return _idRemesaTipo; }
            set { _idRemesaTipo = value; }
        }
    }
}
