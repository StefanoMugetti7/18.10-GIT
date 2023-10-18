using Afiliados;
using Afiliados.Entidades;
using Compras;
using Compras.Entidades;
using Comunes.Entidades;
using Facturas;
using Facturas.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Facturas.Controles
{
    public partial class RemitosDatos : ControlesSeguros
    {
        private VTARemitos MiRemito
        {
            get { return (VTARemitos)Session[this.MiSessionPagina + "RemitosDatosMiRemito"]; }
            set { Session[this.MiSessionPagina + "RemitosDatosMiRemito"] = value; }
        }

        private VTANotasPedidos MiNotaPedido
        {
            get { return (VTANotasPedidos)Session[this.MiSessionPagina + "RemitosDatosMiNotaPedido"]; }
            set { Session[this.MiSessionPagina + "RemitosDatosMiNotaPedido"] = value; }
        }

        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "RemitosModificarDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "RemitosModificarDatosMiIndiceDetalleModificar"] = value; }
        }

        private List<VTATiposPuntosVentas> MisTiposPuntosVentas
        {
            get { return (List<VTATiposPuntosVentas>)Session[this.MiSessionPagina + "RemitosDatosMisTiposPuntosVentas"]; }
            set { Session[this.MiSessionPagina + "RemitosDatosMisTiposPuntosVentas"] = value; }
        }

        private List<VTAFilialesPuntosVentas> MisFilialesPuntosVentas
        {
            get { return (List<VTAFilialesPuntosVentas>)Session[this.MiSessionPagina + "RemitosDatosMisFilialesPuntosVentas"]; }
            set { Session[this.MiSessionPagina + "RemitosDatosMisFilialesPuntosVentas"] = value; }
        }

        private List<TGETiposFacturas> MisTiposFacturas
        {
            get { return (List<TGETiposFacturas>)Session[this.MiSessionPagina + "RemitosDatosMisTiposFacturas"]; }
            set { Session[this.MiSessionPagina + "RemitosDatosMisTiposFacturas"] = value; }
        }


        //public delegate void RemitosDatosAceptarEventHandler(object sender, VTARemitos e);
        //public event RemitosDatosAceptarEventHandler RemitosModificarDatosAceptar;

        public delegate void RemitosDatosCancelarEventHandler();
        public event RemitosDatosCancelarEventHandler RemitosModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            //this.ctrBuscarClientePopUp.AfiliadosBuscarSeleccionar += new Afiliados.Controles.ClientesBuscarPopUp.AfiliadosBuscarEventHandler(ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar);
            //this.ctrBuscarProductoPopUp.ProductosBuscarSeleccionar += new CuentasPagar.Controles.BuscarProductoPopUp.ProductosBuscarEventHandler(ctrBuscarProductoPopUp_ProductosBuscarSeleccionar);
            //ctrBuscarProductoPrecioPoup.ProductosBuscarSeleccionar += CtrBuscarProductoPrecioPoup_ProductosBuscarSeleccionar;
            this.ctrBuscarFacturasPopUp.FacturasBuscarSeleccionar += CtrBuscarFacturasPopUp_FacturasBuscarSeleccionar;
            ctrBuscarFacturasPopUp.FacturasBuscarAcopio += CtrBuscarFacturasPopUp_FacturasBuscarAcopio;
            this.ctrBuscarPresupuestoPopUp.PresupuestosBuscarSeleccionar += CtrBuscarPresupuestoPopUp_PresupuestosBuscarSeleccionar;
            this.ctrBuscarNotaPedidoPopUp.NotasPedidosBuscarSeleccionar += ctrBuscarNotaPedidoPopUp_NotasPedidoBuscarSeleccionar;
            this.ctrDomicilios.AfiliadosModificarDatosAceptar += CtrDomicilios_AfiliadosModificarDatosAceptar;
            ctrBuscarCliente.BuscarCliente += new Afiliados.Controles.ClientesDatosCabeceraAjax.ClientesDatosCabeceraAjaxEventHandler(CtrBuscarCliente_BuscarCliente);
            if (!this.IsPostBack)
            {
                if (this.MiRemito == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
            else
            {
                this.PersistirDatosGrilla();
            }
        }
        public void IniciarControl(VTARemitos pRemito, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiRemito = pRemito;
            this.CargarCombos();
            List<TGEEstados> estados;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    estados = TGEGeneralesF.TGEEstadosObtenerLista("VTARemitos");
                    estados = estados.Where(x => x.IdEstado != (int)EstadosRemitos.Baja).ToList();
                    this.ddlEstadosRemtios.DataSource = estados;
                    this.ddlEstadosRemtios.DataValueField = "IdEstado";
                    this.ddlEstadosRemtios.DataTextField = "Descripcion";
                    this.ddlEstadosRemtios.DataBind();
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlEstadosRemtios, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                    this.ctrComentarios.IniciarControl(this.MiRemito, this.GestionControl);
                    this.ctrArchivos.IniciarControl(this.MiRemito, this.GestionControl);
                    this.txtNumeroRemito.Enabled = true;
                    //this.txtPrefijoNumeroRemito.Enabled = true;
                    //this.MiRemito.NumeroRemitoPrefijo = "0";//this.UsuarioActivo.FilialPredeterminada.AfipPuntoVenta.ToString().PadLeft(4, '0');
                    this.MiRemito.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;

                    this.btnImportarPresupuesto.Visible = true;
                    this.btnImportarNotaPedido.Visible = true;
                    this.btnImportarFactura.Visible = true;
                    this.ddlDomicilioEntrega.Enabled = true;
                    this.btnAgregarDomicilio.Visible = true;
                    this.ddlTipoOperacion.Enabled = true;
                    this.txtObservacionComprobante.Enabled = true;
                    this.txtObservacionInterna.Enabled = true;
                    if (this.MisTiposPuntosVentas.Count == 0)
                    {
                        List<string> lista = new List<string>
                        {
                            this.UsuarioActivo.FilialPredeterminada.Filial
                        };
                        this.MostrarMensaje("ValidarFilialPuntosVentas", true, lista);
                    }

                    if (this.MisParametrosUrl.Contains("IdAfiliado"))
                    {
                        this.MiRemito.Afiliado.IdAfiliado = Convert.ToInt32(this.MisParametrosUrl["IdAfiliado"].ToString());
                        this.ctrBuscarCliente.BuscarCliente += new Afiliados.Controles.ClientesDatosCabeceraAjax.ClientesDatosCabeceraAjaxEventHandler(this.CtrBuscarCliente_BuscarCliente);
                        this.ctrBuscarCliente.IniciarControl(this.MiRemito.Afiliado, this.GestionControl);
                        //this.txtNumeroSocio.Text = this.MisParametrosUrl["IdAfiliado"].ToString();
                        //this.txtNumeroSocio_TextChanged(this.txtNumeroSocio, EventArgs.Empty);
                    }
                    //this.ctrBuscarCliente.IniciarControlAfiliados(this.MiRemito.Factura, this.GestionControl);
                    VTARemitos remito = new VTARemitos();
                    AyudaProgramacion.MatchObjectProperties(this.MiRemito, remito);
                    remito.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
                    remito.Afiliado.IdAfiliado = pRemito.Afiliado.IdAfiliado;
                    remito = FacturasF.RemitosObtenerDatosPreCargados(remito);
                    if (remito.Filtro == "Precargados")
                    {
                        this.MapearObjetoControlesPrecarga(remito);
                    }
                    this.IniciarGrilla();
                    break;
                case Gestion.Modificar:
                    estados = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosRemitos));
                    estados = estados.Where(x => x.IdEstado != (int)EstadosRemitos.Baja).ToList();
                    this.ddlEstadosRemtios.DataSource = estados;
                    this.ddlEstadosRemtios.DataValueField = "IdEstado";
                    this.ddlEstadosRemtios.DataTextField = "Descripcion";
                    this.ddlEstadosRemtios.DataBind();
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlEstadosRemtios, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                    this.MiRemito = FacturasF.RemitosObtenerDatosCompletos(pRemito);
                    this.MiRemito.Afiliado = AfiliadosF.AfiliadosObtenerDatosCompletos(this.MiRemito.Afiliado);
                    this.CargarComboDomicilio();
                    this.MapearObjetoAControles(this.MiRemito);
                    this.FirmarDocumento();
                    //this.btnBuscarSocio.Enabled = false;
                    //this.txtNumeroSocio.Enabled = false;
                    this.phAgregarItem.Visible = false;
                    this.ddlFilialPuntoVenta.Enabled = false;
                    this.ddlTipoFactura.Enabled = false;
                    this.ddlPrefijoNumeroFactura.Enabled = false;
                    this.txtFechaRemito.Enabled = true;
                    this.ddlPrefijoNumeroFactura.Enabled = false;
                    this.btnImportarFactura.Visible = false;
                    this.btnImportarPresupuesto.Visible = false;
                    this.btnImportarNotaPedido.Visible = false;
                    this.ddlDomicilioEntrega.Enabled = true;
                    this.btnAgregarDomicilio.Visible = true;
                    this.txtObservacionComprobante.Enabled = true;
                    this.txtObservacionInterna.Enabled = true;
                    break;
                case Gestion.Consultar:
                    this.MiRemito = FacturasF.RemitosObtenerDatosCompletos(pRemito);
                    this.MapearObjetoAControles(this.MiRemito);
                    //this.btnBuscarSocio.Enabled = false;
                    //this.txtNumeroSocio.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.phAgregarItem.Visible = false;
                    this.btnImprimir.Visible = true;
                    // this.btnFirmaDigital.Visible = true;
                    this.FirmarDocumento();
                    this.ddlFilialPuntoVenta.Enabled = false;
                    this.ddlTipoFactura.Enabled = false;
                    this.ddlPrefijoNumeroFactura.Enabled = false;
                    this.txtFechaRemito.Enabled = false;
                    this.ddlPrefijoNumeroFactura.Enabled = false;
                    this.ddlEstadosRemtios.Enabled = false;
                    this.ddlFilialEntrega.Enabled = false;
                    this.btnImportarPresupuesto.Visible = false;
                    this.btnImportarNotaPedido.Visible = false;
                    break;
                case Gestion.Anular:
                    this.MiRemito = FacturasF.RemitosObtenerDatosCompletos(pRemito);
                    this.MapearObjetoAControles(this.MiRemito);
                    //this.btnBuscarSocio.Enabled = false;
                    //this.txtNumeroSocio.Enabled = false;
                    this.phAgregarItem.Visible = false;
                    this.ddlFilialPuntoVenta.Enabled = false;
                    this.ddlTipoFactura.Enabled = false;
                    this.ddlPrefijoNumeroFactura.Enabled = false;
                    this.txtFechaRemito.Enabled = false;
                    this.ddlPrefijoNumeroFactura.Enabled = false;
                    this.ddlEstadosRemtios.Enabled = false;
                    this.ddlFilialEntrega.Enabled = false;
                    this.btnImportarFactura.Visible = false;
                    this.btnImportarPresupuesto.Visible = false;
                    this.btnImportarNotaPedido.Visible = false;
                    break;
                default:
                    break;
            }
        }
        private void FirmarDocumento()
        {
            TGEFirmarDocumentos firmarDoc = new TGEFirmarDocumentos();
            firmarDoc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            firmarDoc.IdRefTabla = this.MiRemito.IdRemito;
            firmarDoc.Tabla = "VTARemitos";
            PropertyInfo prop = MiRemito.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() == 1).ToList()[0];
            firmarDoc.Key = prop.Name;
            firmarDoc.CodigoPlantilla = "VTARemitos";
            this.btnFirmaDigital.Visible = TGEGeneralesF.FirmarDocumentosValidar(firmarDoc);
            this.btnFirmaDigitalBaja.Visible = !this.btnFirmaDigital.Visible;
            this.copyClipboard.Visible = btnFirmaDigital.Visible;
            this.btnWhatsAppFirmarDocumento.Visible = btnFirmaDigital.Visible;
            this.hfLinkFirmarDocumento.Value = TGEGeneralesF.FirmarDocumentosArmarLinkFirmarDocumento(firmarDoc).Link;
            if (this.btnFirmaDigitalBaja.Visible)
            {
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarFirmaDigitalManuscritaBaja"));
                this.btnFirmaDigitalBaja.Attributes.Add("OnClick", funcion);
            }
        }
        private void CargarCombos()
        {
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            VTAFilialesPuntosVentas filtro = new VTAFilialesPuntosVentas();
            filtro.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
            this.MisTiposPuntosVentas = FacturasF.VTAFilialesPuntosVentasObtenerPorFilial(filtro);
            if (this.MisTiposPuntosVentas.Exists(x => x.IdTipoPuntoVenta == (int)EnumAFIPTiposPuntosVentas.ComprobanteEnLinea))
            {
                this.MisTiposPuntosVentas.Remove(this.MisTiposPuntosVentas.Find(x => x.IdTipoPuntoVenta == (int)EnumAFIPTiposPuntosVentas.ComprobanteEnLinea));
                this.MisTiposPuntosVentas = AyudaProgramacion.AcomodarIndices<VTATiposPuntosVentas>(this.MisTiposPuntosVentas);
            }
            this.ddlFilialPuntoVenta.DataSource = this.MisTiposPuntosVentas;
            this.ddlFilialPuntoVenta.DataValueField = "IdTipoPuntoVenta";
            this.ddlFilialPuntoVenta.DataTextField = "Descripcion";
            this.ddlFilialPuntoVenta.DataBind();
            if (this.ddlFilialPuntoVenta.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlFilialPuntoVenta, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            AyudaProgramacion.AgregarItemSeleccione(this.ddlPrefijoNumeroFactura, "0000");

            this.ddlFilialEntrega.DataSource = TGEGeneralesF.FilialesEntregaObtenerListaActiva();
            this.ddlFilialEntrega.DataValueField = "IdFilialEntrega";
            this.ddlFilialEntrega.DataTextField = "Filial";
            this.ddlFilialEntrega.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilialEntrega, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            TGETiposOperaciones operaciones = new TGETiposOperaciones();
            operaciones.TipoFuncionalidad.IdTipoFuncionalidad = this.paginaSegura.paginaActual.IdTipoFuncionalidad;
            operaciones.Estado.IdEstado = (int)Estados.Activo;
            this.ddlTipoOperacion.DataSource = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(operaciones);
            this.ddlTipoOperacion.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacion.DataTextField = "TipoOperacion";
            this.ddlTipoOperacion.DataBind();
            this.ddlTipoOperacion.SelectedValue = ((int)EnumTGETiposOperaciones.RemitosVentas).ToString();

            AyudaProgramacion.AgregarItemSeleccione(this.ddlDomicilioEntrega, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void CargarComboDomicilio()
        {
            this.ddlDomicilioEntrega.Items.Clear();
            this.ddlDomicilioEntrega.SelectedIndex = -1;
            this.ddlDomicilioEntrega.SelectedValue = null;
            this.ddlDomicilioEntrega.ClearSelection();
            this.ddlDomicilioEntrega.DataSource = this.MiRemito.Afiliado.Domicilios; //AfiliadosF.AfiliadosObtenerDomicilios(pAfiliado);
            this.ddlDomicilioEntrega.DataValueField = "IdDomicilio";
            this.ddlDomicilioEntrega.DataTextField = "DomicilioCompleto";
            this.ddlDomicilioEntrega.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlDomicilioEntrega, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void btnAgregarDomicilio_Click(object sender, EventArgs e)
        {
            this.ctrDomicilios.IniciarControl(new AfiDomicilios(), Gestion.Agregar);
        }
        private void CtrDomicilios_AfiliadosModificarDatosAceptar(object sender, AfiDomicilios e, Gestion pGestion)
        {
            e.IdAfiliado = this.MiRemito.Afiliado.IdAfiliado;
            e.EstadoColeccion = EstadoColecciones.Agregado;
            if (AfiliadosF.AfiliadosAgregarDomicilio(e))
            {
                this.MiRemito.Afiliado.Domicilios.Add(e);
                this.CargarComboDomicilio();
                this.ddlDomicilioEntrega.SelectedValue = e.IdDomicilio.ToString();
            }
            else
            {
                this.MostrarMensaje(e.CodigoMensaje, true, e.CodigoMensajeArgs);
            }
            this.UpdatePanel1.Update();
        }

        protected void ddlEstadosRemtios_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool deshabilitarControles = true;
            if (!string.IsNullOrEmpty(this.ddlEstadosRemtios.SelectedValue))
            {
                if (Convert.ToInt32(this.ddlEstadosRemtios.SelectedValue) == (int)EstadosRemitos.Entregado)
                {
                    this.txtFechaEntrega.Enabled = true;
                    this.rfvFechaEntrega.Enabled = true;
                    deshabilitarControles = false;
                }
            }
            if (deshabilitarControles)
            {
                this.txtFechaEntrega.Text = string.Empty;
                this.txtFechaEntrega.Enabled = false;
                this.MiRemito.FechaEntrega = default(DateTime?);
                this.rfvFechaEntrega.Enabled = false;
            }
        }
        protected void ddlFilialPuntoVenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlFilialPuntoVenta.SelectedValue))
            {
                //this.MiFactura.FilialPuntoVenta = this.MisFilialesPuntosVentas[this.ddlFilialPuntoVenta.SelectedIndex];
                //this.MiFactura.PrefijoNumeroFactura = this.MiFactura.FilialPuntoVenta.AfipPuntoVenta.ToString().PadLeft(4, '0');
                //this.txtPrefijoNumeroFactura.Text = this.MiFactura.PrefijoNumeroFactura;
                this.MiRemito.FilialPuntoVenta.TipoPuntoVenta = this.MisTiposPuntosVentas[this.ddlFilialPuntoVenta.SelectedIndex];

                //Cargo los comprobantes habilitados para el Cliente
                this.MisTiposFacturas = FacturasF.TiposFacturasSeleccionarPorRemitos(this.MiRemito);
                this.ddlTipoFactura.Items.Clear();
                this.ddlTipoFactura.SelectedValue = null;
                this.ddlTipoFactura.DataSource = this.MisTiposFacturas;
                this.ddlTipoFactura.DataValueField = "IdTipoFactura";
                this.ddlTipoFactura.DataTextField = "Descripcion";
                this.ddlTipoFactura.DataBind();
                if (this.ddlTipoFactura.Items.Count != 1)
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                if (this.MiRemito.TipoFactura.IdTipoFactura > 0)
                {
                    ListItem item = this.ddlTipoFactura.Items.FindByValue(this.MiRemito.TipoFactura.IdTipoFactura.ToString());
                    if (item == null)
                        this.ddlTipoFactura.Items.Add(new ListItem(this.MiRemito.TipoFactura.Descripcion, this.MiRemito.TipoFactura.IdTipoFactura.ToString()));
                    this.ddlTipoFactura.SelectedValue = this.MiRemito.TipoFactura.IdTipoFactura.ToString();

                }
                this.ddlTipoFactura_SelectedIndexChanged(null, EventArgs.Empty);
            }
            else
            {
                this.ddlTipoFactura.Items.Clear();
                this.ddlTipoFactura.SelectedValue = null;
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                this.MiRemito.FilialPuntoVenta = new VTAFilialesPuntosVentas();
                this.MiRemito.NumeroRemitoPrefijo = string.Empty;
                this.ddlPrefijoNumeroFactura.Items.Clear();
                this.ddlPrefijoNumeroFactura.SelectedValue = null;
                AyudaProgramacion.AgregarItemSeleccione(this.ddlPrefijoNumeroFactura, "0000");
                this.UpdatePanel1.Update();
            }
        }

        protected void ddlTipoFactura_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlTipoFactura.SelectedValue))
            {
                this.MiRemito.TipoFactura.IdTipoFactura = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].IdTipoFactura;
                this.MiRemito.TipoFactura.CodigoValor = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].CodigoValor;
                this.MiRemito.TipoFactura.Descripcion = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].Descripcion;

                this.ddlPrefijoNumeroFactura.Items.Clear();
                this.ddlPrefijoNumeroFactura.SelectedValue = null;
                this.MiRemito.FilialPuntoVenta.IdFilial = this.MiRemito.Filial.IdFilial;
                this.MiRemito.FilialPuntoVenta.IdTipoFactura = this.MiRemito.TipoFactura.IdTipoFactura;
                this.MisFilialesPuntosVentas = FacturasF.VTAFilialesPuntosVentasObtenerListaFiltro(this.MiRemito.FilialPuntoVenta);
                this.ddlPrefijoNumeroFactura.DataSource = this.MisFilialesPuntosVentas;
                this.ddlPrefijoNumeroFactura.DataValueField = "AfipPuntoVenta";
                this.ddlPrefijoNumeroFactura.DataTextField = "AfipPuntoVentaNumero";
                this.ddlPrefijoNumeroFactura.DataBind();

                if (this.MiRemito.Filtro == "Precargados")
                {
                    ListItem item = this.ddlPrefijoNumeroFactura.Items.FindByText(this.MiRemito.NumeroRemitoPrefijo);
                    if (item == null)
                        this.ddlPrefijoNumeroFactura.Items.Add(new ListItem(this.MiRemito.NumeroRemitoPrefijo, Convert.ToInt32(this.MiRemito.NumeroRemitoPrefijo).ToString()));
                    this.ddlPrefijoNumeroFactura.SelectedValue = Convert.ToInt32(this.MiRemito.NumeroRemitoPrefijo).ToString();
                }
                this.ddlPrefijoNumeroFactura_SelectedIndexChanged(null, EventArgs.Empty);
            }
            else
            {
                this.MiRemito.TipoFactura = new TGETiposFacturas();
                this.ddlPrefijoNumeroFactura.Items.Clear();
                this.ddlPrefijoNumeroFactura.SelectedValue = null;
                AyudaProgramacion.AgregarItemSeleccione(this.ddlPrefijoNumeroFactura, "0000");
                this.txtNumeroRemito.Text = string.Empty;
            }
            this.UpdatePanel1.Update();
        }
        protected void ddlPrefijoNumeroFactura_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlPrefijoNumeroFactura.SelectedValue)
                && Convert.ToInt32(this.ddlPrefijoNumeroFactura.SelectedValue) > 0)
            {
                this.MiRemito.FilialPuntoVenta.AfipPuntoVenta = Convert.ToInt32(this.ddlPrefijoNumeroFactura.SelectedValue);
                this.MiRemito.NumeroRemitoPrefijo = this.MiRemito.FilialPuntoVenta.AfipPuntoVentaNumero;
                if (FacturasF.RemitosObtenerProximoNumeroComprobanteTmp(this.MiRemito))
                {
                    this.MiRemito.NumeroRemitoPrefijo = this.MiRemito.FilialPuntoVenta.AfipPuntoVentaNumero;
                    //this.txtPrefijoNumeroFactura.Text = this.MiFactura.PrefijoNumeroFactura;
                    this.txtNumeroRemito.Text = this.MiRemito.NumeroRemitoSuFijo;
                    this.txtNumeroRemito.Enabled = Convert.ToInt32(this.ddlFilialPuntoVenta.SelectedValue) == (int)EnumAFIPTiposPuntosVentas.ComprobanteManual
                                                    || (Convert.ToInt32(this.ddlFilialPuntoVenta.SelectedValue) == (int)EnumAFIPTiposPuntosVentas.WebServiceFacturaElectronica
                                                        && this.MiRemito.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.RemitoR);
                }
                else
                {
                    //this.txtPrefijoNumeroFactura.Text = string.Empty;
                    this.txtNumeroRemito.Text = string.Empty;
                    this.txtNumeroRemito.Enabled = false;
                    this.MostrarMensaje(this.MiRemito.CodigoMensaje, true, this.MiRemito.CodigoMensajeArgs);
                }
            }
        }
        protected void ddlFilialEntrega_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlFilialEntrega.SelectedValue))
            {
                if (this.MiRemito.FilialEntrega.IdFilialEntrega != Convert.ToInt32(this.ddlFilialEntrega.SelectedValue))
                {
                    this.MiRemito.FilialEntrega.IdFilialEntrega = Convert.ToInt32(this.ddlFilialEntrega.SelectedValue);
                    //Verificar el Stock Actual de los Productos Ingresados!
                    CMPStock stock;
                    foreach (VTARemitosDetalles detalle in this.MiRemito.RemitosDetalles)
                    {
                        stock = new CMPStock();
                        stock.IdFilial = this.MiRemito.FilialEntrega.IdFilialEntrega;
                        stock.Producto.IdProducto = detalle.Producto.IdProducto;
                        if (stock.Producto.IdProducto > 0)
                            stock = ComprasF.StockObtenerPorProductoFilial(stock);
                        detalle.StockActual = stock.StockActual;
                    }
                    AyudaProgramacion.CargarGrillaListas<VTARemitosDetalles>(this.MiRemito.RemitosDetalles, false, this.gvItems, true);
                    this.items.Update();
                }
            }
        }
        #region "Mapeo de Datos"
        protected void MapearControlesAObjeto(VTARemitos pRemito)
        {
            //this.MiRemito.TipoFactura.IdTipoFactura = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].IdTipoFactura;
            //this.MiRemito.TipoFactura.CodigoValor = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].CodigoValor;
            //this.MiRemito.TipoFactura.Descripcion = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].Descripcion;
            //this.MiRemito.Afiliado.IdAfiliado = Convert.ToInt32(this.txtNumeroSocio.Text);
            this.MiRemito.FechaRemito = Convert.ToDateTime(this.txtFechaRemito.Text);
            //this.MiRemito.NumeroRemitoPrefijo = this.txtPrefijoNumeroRemito.Text;
            this.MiRemito.NumeroRemitoSuFijo = this.txtNumeroRemito.Text;

            this.MiRemito.Estado.IdEstado = Convert.ToInt32(this.ddlEstadosRemtios.SelectedValue);
            this.MiRemito.Estado.Descripcion = this.ddlEstadosRemtios.SelectedItem.Text;
            this.MiRemito.FechaEntrega = this.txtFechaEntrega.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaEntrega.Text);
            this.MiRemito.DomicilioEntrega = ddlDomicilioEntrega.SelectedValue == string.Empty ? string.Empty : this.ddlDomicilioEntrega.SelectedItem.Text;
            this.MiRemito.IdDomicilio = ddlDomicilioEntrega.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlDomicilioEntrega.SelectedValue);

            this.MiRemito.TipoOperacion.IdTipoOperacion = Convert.ToInt32(this.ddlTipoOperacion.SelectedValue);
            this.MiRemito.TipoOperacion.TipoOperacion = this.ddlTipoOperacion.SelectedItem.Text;
            this.MiRemito.ObservacionComprobante = this.txtObservacionComprobante.Text;
            this.MiRemito.ObservacionInterna = this.txtObservacionInterna.Text;
            this.MiRemito.Campos = this.ctrCamposValores.ObtenerLista();
            this.MiRemito.Comentarios = this.ctrComentarios.ObtenerLista();
            this.MiRemito.Archivos = ctrArchivos.ObtenerLista();
        }

        protected void MapearObjetoAControles(VTARemitos pRemito)
        {
            //Panel Socio --> Se cambio por ctrBuscarCliente:
            ctrBuscarCliente.IniciarControl(MiRemito.Afiliado, this.GestionControl);
            //this.txtNumeroSocio.Text = this.MiRemito.Afiliado.IdAfiliado.ToString();
            //this.txtCUIT.Text = this.MiRemito.Afiliado.CUIL.ToString();
            //this.txtSocio.Text = this.MiRemito.Afiliado.ApellidoNombre;
            //this.txtCondicionFiscal.Text = this.MiRemito.Afiliado.CondicionFiscal.Descripcion;
            //this.txtEstado.Text = this.MiRemito.Afiliado.Estado.Descripcion;
            //Panel Remito
            this.ddlFilialPuntoVenta.SelectedValue = pRemito.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta.ToString();
            ListItem item = this.ddlTipoFactura.Items.FindByValue(pRemito.TipoFactura.IdTipoFactura.ToString());
            if (item == null)
                this.ddlTipoFactura.Items.Add(new ListItem(pRemito.TipoFactura.Descripcion, pRemito.TipoFactura.IdTipoFactura.ToString()));
            this.ddlTipoFactura.SelectedValue = pRemito.TipoFactura.IdTipoFactura.ToString();
            //this.txtPrefijoNumeroFactura.Text = pFactura.PrefijoNumeroFactura;
            item = this.ddlPrefijoNumeroFactura.Items.FindByText(pRemito.NumeroRemitoPrefijo);
            if (item == null)
                this.ddlPrefijoNumeroFactura.Items.Add(new ListItem(pRemito.NumeroRemitoPrefijo, Convert.ToInt32(pRemito.NumeroRemitoPrefijo).ToString()));
            this.ddlPrefijoNumeroFactura.SelectedValue = Convert.ToInt32(pRemito.NumeroRemitoPrefijo).ToString();
            this.txtFechaRemito.Text = this.MiRemito.FechaRemito.ToShortDateString();
            //this.txtPrefijoNumeroRemito.Text = this.MiRemito.NumeroRemitoPrefijo;
            this.txtNumeroRemito.Text = this.MiRemito.NumeroRemitoSuFijo;

            item = this.ddlEstadosRemtios.Items.FindByValue(pRemito.Estado.IdEstado.ToString());
            if (item == null)
                this.ddlEstadosRemtios.Items.Add(new ListItem(pRemito.Estado.Descripcion, pRemito.Estado.IdEstado.ToString()));
            this.ddlEstadosRemtios.SelectedValue = pRemito.Estado.IdEstado.ToString();

            item = this.ddlFilialEntrega.Items.FindByValue(pRemito.FilialEntrega.IdFilialEntrega.ToString());
            if (item == null)
                this.ddlFilialEntrega.Items.Add(new ListItem(pRemito.FilialEntrega.Descripcion, pRemito.FilialEntrega.IdFilialEntrega.ToString()));
            this.ddlFilialEntrega.SelectedValue = pRemito.FilialEntrega.IdFilialEntrega.ToString();

            item = this.ddlTipoOperacion.Items.FindByValue(pRemito.TipoOperacion.IdTipoOperacion.ToString());
            if (item == null)
                this.ddlTipoOperacion.Items.Add(new ListItem(pRemito.TipoOperacion.TipoOperacion, pRemito.TipoOperacion.IdTipoOperacion.ToString()));
            this.ddlTipoOperacion.SelectedValue = pRemito.TipoOperacion.IdTipoOperacion.ToString();

            if (pRemito.IdDomicilio.HasValue)
            {
                item = this.ddlDomicilioEntrega.Items.FindByValue(pRemito.IdDomicilio.Value.ToString());
                if (item == null)
                {
                    this.ddlDomicilioEntrega.Items.Add(new ListItem(pRemito.DomicilioEntrega, pRemito.IdDomicilio.ToString()));
                }
                this.ddlDomicilioEntrega.SelectedValue = pRemito.IdDomicilio.Value.ToString();
                this.ddlDomicilioEntrega.SelectedItem.Text = pRemito.DomicilioEntrega;
            }

            AyudaProgramacion.CargarGrillaListas<VTARemitosDetalles>(this.MiRemito.RemitosDetalles, false, this.gvItems, true);
            if (MiRemito.Factura.IdFactura > 0
                && MiRemito.Factura.AcopioFinanciero.HasValue
                && MiRemito.Factura.AcopioFinanciero.Value)
            {
                this.phDetalleAcopio.Visible = true;
                hdfIdSolicitudPago.Value = MiRemito.Factura.IdFactura.ToString();
                this.txtDetalleAcopio.Text = MiRemito.DetalleAcopio;
                this.txtImporte.Text = MiRemito.Factura.ImporteSinIVA.ToString("C2");
                this.txtImporteRecibido.Text = MiRemito.ImportePrevioEntregado.HasValue ? MiRemito.ImportePrevioEntregado.Value.ToString("C2") : string.Empty;
                this.gvItems.Columns[6].Visible = true;
                this.gvItems.Columns[7].Visible = true;
                ScriptManager.RegisterStartupScript(this.items, this.items.GetType(), "CalcularItemScript", "CalcularItem();", true);
            }

            //this.items.Update();


            this.ctrCamposValores.IniciarControl(this.MiRemito, new Objeto(), this.GestionControl);
            this.ctrComentarios.IniciarControl(pRemito, this.GestionControl);
            this.ctrArchivos.IniciarControl(pRemito, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pRemito);

            this.txtObservacionComprobante.Text = this.MiRemito.ObservacionComprobante;
            this.txtObservacionInterna.Text = this.MiRemito.ObservacionInterna;

        }
        #endregion
        private void MapearObjetoControlesPrecarga(VTARemitos pRemito)
        {
            ListItem item = this.ddlFilialPuntoVenta.Items.FindByValue(pRemito.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta.ToString());
            if (item == null)
                this.ddlFilialPuntoVenta.Items.Add(new ListItem(pRemito.FilialPuntoVenta.TipoPuntoVenta.Descripcion, pRemito.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta.ToString()));
            this.ddlFilialPuntoVenta.SelectedValue = pRemito.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta.ToString(); //Convert.ToInt32(pRemito.PrefijoNumeroFactura).ToString();
            this.ddlFilialPuntoVenta_SelectedIndexChanged(null, EventArgs.Empty);

            item = this.ddlFilialEntrega.Items.FindByValue(pRemito.FilialEntrega.IdFilialEntrega.ToString());
            if (item == null)
                this.ddlFilialEntrega.Items.Add(new ListItem(pRemito.FilialEntrega.Descripcion, pRemito.FilialEntrega.IdFilialEntrega.ToString()));
            this.ddlFilialEntrega.SelectedValue = pRemito.FilialEntrega.IdFilialEntrega.ToString();
            this.ddlFilialEntrega_SelectedIndexChanged(null, EventArgs.Empty);

            if (this.MiRemito.TipoFactura.IdTipoFactura > 0)
            {
                item = this.ddlTipoFactura.Items.FindByValue(pRemito.TipoFactura.IdTipoFactura.ToString());
                if (item == null)
                    this.ddlTipoFactura.Items.Add(new ListItem(pRemito.TipoFactura.Descripcion, pRemito.TipoFactura.IdTipoFactura.ToString()));
                this.ddlTipoFactura.SelectedValue = pRemito.TipoFactura.IdTipoFactura.ToString();
            }
            this.ddlTipoFactura_SelectedIndexChanged(null, EventArgs.Empty);

            item = this.ddlEstadosRemtios.Items.FindByValue(pRemito.Estado.IdEstado.ToString());
            if (item == null)
                this.ddlEstadosRemtios.Items.Add(new ListItem(pRemito.Estado.Descripcion, pRemito.Estado.IdEstado.ToString()));
            this.ddlEstadosRemtios.SelectedValue = pRemito.Estado.IdEstado.ToString();

            item = this.ddlTipoOperacion.Items.FindByValue(pRemito.TipoOperacion.IdTipoOperacion.ToString());
            if (item == null)
                this.ddlTipoOperacion.Items.Add(new ListItem(pRemito.TipoOperacion.TipoOperacion, pRemito.TipoOperacion.IdTipoOperacion.ToString()));
            this.ddlTipoOperacion.SelectedValue = pRemito.TipoOperacion.IdTipoOperacion.ToString();

            item = this.ddlPrefijoNumeroFactura.Items.FindByText(pRemito.NumeroRemitoPrefijo);
            if (item == null)
                this.ddlPrefijoNumeroFactura.Items.Add(new ListItem(pRemito.NumeroRemitoPrefijo, Convert.ToInt32(pRemito.NumeroRemitoPrefijo).ToString()));
            this.ddlPrefijoNumeroFactura.SelectedValue = Convert.ToInt32(pRemito.NumeroRemitoPrefijo).ToString();

            //this.txtPrefijoNumeroRemito.Text = this.MiRemito.NumeroRemitoPrefijo;
            //this.txtNumeroRemito.Text = pRemito.NumeroRemitoPrefijo;
            this.txtFechaRemito.Text = pRemito.FechaRemito.ToShortDateString();
            this.txtFechaEntrega.Text = pRemito.FechaRemito.ToShortDateString();
        }

        #region "Control Afiliado"
        void CtrBuscarCliente_BuscarCliente(AfiAfiliados e)
        {
            if (e.IdAfiliado > 0)
            {
                this.ddlFilialPuntoVenta_SelectedIndexChanged(null, EventArgs.Empty);
                MiRemito.Afiliado = ctrBuscarCliente.MiAfiliado;
                MiRemito.Afiliado.Domicilios = AfiliadosF.AfiliadosObtenerDomicilios(MiRemito.Afiliado);
                CargarComboDomicilio();
                if (this.GestionControl == Gestion.Agregar)
                {
                    if (MiRemito.Afiliado.Domicilios.Exists(x => x.Predeterminado))
                    {
                        this.ddlDomicilioEntrega.SelectedValue = MiRemito.Afiliado.Domicilios.First(x => x.Predeterminado).IdDomicilio.ToString();
                    }
                }
                UpdatePanel1.Update();
            }
        }
        //void ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar(AfiAfiliados pAfiliado)
        //{
        //    this.MiRemito.Afiliado = pAfiliado;
        //    this.MapearObjetoAControlesAfiliado(pAfiliado);
        //    this.ddlFilialPuntoVenta_SelectedIndexChanged(null, EventArgs.Empty);
        //    this.upPnlAfiliado.Update();
        //    this.UpdatePanel1.Update();
        //}

        //private void MapearObjetoAControlesAfiliado(AfiAfiliados pAfiliado)
        //{
        //    if (this.MisParametrosUrl.Contains("IdAfiliado"))
        //        this.MisParametrosUrl["IdAfiliado"] = pAfiliado.IdAfiliado;
        //    this.txtNumeroSocio.Text = pAfiliado.IdAfiliado.ToString();
        //    this.txtSocio.Text = pAfiliado.RazonSocial;
        //    lblCUIT.Text = pAfiliado.TipoDocumento.TipoDocumento;
        //    this.txtCUIT.Text = pAfiliado.NumeroDocumento.ToString();
        //    this.txtEstado.Text = pAfiliado.Estado.Descripcion;
        //    this.txtCondicionFiscal.Text = pAfiliado.CondicionFiscal.Descripcion;
        //    this.MiRemito.Afiliado = AfiliadosF.AfiliadosObtenerDatosCompletos(pAfiliado);
        //    CargarComboDomicilio();
        //    if (this.GestionControl == Gestion.Agregar)
        //    {
        //        if (pAfiliado.Domicilios.Exists(x => x.Predeterminado))
        //        {
        //            this.ddlDomicilioEntrega.SelectedValue = pAfiliado.Domicilios.First(x => x.Predeterminado).IdDomicilio.ToString();
        //        }
        //            if (this.MiRemito.RemitosDetalles.Exists(x => x.Producto.IdProducto != 0))
        //        {
        //            this.MiRemito.RemitosDetalles.Clear();
        //            this.IniciarGrilla();
        //            this.items.Update();
        //        }
        //    }
        //}

        //protected void txtNumeroSocio_TextChanged(object sender, EventArgs e)
        //{
        //    string txtNumeroSocio = ((TextBox)sender).Text;
        //    AfiAfiliados parametro = new AfiAfiliados();
        //    parametro.IdAfiliado = Convert.ToInt32(txtNumeroSocio);
        //    this.MiRemito.Afiliado = AfiliadosF.AfiliadosObtenerDatos(parametro); //y aca buscar por id afiliado
        //    if (this.MiRemito.Afiliado.IdAfiliado != 0)
        //    {
        //        this.ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar(this.MiRemito.Afiliado);
        //    }
        //    else
        //    {
        //        this.txtNumeroSocio.Text = string.Empty;
        //        parametro.CodigoMensaje = "NumeroSocioNoExiste";
        //        this.upPnlAfiliado.Update();
        //        this.MostrarMensaje(parametro.CodigoMensaje, true);
        //    }
        //}

        //protected void btnBuscarCliente_Click(object sender, EventArgs e)
        //{
        //    this.ctrBuscarClientePopUp.IniciarControl(true);
        //}
        #endregion

        #region "PopUp Facturas"
        private void CtrBuscarFacturasPopUp_FacturasBuscarSeleccionar(List<VTAFacturasDetalles> e)
        {
            if (ctrBuscarCliente.MiAfiliado.IdAfiliado < 0)
            {
                this.MiRemito.CodigoMensaje = "RemitoSeleccioneCliente";
                this.MostrarMensaje(this.MiRemito.CodigoMensaje, true);
            }
            else
            {
                //this.MiRemito.RemitosDetalles[this.MiIndiceDetalleModificar].Producto = e.FacturasDetalles[this.MiIndiceDetalleModificar].Producto;
                if (e.Count() > 0)
                {
                    btnEliminarAcopio_Click(btnEliminarAcopio, EventArgs.Empty);
                    MapFacturaRemito(e);
                    AyudaProgramacion.CargarGrillaListas<VTARemitosDetalles>(this.MiRemito.RemitosDetalles, false, this.gvItems, true);
                    this.items.Update();
                    //this.txtCodigoFactura.Text = e.IdFactura.ToString();
                    //this.txtPrefijoNumeroFactura.Text = e.PrefijoNumeroFactura;
                    //this.txtNumeroFactura.Text = e.NumeroFactura;
                    //this.pnlFacturas.Update();

                }
                else
                {
                    this.MiRemito.CodigoMensaje = "RemitoFacturasCantidadItems";

                }
            }
        }

        void MapFacturaRemito(List<VTAFacturasDetalles> pFacturasDetalles)
        {
            List<VTARemitosDetalles> lista = new List<VTARemitosDetalles>();
            TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ValidarStockNegativo);
            bool validarStock = paramValor.ParametroValor == string.Empty ? false : Convert.ToBoolean(paramValor.ParametroValor);
            VTARemitosDetalles remDet;
            foreach (VTAFacturasDetalles det in pFacturasDetalles)
            {
                remDet = new VTARemitosDetalles();
                remDet.EstadoColeccion = EstadoColecciones.Agregado;
                remDet.Producto = det.ListaPrecioDetalle.Producto;
                remDet.StockActual = det.ListaPrecioDetalle.StockActual;
                remDet.CantidadEntregada = det.CantidadEntregada.HasValue ? det.CantidadEntregada.Value : 0;
                remDet.Descripcion = det.Descripcion;
                remDet.TipoNumeroFactura = det.TipoNumeroFactura;
                remDet.CantidadRestante = det.CantidadRestante;
                if (validarStock)
                {
                    if (det.CantidadRestante <= det.ListaPrecioDetalle.StockActual)
                    {
                        remDet.Cantidad = det.CantidadRestante.Value;
                    }
                    else
                    {
                        remDet.Cantidad = det.ListaPrecioDetalle.StockActual;
                    }
                }
                else
                {
                    remDet.Cantidad = det.CantidadRestante.Value;
                }
                remDet.IdFacturaDetalle = det.IdFacturaDetalle;
                lista.Add(remDet);
                remDet.IndiceColeccion = lista.IndexOf(remDet);
            }
            this.MiRemito.RemitosDetalles = lista;
            //this.MiRemito.Factura = pFactura;
        }

        private void CtrBuscarFacturasPopUp_FacturasBuscarAcopio(VTAFacturas e)
        {
            if (e.IdFactura > 0)
            {
                this.MiRemito.Factura = e;
                this.hdfIdSolicitudPago.Value = e.IdFactura.ToString();
                this.hdfFechaAcopio.Value = e.FechaFactura.ToShortDateString();
                this.phDetalleAcopio.Visible = true;
                this.btnEliminarAcopio.Visible = true;
                this.gvItems.Columns[6].Visible = true;
                this.gvItems.Columns[7].Visible = true;
                this.txtDetalleAcopio.Text = e.Filtro;
                this.txtImporte.Text = e.ImporteSinIVA.ToString("C2");
                this.txtImporteRecibido.Text = e.ImporteParcial.ToString("C2");
                MiRemito.ImportePrevioEntregado = e.ImporteParcial;
                MiRemito.RemitosDetalles = new List<VTARemitosDetalles>();
                IniciarGrilla();
            }
            else
            {
                this.MiRemito.Factura = new VTAFacturas();
                MiRemito.ImportePrevioEntregado = default(decimal?);
                this.hdfIdSolicitudPago.Value = "0";
                this.hdfFechaAcopio.Value = string.Empty;
                this.phDetalleAcopio.Visible = false;
                this.gvItems.Columns[6].Visible = false;
                this.gvItems.Columns[7].Visible = false;
                this.txtDetalleAcopio.Text = string.Empty;
                this.txtImporte.Text = string.Empty;
                this.txtImporteRecibido.Text = string.Empty;
            }
            ScriptManager.RegisterStartupScript(this.items, this.items.GetType(), "CalcularItemScript", "CalcularItem();", true);
            this.items.Update();
        }

        protected void btnEliminarAcopio_Click(object sender, EventArgs e)
        {
            this.MiRemito.Factura = new VTAFacturas();
            MiRemito.ImportePrevioEntregado = default(decimal?);
            this.hdfIdSolicitudPago.Value = "0";
            this.phDetalleAcopio.Visible = false;
            this.gvItems.Columns[6].Visible = false;
            this.gvItems.Columns[7].Visible = false;
            this.txtDetalleAcopio.Text = string.Empty;
            this.items.Update();
        }

        protected void btnImportarFactura_Click(object sender, EventArgs e)
        {
            if (ctrBuscarCliente.MiAfiliado.IdAfiliado < 0)
            {
                this.MiRemito.CodigoMensaje = "RemitoSeleccioneSocio";
                this.MostrarMensaje(this.MiRemito.CodigoMensaje, true);
                return;
            }
            if (string.IsNullOrEmpty(this.ddlFilialEntrega.SelectedValue))
            {
                this.MostrarMensaje("ValidarSeleccionarFilialEntrega", true);
                return;
            }
            VTAFacturas factura = new VTAFacturas();
            factura.Afiliado = ctrBuscarCliente.MiAfiliado;
            factura.Filial.IdFilial = Convert.ToInt32(this.ddlFilialEntrega.SelectedValue);
            this.ctrBuscarFacturasPopUp.IniciarControl(factura);

        }

        //protected void txtCodigoFactura_TextChanged(object sender, EventArgs e)
        //{
        //    string txtCodigo = ((TextBox)sender).Text;
        //    VTAFacturas factura = new VTAFacturas();
        //    factura.IdFactura = Convert.ToInt32(this.txtCodigoFactura.Text);
        //    //parametro.Afiliado.NumeroSocio = this.txtNumeroSocio.Text;
        //    factura = FacturasF.FacturasObtenerDatosCompletos(factura);
        //    //this.MiRemito.Factura.IdFactura = Convert.ToInt32(parametro.IdFactura);

        //    if (factura.IdFactura > 0 && factura.Afiliado.IdAfiliado == (this.txtNumeroSocio.Text == string.Empty ? 0 : Convert.ToInt32(this.txtNumeroSocio.Text)))
        //        this.ctrBuscarFacturasPopUp_FacturasBuscarSeleccionar(factura);
        //    else
        //    {
        //        this.txtCodigoFactura.Text = null;
        //        this.txtPrefijoNumeroFactura.Text = null;
        //        this.txtNumeroFactura.Text = null;
        //        this.pnlFacturas.Update();
        //        this.VaciarGrilla();

        //        factura.CodigoMensaje = "SocioFacturaDiferentes";
        //        this.MostrarMensaje(factura.CodigoMensaje, true);
        //    }

        //}
        #endregion

        #region "PopUpProductos"
        //void ctrBuscarProductoPopUp_ProductosBuscarSeleccionar(CMPProductos e)
        //{
        //    this.MiRemito.RemitosDetalles[this.MiIndiceDetalleModificar].Producto = e;
        //    this.MiRemito.RemitosDetalles[this.MiIndiceDetalleModificar].StockActual = e.StockActual;
        //    AyudaProgramacion.CargarGrillaListas<VTARemitosDetalles>(this.MiRemito.RemitosDetalles, false, this.gvItems, true);
        //    AgregarItemGrilla();
        //    this.items.Update();
        //}

        //private void CtrBuscarProductoPrecioPoup_ProductosBuscarSeleccionar(CMPListasPreciosDetalles e)
        //{
        //    this.MiRemito.RemitosDetalles[this.MiIndiceDetalleModificar].Producto = e.Producto;
        //    this.MiRemito.RemitosDetalles[this.MiIndiceDetalleModificar].PrecioUnitario = e.PrecioConMargen;
        //    this.MiRemito.RemitosDetalles[this.MiIndiceDetalleModificar].NoIncluidoEnAcopio = e.NoIncluidoEnAcopio;
        //    MiRemito.RemitosDetalles[this.MiIndiceDetalleModificar].Cantidad = Convert.ToDecimal(1.00); //Por defecto 1
        //    AyudaProgramacion.CargarGrillaListas<VTARemitosDetalles>(this.MiRemito.RemitosDetalles, false, this.gvItems, true);
        //    AgregarItemGrilla();
        //    ScriptManager.RegisterStartupScript(this.items, this.items.GetType(), "CalcularItemScript", "CalcularItem();", true);
        //    this.items.Update();
        //}

        //protected void txtCodigo_TextChanged(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(this.ddlFilialEntrega.SelectedValue))
        //    {
        //        this.MostrarMensaje("ValidarSeleccionarFilialEntrega", true);
        //        return;
        //    }
        //    CMPProductos filtro = new CMPProductos();
        //    filtro.IdFilial = Convert.ToInt32(this.ddlFilialEntrega.SelectedValue);
        //    GridViewRow row = ((GridViewRow)((TextBox)sender).Parent.Parent);
        //    int IndiceColeccion = row.RowIndex;
        //    this.MiIndiceDetalleModificar = IndiceColeccion;
        //    string contenido = ((TextBox)sender).Text;

        //    if (MiRemito.Factura.IdFactura > 0)
        //    {
        //        CMPListasPreciosDetalles itemFiltro = new CMPListasPreciosDetalles();
        //        itemFiltro.Producto.IdProducto = contenido == string.Empty ? 0 : Convert.ToInt32(contenido);
        //        if (itemFiltro.Producto.IdProducto > 0)
        //        {
        //            itemFiltro.NoIncluidoEnAcopio = true;
        //            itemFiltro.Fecha = MiRemito.Factura.FechaFactura;
        //            itemFiltro.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
        //            itemFiltro.ListaPrecio.IdAfiliado = MiRemito.Afiliado.IdAfiliado;
        //            itemFiltro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
        //            itemFiltro = ComprasF.ListasPreciosDetallesObtenerDatosPorProducto(itemFiltro);
        //            if (itemFiltro.IdListaPrecioDetalle > 0)
        //                this.CtrBuscarProductoPrecioPoup_ProductosBuscarSeleccionar(itemFiltro);
        //            else
        //                ((TextBox)sender).Text = "0";
        //        }
        //    }
        //    else
        //    {
        //        this.MiRemito.RemitosDetalles[IndiceColeccion].Producto.IdProducto = Convert.ToInt32(contenido);
        //        this.MiRemito.RemitosDetalles[IndiceColeccion].Producto.Venta = true;
        //        this.MiRemito.RemitosDetalles[IndiceColeccion].Producto = ComprasF.ProductosObtenerPorIdProducto(this.MiRemito.RemitosDetalles[IndiceColeccion].Producto);
        //        if (this.MiRemito.RemitosDetalles[IndiceColeccion].Producto.IdProducto == 0)
        //        {
        //            //this.ctrBuscarProductoPopUp.IniciarControl(EnumTiposProductos.Ventas, filtro);
        //        }
        //        else
        //        {
        //            CMPStock stock = new CMPStock();
        //            stock.IdFilial = filtro.IdFilial;
        //            stock.Producto.IdProducto = this.MiRemito.RemitosDetalles[IndiceColeccion].Producto.IdProducto;
        //            stock = ComprasF.StockObtenerPorProductoFilial(stock);
        //            this.MiRemito.RemitosDetalles[IndiceColeccion].StockActual = stock.StockActual;
        //            AgregarItemGrilla();
        //            AyudaProgramacion.CargarGrillaListas<VTARemitosDetalles>(this.MiRemito.RemitosDetalles, false, this.gvItems, true);
        //        }
        //    }
        //}

        //private void AgregarItemGrilla()
        //{
        //    if (!this.MiRemito.RemitosDetalles.Exists(x => x.Producto.IdProducto == 0))
        //        btnAgregarItem_Click(btnAgregarItem, EventArgs.Empty);
        //}
        #endregion

        #region Importar Presupuesto
        private void CtrBuscarPresupuestoPopUp_PresupuestosBuscarSeleccionar(VTAPresupuestos e)
        {
            this.MapearPresupuestoAFactura(e);
            AyudaProgramacion.CargarGrillaListas<VTARemitosDetalles>(this.MiRemito.RemitosDetalles, false, gvItems, true);
            //ScriptManager.RegisterStartupScript(this.items, this.items.GetType(), "CalcularItemScript", "CalcularItem();", true);
            this.items.Update();
        }

        private void MapearPresupuestoAFactura(VTAPresupuestos e)
        {
            List<VTARemitosDetalles> lista = new List<VTARemitosDetalles>();
            VTARemitosDetalles fDetalle;
            this.MiRemito.RemitosDetalles = new List<VTARemitosDetalles>();

            foreach (VTAPresupuestosDetalles detPresu in e.PresupuestosDetalles)
            {
                fDetalle = new VTARemitosDetalles();
                fDetalle.Producto.IdProducto = detPresu.ListaPrecioDetalle.Producto.IdProducto;//ver como se carga la lista de precio
                fDetalle.IdListaPrecioDetalle = detPresu.ListaPrecioDetalle.IdListaPrecioDetalle;
                fDetalle.Producto.Descripcion = detPresu.DescripcionProducto;
                fDetalle.Descripcion = detPresu.Descripcion;
                fDetalle.Cantidad = detPresu.Cantidad.HasValue ? detPresu.Cantidad.Value : 0;
                CMPStock stock = new CMPStock();
                stock.IdFilial = this.MiRemito.FilialEntrega.IdFilial;
                stock.Producto.IdProducto = fDetalle.Producto.IdProducto;
                stock = ComprasF.StockObtenerPorProductoFilial(stock);
                fDetalle.StockActual = stock.StockActual;
                lista.Add(fDetalle);
                fDetalle.IndiceColeccion = lista.IndexOf(fDetalle);
            }

            this.MiRemito.RemitosDetalles = lista;
        }

        protected void btnImportarPresupuesto_Click(object sender, EventArgs e)
        {
            if (ctrBuscarCliente.MiAfiliado.IdAfiliado < 0)
            {
                this.MiRemito.CodigoMensaje = "RemitoSeleccioneSocio";
                this.MostrarMensaje(this.MiRemito.CodigoMensaje, true);
                return;
            }
            if (string.IsNullOrEmpty(this.ddlFilialEntrega.SelectedValue))
            {
                this.MostrarMensaje("ValidarSeleccionarFilialEntrega", true);
                return;
            }

            VTAPresupuestos remito = new VTAPresupuestos();
            remito.Afiliado = ctrBuscarCliente.MiAfiliado;
            this.ctrBuscarPresupuestoPopUp.IniciarControl(remito);
        }
        #endregion

        #region Importar Nota Pedido
        void ctrBuscarNotaPedidoPopUp_NotasPedidoBuscarSeleccionar(VTANotasPedidos e)
        {
            MiNotaPedido = e;

            this.MapearNotaPedidoAFactura(e);
            AyudaProgramacion.CargarGrillaListas<VTARemitosDetalles>(this.MiRemito.RemitosDetalles, false, gvItems, true);
            //ScriptManager.RegisterStartupScript(this.items, this.items.GetType(), "CalcularItemScript", "CalcularItem();", true);
            this.items.Update();
        }

        private void MapearNotaPedidoAFactura(VTANotasPedidos e)
        {
            List<VTARemitosDetalles> lista = new List<VTARemitosDetalles>();
            TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ValidarStockNegativo);
            bool validarStock = paramValor.ParametroValor == string.Empty ? false : Convert.ToBoolean(paramValor.ParametroValor);
            VTARemitosDetalles fDetalle;

            foreach (VTANotasPedidosDetalles detNotaPedido in e.NotasPedidosDetalles)
            {
                fDetalle = new VTARemitosDetalles();
                fDetalle.Producto.IdProducto = detNotaPedido.Producto.IdProducto;//ver como se carga la lista de precio
                fDetalle.IdListaPrecioDetalle = detNotaPedido.ListaPrecioDetalle.IdListaPrecioDetalle;
                fDetalle.Producto.Descripcion = detNotaPedido.Producto.Descripcion;
                fDetalle.Descripcion = detNotaPedido.Descripcion;
                fDetalle.Cantidad = detNotaPedido.Cantidad.HasValue ? detNotaPedido.Cantidad.Value : 0;
                fDetalle.StockActual = detNotaPedido.ListaPrecioDetalle.StockActual;
                fDetalle.CantidadRestante = detNotaPedido.CantidadRemitada;

                if (validarStock)
                {
                    if (detNotaPedido.CantidadRemitada <= detNotaPedido.ListaPrecioDetalle.StockActual)
                    {
                        fDetalle.Cantidad = detNotaPedido.CantidadRemitada.Value;
                    }
                    else
                    {
                        fDetalle.Cantidad = detNotaPedido.ListaPrecioDetalle.StockActual;
                    }
                }
                else
                {
                    fDetalle.Cantidad = detNotaPedido.CantidadRemitada.Value;
                }

                fDetalle.NotasPedidosDetalles.Add(detNotaPedido);
                lista.Add(fDetalle);
                fDetalle.IndiceColeccion = lista.IndexOf(fDetalle);
            }

            this.MiRemito.RemitosDetalles = lista;
        }

        protected void btnImportarNotaPedido_Click(object sender, EventArgs e)
        {
            if (ctrBuscarCliente.MiAfiliado.IdAfiliado < 0)
            {
                this.MiRemito.CodigoMensaje = "RemitoSeleccioneSocio";
                this.MostrarMensaje(this.MiRemito.CodigoMensaje, true);
                return;
            }
            if (string.IsNullOrEmpty(this.ddlFilialEntrega.SelectedValue))
            {
                this.MostrarMensaje("ValidarSeleccionarFilialEntrega", true);
                return;
            }

            VTANotasPedidos notaPedido = new VTANotasPedidos();
            notaPedido.Afiliado = ctrBuscarCliente.MiAfiliado;
            this.ctrBuscarNotaPedidoPopUp.IniciarControl(notaPedido);
        }

        #endregion

        #region Grilla

        private void IniciarGrilla()
        {
            VTARemitosDetalles item;
            for (int i = 0; i < 5; i++)
            {
                item = new VTARemitosDetalles();
                item.EstadoColeccion = EstadoColecciones.AgregadoPrevio;
                this.MiRemito.RemitosDetalles.Add(item);
                item.IndiceColeccion = this.MiRemito.RemitosDetalles.IndexOf(item);
            }
            AyudaProgramacion.CargarGrillaListas<VTARemitosDetalles>(this.MiRemito.RemitosDetalles, false, this.gvItems, true);
        }
        private void VaciarGrilla()
        {
            this.MiRemito.RemitosDetalles.Clear();
            this.IniciarGrilla();
            this.items.Update();
        }
        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {
            this.PersistirDatosGrilla();
            this.AgregarItem();
            this.txtCantidadAgregar.Text = string.Empty;
            //VTARemitosDetalles item;
            //item = new VTARemitosDetalles();
            //this.MiRemito.RemitosDetalles.Add(item);
            //item.IndiceColeccion = this.MiRemito.RemitosDetalles.IndexOf(item);
            //AyudaProgramacion.CargarGrillaListas<VTARemitosDetalles>(this.MiRemito.RemitosDetalles, false, this.gvItems, true);
        }
        private void AgregarItem()
        {
            VTARemitosDetalles item;
            if (this.txtCantidadAgregar.Text == string.Empty || txtCantidadAgregar.Text == "0")
            {
                this.txtCantidadAgregar.Text = "1";
            }
            int cantidad = Convert.ToInt32(this.txtCantidadAgregar.Text);
            for (int i = 0; i < cantidad; i++)
            {
                item = new VTARemitosDetalles();
                this.MiRemito.RemitosDetalles.Add(item);
                item.IndiceColeccion = this.MiRemito.RemitosDetalles.IndexOf(item);
                item.IdFacturaDetalle = item.IndiceColeccion * -1;
                //this.gvItems.Rows[item.IndiceColeccion].FindControl("ddlProducto").Focus();
            }
            AyudaProgramacion.CargarGrillaListas<VTARemitosDetalles>(this.MiRemito.RemitosDetalles, false, this.gvItems, true);
            //ScriptManager.RegisterStartupScript(this.upDetalleGastos, this.upDetalleGastos.GetType(), "CalcularPrecioScript", "CalcularPrecio();", true);
        }
        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            this.MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            if (e.CommandName == "Borrar")
            {
                this.MiRemito.RemitosDetalles.RemoveAt(this.MiIndiceDetalleModificar);
                this.MiRemito.RemitosDetalles = AyudaProgramacion.AcomodarIndices<VTARemitosDetalles>(this.MiRemito.RemitosDetalles);
                AyudaProgramacion.CargarGrillaListas<VTARemitosDetalles>(this.MiRemito.RemitosDetalles, false, this.gvItems, true);
            }
            //else if (e.CommandName == "BuscarProducto")
            //{
            //    if (string.IsNullOrEmpty(this.ddlFilialEntrega.SelectedValue))
            //    {
            //        this.MostrarMensaje("ValidarSeleccionarFilialEntrega", true);
            //        return;
            //    }
            //    if (MiRemito.Factura.IdFactura > 0)
            //    {
            //        //hdfIdSolicitudPago.value > 0
            //        CMPListasPrecios filtro = new CMPListasPrecios();
            //        filtro.IdAfiliado = MiRemito.Afiliado.IdAfiliado;
            //        filtro.FechaAcopio = MiRemito.Factura.FechaFactura;
            //        filtro.NoIncluidoEnAcopio = true;
            //        //this.ctrBuscarProductoPrecioPoup.IniciarControl(filtro);
            //    }
            //    else
            //    {
            //        //hdfIdSolicitudPago.value == 0  or ""
            //        CMPProductos filtro = new CMPProductos();
            //        filtro.IdFilial = Convert.ToInt32(this.ddlFilialEntrega.SelectedValue);
            //        //this.ctrBuscarProductoPopUp.IniciarControl(EnumTiposProductos.Ventas, filtro);
            //    }
            //}

        }

        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                VTARemitosDetalles item = (VTARemitosDetalles)e.Row.DataItem;

                DropDownList ddlProducto = ((DropDownList)e.Row.FindControl("ddlProducto"));
                if (item.Producto.IdProducto > 0)
                    ddlProducto.Items.Add(new ListItem(item.Producto.Descripcion, item.Producto.IdProducto.ToString()));


                //NumericTextBox codigo = (NumericTextBox)e.Row.FindControl("txtCodigo");
                //TextBox producto = (TextBox)e.Row.FindControl("txtProducto");
                TextBox descripcion = (TextBox)e.Row.FindControl("txtDescripcion");
                //ImageButton btnBuscarProducto = (ImageButton)e.Row.FindControl("btnBuscarProducto");
                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                Image imgNoIncluidoEnAcopio = (Image)e.Row.FindControl("imgNoIncluidoEnAcopio");
                Evol.Controls.CurrencyTextBox cantidad = (Evol.Controls.CurrencyTextBox)e.Row.FindControl("txtCantidad");
                imgNoIncluidoEnAcopio.Visible = item.NoIncluidoEnAcopio;
                Label lblProductoDescripcion = (Label)e.Row.FindControl("lblProductoDescripcion");
                switch (this.GestionControl)
                {
                    case Gestion.Agregar:
                        //btnBuscarProducto.Visible = true;
                        //codigo.Enabled = true;
                        //producto.Enabled = true;
                        ddlProducto.Enabled = true;
                        descripcion.Enabled = true;
                        cantidad.Enabled = true;
                        btnEliminar.Visible = true;
                        if (item.IdFacturaDetalle > 0)
                        {
                            lblProductoDescripcion.Visible = true;
                            descripcion.Visible = false;
                            cantidad.Attributes.Add("onchange", "CalcularItem();");
                        }
                        if (this.MiRemito.Factura.IdFactura > 0)
                        {
                            cantidad.Attributes.Add("onchange", "CalcularItem();");
                            Evol.Controls.CurrencyTextBox txtPrecioUnitario = (Evol.Controls.CurrencyTextBox)e.Row.FindControl("txtPrecioUnitario");
                            txtPrecioUnitario.Attributes.Add("onchange", "CalcularItem();");
                            txtPrecioUnitario.Enabled = true;
                            string numberSymbol = txtPrecioUnitario.Prefix == string.Empty ? "N" : "C";
                            decimal precioUni = item.PrecioUnitario.HasValue ? item.PrecioUnitario.Value : 0;
                            txtPrecioUnitario.Text = precioUni.ToString(string.Concat(numberSymbol, "2"));
                        }
                        //if (item.IdFacturaDetalle == null)
                        //{
                        //    btnBuscarProducto.Visible = true;
                        //}
                        //else
                        //{                            
                        //    btnBuscarProducto.Visible = false;
                        //    codigo.Enabled = false;
                        //}
                        break;
                    case Gestion.Modificar:
                        btnEliminar.Visible = false;
                        //btnBuscarProducto.Visible = false;
                        cantidad.Enabled = false;
                        //producto.Enabled = false;
                        descripcion.Enabled = false;
                        //codigo.Enabled = false;
                        ddlProducto.Enabled = false;
                        break;
                    case Gestion.Consultar:
                        lblProductoDescripcion.Visible = true;
                        TextBox txtDescripcion = (TextBox)e.Row.FindControl("txtDescripcion");
                        txtDescripcion.Visible = false;
                        Label lblProducto = (Label)e.Row.FindControl("lblProducto");
                        lblProducto.Visible = true;
                        lblProducto.Enabled = true;
                        ddlProducto.Visible = false;
                        btnEliminar.Visible = false;
                        //btnBuscarProducto.Visible = false;
                        cantidad.Enabled = false;
                        //producto.Enabled = false;
                        //descripcion.Enabled = false;
                        //descripcion.Visible = item.Descripcion.Trim().Length > 0;
                        //codigo.Enabled = false;
                        ddlProducto.Enabled = false;
                        break;
                    case Gestion.Anular:
                        btnEliminar.Visible = false;
                        //btnBuscarProducto.Visible = false;
                        cantidad.Enabled = false;
                        //producto.Enabled = false;
                        descripcion.Enabled = false;
                        //codigo.Enabled = false;
                        ddlProducto.Enabled = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private void PersistirDatosGrilla()
        {
            VTARemitosDetalles det;
            foreach (GridViewRow fila in this.gvItems.Rows)
            {
                DropDownList ddlProducto = ((DropDownList)fila.FindControl("ddlProducto"));
                HiddenField hdfIdProducto = (HiddenField)fila.FindControl("hdfIdProducto");
                HiddenField hdfProductoDetalle = (HiddenField)fila.FindControl("hdfProductoDetalle");
                bool hdfNoIncluidoEnAcopio = ((HiddenField)fila.FindControl("hdfNoIncluidoEnAcopio")).Value == string.Empty ? false : Convert.ToBoolean(((HiddenField)fila.FindControl("hdfNoIncluidoEnAcopio")).Value);
                //string cantidad = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtCantidad")).Decimal;
                decimal cantidad = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtCantidad")).Decimal;
                //decimal cantidaddecimal = ((TextBox)fila.FindControl("txtCantidad")).Text == string.Empty ? 0 : decimal.Parse(((TextBox)fila.FindControl("txtCantidad")).Text, NumberStyles.Currency);

                string descripcion = ((TextBox)fila.FindControl("txtDescripcion")).Text;
                Label lblStockActual = (Label)fila.FindControl("lblStockActual");
                //HiddenField hdfStockActual = (HiddenField)fila.FindControl("hdfStockActual");
                decimal hdfStockActual = ((HiddenField)fila.FindControl("hdfStockActual")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfStockActual")).Value.ToString().Replace('.', ','));

                bool hdfStockeable = ((HiddenField)fila.FindControl("hdfStockeable")).Value == string.Empty ? false : Convert.ToBoolean(((HiddenField)fila.FindControl("hdfStockeable")).Value);
                //if (cantidad != 0)
                //{

                //    det = this.MiRemito.RemitosDetalles[fila.RowIndex] == null ? new VTARemitosDetalles() : this.MiRemito.RemitosDetalles[fila.RowIndex];

                if (fila.RowIndex >= 0 && fila.RowIndex < this.MiRemito.RemitosDetalles.Count)
                {
                    det = this.MiRemito.RemitosDetalles[fila.RowIndex];
                }
                else
                {
                    det = new VTARemitosDetalles();
                }


                if (cantidad > 0)
                {
                    det.Cantidad = cantidad;//decimal.Parse(cantidad.Replace(".", ","), NumberStyles.AllowDecimalPoint); //Convert.ToDecimal(cantidad);
                }
                //det.Cantidad = cantidad;
                det.Descripcion = descripcion;

                if (hdfStockActual > 0)
                {
                    det.StockActual = hdfStockActual;//Convert.ToDecimal(hdfStockActual.Value);
                }

                if (hdfIdProducto.Value != string.Empty && Convert.ToInt32(hdfIdProducto.Value) > 0)
                {
                    det.Producto.IdProducto = Convert.ToInt32(hdfIdProducto.Value);
                    det.Producto.Descripcion = hdfProductoDetalle.Value;
                    det.Producto.Venta = true;
                    det.Producto.Familia.Stockeable = hdfStockeable;
                }

                if (this.MiRemito.Factura.IdFactura > 0)
                {
                    Evol.Controls.CurrencyTextBox txtPrecioUnitario = (Evol.Controls.CurrencyTextBox)fila.FindControl("txtPrecioUnitario");
                    if (!det.PrecioUnitario.HasValue || (det.PrecioUnitario.HasValue && det.PrecioUnitario != txtPrecioUnitario.Decimal))
                    {
                        det.PrecioUnitario = txtPrecioUnitario.Decimal;
                    }
                    det.NoIncluidoEnAcopio = hdfNoIncluidoEnAcopio;
                }
                else
                    det.PrecioUnitario = default(decimal?);
                //}

            }
        }
        #endregion

        //protected void popUpMensajes_popUpMensajesPostBackAceptar()
        //{
        //    if (this.RemitosModificarDatosAceptar != null)
        //        this.RemitosModificarDatosAceptar(null, this.MiRemito);
        //}

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiRemito);

            //this.MiRemito.AppPath = this.Request.PhysicalApplicationPath;
            this.MiRemito.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    foreach (VTARemitosDetalles detalle in this.MiRemito.RemitosDetalles)
                    {
                        if (detalle.Producto.IdProducto != 0)
                        {
                            detalle.EstadoColeccion = EstadoColecciones.Agregado;
                            detalle.Estado.IdEstado = (int)Estados.Activo;
                            detalle.IdFacturaDetalle = detalle.IdFacturaDetalle <= 0 ? default(int?) : detalle.IdFacturaDetalle;
                        }
                    }
                    this.MiRemito.NumeroRemitoPrefijo = this.MiRemito.NumeroRemitoPrefijo.PadLeft(4, '0');
                    this.MiRemito.NumeroRemitoSuFijo = this.MiRemito.NumeroRemitoSuFijo.PadLeft(8, '0');
                    guardo = FacturasF.RemitosAgregar(this.MiRemito);
                    //foreach(VTANotasPedidos notasPedidos in)
                    //FacturasF.NotasPedidosModificar()
                    break;
                case Gestion.Modificar:
                    guardo = FacturasF.RemitosModificar(this.MiRemito);
                    break;
                case Gestion.Anular:
                    this.MiRemito.Estado.IdEstado = (int)EstadosRemitos.Baja;
                    foreach (VTARemitosDetalles detalle in this.MiRemito.RemitosDetalles)
                    {
                        detalle.EstadoColeccion = EstadoColecciones.Borrado;
                        detalle.Estado.IdEstado = (int)EstadosRemitos.Baja;
                    }
                    guardo = FacturasF.RemitosAnular(this.MiRemito);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                //this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiRemito.CodigoMensaje));
                this.MostrarMensaje(this.MiRemito.CodigoMensaje, false);
                this.btnAceptar.Visible = false;
                this.btnImprimir.Visible = true;
                this.copyClipboard.Visible = true;
                this.btnFirmaDigital.Visible = true;
            }
            else
            {
                this.MostrarMensaje(this.MiRemito.CodigoMensaje, true, this.MiRemito.CodigoMensajeArgs);
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            VTARemitos remitoPDF = new VTARemitos();
            remitoPDF.IdRemito = this.MiRemito.IdRemito;
            remitoPDF = FacturasF.RemitosObtenerArchivo(remitoPDF);

            //TGEArchivos archivo = new TGEArchivos();
            //archivo.Archivo = remitoPDF.RemitoPDF;
            //archivo.NombreArchivo = string.Concat(this.MiRemito.NumeroRemitoPrefijo, "-", this.MiRemito.NumeroRemitoSuFijo, ".pdf");
            //archivo.TipoArchivo = "application/pdf";
            //this.ctrPopUpComprobantes.CargarArchivo(archivo);
            List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
            TGEArchivos archivo = new TGEArchivos();
            archivo.Archivo = remitoPDF.RemitoPDF;
            if (archivo.Archivo != null)
                listaArchivos.Add(archivo);
            TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();
            string nombreArchivo = string.Concat(empresa.CUIT, "_", MiRemito.TipoFactura.CodigoValor, "_", MiRemito.NumeroRemitoPrefijo, "_", MiRemito.NumeroRemitoSuFijo, ".pdf");
            ExportPDF.ConvertirArchivoEnPdf(this.UpdatePanel3, listaArchivos, nombreArchivo);
        }

        protected void btnFirmaDigital_Click(object sender, EventArgs e)
        {
            TGEParametrosValores paramVal = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.FirmaDigitalHabilitar);
            bool firmaDigital = paramVal.ParametroValor == string.Empty ? false : Convert.ToBoolean(paramVal.ParametroValor);
            if (!firmaDigital)
            {
                this.MostrarMensaje(this.ObtenerMensajeSistema("ValidarFirmaDigitalManuscritaHabilitar"), true);
                return;
            }

            TGEFirmarDocumentos firmarDoc = new TGEFirmarDocumentos();
            firmarDoc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            firmarDoc.IdRefTabla = MiRemito.IdRemito;
            PropertyInfo prop = MiRemito.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() == 1).ToList()[0];
            firmarDoc.Key = prop.Name;
            firmarDoc.CodigoPlantilla = "VTARemitos";
            firmarDoc.Tabla = "VTARemitos";
            firmarDoc.OcultarCaptcha = true;
            firmarDoc.UrlReferrer = true;


            var URL = TGEGeneralesF.FirmarDocumentosArmarLinkFirmarDocumento(firmarDoc).Link;
            string[] sURL = URL.Split('?');
            if (sURL.Length > 1)
            {
                URL = AyudaProgramacion.ObtenerUrlParametros(sURL[0]) + sURL[1];
            }
            this.Response.Redirect(URL, true);
        }

        protected void btnFirmaDigitalBaja_Click(object sender, EventArgs e)
        {
            TGEFirmarDocumentos firmarDoc = new TGEFirmarDocumentos();
            firmarDoc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            firmarDoc.IdRefTabla = this.MiRemito.IdRemito;
            firmarDoc.Tabla = "VTARemitos";
            firmarDoc.IdTipoOperacion = 0;
            firmarDoc.Estado.IdEstado = (int)Estados.Baja;
            bool resultado = TGEGeneralesF.FirmarDocumentosModificar(firmarDoc);
            if (resultado)
            {
                this.btnFirmaDigitalBaja.Visible = false;
                this.btnFirmaDigital.Visible = true;
                this.copyClipboard.Visible = true;
                this.btnWhatsAppFirmarDocumento.Visible = true;
                this.MostrarMensaje(firmarDoc.CodigoMensaje, false);
            }
            else
            {
                this.MostrarMensaje(firmarDoc.CodigoMensaje, true, firmarDoc.CodigoMensajeArgs);
            }
        }

        protected void btnWhatsAppFirmarDocumento_Click(object sender, EventArgs e)
        {
            TGEParametrosValores paramVal = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.FirmaDigitalHabilitar);
            bool firmaDigital = paramVal.ParametroValor == string.Empty ? false : Convert.ToBoolean(paramVal.ParametroValor);
            if (!firmaDigital)
            {
                this.MostrarMensaje(this.ObtenerMensajeSistema("ValidarFirmaDigitalManuscritaHabilitar"), true);
                return;
            }

            TGEFirmarDocumentos firmarDoc = new TGEFirmarDocumentos();
            firmarDoc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            firmarDoc.IdRefTabla = MiRemito.IdRemito;
            PropertyInfo prop = MiRemito.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() == 1).ToList()[0];
            firmarDoc.Key = prop.Name;
            firmarDoc.CodigoPlantilla = "VTARemitos";
            firmarDoc.Tabla = "VTARemitos";
            firmarDoc.OcultarCaptcha = true;
            firmarDoc.UrlReferrer = true;

            string text = string.Concat("Estimado ", MiRemito.Afiliado.RazonSocial, " haga clic en el siguiente link para firmar el documento ", TGEGeneralesF.FirmarDocumentosArmarLinkFirmarDocumento(firmarDoc).Link);
            AfiTelefonos cel = AfiliadosF.AfiliadosObtenerTelefonoCelular(MiRemito.Afiliado);
            string numero = /*cel.Prefijo.ToString() + */cel.Numero.ToString();
            string urlwa = string.Format("https://api.whatsapp.com/send?phone={0}&text={1}", numero, HttpUtility.UrlEncode(text));
            ScriptManager.RegisterStartupScript(this.UpdatePanel3, this.UpdatePanel3.GetType(), "scriptWa", string.Format("EnviarWhatsApp('{0}');", urlwa), true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.RemitosModificarDatosCancelar != null)
                this.RemitosModificarDatosCancelar();
        }
    }
}
