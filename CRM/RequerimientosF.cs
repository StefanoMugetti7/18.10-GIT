using CRM.Entidades;
using CRM.LogicaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CRM
{
    public class RequerimientosF
    {
        public static CRMRequerimientos RequerimientosObtenerDatosCompletos(CRMRequerimientos pPuntoVenta)
        {
            return new RequerimientosLN().ObtenerDatosCompletos(pPuntoVenta);
        }
        public static bool RequerimientosAgregar(CRMRequerimientos pParametro)
        {
            return new RequerimientosLN().Agregar(pParametro);
        }

        public static bool RequerimientosModificar(CRMRequerimientos pParametro)
        {
            return new RequerimientosLN().Modificar(pParametro);
        }

        public static DataTable RequerimientosObtenerListaGrilla(CRMRequerimientos pParametro)
        {
            return new RequerimientosLN().ObtenerListaGrilla(pParametro);
        }
        public static List<CRMRequerimientos> RequerimientosObtenerAcciones()
        {
            return new RequerimientosLN().ObtenerAcciones();
        }
        public static List<CRMRequerimientos> RequerimientosObtenerCategorias()
        {
            return new RequerimientosLN().ObtenerCategorias();
        }
        public static List<CRMRequerimientos> RequerimientosObtenerPrioridades()
        {
            return new RequerimientosLN().ObtenerPrioridades();
        } 
        public static List<CRMRequerimientos> RequerimientosObtenerTiposRequerimientos()
        {
            return new RequerimientosLN().ObtenerTiposRequerimientos();
        }
        public static List<CRMRequerimientos> RequerimientosObtenerEntidadesOrigen()
        {
            return new RequerimientosLN().ObtenerEntidadesOrigen();
        }
        public static List<CRMRequerimientos> RequerimientosObtenerEntidadesDestino()
        {
            return new RequerimientosLN().ObtenerEntidadesDestino();
        }
        public static List<CRMRequerimientos> RequerimientosObtenerOrigenSolicitud()
        {
            return new RequerimientosLN().ObtenerOrigenSolicitud();
        }
        public static DataTable RequerimientosCargarCardsBootStrap(CRMRequerimientos pParametro)
        {
            return new RequerimientosLN().ObtenerCardsBootStrap(pParametro);
        }
        public static bool RequerimientosAgregarSeguimiento(CRMSeguimientos pParametro)
        {
            return new RequerimientosLN().AgregarSeguimiento(pParametro);
        }
        public static bool RequerimientosAgregarSolucion(CRMSeguimientos pParametro)
        {
            return new RequerimientosLN().AgregarSolucion(pParametro);
        }
        public static List<CRMRequerimientos> RequerimientosObtenerTecnicos()
        {
            return new RequerimientosLN().ObtenerTecnicos();
        }
        public static DataTable RequerimientosCargarCardsBootStrapListar(CRMRequerimientos pParametro)
        {
            return new RequerimientosLN().ObtenerCardsBootStrapListar(pParametro);
        }
    }
}
