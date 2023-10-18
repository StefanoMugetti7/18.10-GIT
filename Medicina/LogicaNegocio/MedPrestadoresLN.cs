using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Medicina.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Generales.FachadaNegocio;
using Auditoria;
using Auditoria.Entidades;

namespace Medicina.LogicaNegocio
{
    class MedPrestadoresLN : BaseLN<MedPrestadores>
    {
        public override bool Agregar(MedPrestadores pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            //if (!this.Validar(pParametro))
            //    return false;

            pParametro.FechaEvento = DateTime.Now;
            pParametro.Estado.IdEstado = (int)Estados.Activo;
            pParametro.FechaAlta = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdPrestador = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "MedPrestadoresInsertar");
                    if (pParametro.IdPrestador == 0)
                        resultado = false;

                    if (resultado && !this.ActualizarPrestadoresDiasHoras(pParametro, new MedPrestadores(), bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarPrestadoresEspecializaciones(pParametro, new MedPrestadores(), bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
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

        public override bool Modificar(MedPrestadores pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //if (!this.Validar(pParametro))
            //    return false;

            pParametro.FechaEvento = DateTime.Now;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            MedPrestadores valorViejo = new MedPrestadores();
            valorViejo.IdPrestador = pParametro.IdPrestador;
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
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "MedPrestadoresActualizar"))
                        resultado = false;

                    if (resultado && !this.ActualizarPrestadoresDiasHoras(pParametro, valorViejo, bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarPrestadoresEspecializacionesDiasHoras(pParametro, valorViejo, bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarPrestadoresEspecializaciones(pParametro, valorViejo, bd, tran))
                        resultado = false;

                

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
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

        public override MedPrestadores ObtenerDatosCompletos(MedPrestadores pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<MedPrestadores>("MedPrestadoresSeleccionar", pParametro);
            pParametro.PrestadoresDiasHoras = BaseDatos.ObtenerBaseDatos().ObtenerLista<MedPrestadoresDiasHoras>("MedPrestadoresDiasHorasSeleccionarPorMedPrestadores", pParametro);
            foreach(MedPrestadoresDiasHoras prestador in pParametro.PrestadoresDiasHoras )
            {
                prestador.Especializaciones = BaseDatos.ObtenerBaseDatos().ObtenerLista<MedEspecializaciones>("MedPrestadoresDiasHorasSeleccionarEspecializaciones", prestador);
            }
           
                pParametro.PrestadoresEspecializaciones = BaseDatos.ObtenerBaseDatos().ObtenerLista<MedPrestadoresEspecializaciones>("MedPrestadoresEspecializacionesSeleccionarPorMedPrestadores", pParametro);
            pParametro.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }
        public MedPrestadores ObtenerDatosCompletosTurnera(MedPrestadores pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<MedPrestadores>("MedPrestadoresSeleccionar", pParametro);
            pParametro.PrestadoresDiasHoras = BaseDatos.ObtenerBaseDatos().ObtenerLista<MedPrestadoresDiasHoras>("MedPrestadoresDiasHorasSeleccionarPorMedPrestadores", pParametro);
            foreach(MedPrestadoresDiasHoras prestador in pParametro.PrestadoresDiasHoras )
            {
                prestador.Especializaciones = BaseDatos.ObtenerBaseDatos().ObtenerLista<MedEspecializaciones>("MedPrestadoresDiasHorasSeleccionarEspecializaciones", prestador);
            }
           
                pParametro.PrestadoresEspecializaciones = BaseDatos.ObtenerBaseDatos().ObtenerLista<MedPrestadoresEspecializaciones>("MedPrestadoresDiasHorasSeleccionarPorMedPrestadoresTurnera", pParametro);
            pParametro.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }

        public override List<MedPrestadores> ObtenerListaFiltro(MedPrestadores pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<MedPrestadores>("MedPrestadoresSeleccionarPorFiltros", pParametro);
        }

        private bool ActualizarPrestadoresDiasHoras(MedPrestadores pParametro, MedPrestadores pValorViejo, Database db, DbTransaction tran)
        {
            foreach (MedPrestadoresDiasHoras item in pParametro.PrestadoresDiasHoras)
            {
                item.UsuarioLogueado = pParametro.UsuarioLogueado;
                switch (item.EstadoColeccion)
                {
                    #region "Agregado"
                    case EstadoColecciones.Agregado:
                        item.Estado.IdEstado = (int)Estados.Activo;
                        item.IdPrestador = pParametro.IdPrestador;
                        item.IdPrestadorDiaHora = BaseDatos.ObtenerBaseDatos().Agregar(item, db, tran, "MedPrestadoresDiasHorasInsertar");
                        if (item.IdPrestadorDiaHora == 0)
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        break;
                    #endregion

                    #region "Modificado"
                    case EstadoColecciones.Modificado:
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, db, tran, "MedPrestadoresDiasHorasActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.PrestadoresDiasHoras.Find(x => x.IdPrestadorDiaHora == item.IdPrestadorDiaHora), Acciones.Update, item, db, tran))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }

                        break;
                    #endregion
                }
            }
            return true;
        }

        private bool ActualizarPrestadoresEspecializaciones(MedPrestadores pParametro, MedPrestadores pValorViejo, Database db, DbTransaction tran)
        {
            foreach (MedPrestadoresEspecializaciones item in pParametro.PrestadoresEspecializaciones)
            {
                switch (item.EstadoColeccion)
                {
                    #region "Agregado"
                    case EstadoColecciones.Agregado:
                        item.Estado.IdEstado = (int)Estados.Activo;
                        item.IdPrestador = pParametro.IdPrestador;
                        item.IdPrestadorEspecializacion = BaseDatos.ObtenerBaseDatos().Agregar(item, db, tran, "MedPrestadoresEspecializacionesInsertar");
                        if (item.IdPrestadorEspecializacion == 0)
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }

                        if (!TGEGeneralesF.CamposActualizarCamposValores(item, db, tran))
                            return false;

                        break;
                    #endregion

                    #region "Modificado"
                    case EstadoColecciones.Modificado:
                        item.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, db, tran, "MedPrestadoresEspecializacionesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.PrestadoresEspecializaciones.Find(x => x.IdPrestadorEspecializacion == item.IdPrestadorEspecializacion), Acciones.Update, item, db, tran))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }

                        if (!TGEGeneralesF.CamposActualizarCamposValores(item, db, tran))
                            return false;

                        break;
                    #endregion
                }
            }
            return true;
        }


        private bool ActualizarPrestadoresEspecializacionesDiasHoras(MedPrestadores pParametro, MedPrestadores pValorViejo, Database db, DbTransaction tran)
        {
            foreach (MedPrestadoresDiasHoras item in pParametro.PrestadoresDiasHoras)
            {

                foreach(MedEspecializaciones medEspecializaciones in item.Especializaciones)
                {
                    switch (medEspecializaciones.EstadoColeccion)
                    {
                        #region "Agregado"
                        case EstadoColecciones.Agregado:
                            MedPrestadoresEspecializacionesDiasHoras especializacion = new MedPrestadoresEspecializacionesDiasHoras();

                            especializacion.Estado.IdEstado = (int)Estados.Activo;
                            especializacion.IdEspecializacion = medEspecializaciones.IdEspecializacion;
                            especializacion.IdPrestadorDiaHora = item.IdPrestadorDiaHora;

                            BaseDatos.ObtenerBaseDatos().Actualizar(especializacion, db, tran, "MedPrestadoresEspecializacionesDiasHorasInsertar");
                          
                            break;
                        #endregion

                        #region "Modificado"
                        //case EstadoColecciones.Modificado:
                        //    item.UsuarioLogueado = pParametro.UsuarioLogueado;
                        //    if (!BaseDatos.ObtenerBaseDatos().Actualizar(medEspecializaciones, db, tran, "MedPrestadoresEspecializacionesDiasHorasActualizar"))
                        //    {
                        //        AyudaProgramacionLN.MapearError(item, pParametro);
                        //        return false;
                        //    }
                        //    if (!AuditoriaF.AuditoriaAgregar(
                        //        pValorViejo.PrestadoresDiasHoras.Find(x => x.IdPrestadorDiaHora == medEspecializaciones.IdEspecializacion), Acciones.Update, item, db, tran))
                        //    {
                        //        AyudaProgramacionLN.MapearError(item, pParametro);
                        //        return false;
                        //    }

                        //    break;
                        case EstadoColecciones.Borrado:
                           
                            MedPrestadoresEspecializacionesDiasHoras especializacionBorrar = new MedPrestadoresEspecializacionesDiasHoras();

                            especializacionBorrar.Estado.IdEstado = (int)Estados.Activo;
                            especializacionBorrar.IdEspecializacion = medEspecializaciones.IdEspecializacion;
                            especializacionBorrar.IdPrestadorDiaHora = item.IdPrestadorDiaHora;
                            if (!BaseDatos.ObtenerBaseDatos().Actualizar(especializacionBorrar, db, tran, "MedPrestadoresEspecializacionesDiasHorasBorrar"))
                            {
                                AyudaProgramacionLN.MapearError(medEspecializaciones, pParametro);
                                return false;

                            }
                            //if (!AuditoriaF.AuditoriaAgregar(
                            //  pValorViejo.PrestadoresDiasHoras.Find(x => x.IdPrestadorDiaHora == item.IdPrestadorDiaHora), Acciones.Update, item, db, tran))
                            //{
                            //    AyudaProgramacionLN.MapearError(item, pParametro);
                            //    return false;
                            //}
                            break;
                            #endregion
                    }
                }

                
            }
            return true;
        }
    }
}
