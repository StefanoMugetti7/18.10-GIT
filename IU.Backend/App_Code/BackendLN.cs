using Comunes.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace IU.Backend.App_Code
{
    public class BackendLN
    {
        public bool ActualizarBaseDatos(GRPGruposEmpresas pParametro)
        {
            //AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            //BaseDatos datos = BaseDatos.ObtenerBaseDatos();
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            Database bd = factory.CreateDefault();
            bool resultado = true;

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();
                try
                {
                    if (!BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pParametro, bd, tran, "BackEndAcutalizarStoredProcedureDesdeStartUP"))
                    {
                        resultado = false;
                    }
                    if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pParametro, bd, tran, string.Concat(pParametro.BaseDatos, ".dbo.IMPLEAcutalizarDatosPrincipalesDesdeStartUP")))
                    {
                        resultado = false;
                    }

                    if (resultado)
                    {
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                        tran.Commit();
                    }
                    else
                    {
                        pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                        tran.Rollback();
                    }
                    return resultado;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    return false;
                }
            }
        }

        public List<GRPGruposEmpresas> EmpresasListar()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<GRPGruposEmpresas>("GRPGruposEmpresasListarActivas");
        }

        public List<GRPGruposEmpresas> EmpresasSeleccionar(GRPGruposEmpresas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<GRPGruposEmpresas>("GRPGruposEmpresasSeleccionarFiltro", pParametro);
        }
    }
}