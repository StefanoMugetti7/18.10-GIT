using Afiliados.Entidades;
using Afiliados.Entidades.Entidades;
using Auditoria;
using Auditoria.Entidades;
using Comunes;
using Comunes.LogicaNegocio;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afiliados.LogicaNegocio
{
    class AfiAfiliadosDatosUIFLN : BaseLN<AfiAfiliadosDatosUIF>
    {
        public override bool Agregar(AfiAfiliadosDatosUIF pParametro)
        {

            if (pParametro.IdAfiliadoDatosUIF > 0)
                return false;

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
                    if (resultado && !this.Agregar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        foreach (AfiAfiliadosMatrizRiesgo item in pParametro.MatricesDeRiesgo)
                        {
                            item.IdAfiliado = pParametro.IdAfiliado;
                            item.UsuarioLogueado = pParametro.UsuarioLogueado;
                            if(item.IdMatriz > 0)
                            {
                                item.IdAfiliadoMatriz = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "AfiAfiliadosMatricesRiesgosInsertar");
                                if (item.IdAfiliadoMatriz == 0)
                                {
                                    resultado = false;
                                    break;
                                }
                            }
                        }
                    }

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdAfiliadoDatosUIF.ToString());
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
        internal bool Agregar(AfiAfiliadosDatosUIF pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdAfiliadoDatosUIF = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "AfiAfiliadosDatosUIFInsertar");
            if (pParametro.IdAfiliadoDatosUIF == 0)
                return false;

            return true;
        }
        private bool Validaciones(AfiAfiliadosDatosUIF pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "AfiAfiliadosDatosUIFValidaciones");
        }

        public override bool Modificar(AfiAfiliadosDatosUIF pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;

            if (!this.Validaciones(pParametro))
                return false;

            AfiAfiliadosDatosUIF valorViejo = new AfiAfiliadosDatosUIF();
            valorViejo.IdAfiliadoDatosUIF = pParametro.IdAfiliadoDatosUIF;
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
                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "AfiAfiliadosDatosUIFActualizar"))
                        resultado = false;

                    if (resultado)
                    {
                        foreach (AfiAfiliadosMatrizRiesgo item in pParametro.MatricesDeRiesgo)
                        {
                            item.UsuarioLogueado = pParametro.UsuarioLogueado;
                            item.IdAfiliado = pParametro.IdAfiliado;
                            if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "AfiAfiliadosMatricesRiesgosActualizar"))
                            {
                                resultado = false;
                                break;
                            }
                        }
                    }

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
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

  

        public override List<AfiAfiliadosDatosUIF> ObtenerListaFiltro(AfiAfiliadosDatosUIF pParametro)
        {
            throw new NotImplementedException();
        }

        public override AfiAfiliadosDatosUIF ObtenerDatosCompletos(AfiAfiliadosDatosUIF pParametro)
        {
            AfiAfiliadosDatosUIF retorno = BaseDatos.ObtenerBaseDatos().Obtener<AfiAfiliadosDatosUIF>("AfiAfiliadosDatosUIFSeleccionarPorIdAfiliado", pParametro);
            retorno.MatricesDeRiesgo = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiAfiliadosMatrizRiesgo>("AfiAfiliadosMatricesSeleccionar", retorno);
            retorno.Comentarios = TGEGeneralesF.ComentariosObtenerLista(retorno);
            retorno.Archivos = TGEGeneralesF.ArchivosObtenerLista(retorno);
            return retorno;
        }
        public AfiAfiliadosDatosUIF ObtenerDatosCompletosPorIdAfiliado(AfiAfiliadosDatosUIF pParametro)
        {
            AfiAfiliadosDatosUIF retorno = BaseDatos.ObtenerBaseDatos().Obtener<AfiAfiliadosDatosUIF>("AfiAfiliadosDatosUIFSeleccionarPorIdAfiliado", pParametro);
            
            retorno.MatricesDeRiesgo = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiAfiliadosMatrizRiesgo>("AfiAfiliadosMatricesSeleccionar", retorno);
            retorno.Comentarios = TGEGeneralesF.ComentariosObtenerLista(retorno);
            retorno.Archivos = TGEGeneralesF.ArchivosObtenerLista(retorno);
            return retorno;
        }

        public DataTable ObtenerMatricesDeRiesgo(AfiAfiliadosDatosUIF pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("AfiAfiliadosDatosUIFObtenerMatricesRiesgo", pParametro);
        }
    }
}
