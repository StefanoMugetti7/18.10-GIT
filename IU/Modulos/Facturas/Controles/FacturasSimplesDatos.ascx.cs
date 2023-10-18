using Afiliados;
using Afiliados.Entidades;
using Bancos;
using Bancos.Entidades;
using Compras;
using Compras.Entidades;
using Comunes.Entidades;
using Evol.Controls;
using Facturas;
using Facturas.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Reportes.Entidades;
using Reportes.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace IU.Modulos.Facturas.Controles
{
    public partial class FacturasSimplesDatos : ControlesSeguros
    {
        const int MiCantidadDecimales = 2;
        private VTAFacturas MiFactura
        {
            get { return (VTAFacturas)Session[this.MiSessionPagina + "FacturasDatosMiFactura"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMiFactura"] = value; }
        }
        private List<TESBancosCuentas> MisBancosCuentas
        {
            get { return (List<TESBancosCuentas>)Session[this.MiSessionPagina + "FacturasDatosMisBancosCuentas"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMisBancosCuentas"] = value; }
        }
        private List<VTATiposPuntosVentas> MisTiposPuntosVentas
        {
            get { return (List<VTATiposPuntosVentas>)Session[this.MiSessionPagina + "FacturasDatosMisTiposPuntosVentas"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMisTiposPuntosVentas"] = value; }
        }
        private List<VTAFilialesPuntosVentas> MisFilialesPuntosVentas
        {
            get { return (List<VTAFilialesPuntosVentas>)Session[this.MiSessionPagina + "FacturasDatosMisFilialesPuntosVentas"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMisFilialesPuntosVentas"] = value; }
        }
        private TGEIVA MiIva
        {
            get { return (TGEIVA)Session[this.MiSessionPagina + "FacturasDatosSimplesMisIvas"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosSimplesMisIvas"] = value; }
        }
        private List<TGETiposFacturas> MisTiposFacturas
        {
            get { return (List<TGETiposFacturas>)Session[this.MiSessionPagina + "FacturasDatosMisTiposFacturas"]; }
            set { Session[this.MiSessionPagina + "FacturasDatosMisTiposFacturas"] = value; }
        }
        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "FacturaModificarDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "FacturaModificarDatosMiIndiceDetalleModificar"] = value; }
        }
        public delegate void FacturasDatosCancelarEventHandler();
        public event FacturasDatosCancelarEventHandler FacturaModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                if (this.MiFactura == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(this.hdfIdAfiliado.Value) && Convert.ToInt32(this.hdfIdAfiliado.Value) > 0)
                {
                    this.ddlNumeroSocio.Items.Add(new ListItem(this.hdfRazonSocial.Value, this.hdfIdAfiliado.Value));
                    this.ddlNumeroSocio.SelectedValue = this.hdfIdAfiliado.Value;
                }
                else
                {
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlNumeroSocio, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                }
                this.PersistirDatosGrilla();
            }
        }
        public void IniciarControl(VTAFacturas pFactura, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiFactura = pFactura;
            this.CargarCombos();
            this.hdfIdFilialPredeterminada.Value = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
            this.hdfIdUsuarioEvento.Value = this.UsuarioActivo.IdUsuarioEvento.ToString();
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
                    List<TGEIVA> ivas = TGEGeneralesF.TGEIVAAlicuotaObtenerListaActiva(new TGEIVA());
                    this.MiIva = ivas.Find(x => x.IdIVA == 1); // ---> ESTO ES 21
                    this.txtFechaFactura.Text = DateTime.Now.ToShortDateString();
                    this.MiFactura.Filial = this.UsuarioActivo.FilialPredeterminada;
                    this.txtFechaFactura.Enabled = true;
                    if (this.MisTiposPuntosVentas.Count == 0)
                    {
                        List<string> lista = new List<string>
                        {
                            this.UsuarioActivo.FilialPredeterminada.Filial,
                            this.UsuarioActivo.Usuario
                        };
                        this.MostrarMensaje("ValidarFilialPuntosVentas", true, lista);
                        this.btnAceptar.Visible = false;
                        return;
                    }
                    this.IniciarGrilla();
                    if (this.MisParametrosUrl.Contains("UrlReferrer"))
                        this.paginaSegura.viewStatePaginaSegura = this.MisParametrosUrl["UrlReferrer"].ToString();

                    VTAFacturas factura = new VTAFacturas
                    {
                        UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo)
                    };
                    factura.Afiliado.IdAfiliado = this.MiFactura.Afiliado.IdAfiliado;
                    this.MiFactura = FacturasF.FacturasSimpleObtenerDatosPreCargados(factura);
                    if (this.MiFactura.Filtro == "Precargados")
                    {
                        this.MapearObjetoControlesPrecarga(this.MiFactura);
                    }
                    this.txtTotalConIva.Text = "$0,00";
                    this.ddlBancoCuentaAgrupado_SelectedIndexChanged(null, new EventArgs());
                    break;
                default:
                    break;
            }
            this.MiFactura.DirPath = this.Request.PhysicalApplicationPath;
            this.MiFactura.AppPath = this.ObtenerAppPath();
        }
        private void CargarCombos()
        {
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            VTAFilialesPuntosVentas filtro = new VTAFilialesPuntosVentas
            {
                IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial
            };
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

            TESBancosCuentas filtroCtas = new TESBancosCuentas
            {
                UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo)
            };
            this.MisBancosCuentas = BancosF.BancosCuentasObtenerListaGrupo(filtroCtas);
            this.ddlBancoCuentaAgrupado.DataSource = this.MisBancosCuentas;
            this.ddlBancoCuentaAgrupado.DataValueField = "Filtro";
            this.ddlBancoCuentaAgrupado.DataTextField = "Denominacion";
            this.ddlBancoCuentaAgrupado.DataGroupField = "Grupo";
            this.ddlBancoCuentaAgrupado.DataBind();

            CMPListasPrecios lista = new CMPListasPrecios
            {
                UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo),
                IdFilialEvento = string.IsNullOrEmpty(this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString()) ? 0 : this.UsuarioActivo.FilialPredeterminada.IdFilial
            };
            this.ddlListaPrecio.DataSource = ComprasF.ObtenerListasPrecios(lista);
            this.ddlListaPrecio.DataValueField = "IdListaPrecio";
            this.ddlListaPrecio.DataTextField = "Descripcion";
            this.ddlListaPrecio.DataBind();

            AyudaProgramacion.AgregarItemSeleccione(this.ddlListaPrecio, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void ddlFilialPuntoVenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlFilialPuntoVenta.SelectedValue))
            {
                this.MiFactura.FilialPuntoVenta.TipoPuntoVenta = this.MisTiposPuntosVentas[this.ddlFilialPuntoVenta.SelectedIndex];
                //Cargo los comprobantes habilitados para el Cliente
                AfiAfiliados afi = new AfiAfiliados
                {
                    IdAfiliado = hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(hdfIdAfiliado.Value)
                };
                afi = AfiliadosF.AfiliadosObtenerDatos(afi);
                this.MiFactura.Afiliado = afi;
                this.MisTiposFacturas = FacturasF.TiposFacturasSeleccionarPorCondicionFiscal(this.MiFactura);
                this.ddlTipoFactura.Items.Clear();
                this.ddlTipoFactura.SelectedValue = null;
                this.ddlTipoFactura.DataSource = this.MisTiposFacturas;
                this.ddlTipoFactura.DataValueField = "IdTipoFactura";
                this.ddlTipoFactura.DataTextField = "Descripcion";
                this.ddlTipoFactura.DataBind();
                if (this.ddlTipoFactura.Items.Count != 1)
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                this.ddlTipoFactura_SelectedIndexChanged(null, EventArgs.Empty);
            }
            else
            {
                this.ddlTipoFactura.Items.Clear();
                this.ddlTipoFactura.SelectedValue = null;
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                this.MiFactura.FilialPuntoVenta = new VTAFilialesPuntosVentas();
                this.MiFactura.PrefijoNumeroFactura = string.Empty;
            }
            this.UpdatePanel1.Update();
        }
        protected void ddlTipoFactura_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlTipoFactura.SelectedValue))
            {
                this.MiFactura.TipoFactura.IdTipoFactura = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].IdTipoFactura;
                this.MiFactura.TipoFactura.CodigoValor = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].CodigoValor;
                this.MiFactura.TipoFactura.Descripcion = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].Descripcion;
                this.MiFactura.TipoFactura.Signo = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].Signo;

                this.MiFactura.FilialPuntoVenta.IdFilial = this.MiFactura.Filial.IdFilial;
                this.MiFactura.FilialPuntoVenta.IdTipoFactura = this.MiFactura.TipoFactura.IdTipoFactura;
                this.MiFactura.FilialPuntoVenta.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

                this.MisFilialesPuntosVentas = FacturasF.VTAFilialesPuntosVentasObtenerListaFiltro(this.MiFactura.FilialPuntoVenta);

                if (this.MisFilialesPuntosVentas.Count == 0)
                {
                    this.UpdatePanel1.Update();
                    this.upItems.Update();
                    List<string> lista = new List<string>
                    {
                        this.UsuarioActivo.FilialPredeterminada.Filial,
                        this.UsuarioActivo.ApellidoNombre
                    };
                    this.MostrarMensaje("ValidarFilialPuntosVentas", true, lista);
                    return;
                }

                this.MiFactura.TipoFactura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                if (this.MiFactura.FacturasDetalles.Count > 0)
                    AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(this.MiFactura.FacturasDetalles, false, this.gvItems, true);
                else
                    this.IniciarGrilla();

                ScriptManager.RegisterStartupScript(this.UpdatePanel1, this.UpdatePanel1.GetType(), "CalcularItemScript", "CalcularItem();", true);
            }
            else
            {
                this.MiFactura.TipoFactura = new TGETiposFacturas();
            }
            this.UpdatePanel1.Update();
            this.upItems.Update();
        }
        #region Grilla Comprobantes
        private void IniciarGrilla()
        {
            VTAFacturasDetalles item;
            for (int i = 0; i < 1; i++)
            {
                item = new VTAFacturasDetalles
                {
                    EstadoColeccion = EstadoColecciones.AgregadoPrevio
                };
                this.MiFactura.FacturasDetalles.Add(item);
                item.IndiceColeccion = this.MiFactura.FacturasDetalles.IndexOf(item);
            }
            AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(this.MiFactura.FacturasDetalles, false, this.gvItems, true);
        }
        private void PersistirDatosGrilla()

        {
            if (this.MiFactura.FacturasDetalles.Count == 0)
                return;

            foreach (GridViewRow fila in this.gvItems.Rows)
            {
                //DropDownList ddlProducto = ((DropDownList)fila.FindControl("ddlProducto"));
                HiddenField hdfIdProducto = (HiddenField)fila.FindControl("hdfIdProducto");
                HiddenField hdfProductoDetalle = (HiddenField)fila.FindControl("hdfProductoDetalle");
                decimal cantidad = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtCantidad")).Decimal;
                //decimal precioUnitario = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtPrecioUnitario")).Decimal;
                decimal importe = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtPrecioUnitario")).Decimal;                //CurrencyTextBox importe = (CurrencyTextBox)fila.FindControl("txtPrecioUnitario");
                //decimal precioUnitario = ((HiddenField)fila.FindControl("hdfPreUnitario")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfPreUnitario")).Value.ToString().Replace('.', ','));
                decimal subTotal = ((HiddenField)fila.FindControl("hdfSubtotal")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfSubtotal")).Value.ToString().Replace('.', ','));
                decimal subTotalConIva = ((HiddenField)fila.FindControl("hdfSubtotalConIva")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfSubtotalConIva")).Value.ToString().Replace('.', ','));
                bool modificaPrecio = ((HiddenField)fila.FindControl("hdfModificaPrecio")).Value == string.Empty ? false : Convert.ToBoolean(((HiddenField)fila.FindControl("hdfModificaPrecio")).Value);
                this.MiFactura.FacturasDetalles[fila.RowIndex].ModificaPrecio = modificaPrecio;
                bool hdfStockeable = ((HiddenField)fila.FindControl("hdfStockeable")).Value == string.Empty ? false : Convert.ToBoolean(((HiddenField)fila.FindControl("hdfStockeable")).Value);
                decimal importeIva = ((HiddenField)fila.FindControl("hdfImporteIva")).Value == string.Empty ? 0 : Convert.ToDecimal(((HiddenField)fila.FindControl("hdfImporteIva")).Value.ToString().Replace('.', ','));
                if (hdfIdProducto.Value != string.Empty && Convert.ToInt32(hdfIdProducto.Value) > 0)
                {
                    this.MiFactura.FacturasDetalles[fila.RowIndex].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiFactura.FacturasDetalles[fila.RowIndex], GestionControl);
                    this.MiFactura.FacturasDetalles[fila.RowIndex].Estado.IdEstado = (int)EstadosFacturasDetalles.Activo;
                    this.MiFactura.FacturasDetalles[fila.RowIndex].ListaPrecioDetalle.Producto.IdProducto = Convert.ToInt32(hdfIdProducto.Value);
                    this.MiFactura.FacturasDetalles[fila.RowIndex].ListaPrecioDetalle.Producto.Descripcion = hdfProductoDetalle.Value;
                    this.MiFactura.FacturasDetalles[fila.RowIndex].ListaPrecioDetalle.Producto.Familia.Stockeable = hdfStockeable;
                }
                this.MiFactura.FacturasDetalles[fila.RowIndex].DescripcionProducto = hdfProductoDetalle.Value;
                if (cantidad > 0)
                {
                    this.MiFactura.FacturasDetalles[fila.RowIndex].Cantidad = cantidad;
                }
                this.MiFactura.FacturasDetalles[fila.RowIndex].PrecioUnitarioSinIva = Convert.ToDecimal(importe);

                if (cantidad > 0) // && precioUnitario != string.Empty)
                {
                    this.MiFactura.FacturasDetalles[fila.RowIndex].SubTotal = subTotal;
                    this.MiFactura.FacturasDetalles[fila.RowIndex].SubTotalConIva = subTotalConIva;
                    this.MiFactura.FacturasDetalles[fila.RowIndex].ImporteIVA = importeIva;
                    this.MiFactura.FacturasDetalles[fila.RowIndex].IVA = this.MiIva;
                }
                if (modificaPrecio)
                {
                    this.MiFactura.FacturasDetalles[fila.RowIndex].ListaPrecioDetalle.PrecioEditable = true;
                }
            }
            this.CalcularTotal();
        }
        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            this.MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            if (e.CommandName == "Borrar")
            {
                this.MiFactura.FacturasDetalles.RemoveAt(this.MiIndiceDetalleModificar);
                this.MiFactura.FacturasDetalles = AyudaProgramacion.AcomodarIndices<VTAFacturasDetalles>(this.MiFactura.FacturasDetalles);
                AyudaProgramacion.CargarGrillaListas<VTAFacturasDetalles>(this.MiFactura.FacturasDetalles, false, this.gvItems, true);
                this.CalcularTotal();
            }
        }
        protected void gvItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                VTAFacturasDetalles item = (VTAFacturasDetalles)e.Row.DataItem;

                DropDownList ddlProducto = ((DropDownList)e.Row.FindControl("ddlProducto"));
                CurrencyTextBox txtCantidad = ((CurrencyTextBox)e.Row.FindControl("txtCantidad"));

                if (item.ListaPrecioDetalle.Producto.IdProducto > 0)
                    ddlProducto.Items.Add(new ListItem(item.ListaPrecioDetalle.Producto.Descripcion == string.Empty ? item.DescripcionProducto : item.ListaPrecioDetalle.Producto.Descripcion, item.ListaPrecioDetalle.Producto.IdProducto.ToString()));

                if (this.GestionControl == Gestion.Agregar)
                {
                    bool permitenegativo = false;
                    if (this.ddlTipoFactura.SelectedValue != string.Empty)
                    {
                        permitenegativo = this.MisTiposFacturas.FirstOrDefault(x => x.IdTipoFactura.ToString() == this.ddlTipoFactura.SelectedValue).Signo > 0 ? true : false;
                    }

                    if (item.DetalleImportado == true)
                    {
                        ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                        btnEliminar.Visible = true;
                        ddlProducto.Enabled = false;
                        CurrencyTextBox cantidad = (CurrencyTextBox)e.Row.FindControl("txtCantidad");
                        cantidad.Attributes.Add("onchange", "CalcularItem();");
                        cantidad.Enabled = true;

                        Evol.Controls.CurrencyTextBox importe = (Evol.Controls.CurrencyTextBox)e.Row.FindControl("txtPrecioUnitario");
                        importe.AllowNegative = permitenegativo;
                        HiddenField hdfModifPrecio = (HiddenField)e.Row.FindControl("hdfModificaPrecio");
                        importe.Attributes.Add("onchange", "ModificaPrecio('" + hdfModifPrecio.ClientID + "'); CalcularItem();");
                        importe.NumberOfDecimals = MiCantidadDecimales;
                        string numberSymbol = importe.Prefix == string.Empty ? "N" : "C";
                        decimal precioUni = item.PrecioUnitarioSinIva.HasValue ? item.PrecioUnitarioSinIva.Value : 0;
                        importe.Text = precioUni.ToString(string.Concat(numberSymbol, MiCantidadDecimales.ToString()));
                        if (item.ListaPrecioDetalle.PrecioEditable)
                        {
                            importe.Enabled = true;
                        }
                        else
                        {
                            importe.Enabled = false;
                        }
                    }
                    else
                    {
                        ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                        btnEliminar.Visible = true;
                        ddlProducto.Enabled = false;
                        CurrencyTextBox cantidad = (CurrencyTextBox)e.Row.FindControl("txtCantidad");
                        cantidad.Attributes.Add("onchange", "CalcularItem();");
                        cantidad.Enabled = true;

                        CurrencyTextBox importe = (CurrencyTextBox)e.Row.FindControl("txtPrecioUnitario");
                        importe.AllowNegative = permitenegativo;
                        HiddenField hdfModifPrecio = (HiddenField)e.Row.FindControl("hdfModificaPrecio");
                        importe.Attributes.Add("onchange", "ModificaPrecio('" + hdfModifPrecio.ClientID + "'); CalcularItem();");
                        importe.NumberOfDecimals = MiCantidadDecimales;

                        string numberSymbol = importe.Prefix == string.Empty ? "N" : "C";
                        decimal precioUni = item.PrecioUnitarioSinIva.HasValue ? item.PrecioUnitarioSinIva.Value : 0;
                        importe.Text = precioUni.ToString(string.Concat(numberSymbol, MiCantidadDecimales.ToString()));
                        if (item.ListaPrecioDetalle.PrecioEditable)
                            importe.Enabled = true;
                        else
                            importe.Enabled = false;

                        if (item.EsFacturaCargos)
                        {
                            importe.Attributes.Add("onchange", "ModificaPrecio('" + hdfModifPrecio.ClientID + "'); CalcularItem(); ValidarFacturaCargo(); ");
                            importe.Enabled = true;
                        }
                    }
                }
            }
        }
        protected void buttonAfi_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.hdfIdAfiliado.Value) && Convert.ToInt32(this.hdfIdAfiliado.Value) > 0)
            {
                this.ddlNumeroSocio.Items.Add(new ListItem(this.hdfRazonSocial.Value, this.hdfIdAfiliado.Value));
                this.ddlNumeroSocio.SelectedValue = this.hdfIdAfiliado.Value;
                this.ddlFilialPuntoVenta_SelectedIndexChanged(null, new EventArgs());
            }
            else
                AyudaProgramacion.AgregarItemSeleccione(this.ddlNumeroSocio, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void button_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.hdfIdProductoCodigo.ToString()))
            {
                List<VTAFacturasDetalles> list = this.MiFactura.FacturasDetalles.Where(x => x.ListaPrecioDetalle.Producto.IdProducto > 0).ToList();

                this.MiFactura.FacturasDetalles = list;

                VTAFacturasDetalles producto = new VTAFacturasDetalles();
                producto.Cantidad = this.txtCantidadCodigo.Decimal;
                producto.ListaPrecioDetalle.Producto.IdProducto = Convert.ToInt32(this.hdfIdProductoCodigo.Value);
                producto.DescripcionProducto = this.hdfProductoDetalleCodigo.Value;
                producto.ListaPrecioDetalle.Precio = this.hdfPrecioCodigo.Value == string.Empty ? 0 : Convert.ToDecimal(this.hdfPrecioCodigo.Value);
                producto.ListaPrecioDetalle.PrecioEditable = this.hdfModificaPrecioCodigo.Value == string.Empty ? true : Convert.ToBoolean(this.hdfModificaPrecioCodigo.Value);
                producto.PrecioUnitarioSinIva = this.hdfPreUnitarioCodigo.Value == string.Empty ? 0 : Convert.ToDecimal(this.hdfPreUnitarioCodigo.Value);
                /*SI VIENEN NULOS POR DEFECTO 21*/
                producto.IVA.IdIVA = this.MiIva.IdIVA;
                producto.IVA.Alicuota = this.MiIva.Alicuota;
                if (this.MiFactura.FacturasDetalles.Exists(x => x.ListaPrecioDetalle.Producto.IdProducto == producto.ListaPrecioDetalle.Producto.IdProducto))
                {
                    this.MiFactura.FacturasDetalles.FirstOrDefault(x => x.ListaPrecioDetalle.Producto.IdProducto == producto.ListaPrecioDetalle.Producto.IdProducto).Cantidad += this.txtCantidadCodigo.Decimal;
                }
                else
                    this.MiFactura.FacturasDetalles.Add(producto);

                this.lblDescripcionProductoCodigo.Text = "Ultimo Producto Cargado: " + producto.DescripcionProducto + " - Cantidad: " + producto.Cantidad + " - Precio: $" + producto.ListaPrecioDetalle.Precio;

                this.gvItems.DataSource = this.MiFactura.FacturasDetalles;
                this.gvItems.DataBind();
                ScriptManager.RegisterStartupScript(this.upItems, this.upItems.GetType(), "CalcularItemScript", "CalcularItem();", true);
                this.txtCantidadCodigo.Text = "1";
                this.upItems.Update();
            }
        }
        #endregion
        protected void MapearControlesAObjeto(VTAFacturas pFactura)
        {
            pFactura.TipoFactura.IdTipoFactura = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].IdTipoFactura;
            pFactura.TipoFactura.CodigoValor = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].CodigoValor;
            pFactura.TipoFactura.Descripcion = this.MisTiposFacturas[this.ddlTipoFactura.SelectedIndex].Descripcion;
            pFactura.FechaFactura = Convert.ToDateTime(this.txtFechaFactura.Text);
            pFactura.IdListaPrecio = this.ddlListaPrecio.SelectedValue == string.Empty ? (Nullable<int>)null : Convert.ToInt32(this.ddlListaPrecio.SelectedValue);
            pFactura.Afiliado.IdAfiliado = Convert.ToInt32(this.hdfIdAfiliado.Value);
            pFactura.FacturaContado = true;
        }
        private void MapearObjetoControlesPrecarga(VTAFacturas pFactura)
        {
            ListItem item = this.ddlFilialPuntoVenta.Items.FindByValue(pFactura.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta.ToString());
            if (item == null)
                this.ddlFilialPuntoVenta.Items.Add(new ListItem(pFactura.FilialPuntoVenta.TipoPuntoVenta.Descripcion, pFactura.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta.ToString()));
            this.ddlFilialPuntoVenta.SelectedValue = pFactura.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta.ToString(); //Convert.ToInt32(pFactura.PrefijoNumeroFactura).ToString();
            this.ddlFilialPuntoVenta_SelectedIndexChanged(null, EventArgs.Empty);

            if (pFactura.TipoFactura.IdTipoFactura > 0)
            {
                item = this.ddlTipoFactura.Items.FindByValue(pFactura.TipoFactura.IdTipoFactura.ToString());
                if (item == null)
                    this.ddlTipoFactura.Items.Add(new ListItem(pFactura.TipoFactura.Descripcion, pFactura.TipoFactura.IdTipoFactura.ToString()));
                this.ddlTipoFactura.SelectedValue = pFactura.TipoFactura.IdTipoFactura.ToString();
                if (this.MisTiposFacturas.Count > 0)
                    this.ddlTipoFactura_SelectedIndexChanged(null, EventArgs.Empty);

            }
            this.txtFechaFactura.Text = pFactura.FechaFactura.ToShortDateString();

            if (pFactura.IdListaPrecio.HasValue && pFactura.IdListaPrecio.Value > 0)
            {
                item = this.ddlListaPrecio.Items.FindByValue(pFactura.IdListaPrecio.Value.ToString());
                if (item == null)
                    this.ddlListaPrecio.Items.Add(new ListItem(pFactura.ListaPrecio, pFactura.IdListaPrecio.Value.ToString()));
                this.ddlListaPrecio.SelectedValue = pFactura.IdListaPrecio.Value.ToString();
            }
        }
        protected void CalcularTotal()
        {
            decimal? totalSinIva = 0;
            decimal? totalIva = 0;
            decimal? totalConIva = 0;
            decimal totalPercepciones = 0;

            totalSinIva = this.MiFactura.FacturasDetalles.Sum(x => x.SubTotal == null ? 0 : x.SubTotal);
            totalIva = this.MiFactura.FacturasDetalles.Sum(x => x.ImporteIVA == null ? 0 : x.ImporteIVA);
            totalConIva = totalSinIva + totalIva + totalPercepciones;
            this.MiFactura.ImporteSinIVA = totalSinIva.Value;
            this.MiFactura.IvaTotal = totalIva.Value;
            this.MiFactura.ImporteTotal = totalConIva.Value;

            this.txtTotalConIva.Text = totalConIva.Value.ToString("C2");
        }
        protected void btnAgregarNuevo_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasSimplesAgregar.aspx"), true);
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            try
            {
                this.btnAceptar.Visible = false;
                this.Page.Validate("Aceptar");
                if (!this.Page.IsValid)
                {
                    guardo = false;
                    this.UpdatePanel1.Update();
                    ScriptManager.RegisterStartupScript(this.upItems, this.upItems.GetType(), "CalcularItemScript", "CalcularItem();", true);
                    this.upItems.Update();
                    this.btnAceptar.Visible = true;
                    return;
                }
                this.MiFactura.DirPath = this.Request.PhysicalApplicationPath;
                this.MiFactura.AppPath = this.ObtenerAppPath();
                this.MiFactura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                this.MiFactura.ObtenerListaFacturasDetalles();
                this.MiFactura.CuentasCobrosXML = new XmlDocument();
                this.MiFactura.CuentasCobrosXML.LoadXml(this.ArmarXML());
                switch (this.GestionControl)
                {
                    case Gestion.Agregar:
                        this.MapearControlesAObjeto(this.MiFactura);
                        this.MiFactura.FacturasDetalles = this.MiFactura.FacturasDetalles.Where(x => x.EstadoColeccion == EstadoColecciones.Agregado).ToList();
                        AyudaProgramacion.AcomodarIndices<VTAFacturasDetalles>(this.MiFactura.FacturasDetalles);
                        AyudaProgramacion.CargarGrillaListas(this.MiFactura.FacturasDetalles, false, this.gvItems, true);
                        this.upItems.Update();
                        this.MiFactura.UsuarioAlta.IdUsuarioAlta = this.MiFactura.UsuarioLogueado.IdUsuario;
                        this.MiFactura.Filial.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                        this.MiFactura.FacturaContado = true;//this.chkFacturaContado.Checked;

                        guardo = FacturasF.FacturasAgregar(this.MiFactura, new VTARemitos());
                        if (guardo)
                        {
                            this.ctrAsientoMostrar.IniciarControl(this.MiFactura);
                            if (this.MiFactura.Estado.IdEstado == (int)EstadosFacturas.FESinValidadaAfip)
                            {
                                this.MostrarMensaje("ResultadoTransaccionFacturaElectronicaRechazo", System.Drawing.Color.BlueViolet, true, this.MiFactura.CodigoMensajeArgs);
                            }
                            else
                            {
                                this.btnAgregarNuevo.Visible = true;
                                this.btnImprimir.Visible = true;
                                this.btnEnviarMail.Visible = true;
                                this.MostrarArchivo();
                            }
                        }
                        break;
                    default:
                        break;
                }
                if (!guardo)
                {
                    this.btnAceptar.Visible = true;
                    this.MostrarMensaje(this.MiFactura.CodigoMensaje, true, this.MiFactura.CodigoMensajeArgs);
                    if (this.MiFactura.dsResultado != null)
                    {
                        this.ctrPopUpGrilla.IniciarControl(this.MiFactura);
                        this.MiFactura.dsResultado = null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje(ex.Message, true);
            }
            finally
            {
                this.btnAceptar.Visible = !guardo;
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.FacturaModificarDatosCancelar != null)
                this.FacturaModificarDatosCancelar();
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            this.MostrarArchivo();
        }
        protected void btnEnviarMail_Click(object sender, EventArgs e)
        {
            MailMessage mail = new MailMessage();
            if (FacturasF.FacturaArmarMail(this.MiFactura, mail))
            {
                this.popUpMail.IniciarControl(mail, this.MiFactura);
            }
        }
        private void MostrarArchivo()
        {
            VTAFacturas factura = this.MiFactura;
            factura = FacturasF.FacturasObtenerDatosCompletos(factura);
            factura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            VTAFacturas facturaPdf = FacturasF.FacturasObtenerArchivo(factura);

            List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
            TGEArchivos archivo = new TGEArchivos
            {
                Archivo = facturaPdf.FacturaPDF
            };
            if (archivo.Archivo != null)
                listaArchivos.Add(archivo);

            archivo = new TGEArchivos();
            RepReportes reporte = new RepReportes();
            RepParametros param = new RepParametros
            {
                ValorParametro = this.MiFactura.IdFactura.ToString()
            };
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdFactura";
            reporte.Parametros.Add(param);
            if (factura.FacturaContado)
            {
                TGEPlantillas plantilla = new TGEPlantillas
                {
                    Codigo = "OrdenesCobros"
                };
                plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

                reporte.StoredProcedure = plantilla.NombreSP;
                DataSet ds = ReportesF.ReportesObtenerDatos(reporte);

                archivo.Archivo = ReportesF.ExportPDFGenerarReportePDF(plantilla, ds, "IdFactura", this.MiFactura.UsuarioLogueado);
                if (archivo.Archivo != null)
                    listaArchivos.Add(archivo);
            }
            TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();
            string nombreArchivo = string.Concat(empresa.CUIT, "_", factura.TipoFactura.CodigoValor, "_", factura.PrefijoNumeroFactura, "_", factura.NumeroFactura, ".pdf");
            ExportPDF.ConvertirArchivoEnPdf(this.UpdatePanel3, listaArchivos, nombreArchivo);
        }
        protected void ddlBancoCuentaAgrupado_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedvalue = string.Empty;
            if (!string.IsNullOrEmpty(this.ddlBancoCuentaAgrupado.SelectedValue))
            {
                selectedvalue = this.ddlBancoCuentaAgrupado.SelectedValue;
                TESBancosCuentas bc = this.MisBancosCuentas.Find(x => x.Filtro == selectedvalue);
                int idTipoValor = Convert.ToInt32(bc.Filtro.Split('|')[2]);
                switch (idTipoValor)
                {
                    case (int)EnumTiposValores.TarjetaCredito:
                        this.lblLoteEnvio.Visible = true;
                        this.txtLoteEnvio.Visible = true;
                        this.lblCupon.Visible = true;
                        this.txtCupon.Visible = true;
                        break;
                    default:
                        this.lblLoteEnvio.Visible = false;
                        this.txtLoteEnvio.Visible = false;
                        this.lblCupon.Visible = false;
                        this.txtCupon.Visible = false;
                        break;
                }
            }
            this.ddlBancoCuentaAgrupado.Items.Clear();
            this.ddlBancoCuentaAgrupado.SelectedValue = null;
            this.ddlBancoCuentaAgrupado.DataSource = this.MisBancosCuentas;
            this.ddlBancoCuentaAgrupado.DataValueField = "Filtro";
            this.ddlBancoCuentaAgrupado.DataTextField = "Denominacion";
            this.ddlBancoCuentaAgrupado.DataGroupField = "Grupo";
            this.ddlBancoCuentaAgrupado.DataBind();
            if (!string.IsNullOrEmpty(selectedvalue))
                this.ddlBancoCuentaAgrupado.SelectedValue = selectedvalue.ToString();
        }
        private string ArmarXML()
        {
            TESBancosCuentas aux = this.MisBancosCuentas.Find(x => x.Filtro == this.ddlBancoCuentaAgrupado.SelectedValue);
            string idTipoValor = aux.Filtro.Split('|')[2];
            string idBancoCuenta = aux.Filtro.Split('|')[0];
            string XML = "<Detalles>";
            XML = string.Concat(XML, "<Detalle>" + "<IdTipoValor>" + idTipoValor + "</IdTipoValor>");
            XML = string.Concat(XML, "<IdRefTipoValor>" + idBancoCuenta + "</IdRefTipoValor>");
            XML = string.Concat(XML, "<NumeroLote>" + this.txtLoteEnvio.Text + "</NumeroLote>");
            XML = string.Concat(XML, "<NumeroTransaccionPosnet>" + this.txtCupon.Text + "</NumeroTransaccionPosnet>");
            XML = string.Concat(XML, "</Detalle>");
            return string.Concat(XML, "</Detalles>");
        }
    }
}