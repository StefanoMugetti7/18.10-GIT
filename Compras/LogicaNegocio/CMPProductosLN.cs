using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Comunes.Entidades;
using Servicio.AccesoDatos;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Generales.FachadaNegocio;
using Auditoria;
using System.Collections;
using Compras.Entidades;
using Comunes.LogicaNegocio;
using Auditoria.Entidades;
using System.Data;

namespace Compras.LogicaNegocio
{
    public class CMPProductosLN : BaseLN<CMPProductos>
    {
        public CMPProductos ObtenerPorIdProducto(CMPProductos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CMPProductos>("CMPProductosSeleccionar", pParametro);
            //List<CMPProductos> lista = this.ObtenerListaFiltro(pParametro);
            //if (lista.Count == 1)
            //    return lista[0];
            
            //else
            //    return new CMPProductos();
        }

        public CMPProductos ObtenerPorCodigo(CMPProductos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CMPProductos>("CMPProductosSeleccionarPorCodigo", pParametro);
            //List<CMPProductos> lista = this.ObtenerListaFiltro(pParametro);
            //if (lista.Count == 1)
            //    return lista[0];

            //else
            //    return new CMPProductos();
        }

        public override List<CMPProductos> ObtenerListaFiltro(CMPProductos pParametro)
        {
            //TODO
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CMPProductos>("CMPProductosSeleccionarFiltros", pParametro);
        }

        public DataTable ObtenerListaFiltroDT(CMPProductos pParametro)
        {
            //TODO
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CMPProductosSeleccionarFiltros", pParametro);
        }

        public DataTable ObtenerProductosServiciosTurismo(CMPProductos pParametro)
        {
            //TODO
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CMPProductosSeleccionarServiciosTurismo", pParametro);
        }

        public DataTable ObtenerGrilla(CMPProductos pParametro)
        {
            //TODO
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CMPProductosSeleccionarFiltros", pParametro);
        }

        public override bool Agregar(CMPProductos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            if (!this.Validar(pParametro))
                return false;

            //pParametro.FechaAlta = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {

                    pParametro.IdProducto = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CMPProductosInsertar");
                    if (pParametro.IdProducto == 0)
                        resultado = false;

                    //if (resultado && !this.DomiciliosActualizar(pParametro, new CMPProductos(), bd, tran))
                    //    resultado = false;

                    //if (resultado && !this.TelefonoActualizar(pParametro, new CMPProductos(), bd, tran))
                    //    resultado = false;

                    //if (resultado && !this.AlertasTiposActualizar(pParametro, new CMPProductos(), bd, tran))
                    //    resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

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

        private bool Validar(CMPProductos pParametro)
        {

            switch (pParametro.EstadoColeccion)
            {
                case EstadoColecciones.SinCambio:
                    break;
                case EstadoColecciones.Agregado:
                    
                    break;
                case EstadoColecciones.AgregadoBorradoMemoria:
                    break;
                case EstadoColecciones.Borrado:
                    break;
                case EstadoColecciones.Modificado:
                  

                    break;
                default:
                    break;
            }
            return true;
        }

        public override bool Modificar(CMPProductos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CMPProductos valorViejo = new CMPProductos();
            valorViejo.IdProducto = pParametro.IdProducto;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerPorIdProducto(valorViejo);

            if (!this.Validar(pParametro))
                return false;

            //this.ActualizarFechaCambioEstado(pParametro, valorViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CMPProductosActualizar"))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

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

        public override CMPProductos ObtenerDatosCompletos(CMPProductos pParametro)
        {
            CMPProductos producto= BaseDatos.ObtenerBaseDatos().Obtener<CMPProductos>("CMPProductosSeleccionarDescripcion", pParametro);
            return producto;
        }

        public DataTable ObtenerListaCompradores(CMPProductos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CmpProductosSeleccionarUltimasCompras", pParametro);
        }
    }
}
