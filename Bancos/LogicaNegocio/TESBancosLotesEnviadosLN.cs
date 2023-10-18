using Auditoria;
using Auditoria.Entidades;
using Bancos.Entidades;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using CuentasPagar.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Bancos.LogicaNegocio
{
    public class TESBancosLotesEnviadosLN : BaseLN<TESBancosLotesEnviados>
    {
        public override bool Agregar(TESBancosLotesEnviados pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = false;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

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
                    if (Validaciones(pParametro))
                    {
                        resultado = this.Agregar(pParametro, bd, tran);
                    }

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
        internal bool Agregar(TESBancosLotesEnviados pParametro, Database bd, DbTransaction tran)
        {
            int agregarDetalles = 0;

            pParametro.IdBancoLoteEnvio = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "TESBancosLotesEnviadosInsertar");
            if (pParametro.IdBancoLoteEnvio == 0)
                return false;

            int orden= 1;
            foreach (TESBancosLotesEnviadosDetalle item in pParametro.Detalles)
            {
                item.Orden = orden;
                item.IdBancoLoteEnviado = pParametro.IdBancoLoteEnvio;
                agregarDetalles = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "TESBancosLotesEnviadosInsertarDetalle");
                orden++;
                if (agregarDetalles == 0)
                    return false;
            }

            return true;
        }
        public override bool Modificar(TESBancosLotesEnviados pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //if (!this.Validar(pParametro))
            //    return false;

            TESBancosLotesEnviados valorViejo = new TESBancosLotesEnviados();
            valorViejo.IdBancoCuenta = pParametro.IdBancoCuenta;
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
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TESBancosLotesEnviadosActualizar");

                    if(pParametro.Estado.IdEstado != (int)Estados.Baja)
                    {

                        var aux = pParametro.Detalles.Find(x => x.Estado.IdEstado == (int)EstadosOrdenesPago.Rechazado);
                        if(aux != null)
                        {
                            foreach (TESBancosLotesEnviadosDetalle item in pParametro.Detalles)
                            {
                                resultado = BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "TESBancosLotesEnviadosDetallesConfirmar");
                                if (!resultado)
                                    break;
                            }
                        }
                    }
                    else if (pParametro.Estado.IdEstado == (int)Estados.Baja)
                    {
                        resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TESBancosLotesEnviadosLiberarDetalles");
                        //if(resultado)
                        //{
                        //    foreach (TESBancosLotesEnviadosDetalle item in pParametro.Detalles)
                        //    {
                        //        resultado = BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "TESBancosLotesEnviadosAnularDetalles");
                        //        if (!resultado)
                        //            break;
                        //    }
                        //}
                    }

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    // if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                    //   resultado = false;

                    // if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                    //   resultado = false;
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
        public bool Autorizar(TESBancosLotesEnviados pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = false;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //if (!this.Validar(pParametro))
            //    return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            //TESBancosLotesEnviados valorViejo = new TESBancosLotesEnviados();
            //valorViejo.IdBancoCuenta = pParametro.IdBancoCuenta;
            //valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            //valorViejo = this.ObtenerDatosCompletos(valorViejo);
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (Validaciones(pParametro))
                    {
                        resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TESBancosLotesEnviadosAutorizar");

                        //if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        //   resultado = false;
                    }
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
        public bool Anular(TESBancosLotesEnviados pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TESBancosLotesEnviadosAnular");

                    if (resultado)
                    {
                            resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TESBancosLotesEnviadosLiberarDetalles");
                    }

                    //if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                    //   resultado = false;

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

        public bool Confirmar(TESBancosLotesEnviados pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = false;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (Validaciones(pParametro))
                    {
                        resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TESBancosLotesEnviadosConfirmar");
                        if (resultado)
                        {
                            foreach (TESBancosLotesEnviadosDetalle item in pParametro.Detalles)
                            {
                                if (ValidacionesDetalles(item))
                                {
                                    resultado =BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "TESBancosLotesEnviadosDetallesConfirmar");
                                    if (!resultado)
                                    break;
                                }
                                else
                                {
                                    throw new Exception(item.CodigoMensajeArgs[0]);
                                }
                            }
                            if(resultado)
                            {
                                resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(pParametro, bd, tran, "TESBancosLotesEnviadosConfirmarCajasMovimientos");
                            }
                        }
                    }
                    //if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                    //   resultado = false;

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
        public bool CancelarConfirmado(TESBancosLotesEnviados pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = false;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (Validaciones(pParametro))
                    {
                        resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TESBancosLotesEnviadosCancelarConfirmado");

                        if (resultado)
                        {
                            resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TESBancosLotesEnviadosLiberarDetalles");
                            if (resultado)
                            {
                                foreach (TESBancosLotesEnviadosDetalle item in pParametro.Detalles)
                                {
                                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "TESBancosLotesEnviadosAnularDetalles");
                                    if (!resultado)
                                        break;
                                }
                            }
                        }
                    }
                    //if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                    //   resultado = false;

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
        public override TESBancosLotesEnviados ObtenerDatosCompletos(TESBancosLotesEnviados pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<TESBancosLotesEnviados>("TESBancosLotesEnviadosSeleccionar", pParametro);
            pParametro.Detalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<TESBancosLotesEnviadosDetalle>("TESBancosLotesEnviadosObtenerDetalleCargados", pParametro);

            return pParametro;
        }

        public override List<TESBancosLotesEnviados> ObtenerListaFiltro(TESBancosLotesEnviados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESBancosLotesEnviados>("TESBancosLotesEnviadosSeleccionarFiltro", pParametro);
        }
        public List<TESBancosLotesEnviados> ObtenerCuentas()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESBancosLotesEnviados>("TESBancosLotesEnviadosObtenerCuentas", new TESBancosLotesEnviados());
        }
        public List<TESBancosLotesEnviados> ObtenerTiposArchivos()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESBancosLotesEnviados>("TESBancosLotesEnviadosObtenerTiposArchivos", new TESBancosLotesEnviados());
        }
        public List<TESBancosLotesEnviadosDetalle> ObtenerTiposOperaciones()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESBancosLotesEnviadosDetalle>("TESBancosLotesEnviadosObtenerTiposOperaciones", new TESBancosLotesEnviadosDetalle());
        }
        public List<TESBancosLotesEnviadosDetalle> ObtenerDetalles(TESBancosLotesEnviados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESBancosLotesEnviadosDetalle>("TESBancosLotesEnviadosObtenerDetalle", pParametro);
        }

        public bool Validaciones(TESBancosLotesEnviados pParametro)
        {
            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "TesBancosLotesEnviadosValidaciones"))
                return false;

            return true;
        }
        public bool ValidacionesDetalles(TESBancosLotesEnviadosDetalle pParametro)
        {
            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "TesBancosLotesEnviadosDetallesValidaciones"))
                return false;

            return true;
        }
        public DataTable ObtenerDatosTxt(TESBancosLotesEnviados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("TESBancosLotesEnviadosTxt", pParametro);
        }
        private byte[] ObtenerSelloTiempo(TESBancosLotesEnviados pParametro, Database db, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerSelloTiempo("TESBancosLotesEnviadosSeleccionar", pParametro, db, tran);
        }
        private byte[] ObtenerSelloTiempoDetalle(TESBancosLotesEnviadosDetalle pParametro, Database db, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerSelloTiempo("TESBancosLotesEnviadosDetalleSeleccionar", pParametro, db, tran);
        }
    }
}
