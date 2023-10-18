using Generales.Entidades;
using System;

namespace Afiliados.Entidades
{
    [Serializable]
    public class AfiPacientes : AfiAfiliados
    {
        TGEObrasSociales _obraSocial;


        public TGEObrasSociales ObraSocial
        {
            get { return _obraSocial == null ? (_obraSocial = new TGEObrasSociales()) : _obraSocial; }
            set { _obraSocial = value; }
        }

    }
}
