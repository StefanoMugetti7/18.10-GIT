using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Afiliados.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Generales.FachadaNegocio;
using Auditoria;
using Auditoria.Entidades;

namespace Afiliados.LogicaNegocio
{
    class AfiCategoriasLN : BaseLN<AfiCategorias>
    {
        public override AfiCategorias ObtenerDatosCompletos(AfiCategorias pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<AfiCategorias>("[AfiCategoriasSeleccionar]", pParametro);
            pParametro.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }
        
        public override List<AfiCategorias> ObtenerListaFiltro(AfiCategorias pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiCategorias>("AfiCategoriasSeleccionarFiltro", pParametro);
        }

        public List<AfiCategorias> ObtenerListaFiltro(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiCategorias>("AfiCategoriasSeleccionarFiltro", pParametro);
        }

        public List<AfiCategorias> ObtenerListaActiva(AfiAfiliados pParametro)
        {
            AfiAfiliados filtro = AyudaProgramacionLN.Clone<AfiAfiliados>(pParametro);            
            filtro.Estado.IdEstado = (int)Estados.Activo;
            return this.ObtenerListaFiltro(filtro);
        }

        public override bool Agregar(AfiCategorias pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            //if (!this.Validar(pParametro))
            //    return false;

            //pParametro.FechaAlta = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdCategoria = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "AfiCategoriasInsertar");
                    if (pParametro.IdCategoria == 0)
                        resultado = false;

                    if (!TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (!TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
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
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
            }
            return resultado;
        }

        public override bool Modificar(AfiCategorias pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //if (!this.Validar(pParametro))
            //    return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            AfiCategorias valorViejo = new AfiCategorias();
            valorViejo.IdCategoria = pParametro.IdCategoria;
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
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "AfiCategoriasActualizar"))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
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
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
            }
            return resultado;
        }
        public List<AfiCategorias> ObtenerTiposCategorias()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiCategorias>("AfiCategoriasObtenerTiposCategorias", new AfiCategorias());
        }
    }
}
