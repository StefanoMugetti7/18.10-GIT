using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using CuentasPagar.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Proveedores.Entidades;
using Proveedores;
using Compras.Entidades;
using CuentasPagar.FachadaNegocio;
using Compras;
using Cargos.Entidades;
using Contabilidad;
using Contabilidad.Entidades;
using Evol.Controls;
using Facturas.Entidades;
using Facturas;
using CuentasPagar.LogicaNegocio;
using Reportes.FachadaNegocio;

namespace IU.Modulos.CuentasPagar.Controles
{
    public partial class SolicitudPagoDatos : ControlesSeguros
    {
        const int CantidadItems = 2;

        public int MiCantidadDecimales 
        {
            get { return (int)Session[this.MiSessionPagina + "SolicitudDatosCantidadDecimales"]; }
            set { Session[this.MiSessionPagina + "SolicitudDatosCantidadDecimales"] = value; }
        }

        private int idTipoOperacionAsiento = 0;

        private CapSolicitudPago MiSolicitud
        {
            get { return (CapSolicitudPago)Session[this.MiSessionPagina + "SolicitudDatosMiSolicitud"]; }
            set { Session[this.MiSessionPagina + "SolicitudDatosMiSolicitud"] = value; }
        }

        private CapSolicitudPago MiSolicitudSeleccionada
        {
            get { return (CapSolicitudPago)Session[this.MiSessionPagina + "SolicitudDatosMiSolicitudSeleccionada"]; }
            set { Session[this.MiSessionPagina + "SolicitudDatosMiSolicitudSeleccionada"] = value; }
        }

        //private CapProveedores MiProveedor
        //{
        //    get { return (CapProveedores)Session[this.MiSessionPagina + "SolicitudPagoMiProveedor"]; }
        //    set { Session[this.MiSessionPagina + "SolicitudPagoMiProveedor"] = value; }
        //}
        private List<TGEIVA> MisIvas
        {
            get { return (List<TGEIVA>)Session[this.MiSessionPagina + "SolicitudesPagosDatosMisIvas"]; }
            set { Session[this.MiSessionPagina + "SolicitudesPagosDatosMisIvas"] = value; }
        }
        
        private List<TGETiposFacturas> MisTiposFacturas
        {
            get { return (List<TGETiposFacturas>)Session[this.MiSessionPagina + "FacturasDatosMisTiposFacturas"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMisTiposFacturas"] = value; }
        }

        private List<CtbCentrosCostosProrrateos> MisCentrosCostos
        {
            get { return (List<CtbCentrosCostosProrrateos>)Session[this.MiSessionPagina + "SolicitudesPagosDatosMisCentrosCostos"]; }
            set { Session[this.MiSessionPagina + "SolicitudesPagosDatosMisCentrosCostos"] = value; }
        }
        
        private List<TGEFiliales> MisFiliales
        {
            get { return (List<TGEFiliales>)Session[this.MiSessionPagina + "SolicitudesPagosDatosMisFiliales"]; }
            set { Session[this.MiSessionPagina + "SolicitudesPagosDatosMisFiliales"] = value; }
        }

        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "SolicitudModificarDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "SolicitudModificarDatosMiIndiceDetalleModificar"] = value; }
        }

        private List<TGEListasValoresSistemasDetalles> MisPercepciones
        {
            get { return (List<TGEListasValoresSistemasDetalles>)Session[this.MiSessionPagina + "SolicitudesPagosDatosMisPercepciones"]; }
            set { Session[this.MiSessionPagina + "SolicitudesPagosDatosMisPercepciones"] = value; }
        }

        public delegate void SolicitudPagoDatosAceptarEventHandler(object sender, CapSolicitudPago e);
        public event SolicitudPagoDatosAceptarEventHandler SolicitudPagoModificarDatosAceptar;

        public delegate void SolicitudPagoDatosCancelarEventHandler();
        public event SolicitudPagoDatosCancelarEventHandler SolicitudPagoModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            //this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.ctrImportarRemito.InformesBuscarSeleccionar += new CuentasPagar.Controles.ImportarRemitoPopUp.InformesBuscarEventHandler(ctrImportarRemito_InformesBuscarSeleccionar);
            this.ctrCentrosCostos.ControlDatosAceptar += new Contabilidad.Controles.CentrosCostosPrrorrateosDatosPopUp.AsientoContableDatosAceptarEventHandler(ctrCentrosCostos_ControlDatosAceptar);
            ctrBuscarProveedor.BuscarProveedor += new Proveedores.Controles.ProveedoresCabecerasDatos.ProveedoresDatosCabeceraAjaxEventHandler(CtrBuscarProveedor_BuscarProveedor);
            ctrBuscarSolPago.ControlBuscarSeleccionar += new CuentasPagar.Controles.SolicitudPagoBuscarPopUp.ControlBuscarEventHandler(ctrBuscarSolPago_ControlBuscarSeleccionar);
            //this.ctrCentrosCostos.ControlDatosCancelar += new Contabilidad.Controles.CentrosCostosPrrorrateosDatosPopUp.ControlDatosCancelarEventHandler(ctrCentrosCostos_ControlDatosCancelar);
            if (!this.IsPostBack)
            {
                if (this.MiSolicitud == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
                this.txtDescuentoTotal.Attributes.Add("onchange", "CalcularItem();");

            }
            else
            {
                if (GestionControl == Gestion.Agregar ||
                    GestionControl == Gestion.Autorizar ||
                    GestionControl == Gestion.Modificar)
                {
                    this.PersistirDatosGrilla();
                    this.PersistirPercepciones();
                }
            }
            
        }
       
        void ctrBuscarOrdenesCompras_OrdenesComprasBuscarSeleccionar(List<CmpOrdenesCompras> e)
        {
            List<CapSolicitudPagoDetalles> lista = new List<CapSolicitudPagoDetalles>();
            CapSolicitudPagoDetalles spd;
            foreach (CmpOrdenesCompras oc in e)
            {
                oc.OrdenesComprasDetalles = ComprasF.OrdenCompraObtenerDetalles(oc);
                foreach (CmpOrdenesComprasDetalles ocd in oc.OrdenesComprasDetalles)
                {
                    spd = new CapSolicitudPagoDetalles();
                    spd.OrdenCompraDetalle = ocd;
                    AyudaProgramacion.MatchObjectProperties(oc, spd.OrdenCompraDetalle.OrdenCompra);
                    spd.AlicuotaIVA = ocd.IVA.Alicuota;
                    spd.Cantidad = ocd.Cantidad - (ocd.CantidadPagada.HasValue ? ocd.CantidadPagada.Value : 0);
                    spd.Descripcion = ocd.Descripcion;
                    spd.Producto = ocd.Producto;
                    spd.Producto.Descripcion = string.Concat(oc.Afiliado.DatosAfiliado, " ", ocd.Descripcion);
                    spd.PrecioUnitarioSinIva = oc.ImporteDescontar.Value;
                    lista.Add(spd);
                    //Solo tienen un item las OC de Terceros
                    break;
                }
            }

            //this.MiSolicitud.SolicitudPagoDetalles = new List<CapSolicitudPagoDetalles>();
            this.MiSolicitud.SolicitudPagoDetalles = this.MiSolicitud.SolicitudPagoDetalles.Where(x => x.Producto.IdProducto > 0).ToList();
            this.MiSolicitud.SolicitudPagoDetalles.AddRange(lista);
            //if (this.MiSolicitud.SolicitudPagoDetalles.Count < CantidadItems)
            //    this.IniciarGrilla(CantidadItems - this.MiSolicitud.SolicitudPagoDetalles.Count);
            AyudaProgramacion.CargarGrillaListas(this.MiSolicitud.SolicitudPagoDetalles, false, this.gvItems, true);
            this.items.Update();

            this.CalcularTotal();
        }

        void ctrBuscarSolPago_ControlBuscarSeleccionar(CapSolicitudPago e)
        {
            CapSolicitudPago solPago = CuentasPagarF.SolicitudPagoObtenerDatosCompletos(e);
            this.MiSolicitud.TiposFacturas = solPago.TiposFacturas;
            this.MisIvas = TGEGeneralesF.TGEIVAAlicuotaObtenerListaActiva(solPago.TiposFacturas, false);
            this.MiSolicitud.TipoSolicitudPago = solPago.TipoSolicitudPago;
            this.MiSolicitud.SolicitudPagoDetalles=new List<CapSolicitudPagoDetalles>();
            this.MiSolicitud.SolicitudPagoDetalles.AddRange(solPago.SolicitudPagoDetalles);
            AyudaProgramacion.CargarGrillaListas(this.MiSolicitud.SolicitudPagoDetalles, false, this.gvItems, true);
            this.ddlTipoFactura.SelectedValue = this.MiSolicitud.TiposFacturas.IdTipoFactura.ToString();
            ScriptManager.RegisterStartupScript(this.items, this.items.GetType(), "CalcularItemScript", "CalcularItem();", true);
            this.items.Update();
            this.upFactura.Update();
            this.CalcularTotal();
        }

        public void IniciarControl(CapSolicitudPago pSolicitud, Gestion pGestion)
        {
            CapProveedores prov = new CapProveedores();
            this.GestionControl = pGestion;
            this.MisIvas = new List<TGEIVA>();// TGEGeneralesF.TGEIVAAlicuotaObtenerListaActiva();
            CtbCentrosCostosProrrateos ccp = new CtbCentrosCostosProrrateos();
            ccp.UsuarioLogueado.IdUsuarioEvento = this.UsuarioActivo.IdUsuarioEvento;
            this.MisCentrosCostos = ContabilidadF.CentrosCostosProrrateosObtenerCombo(ccp);
            this.MisFiliales = this.UsuarioActivo.Filiales;
            this.MisPercepciones = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.AFIPTiposPercepciones);
            this.MisPercepciones.AddRange(TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.AFIPTiposRetenciones));
            this.MiSolicitud = pSolicitud;
            this.idTipoOperacionAsiento = pSolicitud.TipoOperacion.IdTipoOperacion;
            
            this.MiIndiceDetalleModificar = 0;
            this.CargarCombos();

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                 
                    this.MiCantidadDecimales = 2;
                    this.ddlCantidadDecimales.Enabled = true;
                    this.ddlFilialPago.Enabled = true;
                    this.rfvFilialPago.Enabled = true;
                    this.MiSolicitud.Estado.IdEstado = (int)Estados.Activo;
                    this.MiSolicitud.Estado = TGEGeneralesF.TGEEstadosObtener(this.MiSolicitud.Estado);
                    this.txtEstado.Text = this.MiSolicitud.Estado.Descripcion;
                    if (MiSolicitud.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosTerceros)
                    {
                        //Descuento Afiliado
                        //this.txtCuotasDescuentoAfiliado.Enabled = true;
                        this.ddlFormasCobros.Enabled = true;
                        
                        //this.txtCuotasDescuentoProveedor.Enabled = true;
                        this.btnAgregarItem.Visible = false;
                        this.IniciarGrilla(1);
                    }
                    else
                    {
                        this.btnImportarFactura.Visible = true;
                        this.btnOrdenesCompras.Visible = true;
                        this.AgregarPercepcion.Visible = true;
                        this.ddlCentrosCostosAsignar.Enabled = true;
                        this.IniciarGrilla(CantidadItems);
                    }
                    ListItem filialPago = this.ddlFilialPago.Items.FindByValue(this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString());
                    if (filialPago != null)
                        this.ddlFilialPago.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
                    //para que empiece con el foco en el codigo de proveedor
                    
