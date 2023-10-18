﻿using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mailing.Entidades
{
    [Serializable]
    public class TGEMailingProcesamientosAdjuntos : Objeto
    {
        TGEPlantillas _plantilla;
        [PrimaryKey]
        public int IdMailingProcesamientoAdjunto { get; set; }
        public int IdMailingProcesamiento { get; set; }
        public TGEPlantillas Plantilla
        {
            get { return _plantilla == null ? (_plantilla = new TGEPlantillas()) : _plantilla; }
            set { _plantilla = value; }
        }
    }
}
