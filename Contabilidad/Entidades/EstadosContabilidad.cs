using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Contabilidad.Entidades
{
    public enum EstadosContabilidad
    {
        Activo=1,
        Baja=0,
        Contabilizado=99, //verificar
    }

    public enum EstadosEjerciosContables
    {
        Baja=0,
        Activo=1,
        Cerrado,
        Copiado,
    }
}
