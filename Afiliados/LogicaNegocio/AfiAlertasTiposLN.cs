using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Afiliados.Entidades;
using Comunes;
using Servicio.AccesoDatos;

namespace Afiliados.LogicaNegocio
{
    class AfiAlertasTiposLN : BaseLN<AfiAlertasTipos>
    {
        public override AfiAlertasTipos ObtenerDatosCompletos(AfiAlertasTipos pParametro)
        {
            throw new NotImplementedException();
        }

        public override List<AfiAlertasTipos> ObtenerListaFiltro(AfiAlertasTipos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiAlertasTipos>("AfiAlertasTiposListar", pParametro);
        }

        public override bool Agregar(AfiAlertasTipos pParametro)
        {
            throw new NotImplementedException();
        }

        public override bool Modificar(AfiAlertasTipos pParametro)
        {
            throw new NotImplementedException();
        }
    }
}
