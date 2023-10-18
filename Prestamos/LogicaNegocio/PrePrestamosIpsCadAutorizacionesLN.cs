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
    class PrePrestamosIpsCadAutorizacionesLN : BaseLN<PrePrestamosIpsCadAutorizaciones>
    {
        public override bool Agregar(PrePrestamosIpsCadAutorizaciones pParametro)
        {
            throw new NotImplementedException();
        }

        public override bool Modificar(PrePrestamosIpsCadAutorizaciones pParametro)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pParametro">Numero</param>
        /// <returns></returns>
        public override PrePrestamosIpsCadAutorizaciones ObtenerDatosCompletos(PrePrestamosIpsCadAutorizaciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<PrePrestamosIpsCadAutorizaciones>("PrePrestamosIpsCadAutorizacionesSeleccionar", pParametro);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pParametro">Numero</param>
        /// <returns></returns>
        internal PrePrestamosIpsCadAutorizaciones ObtenerDatosCompletos(PrePrestamosIpsCadAutorizaciones pParametro, Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<PrePrestamosIpsCadAutorizaciones>("PrePrestamosIpsCadAutorizacionesSeleccionar", pParametro, bd, tran);
        }

        public override List<PrePrestamosIpsCadAutorizaciones> ObtenerListaFiltro(PrePrestamosIpsCadAutorizaciones pParametro)
        {
            throw new NotImplementedException();
        }

        public DataTable ObtenerSeleccionarGrilla(PrePrestamosIpsCadAutorizaciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("PrePrestamosIpsCadAutorizacionesSeleccionarGrilla", pParametro);
        }

        internal bool ModificarEstado(PrePrestamosIpsCadAutorizaciones pParametro, Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "PrePrestamosIpsCadAutorizacionesActualizarEstado");
        }

        public List<PrePrestamosIpsCadAutorizaciones> ObtenerPeriodos()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<PrePrestamosIpsCadAutorizaciones>("PrePrestamosIpsCadAutorizacionesListarPeriodos");
        }
    }
}
