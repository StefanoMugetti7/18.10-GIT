using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ahorros.Entidades;
using Comunes;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Servicio.AccesoDatos;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Afiliados.Entidades;
using Comunes.LogicaNegocio;
using Generales.FachadaNegocio;
using Auditoria.Entidades;
using Auditoria;

namespace Ahorros.LogicaNegocio
{
    class AhoCuentasLN : BaseLN<AhoCuentas>
    {
        public override AhoCuentas ObtenerDatosCompletos(AhoCuentas pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<AhoCuentas>("AhoCuentasSeleccionarDescripcion", pParametro);
            pParametro.Cotitulares = BaseDatos.ObtenerBaseDatos().ObtenerLista<AhoCotitulares>("AhoCotitularesSeleccionarPorCuenta", pParametro);
            pParametro.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }

        public AhoCuentas ObtenerDatosCompletos(AhoCuentas pParametro, Database bd, DbTransaction tran)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<AhoCuentas>("AhoCuentasSeleccionarDescripcion", pParametro, bd, tran);
            //pParametro.Cotitulares = BaseDatos.ObtenerBaseDatos().ObtenerLista<AhoCotitulares>("AhoCotitularesSeleccionarPorCuenta", pParametro);
            //pParametro.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            //pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }
         

        /// <summary>
        /// Devuelve una lista de Cuentas de Ahorro del Afiliado
        /// </summary>
        /// <param name="pParametro">IdAfiliado, [IdCuentaTipo]</param>
        /// <returns></returns>
        public override List<AhoCuentas> ObtenerListaFiltro(AhoCuentas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AhoCuentas>("AhoCuentasSeleccionarPorAfiliado", pParametro);
        }

        public override bool Agregar(AhoCuentas pParametro)
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
                    pParametro.NumeroCuenta = this.ObtenerProximoNumeroCuenta(bd, tran);
                    pParametro.IdCuenta = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "AhoCuentasInsertar");
                    if (pParametro.IdCuenta == 0)
                        resultado = false;

                    if (resultado && !this.ActualizarCotitulares(pParametro, bd, tran))
                        resultado = false;

                    //Control Comentarios y Archivos
                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
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

        public override bool Modificar(AhoCuentas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (!this.Validar(pParametro))
                return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            AhoCuentas valorViejo = new AhoCuentas();
            valorViejo.IdCuenta = pParametro.IdCuenta;
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

                    if (resultado && !this.ActualizarCotitulares(pParametro, bd, tran))
                        resultado = false;

                    //Control Comentarios y Archivos
                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
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

        private bool Validar(AhoCuentas pParametro)
        {
            if (pParametro.Estado.IdEstado == (int)EstadosAhorrosCuentas.CuentaCerrada)
            {
                if (pParametro.SaldoActual > 0)
                {
                    pParametro.CodigoMensaje = "ValidarCierreSaldoActual";
                    pParametro.CodigoMensajeArgs.Add(pParametro.SaldoActual.ToString("C2"));
                    return false;
                }
            }
            return true;
        }

        public bool Modificar(AhoCuentas pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "AhoCuentasActualizar"))
                return false;

            return true;
        }

        private int ObtenerProximoNumeroCuenta(Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<AhoCuentas>("AhoCuentasObtenerProximoNumeroCuenta", new Objeto(), bd, tran).NumeroCuenta;
        }

        private bool ActualizarCotitulares(AhoCuentas pParametro, Database bd, DbTransaction tran)
        {
            foreach (AhoCotitulares cotitular in pParametro.Cotitulares)
            {
                switch (cotitular.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        cotitular.IdCuenta = pParametro.IdCuenta;
                        cotitular.Estado.IdEstado = (int)Estados.Activo;
                        cotitular.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuario;
                        cotitular.UsuarioLogueado = pParametro.UsuarioLogueado;
                        cotitular.FechaAlta = DateTime.Now;
                        cotitular.IdCotitular = BaseDatos.ObtenerBaseDatos().Agregar(cotitular, bd, tran, "AhoCotitularesInsertar");
                        if (cotitular.IdCotitular == 0)
                        {
                            AyudaProgramacionLN.MapearError(cotitular, pParametro);
                            return false;
                        }
                        break;
                    case EstadoColecciones.Borrado:
                    case EstadoColecciones.Modificado:
                        cotitular.Estado.IdEstado = (int)Estados.Baja;
                        cotitular.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(cotitular, bd, tran, "AhoCotitularesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(cotitular, pParametro);
                            return false;
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;
        }
    }
}
