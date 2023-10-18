using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Medicina.Entidades;
using Medicina.LogicaNegocio;

namespace Medicina
{
    public class MedicinaF
    {
        #region Historias Clinicas

        public static bool HistoriasClinicasValidarExiste(MedHistoriasClinicas pParametro)
        {
            return new MedHistoriasClinicasLN().ValidarExiste(pParametro);
        }

        public static bool HistoriasClinicasAgregar(MedHistoriasClinicas pParametro)
        {
            return new MedHistoriasClinicasLN().Agregar(pParametro);
        }

        public static bool HistoriasClinicasModificar(MedHistoriasClinicas pParametro)
        {
            return new MedHistoriasClinicasLN().Modificar(pParametro);
        }

        public static MedHistoriasClinicas HistoriasClinicasObtenerDatosCompletos(MedHistoriasClinicas pParametro)
        {
            return new MedHistoriasClinicasLN().ObtenerDatosCompletos(pParametro);
        }


        #endregion

        #region Nomencladores

        public static MedNomencladores NomencladoresObtenerDatosCompletos(MedNomencladores pParametro)
        {
            return new MedNomencladoresLN().ObtenerDatosCompletos(pParametro);
        }
        public static List<MedNomencladores> NomencladoresObtenerListaFiltro(MedNomencladores pParametro)
        {
            return new MedNomencladoresLN().ObtenerListaFiltro(pParametro);
        }
        public static DataTable NomencladoresObtenerGrilla(MedNomencladores pParametro)
        {
            return new MedNomencladoresLN().ObtenerGrilla(pParametro);
        }
        public static bool NomencladoresAgregar(MedNomencladores pParametro)
        {
            return new MedNomencladoresLN().Agregar(pParametro);
        }

        public static bool NomencladoresModificar(MedNomencladores pParametro)
        {
            return new MedNomencladoresLN().Modificar(pParametro);
        }
        public static List<MedNomencladores> NomencladoresObtenerEspecializaciones(MedNomencladores pParametro)
        {
            return new MedNomencladoresLN().ObtenerEspecializaciones(pParametro);
        }
        public static List<MedNomencladores> NomencladoresObtenerListaCombo(MedNomencladores pParametro)
        {
            return new MedNomencladoresLN().ObtenerListaCombo(pParametro);
        }
        #endregion

        #region Prestaciones
        public static bool PrestacionesAgregar(MedPrestaciones pParametro)
        {
            return new MedPrestacionesLN().Agregar(pParametro);
        }

        public static bool PrestacionesModificar(MedPrestaciones pParametro)
        {
            return new MedPrestacionesLN().Modificar(pParametro);
        }

        public static MedPrestaciones PrestacionesObtenerDatosCompletos(MedPrestaciones pParametro)
        {
            return new MedPrestacionesLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<MedPrestaciones> PrestacionesObtenerListaFiltro(MedPrestaciones pParametro)
        {
            return new MedPrestacionesLN().ObtenerListaFiltro(pParametro);
        }

        public static List<MedPrestaciones> PrestacionesObtenerListaFiltroPacientes(MedPrestaciones pParametro)
        {
            return new MedPrestacionesLN().ObtenerListaFiltroPacientes(pParametro);
        }
        #endregion

        #region ESTUDIOS

        public static bool EstudiosAgregar(MedEstudios pParametro)
        {
            return new MedEstudiosLN().Agregar(pParametro);
        }

        public static bool EstudiosModificar(MedEstudios pParametro)
        {
            return new MedEstudiosLN().Modificar(pParametro);
        }
        public static bool EstudiosAnular(MedEstudios pParametro)
        {
            return new MedEstudiosLN().Anular(pParametro);
        }

        public static MedEstudios EstudiosObtenerDatosCompletos(MedEstudios pParametro)
        {
            return new MedEstudiosLN().ObtenerDatosCompletos(pParametro);
        }

        public static List<MedEstudios> EstudiosObtenerListaFiltro(MedEstudios pParametro)
        {
            return new MedEstudiosLN().ObtenerListaFiltro(pParametro);
        }

        public static DataSet EstudiosObtenerDataSetPdf(MedEstudios pParametro)
        {
            return new MedEstudiosLN().ObtenerDataSetPdf(pParametro);
        }

        public static string EstudiosObtenerPlantilla(MedEstudios pParametro)
        {
            return new MedEstudiosLN().ObtenerPlantilla(pParametro);
        }

        public static byte[] EstudiosObtenerComprobante(MedEstudios pParametro)
        {
            return new MedEstudiosLN().ObtenerComprobante(pParametro);
        }

        #endregion

        #region Prestadores

        public static MedPrestadores PrestadoresObtenerDatosCompletos(MedPrestadores pParametro)
        {
            return new MedPrestadoresLN().ObtenerDatosCompletos(pParametro);
        }
        public static List<MedPrestadores> PrestadoresObtenerListaFiltro(MedPrestadores pParametro)
        {
            return new MedPrestadoresLN().ObtenerListaFiltro(pParametro);
        }
        
        public static bool PrestadoresAgregar(MedPrestadores pParametro)
        {
            return new MedPrestadoresLN().Agregar(pParametro);
        }

        public static bool PrestadoresModificar(MedPrestadores pParametro)
        {
            return new MedPrestadoresLN().Modificar(pParametro);
        }
        
        #endregion

        #region Turneras
        public static List<MedTurneras> TurnerasObtenerTurneras(MedTurneras pTurnera, int pCantidad)
        {
            return new MedTurnerasLN().ObtenerTurneras(pTurnera, pCantidad);
        }

        public static bool TurnerasAgregarActualizar(MedTurneras pTurnera)
        {
            return new MedTurnerasLN().AgregarActualizar(pTurnera);
        }

        #endregion

        #region Turnos
        public static List<MedTurnos> TurnosObtenerListaFiltro(MedTurnos pParametro)
        {
            return new MedTurnerasLN().ObtenerTurnos(pParametro);
        }
        public static List<MedTurnos> TurnosObtenerListaFiltroPacientes(MedTurnos pParametro)
        {
            return new MedTurnerasLN().ObtenerTurnosPacientes(pParametro);
        }
        public static bool TurnosValidarObtenerTurnos(MedTurnos pParametro)
        {
            return new MedTurnerasLN().ValidarObtenerTurnos(pParametro);
        }

  



        #endregion
    }
}
