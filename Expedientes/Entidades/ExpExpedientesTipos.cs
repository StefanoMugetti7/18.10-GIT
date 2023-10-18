using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generales.Entidades;
using Comunes.Entidades;

namespace Expedientes.Entidades
{
    public class ExpExpedientesTipos : TGEListasValoresDetalles
    {
        int _idExpedienteTipo;

        public int IdExpedienteTipo
        {
            get { return _idExpedienteTipo; }
            set { _idExpedienteTipo = value; }
        }
    }
}
