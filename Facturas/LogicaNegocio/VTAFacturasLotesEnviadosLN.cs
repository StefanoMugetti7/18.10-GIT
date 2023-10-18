using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facturas.Entidades;
using System.Data;
using Servicio.AccesoDatos;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Generales.FachadaNegocio;
using Generales.Entidades;

namespace Facturas.LogicaNegocio
{
    public class VTAFacturasLotesEnviadosLN
    {
        public delegate void ProcesosDatosEjecutarSPMensajes(List<string> e);
        public event ProcesosDatosEjecutarSPMensajes ProcesoDatosEjecutarSPMensajesCallback;

        public List<string> mensajesBasedatos;

        public bool Validaciones(VTAFacturasLotesEnviados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "VTAFacturasLotesEnviadosValidaciones");
        }

        public DataTable ObtenerGrilla(VTAFacturasLotesEnviados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("VtaFacturasLotesEnviadosSeleccionar", pParametro);
        }

        public DataTable ObtenerFacturasGrilla(VTAFacturasLotesEnviados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("VtaFacturasLotesEnviadosFacturasSeleccionar", pParametro);
        }

        public List<VTAFacturasLotesEnviados> ObtenerCombo()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<VTAFacturasLotesEnviados>("VTAFacturasLotesEnviadosSeleccionarCombo");
        }

        public DataSet AgregarDevolverLote(VTAFacturasLotesEnviados pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            return BaseDatos.ObtenerBaseDatos().ObtenerDataSet("VTAFacturasLotesEnviadosFacturasInsertar", pParametro);
        }

        public bool ConfirmarLote(VTAFacturasLotesEnviados pParametro)
        {
            bool resultado = true;
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            int tiempoEspera = Convert.ToInt32(TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.DBTiempoEjecucionProcesosDatos).ParametroValor);

            BaseDatos accesoDatos = BaseDatos.ObtenerBaseDatos();
            mensajesBasedatos = new List<string>();
            accesoDatos.BaseDatosEjecutarSPMensajesCallback += new BaseDatos.BaseDatosEjecutarSPMensajes(bd_BaseDatosEjecutarSPMensajesCallback);
            pParametro.IdFacturaLoteEnviado = accesoDatos.EjecutarSPMensajes(pParametro, "VTAFacturasLotesEnviadosConfirmar", tiempoEspera);

            if (pParametro.IdFacturaLoteEnviado == 0)
            {
                resultado = false;
            }
            else
            {
                if (pParametro.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta == (int)EnumAFIPTiposPuntosVentas.WebServiceFacturaElectronica)
                    pParametro.CodigoMensaje = "FacturasLotesResultadoFE";
                else
                    pParametro.CodigoMensaje = "FacturasLotesResultado";

                pParametro.CodigoMensajeArgs = new List<string> { pParametro.IdFacturaLoteEnviado.ToString() };
            }

            return resultado;
        }

        public void bd_BaseDatosEjecutarSPMensajesCallback(string e)
        {
            this.mensajesBasedatos.Add(e);
            if (this.ProcesoDatosEjecutarSPMensajesCallback != null)
                this.ProcesoDatosEjecutarSPMensajesCallback(this.mensajesBasedatos);
        }

        public List<TGEListasValoresDetalles> ObtenerCargosPorPeriodo(VTAFacturasLotesEnviados pParametros)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEListasValoresDetalles>("CarCuentasCorrientesSeleccionarCargosPorPeriodo", pParametros);
        }

    }
}
