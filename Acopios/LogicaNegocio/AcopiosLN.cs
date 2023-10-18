using Acopios.Entidades;
using Auditoria;
using Auditoria.Entidades;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acopios.LogicaNegocio
{
    class AcopiosLN : BaseLN<AcpAcopios>
    {
        public override bool Agregar(AcpAcopios pParametro)
        {
            if (pParametro.IdAcopio > 0)
                return true;

            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.Estado.IdEstado = (int)Estados.Activo;

            if (!this.Validaciones(pParametro))
                return false;

            DbTransaction tran;
            Database bd = DatabaseFactory.CreateDatabase();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "AcpAcopiosValidaciones");

                    if (resultado)
                    {
                        pParametro.IdAcopio = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "AcpAcopiosInsertar");
                        if (pParametro.IdAcopio == 0)
                            resultado = false;
                    }

                    if (resultado && !this.ItemsActualizar(pParametro, new AcpAcopios(), bd, tran))
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
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdAcopio.ToString());
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionPolicy.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }

            }
            return resultado;
        }

        public override bool Modificar(AcpAcopios pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (!this.Validaciones(pParametro))
                return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            AcpAcopios valorViejo = new AcpAcopios();
            valorViejo.IdAcopio = pParametro.IdAcopio;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            DbTransaction tran;
            Database bd = DatabaseFactory.CreateDatabase();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "AcpAcopiosValidaciones");

                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "AcpAcopiosActualizar"))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ItemsActualizar(pParametro, valorViejo, bd, tran))
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
                    ExceptionPolicy.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        private bool ItemsActualizar(AcpAcopios pParametro, AcpAcopios pValorViejo, Database bd, DbTransaction tran)
        {
            foreach (AcpAcopiosImportes item in pParametro.AcopiosImportes.Where(x=>x.IdRefTabla > 0))
            {
                switch (item.EstadoColeccion)
                {
                    #region Agregado
                    case EstadoColecciones.Agregado:
                        item.UsuarioLogueado = pParametro.UsuarioLogueado;
                        item.IdAcopio = pParametro.IdAcopio;
                        item.IdAcopioImporte = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "AcpAcopiosImportesInsertar");
                        if (item.IdAcopioImporte == 0)
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        break;
                    #endregion

                    #region Modificado
                    case EstadoColecciones.Modificado:
                    case EstadoColecciones.Borrado:
                        item.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "AcpAcopiosImportesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.AcopiosImportes.Find(x => x.IdAcopioImporte == item.IdAcopioImporte), Acciones.Update, item, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
                
            } 
            return true;
        }

        private bool Validaciones(AcpAcopios pParametro)
        {
            if (pParametro.AcopiosImportes.Count(x => x.Estado.IdEstado == (int)Estados.Activo) == 0)
            {
                pParametro.CodigoMensaje = "ValidarAcopiosImportesItemsCantidad";
                return false;
            }

            return true;
        }

        public override AcpAcopios ObtenerDatosCompletos(AcpAcopios pParametro)
        {
            AcpAcopios acopio = BaseDatos.ObtenerBaseDatos().Obtener<AcpAcopios>("AcpAcopiosSeleccionar", pParametro);
            acopio.AcopiosImportes = BaseDatos.ObtenerBaseDatos().ObtenerLista<AcpAcopiosImportes>("AcpAcopiosImportesSeleccionarPorIdAcopio", pParametro);
            return acopio;
        }

        public DataTable ObtenerListaGrilla(AcpAcopios pParametro)
        {
            DataTable dt= new DataTable();
            if(pParametro.Tabla == "CapProveedores")
                dt = BaseDatos.ObtenerBaseDatos().ObtenerLista("AcpAcopiosSeleccionarProveedor", pParametro);
            else if (pParametro.Tabla == "AfiClientes")
                dt = BaseDatos.ObtenerBaseDatos().ObtenerLista("AcpAcopiosSeleccionarCliente", pParametro);
            return dt;
        }

        public override List<AcpAcopios> ObtenerListaFiltro(AcpAcopios pParametro)
        {
            throw new NotImplementedException();
        }
    }
}
