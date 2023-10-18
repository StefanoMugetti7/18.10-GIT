using Comunes.Entidades;
using Hoteles.Entidades;
using Hoteles.LogicaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Hoteles
{
    public class HotelesF
    {
        #region DESCUENTOS
        public static List<HTLDescuentos> DescuentosObtenerPorReserva(HTLDescuentosFiltros pParametro)
        {
            return new DescuentosLN().ObtenerPorReserva(pParametro);
        }

        public static bool DescuentosAgregar(HTLDescuentos pParametro)
        {
            return new DescuentosLN().Agregar(pParametro);
        }
        public static bool DescuentosModificar(HTLDescuentos pParametro)
        {
            return new DescuentosLN().Modificar(pParametro);
        }
        public static HTLDescuentos DescuentosObtenerDatosCompletos(HTLDescuentos pReservas)
        {
            return new DescuentosLN().ObtenerDatosCompletos(pReservas);
        }
        public static List<HTLDescuentos> ObtenerLista(HTLDescuentos pDescuentos)
        {
            return new DescuentosLN().ObtenerListaFiltro(pDescuentos);
        }
        #endregion

        #region HABITACIONES

        //public static HTLReservasDetalles HabitacionesObtenerHabitacionDetalle(HTLReservasDetalles pParametro)
        //{
        //    return new HabitacionesLN().ObtenerHabitacionDetalle(pParametro);
        //}
        public static DataTable HabitacionesObtenerListaGrilla(HTLHabitaciones pParametro)
        {
            return new HabitacionesLN().ObtenerListaGrilla(pParametro);
        }


        public static List<HTLHabitaciones> HabitacionesObtenerAgenda(HTLHabitaciones pParametro)
        {
            return new HabitacionesLN().ObtenerAgenda(pParametro);
        }

        public static HTLHabitaciones HabitacionesObtenerDatosCompletos(HTLHabitaciones pReservas)
        {
            return new HabitacionesLN().ObtenerDatosCompletos(pReservas);
        }

        public static bool HabitacionesAgregar(HTLHabitaciones pParametro)
        {
            return new HabitacionesLN().Agregar(pParametro);
        }

        public static bool HabitacionesModificar(HTLHabitaciones pParametro)
        {
            return new HabitacionesLN().Modificar(pParametro);
        }

        public static DataTable ProductosObtenerTiposProductosHoteles(Objeto pParametro)
        {
            return new HabitacionesLN().ObtenerTiposProductosHoteles(pParametro);
        }
        public static List<HTLDescuentos> DescuentosObtenerLista(HTLDescuentosFiltros pParametro)
        {
            return new DescuentosLN().ObtenerPorReserva(pParametro);
        }

        /*HTLReservas - Dashboard*/
        public static HTLReservasDetalles HabitacionesSeleccionarHabitacionesDetallesAgenda (HTLReservasDetalles pParametro)
        {
            return new HabitacionesLN().SeleccionarHabitacionesDetallesAgenda(pParametro);
        }

        #endregion

        #region HOTELES
        public static List<HTLHoteles> HotelesObtenerListaActiva(HTLHoteles pParametro)
        {
            return new HotelesLN().ObtenerListaActiva(pParametro);
        }

        public static DataTable HotelesObtenerListaGrilla(HTLHoteles pParametro)
        {
            return new HotelesLN().ObtenerListaGrilla(pParametro);
        }

        public static DataTable HotelesObtenerAgenda(HTLHoteles pParametro)
        {
            //return new HotelesLN().ObtenerAgenda(pParametro);
            return new DataTable();
        }


        public static HTLHoteles HotelesObtenerDatosCompletos(HTLHoteles pHoteles)
        {
            return new HotelesLN().ObtenerDatosCompletos(pHoteles);
        }

        public static bool HotelesAgregar(HTLHoteles pParametro)
        {
            return new HotelesLN().Agregar(pParametro);
        }

        public static bool HotelesModificar(HTLHoteles pParametro)
        {
            return new HotelesLN().Modificar(pParametro);
        }

        #endregion

        #region RESERVAS

        public static DataTable ReservasObtenerListaGrilla(HTLReservas pParametro)
        {
            return new ReservasLN().ObtenerListaGrilla(pParametro);
        }

        public static DataTable ReservasObtenerAgenda(HTLReservas pParametro)
        {
            return new ReservasLN().ObtenerAgenda(pParametro);
        }

        public static DataTable ReservasObtenerProductosPorTipo(HTLReservasDetalles pParametro)
        {
            return new ReservasLN().ObtenerProductosPorTipo(pParametro);
        }

        public static List<T> ReservasDetallesObtenerAjaxComboDetalleGastos<T>(HTLReservasDetalles filtro, List<HTLReservasDetalles> gastosSeleccionados) where T : new()
        {
            return new ReservasLN().ObtenerAjaxComboDetalleGastos<T>(filtro, gastosSeleccionados);
        }

        public static T ReservasDetallesObtenerDetalleGastos<T>(HTLReservasDetalles filtro) where T : new()
        {
            return new ReservasLN().ObtenerDetalleGastos<T>(filtro);
        }

        public static HTLReservas ReservasObtenerDatosCompletos(HTLReservas pReservas)
        {
            return new ReservasLN().ObtenerDatosCompletos(pReservas);
        }

        public static decimal ReservasDetallesObtenerPrecio(HTLReservasDetalles pParametro)
        {
            return new ReservasLN().ObtenerPrecio(pParametro);
        }

        public static bool ReservasAgregar(HTLReservas pParametro)
        {
            return new ReservasLN().Agregar(pParametro);
        }

        public static bool ReservasModificar(HTLReservas pParametro)
        {
            return new ReservasLN().Modificar(pParametro);
        }

        #endregion

        #region LISTAESPERA

        public static bool ListaEsperaAgregar(HTLListaEspera pParametro)
        {
            return new ListaEsperaLN().Agregar(pParametro);
        }

        public static bool ListaEsperaModificar(HTLListaEspera pParametro)
        {
            return new ListaEsperaLN().Modificar(pParametro);
        }

        public static HTLListaEspera ListaEsperaObtenerDatosCompletos(HTLListaEspera pReservas)
        {
            return new ListaEsperaLN().ObtenerDatosCompletos(pReservas);
        }
        public static DataTable ListaEsperaObtenerListaGrilla(HTLListaEspera pParametro)
        {
            return new ListaEsperaLN().ObtenerListaGrilla(pParametro);
        }
        #endregion
    }
}
