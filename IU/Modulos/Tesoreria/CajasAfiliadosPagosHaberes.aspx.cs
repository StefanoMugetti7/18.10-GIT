using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tesorerias.Entidades;
using Cargos.Entidades;
using Haberes;
using Haberes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using Tesorerias;
using Reportes.Entidades;

namespace IU.Modulos.Tesoreria
{
    public partial class CajasAfiliadosPagosHaberes : PaginaCajasAfiliados
    {
        protected CarCuentasCorrientes MiRefTipoOperacion
        {
            get
            {
                if (Session[this.MiSessionPagina + "CajasAfiliadosExtraerMiRefTipoOperacion"] != null)
                    return (CarCuentasCorrientes)Session[this.MiSessionPagina + "CajasAfiliadosExtraerMiRefTipoOperacion"];
                else
                {
                    return (CarCuentasCorrientes)(Session[this.MiSessionPagina + "CajasAfiliadosExtraerMiRefTipoOperacion"] = new CarCuentasCorrientes());
                }
            }
            set { Session[this.MiSessionPagina + "CajasAfiliadosExtraerMiRefTipoOperacion"] = value; }
        }

        protected HabRecibosCom MiReciboCom
        {
            get
            {
                if (Session[this.MiSessionPagina + "CajasAfiliadosExtraerMiReciboCom"] != null)
                    return (HabRecibosCom)Session[this.MiSessionPagina + "CajasAfiliadosExtraerMiReciboCom"];
                else
                {
                    return (HabRecibosCom)(Session[this.MiSessionPagina + "CajasAfiliadosExtraerMiReciboCom"] = new HabRecibosCom());
                }
            }
            set { Session[this.MiSessionPagina + "CajasAfiliadosExtraerMiReciboCom"] = value; }
        }

        protected override void PageLoadEventCajasAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventCajasAfiliados(sender, e);
            //this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
                string mensaje = this.ObtenerMensajeSistema("CajaExtraerConfirmar");
                //mensaje = string.Format(mensaje, string.Concat(item.Afiliado.ApellidoNombre));
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                this.btnAceptar.Attributes.Add("OnClick", funcion);

