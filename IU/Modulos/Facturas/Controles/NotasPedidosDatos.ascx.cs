using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facturas.Entidades;
using Comunes.Entidades;
using Afiliados.Entidades;
using Afiliados;
using Compras.Entidades;
using Compras;
using Facturas;
using SKP.ASP.Controls;
using Generales.Entidades;
using System.IO;
using Comunes.LogicaNegocio;
using Generales.FachadaNegocio;
using System.Globalization;
using Reportes.FachadaNegocio;
using System.Web.UI.HtmlControls;
using System.Collections;

namespace IU.Modulos.Facturas.Controles
{
    public partial class NotasPedidosDatos : ControlesSeguros
    {
        private VTANotasPedidos MiNotaPedido
        {
            get { return (VTANotasPedidos)Session[this.MiSessionPagina + "VTANotasPedidosMiPedido"]; }
            set { Session[this.MiSessionPagina + "VTANotasPedidosMiPedido"] = value; }
        }
        public delegate void NotasPedidosDatosEventHandler();
        public event NotasPedidosDatosEventHandler NotasPedidosModificarDatosCancelar;

        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "NotasPedidosModificarDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "NotasPedidosModificarDatosMiIndiceDetalleModificar"] = value; }
        }

        private List<TGEIVA> MisIvas
        {
            get { return (List<TGEIVA>)Session[this.MiSessionPagina + "FacturasDatosMisIvas"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMisIvas"] = value; }
        }

        private List<TGEMonedas> MisMonedas
        {
            get { return (List<TGEMonedas>)Session[this.MiSessionPagina + "FacturasDatosMisMonedas"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMisMonedas"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrDomicilios.AfiliadosModificarDatosAceptar += CtrDomicilios_AfiliadosModificarDatosAceptar;
            ctrBuscarCliente.BuscarCliente += new Afiliados.Controles.ClientesDatosCabeceraAjax.ClientesDatosCabeceraAjaxEventHandler(CtrBuscarCliente_BuscarCliente);
            ctrBuscarFacturasPopUp.FacturasBuscarSeleccionar += CtrBuscarFacturasPopUp_FacturasBuscarSeleccionar;
            ctrBuscarPresupuestoPopUp.PresupuestosBuscarSeleccionar += CtrBuscarPresupuestoPopUp_PresupuestosBuscarSeleccionar;
            if (!this.IsPostBack)
            {
                if (this.MiNotaPedido == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
                
            }
            else
            {
                this.PersistirDatosGrilla();
            }

        }

        private void CtrBuscarPresupuestoPopUp_PresupuestosBuscarSeleccionar(VTAPresupuestos e)
        {
            this.MapearPresupuestoANotaPedido(e);
            AyudaProgramacion.CargarGrillaListas<VTANotasPedidosDetalles>(this.MiNotaPedido.NotasPedidosDetalles, false, gvItems, true);
            this.items.Update();

            ScriptManager.RegisterStartupScript(this.items, this.items.GetType(), "CalcularItem", "CalcularItem();", true);

        }

        private void CtrBuscarFacturasPopUp_FacturasBuscarSeleccionar(List<VTAFacturasDetalles> e)
        {
            if (ctrBuscarCliente.MiAfiliado.IdAfiliado < 0)
            {
                this.MiNotaPedido.CodigoMensaje = "RemitoSeleccioneCliente";
                this.MostrarMensaje(this.MiNotaPedido.CodigoMensaje, true);
            }
            else
            {
                //this.MiRemito.RemitosDetalles[this.MiIndiceDetalleModificar].Producto = e.FacturasDetalles[this.MiIndiceDetalleModificar].Producto;
                if (e.Count() > 0)
                {
                    MapearFacturaANotaDePedido(e);
                    AyudaProgramacion.CargarGrillaListas<VTANotasPedidosDetalles>(this.MiNotaPedido.NotasPedidosDetalles, false, this.gvItems, true);
                    this.items.Update();

                    ScriptManager.RegisterStartupScript(this.items, this.items.GetType(), "CalcularItem", "CalcularItem();", true);
                }
                else
                {
                    this.MiNotaPedido.CodigoMensaje = "RemitoFacturasCantidadItems";

                }
            }
        }

        public void IniciarControl(VTANotasPedidos pNotasPedidos, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiNotaPedido = pNotasPedidos;
            this.CargarCombos();
            List<TGEEstados> estados;
            hdfIdUsuarioEvento.Value = this.UsuarioActivo.IdUsuarioEvento.ToString();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.RemitoVentaAutomatico);

                    bool bvalor = valor.ParametroValor.Trim() == string.Empty ? false : Convert.ToBoolean(valor.ParametroValor);

                    valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.RemitoVentaConfirmacionPopUp);
                    bvalor = valor.ParametroValor.Trim() == string.Empty ? false : Convert.ToBoolean(valor.ParametroValor);
                    if (bvalor)
                    {
                        string funcion = string.Format("ValidarShowConfirm(this,'{0}');", this.ObtenerMensajeSistema("RemitoVentaConfirmacionPopUp"));
                        this.btnAceptar.Attributes.Add("OnClick", funcion);
                    }

                    valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.FacturasPorcentajeDescuentoSocios);
                   
                    this.IniciarGrilla();
                    btnCopiar.Visible = false;

                    this.txtCantidadAgregar.Enabled = true;

                    if (this.MisParametrosUrl.Contains("UrlReferrer"))
                        this.paginaSegura.viewStatePaginaSegura = this.MisParametrosUrl["UrlReferrer"].ToString();


                    estados = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosNotasPedidos));
                    estados = estados.Where(x => x.IdEstado != (int)EstadosNotasPedidos.Baja).ToList();

                    this.ctrComentarios.IniciarControl(this.MiNotaPedido, this.GestionControl);
                    this.ctrArchivos.IniciarControl(this.MiNotaPedido, this.GestionControl);
                    this.MiNotaPedido.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    this.ddlDomicilio.Enabled = true;
                    btnAgregarDomicilio.Visible = true;
                    txtDescripcion.Enabled = true;

                    txtCantidadAgregar.Enabled = true;
                    gvItems.Enabled = true;

                    if (this.MisParametrosUrl.Contains("IdAfiliado"))
                    {
                        MiNotaPedido.Afiliado.IdAfiliado = Convert.ToInt32(this.MisParametrosUrl["IdAfiliado"].ToString());
                        ctrBuscarCliente.BuscarCliente += new Afiliados.Controles.ClientesDatosCabeceraAjax.ClientesDatosCabeceraAjaxEventHandler(CtrBuscarCliente_BuscarCliente);
                        ctrBuscarCliente.IniciarControl(MiNotaPedido.Afiliado, this.GestionControl);
                        //this.txtNumeroSocio.Text = this.MisParametrosUrl["IdAfiliado"].ToString();
                        //this.txtNumeroSocio_TextChanged(this.txtNumeroSocio, EventArgs.Empty);
                    }
                    //this.ctrBuscarCliente.IniciarControlAfiliados(this.MiNotaPedido.Factura, this.GestionControl);
                    break;
                case Gestion.Modificar:
                    MiNotaPedido = FacturasF.NotasPedidosObtenerDatosCompletos(MiNotaPedido);
                    MapearObjetoAControles(this.MiNotaPedido);
                    this.btnCopiar.Visible = true;
                    this.ddlDomicilio.Enabled = true;
                    btnAgregarDomicilio.Visible = true;
                    ddlFilialEntrega.Enabled = false;
                    txtDescripcion.Enabled = false;
                    ddlDomicilio.Enabled = false;
                    ddlMonedas.Enabled = false;
                    phAgregarItem.Visible = true;
                    ddlEstado.Enabled = true;
                    btnImprimir.Visible = true;
                    txtFecha.Enabled = false;
                    phAgregarItem.Visible = false;
                    break;
                case Gestion.Anular:
                    MiNotaPedido = FacturasF.NotasPedidosObtenerDatosCompletos(MiNotaPedido);
                    MapearObjetoAControles(this.MiNotaPedido);
                    btnCopiar.Visible = false;
                    ddlFilialEntrega.Enabled = false;
                    ddlMonedas.Enabled = false;
                    phAgregarItem.Visible = false;
                    ddlEstado.Enabled = false;
                    btnImprimir.Visible = true;
                    txtFecha.Enabled = false;
                    phAgregarItem.Visible = false;
                    txtDescripcion.Enabled = false;
                    txtFecha.Enabled = false;
                    ddlEstado.Enabled = false;
                    ddlFilialEntrega.Enabled = false;
                    ddlMonedas.Enabled = false;
                    break;
                case Gestion.Copiar:
                    this.MiNotaPedido = FacturasF.NotasPedidosObtenerDatosCompletos(pNotasPedidos);
                    foreach (VTANotasPedidosDetalles det in MiNotaPedido.NotasPedidosDetalles)
                    {
                        det.EstadoColeccion = EstadoColecciones.Agregado;
                        det.IdNotaPedido = 0;
                    }
                    this.btnCopiar.Visible = true;
                    this.MiNotaPedido.FechaAlta = DateTime.Now;
                    this.GestionControl = Gestion.Agregar;
                    this.MiNotaPedido.IdNotaPedido = 0;
                    this.MapearObjetoAControles(this.MiNotaPedido);
                    if (this.MisParametrosUrl.Contains("UrlReferrer"))
                        this.paginaSegura.viewStatePaginaSegura = this.MisParametrosUrl["UrlReferrer"].ToString();
                    btnCopiar.Visible = false;
                    phAgregarItem.Visible = true;
                        PersistirDatosGrilla();
                    MisParametrosUrl = new Hashtable();
                    break;
                case Gestion.Consultar:
                    MiNotaPedido = FacturasF.NotasPedidosObtenerDatosCompletos(MiNotaPedido);
                    MapearObjetoAControles(this.MiNotaPedido);
                    btnCopiar.Visible = true;
                    btnAceptar.Visible = false;
                    ddlFilialEntrega.Enabled = false;
                    ddlMonedas.Enabled = false;
                    phAgregarItem.Visible = false;
                    ddlEstado.Enabled = false;
                    btnImprimir.Visible = true;
                    txtFecha.Enabled = false;
                    phAgregarItem.Visible = false;
                    txtDescripcion.Enabled = false;
                    txtFecha.Enabled = false;
                    ddlEstado.Enabled = false;
                    ddlFilialEntrega.Enabled = false;
                    ddlMonedas.Enabled = false;

                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            TGEIVA pParamaetro = new TGEIVA();

            pParamaetro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            this.MisIvas = TGEGeneralesF.TGEIVAAlicuotaObtenerListaActiva(pParamaetro);

            TGETiposOperaciones operaciones = new TGETiposOperaciones();
            operaciones.TipoFuncionalidad.IdTipoFuncionalidad = this.paginaSegura.paginaActual.IdTipoFuncionalidad;
            operaciones.Estado.IdEstado = (int)Estados.Activo;

            AyudaProgramacion.AgregarItemSeleccione(this.ddlDomicilio, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlFilialEntrega.Items.Clear();
            this.ddlFilialEntrega.SelectedValue = null;
            this.ddlFilialEntrega.DataSource = TGEGeneralesF.FilialesEntregaObtenerListaActiva();
            this.ddlFilialEntrega.DataValueField = "IdFilialEntrega";
            this.ddlFilialEntrega.DataTextField = "Filial";
            this.ddlFilialEntrega.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilialEntrega, this.ObtenerMensajeSistema("SeleccioneOpcion"));

    
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosNotasPedidos));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstado, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            MisMonedas = TGEGeneralesF.MonedasCotizacionesObtenerCombo();
            this.ddlMonedas.DataSource = MisMonedas;
            this.ddlMonedas.DataValueField = "IdMoneda";
            this.ddlMonedas.DataTextField = "MonedaDescripcionCotizacion";
            this.ddlMonedas.DataBind();
        }

        private void CargarComboDomicilio()
        {
            this.ddlDomicilio.Items.Clear();
            this.ddlDomicilio.SelectedIndex = -1;
            this.ddlDomicilio.SelectedValue = null;
            this.ddlDomicilio.ClearSelection();
            this.ddlDomicilio.DataSource = this.MiNotaPedido.Afiliado.Domicilios;
            this.ddlDomicilio.DataValueField = "IdDomicilio";
            this.ddlDomicilio.DataTextField = "DomicilioCompleto";
            this.ddlDomicilio.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlDomicilio, this.ObtenerMensajeSistema("SeleccioneOpcion"));

        }

        protected void btnAgregarDomicilio_Click(object sender, EventArgs e)
        {
            this.ctrDomicilios.IniciarControl(new AfiDomicilios(), Gestion.Agregar);
        }

        private void CtrDomicilios_AfiliadosModificarDatosAceptar(object sender, AfiDomicilios e, Gestion pGestion)
        {
            e.IdAfiliado = this.MiNotaPedido.Afiliado.IdAfiliado;
            e.EstadoColeccion = EstadoColecciones.Agregado;
            if (AfiliadosF.AfiliadosAgregarDomicilio(e))
            {
                this.MiNotaPedido.Afiliado.Domicilios.Add(e);
                CargarComboDomicilio();
                ddlDomicilio.SelectedValue = e.IdDomicilio.ToString();
            }
            else
            {
                MostrarMensaje(e.CodigoMensaje, true, e.CodigoMensajeArgs);
            }
            UpdatePanel1.Update();
        }

        #region Grilla Comprobantes
        private void IniciarGrilla()
        {
            VTANotasPedidosDetalles item;
            for (int i = 0; i < 2; i++)
            {
                item = new VTANotasPedidosDetalles();
                item.EstadoColeccion = EstadoColecciones.AgregadoPrevio;
                this.MiNotaPedido.NotasPedidosDetalles.Add(item);
                item.IndiceColeccion = this.MiNotaPedido.NotasPedidosDetalles.IndexOf(item);
            }
            AyudaProgramacion.CargarGrillaListas<VTANotasPedidosDetalles>(this.MiNotaPedido.NotasPedidosDetalles, false, this.gvItems, true);
        }

        private void PersistirDatosGrilla()
        {
            if (this.MiNotaPedido.NotasPedidosDetalles.Count == 0)
                return;

            foreach (GridViewRow fila in this.gvItems.Rows)
            {
                DropDownList ddlProducto = ((DropDownList)fila.FindControl("ddlProducto"));
                HiddenField hdfIdProducto = (HiddenField)fila.FindControl("hdfIdProducto");
                HiddenField hdfProductoDetalle = (HiddenField)fila.FindControl("hdfProductoDetalle");
                Label lblProductoDescripcion = (Label)fila.FindControl("lblProductoDescripcion");
                string descripcion = ((TextBox)fila.FindControl("txtDescripcion")).Text;
                decimal cantidad = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtCantidad")).Decimal;
                decimal importe = ((HiddenField)fila.FindControl("hdfPreUnitario")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfPreUnitario")).Value.ToString().Replace('.', ','));               
                decimal precioUnitario = ((HiddenField)fila.FindControl("hdfPreUnitario")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfPreUnitario")).Value.ToString().Replace('.', ','));
                decimal subTotal = ((HiddenField)fila.FindControl("hdfSubtotal")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfSubtotal")).Value.ToString().Replace('.', ','));
                DropDownList ddlAlicuotaIVA = ((DropDownList)fila.FindControl("ddlAlicuotaIVA"));
                decimal importeIva = ((HiddenField)fila.FindControl("hdfImporteIva")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfImporteIva")).Value.ToString().Replace('.', ','));
                decimal subTotalConIva = ((HiddenField)fila.FindControl("hdfSubtotalConIva")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfSubtotalConIva")).Value.ToString().Replace('.', ','));
                bool modificaPrecio = ((HiddenField)fila.FindControl("hdfModificaPrecio")).Value == string.Empty ? false : Convert.ToBoolean(((HiddenField)fila.FindControl("hdfModificaPrecio")).Value);
                decimal importeDesc = ((HiddenField)fila.FindControl("hdfDescuentoImporte")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfDescuentoImporte")).Value.ToString().Replace('.', ','));
                bool hdfStockeable = ((HiddenField)fila.FindControl("hdfStockeable")).Value == string.Empty ? false : Convert.ToBoolean(((HiddenField)fila.FindControl("hdfStockeable")).Value);

                if (hdfIdProducto.Value != string.Empty && Convert.ToInt32(hdfIdProducto.Value) > 0)
                {
                    this.MiNotaPedido.NotasPedidosDetalles[fila.RowIndex].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiNotaPedido.NotasPedidosDetalles[fila.RowIndex], GestionControl);
                    this.MiNotaPedido.NotasPedidosDetalles[fila.RowIndex].Estado.IdEstado = (int)Estados.Activo;
                    this.MiNotaPedido.NotasPedidosDetalles[fila.RowIndex].ListaPrecioDetalle.Producto.IdProducto = Convert.ToInt32(hdfIdProducto.Value);
                    this.MiNotaPedido.NotasPedidosDetalles[fila.RowIndex].ListaPrecioDetalle.Producto.Descripcion = hdfProductoDetalle.Value;
                    this.MiNotaPedido.NotasPedidosDetalles[fila.RowIndex].ListaPrecioDetalle.Producto.Familia.Stockeable = hdfStockeable;
            
                this.MiNotaPedido.NotasPedidosDetalles[fila.RowIndex].Descripcion = descripcion;
                this.MiNotaPedido.NotasPedidosDetalles[fila.RowIndex].DescripcionProducto = hdfProductoDetalle.Value;
                if (cantidad > 0)
                {
                    this.MiNotaPedido.NotasPedidosDetalles[fila.RowIndex].Cantidad = cantidad;
                }
                this.MiNotaPedido.NotasPedidosDetalles[fila.RowIndex].PrecioUnitarioSinIva = Convert.ToDecimal(importe);

                if (cantidad > 0)
                {

                    this.MiNotaPedido.NotasPedidosDetalles[fila.RowIndex].DescuentoImporte = importeDesc;
                    this.MiNotaPedido.NotasPedidosDetalles[fila.RowIndex].SubTotal = subTotal;

                    this.MiNotaPedido.NotasPedidosDetalles[fila.RowIndex].ImporteIVA = importeIva;

                    this.MiNotaPedido.NotasPedidosDetalles[fila.RowIndex].SubTotalConIva = subTotalConIva;
                    if (ddlAlicuotaIVA.SelectedValue != string.Empty)
                    {
                        this.MiNotaPedido.NotasPedidosDetalles[fila.RowIndex].IVA = this.MisIvas[ddlAlicuotaIVA.SelectedIndex];
                    }
                }
                }
                //if (modificaPrecio)
                //{
                //    MiNotaPedido.NotasPedidosDetalles[fila.RowIndex].ListaPrecioDetalle.PrecioEditable = true;
                //}

            }
            this.CalcularTotal();
        }

        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            this.MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            if (e.CommandName == "Borrar")
            {
                this.MiNotaPedido.NotasPedidosDetalles.RemoveAt(this.MiIndiceDetalleModificar);
                this.MiNotaPedido.NotasPedidosDetalles = AyudaProgramacion.AcomodarIndices<VTANotasPedidosDetalles>(this.MiNotaPedido.NotasPedidosDetalles);
                AyudaProgramacion.CargarGrillaListas<VTANotasPedidosDetalles>(this.MiNotaPedido.NotasPedidosDetalles, false, this.gvItems, true);
                this.CalcularTotal();
            }
        }


        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                VTANotasPedidosDetalles item = (VTANotasPedidosDetalles)e.Row.DataItem;
                TextBox descripcion = (TextBox)e.Row.FindControl("txtDescripcion");
                DropDownList ddlProducto = ((DropDownList)e.Row.FindControl("ddlProducto"));
                HiddenField hdfIdProducto = ((HiddenField)e.Row.FindControl("hdfIdProducto"));
                HiddenField hdfProductoDetalle = ((HiddenField)e.Row.FindControl("hdfProductoDetalle"));
                HiddenField hdfPrecio = (HiddenField)e.Row.FindControl("hdfPrecio");
                HiddenField hdfMargenImporte = (HiddenField)e.Row.FindControl("hdfMargenImporte");
                HiddenField hdfMargenPorcentaje = (HiddenField)e.Row.FindControl("hdfMargenPorcentaje");
                if (item.Producto.IdProducto > 0)
                {
                    
                    ddlProducto.Items.Add(new ListItem(item.Producto.Descripcion, item.Producto.IdProducto.ToString()));
       
                    hdfIdProducto.Value = item.Producto.IdProducto.ToString();
                    hdfProductoDetalle.Value = item.Producto.Descripcion.ToString();
                    hdfPrecio.Value = item.ListaPrecioDetalle.Precio.ToString() == string.Empty ? "0.00" : item.ListaPrecioDetalle.Precio.ToString();
                    hdfMargenImporte.Value = item.ListaPrecioDetalle.MargenImporte.ToString();
                    hdfMargenPorcentaje.Value = item.ListaPrecioDetalle.MargenPorcentaje.ToString();
                }
                DropDownList ddlAlicuotaIVA = ((DropDownList)e.Row.FindControl("ddlAlicuotaIVA"));
                ddlAlicuotaIVA.DataSource = this.MisIvas;
                ddlAlicuotaIVA.DataValueField = "IdIVAAlicuota";
                ddlAlicuotaIVA.DataTextField = "Descripcion";
                ddlAlicuotaIVA.DataBind();
               
                if(item.IVA.Alicuota > 0)
                    ddlAlicuotaIVA.SelectedValue = item.IVA.Alicuota.ToString();
    

                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                Image imgNoIncluidoEnAcopio = (Image)e.Row.FindControl("imgNoIncluidoEnAcopio");
                Evol.Controls.CurrencyTextBox cantidad = (Evol.Controls.CurrencyTextBox)e.Row.FindControl("txtCantidad");
                cantidad.Attributes.Add("onchange", "CalcularItem();");
                Label lblProductoDescripcion = (Label)e.Row.FindControl("lblProductoDescripcion");
                Evol.Controls.CurrencyTextBox txtPrecioUnitario = (Evol.Controls.CurrencyTextBox)e.Row.FindControl("txtPrecioUnitario");
                txtPrecioUnitario.Attributes.Add("onchange", "CalcularItem();");
                Evol.Controls.CurrencyTextBox txtDescuentoPorcentual = (Evol.Controls.CurrencyTextBox)e.Row.FindControl("txtDescuentoPorcentual");
                txtDescuentoPorcentual.Attributes.Add("onchange", "CalcularItem();");

                if (ddlAlicuotaIVA.Items.Count == 0)
                    AyudaProgramacion.AgregarItemSeleccione(ddlAlicuotaIVA, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                else
                {
                    ddlAlicuotaIVA.SelectedValue = item.IVA.IdIVA.ToString();
                    ddlAlicuotaIVA.Attributes.Add("onchange", "CalcularItem();");
                    ddlAlicuotaIVA.Enabled = true;
                }
                switch (GestionControl)
                {
                    case Gestion.Agregar:
                        ddlProducto.Enabled = true;
                        descripcion.Enabled = true;
                        descripcion.Visible = true;
                        cantidad.Enabled = true;

                        btnEliminar.Visible = true;
                        txtPrecioUnitario.Enabled = true;
                        txtDescuentoPorcentual.Enabled = true;
                        if (item.IdNotaPedidoDetalle > 0)
                        {
                            lblProductoDescripcion.Visible = true;

                            cantidad.Attributes.Add("onchange", "CalcularItem();");
                            if(item.ListaPrecioDetalle.PrecioEditable)
                            {
                                txtPrecioUnitario.Enabled = true;
                            }
                        }
                        break;
                    case Gestion.Modificar:
                        lblProductoDescripcion = (Label)e.Row.FindControl("lblProductoDescripcion");
                        lblProductoDescripcion.Visible = true;
                        TextBox txtDescripcion = (TextBox)e.Row.FindControl("txtDescripcion");
                        txtDescripcion.Visible = false;
                        Label lblProducto = (Label)e.Row.FindControl("lblProducto");
                        lblProducto.Visible = true;
                        lblProducto.Enabled = true;
                        ddlProducto.Visible = false;
                        ddlAlicuotaIVA.SelectedValue = item.IVA.IdIVAAlicuota;
                        ddlAlicuotaIVA.Enabled = false;
                        break;
                    case Gestion.Consultar:
                        btnAceptar.Visible = false;
                        btnImprimir.Visible = true;
                        lblProductoDescripcion.Visible = true;
                        txtDescripcion = (TextBox)e.Row.FindControl("txtDescripcion");
                        txtDescripcion.Visible = false;
                        lblProducto = (Label)e.Row.FindControl("lblProducto");
                        lblProducto.Visible = true;
                        lblProducto.Enabled = true;
                        ddlProducto.Visible = false;
                        btnEliminar.Visible = false;
                        cantidad.Enabled = false;
                        ddlProducto.Enabled = false;
                        break;
                    case Gestion.Anular:
                        lblProductoDescripcion.Visible = true;
                        txtDescripcion = (TextBox)e.Row.FindControl("txtDescripcion");
                        txtDescripcion.Visible = false;
                        lblProducto = (Label)e.Row.FindControl("lblProducto");
                        lblProducto.Visible = true;
                        lblProducto.Enabled = true;
                        ddlProducto.Visible = false;
                        btnEliminar.Visible = false;
                        cantidad.Enabled = false;
                        ddlProducto.Enabled = false;
                        break;
                    default:
                        break;
                }
            }
        }

        protected void btnAgregarItem_Click(object sender, EventArgs e)
        {
            this.PersistirDatosGrilla();
            this.AgregarItem();
            txtCantidadAgregar.Text = string.Empty;
        }

        private void AgregarItem()
        {
            VTANotasPedidosDetalles item;
            if (this.txtCantidadAgregar.Text == string.Empty || txtCantidadAgregar.Text == "0")
            {
                txtCantidadAgregar.Text = "1";
            }
            int cantidad = Convert.ToInt32(this.txtCantidadAgregar.Text);
            for (int i = 0; i < cantidad; i++)
            {
                item = new VTANotasPedidosDetalles();
                this.MiNotaPedido.NotasPedidosDetalles.Add(item);
                item.IndiceColeccion = this.MiNotaPedido.NotasPedidosDetalles.IndexOf(item);
                item.IdNotaPedidoDetalle = item.IndiceColeccion * -1;

            }
            AyudaProgramacion.CargarGrillaListas<VTANotasPedidosDetalles>(this.MiNotaPedido.NotasPedidosDetalles, false, this.gvItems, true);
        }
        #endregion

        #region "Mapeo de Datos"
        protected void MapearControlesAObjeto(VTANotasPedidos pNotaPedido)
        {
            MiNotaPedido.FechaEntrega = txtFecha.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(txtFecha.Text);
            MiNotaPedido.DomicilioEntrega = ddlDomicilio.SelectedValue == string.Empty ? string.Empty : ddlDomicilio.SelectedItem.Text;
            MiNotaPedido.IdDomicilio = ddlDomicilio.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(ddlDomicilio.SelectedValue);
            MiNotaPedido.Descripcion = txtDescripcion.Text;
            MiNotaPedido.Afiliado.IdAfiliado = MiNotaPedido.Afiliado.IdAfiliado;
            MiNotaPedido.Estado.IdEstado = Convert.ToInt32(ddlEstado.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(ddlEstado.SelectedValue));
            MiNotaPedido.Filial.IdFilial = Convert.ToInt32(ddlFilialEntrega.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(ddlFilialEntrega.SelectedValue));

            TGEMonedas mon = MisMonedas.Find(x => x.IdMoneda == Convert.ToInt32(ddlMonedas.SelectedValue));
            MiNotaPedido.MonedaCotizacion = mon.MonedeaCotizacion.MonedaCotizacion;
            MiNotaPedido.Moneda.IdMoneda = mon.IdMoneda;
            
            MiNotaPedido.Campos = ctrCamposValores.ObtenerLista();
            MiNotaPedido.Comentarios = ctrComentarios.ObtenerLista();
        }

        protected void MapearObjetoAControles(VTANotasPedidos pNotaPedido)
        {
            txtDescripcion.Text = pNotaPedido.Descripcion;

            txtFecha.Text = pNotaPedido.FechaAlta.ToShortDateString();

            var item = ddlFilialEntrega.Items.FindByValue(pNotaPedido.Filial.IdFilial.ToString());
            if (item == null)
                ddlFilialEntrega.Items.Add(new ListItem(pNotaPedido.Filial.Descripcion, pNotaPedido.Filial.IdFilial.ToString()));
            ddlFilialEntrega.SelectedValue = pNotaPedido.Filial.IdFilial.ToString();


            ddlEstado.SelectedValue = pNotaPedido.Estado.IdEstado.ToString();

            item = this.ddlMonedas.Items.FindByValue(pNotaPedido.Moneda.IdMoneda.ToString());
            if (item == null)
                this.ddlMonedas.Items.Add(new ListItem(pNotaPedido.Moneda.miMonedaDescripcion, pNotaPedido.Moneda.IdMoneda.ToString()));
            this.ddlMonedas.SelectedValue = pNotaPedido.Moneda.IdMoneda.ToString();
            if (pNotaPedido.Moneda.IdMoneda != (int)EnumTGEMonedas.PesosArgentinos)
                this.ddlMonedas.SelectedItem.Text = String.Concat(this.ddlMonedas.SelectedItem.Text, " (", pNotaPedido.MonedaCotizacion.ToString("C2"), " )");
            TGEMonedas moneda = MisMonedas.FirstOrDefault(x => x.IdMoneda == Convert.ToInt32(this.ddlMonedas.SelectedValue));
            SetInitializeCulture(moneda.Moneda);

            AyudaProgramacion.CargarGrillaListas<VTANotasPedidosDetalles>(pNotaPedido.NotasPedidosDetalles, true, gvItems, true);

            txtTotalSinIva.Text = this.MiNotaPedido.ImporteSinIVA.ToString();
            txtTotalIva.Text = this.MiNotaPedido.IvaTotal.ToString();
            txtTotalConIva.Text = this.MiNotaPedido.ImporteTotal.ToString();
            ctrBuscarCliente.IniciarControl(pNotaPedido.Afiliado, GestionControl);

            if (pNotaPedido.IdDomicilio.HasValue)
            {
                item = this.ddlDomicilio.Items.FindByValue(pNotaPedido.IdDomicilio.Value.ToString());
                if (item == null)
                {
                    this.ddlDomicilio.Items.Add(new ListItem(pNotaPedido.DomicilioEntrega, pNotaPedido.IdDomicilio.ToString()));
                }
                this.ddlDomicilio.SelectedValue = pNotaPedido.IdDomicilio.Value.ToString();
                this.ddlDomicilio.SelectedItem.Text = pNotaPedido.DomicilioEntrega;
            }
        }
        #endregion

        protected void ddlMonedas_OnSelectedIndexChanged(object sender, EventArgs e)
        {

            MiNotaPedido.NotasPedidosDetalles = new List<VTANotasPedidosDetalles>();
            IniciarGrilla();


            if (!string.IsNullOrWhiteSpace(this.ddlMonedas.SelectedValue))
            {
                TGEMonedas moneda = MisMonedas.FirstOrDefault(x => x.IdMoneda == Convert.ToInt32(this.ddlMonedas.SelectedValue));
                SetInitializeCulture(moneda.Moneda);
            }

            items.Update();

        }

        #region "Control Afiliado"
        void CtrBuscarCliente_BuscarCliente(AfiAfiliados e)
        {
            PersistirDatosGrilla();
            if (e.IdAfiliado > 0)
            {
                MiNotaPedido.Afiliado = ctrBuscarCliente.MiAfiliado;
                MiNotaPedido.Afiliado.Domicilios = AfiliadosF.AfiliadosObtenerDomicilios(MiNotaPedido.Afiliado);
                CargarComboDomicilio();
                if (this.GestionControl == Gestion.Agregar)
                {
                    if (MiNotaPedido.Afiliado.Domicilios.Exists(x => x.Predeterminado))
                    {
                        this.ddlDomicilio.SelectedValue = MiNotaPedido.Afiliado.Domicilios.First(x => x.Predeterminado).IdDomicilio.ToString();
                    }
                }
                UpdatePanel1.Update();
            }
        }
        #endregion

        #region Grilla

        private void VaciarGrilla()
        {
            this.MiNotaPedido.NotasPedidosDetalles.Clear();
            this.IniciarGrilla();
            this.items.Update();
        }

        protected void CalcularTotal()
        {
            decimal? totalSinIva = 0;
            decimal? totalIva = 0;
            decimal? totalConIva = 0;
            decimal totalPercepciones = 0;

            totalSinIva = this.MiNotaPedido.NotasPedidosDetalles.Sum(x => x.SubTotal == null ? 0 : x.SubTotal);
            totalIva = this.MiNotaPedido.NotasPedidosDetalles.Sum(x => x.ImporteIVA == null ? 0 : x.ImporteIVA);
            totalConIva = totalSinIva + totalIva + totalPercepciones;

            this.MiNotaPedido.ImporteSinIVA = totalSinIva.Value;
            this.MiNotaPedido.IvaTotal = totalIva.Value;
            this.MiNotaPedido.ImporteTotal = totalConIva.Value;

            this.txtTotalConIva.Text = totalConIva.Value.ToString("C2");
            this.txtTotalSinIva.Text = totalSinIva.Value.ToString("C2");
            this.txtTotalIva.Text = totalIva.Value.ToString("C2");
        }
        #endregion

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiNotaPedido);

            //this.MiNotaPedido.AppPath = this.Request.PhysicalApplicationPath;
            this.MiNotaPedido.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    foreach (VTANotasPedidosDetalles detalle in this.MiNotaPedido.NotasPedidosDetalles)
                    {
                        if (detalle.Producto.IdProducto != 0)
                        {
                            detalle.EstadoColeccion = EstadoColecciones.Agregado;
                            detalle.Estado.IdEstado = (int)Estados.Activo;
                        }
                    }
                    this.MiNotaPedido.NotasPedidosDetalles = this.MiNotaPedido.NotasPedidosDetalles.Where(x => x.EstadoColeccion == EstadoColecciones.Agregado).ToList();
                    guardo = FacturasF.NotasPedidosAgregar(this.MiNotaPedido);
                    break;
                case Gestion.Modificar:
                    guardo = FacturasF.NotasPedidosModificar(this.MiNotaPedido);
                    break;
                case Gestion.Anular:
                    this.MiNotaPedido.Estado.IdEstado = (int)EstadosNotasPedidos.Baja;
                    foreach (VTANotasPedidosDetalles detalle in this.MiNotaPedido.NotasPedidosDetalles)
                    {
                        detalle.EstadoColeccion = EstadoColecciones.Borrado;
                        detalle.Estado.IdEstado = (int)EstadosNotasPedidos.Baja;
                    }
                    guardo = FacturasF.NotasPedidosAnular(this.MiNotaPedido);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                //this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiNotaPedido.CodigoMensaje));
                this.MostrarMensaje(this.MiNotaPedido.CodigoMensaje, false);
                this.btnAceptar.Visible = false;
                this.btnImprimir.Visible = true;
                //this.btnFirmaDigital.Visible = true;
            }
            else
            {
                this.MostrarMensaje(this.MiNotaPedido.CodigoMensaje, true, this.MiNotaPedido.CodigoMensajeArgs);
            }
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            VTANotasPedidos NotaPedidoPDF = new VTANotasPedidos();
            NotaPedidoPDF.IdNotaPedido = this.MiNotaPedido.IdNotaPedido;

            UsuarioLogueado usulog = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF( EnumTGEComprobantes.SinComprobante , "VTANotasPedidos", NotaPedidoPDF , usulog);
            ExportPDF.ExportarPDF(pdf, this.UpdatePanel3, usulog);
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.NotasPedidosModificarDatosCancelar != null)
                this.NotasPedidosModificarDatosCancelar();
        }
        protected void btnCopiar_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", Gestion.Copiar);
            this.MisParametrosUrl.Add("IdNotaPedido", this.MiNotaPedido.IdNotaPedido);
            this.MisParametrosUrl.Add("IdAfiliado", this.MiNotaPedido.Afiliado.IdAfiliado);
            if (!string.IsNullOrEmpty(this.paginaSegura.viewStatePaginaSegura))
                this.MisParametrosUrl.Add("UrlReferrer", this.paginaSegura.viewStatePaginaSegura.ToString());
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/NotasPedidosAgregar.aspx"), true);
        }

        protected void btnImportarPresupuesto_Click(object sender, EventArgs e)
        {
            if (ctrBuscarCliente.MiAfiliado.IdAfiliado < 0)
            {
                this.MiNotaPedido.CodigoMensaje = "RemitoSeleccioneSocio";
                this.MostrarMensaje(this.MiNotaPedido.CodigoMensaje, true);
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

        protected void btnImportarFactura_Click(object sender, EventArgs e)
        {
            if (ctrBuscarCliente.MiAfiliado.IdAfiliado < 0)
            {
                this.MiNotaPedido.CodigoMensaje = "RemitoSeleccioneSocio";
                this.MostrarMensaje(this.MiNotaPedido.CodigoMensaje, true);
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

        protected void MapearFacturaANotaDePedido(List<VTAFacturasDetalles> pFacturasDetalles)
        {
            List<VTANotasPedidosDetalles> lista = new List<VTANotasPedidosDetalles>();
            TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ValidarStockNegativo);
            bool validarStock = paramValor.ParametroValor == string.Empty ? false : Convert.ToBoolean(paramValor.ParametroValor);
            VTANotasPedidosDetalles detalle;
            foreach (VTAFacturasDetalles det in pFacturasDetalles)
            {
                detalle = new VTANotasPedidosDetalles();
                detalle.EstadoColeccion = EstadoColecciones.Agregado;
                detalle.PrecioUnitarioSinIva = det.PrecioUnitarioSinIva;
                detalle.DescuentoPorcentual = det.DescuentoPorcentual;
                detalle.Producto = det.ListaPrecioDetalle.Producto;
                detalle.Producto.StockActual = det.ListaPrecioDetalle.StockActual;
                detalle.IVA.IdIVA = det.IVA.IdIVA;
                detalle.Cantidad = det.CantidadEntregada.HasValue ? det.CantidadEntregada.Value : 0;
                detalle.Descripcion = det.Descripcion;
                if (validarStock)
                {
                    if (det.CantidadRestante <= det.ListaPrecioDetalle.StockActual)
                    {
                        detalle.Cantidad = det.CantidadRestante.Value;
                    }
                    else
                    {
                        detalle.Cantidad = det.ListaPrecioDetalle.StockActual;
                    }
                }
                else
                {
                    detalle.Cantidad = det.CantidadRestante.Value;
                }
                detalle.IdFacturaDetalle = det.IdFacturaDetalle;
                lista.Add(detalle);
                detalle.IndiceColeccion = lista.IndexOf(detalle);
            }
            this.MiNotaPedido.NotasPedidosDetalles = lista;
        }

        private void MapearPresupuestoANotaPedido(VTAPresupuestos e)
        {
            List<VTANotasPedidosDetalles> lista = new List<VTANotasPedidosDetalles>();
            VTANotasPedidosDetalles fDetalle;
            this.MiNotaPedido.NotasPedidosDetalles = new List<VTANotasPedidosDetalles>();

            foreach (VTAPresupuestosDetalles detPresu in e.PresupuestosDetalles)
            {
                fDetalle = new VTANotasPedidosDetalles();
                fDetalle.Producto.IdProducto = detPresu.ListaPrecioDetalle.Producto.IdProducto;//ver como se carga la lista de precio
                fDetalle.IdListaPrecioDetalle = detPresu.ListaPrecioDetalle.IdListaPrecioDetalle;
                fDetalle.Producto.Descripcion = detPresu.DescripcionProducto;
                fDetalle.Descripcion = detPresu.Descripcion;
                fDetalle.PrecioUnitarioSinIva = detPresu.PrecioUnitarioSinIva;
                fDetalle.DescuentoPorcentual = detPresu.DescuentoPorcentual;
                fDetalle.Cantidad = detPresu.Cantidad.HasValue ? detPresu.Cantidad.Value : 0;
                CMPStock stock = new CMPStock();
                stock.IdFilial = this.MiNotaPedido.Filial.IdFilial;
                stock.Producto.IdProducto = fDetalle.Producto.IdProducto;
                stock = ComprasF.StockObtenerPorProductoFilial(stock);
                fDetalle.Producto.StockActual = stock.StockActual;
                lista.Add(fDetalle);
                fDetalle.IndiceColeccion = lista.IndexOf(fDetalle);
            }

            this.MiNotaPedido.NotasPedidosDetalles = lista;
        }
    }
}