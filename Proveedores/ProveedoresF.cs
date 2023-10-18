using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Proveedores.Entidades;
using Proveedores.LogicaNegocio;
using System.Data;
using System.Net.Mail;

namespace Proveedores
{
    public class ProveedoresF
    {

        #region CUENTA CORRIENTE

        public static List<CapCuentasCorrientes> CuentasCorrientesSeleccionarPorProveedor(CapCuentasCorrientes pParametro)
        {
            return new CapCuentasCorrientesLN().ObtenerPorProveedor(pParametro);
        }

        public static List<CapCuentasCorrientes> CuentasCorrientesObtenerListaFiltroFechas(CapProveedores pParametro)
        {
            return new CapCuentasCorrientesLN().ObtenerPorFiltroFecha(pParametro);
        }

        public static DataTable CuentasCorrientesObtenerListaFiltroFechasDT(CapProveedores pParametro)
        {
            return new CapCuentasCorrientesLN().ObtenerPorFiltroFechaDT(pParametro);
        }

        #endregion

        #region Proveedores
        public static List<CapProveedores> ProveedoresObtenerListaFiltro(CapProveedores pProveedor)
        {
            return new CapProveedoresLN().ObtenerListaFiltro(pProveedor);
        }

        public static DataTable ProveedoresObtenerListaFiltroDT(CapProveedores pParametro)
        {
            return new CapProveedoresLN().ObtenerListaFiltroDT(pParametro);
        }

        public static List<CapProveedores> ProveedoresObtenerEsVendedor()
        {
            return new CapProveedoresLN().ObtenerEsVendedor();
        }

        public static CapProveedores ProveedoresObtenerDatosCompletos(CapProveedores pProveedor)
        {
            return new CapProveedoresLN().ObtenerDatosCompletos(pProveedor);
        }

        public static bool ProveedoresAgregar(CapProveedores pParametro)
        {
            return new CapProveedoresLN().Agregar(pParametro);
        }

        public static bool ProveedoresModificar(CapProveedores pParametro)
        {
            return new CapProveedoresLN().Modificar(pParametro);
        }

        public static bool ProveedoresArmarMailResumenCuenta(CapProveedores pParametro, MailMessage mail)
        {
            return new CapProveedoresLN().ArmarMailResumenCuenta(pParametro, mail);
        }

        public static bool ProveedoresObtenerDatosAFIP(CapProveedores pProveedor)
        {
            return new CapProveedoresLN().ObtenerDatosAFIP(pProveedor);
        }

        public static CapProveedores ProveedoresObtenerDatosCbu(CapProveedores pParametro)
        {
            return new CapProveedoresLN().ObtenerDatosCbu(pParametro);
        }
        #endregion

        #region Proveedores Porcentajes Comisiones

        public static bool ProveedoresPorcentajesComisionesAgregar(CapProveedoresPorcentajesComisiones pParametro)
        {
            return new CapProveedoresPorcentajesComisionesLN().Agregar(pParametro);
        }

        public static bool ProveedoresPorcentajesComisionesModificar(CapProveedoresPorcentajesComisiones pParametro)
        {
            return new CapProveedoresPorcentajesComisionesLN().Modificar(pParametro);
        }

        public static List<CapProveedoresPorcentajesComisiones> CapProveedoresPorcentajesComisionesObtenerListaFiltro(CapProveedoresPorcentajesComisiones pParametro)
        {
            return new CapProveedoresPorcentajesComisionesLN().ObtenerListaFiltro(pParametro);
        }

        public static List<CapProveedores> CapProveedoresPorcentajesComisionesObtenerProveedores(CapProveedoresPorcentajesComisiones pParametro)
        {
            return new CapProveedoresPorcentajesComisionesLN().ObtenerProveedores(pParametro);
        }
        public static CapProveedoresPorcentajesComisiones ProveedoresPorcentajesComisionesObtenerDatosCompletos(CapProveedoresPorcentajesComisiones pProveedor)
        {
            return new CapProveedoresPorcentajesComisionesLN().ObtenerDatosCompletos(pProveedor);
        }

        #endregion
    }
}
