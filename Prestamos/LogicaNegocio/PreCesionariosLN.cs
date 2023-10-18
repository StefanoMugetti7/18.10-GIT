using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prestamos.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Xml;

namespace Prestamos.LogicaNegocio
{
    public class PreCesionariosLN : BaseLN<PreCesionarios>
    {
        public override PreCesionarios ObtenerDatosCompletos(PreCesionarios pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<PreCesionarios>("PreCesionariosSeleccionar", pParametro);
        }

        public override List<PreCesionarios> ObtenerListaFiltro(PreCesionarios pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<PreCesionarios>("PreCesionariosSeleccionarFiltro", pParametro);
        }

        public override bool Agregar(PreCesionarios pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.Estado.IdEstado = (int)Estados.Activo;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    //Guardo el Prestamo
                    pParametro.IdCesionario = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "PreCesionariosInsertar");
                    if (pParametro.IdCesionario == 0)
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

        public override bool Modificar(PreCesionarios pParametro)
        {
            throw new NotImplementedException();
        }
    }
}
