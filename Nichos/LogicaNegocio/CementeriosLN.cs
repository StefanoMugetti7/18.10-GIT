using Comunes;
using Comunes.LogicaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Nichos.Entidades;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Nichos.LogicaNegocio
{
    public class CementeriosLN : BaseLN<NCHCementerios>
    {
        public override List<NCHCementerios> ObtenerListaFiltro(NCHCementerios pParametro)
        {
            throw new NotImplementedException();
        }

        public List<NCHCementerios> ObtenerListaActiva(NCHCementerios pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<NCHCementerios>("NCHCementeriosSeleccionarListaActiva", pParametro);
        }

        public DataTable ObtenerListaGrilla(NCHCementerios pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[NCHCementeriosSeleccionarDescripcionPorFiltro]", pParametro);
        }

        public override NCHCementerios ObtenerDatosCompletos(NCHCementerios pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<NCHCementerios>("NCHCementeriosSeleccionar", pParametro);
        }

        public override bool Agregar(NCHCementerios pParametro)
        {
            if (pParametro.IdCementerio > 0)
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

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdCementerio.ToString());
                    }
                    else                   
                        tran.Rollback();
                    
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

        internal bool Agregar(NCHCementerios pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdCementerio = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "NCHCementeriosInsertar");
            if (pParametro.IdCementerio == 0)
                return false;

            return true;
        }

        public override bool Modificar(NCHCementerios pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;

            if (!this.Validaciones(pParametro))
                return false;

            NCHCementerios valorViejo = new NCHCementerios();
            valorViejo.IdCementerio = pParametro.IdCementerio;
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

                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "NCHCementeriosActualizar"))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else                  
                        tran.Rollback();
                    
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

        private bool Validaciones(NCHCementerios pParametro)
        {
            return true;
        }
    }
}
