using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compras.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Auditoria.Entidades;
using Auditoria;
using Generales.FachadaNegocio;

namespace Compras.LogicaNegocio
{
    class CMPFamiliasLN : BaseLN<CMPFamilias>
    {
        public override bool Agregar(CMPFamilias pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            //if (!this.Validar(pParametro))
            //    return false;

            pParametro.FechaAlta = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CMPFamiliasValidaciones"))
                    {
                        resultado = false;
                    }

                    if (resultado)
                    {
                        pParametro.IdFamilia = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CMPFamiliasInsertar");
                        if (pParametro.IdFamilia == 0)
                        {
                            resultado = false;
                        }
                    }

                    //if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    //if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                    //    resultado = false;
                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
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

        public override bool Modificar(CMPFamilias pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CMPFamilias valorViejo = new CMPFamilias();
            valorViejo.IdFamilia = pParametro.IdFamilia;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            //if (!this.Validar(pParametro))
            //    return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CMPFamiliasValidaciones"))
                        resultado = false;

                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CMPFamiliasActualizar"))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    //if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    //if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                    //    resultado = false;
                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
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

        public override CMPFamilias ObtenerDatosCompletos(CMPFamilias pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CMPFamilias>("CMPFamiliasSeleccionar", pParametro);
        }

        public override List<CMPFamilias> ObtenerListaFiltro(CMPFamilias pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CMPFamilias>("CMPFamiliasSeleccionarFiltros", pParametro);
        }
    }
}
