using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Expedientes.Entidades;
using Servicio.AccesoDatos;
using Comunes.LogicaNegocio;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Comunes.Entidades;
using Auditoria;
using Auditoria.Entidades;
using Generales.Entidades;

namespace Expedientes.LogicaNegocio
{
    class ExpExpedientesLN : BaseLN<ExpExpedientes>
    {
        public override ExpExpedientes ObtenerDatosCompletos(ExpExpedientes pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<ExpExpedientes>("ExpExpedientesSeleccionar", pParametro);
            pParametro.ExpedientesTracking = BaseDatos.ObtenerBaseDatos().ObtenerLista<ExpExpedientesTracking>("ExpExpedientesTrackingSeleccionarPorExpediente", pParametro);
            pParametro.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }

        public override List<ExpExpedientes> ObtenerListaFiltro(ExpExpedientes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<ExpExpedientes>("ExpExpedientesSeleccionarFiltros", pParametro);
        }

        public bool ValidarExpedientesPendientes(Usuarios pUsuario)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pUsuario, "ExpExpedientesValidarPendientes");
        }

        public bool Agregar(ExpExpedientes pParametro, TGESectores pSector)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;

            if (!this.Validar(pParametro))
                return false;

            List<ExpExpedientes> lista = new List<ExpExpedientes>();
            ExpExpedientes expte;
            ExpExpedientesTracking tracking;
            if (pParametro.ExpedientesTracking.Count > 0)
            {                
                foreach (ExpExpedientesTracking derivacionesAltas in pParametro.ExpedientesTracking)
                {
                    expte = new ExpExpedientes();
                    AyudaProgramacionLN.MatchObjectProperties(pParametro, expte);
                    expte.Comentarios = new List<TGEComentarios>(pParametro.Comentarios);
                    expte.Archivos = new List<TGEArchivos>(pParametro.Archivos);
                    expte.ExpedientesTracking = new List<ExpExpedientesTracking>();
                    //Primer Track para Log
                    tracking = new ExpExpedientesTracking();
                    tracking.Fecha = DateTime.Now;
                    tracking.Sector = pSector;
                    tracking.Sector.Filial = pSector.Filial;
                    tracking.Estado.IdEstado = (int)EstadosExpedientesTracking.Activo;
                    tracking.EstadoColeccion = EstadoColecciones.Agregado;
                    expte.ExpedientesTracking.Add(tracking);

                    //Agrego la Derivación hecha en el alta
                    tracking = new ExpExpedientesTracking();
                    AyudaProgramacionLN.MatchObjectProperties(derivacionesAltas, tracking);
                    tracking.EstadoColeccion = EstadoColecciones.Agregado;
                    tracking.Fecha = DateTime.Now;
                    tracking.Estado.IdEstado = (int)EstadosExpedientesTracking.Derivado;
                    expte.ExpedientesTracking.Add(tracking);

                    lista.Add(expte);
                }
            }
            else
            {
                pParametro.ExpedientesTracking = new List<ExpExpedientesTracking>();
                //Primer Track para Log
                tracking = new ExpExpedientesTracking();
                tracking.Fecha = DateTime.Now;
                tracking.Sector = pSector;
                tracking.Sector.Filial = pSector.Filial;
                tracking.Estado.IdEstado = (int)EstadosExpedientesTracking.Activo;
                tracking.EstadoColeccion = EstadoColecciones.Agregado;
                pParametro.ExpedientesTracking.Add(tracking);
                lista.Add(pParametro);
            }

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    foreach (ExpExpedientes item in lista)
                    {
                        item.IdExpediente = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "ExpExpedientesInsertar");
                        if (item.IdExpediente == 0)
                        {
                            resultado = false;
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            break;
                        }
                        pParametro.IdExpediente = item.IdExpediente;

                        if (resultado && !this.ActualizarExpedientesTracking(item, bd, tran))
                        {
                            resultado = false;
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            break;
                        }

                        //Control Comentarios y Archivos
                        if (resultado && !TGEGeneralesF.ComentariosActualizar(item, bd, tran))
                        {
                            resultado = false;
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            break;
                        }

                        if (resultado && !TGEGeneralesF.ArchivosActualizar(item, bd, tran))
                        {
                            resultado = false;
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            break;
                        }
                        //Fin control Comentarios y Archivos
                    }

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoAlta";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdExpediente.ToString());
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
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
            }
            return resultado;
        }

        public override bool Modificar(ExpExpedientes pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            if (!this.Validar(pParametro))
                return false;

            this.ValidarDerivacionSectorPropio(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            ExpExpedientes valorViejo = new ExpExpedientes();
            valorViejo.IdExpediente = pParametro.IdExpediente;
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
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "ExpExpedientesActualizar"))
                        resultado = false;

                    if (resultado && !this.ActualizarExpedientesTracking(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    //Control Comentarios y Archivos
                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;
                    //Fin control Comentarios y Archivos

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
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
            }
            return resultado;
        }

        public bool AceptarDerivaciones(Objeto pResultado, List<ExpExpedientes> pLista, TGESectores pSector)
        {
            bool resultado = true;
            AyudaProgramacionLN.LimpiarMensajesError(pResultado);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    ExpExpedientesTracking tracking;
                    foreach (ExpExpedientes pParametro in pLista)
                    {
                        tracking = new ExpExpedientesTracking();
                        tracking.Fecha = DateTime.Now;
                        tracking.Sector = pSector;
                        tracking.Sector.Filial = pSector.Filial;
                        tracking.Estado.IdEstado = (int)EstadosExpedientesTracking.Activo;
                        tracking.EstadoColeccion = EstadoColecciones.Agregado;
                        pParametro.ExpedientesTracking.Add(tracking);

                        //Dejo el Update para que Valide la Concurrencia
                        //No se actualizan datos del Expediente
                        if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "ExpExpedientesActualizar"))
                        {
                            resultado = false;
                            AyudaProgramacionLN.MapearError(pParametro, pResultado);
                        }

                        if (resultado && !this.ActualizarExpedientesTracking(pParametro, bd, tran))
                        {
                            resultado = false;
                            AyudaProgramacionLN.MapearError(pParametro, pResultado);
                        }
                    }

                    if (resultado)
                    {
                        tran.Commit();
                        pResultado.CodigoMensaje = "ResultadoTransaccion";
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
                    pResultado.CodigoMensaje = ex.Message;
                    return false;
                }
            }
            return resultado;
        }

        public bool AceptarDerivacion(ExpExpedientes pParametro, TGESectores pSector)
        {
            ExpExpedientesTracking tracking = new ExpExpedientesTracking();
            tracking.Fecha = DateTime.Now;
            tracking.Sector = pSector;
            tracking.Sector.Filial = pSector.Filial;
            tracking.Estado.IdEstado = (int)EstadosExpedientesTracking.Activo;
            tracking.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.ExpedientesTracking.Add(tracking);
            return this.Modificar(pParametro);
        }

        public bool RechazarDerivacion(ExpExpedientes pParametro, TGESectores pSector)
        {
            ExpExpedientesTracking tracking = new ExpExpedientesTracking();
            tracking.Fecha = DateTime.Now;
            tracking.Sector = pSector;
            tracking.Sector.Filial = pSector.Filial;
            tracking.Estado.IdEstado = (int)EstadosExpedientesTracking.DerivacionRechazada;
            tracking.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.ExpedientesTracking.Add(tracking);
            return this.Modificar(pParametro);
        }

        private bool Validar(ExpExpedientes pParametro)
        {
            if (pParametro.ExpedientesTracking.Exists(x => x.Sector.IdSector == 0))
            {
                pParametro.CodigoMensaje = "ValidarSectorNoDefinido";
                return false;
            }
            return true;
        }

        private void ValidarDerivacionSectorPropio(ExpExpedientes pParametro)
        {
            ExpExpedientesTracking expTracking = pParametro.ExpedientesTracking.First(x => x.Estado.IdEstado == (int)EstadosExpedientesTracking.Activo);
            ExpExpedientesTracking ultimoTracking = pParametro.ExpedientesTracking[0];
            if (expTracking.Sector.IdSector == ultimoTracking.Sector.IdSector
                && ultimoTracking.Estado.IdEstado==(int)EstadosExpedientesTracking.Derivado)
            {
                ExpExpedientesTracking nuevoTrack = new ExpExpedientesTracking();
                nuevoTrack.Fecha = DateTime.Now;
                nuevoTrack.Sector = ultimoTracking.Sector;
                nuevoTrack.Sector.Filial = ultimoTracking.Sector.Filial;
                nuevoTrack.Estado.IdEstado = (int)EstadosExpedientesTracking.Activo;
                nuevoTrack.EstadoColeccion = EstadoColecciones.Agregado;
                pParametro.ExpedientesTracking.Add(nuevoTrack);
            }
        }

        private bool ActualizarExpedientesTracking(ExpExpedientes pParametro, Database bd, DbTransaction tran)
        {
            foreach (ExpExpedientesTracking expTracking in pParametro.ExpedientesTracking)
            {
                switch (expTracking.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        expTracking.UsuarioLogueado = pParametro.UsuarioLogueado;
                        expTracking.IdExpediente = pParametro.IdExpediente;
                        expTracking.Fecha = DateTime.Now;
                        expTracking.IdExpedienteTracking = BaseDatos.ObtenerBaseDatos().Agregar(expTracking, bd, tran, "ExpExpedientesTrackingInsertar");
                        if (expTracking.IdExpedienteTracking == 0)
                        {
                            AyudaProgramacionLN.MapearError(expTracking, pParametro);
                            return false;
                        }
                        break;
                    case EstadoColecciones.Borrado:
                    case EstadoColecciones.Modificado:
                        expTracking.Estado.IdEstado = (int)Estados.Baja;
                        expTracking.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(expTracking, bd, tran, "ExpExpedientesTrackingActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(expTracking, pParametro);
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        /// <summary>
        /// Obtiene una lista de Filiales posibles de Derivar
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public List<TGEFiliales> ExpedientesObtenerFilialesDerivar(ExpExpedientes pParametro, TGEFiliales pFilial, TGESectores pSector)
        {
            List<TGEFiliales> filiales = TGEGeneralesF.FilialesObenerLista();
            //Para Modificaciones desde los Tracking
            if (pParametro.ExpedientesTracking.Exists(x=>x.Estado.IdEstado==(int)EstadosExpedientesTracking.Activo))
            {
                ExpExpedientesTracking expTracking = pParametro.ExpedientesTracking.First(x => x.Estado.IdEstado == (int)EstadosExpedientesTracking.Activo);
                //Filiales para Sede Central -> Sectores excepto Mesa de Entrada
                if (expTracking.Sector.Filial.IdFilial == (int)EnumTGEFiliales.SedeCentral
                    && expTracking.Sector.IdSector != (int)EnumTGESectores.MesaEntrada)
                {
                    //Solo dejo Sede Central
                    TGEFiliales filial = filiales.Find(x => x.IdFilial == (int)EnumTGEFiliales.SedeCentral);
                    filiales = new List<TGEFiliales>();
                    filiales.Add(filial);
                }
            }
                //Para el Alta segun los datos del usuario activo
            else if (pFilial.IdFilial == (int)EnumTGEFiliales.SedeCentral
                && pSector.IdSector != (int)EnumTGESectores.MesaEntrada)
            {
                //Solo dejo Sede Central
                TGEFiliales filial = filiales.Find(x => x.IdFilial == (int)EnumTGEFiliales.SedeCentral);
                filiales = new List<TGEFiliales>();
                filiales.Add(filial);
            }
            filiales = AyudaProgramacionLN.ReacomodarIndicesColecion<TGEFiliales>(filiales);
            return filiales;
        }

        /// <summary>
        /// Obtiene la lista de Sectores posibles de Derivar
        /// </summary>
        /// <param name="pParametro">Expediente</param>
        /// <param name="pFilial">Filial a Derivar</param>
        /// <returns></returns>
        public List<TGESectores> ExpedientesObtenerSectoresDerivar(ExpExpedientes pParametro, TGEFiliales pFilial)
        {
            TGESectores filtro = new TGESectores();
            filtro.Filial.IdFilial = pFilial.IdFilial;
            filtro.Estado.IdEstado = (int)Estados.Activo;
            List<TGESectores> sectores = TGEGeneralesF.SectoresObtenerListaFiltro(filtro);
            if (pParametro.ExpedientesTracking.Exists(x=>x.Estado.IdEstado==(int)EstadosExpedientesTracking.Activo))
            {
                ExpExpedientesTracking expTracking = pParametro.ExpedientesTracking.First(x => x.Estado.IdEstado == (int)EstadosExpedientesTracking.Activo);
                ExpExpedientesTracking ultimoTracking = pParametro.ExpedientesTracking[0];
                //Si la Filial a Derivar es igual a la que Derivo saco el sector en el que esta
                if (expTracking.Sector.Filial.IdFilial == pFilial.IdFilial
                    && ultimoTracking.Estado.IdEstado != (int)EstadosExpedientesTracking.Derivado)
                {
                    //Saco el sector si es de la misma filial
                    TGESectores sector = sectores.Find(x => x.IdSector == expTracking.Sector.IdSector);
                    if (sector != null)
                        sectores.Remove(sector);
                }
            }
            //else 
            //{
            //    if (
            //    //Saco el sector si es de la misma filial
            //    TGESectores sector = sectores.Find(x => x.IdSector == pSector.IdSector);
            //    if (sector != null)
            //        sectores.Remove(sector);
            //}

            sectores = AyudaProgramacionLN.ReacomodarIndicesColecion<TGESectores>(sectores);
            return sectores;
        }

        public override bool Agregar(ExpExpedientes pParametro)
        {
            throw new NotImplementedException();
        }
    }
}
