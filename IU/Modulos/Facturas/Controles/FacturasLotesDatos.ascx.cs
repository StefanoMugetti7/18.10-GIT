using Comunes.Entidades;
using Facturas;
using Facturas.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace IU.Modulos.Facturas.Controles
{
    public partial class FacturasLotesDatos : ControlesSeguros
    {
        private VTAFacturasLotesEnviados MiFacturaLoteEnviado
        {
            get { return (VTAFacturasLotesEnviados)Session[this.MiSessionPagina + "FacturasLotesDatosMiFacturaLoteEnviado"]; }
            set { Session[this.MiSessionPagina + "FacturasLotesDatosMiFacturaLoteEnviado"] = value; }
        }
        private string SessionId
        {
            get { return (string)Session["ProcesosDatosModificarSessionId"]; }
            set { Session["ProcesosDatosModificarSessionId"] = value; }
        }
        private DataSet MiFacturasLotesEnviadosFacturas
        {
            get { return (DataSet)Session[this.MiSessionPagina + "FacturasLotesDatosMiFacturasLotesEnviadosFacturas"]; }
            set { Session[this.MiSessionPagina + "FacturasLotesDatosMiFacturasLotesEnviadosFacturas"] = value; }
        }
        private List<VTATiposPuntosVentas> MisTiposPuntosVentas
        {
            get { return (List<VTATiposPuntosVentas>)Session[this.MiSessionPagina + "FacturasLotesDatosMisTiposPuntosVentas"]; }
            set { Session[this.MiSessionPagina + "FacturasLotesDatosMisTiposPuntosVentas"] = value; }
        }
        private List<VTAFilialesPuntosVentas> MisFilialesPuntosVentas
        {
            get { return (List<VTAFilialesPuntosVentas>)Session[this.MiSessionPagina + "FacturasLotesDatosMisFilialesPuntosVentas"]; }
            set { Session[this.MiSessionPagina + "FacturasLotesDatosMisFilialesPuntosVentas"] = value; }
        }
        private List<TGEMonedas> MisMonedas
        {
            get { return (List<TGEMonedas>)Session[this.MiSessionPagina + "FacturasLotesDatosMisMonedas"]; }
            set { Session[this.MiSessionPagina + "FacturasLotesDatosMisMonedas"] = value; }
        }
        private List<TGEListasValoresSistemasDetalles> MisConceptosComprobantes
        {
            get { return (List<TGEListasValoresSistemasDetalles>)Session[this.MiSessionPagina + "FacturasLotesDatosConceptosComprobantes"]; }
            set { Session[this.MiSessionPagina + "FacturasLotesDatosConceptosComprobantes"] = value; }
        }

        //public delegate void FacturasLotesDatosAceptarEventHandler();
        //public event FacturasLotesDatosAceptarEventHandler FacturasLotesDatosAceptar;
        public delegate void FacturasLotesDatosCancelarEventHandler();
        public event FacturasLotesDatosCancelarEventHandler FacturasLotesDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ctrBuscarProductoPopUp.ProductosBuscarSeleccionar += new BuscarProductoPopUp.ProductosBuscarEventHandler(ctrBuscarProductoPopUp_ProductosBuscarSeleccionar);
            if (!this.IsPostBack)
            {
            }
        }
        public void IniciarControl(VTAFacturasLotesEnviados pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiFacturaLoteEnviado = pParametro;
            this.CargarCombos();
            this.hdfIdFilialPredeterminada.Value = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
            this.hdfIdUsuarioEvento.Value = this.UsuarioActivo.IdUsuarioEvento.ToString();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.txtFechaFactura.Text = DateTime.Now.ToShortDateString();
                    this.txtFechaVencimiento.Text = DateTime.Now.AddDays(10).ToShortDateString();
                    this.MiFacturaLoteEnviado.Filial = this.UsuarioActivo.FilialPredeterminada;

                    if (this.MisTiposPuntosVentas.Count == 0)
                    {
                        List<string> lista = new List<string>
                        {
                            this.UsuarioActivo.FilialPredeterminada.Filial,
                            this.UsuarioActivo.Usuario
                        };
                        this.MostrarMensaje("ValidarFilialPuntosVentas", true, lista);
                    }
                    this.btnGenerarLote.Visible = true;
                    this.txtPeriodo.Enabled = true;
                    this.ddlTiposLotes.Enabled = true;
                    this.ddlEmpresas.Enabled = true;
                    this.ddlIvas.Enabled = true;
                    this.ddlProducto.Enabled = true;
                    this.ddlMonedas.Enabled = true;
                    //this.btnBuscarProducto.Visible = true;
                    //this.txtCodigo.Enabled = true;
                    this.MiFacturaLoteEnviado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    this.MiFacturaLoteEnviado.UsuarioAlta.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;

                    this.ddlFilialPuntoVenta_SelectedIndexChanged(null, EventArgs.Empty);
                    this.ctrCamposValores.IniciarControl(pParametro, new Objeto(), GestionControl);
                    break;
                case Gestion.Anular:
                    break;
                case Gestion.Modificar:
                    // ANULAR
                    this.MapearObjetoControles(this.MiFacturaLoteEnviado);
                    this.ddlMonedas_OnSelectedIndexChanged(null, EventArgs.Empty);
                    break;
                case Gestion.Consultar:
                    this.ddlMonedas.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.ddlTiposLotes.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposLotesEnviados);
            this.ddlTiposLotes.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTiposLotes.DataTextField = "Descripcion";
            this.ddlTiposLotes.DataBind();
            if (this.ddlTiposLotes.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposLotes, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposLotes, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEmpresas.DataSource = TGEGeneralesF.GruposEmpresasObtenerPorBaseDatos();
            this.ddlEmpresas.DataValueField = "BaseDatos";
            this.ddlEmpresas.DataTextField = "Empresa";
            this.ddlEmpresas.DataBind();
            if (this.ddlEmpresas.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlEmpresas, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            TGEIVA pParamaetro = new TGEIVA();

            pParamaetro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            this.ddlIvas.DataSource = TGEGeneralesF.TGEIVAAlicuotaObtenerListaActiva(pParamaetro);
            this.ddlIvas.DataValueField = "IdIVA";
            this.ddlIvas.DataTextField = "Descripcion";
            this.ddlIvas.DataBind();
            if (this.ddlIvas.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlIvas, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlIvas, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.MisConceptosComprobantes = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.AFIPConceptosComprobantes);
            this.ddlConceptoComprobante.DataSource = this.MisConceptosComprobantes;
            this.ddlConceptoComprobante.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlConceptoComprobante.DataTextField = "Descripcion";
            this.ddlConceptoComprobante.DataBind();
            if (this.ddlConceptoComprobante.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlConceptoComprobante, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            //this.ddlConceptoComprobante.SelectedValue = "291";
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlConceptoComprobante, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.MisMonedas = TGEGeneralesF.MonedasObtenerListaActiva();
            this.ddlMonedas.DataSource = this.MisMonedas;
            this.ddlMonedas.DataValueField = "IdMoneda";
            this.ddlMonedas.DataTextField = "miMonedaDescripcion";
            this.ddlMonedas.DataBind();
            if (this.ddlMonedas.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlMonedas, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            VTAFilialesPuntosVentas filtro = new VTAFilialesPuntosVentas();
            filtro.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
            this.MisTiposPuntosVentas = FacturasF.VTAFilialesPuntosVentasObtenerPorFilial(filtro);
            this.MisTiposPuntosVentas = this.MisTiposPuntosVentas.Where(x => x.IdTipoPuntoVenta != (int)EnumAFIPTiposPuntosVentas.ComprobanteEnLinea).ToList();
            this.MisTiposPuntosVentas = AyudaProgramacion.AcomodarIndices<VTATiposPuntosVentas>(this.MisTiposPuntosVentas);
            if (this.MisTiposPuntosVentas.Count > 0)
            {
                this.ddlFilialPuntoVenta.DataSource = this.MisTiposPuntosVentas;
                this.ddlFilialPuntoVenta.DataValueField = "IdTipoPuntoVenta";
                this.ddlFilialPuntoVenta.DataTextField = "Descripcion";
                this.ddlFilialPuntoVenta.DataBind();
            }

            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilialPuntoVenta, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            AyudaProgramacion.AgregarItemSeleccione(this.ddlPrefijoNumeroFactura, "0000");

            TGETiposOperaciones operaciones = new TGETiposOperaciones();
            operaciones.TipoFuncionalidad.IdTipoFuncionalidad = this.paginaSegura.paginaActual.IdTipoFuncionalidad;
            operaciones.Estado.IdEstado = (int)Estados.Activo;
            this.ddlTipoOperacionOC.DataSource = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(operaciones);
            this.ddlTipoOperacionOC.DataTextField = "TipoOperacion";
            this.ddlTipoOperacionOC.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacionOC.DataBind();
            if (ddlTipoOperacionOC.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOperacionOC, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void ddlTiposLotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.MiFacturaLoteEnviado.IdFacturaLoteEnviado = 0;
            this.btnAceptar.Visible = false;
            this.dvTiposCargos.Visible = false;
            if (this.gvDatos.Rows.Count > 0)
            {
                //this.btnAceptar.Attributes.Remove("OnClick");
                this.gvDatos.DataSource = null;
                this.gvDatos.DataBind();
                this.btnExportarExcel.Visible = false;
            }
            //this.pnlBuscarProducto.Visible = false;
            //this.rfvCodigo.Enabled = false;

            if (!string.IsNullOrEmpty(this.ddlTiposLotes.SelectedValue))
            {
                this.rfvPeriodo.Enabled = true;
                this.rfvEmpresas.Enabled = true;
                this.lblPeriodo.Visible = true;
                this.txtPeriodo.Visible = true;
                this.lblEmpresa.Visible = true;
                this.ddlEmpresas.Visible = true;
                this.ddlIvas.Enabled = true;
                this.rfvIvas.Enabled = true;
                this.ddlProducto.Enabled = true;
                //this.txtCodigo.Enabled = true;
                //this.btnBuscarProducto.Enabled = true;
                this.rfvProducto.Enabled = true;
                //this.txtProductoDescripcion.Enabled = true;
                this.MiFacturaLoteEnviado.TiposLotesEnviados.IdTipoLoteEnviado = Convert.ToInt32(this.ddlTiposLotes.SelectedValue);
                if (Convert.ToInt32(this.ddlTiposLotes.SelectedValue) == (int)EnumTiposFacturasLotes.DesdeArchivo)
                {
                    this.rfvPeriodo.Enabled = false;
                    this.rfvEmpresas.Enabled = false;
                    this.lblPeriodo.Visible = false;
                    this.txtPeriodo.Visible = false;
                    this.lblEmpresa.Visible = false;
                    this.ddlEmpresas.Visible = false;
                }
                else if (Convert.ToInt32(this.ddlTiposLotes.SelectedValue) == (int)EnumTiposFacturasLotes.Convenios)
                {
                    this.ddlIvas.SelectedValue = string.Empty;
                    this.ddlProducto.SelectedValue = string.Empty;
                    this.ddlProducto.Enabled = false;
                    this.rfvProducto.Enabled = false;
                    //this.txtCodigo.Text = string.Empty;
                    //this.txtProductoDescripcion.Text = string.Empty;
                    this.ddlIvas.Enabled = false;
                    this.rfvIvas.Enabled = false;
                    //this.txtCodigo.Enabled = false;
                    //this.btnBuscarProducto.Enabled = false;
                    //this.rfvCodigo.Enabled = false;
                    //this.txtProductoDescripcion.Enabled = false;                    
                }
                else if (Convert.ToInt32(this.ddlTiposLotes.SelectedValue) == (int)EnumTiposFacturasLotes.Cargos)
                {
                    VTAFacturasLotesEnviados filtro = new VTAFacturasLotesEnviados();
                    filtro.Periodo = this.txtPeriodo.Text == string.Empty ? 0 : Convert.ToInt32(txtPeriodo.Text);
                    this.dvTiposCargos.Visible = true;
                    this.ddlTiposCargos.DataSource = FacturasF.FacturasLotesObtenerCargosPorPeriodo(filtro);
                    this.ddlTiposCargos.DataValueField = "IdListaValorDetalle";
                    this.ddlTiposCargos.DataTextField = "Descripcion";
                    this.ddlTiposCargos.DataBind();
                }
                this.ctrCamposValores.IniciarControl(this.MiFacturaLoteEnviado, this.MiFacturaLoteEnviado.TiposLotesEnviados, GestionControl);
            }
            //this.upBuscarProducto.Update();
        }
        protected void ddlFilialPuntoVenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlFilialPuntoVenta.SelectedValue))
            {
                this.MiFacturaLoteEnviado.FilialPuntoVenta.TipoPuntoVenta = this.MisTiposPuntosVentas[this.ddlFilialPuntoVenta.SelectedIndex];

                this.ddlPrefijoNumeroFactura.Items.Clear();
                this.ddlPrefijoNumeroFactura.SelectedValue = null;
                this.MiFacturaLoteEnviado.FilialPuntoVenta.IdFilial = this.MiFacturaLoteEnviado.Filial.IdFilial;
                this.MiFacturaLoteEnviado.FilialPuntoVenta.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                this.MisFilialesPuntosVentas = FacturasF.VTAFilialesPuntosVentasObtenerListaFiltro(this.MiFacturaLoteEnviado.FilialPuntoVenta);

                if (this.MisFilialesPuntosVentas.Count == 0)
                {
                    this.ddlPrefijoNumeroFactura.Items.Clear();
                    this.ddlPrefijoNumeroFactura.SelectedValue = null;
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlPrefijoNumeroFactura, "0000");
                    List<string> lista = new List<string>
                    {
                        this.UsuarioActivo.FilialPredeterminada.Filial,
                        this.UsuarioActivo.ApellidoNombre
                    };
                    this.MostrarMensaje("ValidarFilialPuntosVentas", true, lista);
                    return;
                }
                List<VTAFilialesPuntosVentas> filialesPuntosVentas = new List<VTAFilialesPuntosVentas>();
                foreach (VTAFilialesPuntosVentas item in this.MisFilialesPuntosVentas)
                {
                    if (!filialesPuntosVentas.Exists(x => x.AfipPuntoVenta == item.AfipPuntoVenta))
                        filialesPuntosVentas.Add(item);
                }

                this.ddlPrefijoNumeroFactura.DataSource = filialesPuntosVentas;
                this.ddlPrefijoNumeroFactura.DataValueField = "AfipPuntoVenta";
                this.ddlPrefijoNumeroFactura.DataTextField = "AfipPuntoVentaNumero";
                this.ddlPrefijoNumeroFactura.DataBind();
            }
            else
            {
                this.MiFacturaLoteEnviado.FilialPuntoVenta = new VTAFilialesPuntosVentas();
                this.MiFacturaLoteEnviado.PrefijoNumeroFactura = string.Empty;
                this.ddlPrefijoNumeroFactura.Items.Clear();
                this.ddlPrefijoNumeroFactura.SelectedValue = null;
                AyudaProgramacion.AgregarItemSeleccione(this.ddlPrefijoNumeroFactura, "0000");
            }
        }
        protected void ddlConceptoComprobante_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.rfvPeriodoFechaDesde.Enabled = false;
            this.rfvPeriodoFechaHasta.Enabled = false;
            this.pnlPeriodoFechas.Visible = false;

            if (!string.IsNullOrEmpty(this.ddlConceptoComprobante.SelectedValue))
            {
                int idConcepto = Convert.ToInt32(this.ddlConceptoComprobante.SelectedValue);
                if (idConcepto == (int)EnumConceptosComprobantes.ProductosYServicios
                    || idConcepto == (int)EnumConceptosComprobantes.Servicios)
                {
                    this.rfvPeriodoFechaDesde.Enabled = true;
                    this.rfvPeriodoFechaHasta.Enabled = true;
                    this.pnlPeriodoFechas.Visible = true;
                }
            }
            this.upConceptoComprobante.Update();
        }
        //protected void txtCodigo_TextChanged(object sender, EventArgs e)
        //{
        //    string contenido = ((TextBox)sender).Text;
        //    CMPListasPreciosDetalles itemFiltro = new CMPListasPreciosDetalles();
        //    itemFiltro.Producto.IdProducto = contenido == string.Empty ? 0 : Convert.ToInt32(contenido);
        //    if (itemFiltro.Producto.IdProducto > 0)
        //    {
        //        itemFiltro.Fecha = DateTime.Now;
        //        itemFiltro.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
        //        itemFiltro = ComprasF.ListasPreciosDetallesObtenerDatosPorProducto(itemFiltro);
        //        if (itemFiltro.IdListaPrecioDetalle > 0) { }
        //            //this.ctrBuscarProductoPopUp_ProductosBuscarSeleccionar(itemFiltro);
        //        else
        //            ((TextBox)sender).Text = string.Empty;
        //    }
        //}

        //protected void btnBuscarProducto_Click(object sender, EventArgs e)
        //{
        //    CMPListasPrecios filtro = new CMPListasPrecios();
        //    this.ctrBuscarProductoPopUp.IniciarControl(filtro);
        //}

        //void ctrBuscarProductoPopUp_ProductosBuscarSeleccionar(CMPListasPreciosDetalles e)
        //{
        //    this.txtCodigo.Text = e.Producto.IdProducto.ToString();
        //    this.txtProductoDescripcion.Text = e.Producto.Descripcion;
        //    this.upBuscarProducto.Update();
        //}
        protected void ddlMonedas_OnSelectedIndexChanged(object sender, EventArgs e)
        {


            if (!string.IsNullOrWhiteSpace(this.ddlMonedas.SelectedValue))
            {
                TGEMonedas moneda = MisMonedas.FirstOrDefault(x => x.IdMoneda == Convert.ToInt32(this.ddlMonedas.SelectedValue));
                SetInitializeCulture(moneda.Moneda);
            }

            upFacturasLotes.Update();
        }
        protected void MapearControlesAObjeto(VTAFacturasLotesEnviados pFactura)
        {
            //pFactura.TipoFactura.IdTipoFactura = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].IdTipoFactura;
            //pFactura.TipoFactura.CodigoValor = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].CodigoValor;
            //pFactura.TipoFactura.Descripcion = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].Descripcion;
            //pFactura.ConceptoComprobante.IdConceptoComprobante = this.MisConceptosComprobantes[this.ddlConceptoComprobante.SelectedIndex].IdListaValorSistemaDetalle;
            //pFactura.ConceptoComprobante.CodigoValor = this.MisConceptosComprobantes[this.ddlConceptoComprobante.SelectedIndex].CodigoValor;
            //pFactura.ConceptoComprobante.Descripcion = this.MisConceptosComprobantes[this.ddlConceptoComprobante.SelectedIndex].Descripcion;

            //pFactura.FechaFactura = Convert.ToDateTime(this.txtFechaFactura.Text);
            //pFactura.FechaVencimiento = this.txtFechaVencimiento.Text == string.Empty ? DateTime.Now.AddDays(10) : Convert.ToDateTime(this.txtFechaVencimiento.Text);
            //pFactura.Moneda = this.MisMonedas[this.ddlMonedas.SelectedIndex];
            //pFactura.Observacion = this.txtObservacion.Text;
            //pFactura.NumeroFactura = this.txtNumeroFactura.Text;
            //pFactura.CantidadCuotas = this.ddlCantidadCuotas.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlCantidadCuotas.SelectedValue);
            //pFactura.Proveedor.IdProveedor = this.ddlProveedores.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlProveedores.SelectedValue);
        }
        private void MapearObjetoControles(VTAFacturasLotesEnviados pFactura)
        {
            //this.MapearObjetoAControlesAfiliado(pFactura.Afiliado);
            //this.ddlFilialPuntoVenta.SelectedValue = pFactura.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta.ToString(); //Convert.ToInt32(pFactura.PrefijoNumeroFactura).ToString();
            //this.txtFechaFactura.Text = pFactura.FechaFactura.ToShortDateString();
            //ListItem item = this.ddlTipoFactura.Items.FindByValue(pFactura.TipoFactura.IdTipoFactura.ToString());
            //if (item == null)
            //    this.ddlTipoFactura.Items.Add(new ListItem(pFactura.TipoFactura.Descripcion, pFactura.TipoFactura.IdTipoFactura.ToString()));
            //this.ddlTipoFactura.SelectedValue = pFactura.TipoFactura.IdTipoFactura.ToString();
            //item = this.ddlPrefijoNumeroFactura.Items.FindByText(pFactura.PrefijoNumeroFactura);
            //if (item == null)
            //    this.ddlPrefijoNumeroFactura.Items.Add(new ListItem(pFactura.PrefijoNumeroFactura, Convert.ToInt32(pFactura.PrefijoNumeroFactura).ToString()));
            //this.ddlPrefijoNumeroFactura.SelectedValue = Convert.ToInt32(pFactura.PrefijoNumeroFactura).ToString();
            //this.txtNumeroFactura.Text = pFactura.NumeroFactura;
            //this.ddlConceptoComprobante.SelectedValue = pFactura.ConceptoComprobante.IdConceptoComprobante.ToString();
            //this.ddlMonedas.SelectedValue = pFactura.Moneda.IdMoneda.ToString();
            //this.txtObservacion.Text = pFactura.Observacion;
            //AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(pFactura.FacturasDetalles, false, this.gvItems, true);
            //if (pFactura.FacturasAsociadas.Count > 0)
            //{
            //    AyudaProgramacion.CargarGrillaListas<VTAFacturas>(pFactura.FacturasAsociadas, false, this.gvDatos, true);
            //    this.pnlComprobantesAsociados.Visible = true;
            //}

            //item = this.ddlCantidadCuotas.Items.FindByValue(pFactura.CantidadCuotas.ToString());
            //if (item == null && pFactura.CantidadCuotas > 1)
            //    this.ddlCantidadCuotas.Items.Add(new ListItem(pFactura.CantidadCuotas.ToString(), pFactura.CantidadCuotas.ToString()));
            //this.ddlCantidadCuotas.SelectedValue = pFactura.CantidadCuotas <= 1 ? (1).ToString() : pFactura.CantidadCuotas.ToString();

            //this.txtTotalConIva.Text = pFactura.ImporteTotal.ToString("C2");
            //this.txtTotalSinIva.Text = pFactura.ImporteSinIVA.ToString("C2");
            //this.txtTotalIva.Text = pFactura.IvaTotal.ToString("C2");

            //if (pFactura.Proveedor.IdProveedor.HasValue)
            //{
            //    this.pnlProveedores.Visible = true;
            //    item = this.ddlProveedores.Items.FindByValue(pFactura.Proveedor.IdProveedor.Value.ToString());
            //    if (item == null)
            //        this.ddlProveedores.Items.Add(new ListItem(pFactura.Proveedor.RazonSocial, pFactura.Proveedor.IdProveedor.Value.ToString()));
            //    this.ddlProveedores.SelectedValue = pFactura.Proveedor.IdProveedor.Value.ToString();
            //}

            //this.ctrAsientoMostrar.IniciarControl(pFactura);

            //if (this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoA
            //        || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoB
            //        || this.MiFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoC)
            //{
            //    this.pnlRemito.Visible = false;
            //}
        }
        protected void btnGenerarLote_Click(object sender, EventArgs e)
        {
            this.MiFacturaLoteEnviado.IdFacturaLoteEnviado = 0;
            this.btnAceptar.Visible = false;
            if (this.gvDatos.Rows.Count > 0)
            {
                //this.btnAceptar.Attributes.Remove("OnClick");
                this.gvDatos.DataSource = null;
                this.gvDatos.DataBind();
                this.btnExportarExcel.Visible = false;
            }

            TGEParametrosValores RutaDelArchivo = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ProcesosDatosDirectorioArchivo);
            string RutaDelArchivoTexto = string.Concat(RutaDelArchivo.ParametroValor.ToString(), this.MiFacturaLoteEnviado.NombreArchivo);
            if (Convert.ToInt32(this.ddlTiposLotes.SelectedValue) == (int)EnumTiposFacturasLotes.DesdeArchivo
                && !File.Exists(RutaDelArchivoTexto))
            {
                this.MostrarMensaje("Debe seleccionar un Archivo para Procesar", true);
                return;
            }

            this.MiFacturaLoteEnviado.Periodo = this.txtPeriodo.Text == string.Empty ? 0 : Convert.ToInt32(this.txtPeriodo.Text);
            this.MiFacturaLoteEnviado.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta = Convert.ToInt32(this.ddlFilialPuntoVenta.SelectedValue);
            this.MiFacturaLoteEnviado.TiposLotesEnviados.IdTipoLoteEnviado = Convert.ToInt32(this.ddlTiposLotes.SelectedValue);
            this.MiFacturaLoteEnviado.BaseDatos = this.ddlEmpresas.SelectedValue;
            this.MiFacturaLoteEnviado.PrefijoNumeroFactura = this.ddlPrefijoNumeroFactura.SelectedValue;
            this.MiFacturaLoteEnviado.FechaFactura = Convert.ToDateTime(this.txtFechaFactura.Text);
            this.MiFacturaLoteEnviado.FechaVencimiento = this.txtFechaVencimiento.Text == string.Empty ? DateTime.Now.AddDays(10) : Convert.ToDateTime(this.txtFechaVencimiento.Text);
            this.MiFacturaLoteEnviado.ConceptoComprobante.IdConceptoComprobante = this.MisConceptosComprobantes[this.ddlConceptoComprobante.SelectedIndex].IdListaValorSistemaDetalle;
            this.MiFacturaLoteEnviado.ConceptoComprobante.CodigoValor = this.MisConceptosComprobantes[this.ddlConceptoComprobante.SelectedIndex].CodigoValor;
            this.MiFacturaLoteEnviado.ConceptoComprobante.Descripcion = this.MisConceptosComprobantes[this.ddlConceptoComprobante.SelectedIndex].Descripcion;
            this.MiFacturaLoteEnviado.Moneda = this.MisMonedas[this.ddlMonedas.SelectedIndex];
            this.MiFacturaLoteEnviado.IVA.IdIVA = this.ddlIvas.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlIvas.SelectedValue);
            this.MiFacturaLoteEnviado.PeriodoFacturadoDesde = this.txtPeriodoFechaDesde.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtPeriodoFechaDesde.Text);
            this.MiFacturaLoteEnviado.PeriodoFacturadoHasta = this.txtPeriodoFechaHasta.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtPeriodoFechaHasta.Text);
            this.MiFacturaLoteEnviado.Observacion = string.Empty;
            this.MiFacturaLoteEnviado.ObservacionComprobante = string.Empty;
            this.MiFacturaLoteEnviado.Producto.IdProducto = this.hdfIdProducto.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdProducto.Value);
            this.MiFacturaLoteEnviado.TipoOperacion.IdTipoOperacion = Convert.ToInt32(this.ddlTipoOperacionOC.SelectedValue);

            if (MiFacturaLoteEnviado.Producto.IdProducto != 0)
            {
                this.MiFacturaLoteEnviado.Producto.Descripcion = hdfProductoDetalle.Value;
                this.ddlProducto.Items.Add(new ListItem(MiFacturaLoteEnviado.Producto.Descripcion.ToString(), MiFacturaLoteEnviado.Producto.IdProducto.ToString()));
                this.ddlProducto.SelectedValue = MiFacturaLoteEnviado.Producto.IdProducto.ToString();
            }

            this.MiFacturaLoteEnviado.LoteCargos = new XmlDocument();
            XmlNode cargoNode = this.MiFacturaLoteEnviado.LoteCargos.CreateElement("FacturasLotesEnviadosCargos");
            this.MiFacturaLoteEnviado.LoteCargos.AppendChild(cargoNode);

            XmlNode itemNode;
            XmlAttribute cargottribute;
            foreach (ListItem chkIncluir in this.ddlTiposCargos.Items)
            {
                if (chkIncluir.Selected)
                {
                    itemNode = this.MiFacturaLoteEnviado.LoteCargos.CreateElement("FacturasLotesEnviadosCargo");
                    cargottribute = this.MiFacturaLoteEnviado.LoteCargos.CreateAttribute("IdTipoCargo");
                    cargottribute.Value = chkIncluir.Value;
                    itemNode.Attributes.Append(cargottribute);
                    cargoNode.AppendChild(itemNode);
                }
            }

            this.MiFacturaLoteEnviado.LoteCamposValores = this.ctrCamposValores.ObtenerListaCamposValores();

            if (!FacturasF.FacturasLotesEnviadosValidaciones(this.MiFacturaLoteEnviado))
            {
                this.MostrarMensaje(this.MiFacturaLoteEnviado.CodigoMensaje, true);
                return;
            }

            this.MiFacturasLotesEnviadosFacturas = FacturasF.FacturasLotesEnviadosAgregarDevolverLote(this.MiFacturaLoteEnviado);

            if (this.MiFacturasLotesEnviadosFacturas.Tables[0].Rows.Count > 0)
            {
                this.MiFacturaLoteEnviado.IdFacturaLoteEnviado = Convert.ToInt32(this.MiFacturasLotesEnviadosFacturas.Tables[0].Rows[0]["IdFacturaLoteEnviado"]);

                this.gvDatos.DataSource = this.MiFacturasLotesEnviadosFacturas.Tables[0];
                this.gvDatos.DataBind();

                if (this.MiFacturasLotesEnviadosFacturas.Tables[0].Rows.Count > 0)
                    this.btnExportarExcel.Visible = true;
                else
                    this.btnExportarExcel.Visible = false;

                //string funcion = string.Format("showConfirm(this,'{0}'); return false;", msg);
                //string funcion = "showConfirmFacturasLotes(this); return false;";
                //this.btnAceptar.Attributes.Add("OnClick", funcion);
                this.btnAceptar.Visible = true;

                if (File.Exists(RutaDelArchivoTexto))
                {
                    File.Copy(RutaDelArchivoTexto, RutaDelArchivo.ParametroValor.ToString() + "VTAFacturasLotesEnviadosArchivosOrigenes_" + this.MiFacturaLoteEnviado.IdFacturaLoteEnviado.ToString());
                    File.Delete(RutaDelArchivoTexto);
                }
            }
            ScriptManager.RegisterStartupScript(this.upFacturasLotes, this.upFacturasLotes.GetType(), "CalcularTotalesScript", "CalcularTotales();", true);
            this.upBotones.Update();
        }
        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MiFacturasLotesEnviadosFacturas;
            this.gvDatos.DataBind();
            //GridViewExportUtil.Export("BancosMovimientos.xls", this.gvDatos);
            ExportData exportData = new ExportData();
            exportData.ExportExcel(this.Page, this.MiFacturasLotesEnviadosFacturas.Tables[0].Copy());
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            //int index = Convert.ToInt32(e.CommandArgument);
            //int IdFacturaLoteEnviadoFactura = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdFacturaLoteEnviadoFactura"].ToString());
            //GridViewRow row = ((GridView)sender).Rows[index];
            //GridView gvDatosDetalles = row.FindControl("gvDatosDetalles") as GridView;
            //UpdatePanel upDatosDetalles = row.FindControl("upDatosDetalles") as UpdatePanel;
            //ImageButton ibtnConsultar = (ImageButton)row.FindControl("btnConsultar");
            //ImageButton ibtnOcultar = (ImageButton)row.FindControl("btnOcultar");
            //Panel pnlDatosDetalles = (Panel)row.FindControl("pnlDatosDetalles");
            //ibtnConsultar.Visible = !ibtnConsultar.Visible;
            //ibtnOcultar.Visible = !ibtnOcultar.Visible;
            //pnlDatosDetalles.Visible = !pnlDatosDetalles.Visible;
            //DataView dv = new DataView(this.MiFacturasLotesEnviadosFacturas.Tables[1]);
            //dv.RowFilter = "IdFacturaLoteEnviadoFactura=" + IdFacturaLoteEnviadoFactura.ToString();
            //gvDatosDetalles.DataSource = dv;
            //gvDatosDetalles.DataBind();
            //upDatosDetalles.Update();
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ////ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                DataRowView dr = (DataRowView)e.Row.DataItem;
                //string IdFacturaLoteEnviadoFactura = gvDatos.DataKeys[e.Row.DataItemIndex].Value.ToString();
                //GridView gvDatosDetalles = e.Row.FindControl("gvDatosDetalles") as GridView;
                //DataView dv = new DataView(this.MiFacturasLotesEnviadosFacturas.Tables[1]);
                //dv.RowFilter = "IdFacturaLoteEnviadoFactura=" + IdFacturaLoteEnviadoFactura.ToString();
                //gvDatosDetalles.ShowHeaderWhenEmpty = true;
                //gvDatosDetalles.DataSource = new List<VTAFacturasDetalles>();
                //gvDatosDetalles.DataBind();

                CheckBox chkIncluir = (CheckBox)e.Row.FindControl("chkIncluir");
                if (Convert.ToInt32(dr["IdAfiliado"]) > 0 && Convert.ToBoolean(dr["Habilitada"]))
                {
                    chkIncluir.Enabled = true;
                    chkIncluir.Checked = true;
                }
                else
                {
                    chkIncluir.Enabled = false;
                    chkIncluir.Checked = false;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //Label lblImporteTotal = (Label)e.Row.FindControl("lblImporteTotal");
                //lblImporteTotal.Text = Convert.ToDecimal(this.MiFacturasLotesEnviadosFacturas.Tables[0].Compute("Sum(ImporteTotal)", string.Empty)).ToString("C2");

                //Label lblIvaTotal = (Label)e.Row.FindControl("lblIvaTotal");
                //lblIvaTotal.Text = Convert.ToDecimal(this.MiFacturasLotesEnviadosFacturas.Tables[0].Compute("Sum(IvaTotal)", string.Empty)).ToString("C2");

                //Label lblImporte = (Label)e.Row.FindControl("lblImporte");
                //lblImporte.Text = Convert.ToDecimal(this.MiFacturasLotesEnviadosFacturas.Tables[0].Compute("Sum(ImporteSinIVA)", string.Empty)).ToString("C2");

                Label lblCantidadRegistros = (Label)e.Row.FindControl("lblCantidadRegistros");
                lblCantidadRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiFacturasLotesEnviadosFacturas.Tables[0].Rows.Count);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            VTAFacturasLotesEnviados parametros = this.BusquedaParametrosObtenerValor<VTAFacturasLotesEnviados>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<VTAFacturasLotesEnviados>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MiFacturasLotesEnviadosFacturas;
            gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //this.MiFacturasLotesEnviadosFacturas = this.OrdenarGrillaDatos(this.MiFacturasLotesEnviadosFacturas, e);
            //this.gvDatos.DataSource = this.MiFacturasLotesEnviadosFacturas;
            //this.gvDatos.DataBind();
        }
        protected void btnProcesar_Click(object sender, EventArgs e)
        {
            //bool guardo = true;
            this.btnAceptar.Visible = false;
            this.Page.Validate("Aceptar");
            if (!this.Page.IsValid)
            {
                this.btnAceptar.Visible = true;
                this.upFacturasLotes.Update();
                return;
            }
            this.MiFacturaLoteEnviado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

            this.MiFacturaLoteEnviado.LoteFacturas = new XmlDocument();
            XmlNode facturasNode = this.MiFacturaLoteEnviado.LoteFacturas.CreateElement("FacturasLotesEnviadosFacturas");
            this.MiFacturaLoteEnviado.LoteFacturas.AppendChild(facturasNode);

            XmlNode factNode;
            XmlAttribute factAttribute;
            int cantidad = 0;
            foreach (GridViewRow pre in this.gvDatos.Rows)
            {
                CheckBox chkIncluir = (CheckBox)pre.FindControl("chkIncluir");
                if (chkIncluir.Checked)
                {
                    cantidad++;
                    factNode = this.MiFacturaLoteEnviado.LoteFacturas.CreateElement("FacturaLoteEnviadoFactura");
                    factAttribute = this.MiFacturaLoteEnviado.LoteFacturas.CreateAttribute("IdFacturaLoteEnviadoFactura");
                    factAttribute.Value = this.gvDatos.DataKeys[pre.DataItemIndex].Value.ToString();
                    factNode.Attributes.Append(factAttribute);
                    facturasNode.AppendChild(factNode);
                }
            }

            if (cantidad == 0)
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje("FacturasLotesValidarItems", true);
                return;
            }

            HttpRuntime.Cache.Insert(Session.SessionID + "CacheProcesoProcesando", "#PROCESANDO", null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            Thread workerThread = new Thread(new ParameterizedThreadStart(EjecutarProceso));
            this.MiFacturaLoteEnviado.Filtro = Session.SessionID;
            workerThread.Start(this.MiFacturaLoteEnviado);
            #region ProcessBak
            //switch (this.GestionControl)
            //{
            //    case Gestion.Agregar:
            //        this.MiFacturaLoteEnviado.LoteFacturas = new XmlDocument();
            //        XmlNode facturasNode = this.MiFacturaLoteEnviado.LoteFacturas.CreateElement("FacturasLotesEnviadosFacturas");
            //        this.MiFacturaLoteEnviado.LoteFacturas.AppendChild(facturasNode);

            //        XmlNode factNode;
            //        XmlAttribute factAttribute;
            //        int cantidad = 0;
            //        foreach (GridViewRow pre in this.gvDatos.Rows)
            //        {
            //            CheckBox chkIncluir = (CheckBox)pre.FindControl("chkIncluir");
            //            if (chkIncluir.Checked)
            //            {
            //                cantidad++;
            //                factNode = this.MiFacturaLoteEnviado.LoteFacturas.CreateElement("FacturaLoteEnviadoFactura");
            //                factAttribute = this.MiFacturaLoteEnviado.LoteFacturas.CreateAttribute("IdFacturaLoteEnviadoFactura");
            //                factAttribute.Value = this.gvDatos.DataKeys[pre.DataItemIndex].Value.ToString();
            //                factNode.Attributes.Append(factAttribute);
            //                facturasNode.AppendChild(factNode);
            //            }
            //        }

            //        if (cantidad == 0)
            //        {
            //            this.btnAceptar.Visible = true;
            //            this.MostrarMensaje("FacturasLotesValidarItems", true);
            //            return;
            //        }

            //        guardo = FacturasF.FacturasLotesEnviadosConfirmarLote(this.MiFacturaLoteEnviado);
            //        if (guardo)
            //        {
            //            this.btnCancelar.Visible = false;
            //            this.btnContinuar.Visible = true;
            //            this.btnGenerarLote.Visible = false;
            //            this.upFacturasLotes.Update();
            //            this.MostrarMensaje(this.MiFacturaLoteEnviado.CodigoMensaje, false, this.MiFacturaLoteEnviado.CodigoMensajeArgs);
            //            //if (this.MiFacturaLoteEnviado.FilialPuntoVenta.IdTipoFactura == (int)EnumAFIPTiposPuntosVentas.WebServiceFacturaElectronica)
            //            //    this.btnValidarFacturasElectronicas.Visible = true;
            //        }
            //        else
            //        {
            //            this.btnAceptar.Visible = true;
            //            this.MostrarMensaje(this.MiFacturaLoteEnviado.CodigoMensaje, true, this.MiFacturaLoteEnviado.CodigoMensajeArgs);
            //            if (this.MiFacturaLoteEnviado.dsResultado != null)
            //            {
            //                this.ctrPopUpGrilla.IniciarControl(this.MiFacturaLoteEnviado);
            //                this.MiFacturaLoteEnviado.dsResultado = null;
            //            }
            //        }
            //        break;
            //    case Gestion.Anular:
            //        //this.MiFacturaLoteEnviado.Estado.IdEstado = (int)EstadosFacturas.Baja;
            //        //this.MiFacturaLoteEnviado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            //        //guardo = FacturasF.FacturasAnular(this.MiFacturaLoteEnviado);
            //        //if (guardo)
            //        //{
            //        //    this.MostrarMensaje(this.MiFacturaLoteEnviado.CodigoMensaje, false);
            //        //}
            //        break;
            //    case Gestion.Modificar:
            //        //this.MapearControlesAObjeto(this.MiFacturaLoteEnviado);
            //        //guardo = FacturasF.FacturasModificar(this.MiFacturaLoteEnviado);
            //        //if (guardo)
            //        //{
            //        //    this.MostrarMensaje(this.MiFacturaLoteEnviado.CodigoMensaje, false);
            //        //}
            //        break;
            //    default:
            //        break;
            //}
            #endregion
        }
        private void EjecutarProceso(object pProcesoProcesamiento)
        {
            bool resultado = false;
            VTAFacturasLotesEnviados objProc = (VTAFacturasLotesEnviados)pProcesoProcesamiento;
            this.SessionId = this.MiFacturaLoteEnviado.Filtro;

            global::Facturas.LogicaNegocio.VTAFacturasLotesEnviadosLN factLotesLN = new global::Facturas.LogicaNegocio.VTAFacturasLotesEnviadosLN();
            factLotesLN.ProcesoDatosEjecutarSPMensajesCallback += new global::Facturas.LogicaNegocio.VTAFacturasLotesEnviadosLN.ProcesosDatosEjecutarSPMensajes(factLotesLN_ProcesoDatosEjecutarSPMensajesCallback);
            resultado = factLotesLN.ConfirmarLote(objProc);
            factLotesLN = null;

            string CacheProcesoProcesando;
            if (resultado)
                CacheProcesoProcesando = "#FINALIZADO";
            else
                CacheProcesoProcesando = "#ERROR";
            HttpRuntime.Cache.Insert(this.SessionId + "CacheProcesoProcesando", CacheProcesoProcesando, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            HttpRuntime.Cache.Insert(this.SessionId + "Resultado", resultado, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            HttpRuntime.Cache.Insert(this.SessionId + "objProcesoProcesamiento", objProc, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
        }
        protected void btnFinalizarProceso_Click(object sender, EventArgs e)
        {
            HttpRuntime.Cache.Remove(Session.SessionID + "CacheMensajes");
            HttpRuntime.Cache.Remove(Session.SessionID + "CacheProcesoProcesando");
            object proc = HttpRuntime.Cache.Get(Session.SessionID + "Resultado");
            bool resultado = false;
            if (proc != null)
                Boolean.TryParse(proc.ToString(), out resultado);
            HttpRuntime.Cache.Remove(Session.SessionID + "Resultado");

            VTAFacturasLotesEnviados factLote = (VTAFacturasLotesEnviados)HttpRuntime.Cache.Get(Session.SessionID + "objProcesoProcesamiento");
            HttpRuntime.Cache.Remove(Session.SessionID + "objProcesoProcesamiento");

            if (resultado)
            {
                //this.ctrMensajesPostBack.MostrarMensaje(this.ObtenerMensajeSistema(this.MiProcesoProcesamiento.CodigoMensaje));
                this.btnAceptar.Visible = false;
                this.btnContinuar.Visible = true;
                this.MostrarMensaje(factLote.CodigoMensaje, false, factLote.CodigoMensajeArgs);
            }
            else
            {
                this.btnAceptar.Visible = true;
                this.btnContinuar.Visible = false;
                if (factLote == null)
                {
                    factLote = new VTAFacturasLotesEnviados();
                    factLote.CodigoMensaje = "El proceso no se pudo ejecutar. Vuelvalo a intentar.";
                }
                this.MostrarMensaje(this.ObtenerMensajeSistema(factLote.CodigoMensaje), true, factLote.CodigoMensajeArgs);
                if (factLote.dsResultado != null)
                    this.ctrPopUpGrilla.IniciarControl(factLote);
            }
        }
        void factLotesLN_ProcesoDatosEjecutarSPMensajesCallback(List<string> e)
        {
            HttpRuntime.Cache.Insert(Session.SessionID + "CacheMensajes", e, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
        }
        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable
            {
                { "IdFacturaLoteEnviado", this.MiFacturaLoteEnviado.IdFacturaLoteEnviado }
            };
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasListar.aspx"), true);
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.FacturasLotesDatosCancelar != null)
                this.FacturasLotesDatosCancelar();
        }
        protected void FileUploadComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            this.MiFacturaLoteEnviado.NombreArchivo = string.Concat(Session.SessionID, "_", AsyncFileUpload1.FileName);

            string HoyTexto = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss");

            TGEParametrosValores RutaDelArchivo = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ProcesosDatosDirectorioArchivo);
            string RutaDelArchivoTexto = string.Concat(RutaDelArchivo.ParametroValor.ToString(), this.MiFacturaLoteEnviado.NombreArchivo);
            if (File.Exists(RutaDelArchivoTexto))// + strFileName))
            {
                File.Copy(RutaDelArchivoTexto, RutaDelArchivoTexto + "-" + HoyTexto);
                File.Delete(RutaDelArchivoTexto);
            }
            //Utilizar el AsyncFileUpload
            this.AsyncFileUpload1.SaveAs(RutaDelArchivoTexto);
            //SistemasF.SistemaActualizar(MiProceso);           
            //}
        }
    }
}