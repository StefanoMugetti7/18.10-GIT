using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Turismo.Entidades;

namespace Turismo
{
    public class ConveniosLN : BaseLN<TurConvenios>
    {
        public ConveniosLN()
        {
        }
        public override bool Agregar(TurConvenios pParametro)
        {

            if (pParametro.IdConvenio > 0)
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
                    pParametro.LoteDetalles = pParametro.Detalles.ToDataTable().ToXmlDocument();
                    pParametro.LoteExcepciones = pParametro.Excepciones.ToDataTable().ToXmlDocument();

                    if (!this.Validaciones(pParametro))
                        return false;

                    if (resultado && !this.Agregar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdConvenio.ToString());
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

        private bool Agregar(TurConvenios pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdConvenio = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "TurConveniosInsertar");
            if (pParametro.IdConvenio == 0)
                return false;

            return true;
        }

        private bool Validaciones(TurConvenios pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "TurConveniosValidaciones");
        }

        public override bool Modificar(TurConvenios pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            //t valorViejo = new t();
            //valorViejo.t = pParametro.IdListaEleccion;
            //valorViejo.t = pParametro.UsuarioLogueado;
            //valorViejo = this.ObtenerDatosCompletos(valorViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();
            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.LoteDetalles = pParametro.Detalles.ToDataTable().ToXmlDocument();
                    pParametro.LoteExcepciones = pParametro.Excepciones.ToDataTable().ToXmlDocument();

                    resultado = Validaciones(pParametro);

                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TurConveniosActualizar"))
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

        public override TurConvenios ObtenerDatosCompletos(TurConvenios pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<TurConvenios>("TurConveniosSeleccionar", pParametro);
            pParametro.Detalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<TurConveniosDetalles>("TurConveniosDetallesSeleccionarPorIdConvenio", pParametro);
            pParametro.Excepciones = BaseDatos.ObtenerBaseDatos().ObtenerLista<TurConveniosExcepciones>("TurConveniosExcepcionesSeleccionarPorIdConvenio", pParametro);
            return pParametro;
        }

        public override List<TurConvenios> ObtenerListaFiltro(TurConvenios pParametro)
        {
            throw new NotImplementedException();
        }

        public DataTable ObtenerListaGrilla(TurConvenios pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[TurConveniosSeleccionarDescripcionPorFiltro]", pParametro);
        }

        public List<TurConvenios> ObtenerHoteles(TurConvenios pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TurConvenios>("[TurConveniosObtenerHoteles]", pParametro);
        }
    }
}