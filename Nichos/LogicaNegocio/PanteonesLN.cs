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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nichos.LogicaNegocio
{
    class PanteonesLN : BaseLN<NCHPanteones>
    {
        public override List<NCHPanteones> ObtenerListaFiltro(NCHPanteones pParametro)
        {
            throw new NotImplementedException();
        }

        public List<NCHPanteones> ObtenerListaActiva(NCHPanteones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<NCHPanteones>("NCHPanteonesSeleccionarListaActiva", pParametro);
        }

        public DataTable ObtenerListaGrilla(NCHPanteones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[NCHPanteonesSeleccionarDescripcionPorFiltro]", pParametro);
        }

        public override NCHPanteones ObtenerDatosCompletos(NCHPanteones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<NCHPanteones>("NCHPanteonesSeleccionar", pParametro);
        }

        public override bool Agregar(NCHPanteones pParametro)
        {
            if (pParametro.IdPanteon > 0)
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
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdPanteon.ToString());
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

        internal bool Agregar(NCHPanteones pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdPanteon = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "NCHPanteonesInsertar");
            if (pParametro.IdPanteon == 0)
                return false;

            return true;
        }

        public override bool Modificar(NCHPanteones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;

            if (!this.Validaciones(pParametro))
                return false;

            NCHPanteones valorViejo = new NCHPanteones();
            valorViejo.IdPanteon = pParametro.IdPanteon;
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

                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "NCHPanteonesActualizar"))
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

        private bool Validaciones(NCHPanteones pParametro)
        {
            return true;
        }
    }
}
