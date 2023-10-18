using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cargos.Entidades
{
    public enum EnumTiposCargos
    {
        DescuentosPendientes=15,
        AyudaEconomicaCortoPlazo=67,
        AyudaEconomicaLargoPlazo=68,
    }

    public enum EnumTiposCargosProcesos
    {
        Automatico = 127,
        Administrable = 128,
        PorModulo = 129,
        AdministrableCargoFijo=130,
    }
}
