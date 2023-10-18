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
    public class NichosLN : BaseLN<NCHNichos>
    {
        public override List<NCHNichos> ObtenerListaFiltro(NCHNichos pParametro)
        {
            throw new NotImplementedException();
        }
        public List<NCHNichos> ObtenerListaActiva(NCHNichos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<NCHNichos>("NCHNichosSeleccionarListaActiva", pParametro);
        }
        public List<NCHNichos> ObtenerListaSegunPanteon(NCHNichos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<NCHNichos>("NCHNichosObtenerListaSegunPanteon", pParametro);
        }

        public DataTable ObtenerListaGrilla(NCHNichos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[NCHNichosSeleccionarDescripcionPorFiltro]", pParametro);
        }
        public DataTable ObtenerDisponibles(NCHNichos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[NCHNichosObtenerDisponibles]", pParametro);
        }
        public DataTable ObtenerImporte(NCHNichos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("[NCHCalcularImporteNichos]", pParametro);
        }

        public override NCHNichos ObtenerDatosCompletos(NCHNichos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<NCHNichos>("NCHNichosSeleccionar", pParametro);
        }
        public DataTable ObtenerCardsBootStrap(NCHNichos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("NCHNichosSeleccionarCards", pParametro);
        } 
        public DataTable ObtenerNichosAfiliados(NCHNichos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("NCHNichosObtenerAfiliados", pParametro);
        }
        public override bool Agregar(NCHNichos pParametro)
        {
            if (pParametro.IdNicho> 0)
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
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdNicho.ToString());
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

        internal bool Agregar(NCHNichos pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdNicho = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "NCHNichosInsertar");
            if (pParametro.IdNicho == 0)
                return false;

            return true;
        }

        public override bool Modificar(NCHNichos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;

            if (!this.Validaciones(pParametro))
                return false;

            NCHNichos valorViejo = new NCHNichos();
            valorViejo.IdNicho = pParametro.IdNicho;
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

                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "NCHNichosActualizar"))
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

        private bool Validaciones(NCHNichos pParametro)
        {
            return true;
        }
    }
}
