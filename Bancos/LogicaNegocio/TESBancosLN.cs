using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bancos.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using Comunes.Entidades;

namespace Bancos.LogicaNegocio
{
    class TESBancosLN : BaseLN<TESBancos>
    {
        public override TESBancos ObtenerDatosCompletos(TESBancos pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<TESBancos>("TesCuentasSeleccionarDescripcion", pParametro);
            return pParametro;
        }

        public override List<TESBancos> ObtenerListaFiltro(TESBancos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESBancos>("TESBancosSeleccionarFiltro", pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Bancos que tienen una Cuenta Banacaria relacionada
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public List<TESBancos> ObtenerListaFiltroConCuentas(TESBancos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESBancos>("TESBancosSeleccionarFiltroConCuentas", pParametro);
        }

        /// <summary>
        /// Devuelve una Lista de Bancos que tienen Cuentas Bancarias para una Filial
        /// </summary>
        /// <param name="pParametro">IdFilial, [IdEstado]</param>
        /// <returns></returns>
        public List<TESBancos> ObtenerListaFilialFiltro(TGEFiliales pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESBancos>("TESBancosSeleccionarFilialFiltro", pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Filiales Destino para Transferencias Bancarias
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public List<TGEFiliales> ObtenerListaTransferenciaDestino(TGEFiliales pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEFiliales>("[TGEFilialesBancosTransferenciaDestino]", pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Filiales Destino de Tesorerias
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public List<TGEFiliales> ObtenerListaTesoreriasDestino(TGEFiliales pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEFiliales>("[TGEFilialesBancosTesoreriasDestino]", pParametro);
        }

        public override bool Agregar(TESBancos pParametro)
        {
            throw new NotImplementedException();
        }

        public override bool Modificar(TESBancos pParametro)
        {
            throw new NotImplementedException();
        }
    }
}
