using Comunes;
using Comunes.LogicaNegocio;
using Medicina.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Medicina.LogicaNegocio
{
    class MedNomencladoresLN : BaseLN<MedNomencladores>
    {
        public override bool Agregar(MedNomencladores pParametro)
        {
            if (pParametro.IdNomenclador > 0)
                return false;

            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            if (!this.Validaciones(pParametro))
                return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {

                    if (resultado && !this.Agregar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdNomenclador.ToString());
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }

            }
            return resultado;
        }
        internal bool Agregar(MedNomencladores pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdNomenclador = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "MedNomencladorInsertar");
            if (pParametro.IdNomenclador == 0)
                return false;

            return true;
        }
        private bool Validaciones(MedNomencladores pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "MedNomencladorValidaciones");
        }
        public override bool Modificar(MedNomencladores pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;

            if (!this.Validaciones(pParametro))
                return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "MedNomencladorActualizar"))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }
        public override MedNomencladores ObtenerDatosCompletos(MedNomencladores pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<MedNomencladores>("[MedNomencladorSeleccionar]", pParametro);
        }

        public override List<MedNomencladores> ObtenerListaFiltro(MedNomencladores pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<MedNomencladores>("MedNomencladoresSeleccionarFiltro", pParametro);
        }

        public DataTable ObtenerGrilla(MedNomencladores pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("MedNomencladoresSeleccionarFiltroDT", pParametro);
        }

        public List<MedNomencladores> ObtenerEspecializaciones(MedNomencladores pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<MedNomencladores>("MedNomencladoresObtenerEspecializaciones", pParametro);
        }

        public List<MedNomencladores> ObtenerListaCombo(MedNomencladores pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<MedNomencladores>("MedNomencladoresObtenerListaCombo", pParametro);
        }
    }
}
