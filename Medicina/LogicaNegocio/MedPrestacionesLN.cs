using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Medicina.Entidades;
using Comunes;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Servicio.AccesoDatos;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria;
using Auditoria.Entidades;

namespace Medicina.LogicaNegocio
{
    class MedPrestacionesLN : BaseLN<MedPrestaciones>
    {
        public override bool Agregar(MedPrestaciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            //if (!this.Validar(pParametro))
            //    return false;

            pParametro.FechaEvento = DateTime.Now;
            pParametro.Estado.IdEstado = (int)EstadosPrestaciones.Activo;
            pParametro.Fecha = DateTime.Now;
            pParametro.FechaAlta = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {


                    pParametro.IdPrestacion = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "MedPrestacionesInsertar");
                    if (pParametro.IdPrestacion == 0)
                        resultado = false;

                    if (pParametro.Turno.IdTurno > 0)
                    {
                        pParametro.Turno.UsuarioLogueado = pParametro.UsuarioLogueado;
                        pParametro.Turno.Estado.IdEstado = (int)EstadosTurnos.Atendido;

                        if (!new MedTurnerasLN().ActualizarTurnos(pParametro.Turno, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(pParametro.Turno, pParametro);
                            return false;
                        }
                    }

                    //if (resultado && !this.ActualizarPrestadoresDiasHoras(pParametro, new MedPrestadores(), bd, tran))
                    //    resultado = false;

                    //if (resultado && !this.ActualizarPrestadoresEspecializaciones(pParametro, new MedPrestadores(), bd, tran))
                    //    resultado = false;

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

        public override bool Modificar(MedPrestaciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //if (!this.Validar(pParametro))
            //    return false;

            pParametro.FechaEvento = DateTime.Now;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            MedPrestaciones valorViejo = new MedPrestaciones();
            valorViejo.IdPrestacion = pParametro.IdPrestacion;
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
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "MedPrestacionesActualizar"))
                        resultado = false;

                    //if (resultado && !this.ActualizarPrestadoresDiasHoras(pParametro, valorViejo, bd, tran))
                    //    resultado = false;

                    //if (resultado && !this.ActualizarPrestadoresEspecializaciones(pParametro, valorViejo, bd, tran))
                    //    resultado = false;

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

        public override MedPrestaciones ObtenerDatosCompletos(MedPrestaciones pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<MedPrestaciones>("MedPrestacionesSeleccionar", pParametro);
            pParametro.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }

        public override List<MedPrestaciones> ObtenerListaFiltro(MedPrestaciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<MedPrestaciones>("MedPrestacionesSeleccionarFiltro", pParametro);
        }
        public List<MedPrestaciones> ObtenerListaFiltroPacientes(MedPrestaciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<MedPrestaciones>("MedPrestacionesSeleccionarFiltroPacientes", pParametro);
        }
    }
}
