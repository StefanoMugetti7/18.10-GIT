using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Afiliados.Entidades;
using Servicio.AccesoDatos;
using Generales.FachadaNegocio;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria.Entidades;
using Auditoria;

namespace Afiliados.LogicaNegocio
{
    class AfiCertificadoSupervivenciaLN : BaseLN<AfiCertificadosSupervivencia>
    {
        public override AfiCertificadosSupervivencia ObtenerDatosCompletos(AfiCertificadosSupervivencia pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<AfiCertificadosSupervivencia>("AfiCertificadosSupervivenciaSeleccionar", pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }

        /// <summary>
        /// Devuelve una lista de cerfificados por filtros
        /// </summary>
        /// <param name="pParametro">IdAfiliado</param>
        /// <returns></returns>
        public override List<AfiCertificadosSupervivencia> ObtenerListaFiltro(AfiCertificadosSupervivencia pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiCertificadosSupervivencia>("AfiCertificadosSupervivenciaSeleccionarFiltro", pParametro);
        }

        public override bool Agregar(AfiCertificadosSupervivencia pParametro)
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
                    pParametro.IdCertificadoSupervivencia = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "AfiCertificadosSupervivenciaInsertar");
                    if (pParametro.IdCertificadoSupervivencia == 0)
                        resultado = false;

                    //Control Comentarios y Archivos
                    //if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                    //    resultado = false;

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
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        public override bool Modificar(AfiCertificadosSupervivencia pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            AfiCertificadosSupervivencia valorViejo = new AfiCertificadosSupervivencia();
            valorViejo.IdCertificadoSupervivencia = pParametro.IdCertificadoSupervivencia;
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
                    resultado = this.Modificar(pParametro, bd, tran);

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    //Control Comentarios y Archivos
                    //if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                    //    resultado = false;

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
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        public bool Modificar(AfiCertificadosSupervivencia pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "AfiCertificadosSupervivenciaActualizar"))
                return false;

            return true;
        }


    }
}
