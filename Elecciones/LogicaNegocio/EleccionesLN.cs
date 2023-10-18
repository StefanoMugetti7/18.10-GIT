using Auditoria;
using Auditoria.Entidades;
using Comunes;
using Comunes.LogicaNegocio;
using Elecciones.Entidades;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elecciones.LogicaNegocio
{
    public class EleccionesLN : BaseLN<EleElecciones>
    {
        public override bool Agregar(EleElecciones pParametro)
        {
            if (pParametro.IdEleccion > 0)
                return true;

            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.LoteEtapas = pParametro.Etapas.ToDataTable().ToXmlDocument();
                    if (!this.Validaciones(pParametro))
                        return false;

                    if (resultado && !this.Agregar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        foreach (EleEleccionesEtapas item in pParametro.Etapas)
                        {
                            item.IdEleccion = pParametro.IdEleccion;
                            item.UsuarioLogueado = pParametro.UsuarioLogueado;
                            item.IdEleccionEtapa = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "ELEEleccionesEtapasInsertar");
                            if (item.IdEleccionEtapa == 0)
                            {
                                resultado = false;
                                break;
                            }
                        }
                    }

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;
                    //if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                    //    resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdEleccion.ToString());
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

        internal bool Agregar(EleElecciones pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdEleccion = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "ELEEleccionesInsertar");
            if (pParametro.IdEleccion == 0)
                return false;

            return true;
        }

        private bool Validaciones(EleElecciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "ELEEleccionesValidaciones");
        }
        public override bool Modificar(EleElecciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;

            if (!this.Validaciones(pParametro))
                return false;

            EleElecciones valorViejo = new EleElecciones();
            valorViejo.IdEleccion = pParametro.IdEleccion;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {

                    pParametro.LoteEtapas = pParametro.Etapas.ToDataTable().ToXmlDocument();

                    resultado = Validaciones(pParametro);

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;


                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "ELEEleccionesActualizar"))
                        resultado = false;

                    if(resultado)
                    {
                        foreach (EleEleccionesEtapas item in pParametro.Etapas)
                        {
                            item.UsuarioLogueado = pParametro.UsuarioLogueado;
                            item.IdEleccion = pParametro.IdEleccion;
                            if(!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "ELEEleccionesEtapasActualizar"))
                            {
                                resultado = false;
                                break;
                            }
                        }
                    }
                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    //if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                    //    resultado = false;


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

        public override EleElecciones ObtenerDatosCompletos(EleElecciones pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<EleElecciones>("ELEEleccionesSeleccionar", pParametro);
            pParametro.Etapas = BaseDatos.ObtenerBaseDatos().ObtenerLista<EleEleccionesEtapas>("ELEEleccionesEtapasSeleccionarPorIdEleccion", pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }

        public override List<EleElecciones> ObtenerListaFiltro(EleElecciones pParametro)
        {
            throw new NotImplementedException();
        }
        public DataTable ObtenerListaGrilla(EleElecciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("ELEEleccionesSeleccionarDescripcionPorFiltro", pParametro);
        }
        public DataTable ObtenerEtapas(EleElecciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("ELEEleccionesObtenerEtapas", pParametro);
        }
        public DataTable ObtenerResultadosVotacion(EleListasElecciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("ELEEleccionesObtenerResultadosVotaciones", pParametro);
        }

        public EleElecciones ObtenerEleccionVigente(EleElecciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<EleElecciones>("ELEEleccionesObtenerEleccionVigente", pParametro);
        }
    }
}