                this.MapearObjetoAControles();
            }
        }

        private void MapearObjetoAControles()
        {
            this.MiRefTipoOperacion = new CarCuentasCorrientes();
            this.MiRefTipoOperacion.IdAfiliado = this.MiAfiliado.IdAfiliado;

            this.MiReciboCom = new HabRecibosCom();
            this.MiReciboCom.IdAfiliado = this.MiAfiliado.IdAfiliado;
            this.MiReciboCom = HaberesF.HabRecibosComObtenerUltimoRecibo(this.MiReciboCom); //CargosF.CuentasCorrientesObtenerSaldoActual(this.MiRefTipoOperacion);
            this.MiRefTipoOperacion.SaldoActual = this.MiReciboCom.NetoPagar;

            if (this.MiRefTipoOperacion.SaldoActual < 0)
                this.MiRefTipoOperacion.SaldoActual = 0;
            this.txtSaldoActual.Text = this.MiRefTipoOperacion.SaldoActual.ToString("C2");
            this.txtImporteExtraer.Text = this.MiRefTipoOperacion.SaldoActual.ToString("N2");

        }

        private void MapearControlesAObjeto()
        {
            TGETiposOperaciones tipoOpe = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.PagoHaberes);

            //this.MiRefTipoOperacion = new CarCuentasCorrientes();
            this.MiRefTipoOperacion.IdAfiliado = this.MiAfiliado.IdAfiliado;
            this.MiRefTipoOperacion.Concepto = tipoOpe.TipoOperacion;
            this.MiRefTipoOperacion.Estado.IdEstado = (int)EstadosCuentasCorrientes.Activo;
            this.MiRefTipoOperacion.EstadoColeccion = EstadoColecciones.Agregado;
            this.MiRefTipoOperacion.FechaMovimiento = DateTime.Now;
            this.MiRefTipoOperacion.Periodo = this.MiReciboCom.Periodo;
            this.MiRefTipoOperacion.Importe = Convert.ToDecimal(this.txtImporteExtraer.Text);
            this.MiRefTipoOperacion.TipoOperacion = tipoOpe;
            this.MiRefTipoOperacion.IdRefTipoOperacion = this.MiReciboCom.IdReciboCom;
            this.MiRefTipoOperacion.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
            this.MiRefTipoOperacion.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.MiRefTipoOperacion.Filial = this.UsuarioActivo.FilialPredeterminada;

            this.MiCajaMovimientoPendiente = new TESCajasMovimientos();
            this.MiCajaMovimientoPendiente.CajaMoneda.Moneda.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;
            this.MiCajaMovimientoPendiente.Afiliado = this.MiAfiliado;
            this.MiCajaMovimientoPendiente.Fecha = DateTime.Now;
            this.MiCajaMovimientoPendiente.Importe = Convert.ToDecimal(this.txtImporteExtraer.Text);
            this.MiCajaMovimientoPendiente.Estado.IdEstado = (int)EstadosCajasMovimientos.Activo;
            this.MiCajaMovimientoPendiente.TipoOperacion = tipoOpe;
            this.MiCajaMovimientoPendiente.IdRefTipoOperacion = this.MiReciboCom.IdReciboCom;
            
            //INICIO SOLO PARA EFECTIVO
            TESCajasMovimientosValores movValor = new TESCajasMovimientosValores();
            movValor.EstadoColeccion = EstadoColecciones.Agregado;
            movValor.Estado.IdEstado = (int)Estados.Activo;
            movValor.Importe = Convert.ToDecimal(this.txtImporteExtraer.Text);
            movValor.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
            this.MiCajaMovimientoPendiente.CajasMovimientosValores.Add(movValor);
            //FIN SOLO PARA EFECTIVO
            this.MiCaja.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Validate("Aceptar");
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto();

            if (!TesoreriasF.CajasConfirmarMovimiento(this.MiCaja, this.MiCajaMovimientoPendiente, this.MiRefTipoOperacion))
            {
                this.MostrarMensaje(this.MiCaja.CodigoMensaje, true, this.MiCaja.CodigoMensajeArgs);
            }
            else
            {
                //this.popUpMensajes.MostrarMensaje(this.MiCaja.CodigoMensaje);
                this.btnAceptar.Visible = false;
                this.MostrarMensaje(this.MiCaja.CodigoMensaje, false);
                //Object comprobante = Enum.Parse(typeof(EnumTGEComprobantes), this.MiRefTipoOperacion.GetType().Name);
                //EnumTGETiposOperaciones tipoOperacion = Enum.ToObject(typeof(EnumTGETiposOperaciones), this.MiCajaMovimientoPendiente.TipoOperacion.IdTipoOperacion);
                //if (comprobante != null)
                //{
                //    this.btnImprimir.Visible = true;
                //    this.CargarReporte(this.MiRefTipoOperacion, (EnumTGEComprobantes)comprobante, true);
                //    //this.ctrPopUpComprobantes.CargarReporte(this.MiRefTipoOperacion, (EnumTGEComprobantes)comprobante);
                //}
                //else
                //{
                //this.popUpMensajes.MostrarMensaje(this.MiCaja.CodigoMensaje);
                //}
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.MiCajaMovimientoPendiente = new TESCajasMovimientos();
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasAfiliadosInicio.aspx"), true);
        }

        protected void btnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            RepReportes reporte = new RepReportes();
            //reporte.IdReporte = (int)EnumReportes.ReciboCOM;
            //reporte = ReportesF.ReportesObtenerUno(reporte);
            RepParametros parametro = new RepParametros();
            parametro.Parametro = "Periodo";
            parametro.ValorParametro = this.MiReciboCom.Periodo;
            parametro.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            reporte.Parametros.Add(parametro);
            parametro = new RepParametros();
            parametro.Parametro = "IdAfiliado";
            parametro.ValorParametro = this.MiReciboCom.IdAfiliado;
            parametro.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            reporte.Parametros.Add(parametro);
            parametro = new RepParametros();
            parametro.Parametro = "IdTipoRecibo";
            parametro.ValorParametro = this.MiReciboCom.IdTipoRecibo;
            parametro.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            reporte.Parametros.Add(parametro);

            HabArchivosDetalles archivo = new HabArchivosDetalles();
            archivo.IdAfiliado = this.MiAfiliado.IdAfiliado;

            int perido = this.MiReciboCom.Periodo;
            archivo.ArchivoCabecera.Anio = Convert.ToInt32(perido.ToString().Substring(0, 4));
            archivo.ArchivoCabecera.Mes = Convert.ToInt32(perido.ToString().Substring(4, 2));
            archivo.ArchivoCabecera.RemesaTipo.IdRemesaTipo = this.MiReciboCom.IdTipoRecibo;
            List<HabArchivosDetalles> reporteIAF = HaberesF.ArchivosDetallesSeleccionar(archivo);
            if (reporteIAF.Count > 0)
            {
                //this.btnAceptar.Enabled=true;
                this.ctrPopUpComprobantes.CargarReporte(reporteIAF, reporte, EnumTGEComprobantes.ReciboCOM);
            }
            else
                this.MostrarMensaje("ArchivoReciboNoExiste", true);
        }

        //protected void popUpMensajes_popUpMensajesPostBackAceptar()
        //{
        //    this.MiCajaMovimientoPendiente = new TESCajasMovimientos();
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosAutomaticos.aspx"), true);
        //}

    }
}