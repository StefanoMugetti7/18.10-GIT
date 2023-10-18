using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mailing.Entidades
{
    [Serializable]
    public class TGEMailingProcesamientosPlantillas : TGEPlantillas
    {
        int _idMailingPlantilla;
        int _idMailingProcesamiento;

        [PrimaryKey]
        public int IdMailingPlantilla
        {
            get { return _idMailingPlantilla; }
            set { _idMailingPlantilla = value; }
        }

        public int IdMailingProcesamiento
        {
            get { return _idMailingProcesamiento; }
            set { _idMailingProcesamiento = value; }
        }
    }
}
