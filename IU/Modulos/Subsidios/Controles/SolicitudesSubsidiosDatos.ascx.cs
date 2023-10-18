using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuentasPagar.Entidades;
using Comunes.Entidades;
using CuentasPagar.FachadaNegocio;
using SKP.ASP.Controls;
using Subsidios.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Subsidios;
using Afiliados;
using System.Globalization;
using Afiliados.Entidades;
using Reportes.FachadaNegocio;

namespace IU.Modulos.Subsidios.Controles
{
    public partial class SolicitudesSubsidiosDatos : ControlesSeguros
    {

        private CapSolicitudPago MiSolicitud
        {
            get { return (CapSolicitudPago)Session[this.MiSessionPagina + "SolicitudDatosMiSolicitud"]; }
            set { Session[this.MiSessionPagina + "SolicitudDatosMiSolicitud"] = value; }
        }

        private List<SubSubsidios> MisSubsidios
        {
            get { return (List<SubSubsidios>)Session[this.MiSessionPagina + "SolicitudDatosMisSubsidios"]; }
            set { Session[this.MiSessionPagina + "SolicitudDatosMisSubsidios"] = value; }
        }

        //private int MiIndiceDetalleModificar
        //{
        //    get { return (int)Session[this.MiSessionPagina + "SolicitudesSubsidiosDatosMiIndiceDetalleModificar"]; }
        //    set { Session[this.MiSessionPagina + "SolicitudesSubsidiosDatosMiIndiceDetalleModificar"] = value; }
        //}

        public delegate void SolicitudPagoDatosAceptarEventHandler(object sender, CapSolicitudPago e);
        public event SolicitudPagoDatosAceptarEventHandler SolicitudPagoModificarDatosAceptar;

        public delegate void SolicitudPagoDatosCancelarEventHandler();
        public event SolicitudPagoDatosCancelarEventHandler SolicitudPagoModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
                if (this.MiSolicitud == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
                this.txtImporte.Attributes.Add("onchange", "CalcularItem();");
            }
        }

        public void IniciarControl(CapSolicitudPago pSolicitud, Gestion pGestion)
        {
            this.IniciarControl(pSolicitud, pGestion, true);
        }

