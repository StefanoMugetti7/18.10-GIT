using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cobros.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Cobros.LogicaNegocio;
using Contabilidad.Entidades;
using Afiliados.Entidades;
using Cargos.Entidades;
using Generales.Entidades;
using System.Net.Mail;
using System.Data;
using Facturas.Entidades;

namespace Cobros
{
    public class CobrosF
    {
        public static bool OrdenesCobrosAgregar(CobOrdenesCobros pParametro)
        {
            return new CobOrdenesCobrosLN().Agregar(pParametro);
        }

        public static bool OrdenesCobrosAgregar(CobOrdenesCobros pParametro, CarTiposCargosAfiliadosFormasCobros pCargoAfiliado)
        {
            return new CobOrdenesCobrosLN().Agregar(pParametro, pCargoAfiliado);
        }

        public static bool OrdenesCobrosAgregarAnticipos(CobOrdenesCobros pParametro)
        {
            return new CobOrdenesCobrosLN().AgregarAnticipos(pParametro);
        }

        public static bool OrdenesCobrosAnular(CobOrdenesCobros pParametro)
        {
            return new CobOrdenesCobrosLN().Anular(pParametro);
        }

        public static bool OrdenesCobrosAnularAfiliadoCobrada(CobOrdenesCobros pParametro)
        {
            return new CobOrdenesCobrosLN().AnularAfiliadoCobrada(pParametro);
        }

        public static bool OrdenesCobrosAnularCobrada(CobOrdenesCobros pParametro)
        {
            return new CobOrdenesCobrosLN().AnularCobrada(pParametro);
        }

        /// <summary>
        /// Modifica el Tipo de Operacion de una Orden de Cobro (Factura) y Genera o Revierte el Asiento Contable
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static bool OrdenesCobrosModificar(CobOrdenesCobros pParametro)
        {
            return new CobOrdenesCobrosLN().Modificar(pParametro);
        }

        //public static bool OrdenesCobrosAgregar(CobOrdenesCobros pParametro, Database bd, DbTransaction tran)
        //{
        //    return new CobOrdenesCobrosLN().Agregar(pParametro, bd, tran);
        //}

        public static bool OrdenesCobrosConfirmar(CobOrdenesCobros pParametro, DateTime pFecha, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            return new CobOrdenesCobrosLN().Confirmar(pParametro, pFecha, pValoresImportes, bd, tran);
        }

        public static CobOrdenesCobros OrdenesCobrosObtenerDatosCompletos(CobOrdenesCobros pParametro)
        {
            return new CobOrdenesCobrosLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<CobOrdenesCobros> OrdenesCobrosListaFiltro(CobOrdenesCobros pParametro)
        {
            return new CobOrdenesCobrosLN().ObtenerListaFiltro(pParametro);
        }
        public static DataTable OrdenesCobrosListaFiltroDT(CobOrdenesCobros pParametro)
        {
            return new CobOrdenesCobrosLN().ObtenerListaFiltroDT(pParametro);
        }
        public static List<CobOrdenesCobrosDetalles> FacturaObtenerPendientePago(CobOrdenesCobros pParametro)
        {
            return new CobOrdenesCobrosLN().ObtenerPendientePago(pParametro);
        }

        //public static List<CobOrdenesCobros> OrdenesCobrosFacturaListaFiltro(CobOrdenesCobros pParametro)
        //{
        //    return new CobOrdenesCobrosLN().ObtenerFacturaListaFiltro(pParametro);
        //}

        public static DataTable OrdenesCobrosFacturaListaFiltro(CobOrdenesCobros pAfiliado)
        {
            return new CobOrdenesCobrosLN().ObtenerFacturaListaFiltro(pAfiliado);
        }

        public static CobOrdenesCobros OrdenesCobrosObtenerFacturasDatosCompletos(CobOrdenesCobros pParametro)
        {
            return new CobOrdenesCobrosLN().ObtenerDatosFacturaDatosCompletos(pParametro);
        }

        public static List<CobOrdenesCobros> OrdenesCobrosAnticiposPendientesAplicar(CobOrdenesCobros pParametro)
        {
            return new CobOrdenesCobrosLN().AnticiposPendientesAplicar(pParametro);
        }

        public static List<CarCuentasCorrientes> CuentasCorrientesObtenerPorIdOrdenCobro(CobOrdenesCobros pParametro)
        {
            return new CobOrdenesCobrosLN().ObtenerCobradasPorIdOrdenCobro(pParametro);
        }

        /// <summary>
        /// Devuelve los Archivos de Facturas y Remitos Asociados a la Orden de Cobro
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public static List<TGEArchivos> OrdenesCobrosObtenerArchivos(CobOrdenesCobros pParametro)
        {
            return new CobOrdenesCobrosLN().ObtenerArchivos(pParametro);
        }
        public static List<TGEArchivos> OrdenesCobrosObtenerArchivosPorIdFactura(VTAFacturas pParametro)
        {
            return new CobOrdenesCobrosLN().ObtenerArchivos(pParametro);
        }
        public static List<CobOrdenesCobros> OrdenesCobrosObtenerNoImputadas(CobOrdenesCobros pParametro)
        {
            return new CobOrdenesCobrosLN().ObtenerOCFNoImputadas(pParametro);
        }
        public static bool OrdenesCobroArmarMail(CobOrdenesCobros pParametro, MailMessage mail)
        {
            return new CobOrdenesCobrosLN().ArmarMailFactura(pParametro, mail);
        }
   
    }
}
