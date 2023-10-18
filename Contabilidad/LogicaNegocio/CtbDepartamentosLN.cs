using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Contabilidad.Entidades;
using Servicio.AccesoDatos;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria;
using Auditoria.Entidades;

namespace Contabilidad.LogicaNegocio
{
    class CtbDepartamentosLN : BaseLN<CtbDepartamentos>
    {
        public override CtbDepartamentos ObtenerDatosCompletos(CtbDepartamentos pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CtbDepartamentos>("CtbDepartamentosSeleccionar", pParametro);
            return pParametro;
        }

        public List<CtbDepartamentos> ObtenerLista(CtbDepartamentos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbDepartamentos>("CtbDepartamentosListar", pParametro);
        }


        public override List<CtbDepartamentos> ObtenerListaFiltro(CtbDepartamentos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbDepartamentos>("CtbDepartamentosSeleccionarFiltro", pParametro);
        }

        public override bool Agregar(CtbDepartamentos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            pParametro.FechaAlta = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdDepartamento = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CtbDepartamentosInsertar");
                    if (pParametro.IdDepartamento == 0)
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

        public override bool Modificar(CtbDepartamentos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CtbDepartamentos depatamentoViejo = new CtbDepartamentos();
            depatamentoViejo.IdDepartamento = pParametro.IdDepartamento;
            depatamentoViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            depatamentoViejo = this.ObtenerDatosCompletos(depatamentoViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = this.Modificar(pParametro, bd, tran);

                    if (resultado && !AuditoriaF.AuditoriaAgregar(depatamentoViejo, Acciones.Update, pParametro, bd, tran))
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

        public bool Modificar(CtbDepartamentos pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbDepartamentosActualizar"))
                return false;

            return true;
        }
    }
}
