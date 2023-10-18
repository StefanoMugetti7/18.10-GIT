using Auditoria;
using Auditoria.Entidades;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prestamos.Entidades;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prestamos.LogicaNegocio
{
    class PrePrestamosLotesLN : BaseLN<PrePrestamosLotes>
    {
        public override bool Agregar(PrePrestamosLotes pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            
            bool resultado = true;

            if (pParametro.IdPrestamoLote > 0)
                return true;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            pParametro.FechaAlta = DateTime.Now;

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdPrestamoLote = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "PrePrestamosLotesInsertar");
                    if (pParametro.IdPrestamoLote == 0)
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

        public override bool Modificar(PrePrestamosLotes pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            PrePrestamosLotes valorViejo = new PrePrestamosLotes();
            valorViejo.IdPrestamoLote = pParametro.IdPrestamoLote;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = ObtenerDatosCompletos(valorViejo);


            pParametro.DetalleSeleccionado.TableName = "PrestamosLotesSeleccionados";
            DataView view = new DataView(pParametro.DetalleSeleccionado);
            view.RowStateFilter = DataViewRowState.Added | DataViewRowState.ModifiedCurrent;
            DataTable newTable = view.ToTable();
            newTable.TableName = "PrestamosLotesSeleccionados";
            pParametro.LotePrestamosLotes = newTable.ToXmlDocument();

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "PrePrestamosLotesActualizar");

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    //if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    //if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    //if (resultado && !this.ItemsActualizar(pParametro, valorViejo, bd, tran))
                    //    resultado = false;

                    //if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                    //    resultado = false;

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

        public override PrePrestamosLotes ObtenerDatosCompletos(PrePrestamosLotes pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<PrePrestamosLotes>("PrePrestamosLotesSeleccionar", pParametro);
            pParametro.DetalleSeleccionado = BaseDatos.ObtenerBaseDatos().ObtenerLista("PrePrestamosLotesDetallesSeleccionar", pParametro);
            return pParametro;
        }

        public override List<PrePrestamosLotes> ObtenerListaFiltro(PrePrestamosLotes pParametro)
        {
            throw new NotImplementedException();
        }

        public DataTable ObtenerPorProveedor(PrePrestamosLotes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("PrePrestamosLotesSeleccionarPrestamosPorInversor", pParametro);
        }

        public DataTable ObtenerPorFiltro(PrePrestamosLotes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("PrePrestamosLotesSeleccionarPrestamosDisponiblesPorFiltro", pParametro);
        }

        public DataTable ObtenerGrilla(PrePrestamosLotes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("PrePrestamosLotesSeleccionarGrilla", pParametro);
        }
    }
}
