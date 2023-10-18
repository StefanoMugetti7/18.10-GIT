using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Facturas.Entidades;
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

namespace Facturas.LogicaNegocio
{
    public class VTASectoresPuntosVentasLN : BaseLN<VTASectoresPuntosVentas>
    {
        public override bool Agregar(VTASectoresPuntosVentas pParametro)
        {

            if (pParametro.IdSectorPuntoVenta > 0)
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
                    //resultado = Validaciones(pParametro);

                    if (resultado && !this.Agregar(pParametro, bd, tran))
                        resultado = false;

                    //if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                    //    resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdSectorPuntoVenta.ToString());
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

        internal bool Agregar(VTASectoresPuntosVentas pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdSectorPuntoVenta = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "VTASectoresPuntosVentasInsertar");
            if (pParametro.IdSectorPuntoVenta == 0)
                return false;

            return true;
        }

        private bool Validaciones(VTASectoresPuntosVentas pParametro)
        {
            return true;
        }
        public override bool Modificar(VTASectoresPuntosVentas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;

            if (!this.Validaciones(pParametro))
                return false;

            VTASectoresPuntosVentas valorViejo = new VTASectoresPuntosVentas();
            valorViejo.IdSectorPuntoVenta = pParametro.IdSectorPuntoVenta;
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
                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "VTASectoresPuntosVentasActualizar"))
                        resultado = false;

                    //if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
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

        public override VTASectoresPuntosVentas ObtenerDatosCompletos(VTASectoresPuntosVentas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<VTASectoresPuntosVentas>("VTASectoresPuntosVentasSeleccionar", pParametro);
        }

        public override List<VTASectoresPuntosVentas> ObtenerListaFiltro(VTASectoresPuntosVentas pParametro)
        {
            throw new NotImplementedException();
        }

        public List<TGESectores> ObtenerSectoresPorFilial(VTASectoresPuntosVentas pParametro,int IdFilial)
        {
            TGEFiliales aux = new TGEFiliales();
            aux.IdFilial = IdFilial;
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TGESectores>("VTASectoresPuntosVentasObtenerSectoresPorFilial", pParametro);
        }

        public DataTable ObtenerListaGrilla(VTASectoresPuntosVentas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("VTASectoresPuntosVentasSeleccionarGrilla", pParametro);
        }
    }
}
