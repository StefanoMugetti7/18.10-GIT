using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Afiliados.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using Comunes.Entidades;
using Comunes.LogicaNegocio;

namespace Afiliados.LogicaNegocio
{
    class AfiGradosLN : BaseLN<AfiGrados>
    {
        public override AfiGrados ObtenerDatosCompletos(AfiGrados pParametro)
        {
            throw new NotImplementedException();
        }

        public override List<AfiGrados> ObtenerListaFiltro(AfiGrados pParametro)
        {
            throw new NotImplementedException();
        }

        public List<AfiGrados> ObtenerListaActiva()
        {
            List<AfiGrados> lista = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiGrados>("AfiGradosListar");
            lista = lista.Where(x => x.Estado.IdEstado == (int)Estados.Activo).ToList();
            return AyudaProgramacionLN.ReacomodarIndicesColecion<AfiGrados>(lista);
        }

        public override bool Agregar(AfiGrados pParametro)
        {
            throw new NotImplementedException();
        }

        public override bool Modificar(AfiGrados pParametro)
        {
            throw new NotImplementedException();
        }
    }
}
