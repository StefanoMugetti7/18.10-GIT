using Comunes;
using Comunes.LogicaNegocio;
using CRM.Entidades;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Database = Microsoft.Practices.EnterpriseLibrary.Data.Database;

namespace CRM.LogicaNegocio
{
    public class RequerimientosLN: BaseLN<CRMRequerimientos>
    {
        public override bool Agregar(CRMRequerimientos pParametro)
        {

            if (pParametro.IdRequerimiento > 0)
                return true;

            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            if (!this.Validaciones(pParametro))
                return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = Validaciones(pParametro);

                    if (resultado && !this.Agregar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdRequerimiento.ToString());
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

        public override bool Modificar(CRMRequerimientos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;

            if (!this.Validaciones(pParametro))
                return false;

            CRMRequerimientos valorViejo = new CRMRequerimientos();
            valorViejo.IdRequerimiento = pParametro.IdRequerimiento;
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
                    resultado = Validaciones(pParametro);
                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CRMRequerimientosActualizar"))
                        resultado = false;

                    //if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                    //    resultado = false;
                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
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

        public override CRMRequerimientos ObtenerDatosCompletos(CRMRequerimientos pParametro)
        {
         pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CRMRequerimientos>("CRMRequerimientosSeleccionar", pParametro);
         pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
        return pParametro;

        }

        public override List<CRMRequerimientos> ObtenerListaFiltro(CRMRequerimientos pParametro)
        {
            throw new NotImplementedException();
        }
        internal bool Agregar(CRMRequerimientos pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdRequerimiento = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CRMRequerimientosInsertar");
            if (pParametro.IdRequerimiento == 0)
                return false;

            return true;
        }
        private bool Validaciones(CRMRequerimientos pParametro)
        {
            return true;
        }
        public DataTable ObtenerListaGrilla(CRMRequerimientos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[CRMRequerimientosSeleccionarDescripcionPorFiltro]", pParametro);
        }
        public List<CRMRequerimientos> ObtenerAcciones()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CRMRequerimientos>("CRMRequerimientosObtenerAcciones", new CRMRequerimientos());
        } 
        public List<CRMRequerimientos> ObtenerTiposRequerimientos()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CRMRequerimientos>("CRMRequerimientosObtenerTiposRequerimientos", new CRMRequerimientos());
        }  
        public List<CRMRequerimientos> ObtenerPrioridades()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CRMRequerimientos>("CRMRequerimientosObtenerPrioridades", new CRMRequerimientos());
        }  
        public List<CRMRequerimientos> ObtenerCategorias()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CRMRequerimientos>("CRMRequerimientosObtenerCategorias", new CRMRequerimientos());
        } 
        public List<CRMRequerimientos> ObtenerOrigenSolicitud()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CRMRequerimientos>("CRMRequerimientosObtenerOrigenSolicitud", new CRMRequerimientos());
        }  
        public List<CRMRequerimientos> ObtenerEntidadesOrigen()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CRMRequerimientos>("CRMRequerimientosObtenerEntidadesOrigen", new CRMRequerimientos());
        } 
        public List<CRMRequerimientos> ObtenerEntidadesDestino()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CRMRequerimientos>("CRMRequerimientosObtenerEntidadesDestino", new CRMRequerimientos());
        }  
        public List<CRMRequerimientos> ObtenerTecnicos()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CRMRequerimientos>("CRMRequerimientosObtenerTecnicos", new CRMRequerimientos());
        }
        public DataTable ObtenerCardsBootStrap(CRMRequerimientos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CRMRequerimientosSeleccionarCards", pParametro);
        }
        public DataTable ObtenerCardsBootStrapListar(CRMRequerimientos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CRMRequerimientosSeleccionarCardsListar", pParametro);
        }
        public bool AgregarSeguimiento(CRMSeguimientos pParametro)
        {

            if (pParametro.IdSeguimiento> 0)
                return true;

            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            if (!this.ValidacionesSeguimiento(pParametro))
                return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = ValidacionesSeguimiento(pParametro);

                    if (resultado && !this.AgregarSeguimiento(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdRequerimiento.ToString());
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
        private bool ValidacionesSeguimiento(CRMSeguimientos pParametro)
        {
            return true;
        }
        internal bool AgregarSeguimiento(CRMSeguimientos pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdSeguimiento = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CRMSeguimientosInsertar");
            if (pParametro.IdSeguimiento == 0)
                return false;

            return true;
        }
        public bool AgregarSolucion(CRMSeguimientos pParametro)
        {

            if (pParametro.IdSeguimiento > 0)
                return true;

            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            if (!this.ValidacionesSolucion(pParametro))
                return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = ValidacionesSolucion(pParametro);

                    if (resultado && !this.AgregarSolucion(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdRequerimiento.ToString());
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
        private bool ValidacionesSolucion(CRMSeguimientos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro,"CRMRequerimientosSolucionValidaciones");
        }
        internal bool AgregarSolucion(CRMSeguimientos pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdSeguimiento = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CRMSolucionInsertar");
            if (pParametro.IdSeguimiento == 0)
                return false;

            return true;
        }
    }
}
