using Auditoria;
using Auditoria.Entidades;
using Comunes;
using Comunes.Entidades;
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
using Database = Microsoft.Practices.EnterpriseLibrary.Data.Database;
using EnumElecciones = Elecciones.Entidades.EnumElecciones;

namespace Elecciones.LogicaNegocio
{
    public class ListasEleccionesLN : BaseLN<EleListasElecciones>
    {
        public override bool Agregar(EleListasElecciones pParametro)
        {

            if (pParametro.IdListaEleccion > 0)
                return true;

            bool resultado = true;
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);


            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {

                    pParametro.LoteApoderados = pParametro.Apoderados.ToDataTable().ToXmlDocument();
                    pParametro.LotePostulantes = pParametro.Postulantes.ToDataTable().ToXmlDocument();
                    pParametro.LoteAvales = pParametro.Avales.ToDataTable().ToXmlDocument();


                    if (!this.Validaciones(pParametro))
                        return false;



                    if (resultado && !this.Agregar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        foreach (EleListasEleccionesPostulantes item in pParametro.Postulantes)
                        {
                            if (item.EstadoColeccion == EstadoColecciones.Agregado || item.IdAfiliado < 0)
                            {
                                item.IdListaEleccion = pParametro.IdListaEleccion;
                                item.UsuarioLogueado = pParametro.UsuarioLogueado;
                                item.IdListaEleccionPostulante = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "ELEListasEleccionesPostulantesInsertar");
                                if (item.IdListaEleccionPostulante == 0)
                                {
                                    return false;
                                }
                            }
                        }
                        foreach (EleListasEleccionesApoderados item in pParametro.Apoderados)
                        {
                            if (item.EstadoColeccion == EstadoColecciones.Agregado)
                            {
                                item.IdListaEleccion = pParametro.IdListaEleccion;
                                item.UsuarioLogueado = pParametro.UsuarioLogueado;
                                item.IdListaEleccionApoderado = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "ELEListasEleccionesApoderadosInsertar");
                                if (item.IdListaEleccionApoderado == 0)
                                {
                                    return false;
                                }
                            }
                        }
                        foreach (EleListasEleccionesAvales item in pParametro.Avales)
                        {
                            if (item.EstadoColeccion == EstadoColecciones.Agregado)
                            {
                                item.IdListaEleccion = pParametro.IdListaEleccion;
                                item.UsuarioLogueado = pParametro.UsuarioLogueado;
                                item.IdListaEleccionAval = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "ELEListasEleccionesAvalesInsertar");
                                if (item.IdListaEleccionAval == 0)
                                {
                                    return false;
                                }
                            }
                        }
                    }

                    //if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                    //    resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdListaEleccion.ToString());
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

        internal bool Agregar(EleListasElecciones pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdListaEleccion = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "ELEListasEleccionesInsertar");
            if (pParametro.IdListaEleccion == 0)
                return false;

            return true;
        }
        private bool Validaciones(EleListasElecciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "ELEListasEleccionesValidaciones");
        }
        public override bool Modificar(EleListasElecciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            EleListasElecciones valorViejo = new EleListasElecciones();
            valorViejo.IdListaEleccion = pParametro.IdListaEleccion;
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
                    pParametro.LoteApoderados = pParametro.Apoderados.ToDataTable().ToXmlDocument();
                    pParametro.LotePostulantes = pParametro.Postulantes.ToDataTable().ToXmlDocument();
                    pParametro.LoteAvales = pParametro.Avales.ToDataTable().ToXmlDocument();
                    pParametro.LoteComentarios = pParametro.Comentarios.ToDataTable().ToXmlDocument();
                    resultado = Validaciones(pParametro);
                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "ELEListasEleccionesActualizar"))
                        resultado = false;

                    if (resultado && !this.ActualizarItems(pParametro, bd, tran, valorViejo))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
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

        private List<EleListasEleccionesAvales> ObtenerBorradosAvales(List<EleListasEleccionesAvales> newList, List<EleListasEleccionesAvales> oldList)
        {
            List<EleListasEleccionesAvales> retorno = new List<EleListasEleccionesAvales>();

            foreach (EleListasEleccionesAvales item in oldList)
            {
                EleListasEleccionesAvales aux = newList.Find(x => x.IdAfiliado == item.IdAfiliado);
                if (aux == null)
                {
                    retorno.Add(item);
                }
            }
            return retorno;
        }
        private List<EleListasEleccionesApoderados> ObtenerBorradosApoderados(List<EleListasEleccionesApoderados> newList, List<EleListasEleccionesApoderados> oldList)
        {
            List<EleListasEleccionesApoderados> retorno = new List<EleListasEleccionesApoderados>();

            foreach (EleListasEleccionesApoderados item in oldList)
            {
                EleListasEleccionesApoderados aux = newList.Find(x => x.IdAfiliado == item.IdAfiliado);
                if (aux == null)
                {
                    retorno.Add(item);
                }
            }
            return retorno;
        }
        private bool ActualizarItems(EleListasElecciones pParametro, Database bd, DbTransaction tran, EleListasElecciones valorViejo)
        {
            bool resultado = true;
            #region POSTULANTES


            if (pParametro.Apoderados.Count == 0 && valorViejo.Apoderados.Count > 0)
            {
                if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "ELEListasEleccionesLiberarApoderados"))
                {
                    return false;
                }
            }
            if (pParametro.Avales.Count == 0 && valorViejo.Avales.Count > 0)
            {
                if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "ELEListasEleccionesLiberarAvales"))
                {
                    return false;
                }
            }


            foreach (EleListasEleccionesPostulantes item in pParametro.Postulantes)
            {
                item.IdListaEleccion = pParametro.IdListaEleccion;
                item.UsuarioLogueado = pParametro.UsuarioLogueado;
                if (item.EstadoColeccion == EstadoColecciones.Modificado)
                {
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "ELEListasEleccionesPostulantesActualizar"))
                    {
                        return false;
                    }
                }
            }
            #endregion
            #region AVALES

            List<EleListasEleccionesAvales> avalesEliminados = this.ObtenerBorradosAvales(pParametro.Avales, valorViejo.Avales);
            foreach (EleListasEleccionesAvales item in avalesEliminados)
            {
                item.IdListaEleccion = pParametro.IdListaEleccion;
                item.UsuarioLogueado = pParametro.UsuarioLogueado;
                if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "ELEListasEleccionesAvalesBaja"))
                {
                    return false;
                }
            }
            foreach (EleListasEleccionesAvales item in pParametro.Avales)
            {
                item.IdListaEleccion = pParametro.IdListaEleccion;
                item.UsuarioLogueado = pParametro.UsuarioLogueado;
                if (item.EstadoColeccion == EstadoColecciones.Agregado)
                {
                    item.Estado.IdEstado = (int)Estados.Activo;
                    item.IdListaEleccionAval = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "ELEListasEleccionesAvalesInsertar");
                    if (!(item.IdListaEleccionAval > 0))
                    {
                        return false;
                    }
                }
                else if (item.EstadoColeccion == EstadoColecciones.Modificado)
                {
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "ELEListasEleccionesAvalesActualizar"))
                    {
                        return false;
                    }
                }
            }

            #endregion
            #region APODERADOS
            List<EleListasEleccionesApoderados> apoderadosEliminados = this.ObtenerBorradosApoderados(pParametro.Apoderados, valorViejo.Apoderados);
            foreach (EleListasEleccionesApoderados item in apoderadosEliminados)
            {
                item.IdListaEleccion = pParametro.IdListaEleccion;
                item.UsuarioLogueado = pParametro.UsuarioLogueado;
                if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "ELEListasEleccionesApoderadosBaja"))
                {
                    return false;
                }
            }
            foreach (EleListasEleccionesApoderados item in pParametro.Apoderados)
            {
                item.IdListaEleccion = pParametro.IdListaEleccion;
                item.UsuarioLogueado = pParametro.UsuarioLogueado;
                if (item.EstadoColeccion == EstadoColecciones.Agregado)
                {
                    item.Estado.IdEstado = (int)Estados.Activo;
                    item.IdListaEleccionApoderado = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "ELEListasEleccionesApoderadosInsertar");
                    if (!(item.IdListaEleccionApoderado > 0))
                    {
                        return false;
                    }
                }
                if (item.EstadoColeccion == EstadoColecciones.Modificado)
                {
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "ELEListasEleccionesApoderadosActualizar"))
                    {
                        return false;
                    }
                }
            }
            #endregion
            return resultado;
        }
        public override EleListasElecciones ObtenerDatosCompletos(EleListasElecciones pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<EleListasElecciones>("ELEListasEleccionesSeleccionar", pParametro);
            pParametro.Postulantes = BaseDatos.ObtenerBaseDatos().ObtenerLista<EleListasEleccionesPostulantes>("ELEListasEleccionesPostulantesSeleccionarPorIdListaEleccion", pParametro);
            pParametro.Apoderados = BaseDatos.ObtenerBaseDatos().ObtenerLista<EleListasEleccionesApoderados>("ELEListasEleccionesApoderadosSeleccionarPorIdListaEleccion", pParametro);
            pParametro.Avales = BaseDatos.ObtenerBaseDatos().ObtenerLista<EleListasEleccionesAvales>("ELEListasEleccionesAvalesSeleccionarPorIdListaEleccion", pParametro);
            pParametro.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }

        public override List<EleListasElecciones> ObtenerListaFiltro(EleListasElecciones pParametro)
        {
            throw new NotImplementedException();
        }

        public List<EleElecciones> ObtenerElecciones()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<EleElecciones>("ELEListasEleccionesObtenerElecciones", new EleElecciones());
        }

        public DataTable ObtenerListaGrilla(EleListasElecciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("ELEListasEleccionesSeleccionarDescripcionPorFiltro", pParametro);
        }

        public bool Autorizar(EleListasElecciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;

            if (!this.Validaciones(pParametro))
                return false;

            EleListasElecciones valorViejo = new EleListasElecciones();
            valorViejo.IdListaEleccion = pParametro.IdListaEleccion;
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
                    pParametro.LoteApoderados = pParametro.Apoderados.ToDataTable().ToXmlDocument();
                    pParametro.LotePostulantes = pParametro.Postulantes.ToDataTable().ToXmlDocument();
                    pParametro.LoteAvales = pParametro.Avales.ToDataTable().ToXmlDocument();
                    pParametro.LoteComentarios = pParametro.Comentarios.ToDataTable().ToXmlDocument();

                    resultado = Validaciones(pParametro);
                    pParametro.Estado.IdEstado = (int)EnumElecciones.Autorizado;
                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "ELEListasEleccionesActualizar"))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
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
        public DataTable ObtenerRegiones(EleListasElecciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("ELEListasEleccionesObtenerRegiones", pParametro);
        }
        public DataTable ObtenerResultadosVotacion(EleListasElecciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("ELEEleccionesObtenerResultadosVotaciones", pParametro);
        }
        public DataTable ObtenerListaGrillaNacionalesDT(EleListasElecciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("ELEListasEleccionesObtenerNacionales", pParametro);
        }
        public DataTable ObtenerListaGrillaRegionalesDT(EleListasElecciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("ELEListasEleccionesObtenerRegionales", pParametro);
        }
        public DataTable ObtenerListaGrillaRepresentantesDT(EleListasElecciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("ELEListasEleccionesObtenerRepresentantes", pParametro);
        }
        public List<EleListasElecciones> ObtenerListaGrillaNacionales(EleListasElecciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<EleListasElecciones>("ELEListasEleccionesObtenerNacionalesLista", pParametro);
        }
        public List<EleListasElecciones> ObtenerListaGrillaRegionales(EleListasElecciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<EleListasElecciones>("ELEListasEleccionesObtenerRegionales", pParametro);
        }
        public List<EleListasElecciones> ObtenerListaGrillaRepresentantes(EleListasElecciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<EleListasElecciones>("ELEListasEleccionesObtenerRepresentantes", pParametro);
        }
        public List<EleListasElecciones> ObtenerListasEleccionesRegionalesSinRepresentantes(EleListasElecciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<EleListasElecciones>("ELEListasEleccionesObtenerListasRegionalesNoAsociadas", pParametro);
        }

        public DataTable ObtenerRepresentantesPopUp(EleListasElecciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("ELEListasEleccionesObtenerRepresentantesPopUp", pParametro);
        }
    }
}