                    AyudaProgramacion.CargarGrillaListas<CapSolicitudPagoDetalles>(this.MiSolicitud.SolicitudPagoDetalles, false, this.gvItems, true);
                    
                    this.ctrComentarios.IniciarControl(this.MiSolicitud, this.GestionControl);
                    this.ctrArchivos.IniciarControl(this.MiSolicitud, this.GestionControl);

                    if (this.MisParametrosUrl.Contains("IdRefEntidad"))
                    {
                        //this.hdfIdAfiliado.Value = this.MisParametrosUrl["IdAfiliado"].ToString();
                        //MiProveedor.IdProveedor = 0;
                        ctrBuscarProveedor.BuscarProveedor += new Proveedores.Controles.ProveedoresCabecerasDatos.ProveedoresDatosCabeceraAjaxEventHandler(CtrBuscarProveedor_BuscarProveedor);
                        prov.IdProveedor = Convert.ToInt32(this.MisParametrosUrl["IdRefEntidad"].ToString());
                        ctrBuscarProveedor.IniciarControl(prov, this.GestionControl);
                        //button_Click(null, EventArgs.Empty);
                        //this.txtNumeroSocio.Text = this.MisParametrosUrl["IdAfiliado"].ToString();
                        //this.txtNumeroSocio_TextChanged(this.txtNumeroSocio, EventArgs.Empty);
                    }

                    this.pnlRemito.Visible = true;
                    break;
                case Gestion.Autorizar:
                    this.MiCantidadDecimales = 4;
                    this.MiSolicitud = CuentasPagarF.SolicitudPagoObtenerDatosCompletos(pSolicitud);
                    prov.IdProveedor = this.MiSolicitud.Entidad.IdRefEntidad;

                    //MapearObjetoAControlesProveedor(ProveedoresF.ProveedoresObtenerDatosCompletos(prov));
                    MapearObjetoAControles(this.MiSolicitud);
                    this.btnImprimir.Visible = true;
                    //this.ddlTipoSolicitud.Enabled = false;
                    this.txtPreNumeroFactura.Enabled = false;
                    this.txtNumeroFactura.Enabled = false;
                    this.txtFechaFactura.Enabled = false;
                    this.txtFechaVencimiento.Enabled = false;
                    this.ddlTipoFactura.Enabled = false;
                    this.txtObservacion.Enabled = false;
                    this.btnImportarRemito.Visible = false;
                    this.txtDescuentoTotal.Enabled = true;
                    //this.cdFechaFactura.Enabled = false;
                    //this.cdFechaVencimmiento.Enabled = false;
                    this.btnAgregarItem.Visible = false;
                    this.btnImportarFactura.Visible = false;
                    chkAcopioFinanciero.Enabled = false;
                    //this.ceFechaContable.Enabled = false;
                    //this.imgFechaContable.Visible = false;
                    this.txtFechaContable.Enabled = false;
                    break;

                case Gestion.Anular:
                    this.MiCantidadDecimales = 4;
                    this.MiSolicitud = CuentasPagarF.SolicitudPagoObtenerDatosCompletos(pSolicitud);
                    prov.IdProveedor = this.MiSolicitud.Entidad.IdRefEntidad;
                    
                    //MapearObjetoAControlesProveedor(ProveedoresF.ProveedoresObtenerDatosCompletos(prov));
                    MapearObjetoAControles(this.MiSolicitud);
                    this.txtDescuentoTotal.Enabled = true;
                    //this.ddlTipoSolicitud.Enabled = false;
                    this.txtPreNumeroFactura.Enabled = false;
                    this.txtNumeroFactura.Enabled = false;
                    this.txtFechaFactura.Enabled = false;
                    this.txtFechaVencimiento.Enabled = false;
                    this.ddlTipoFactura.Enabled = false;
                    this.txtObservacion.Enabled = false;
                    this.btnImportarRemito.Visible = false;
                    //this.cdFechaFactura.Enabled = false;
                    //this.cdFechaVencimmiento.Enabled = false;
                    this.btnAgregarItem.Visible = false;
                    this.rfvFechaFactura.Enabled = false;
                    this.rfvFilialPago.Enabled = false;
                    this.rfvTipoFactura.Enabled = false;
                    this.rfvNumeroFactura.Enabled = false;
                    this.rfvPreNumeroFactura.Enabled = false;
                    this.btnImportarFactura.Visible = false;
                    //this.ceFechaContable.Enabled = false;
                    //this.imgFechaContable.Visible = false;
                    chkAcopioFinanciero.Enabled = false;
                    this.txtFechaContable.Enabled = false;
                    break;

                case Gestion.Modificar:
                    this.MiCantidadDecimales = 4;
                    this.MiSolicitud = CuentasPagarF.SolicitudPagoObtenerDatosCompletos(pSolicitud);
                    prov.IdProveedor = this.MiSolicitud.Entidad.IdRefEntidad;
                    this.btnImprimir.Visible = true;
                    // MapearObjetoAControlesProveedor(ProveedoresF.ProveedoresObtenerDatosCompletos(prov));
                    MapearObjetoAControles(this.MiSolicitud);
                    this.txtDescuentoTotal.Enabled = false;
                    //this.ddlTipoSolicitud.Enabled = false;
                    this.txtPreNumeroFactura.Enabled = false;
                    this.txtNumeroFactura.Enabled = false;
                    this.txtFechaFactura.Enabled = false;
                    this.txtFechaVencimiento.Enabled = false;
                    this.ddlTipoFactura.Enabled = false;
                    //this.txtObservacion.Enabled = false;
                    this.btnImportarRemito.Visible = false;
                    //this.cdFechaFactura.Enabled = false;
                    //this.cdFechaVencimmiento.Enabled = false;
                    this.btnAgregarItem.Visible = false;
                    this.rfvFechaFactura.Enabled = false;
                    this.rfvFilialPago.Enabled = false;
                    this.rfvTipoFactura.Enabled = false;
                    this.rfvNumeroFactura.Enabled = false;
                    this.rfvPreNumeroFactura.Enabled = false;
                    this.btnImportarFactura.Visible = false;
                    //this.ceFechaContable.Enabled = false;
                    //this.imgFechaContable.Visible = false;
                    this.txtFechaContable.Enabled = false;
                    break;

                case Gestion.Consultar:
                    this.MiCantidadDecimales = 4;
                    this.MiSolicitud = CuentasPagarF.SolicitudPagoObtenerDatosCompletos(pSolicitud);
                    prov.IdProveedor = this.MiSolicitud.Entidad.IdRefEntidad;
                    this.btnImprimir.Visible = true;
                    // MapearObjetoAControlesProveedor(ProveedoresF.ProveedoresObtenerDatosCompletos(prov));
                    MapearObjetoAControles(this.MiSolicitud);
                    this.txtDescuentoTotal.Enabled = true;
                    //this.ddlTipoSolicitud.Enabled = false;
                    this.txtPreNumeroFactura.Enabled = false;
                    this.txtNumeroFactura.Enabled = false;
                    this.txtFechaFactura.Enabled = false;
                    this.txtFechaVencimiento.Enabled = false;
                    this.ddlTipoFactura.Enabled = false;
                    this.txtObservacion.Enabled = false;
                    this.btnImportarRemito.Visible = false;
                    //this.cdFechaFactura.Enabled = false;
                    //this.cdFechaVencimmiento.Enabled = false;
                    this.btnAgregarItem.Visible = false;
                    this.btnAceptar.Visible = false;
                    this.btnImportarFactura.Visible = false;
                    chkAcopioFinanciero.Enabled = false;
                    //this.ceFechaContable.Enabled = false;
                    //this.imgFechaContable.Visible = false;
                    this.txtFechaContable.Enabled = false;
                    break;
            }

            if (this.MiSolicitud.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosTerceros
                || this.MiSolicitud.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosTercerosAnular)
            {
                this.pnlDescuentoAfiliado.Visible = true;
                this.btnOrdenesCompras.Visible = false;
                this.btnImportarRemito.Visible = false;
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            //this.ctrPopUpComprobantes.CargarReporte(this.MiOrdenPago, EnumTGEComprobantes.CapOrdenesPagos);
            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CapSolicitudPagos, "SolicitudPagoCompras", this.MiSolicitud, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            ExportPDF.ExportarPDF(pdf, this.Page, "SolicitudPagoCompras", this.UsuarioActivo);
        }

        private void CargarCombos()
        {
            MisTiposFacturas = TGEGeneralesF.TiposOperacionesTiposFacturasObtenerLista();

            this.ddlTipoFactura.DataSource = this.MisTiposFacturas; //
            this.ddlTipoFactura.DataValueField = "IdTipoFactura";
            this.ddlTipoFactura.DataTextField = "Descripcion";
            this.ddlTipoFactura.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion"));
/*
            this.ddlTipoSolicitud.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValores.TiposSolicitudesPago);
            this.ddlTipoSolicitud.DataValueField = "IdListaValorDetalle";
            this.ddlTipoSolicitud.DataTextField = "Descripcion";
            this.ddlTipoSolicitud.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoSolicitud, this.ObtenerMensajeSistema("SeleccioneOpcion"));
*/


            if (MiSolicitud.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosTerceros)
            {
                TGEFormasCobrosAfiliados formasCobroAfi = new TGEFormasCobrosAfiliados();
                formasCobroAfi.IdAfiliado = this.MiSolicitud.Afiliado.IdAfiliado;
                formasCobroAfi.IdTipoCargo = (int)EnumTiposCargos.SolicitudesPagosTerceros;
                this.ddlFormasCobros.DataSource = TGEGeneralesF.FormasCobrosAfiliadosObtenerPorAfiliadoTipoCargo(formasCobroAfi);
                this.ddlFormasCobros.DataValueField = "IdFormaCobroAfiliado";
                this.ddlFormasCobros.DataTextField = "FormaCobroDescripcion";
                this.ddlFormasCobros.DataBind();
            }

            this.ddlFilialPago.DataSource = TGEGeneralesF.FilialesPagosObtenerListaActiva();
            this.ddlFilialPago.DataValueField = "IdFilialPago";
            this.ddlFilialPago.DataTextField = "Filial";
            this.ddlFilialPago.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilialPago, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlCantidadDecimales.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.CantidadDecimales);
            this.ddlCantidadDecimales.DataValueField = "CodigoValor";
            this.ddlCantidadDecimales.DataTextField = "Descripcion";
            this.ddlCantidadDecimales.DataBind();

