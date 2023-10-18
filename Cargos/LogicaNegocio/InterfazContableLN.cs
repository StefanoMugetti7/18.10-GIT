using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cargos.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Comunes.Entidades;
using Contabilidad.Entidades;
using Contabilidad;
using Comunes.LogicaNegocio;
using Generales.Entidades;

namespace Cargos.LogicaNegocio
{
    class InterfazContableLN
    {
        public bool PagoHaberes(CarCuentasCorrientes pParametro, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdRefTipoOperacion;// pParametro.IdCuentaCorriente;
            asiento.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asiento.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asiento.FechaAsiento = pParametro.FechaMovimiento;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            asiento.IdEjercicioContable = ContabilidadF.EjerciciosContablesObtenerActivo(asiento, bd, tran).IdEjercicioContable;

            CtbAsientosModelos modelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(asiento, bd, tran);

            if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.Importe, modelo, EnumCodigosAsientosModelos.HabPoderesPagar))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            //if (!ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, pParametro.Importe, modelo, EnumCodigosAsientosModelos.TablaCaja, pParametro.Filial, pParametro.TipoValor.IdTipoValor, 0, bd, tran))
            //{
            //    AyudaProgramacionLN.MapearError(asiento, pParametro);
            //    return false;
            //}

            foreach (InterfazValoresImportes item in pValoresImportes)
            {
                #region "Asiento Caja y Banco"
                if ((item.IdTipoValor == (int)EnumTiposValores.Efectivo)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaCaja, pParametro.Filial, item.IdTipoValor, 0, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

                //if ((item.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
                //    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.ValDepo))
                //{
                //    AyudaProgramacionLN.MapearError(asiento, pParametro);
                //    return false;
                //}

                if ((item.IdTipoValor == (int)EnumTiposValores.Transferencia || item.IdTipoValor == (int)EnumTiposValores.Cheque)
                    && !ContabilidadF.AsientosModelosObtenerAsientoDetalle(asiento, item.Importe, modelo, EnumCodigosAsientosModelos.TablaBanco, pParametro.Filial, item.IdTipoValor, 0, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    return false;
                }
                #endregion
            }  

            if (!ContabilidadF.AsientosContablesAgregar(asiento, bd, tran))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                return false;
            }

            return true;
        }

    }
}
