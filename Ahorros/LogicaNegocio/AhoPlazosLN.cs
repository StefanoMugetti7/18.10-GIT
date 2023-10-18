using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ahorros.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Comunes.Entidades;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Generales.FachadaNegocio;
using Comunes.LogicaNegocio;

namespace Ahorros.LogicaNegocio
{
    class AhoPlazosLN : BaseLN<AhoPlazos>
    {

        public override AhoPlazos ObtenerDatosCompletos(AhoPlazos pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<AhoPlazos>("AhoPlazosSeleccionarDescripcion", pParametro);
            //pParametro.ParametrosValores = TGEGeneralesF.ParametrosValoresObtenerLista(pParametro);
            return pParametro;
        }

        public override List<AhoPlazos> ObtenerListaFiltro(AhoPlazos pParametro)
        {
            if (pParametro.Estado.IdEstado == (int)Estados.Baja)
                pParametro.Estado.IdEstado = (int)EstadosTodos.Todos;

            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AhoPlazos>("AhoPlazosListar", pParametro);
        }

        public List<AhoPlazos> ObtenerListaActiva()
        {
            AhoPlazos parametro = new AhoPlazos();
            parametro.Estado.IdEstado = (int)Estados.Activo;
            return this.ObtenerListaFiltro(parametro);
        }

        public override bool Agregar(AhoPlazos pParametro)
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
                    pParametro.IdPlazos = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "AhoPlazosInsertar");
                    if (pParametro.IdPlazos == 0)
                        resultado = false;

                    //Control Comentarios y Archivos
                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;
                    //Fin control Comentarios y Archivos

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
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
            }
            return resultado;
        }

        public override bool Modificar(AhoPlazos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

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
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "AhoPlazosActualizar"))
                        resultado = false;

                    //Control Comentarios y Archivos
                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;
                    //Fin control Comentarios y Archivos

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
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
            }
            return resultado;
        }

        public List<AhoPlazos> ObtenerAnterior(AhoPlazos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AhoPlazos>("AhoPlazosObtenerAnterior", pParametro);
        }
    }
}