        public void IniciarControl(CapSolicitudPago pSolicitud, Gestion pGestion, bool CargaDatos)
        {
            this.GestionControl = pGestion;
            if (CargaDatos)
                this.CargarCombos();
            this.MiSolicitud = pSolicitud;

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    AyudaProgramacion.HabilitarControles(this.upGeneral, true, this.paginaSegura);
                    this.txtFecha.Enabled = true;
                    this.txtFecha.Text = DateTime.Now.ToShortDateString();
                    this.btnAceptar.Text = this.ObtenerMensajeSistema("btnSiguiente");
                    this.pnlImporte.Visible = false;
                    this.txtImporte.Enabled = false;

                    this.ctrArchivos.IniciarControl(pSolicitud, Gestion.Agregar);
                    break;
                case Gestion.ConfirmarAgregar:
                    //this.MiPrePrestamos.Estado.IdEstado = (int)EstadosPrestamos.Activo;
                    //PrePrestamosF.PrestamosAgregarPrevio(this.MiPrePrestamos);
                    this.MapearObjetoControles(this.MiSolicitud);
                    this.pnlImporte.Visible = true;
                    this.ddlSubsidio.Enabled = false;
                    //this.upCamposDinamicos.EnableViewState = false;
                    this.txtImporte.Enabled = this.MisSubsidios[this.ddlSubsidio.SelectedIndex].ModificaImporte;
                    txtImporteTotal.Enabled = false;
                    this.upGeneral.Update();
                    this.btnAceptar.Text = this.ObtenerMensajeSistema("btnConfirmar");
                    break;
                case Gestion.Anular:
                    this.MiSolicitud = CuentasPagarF.SolicitudPagoObtenerDatosCompletos(this.MiSolicitud);
                    this.MapearObjetoControles(this.MiSolicitud);
                    break;
                case Gestion.Consultar:
                    this.MiSolicitud = CuentasPagarF.SolicitudPagoObtenerDatosCompletos(this.MiSolicitud);
                    this.MapearObjetoControles(this.MiSolicitud);
                    AyudaProgramacion.HabilitarControles(this.UpdatePanel1, false, this.paginaSegura);
                    this.btnAceptar.Visible = false;
                    txtObservacion.Enabled = false;
                    btnImprimir.Visible = true;
                    break;
            }
        }

        private void CargarCombos()
        {
            SubSubsidios filtro = new SubSubsidios();
            filtro.Estado.IdEstado = (int)Estados.Activo;
            this.MisSubsidios = SubsidiosF.SubsidiosObtenerListaFiltro(filtro);
            this.ddlSubsidio.DataSource = this.MisSubsidios;
            this.ddlSubsidio.DataValueField = "IdSubsidio";
            this.ddlSubsidio.DataTextField = "Descripcion";
            this.ddlSubsidio.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlSubsidio, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            PaginaAfiliados pagina = (PaginaAfiliados)this.Page;
            AfiAfiliados afi = pagina.Obtener(this.MiSessionPagina);
            pagina.Guardar(this.MiSessionPagina, AfiliadosF.AfiliadosObtenerDatosCompletos(afi));
            //this.ddlBeneficiario.DataSource = pagina.MiAfiliado.Familiares;
            //this.ddlBeneficiario.DataValueField = "IdAfiliado";
            //this.ddlBeneficiario.DataTextField = "ApellidoNombre";
            //this.ddlBeneficiario.DataBind();
            //this.ddlBeneficiario.Items.Add(new ListItem(pagina.MiAfiliado.ApellidoNombre, pagina.MiAfiliado.IdAfiliado.ToString()));
            //this.ddlBeneficiario.SelectedValue = pagina.MiAfiliado.IdAfiliado.ToString();

            this.ddlFilialPago.DataSource = TGEGeneralesF.FilialesPagosObtenerListaActiva();
            this.ddlFilialPago.DataValueField = "IdFilialPago";
            this.ddlFilialPago.DataTextField = "Filial";
            this.ddlFilialPago.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilialPago, this.ObtenerMensajeSistema("SeleccioneOpcion"));

        }

        protected void ddlSubsidio_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ctrCamposValores.BorrarControlesParametros();
            if (!string.IsNullOrEmpty(this.ddlSubsidio.SelectedValue))
            {
                this.MiSolicitud.Subsidio.IdSubsidio = Convert.ToInt32(this.ddlSubsidio.SelectedValue);
                this.MiSolicitud.Subsidio.Descripcion = this.ddlSubsidio.SelectedItem.Text;
                this.ctrCamposValores.IniciarControl(this.MiSolicitud, this.MiSolicitud.Subsidio, this.GestionControl);
            }
            this.upCamposDinamicos.Update();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.SolicitudPagoModificarDatosAceptar != null)
                this.SolicitudPagoModificarDatosAceptar(null, this.MiSolicitud);
        }

        protected void MapearControlesAObjeto(CapSolicitudPago pSolicitud)
        {
            
            pSolicitud.FechaAlta = Convert.ToDateTime(this.txtFecha.Text);
            pSolicitud.FilialPago.IdFilialPago = this.ddlFilialPago.SelectedValue == string.Empty ? default(int) : Convert.ToInt32(this.ddlFilialPago.SelectedValue);
            pSolicitud.Observacion = this.txtObservacion.Text;
            pSolicitud.IdRefTipoSolicitudPago = Convert.ToInt32(this.ddlSubsidio.SelectedValue);
            pSolicitud.Subsidio.Descripcion = this.ddlSubsidio.SelectedItem.Text;
            pSolicitud.Campos = this.ctrCamposValores.ObtenerLista();
            pSolicitud.Archivos = ctrArchivos.ObtenerLista();
        }

        protected void MapearObjetoControles(CapSolicitudPago pSolicitud)
        {
            this.txtFecha.Text = pSolicitud.FechaAlta.ToShortDateString();
            this.ddlFilialPago.SelectedValue = (pSolicitud.FilialPago.IdFilialPago).ToString();
            this.txtObservacion.Text = pSolicitud.Observacion;
            SubSubsidios subsidio = pSolicitud.SolicitudPagoDetalles[0].Subsidio;
            ListItem item = this.ddlSubsidio.Items.FindByValue(subsidio.IdSubsidio.ToString());
            if (item == null)
                this.ddlSubsidio.Items.Add(new ListItem(subsidio.Descripcion, subsidio.IdSubsidio.ToString()));
            this.ddlSubsidio.SelectedValue = subsidio.IdSubsidio.ToString();
            this.txtImporte.Decimal = pSolicitud.SolicitudPagoDetalles[0].PrecioUnitarioSinIva;
            this.txtImporteTotal.Decimal = pSolicitud.ImporteTotal;
            
            if (this.GestionControl == Gestion.ConfirmarAgregar)
                this.ctrCamposValores.IniciarControl(this.MiSolicitud, subsidio, Gestion.Consultar);
            else
                this.ctrCamposValores.IniciarControl(this.MiSolicitud, subsidio, this.GestionControl);

            this.ctrArchivos.IniciarControl(pSolicitud, this.GestionControl);

        }

        protected void btnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            

                TGEPlantillas plantilla = new TGEPlantillas();
                plantilla.Codigo = "SolicitudSubsidios";
                plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);


                UsuarioLogueado usulog = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.SinComprobante, plantilla.Codigo, MiSolicitud, usulog);
                ExportPDF.ExportarPDF(pdf, this.Page, usulog);
            
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            //this.Page.Validate("ValidadorControlesDinamicos");
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiSolicitud);
            this.MiSolicitud.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    
                    this.MiSolicitud.TipoSolicitudPago.IdTipoSolicitudPago = (int)EnumTiposSolicitudPago.Subsidios;

                    CapSolicitudPagoDetalles item = new CapSolicitudPagoDetalles();
                        item.Subsidio.IdSubsidio = Convert.ToInt32(this.ddlSubsidio.SelectedValue);
                        item.Descripcion = this.ddlSubsidio.Text;
                        item.EstadoColeccion = EstadoColecciones.Agregado;
                        item.Estado.IdEstado = (int)Estados.Activo;
                        if (this.MiSolicitud.SolicitudPagoDetalles.Count > 0)
                            this.MiSolicitud.SolicitudPagoDetalles[0] = item;
                        else
                            this.MiSolicitud.SolicitudPagoDetalles.Add(item);

                        if(Convert.ToDateTime(txtFecha.Text) > DateTime.Now)
                    {
                        this.MostrarMensaje("La fecha no puede ser mayor al Dia de la Fecha", true);
                        return;
                    }

                    if (CuentasPagarF.SolicitudPagoSubsidioObtenerImporteLiquidacion(this.MiSolicitud))
                    {
                        this.IniciarControl(this.MiSolicitud, Gestion.ConfirmarAgregar, false);
                    }
                    else
                        this.MostrarMensaje(this.MiSolicitud.CodigoMensaje, true, this.MiSolicitud.CodigoMensajeArgs);
                    return;
                //break;
                case Gestion.ConfirmarAgregar:                    
                    this.MiSolicitud.IdUsuarioAlta = this.UsuarioActivo.IdUsuarioEvento;
                    this.MiSolicitud.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;;
                    this.MiSolicitud.FechaContable = this.MiSolicitud.FechaAlta;
                    this.MiSolicitud.SolicitudPagoDetalles[0].PrecioUnitarioSinIva = this.txtImporte.Decimal;// == string.Empty ? 0 : Convert.ToDecimal(this.txtImporte.Text);
                    this.MiSolicitud.ImporteTotal = this.MiSolicitud.SolicitudPagoDetalles.Sum(x => x.PrecioTotalItem);
                    this.MiSolicitud.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.SolicitudesPagosSubsidios;
                    this.MiSolicitud.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(this.MiSolicitud.TipoOperacion);
                    this.MiSolicitud.TipoSolicitudPago.IdTipoSolicitudPago = (int)EnumTiposSolicitudPago.Subsidios;
                    guardo = CuentasPagarF.SolicitudPagoAgregar(this.MiSolicitud, new global::Compras.Entidades.CmpInformesRecepciones());
                    break;
                case Gestion.Anular:
                    this.MiSolicitud.Estado.IdEstado = (int)Estados.Baja;
                    this.MiSolicitud.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.SolicitudesPagosSubsidios;
                    guardo = CuentasPagarF.SolicitudPagoAnular(this.MiSolicitud);
                    break;
                default:
                    break;
            }
            if (guardo)
            {

                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiSolicitud.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiSolicitud.CodigoMensaje, true, this.MiSolicitud.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.GestionControl == Gestion.ConfirmarAgregar)
            {
                this.pnlImporte.Visible = false;
                this.ddlSubsidio.Enabled = true;
                AyudaProgramacion.HabilitarControles(this.upCamposDinamicos, true, this.paginaSegura);
                this.txtImporte.Enabled = false;
                this.upGeneral.Update();
                this.IniciarControl(this.MiSolicitud, Gestion.Agregar, false);
            }
            else
            {
                if (this.SolicitudPagoModificarDatosCancelar != null)
                    this.SolicitudPagoModificarDatosCancelar();
            }
        }

    }
}