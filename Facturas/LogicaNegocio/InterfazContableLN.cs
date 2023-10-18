using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facturas.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Comunes.Entidades;
using Generales.Entidades;
using Contabilidad.Entidades;
using Contabilidad;
using Comunes.LogicaNegocio;
using Servicio.AccesoDatos;

namespace Facturas.LogicaNegocio
{
    class InterfazContableLN
    {
        public bool AgregarComprobante(VTAFacturas pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdFactura;
            asiento.Filial.IdFilial = pParametro.Filial.IdFilial;
            asiento.FechaAsiento = pParametro.FechaFactura;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                resultado = false;
            }


            if (BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "VTAFacturasValidacionesAsientoCostoMercaderiaVendida"))
            {
                asiento = new CtbAsientosContables();
                asiento.IdTipoOperacion = 204; //Cambiame BOLUDO!!!
                asiento.IdRefTipoOperacion = pParametro.IdFactura;
                asiento.Filial.IdFilial = pParametro.Filial.IdFilial;
                asiento.FechaAsiento = pParametro.FechaFactura;
                asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
                if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbContabilizacionInterfacesPorOperaciones"))
                {
                    AyudaProgramacionLN.MapearError(asiento, pParametro);
                    resultado = false;
                }
            }
            return resultado;

        }

        public bool AnularComprobante(VTAFacturas pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;
            CtbAsientosContables asiento = new CtbAsientosContables();
            asiento.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            asiento.IdRefTipoOperacion = pParametro.IdFactura;
            asiento.UsuarioLogueado = pParametro.UsuarioLogueado;
            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(asiento, bd, tran, "CtbAsientosContablesAnular"))
            {
                AyudaProgramacionLN.MapearError(asiento, pParametro);
                resultado = false;
            }
            return resultado;
        }
    }
}
