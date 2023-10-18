using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Contabilidad.Entidades;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Servicio.AccesoDatos;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria;
using Auditoria.Entidades;
using Generales.FachadaNegocio;
using System.Data;

namespace Contabilidad.LogicaNegocio
{
    class CtbBienesUsosLN : BaseLN<CtbBienesUsos>
    {
        public override bool Agregar(CtbBienesUsos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            //if (!this.Validar(pParametro))
            //    return false;

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            pParametro.FechaEvento = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdBienUso = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CtbBienesUsosInsertar");
                    if (pParametro.IdBienUso == 0)
                        resultado = false;

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

        public override bool Modificar(CtbBienesUsos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            //if (!this.Validar(pParametro))
            //    return false;

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CtbBienesUsos bienUsoViejo = new CtbBienesUsos();
            bienUsoViejo.IdBienUso = pParametro.IdBienUso;
            bienUsoViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            bienUsoViejo = this.ObtenerDatosCompletos(bienUsoViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = this.Modificar(pParametro, bd, tran);

                    if (resultado && !AuditoriaF.AuditoriaAgregar(bienUsoViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

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

        public override CtbBienesUsos ObtenerDatosCompletos(CtbBienesUsos pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CtbBienesUsos>("CtbBienesUsosSeleccionar", pParametro);
            pParametro.BienesUsosDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbBienesUsosDetalles>("CtbBienesUsosDetallesSeleccionarPorBienesUsos", pParametro);
            return pParametro;
        }

        public List<CtbBienesUsos> ObtenerLista(CtbBienesUsos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbBienesUsos>("CtbBienesUsosListar", pParametro);
        }

        public DataTable ObtenerListaGrilla(CtbBienesUsos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CtbBienesUsosListarFiltro", pParametro);
        }

        public override List<CtbBienesUsos> ObtenerListaFiltro(CtbBienesUsos pParametro)
        {
            var bienesUsos = BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbBienesUsos>("CtbBienesUsosListarFiltro", pParametro);
            //foreach (var bienUso in bienesUsos)
            //{
            //    bienUso.BienesUsosDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbBienesUsosDetalles>("CtbBienesUsosDetallesSeleccionarPorBienesUsos", bienUso);
            //}
            return bienesUsos;
        }

        private bool Modificar(CtbBienesUsos pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbBienesUsosActualizar"))
                return false;

            return true;
        }

        private static bool GuardarBienesUsosDetalles(DbTransaction tran, Database bd, CtbBienesUsosDetalles bienesUsosDetalle)
        {
            bienesUsosDetalle.Estado.IdEstado = (int)Estados.Activo;
            bienesUsosDetalle.IdBienUsoDetalle = BaseDatos.ObtenerBaseDatos().Agregar(bienesUsosDetalle, bd, tran, "CtbBienesUsosDetallesInsertar");
            if (bienesUsosDetalle.IdBienUsoDetalle == 0)
                return false;
            return true;
        }

        private bool Validar(CtbBienesUsos pParametro)
        {
            //TO DO: ver validaciones
            return true;
        }
    }
}