            this.ddlCentrosCostosAsignar.DataSource = this.MisCentrosCostos;
            this.ddlCentrosCostosAsignar.DataValueField = "IdCentroCostoProrrateo";
            this.ddlCentrosCostosAsignar.DataTextField = "CentroCostoProrrateo";
            this.ddlCentrosCostosAsignar.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(ddlCentrosCostosAsignar, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlCentrosCostosAsignar.Attributes.Add("onchange", "return RepetirCentrosCostos();");
            
            //this.ddlTipoActividad.DataSource = ImpuestosF.ActividadTiposObtenerLista();
            //this.ddlTipoActividad.DataValueField = "IdTipoDeActividad";
            //this.ddlTipoActividad.DataTextField = "Descripcion";
            //this.ddlTipoActividad.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoActividad, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlFilialEntrega.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFilialEntrega.DataValueField = "IdFilial";
            this.ddlFilialEntrega.DataTextField = "Filial";
            this.ddlFilialEntrega.DataBind();
            this.ddlFilialEntrega.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
        }

        protected void ddlTipoFactura_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlTipoFactura.SelectedValue))
            {
                this.MiSolicitud.TiposFacturas.IdTipoFactura = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].IdTipoFactura;
                this.MiSolicitud.TiposFacturas.CodigoValor = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].CodigoValor;
                this.MiSolicitud.TiposFacturas.Descripcion = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].Descripcion;
                this.MiSolicitud.TiposFacturas.Signo = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].Signo;
                //this.MiSolicitud.Afiliado.IdAfiliado = ddl
                TGETiposFacturas facturaFiltro = new TGETiposFacturas();
                facturaFiltro.IdTipoFactura = Convert.ToInt32(this.ddlTipoFactura.SelectedValue);
                this.MisIvas = TGEGeneralesF.TGEIVAAlicuotaObtenerListaActiva(facturaFiltro, false);
                PersistirDatosGrilla();
                AyudaProgramacion.CargarGrillaListas<CapSolicitudPagoDetalles>(this.MiSolicitud.SolicitudPagoDetalles, false, this.gvItems, true);
                ScriptManager.RegisterStartupScript(this.items, this.items.GetType(), "CalcularItemScript", "CalcularItem();", true);
                this.items.Update();

                if (this.MiSolicitud.TiposFacturas.Signo == -1)
                {

                    MiSolicitud.Entidad.IdRefEntidad = ctrBuscarProveedor.MiProveedor == null ? 0 : ctrBuscarProveedor.MiProveedor.IdProveedor.Value;
                    MiSolicitud.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
                    ddlTipoComprobanteAsociado.DataSource = CuentasPagarF.SolicitudPagosObtenerComboAsociados(MiSolicitud);
                    ddlTipoComprobanteAsociado.DataValueField = "IdSolicitudPago";
                    ddlTipoComprobanteAsociado.DataTextField = "DescripcionCombo";
                    ddlTipoComprobanteAsociado.DataBind();
                    if (ddlTipoComprobanteAsociado.Items.Count != 1)
                        AyudaProgramacion.AgregarItemSeleccione(ddlTipoComprobanteAsociado, ObtenerMensajeSistema("SeleccioneOpcion"));


                    this.chkGenerarRemito.Checked = false;
                    this.btnImportarRemito.Visible = false;
                    this.txtFechaVencimiento.Text = string.Empty;
                    this.txtFechaVencimiento.Enabled = true;
                    this.pnlComprobantesAsociados.Visible = true;
                }
                else
                {
                    pnlComprobantesAsociados.Visible = false;
                }

                this.upComprobantesAsociados.Update();
                AyudaProgramacion.CargarGrillaListas<CapSolicitudPagoDetalles>(MiSolicitud.SolicitudPagoDetalles, false, this.gvItems, true);
            }
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.SolicitudPagoModificarDatosAceptar != null)
                this.SolicitudPagoModificarDatosAceptar(null, this.MiSolicitud);
        }

        #region "Datos Solicitud"

        protected void MapearControlesAObjeto(CapSolicitudPago pSolicitud)
        {
            //SOLICITUD DATOS
            //TIPO
            pSolicitud.TipoSolicitudPago.IdTipoSolicitudPago = (int)EnumTiposSolicitudPago.Compras;//Convert.ToInt32(this.ddlTipoSolicitud.SelectedValue);
            //FACTURA
            pSolicitud.IdUsuarioAlta = this.UsuarioActivo.IdUsuarioEvento;
            pSolicitud.PrefijoNumeroFactura = txtPreNumeroFactura.Text.ToString().PadLeft(5, '0');
            pSolicitud.NumeroFactura = txtNumeroFactura.Text.ToString().PadLeft(8, '0');
            pSolicitud.FechaFactura = this.txtFechaFactura.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaFactura.Text);
            pSolicitud.FechaContable = this.txtFechaContable.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaContable.Text);
            pSolicitud.FechaVencimiento = this.txtFechaVencimiento.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaVencimiento.Text);
            pSolicitud.TiposFacturas.IdTipoFactura = this.ddlTipoFactura.SelectedValue==string.Empty ? 0 : Convert.ToInt32(this.ddlTipoFactura.SelectedValue);
            if(pSolicitud.TiposFacturas.IdTipoFactura > 0)
            pSolicitud.TiposFacturas.Signo = MisTiposFacturas.First(x => x.IdTipoFactura == pSolicitud.TiposFacturas.IdTipoFactura).Signo;
            pSolicitud.Observacion = this.txtObservacion.Text;
            //ITEMS
            pSolicitud.ImporteSinIVA = decimal.Parse(this.txtTotalSinIva.Text, NumberStyles.Currency);
            //pSolicitud.ImporteConIVA = Convert.ToDecimal(this.txtTotalConIva.Text);
            pSolicitud.IvaTotal = decimal.Parse(this.txtTotalIva.Text, NumberStyles.Currency);
            pSolicitud.Descuento = this.txtDescuentoTotal.Decimal; //decimal.Parse(this.txtDescuentoTotal.Text, NumberStyles.Currency);
            //DATOS PROVEEDOR
            pSolicitud.Entidad.IdEntidad = (int)EnumTGEEntidades.Proveedores;
              pSolicitud.ImporteTotal = decimal.Parse(this.txtTotalConIva.Text, NumberStyles.Currency); //Convert.ToDecimal(this.txtTotalConIva.Text);

            pSolicitud.Entidad.IdRefEntidad = ctrBuscarProveedor.MiProveedor.IdProveedor.Value;
          

            //pSolicitud.CuotasDescuentoAfiliado = this.txtCuotasDescuentoAfiliado.Text == string.Empty ? default(int) : Convert.ToInt32(this.txtCuotasDescuentoAfiliado.Text);
            pSolicitud.FormaCobroAfiliado.IdFormaCobroAfiliado = this.ddlFormasCobros.SelectedValue == string.Empty ? default(int) : Convert.ToInt32(this.ddlFormasCobros.SelectedValue);
            pSolicitud.FormaCobroAfiliado.FormaCobroDescripcion = this.ddlFormasCobros.SelectedValue == string.Empty ? string.Empty : this.ddlFormasCobros.SelectedItem.Text;
            //pSolicitud.CuotasPagoProveedor = this.txtCuotasDescuentoProveedor.Text == string.Empty ? default(int) : Convert.ToInt32(this.txtCuotasDescuentoProveedor.Text);

            pSolicitud.FilialPago.IdFilialPago = this.ddlFilialPago.SelectedValue == string.Empty ? default(int) : Convert.ToInt32(this.ddlFilialPago.SelectedValue);
            pSolicitud.AcopioFinanciero = this.chkAcopioFinanciero.Checked;
            pSolicitud.Comentarios = ctrComentarios.ObtenerLista();
            pSolicitud.Archivos = ctrArchivos.ObtenerLista();
            //pSolicitud.Campos = this.ctrCamposValores.ObtenerLista();

            //REMITOS
            pSolicitud.NumeroRemitoPrefijo = txtPrefijoNumeroRemito.Text.ToString().PadLeft(4, '0');
            pSolicitud.NumeroRemito = txtNumeroRemito.Text.ToString().PadLeft(8, '0');
        }

        private void MapearObjetoAControles(CapSolicitudPago pSolicitud)
        {

            //SOLICITUD DATOS
            //TIPO
            //this.ddlTipoSolicitud.SelectedValue = (pSolicitud.TipoSolicitudPago.IdTipoSolicitudPago).ToString();
            this.txtEstado.Text = this.MiSolicitud.Estado.Descripcion;
            this.txtPreNumeroFactura.Text = pSolicitud.PrefijoNumeroFactura.PadLeft(5, '0');
            this.txtNumeroFactura.Text = pSolicitud.NumeroFactura.PadLeft(8, '0');
            this.txtFechaFactura.Text = pSolicitud.FechaFactura.HasValue ? pSolicitud.FechaFactura.Value.ToShortDateString() : string.Empty;
            this.txtFechaVencimiento.Text = pSolicitud.FechaVencimiento.HasValue ? pSolicitud.FechaVencimiento.Value.ToShortDateString() : string.Empty;
            this.txtFechaContable.Text = pSolicitud.FechaContable.HasValue ? pSolicitud.FechaContable.Value.ToShortDateString() : string.Empty;
            this.ddlTipoFactura.SelectedValue = (pSolicitud.TiposFacturas.IdTipoFactura).ToString();
            

            this.txtPrefijoNumeroRemito.Text = pSolicitud.NumeroRemitoPrefijo.PadLeft(4, '0');
            this.txtNumeroRemito.Text = pSolicitud.NumeroRemito.PadLeft(8, '0');

            this.txtObservacion.Text = pSolicitud.Observacion;

            this.ddlFilialPago.SelectedValue = (pSolicitud.FilialPago.IdFilialPago).ToString();
            this.chkAcopioFinanciero.Checked = pSolicitud.AcopioFinanciero.HasValue ? pSolicitud.AcopioFinanciero.Value : false;
            //ITEMS
            this.txtTotalSinIva.Text = pSolicitud.ImporteSinIVA.ToString("C2");
            this.txtTotalConIva.Text = pSolicitud.ImporteTotal.ToString("C2");
            this.txtTotalIva.Text = pSolicitud.IvaTotal.ToString("C2");

            if (pSolicitud.FormaCobroAfiliado.IdFormaCobroAfiliado > 0)
            {
                ListItem item = this.ddlFormasCobros.Items.FindByValue(pSolicitud.FormaCobroAfiliado.IdFormaCobroAfiliado.ToString());
                if (item == null)
                    this.ddlFormasCobros.Items.Add(new ListItem(pSolicitud.FormaCobroAfiliado.FormaCobroDescripcion, pSolicitud.FormaCobroAfiliado.IdFormaCobroAfiliado.ToString()));
                this.ddlFormasCobros.SelectedValue = pSolicitud.FormaCobroAfiliado.IdFormaCobroAfiliado.ToString();
            }
            //this.txtCuotasDescuentoProveedor.Text = pSolicitud.CuotasPagoProveedor.HasValue ? pSolicitud.CuotasPagoProveedor.Value.ToString() : string.Empty;

            this.MiSolicitud.SolicitudPagoDetalles = pSolicitud.SolicitudPagoDetalles;

            //MOSTRAR DESCUENTO SI EXISTE OC
            if (this.MiSolicitud.Descuento != 0)
            {
                this.txtDescuentoTotal.Decimal = pSolicitud.Descuento;
                this.MostrarCampos(true);
            }
            else
            {
                this.MostrarCampos(false);
            }

            AyudaProgramacion.CargarGrillaListas<CapSolicitudPagoDetalles>(this.MiSolicitud.SolicitudPagoDetalles, false, this.gvItems, true);
            this.items.Update();
            //DDLDecimales >>> PARA CONSULTAR ATORIZAR O ANULAR MUESTRO EL MAXIMO(como esta cargado en la Base de Datos)
            {
                this.ddlCantidadDecimales.SelectedValue = ((int)EnumCantidadDecimales.Decimales4).ToString();
                this.ddlCantidadDecimales_OnClick(null, EventArgs.Empty);
            }


            //Cargando Percepciones
            this.MiSolicitud.SolicitudPagoTiposPercepciones = pSolicitud.SolicitudPagoTiposPercepciones;
            AyudaProgramacion.CargarGrillaListas<CapSolicitudPagoTipoPercepcion>(this.MiSolicitud.SolicitudPagoTiposPercepciones, false, this.gvPercepciones, false);
            this.upPercepciones.Update();

            this.ctrComentarios.IniciarControl(pSolicitud, this.GestionControl);
            this.ctrArchivos.IniciarControl(pSolicitud, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pSolicitud);
            CapSolicitudPago spAsiento = new CapSolicitudPago();
            spAsiento.IdSolicitudPago = pSolicitud.IdSolicitudPago;
            spAsiento.TipoOperacion.IdTipoOperacion = this.idTipoOperacionAsiento > 0 ? this.idTipoOperacionAsiento : pSolicitud.TipoOperacion.IdTipoOperacion;
            this.ctrAsientoMostrar.IniciarControl(spAsiento);

            CapProveedores prov = new CapProveedores();
            this.MiSolicitud = CuentasPagarF.SolicitudPagoObtenerDatosCompletos(pSolicitud);
            prov.IdProveedor = this.MiSolicitud.Entidad.IdRefEntidad;

            MapearObjetoAControlesProveedor(ProveedoresF.ProveedoresObtenerDatosCompletos(prov));
            this.ctrBuscarProveedor.IniciarControl(prov, this.GestionControl);

            if (pSolicitud.ComprobantesAsociados.Count > 0)
            {
                AyudaProgramacion.CargarGrillaListas<CapSolicitudPago>(pSolicitud.ComprobantesAsociados, false, this.gvDatos, true);
                this.pnlComprobantesAsociados.Visible = true;
                lblddlTipoComprobanteAsociado.Visible = false;
                btnAgregarComprobanteAsociado.Visible = false;
                ddlTipoComprobanteAsociado.Visible = false;
                gvDatos.Columns[8].Visible = false;
            }

        }

        #endregion

        #region "Proveedores PopUp"
        void CtrBuscarProveedor_BuscarProveedor(CapProveedores e)
        {
            this.MapearObjetoAControlesProveedor(e);
            //this.UpdatePanelProovedor.Update();
        }

        private void MapearObjetoAControlesProveedor(CapProveedores pProveedor)
        {
            this.MiSolicitud.Entidad.IdCuentaContable = pProveedor.CuentaContable.IdCuentaContable;            
            //en caso de cambiar de proveedor limpia la grilla
            if (this.GestionControl == Gestion.Agregar)
            {
                if (this.MiSolicitud.SolicitudPagoDetalles.Exists(x => x.Producto.IdProducto != 0))
                {
                    this.MiSolicitud.SolicitudPagoDetalles.Clear();
                    this.IniciarGrilla(1);
                    AyudaProgramacion.CargarGrillaListas<CapSolicitudPagoDetalles>(this.MiSolicitud.SolicitudPagoDetalles, false, this.gvItems, true);
                    this.items.Update();
                }
            }
            //this.btnLimpiar.Visible = true;
            //Luego de agregar el Proveedor, dejo el foco en la fecha de factura
            this.txtFechaFactura.Focus();

            ListaParametros listaParam = new ListaParametros(this.MiSessionPagina);
            listaParam.Agregar("IdProveedor", pProveedor.IdProveedor);
        }

        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            string txtCodigo = ((TextBox)sender).Text;
            CapProveedores parametro = new CapProveedores();
            parametro.IdProveedor = Convert.ToInt32(txtCodigo);
            parametro = ProveedoresF.ProveedoresObtenerDatosCompletos(parametro);
            this.MiSolicitud.Entidad.IdRefEntidad = parametro.IdProveedor == null ? 0 : Convert.ToInt32(parametro.IdProveedor);
            //mapear todo prov a Entidad
            if (this.MiSolicitud.Entidad.IdRefEntidad != 0)
            {
                this.MapearObjetoAControlesProveedor(parametro);
            }
            else
            {
                parametro.CodigoMensaje = "ProveedorCodigoNoExiste";
                this.MostrarMensaje(parametro.CodigoMensaje, true);
            }
        }

        #endregion

        #region "Grilla Solicitud Detalles"

        private void PersistirDatosGrilla()
        {
            if (this.MiSolicitud.SolicitudPagoDetalles.Count == 0)
                return;
            bool modifica;
            CapSolicitudPagoDetalles det;
            foreach (GridViewRow fila in this.gvItems.Rows)
            {
                string codigo = ((DropDownList)fila.FindControl("ddlProducto")).SelectedValue;
                string cantidad = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtCantidad")).Text;
                //string descripcion = ((TextBox)fila.FindControl("txtProducto")).Text;
                decimal precioUnitarioSinIva = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtPrecioUnitario")).Decimal;
                string subTotal = ((Label)fila.FindControl("lblSubtotal")).Text == string.Empty ? "0" : Convert.ToString(((Label)fila.FindControl("lblSubtotal")).Text);
                DropDownList ddlAlicuotaIVA = ((DropDownList)fila.FindControl("ddlAlicuotaIVA"));
                DropDownList ddlFiliales = ((DropDownList)fila.FindControl("ddlFiliales"));
                DropDownList ddlCentrosCostos = ((DropDownList)fila.FindControl("ddlCentrosCostos"));
                string importeIva = ((Label)fila.FindControl("lblImporteIva")).Text == string.Empty ? "0" : Convert.ToString(((Label)fila.FindControl("lblImporteIva")).Text);
                string subTotalIva = ((Label)fila.FindControl("lblSubtotalConIva")).Text == string.Empty ? "0" : Convert.ToString(((Label)fila.FindControl("lblSubtotalConIva")).Text);
                decimal noGravado = ((TextBox)fila.FindControl("txtPrecioNoGravado")).Text == string.Empty ? 0 : decimal.Parse(((TextBox)fila.FindControl("txtPrecioNoGravado")).Text, NumberStyles.Currency);
                decimal importeDesc = ((TextBox)fila.FindControl("txtDescuentoImporte")).Text == string.Empty ? 0 : decimal.Parse(((TextBox)fila.FindControl("txtDescuentoImporte")).Text, NumberStyles.Currency);
                HiddenField hdfDescripcionProducto = (HiddenField)fila.FindControl("hdfDescripcionProducto");
                Label lblProductoDescripcion = (Label)fila.FindControl("lblProductoDescripcion");
                string descripcionProducto = ((TextBox)fila.FindControl("txtDescripcionProducto")).Text;

                //NumberStyles.Currency para sacar el signo $     

                modifica = false;
                det = this.MiSolicitud.SolicitudPagoDetalles[fila.DataItemIndex];

                DropDownList ddlProducto = ((DropDownList)fila.FindControl("ddlProducto"));
                HiddenField hdfIdProducto = (HiddenField)fila.FindControl("hdfIdProducto");
                HiddenField hdfProductoDetalle = (HiddenField)fila.FindControl("hdfProductoDetalle");

                if (hdfIdProducto.Value != string.Empty && Convert.ToInt32(hdfIdProducto.Value) > 0)
                {
                    det.Producto.IdProducto = Convert.ToInt32(hdfIdProducto.Value);
                    det.Producto.Descripcion = hdfProductoDetalle.Value;
                    det.Producto.Venta = true;
                }
                this.MiSolicitud.SolicitudPagoDetalles[fila.RowIndex].DescripcionProducto = descripcionProducto;
                if (cantidad != string.Empty)
                {
                    this.MiSolicitud.SolicitudPagoDetalles[fila.DataItemIndex].Cantidad = Convert.ToDecimal(cantidad);
                }

                //if (descripcion != string.Empty)
                //{
                //    this.MiSolicitud.SolicitudPagoDetalles[fila.DataItemIndex].Descripcion = descripcion;
                //}

                //if (precioUnitarioSinIva != 0)
                //{
                /* Comentando 27/12/21 para que las notas de credito permitan valores negativos.*/
                //if (MiSolicitud.TiposFacturas.Signo == -1)
                //{
                //    precioUnitarioSinIva = Math.Abs(precioUnitarioSinIva);
                //}
                this.MiSolicitud.SolicitudPagoDetalles[fila.DataItemIndex].PrecioUnitarioSinIva = precioUnitarioSinIva;
                //}
                //if (noGravado != 0)
                //{
                /* Comentando 27/12/21 para que las notas de credito permitan valores negativos.*/
                //if (MiSolicitud.TiposFacturas.Signo == -1)
                //{
                //    noGravado = Math.Abs(noGravado);
                //}
                this.MiSolicitud.SolicitudPagoDetalles[fila.DataItemIndex].PrecioNoGravado = noGravado;
                //}
                //if (importeDesc != 0)
                //{
                    this.MiSolicitud.SolicitudPagoDetalles[fila.DataItemIndex].DescuentoImporte = importeDesc;
                //}

                if (ddlAlicuotaIVA.SelectedValue != string.Empty && ddlAlicuotaIVA.SelectedValue != this.MiSolicitud.SolicitudPagoDetalles[fila.DataItemIndex].IVA.IdIVAAlicuota)
                {
                    this.MiSolicitud.SolicitudPagoDetalles[fila.DataItemIndex].AlicuotaIVA = this.MisIvas[ddlAlicuotaIVA.SelectedIndex].Alicuota; //Convert.ToDecimal(ddlAlicuotaIVA.SelectedValue);
                    this.MiSolicitud.SolicitudPagoDetalles[fila.DataItemIndex].IVA = this.MisIvas[ddlAlicuotaIVA.SelectedIndex];
                }
                if (ddlFiliales.SelectedValue != "" && Convert.ToInt32(ddlFiliales.SelectedValue) > 0)
                {
                    this.MiSolicitud.SolicitudPagoDetalles[fila.DataItemIndex].Filial.IdFilial = this.MisFiliales[ddlFiliales.SelectedIndex].IdFilial;
                }
                if (ddlCentrosCostos.SelectedValue != "" && Convert.ToInt32(ddlCentrosCostos.SelectedValue)>0)
                {
                    var aux = this.MisCentrosCostos.Find(x => x.IdCentroCostoProrrateo == Convert.ToInt32(ddlCentrosCostos.SelectedValue));
                    if (this.MiSolicitud.SolicitudPagoDetalles[fila.DataItemIndex].CentroCostoProrrateo.IdCentroCostoProrrateo != aux.IdCentroCostoProrrateo)
                    {
                        this.MiSolicitud.SolicitudPagoDetalles[fila.DataItemIndex].EstadoColeccion = EstadoColecciones.Modificado;
                    }
                    this.MiSolicitud.SolicitudPagoDetalles[fila.DataItemIndex].CentroCostoProrrateo = aux;
                }

                ((Label)fila.FindControl("lblImporteIva")).Text = (this.MiSolicitud.SolicitudPagoDetalles[fila.DataItemIndex].ImporteIvaTotal).ToString();
                ((Label)fila.FindControl("lblSubtotal")).Text = (this.MiSolicitud.SolicitudPagoDetalles[fila.DataItemIndex].Subtotal).ToString();
                ((Label)fila.FindControl("lblSubtotalConIva")).Text = (this.MiSolicitud.SolicitudPagoDetalles[fila.DataItemIndex].PrecioTotalItem).ToString();

            }
            this.CalcularTotal();
        }

        private void IniciarGrilla(int pCantidad)
        {
            this.MiSolicitud.SolicitudPagoDetalles.Clear();
            CapSolicitudPagoDetalles item;
            for (int i = 0; i < pCantidad; i++)
            {
                item = new CapSolicitudPagoDetalles();
                this.MiSolicitud.SolicitudPagoDetalles.Add(item);
                item.IndiceColeccion = this.MiSolicitud.SolicitudPagoDetalles.IndexOf(item);

                if (MiSolicitud.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosTerceros)
                {
                    this.MiSolicitud.SolicitudPagoDetalles[0].Producto.IdProducto = (int)EnumProductos.VariosGenerico;
                    this.MiSolicitud.SolicitudPagoDetalles[0].Producto = ComprasF.ProductosObtenerPorIdProducto(this.MiSolicitud.SolicitudPagoDetalles[0].Producto);
                    this.MiSolicitud.SolicitudPagoDetalles[0].Cantidad = 1;
                    break;
                }
            }
        }

        protected void txtCodigoProducto_TextChanged(object sender, EventArgs e)
        {
            GridViewRow row = ((GridViewRow)((TextBox)sender).Parent.Parent);
            int IndiceColeccion = row.RowIndex;

            string contenido = ((TextBox)sender).Text;
            if (contenido == string.Empty)
                return;
            this.MiSolicitud.SolicitudPagoDetalles[IndiceColeccion].Producto.IdProducto = Convert.ToInt32(contenido);
            this.MiSolicitud.SolicitudPagoDetalles[IndiceColeccion].Producto.Compra = true;
            this.MiSolicitud.SolicitudPagoDetalles[IndiceColeccion].Producto = ComprasF.ProductosObtenerPorIdProducto(this.MiSolicitud.SolicitudPagoDetalles[IndiceColeccion].Producto);
            AyudaProgramacion.CargarGrillaListas<CapSolicitudPagoDetalles>(this.MiSolicitud.SolicitudPagoDetalles, false, this.gvItems, true);
            this.gvItems.Rows[IndiceColeccion].FindControl("txtCantidad").Focus();
        }

        void ctrCentrosCostos_ControlDatosAceptar(object sender, CtbCentrosCostosProrrateos e)
        {
            //DropDownList ddlCentroCosto = (DropDownList)this.gvItems.Rows[this.MiIndiceDetalleModificar].FindControl("ddlCentrosCostos");
            this.MiSolicitud.SolicitudPagoDetalles[this.MiIndiceDetalleModificar].CentroCostoProrrateo.IdCentroCostoProrrateo = e.IdCentroCostoProrrateo;
            this.MisCentrosCostos.Add(e);
            this.MisCentrosCostos = AyudaProgramacion.AcomodarIndices<CtbCentrosCostosProrrateos>(  this.MisCentrosCostos.OrderBy(x => x.CentroCostoProrrateo).ToList() );

            this.ddlCentrosCostosAsignar.DataSource = this.MisCentrosCostos;
            this.ddlCentrosCostosAsignar.DataValueField = "IdCentroCostoProrrateo";
            this.ddlCentrosCostosAsignar.DataTextField = "CentroCostoProrrateo";
            this.ddlCentrosCostosAsignar.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(ddlCentrosCostosAsignar, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            AyudaProgramacion.CargarGrillaListas<CapSolicitudPagoDetalles>(this.MiSolicitud.SolicitudPagoDetalles, false, this.gvItems, true);
            this.items.Update();
        }

        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            ///this.MiIndiceDetalleModificar = Convert.ToInt32(e.CommandArgument);
            int index = Convert.ToInt32(e.CommandArgument);
            this.MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            if (e.CommandName == "Borrar")
            {
                //ELIMINA FILA
                //int indiceColeccion = Convert.ToInt32(e.CommandArgument);
                this.MiSolicitud.SolicitudPagoDetalles.RemoveAt(this.MiIndiceDetalleModificar);
                this.MiSolicitud.SolicitudPagoDetalles = AyudaProgramacion.AcomodarIndices<CapSolicitudPagoDetalles>(this.MiSolicitud.SolicitudPagoDetalles);
                AyudaProgramacion.CargarGrillaListas<CapSolicitudPagoDetalles>(this.MiSolicitud.SolicitudPagoDetalles, false, this.gvItems, true);
                this.CalcularTotal();
            }
            else if (e.CommandName == "BuscarProducto")
            {
                CMPProductos filtro = new CMPProductos();
                filtro.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                this.ctrBuscarProductoPopUp.IniciarControl(EnumTiposProductos.Compras, filtro);
            }
            else if (e.CommandName == "AgregarCentrosCostos")
            {
                CtbCentrosCostosProrrateos ccp = new CtbCentrosCostosProrrateos();
                ccp.NoVisible = true;
                this.ctrCentrosCostos.IniciarControl(ccp, Gestion.Agregar);
            }
            else if (e.CommandName == "ConsultarCentrosCostos")
            {
                DropDownList ddlCentroCosto = (DropDownList)this.gvItems.Rows[this.MiIndiceDetalleModificar].FindControl("ddlCentrosCostos");
                CtbCentrosCostosProrrateos ccp = new CtbCentrosCostosProrrateos();
                ccp.IdCentroCostoProrrateo = Convert.ToInt32(ddlCentroCosto.SelectedValue);
                this.ctrCentrosCostos.IniciarControl(ccp, Gestion.Consultar);
            }
        }

        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
               
                CapSolicitudPagoDetalles item = (CapSolicitudPagoDetalles)e.Row.DataItem;

                CurrencyTextBox noGravado = (CurrencyTextBox)e.Row.FindControl("txtPrecioNoGravado");
                CurrencyTextBox descuentoImporte = (CurrencyTextBox)e.Row.FindControl("txtDescuentoImporte");
                CurrencyTextBox PrecioUnitario = (CurrencyTextBox)e.Row.FindControl("txtPrecioUnitario");
                DropDownList ddlAlicuotaIVA = ((DropDownList)e.Row.FindControl("ddlAlicuotaIVA"));
                CurrencyTextBox cantidad = (CurrencyTextBox)e.Row.FindControl("txtCantidad");
                //TextBox producto = (TextBox)e.Row.FindControl("txtProducto");
                //ImageButton btnBuscarProducto = (ImageButton)e.Row.FindControl("btnBuscarProducto");
                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                DropDownList ddlFiliales = (DropDownList)e.Row.FindControl("ddlFiliales");
                DropDownList ddlCentrosCostos = (DropDownList)e.Row.FindControl("ddlCentrosCostos");
                ImageButton btnAgregarCentrosCostos = (ImageButton)e.Row.FindControl("btnAgregarCentrosCostos");
                DropDownList ddlProducto = ((DropDownList)e.Row.FindControl("ddlProducto"));
                if (item.Producto.IdProducto > 0)
                    ddlProducto.Items.Add(new ListItem(item.Producto.Descripcion, item.Producto.IdProducto.ToString()));
                /* Comentando 27/12/21 para que las notas de credito permitan valores negativos. */
                //if (MiSolicitud.TiposFacturas.Signo == -1)
                //{
                //    PrecioUnitario.AllowNegative = false;
                //    noGravado.AllowNegative = false;
                   
                   
                //}
                //else
                //{
                    noGravado.AllowNegative = true;
                    PrecioUnitario.AllowNegative = true;
                //}
                ddlProducto.Enabled = false;
              
                //producto.Enabled = false;
                cantidad.Enabled = false;
                PrecioUnitario.Enabled = false;
                noGravado.Enabled = false;
                descuentoImporte.Enabled = false;
                ddlAlicuotaIVA.Enabled = false;
                ddlFiliales.Enabled = false;
                ddlCentrosCostos.Enabled = false;
                btnEliminar.Visible = false;
                btnAgregarCentrosCostos.Visible = false;

                ddlAlicuotaIVA.Items.Clear();
                ddlAlicuotaIVA.SelectedValue = null;
                ddlAlicuotaIVA.DataSource = this.MisIvas;//TGEGeneralesF.TGEIVAAlicuotaObtenerLista();
                ddlAlicuotaIVA.DataValueField = "IdIVAAlicuota";
                ddlAlicuotaIVA.DataTextField = "Descripcion";
                ddlAlicuotaIVA.DataBind();
                if (ddlAlicuotaIVA.Items.Count == 0)
                    AyudaProgramacion.AgregarItemSeleccione(ddlAlicuotaIVA, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                ddlFiliales.DataSource = this.MisFiliales;
                ddlFiliales.DataValueField = "IdFilial";
                ddlFiliales.DataTextField = "Filial";
                ddlFiliales.DataBind();

                ddlCentrosCostos.DataSource = this.MisCentrosCostos;
                ddlCentrosCostos.DataValueField = "IdCentroCostoProrrateo";
                ddlCentrosCostos.DataTextField = "CentroCostoProrrateo";
                ddlCentrosCostos.DataBind();
                if (ddlCentrosCostos.Items.Count != 1)
                    AyudaProgramacion.AgregarItemSeleccione(ddlCentrosCostos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                //ddlCentrosCostos.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();

                PrecioUnitario.NumberOfDecimals = this.MiCantidadDecimales;

                ListItem itemCombo;

                switch (GestionControl)
                {
                    case Gestion.Agregar:
                        ddlCentrosCostos.Attributes.Add("onchange", "MostrarBotonesCentrosCostos();");
                        PrecioUnitario.Attributes.Add("onchange", "CalcularItem();");
                        string numberSymbol=PrecioUnitario.Prefix == string.Empty ? "N" : "C";
                        PrecioUnitario.Text = item.PrecioUnitarioSinIva.ToString(string.Concat(numberSymbol, this.MiCantidadDecimales.ToString()));
                        //AyudaProgramacion.AgregarItemSeleccione(ddlAlicuotaIVA, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                        //ddlAlicuotaIVA.SelectedValue = item.AlicuotaIVA.ToString();
                        ddlAlicuotaIVA.Attributes.Add("onchange", "CalcularItem();");
                        cantidad.Attributes.Add("onchange", "CalcularItem();");
                        noGravado.Attributes.Add("onchange", "CalcularItem();");
                        descuentoImporte.Attributes.Add("onchange", "CalcularItem();");

                        string mensaje = this.ObtenerMensajeSistema("ConfirmarEliminar");
                        string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                        btnEliminar.Attributes.Add("OnClick", funcion);

                        if (item.Filial.IdFilial > 0)
                            ddlFiliales.SelectedValue = item.Filial.IdFilial.ToString();
                        if (item.CentroCostoProrrateo.IdCentroCostoProrrateo > 0)
                            ddlCentrosCostos.SelectedValue = item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString();
                        else if (ddlCentrosCostos.Items.Count != 1)
                            ddlCentrosCostos.SelectedValue = string.Empty;

                        bool ordenCompraTerceros = item.OrdenCompraDetalle.IdOrdenCompraDetalle > 0;
                        bool remitoImportado = item.InformesDetalles.Count > 0;

                        
                            //codigo.Enabled = !ordenCompraTerceros;
                            //btnBuscarProducto.Visible = !ordenCompraTerceros;
                            //producto.Enabled = !ordenCompraTerceros;
                        cantidad.Enabled = !ordenCompraTerceros;
                        PrecioUnitario.Enabled = !ordenCompraTerceros;
                        noGravado.Enabled = !ordenCompraTerceros;
                        descuentoImporte.Enabled = !ordenCompraTerceros;

                        TextBox txtDescripcionProducto = (TextBox)e.Row.FindControl("txtDescripcionProducto");
                        txtDescripcionProducto.Visible = true;
                        txtDescripcionProducto.Enabled = true;
                        if (remitoImportado)
                        {
                            //codigo.Enabled = !remitoImportado;
                            //btnBuscarProducto.Visible = !remitoImportado;
                            //producto.Enabled = !remitoImportado;
                            cantidad.Enabled = !remitoImportado;
                        

                        }
                        ddlProducto.Enabled = true;
                        ddlAlicuotaIVA.Enabled = true;
                        ddlFiliales.Enabled = true;
                        ddlCentrosCostos.Enabled = true;
                        btnEliminar.Visible = true;
                        btnAgregarCentrosCostos.Visible = true;
                        //itemCombo = ddlAlicuotaIVA.Items.FindByValue(item.AlicuotaIVA.ToString());
                        itemCombo = ddlAlicuotaIVA.Items.FindByValue(item.IVA.IdIVAAlicuota);
                        if (itemCombo != null)
                            ddlAlicuotaIVA.SelectedValue = item.IVA.IdIVAAlicuota;

                        itemCombo = ddlFiliales.Items.FindByValue(item.Filial.IdFilial.ToString());
                        if (itemCombo != null)
                            ddlFiliales.SelectedValue = item.Filial.IdFilial.ToString();

                        itemCombo = ddlCentrosCostos.Items.FindByValue(item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString());
                        if (itemCombo != null)
                            ddlCentrosCostos.SelectedValue = item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString();

                        break;
                    case Gestion.Autorizar:
                        //itemCombo = ddlAlicuotaIVA.Items.FindByValue(item.AlicuotaIVA.ToString());
                        itemCombo = ddlAlicuotaIVA.Items.FindByValue(item.IVA.IdIVAAlicuota);
                        if (itemCombo == null)
                            ddlAlicuotaIVA.Items.Add(new ListItem(item.IVA.Descripcion, item.IVA.IdIVAAlicuota));
                        ddlAlicuotaIVA.SelectedValue = item.IVA.IdIVAAlicuota;

                        itemCombo = ddlFiliales.Items.FindByValue(item.Filial.IdFilial.ToString());
                        if (itemCombo != null)
                            ddlFiliales.Items.Add(new ListItem(item.Filial.Filial, item.Filial.IdFilial.ToString()));
                        ddlFiliales.SelectedValue = item.Filial.IdFilial.ToString();

                        itemCombo = ddlCentrosCostos.Items.FindByValue(item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString());
                        if (itemCombo == null)
                        {
                            this.MisCentrosCostos.Add(item.CentroCostoProrrateo);
                            ddlCentrosCostos.Items.Add(new ListItem(item.CentroCostoProrrateo.CentroCostoProrrateo.ToString(), item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString()));
                        }
                        ddlCentrosCostos.SelectedValue = item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString();
                        ddlCentrosCostos.Enabled = true;
                        break;
                    case Gestion.Anular:
                    case Gestion.Consultar:
                        //itemCombo = ddlAlicuotaIVA.Items.FindByValue(item.AlicuotaIVA.ToString());
                        itemCombo = ddlAlicuotaIVA.Items.FindByValue(item.IVA.IdIVAAlicuota);
                        if (itemCombo == null)
                            ddlAlicuotaIVA.Items.Add(new ListItem(item.IVA.Descripcion, item.IVA.IdIVAAlicuota));
                        ddlAlicuotaIVA.SelectedValue = item.IVA.IdIVAAlicuota;

                        itemCombo = ddlFiliales.Items.FindByValue(item.Filial.IdFilial.ToString());
                        if (itemCombo != null)
                            ddlFiliales.Items.Add(new ListItem(item.Filial.Filial, item.Filial.IdFilial.ToString()));
                        ddlFiliales.SelectedValue = item.Filial.IdFilial.ToString();

                        itemCombo = ddlCentrosCostos.Items.FindByValue(item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString());
                        if (itemCombo == null)
                        {
                            this.MisCentrosCostos.Add(item.CentroCostoProrrateo);
                            ddlCentrosCostos.Items.Add(new ListItem(item.CentroCostoProrrateo.CentroCostoProrrateo.ToString(), item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString()));
                        }
                        ddlCentrosCostos.SelectedValue = item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString();

                        Label lblProductoDescripcion = (Label)e.Row.FindControl("lblProductoDescripcion");
                        lblProductoDescripcion.Visible = true;
                        TextBox txtDescripcion = (TextBox)e.Row.FindControl("txtDescripcionProducto");
                        txtDescripcion.Visible = false;
                        break;
                    case Gestion.Modificar:
                        Label lblProductoDescripcion2 = (Label)e.Row.FindControl("lblProductoDescripcion");
                        lblProductoDescripcion2.Visible = true;
                        TextBox txtDescripcion2 = (TextBox)e.Row.FindControl("txtDescripcionProducto");
                        txtDescripcion2.Visible = false;
                        itemCombo = ddlAlicuotaIVA.Items.FindByValue(item.IVA.IdIVAAlicuota);
                        if (itemCombo == null)
                            ddlAlicuotaIVA.Items.Add(new ListItem(item.IVA.Descripcion, item.IVA.IdIVAAlicuota));
                        ddlAlicuotaIVA.SelectedValue = item.IVA.IdIVAAlicuota;

                        itemCombo = ddlFiliales.Items.FindByValue(item.Filial.IdFilial.ToString());
                        if (itemCombo != null)
                            ddlFiliales.Items.Add(new ListItem(item.Filial.Filial, item.Filial.IdFilial.ToString()));
                        ddlFiliales.SelectedValue = item.Filial.IdFilial.ToString();

                        itemCombo = ddlCentrosCostos.Items.FindByValue(item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString());
                        if (itemCombo == null)
                        {
                            this.MisCentrosCostos.Add(item.CentroCostoProrrateo);
                            ddlCentrosCostos.Items.Add(new ListItem(item.CentroCostoProrrateo.CentroCostoProrrateo.ToString(), item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString()));
                        }
                        ddlCentrosCostos.SelectedValue = item.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString();
                        ddlCentrosCostos.Enabled = true;
                        break;
                    default:
                        break;
                }
            }
        }

        protected void CalcularTotal()
        {
            decimal totalSinIva = 0;
            decimal totalIva = 0;
            decimal totalConIva = 0;
            decimal totalPercepciones = 0;
            decimal totalDescuentos = 0;
            
            if (this.MiSolicitud.SolicitudPagoTiposPercepciones.Count() != 0)
            {
                totalPercepciones = this.MiSolicitud.SolicitudPagoTiposPercepciones.Sum(x => x.Importe);
                this.MiSolicitud.ImportePercepciones = totalPercepciones;
            }
            
            totalSinIva = this.MiSolicitud.SolicitudPagoDetalles.Sum(x => x.Subtotal);
            totalIva = this.MiSolicitud.SolicitudPagoDetalles.Sum(x => x.ImporteIvaTotal);

            if (this.MiSolicitud.SolicitudPagoDetalles == null ? false : this.MiSolicitud.SolicitudPagoDetalles.Exists(x => x.Producto.IdProducto != 0))
            {
                if (this.MiSolicitud.SolicitudPagoDetalles.Exists(x => x.OrdenCompraDetalle.IdOrdenCompraDetalle != 0))
                {
                    totalDescuentos = this.txtDescuentoTotal.Text == string.Empty ? 0 : decimal.Parse(this.txtDescuentoTotal.Text, NumberStyles.Currency);
                    this.MostrarCampos(true);
                }
                else
                {
                    
                    this.MostrarCampos(false);
                }
            }
            else
            {
                this.MostrarTodo();
            }
            ////le estoy sumando las percepciones al total                                         ACA ¬
            totalConIva = this.MiSolicitud.SolicitudPagoDetalles.Sum(x => x.PrecioTotalItem) + totalPercepciones - totalDescuentos;

            if(txtTotalSinIva.Text == string.Empty || txtTotalIva.Text == string.Empty || txtTotalConIva.Text == string.Empty)
            {
                MiSolicitud.ImporteSinIVA = 0;
                MiSolicitud.IvaTotal = 0;
                MiSolicitud.ImporteTotal = 0;
            }
            else
            {
                MiSolicitud.ImporteSinIVA = decimal.Parse(this.txtTotalSinIva.Text, NumberStyles.Currency);
                MiSolicitud.IvaTotal = decimal.Parse(this.txtTotalIva.Text, NumberStyles.Currency);
                MiSolicitud.ImporteTotal = decimal.Parse(this.txtTotalConIva.Text, NumberStyles.Currency);
            }

            

            this.txtTotalConIva.Text = totalConIva.ToString("C2");
            this.txtTotalSinIva.Text = totalSinIva.ToString("C2");
            this.txtTotalIva.Text = totalIva.ToString("C2");
            this.pnTotales.Update();
        }

        //CALCULA EL DESCUENTO DE LA OC. Y MUESTRA LOS CAMPOS DE DESCUENTO TOTAL
        private void MostrarCampos(bool visible)
        {
            this.btnImportarRemito.Visible = !visible;
            this.btnImportarFactura.Visible = !visible;
            this.btnAgregarItem.Visible = !visible;
            this.btnOrdenesCompras.Visible = visible;
            this.pnlDescuento.Visible = visible;
            this.items.Update();
            this.pnTotales.Update();
        }

        private void MostrarTodo()
        {
            this.txtDescuentoTotal.Text = "0.00";
            this.btnImportarRemito.Visible = true;
            this.btnImportarFactura.Visible = true;
            this.btnAgregarItem.Visible = true;
            this.btnOrdenesCompras.Visible = true;
            this.pnlDescuento.Visible = false;
            this.items.Update();
        }

        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {
            CapSolicitudPagoDetalles item;
            item = new CapSolicitudPagoDetalles();
            this.MiSolicitud.SolicitudPagoDetalles.Add(item);
            item.IndiceColeccion = this.MiSolicitud.SolicitudPagoDetalles.IndexOf(item);
            this.CalcularTotal();
            AyudaProgramacion.CargarGrillaListas<CapSolicitudPagoDetalles>(this.MiSolicitud.SolicitudPagoDetalles, false, this.gvItems, true);
            //this.gvItems.Rows[item.IndiceColeccion].FindControl("txtCodigoProducto").Focus();
        }

        protected void btnImportarFactura_Click(object sender, EventArgs e)
        {
            //if (this..Text == string.Empty)
            //{
            //    this.MostrarMensaje("ValidarProveedorSeleccionado", true);
            //}
            //else
            {
                //this.MiSolicitud.Entidad.IdEntidad = (int)EnumTGEEntidades.Proveedores;
                //this.MiSolicitud.Entidad.IdRefEntidad = Convert.ToInt32(this.txtCodigo.Text);
                CapSolicitudPago sp = new CapSolicitudPago();
                sp.Entidad.IdEntidad = (int)EnumTGEEntidades.Proveedores;
                sp.Entidad.IdRefEntidad = Convert.ToInt32(ctrBuscarProveedor.MiProveedor.IdProveedor);
                this.ctrBuscarSolPago.IniciarControl(sp);
            }
        }

        protected void btnOrdenesCompras_Click(object sender, EventArgs e)
        {
            //if (this.txtCodigo.Text == string.Empty)
            //{
            //    this.MostrarMensaje("ValidarProveedorSeleccionado", true);
            //}
            //else
            {
                //this.MiSolicitud.Entidad.IdEntidad = (int)EnumTGEEntidades.Proveedores;
                CmpOrdenesCompras oc = new CmpOrdenesCompras();
                //oc.Proveedor.IdProveedor = Convert.ToInt32(this.txtCodigo.Text);
                oc.TipoOrdenCompra.IdTipoOrdenCompra = (int)EnumTiposOrdenesCompras.Terceros;
                List<CmpOrdenesCompras> listaIncluida = new List<CmpOrdenesCompras>();
                CmpOrdenesCompras item;
                foreach (CapSolicitudPagoDetalles det in this.MiSolicitud.SolicitudPagoDetalles)
                {
                    item = det.OrdenCompraDetalle.OrdenCompra;
                    listaIncluida.Add(item);
                }
                this.ctrBuscarOrdenesCompras.IniciarControl(oc, listaIncluida);
            }
        }

        protected void ddlCantidadDecimales_OnClick(object sender, EventArgs e)
        {
            this.MiCantidadDecimales = Convert.ToInt32(this.ddlCantidadDecimales.SelectedValue);
            string numberSymbol = string.Empty;
            foreach (GridViewRow fila in this.gvItems.Rows)
            {
                Evol.Controls.CurrencyTextBox PrecioUnitario = (Evol.Controls.CurrencyTextBox)fila.FindControl("txtPrecioUnitario");
                PrecioUnitario.NumberOfDecimals = this.MiCantidadDecimales;
                numberSymbol = PrecioUnitario.Prefix == string.Empty ? "N" : "C";
                PrecioUnitario.Text = PrecioUnitario.Decimal.ToString(string.Concat(numberSymbol, this.MiCantidadDecimales));
            }

            this.items.Update();
        }
        #endregion

        #region Grilla Percepciones
        private void PersistirPercepciones()
        {
            foreach (GridViewRow fila in this.gvPercepciones.Rows)
            {
                DropDownList ddlPercepciones = ((DropDownList)fila.FindControl("ddlPercepciones"));
                decimal importePercepcion = ((TextBox)fila.FindControl("txtImportePercepcion")).Text == string.Empty ? 0 : decimal.Parse(((TextBox)fila.FindControl("txtImportePercepcion")).Text, NumberStyles.Currency);

                if (ddlPercepciones.SelectedValue != "")
                {
                    this.MiSolicitud.SolicitudPagoTiposPercepciones[fila.RowIndex].TipoPercepcion.IdTipoPercepcion = Convert.ToInt32(ddlPercepciones.SelectedValue);
                }
                if (importePercepcion != 0)
                {
                    this.MiSolicitud.SolicitudPagoTiposPercepciones[fila.RowIndex].Importe = importePercepcion;
                }

                //((Label)fila.FindControl("lblImporteIva")).Text = (this.MiSolicitud.SolicitudPagoDetalles[fila.RowIndex].ImporteIvaTotal).ToString();
            }
            //this.CalcularTotal();
        }

        protected void gvPercepciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            ///this.MiIndiceDetalleModificar = Convert.ToInt32(e.CommandArgument);
            int index = Convert.ToInt32(e.CommandArgument);
            this.MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            if (e.CommandName == "Borrar")
            {
                //ELIMINA FILA
                //int indiceColeccion = Convert.ToInt32(e.CommandArgument);
                this.MiSolicitud.SolicitudPagoTiposPercepciones.RemoveAt(this.MiIndiceDetalleModificar);
                this.MiSolicitud.SolicitudPagoTiposPercepciones = AyudaProgramacion.AcomodarIndices<CapSolicitudPagoTipoPercepcion>(this.MiSolicitud.SolicitudPagoTiposPercepciones);
                AyudaProgramacion.CargarGrillaListas<CapSolicitudPagoTipoPercepcion>(this.MiSolicitud.SolicitudPagoTiposPercepciones, false, this.gvPercepciones, true);
                //this.CalcularTotal();
            }
            
        }

        protected void gvPercepciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                CapSolicitudPagoTipoPercepcion item = (CapSolicitudPagoTipoPercepcion)e.Row.DataItem;
                TextBox importePercepcion = (TextBox)e.Row.FindControl("txtImportePercepcion");
                DropDownList ddlPercepciones = ((DropDownList)e.Row.FindControl("ddlPercepciones"));
                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");

                ddlPercepciones.Enabled = false;
                importePercepcion.Enabled = false;
                btnEliminar.Visible = false;

                ddlPercepciones.DataSource = this.MisPercepciones;//TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.AFIPTiposPercepciones);
                ddlPercepciones.DataValueField = "IdListaValorSistemaDetalle";
                ddlPercepciones.DataTextField = "Descripcion";
                ddlPercepciones.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(ddlPercepciones, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                
                ListItem itemCombo;

                switch (GestionControl)
                {
                    case Gestion.Agregar:
                        ddlPercepciones.SelectedValue = item.TipoPercepcion.IdTipoPercepcion.ToString();
                        ddlPercepciones.Enabled = true;
                        importePercepcion.Attributes.Add("onchange", "CalcularPercepcion();");;
                        importePercepcion.Enabled = true;
                        btnEliminar.Visible = true;
                        break;
                    case Gestion.Consultar:
                        itemCombo = ddlPercepciones.Items.FindByValue(item.TipoPercepcion.IdTipoPercepcion.ToString());
                        if (itemCombo == null)
                            ddlPercepciones.Items.Add(new ListItem(item.TipoPercepcion.IdTipoPercepcion.ToString(), item.TipoPercepcion.Descripcion));
                        ddlPercepciones.SelectedValue = item.TipoPercepcion.IdTipoPercepcion.ToString();
                        ddlPercepciones.Enabled = false;
                        break;
                    case Gestion.Autorizar:
                        itemCombo = ddlPercepciones.Items.FindByValue(item.TipoPercepcion.IdTipoPercepcion.ToString());
                        if (itemCombo == null)
                            ddlPercepciones.Items.Add(new ListItem(item.TipoPercepcion.IdTipoPercepcion.ToString(), item.TipoPercepcion.Descripcion));
                        ddlPercepciones.SelectedValue = item.TipoPercepcion.IdTipoPercepcion.ToString();
                        ddlPercepciones.Enabled = false;
                        break;
                    default:
                        break;

                }

            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //if (this.GestionControl == Gestion.Consultar
                //    || this.GestionControl == Gestion.Autorizar
                //    || this.GestionControl == Gestion.Anular)
                //{
                    Label lblImportePercepciones = (Label)e.Row.FindControl("lblImportePercepciones");
                    lblImportePercepciones.Text = this.MiSolicitud.ImportePercepciones.ToString("C2");
                //}
                //else if (this.GestionControl == Gestion.Agregar)
                //{
                //    Label lblImportePercepciones = (Label)e.Row.FindControl("lblImportePercepciones");
                //    lblImportePercepciones.Text = this.MiSolicitud.SolicitudPagoTiposPercepciones.Sum(x => x.Importe).ToString("C2");
                //}
            }
        }

        protected void btnAgregarPercepcion_Click(object sender, EventArgs e)
        {
            CapSolicitudPagoTipoPercepcion item;
            item = new CapSolicitudPagoTipoPercepcion();
            this.MiSolicitud.SolicitudPagoTiposPercepciones.Add(item);
            item.IndiceColeccion = this.MiSolicitud.SolicitudPagoTiposPercepciones.IndexOf(item);
            //this.CalcularTotal();
            AyudaProgramacion.CargarGrillaListas<CapSolicitudPagoTipoPercepcion>(this.MiSolicitud.SolicitudPagoTiposPercepciones, false, this.gvPercepciones, true);
            this.gvPercepciones.Rows[item.IndiceColeccion].FindControl("ddlPercepciones").Focus();
            this.upPercepciones.Update();
            
        }
        #endregion

        #region Importar Remitos (Informes Recepcion)
        protected void btnImportarRemito_Click(object sender, EventArgs e)
        {
            //if (this.txtCodigo.Text == string.Empty)
            //{
            //    this.MiSolicitud.CodigoMensaje = "ValidarProveedorSeleccionado";
            //    this.MostrarMensaje(this.MiSolicitud.CodigoMensaje, true);
            //}
            //else
            {
                CmpInformesRecepciones informe= new CmpInformesRecepciones();
                informe.Proveedor.IdProveedor = Convert.ToInt32(ctrBuscarProveedor.MiProveedor.IdProveedor);
                this.MiSolicitud.SolicitudPagoDetalles.ForEach(x => informe.InformesRecepcionesDetalles.AddRange(x.InformesDetalles));
                this.ctrImportarRemito.IniciarControl(informe);
            }
        }

        void ctrImportarRemito_InformesBuscarSeleccionar(CmpInformesRecepciones e)
        {
            this.MapearInformeASolicitud(e);
            AyudaProgramacion.CargarGrillaListas<CapSolicitudPagoDetalles>(this.MiSolicitud.SolicitudPagoDetalles, false, this.gvItems, true);
            this.items.Update();
            this.CalcularTotal();
        }

        private void MapearInformeASolicitud(CmpInformesRecepciones pInforme)
        {
            if (pInforme.InformesRecepcionesDetalles.Count > 0
                && this.MiSolicitud.SolicitudPagoDetalles.Exists(x => x.Producto.IdProducto == 0))
                this.MiSolicitud.SolicitudPagoDetalles = this.MiSolicitud.SolicitudPagoDetalles.Where(x => x.Producto.IdProducto > 0).ToList();

            CapSolicitudPagoDetalles sDetalle;
            foreach (CmpInformesRecepcionesDetalles informeDetalle in pInforme.InformesRecepcionesDetalles)
            {
                if (this.MiSolicitud.SolicitudPagoDetalles.Exists(x => x.Producto.IdProducto == informeDetalle.Producto.IdProducto))
                {
                    this.MiSolicitud.SolicitudPagoDetalles.Find(x => x.Producto.IdProducto == informeDetalle.Producto.IdProducto).Cantidad += informeDetalle.CantidadPendiente;
                    this.MiSolicitud.SolicitudPagoDetalles.Find(x => x.Producto.IdProducto == informeDetalle.Producto.IdProducto).InformesDetalles.Add(informeDetalle);
                }
                else
                {
                    sDetalle = new CapSolicitudPagoDetalles();
                    AyudaProgramacion.MatchObjectProperties(informeDetalle.Producto, sDetalle.Producto);
                    sDetalle.Producto.IdProducto = informeDetalle.Producto.IdProducto;//ver como se carga la lista de precio
                    //sDetalle.ListaPrecioDetalle = ComprasF.ListasPreciosDetallesObtenerDatosPorProducto(sDetalle.ListaPrecioDetalle);
                    sDetalle.Producto.Descripcion = informeDetalle.Producto.Descripcion;
                    //sDetalle.PrecioUnitarioSinIva = sDetalle.ListaPrecioDetalle.CalcularPrecio(Convert.ToInt32(this.ddlCantidadCuotas.SelectedValue));
                    //sDetalle.DetalleImportado = true;
                    sDetalle.Cantidad = informeDetalle.CantidadPendiente;
                    sDetalle.InformesDetalles.Add(informeDetalle);
                    this.MiSolicitud.SolicitudPagoDetalles.Add(sDetalle);
                }
            }
            this.MiSolicitud.SolicitudPagoDetalles = AyudaProgramacion.AcomodarIndices<CapSolicitudPagoDetalles>(this.MiSolicitud.SolicitudPagoDetalles);
        }
        #endregion

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            NotaCreditoValidarCantidad();
            
            PersistirDatosGrilla();
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            CalcularTotal();
            if (MiSolicitud.ImporteTotal <= 0)
            {
                MostrarMensaje("El Total de la Factura Debe ser Mayor a 0", true);
                return;
            }

            this.MapearControlesAObjeto(this.MiSolicitud);
            this.MiSolicitud.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiSolicitud.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    foreach (CapSolicitudPagoDetalles item in this.MiSolicitud.SolicitudPagoDetalles)
                    {
                        if (item.Producto.IdProducto != 0)
                        {
                            item.EstadoColeccion = EstadoColecciones.Agregado;
                        }
                    }
                    if (this.MiSolicitud.SolicitudPagoTiposPercepciones.Count() != 0)
                    {
                        foreach (CapSolicitudPagoTipoPercepcion item in this.MiSolicitud.SolicitudPagoTiposPercepciones)
                        {
                            item.EstadoColeccion = EstadoColecciones.Agregado;
                        }
                    }

                    this.MiSolicitud.SolicitudPagoDetalles = this.MiSolicitud.SolicitudPagoDetalles.Where(x => x.EstadoColeccion == EstadoColecciones.Agregado).ToList();
                    AyudaProgramacion.CargarGrillaListas(this.MiSolicitud.SolicitudPagoDetalles, false, this.gvItems, true);
                    CmpInformesRecepciones remito = new CmpInformesRecepciones();
                    if (this.chkGenerarRemito.Checked)
                    {
                        this.MiSolicitud.RemitoAutomatico = true;
                        remito.FechaEmision = this.txtFechaEntrega.Text == string.Empty ? this.MiSolicitud.FechaFactura.Value : Convert.ToDateTime(this.txtFechaEntrega.Text);
                        remito.NumeroRemitoPrefijo = this.txtPrefijoNumeroRemito.Text == string.Empty ? this.MiSolicitud.NumeroRemitoPrefijo : this.txtPrefijoNumeroRemito.Text;
                        remito.NumeroRemitoSufijo = this.txtNumeroRemito.Text == string.Empty ? this.MiSolicitud.NumeroFactura : this.txtNumeroRemito.Text;
                        remito.Filial.IdFilial = this.ddlFilialEntrega.SelectedValue == string.Empty ? this.UsuarioActivo.FilialPredeterminada.IdFilial : Convert.ToInt32(this.ddlFilialEntrega.SelectedValue);
                    }
                    guardo = CuentasPagarF.SolicitudPagoAgregar(this.MiSolicitud, remito);

                    break;
                case Gestion.Autorizar:
                    //this.MiSolicitud.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    this.MiSolicitud.IdUsuarioAutorizacion = this.UsuarioActivo.IdUsuario;
                    this.MiSolicitud.FechaAutorizacion = DateTime.Now;
                    this.MiSolicitud.Estado.IdEstado = (int)EstadosSolicitudesPagos.Autorizado;
                    guardo = CuentasPagarF.SolicitudPagoAutorizar(this.MiSolicitud);
                    break;
                case Gestion.Modificar:
                    guardo = CuentasPagarF.SolicitudPagoModificar(this.MiSolicitud);
                    break;
                case Gestion.Anular:
                    //this.MiSolicitud.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    this.MiSolicitud.Estado.IdEstado = (int)Estados.Baja;
                    this.MiSolicitud.FechaAnulacion = DateTime.Now;
                    this.MiSolicitud.IdUsuarioAnulacion = this.UsuarioActivo.IdUsuario;
                    //foreach (CapSolicitudPagoDetalles item in this.MiSolicitud.SolicitudPagoDetalles)
                    //{
                    //    if (item.Producto.IdProducto != 0)
                    //    {
                    //        item.EstadoColeccion = EstadoColecciones.Modificado;
                    //    }
                    //}
                    guardo = CuentasPagarF.SolicitudPagoAnular(this.MiSolicitud);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.btnImprimir.Visible = true;
                this.btnAceptar.Visible = false;
                //this.btnAgregar.Visible = true;
                //this.btnAgregarOP.Visible = true;
                string mensajeResult;
                if (this.GestionControl == Gestion.Agregar) //para que muestre el numero de comprobante que se generó
                    mensajeResult = String.Concat(this.ObtenerMensajeSistema(this.MiSolicitud.CodigoMensaje), string.Concat("Comprobante Nro: ", this.MiSolicitud.IdSolicitudPago.ToString()));
                else
                    mensajeResult = this.ObtenerMensajeSistema(this.MiSolicitud.CodigoMensaje);
                this.MostrarMensaje(mensajeResult, false);
                this.ctrAsientoMostrar.IniciarControl(this.MiSolicitud);
            }
            else
            {
                this.MostrarMensaje(this.MiSolicitud.CodigoMensaje, true, this.MiSolicitud.CodigoMensajeArgs);
                if (this.MiSolicitud.dsResultado != null)
                {
                    this.ctrPopUpGrilla.IniciarControl(this.MiSolicitud);
                    this.MiSolicitud.dsResultado = null;
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {

            if (this.SolicitudPagoModificarDatosCancelar != null)
                this.SolicitudPagoModificarDatosCancelar();
        }

        protected void btnAgregarOP_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdEntidad", this.MiSolicitud.Entidad.IdEntidad);
            this.MisParametrosUrl.Add("IdRefEntidad", this.MiSolicitud.Entidad.IdRefEntidad);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/OrdenesPagosAgregar.aspx"), true);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            CapSolicitudPago filtro = new CapSolicitudPago();
            filtro.IdSolicitudPago = ddlTipoComprobanteAsociado.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlTipoComprobanteAsociado.SelectedValue);

            filtro = CuentasPagarF.SolicitudPagoObtenerDatosCompletos(filtro);
            this.MiSolicitud.ComprobantesAsociados.Add(filtro);
            filtro.IndiceColeccion = this.MiSolicitud.ComprobantesAsociados.IndexOf(filtro);

            AyudaProgramacion.CargarGrillaListas<CapSolicitudPago>(this.MiSolicitud.ComprobantesAsociados, false, this.gvDatos, true);

            MiSolicitud.SolicitudPagoDetalles = MiSolicitud.SolicitudPagoDetalles.Where(x => x.Producto.IdProducto > 0).ToList();
            MiSolicitud.SolicitudPagoDetalles.AddRange(filtro.SolicitudPagoDetalles);
            MiSolicitud.SolicitudPagoDetalles.ForEach(x => x.IdComprobanteAsociadoDetalle = x.IdSolicitudPagoDetalle);
            AyudaProgramacion.AcomodarIndices<CapSolicitudPagoDetalles>(this.MiSolicitud.SolicitudPagoDetalles);
            AyudaProgramacion.CargarGrillaListas<CapSolicitudPagoDetalles>(MiSolicitud.SolicitudPagoDetalles, false, gvItems, true);
            ScriptManager.RegisterStartupScript(this.items, this.items.GetType(), "CalcularItemScript", "CalcularItem();", true);

            items.Update();
            CalcularTotal();
        }

        #region Comprobantes Asociados
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CapSolicitudPago solicitud = this.MiSolicitud.ComprobantesAsociados[indiceColeccion];

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.MiSolicitud.ComprobantesAsociados.Remove(solicitud);
                AyudaProgramacion.AcomodarIndices<CapSolicitudPago>(this.MiSolicitud.ComprobantesAsociados);
                AyudaProgramacion.CargarGrillaListas<CapSolicitudPago>(this.MiSolicitud.ComprobantesAsociados, false, this.gvDatos, true);
                this.upComprobantesAsociados.Update();

                MiSolicitud.SolicitudPagoDetalles = new List<CapSolicitudPagoDetalles>();
                IniciarGrilla();
                this.items.Update();
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CapSolicitudPago parametros = this.BusquedaParametrosObtenerValor<CapSolicitudPago>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CapSolicitudPago>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            //gvDatos.DataSource = this.MiFactura.FacturasAsociadas;
            gvDatos.DataBind();
        }
        #endregion

        private void IniciarGrilla()
        {
            CapSolicitudPagoDetalles item;
            for (int i = 0; i < 2; i++)
            {
                item = new CapSolicitudPagoDetalles();
                item.EstadoColeccion = EstadoColecciones.AgregadoPrevio;
                this.MiSolicitud.SolicitudPagoDetalles.Add(item);
                item.IndiceColeccion = this.MiSolicitud.SolicitudPagoDetalles.IndexOf(item);
                //item.IdFacturaDetalle = item.IndiceColeccion * -1;
            }
            AyudaProgramacion.CargarGrillaListas<CapSolicitudPagoDetalles>(this.MiSolicitud.SolicitudPagoDetalles, false, this.gvItems, true);
        }

        private bool NotaCreditoValidarCantidad()
        {
            bool retorno = false;

            for(int i = 0; i < MiSolicitud.SolicitudPagoDetalles.Count; i++)
            {
                decimal cantidad = Convert.ToDecimal(((CurrencyTextBox)gvItems.Rows[i].FindControl("txtCantidad")).Text);
                if (MiSolicitud.SolicitudPagoDetalles[i].Cantidad == cantidad)
                {
                    retorno = true;
                }
            }
            return retorno;
        }
    }
}