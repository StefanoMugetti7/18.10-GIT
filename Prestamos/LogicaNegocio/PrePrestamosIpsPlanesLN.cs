using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prestamos.Entidades;
using Comunes;
using System.Data;
using Servicio.AccesoDatos;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;

namespace Prestamos.LogicaNegocio
{
    class PrePrestamosIpsPlanesLN : BaseLN<PrePrestamosIpsPlanes>
    {
        public override bool Agregar(PrePrestamosIpsPlanes pParametro)
        {
            throw new NotImplementedException();
        }

        public override bool Modificar(PrePrestamosIpsPlanes pParametro)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pParametro">IdPlan</param>
        /// <param name="pParametro">CantidadCuotas</param>
        /// <param name="pParametro">ImporteCuota</param>
        /// <returns></returns>
        public override PrePrestamosIpsPlanes ObtenerDatosCompletos(PrePrestamosIpsPlanes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<PrePrestamosIpsPlanes>("PrePrestamosIpsPlanesSeleccionar", pParametro);
        }

        public override List<PrePrestamosIpsPlanes> ObtenerListaFiltro(PrePrestamosIpsPlanes pParametro)
        {
            throw new NotImplementedException();
        }

        public DataTable ObtenerSeleccionarGrilla(PrePrestamosIpsPlanes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("", pParametro);
        }

    }
}
