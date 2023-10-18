using Mailing.LogicaNegocio;
using Mailing.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml;
using Generales.Entidades;

namespace Mailing
{
    public class MailingF
    {
        public static TGEMailing TGEMailingObtenerDatosCompletos(TGEMailing pParametro)
        {
            return new TGEMailingLN().ObtenerDatosCompletos(pParametro);
        }

        public static bool TGEMailingAgregarMailingProcesamiento(TGEMailing pParametro)
        {
            return new TGEMailingLN().MailingProcesamientosInsertar(pParametro);
        }

        public static TGEMailing TGEMailingObtenerDatosCompletosFiltro(TGEMailing pParametro)
        {
            return new TGEMailingLN().ObtenerDatosCompletosFiltro(pParametro);
        }

        public static DataTable TGEMailingObtenerDatosCompletosFiltroDT(TGEMailing pParametro)
        {
            return new TGEMailingLN().ObtenerDatosCompletosFiltroDT(pParametro);
        }
        public static TGEMailingProcesamientosPlantillas PlantillasObtenerDatosCompletos(TGEMailingProcesamientosPlantillas pParametro)
        {
            return new TGEMailingLN().PlatillaObtenerDatosCompletos(pParametro);
        }
        public static List<TGEMailingProcesamientosPlantillas> PlantillasObtenerLista(TGEPlantillas plantillas)
        {
            return new TGEMailingLN().PlantillasObtenerLista(plantillas);
        }
        public static bool TGEMailingAgregar(TGEMailing pParametro)
        {
            return new TGEMailingLN().Agregar(pParametro);
        }

        public static bool TGEMailingModificar(TGEMailing pParametro)
        {
            return new TGEMailingLN().Modificar(pParametro);
        }

        public static List<TGEMailingProcesos> TGEMailingObtenerListaMailingProceso()
        {
            return new TGEMailingLN().ObtenerListaTGEMailingProceso();
        }
        public static TGEMailing TGEMailingProcesosObtenerPlantilla(TGEMailing pParametro)
        {
            return new TGEMailingLN().ObtenerPlantillaMailingProceso(pParametro);
        }

        public static List<TGEMailing> TGEMailingSeleccionarGrilla()
        {
            return new TGEMailingLN().SeleccionarGrilla();
        }
        //public static List<TGEMailing> TGEMailingObtenerGenerarDatos()
        //{
        //    return new TGEMailingLN().ObtenerGenerarDatos();
        //}

        public static XmlDocument TGEMailingGenerarDatosEnvios(string idMailing, string key, string value)
        {
            return new TGEMailingLN().GenerarDatosEnvios(idMailing, key, value);
        }
        public static XmlDocument TGEMailingGenerarDatosEnviosV2(string idMailing, string idMailingProcesamiento,string txtAsunto ,string key, string value)
        {
            return new TGEMailingLN().GenerarDatosEnviosV2(idMailing, idMailingProcesamiento, txtAsunto, key, value);
        }
        public static bool TGEMailingEnviarMails(TGEMailing pParametro)
        {
            return new TGEMailingLN().EnviarMails(pParametro);
        }
        public static bool TGEMailingEnviarMailsV2(TGEMailing pParametro)
        {
            return new TGEMailingLN().EnviarMailsV2(pParametro);
        }

        public static bool TGEMailingPruebaEnvio(TGEMailing pParametro)
        {
            return new TGEMailingLN().PruebaEnvio(pParametro);
        }

        public static bool TGEMailingEnviarMailsActualizar(TGEMailing pParametro)
        {
            return new TGEMailingLN().EnviarMailsActualizar(pParametro);
        }


        public static TGEMailingProcesamientosPlantillas PlantillasObtenerDatosCompletosPorCodigo(TGEMailingProcesamientosPlantillas pParametro)
        {
            return new TGEMailingLN().ObtenerDatosCompletosPorCodigo(pParametro);
        }

        public static DataTable TGEMailingObtenerMailsAEnviar(TGEMailing pParametro)
        {
            return new TGEMailingLN().ObtenerMailsAEnviar(pParametro);
        }

        public static bool TGEMailingEnviarMailsSeleccionados(DataTable e, TGEMailing pParametro)
        {
            return new TGEMailingLN().EnviarMailsSeleccionados(e, pParametro);
        }

        public static AudMailsEnvios MailsEnviosObtenerMensaje(AudMailsEnvios pParametro)
        {
            return new TGEMailingLN().ObtenerMensaje(pParametro);
        }

        public static bool PlantillasModificar(TGEMailingProcesamientosPlantillas pParametro)
        {
            return new TGEMailingLN().PlantillaModificar(pParametro);
        }


        public static bool PlantillasAgregar(TGEMailingProcesamientosPlantillas pParametro)
        {
            return new TGEMailingLN().PlantillaAgregar(pParametro);
        }

        public static List<TGEMailingProcesamientosPlantillas> PlantillasObtenerListaFiltro(TGEMailingProcesamientosPlantillas pParametro)
        {
            return new TGEMailingLN().ObtenerListaFiltroPlantillas(pParametro);
        }

        public static DataSet ProcesosObtenerDatosParametro(TGEMailingProcesos pProceso)
        {
            return new TGEMailingLN().ObtenerDatosParametro(pProceso);
        }

        public static TGEMailingProcesos MailingProcesosObtenerDatosCompletos(TGEMailing pParametro)
        {
            return new TGEMailingLN().ParametrosObtenerDatosCompletos(pParametro);
        }
    }
}
