using LavaYa.Entidades;
using LavaYa.LogicaNegocio;
using System.Collections.Generic;
using System.Data;

namespace LavaYa
{
    public class EdificiosF
    {
        public static LavEdificios EdificiosObtenerDatosCompletos(LavEdificios pPuntoVenta)
        {
            return new EdificiosLN().ObtenerDatosCompletos(pPuntoVenta);
        }
        public static bool EdificiosAgregar(LavEdificios pParametro)
        {
            return new EdificiosLN().Agregar(pParametro);
        }

        public static bool EdificiosModificar(LavEdificios pParametro)
        {
            return new EdificiosLN().Modificar(pParametro);
        }

        public static DataTable EdificiosObtenerListaGrilla(LavEdificios pParametro)
        {
            return new EdificiosLN().ObtenerListaGrilla(pParametro);
        }    
        public static DataTable EdificiosObtenerListaGrillaRequerimientos(LavEdificios pParametro)
        {
            return new EdificiosLN().ObtenerListaGrillaRequerimientos(pParametro);
        }
        public static DataTable EdificiosObtenerListaGrillaPuntosVentas(LavEdificios pParametro)
        {
            return new EdificiosLN().ObtenerListaGrillaPuntosVentas(pParametro);
        }

        public static List<LavEdificios> EdificiosObtenerOpcionContrato()
        {
            return new EdificiosLN().ObtenerOpcionesContrato();
        } 
        public static List<LavEdificios> EdificiosObtenerOpcionSistemaPago()
        {
            return new EdificiosLN().ObtenerOpcionesSistemaPago();
        }
        public static List<LavEdificios> EdificiosObtenerOpcionHorario()
        {
            return new EdificiosLN().ObtenerOpcionesHorario();
        }
        public static List<LavMaquinas> EdificiosObtenerMaquinasCargadas(LavEdificios pParametro)
        {
            return new EdificiosLN().ObtenerMaquinasCargadas(pParametro);
        }
    }
}
