<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RemitosDatos.ascx.cs" Inherits="IU.Modulos.Facturas.Controles.RemitosDatos" %>
<%--<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>--%>
<%--<%@ Register Src="~/Modulos/Afiliados/Controles/ClientesBuscarPopUp.ascx" TagName="popUpBuscarCliente" TagPrefix="auge" %>--%>
<%@ Register Src="~/Modulos/Facturas/Controles/FacturasBuscarPopUp.ascx" TagName="popUpFacturas" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Facturas/Controles/PresupuestosBuscarPopUp.ascx" TagName="popUpBuscarPresupuesto" TagPrefix="auge" %>
<%--<%@ Register Src="~/Modulos/CuentasPagar/Controles/ProductosBuscarPopUp.ascx" TagName="popUpBuscarProducto" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Facturas/Controles/ProductosBuscarPopUp.ascx" TagName="popUpBuscarProductoPrecio" TagPrefix="auge" %>--%>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Facturas/Controles/NotasPedidosBuscarPopUp.ascx" TagName="popUpBuscarNotaPedido" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadoModificarDatosDomicilioPopUp.ascx" TagName="popUpDomicilio" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/ClientesDatosCabeceraAjax.ascx" TagName="BuscarClienteAjax" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>

<script src="https://cdnjs.cloudflare.com/ajax/libs/clipboard.js/2.0.4/clipboard.min.js"></script>
<script lang="javascript" type="text/javascript">

    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridDetalle);
        intiGridDetalle();
        $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
        $("input[type=text][id$='txtNumeroSocio']").focus();
        //$("input[type=text][id$='txtPrefijoNumeroRemito']").blur(function() { $(this).addLeadingZeros(4); });
        $("input[type=text][id$='txtNumeroRemito']").blur(function () { $(this).addLeadingZeros(8); });
    });
    $.fn.addLeadingZeros = function (length) {
        for (var el of this) {
            _value = el.value.replace(/^0+/, '');
            length = length - _value.length;
            if (length > 0) {
                while (length--) _value = '0' + _value;
            }
            el.value = _value;
        }
    };
    function CopyClipboard() {
        var $temp = $("<input type='text'>");
        $("body").append($temp);
        var link = $("input[type=hidden][id$='hfLinkFirmarDocumento']").val();
        $temp.val(link);
        $temp.focus();
        $temp.select();
        try {
            var successful = document.execCommand('copy');
            MostrarMensaje("Se ha Copiado el Link");
            //$('#copyClipboard').data('tooltip').show();
        } catch (err) {
            console.error('Unable to copy');
        }
        $temp.remove();
    }
    function EnviarWhatsApp(url) {
        var win = window.open(url, '_blank');
        if (win) {
            //Browser has allowed it to be opened
            win.focus();
        } else {
            //Browser has blocked it
            MostrarMensaje('Por favor habilite los popups para este sitio', 'red');
        }
    }
    /******************************************************
       Grilla Detalle
   *******************************************************/
    function intiGridDetalle() {
        var rowindex = 0;
        var idSolicitudPago = $("input[type=hidden][id$='hdfIdSolicitudPago']").val();
        var ddlFilialEntrega = $('select[id$="ddlFilialEntrega"] option:selected').val();
        var cantidadCuotas = 1; //$('select[id$="ddlCantidadCuotas"] option:selected').val();
        var hdfFechaAcopio = $("input[type=hidden][id$='hdfFechaAcopio']");

        $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {
            var ddlProducto = $(this).find('[id*="ddlProducto"]');
            var lblProductoDescripcion = $(this).find('[id*="lblProductoDescripcion"]');
            var hdfIdProducto = $(this).find("input[id*='hdfIdProducto']");
            var hdfProductoDetalle = $(this).find("input:hidden[id*='hdfProductoDetalle']");
            var txtCantidad = $(this).find("input:text[id*='txtCantidad']");
            var txtPrecioUnitario = $(this).find("input:text[id*='txtPrecioUnitario']");
            var hdfPrecioUnitario = $(this).find("input:hidden[id*='hdfPrecioUnitario']");
            var lblStockActual = $(this).find('[id*="lblStockActual"]');
            var hdfStockActual = $(this).find("input:hidden[id*='hdfStockActual']");
            var hdfNoIncluidoEnAcopio = $(this).find("input:hidden[id*='hdfNoIncluidoEnAcopio']");
            var hdfStockeable = $(this).find("input[id*='hdfStockeable']");

            if (hdfIdProducto.val() > 0) {
                var newOption = new Option(hdfProductoDetalle.val(), hdfIdProducto.val(), false, true);
                ddlProducto.append(newOption).trigger('change');
            }
            if (idSolicitudPago > 0) {
                ddlProducto.select2({
                    placeholder: 'Ingrese el codigo o producto',
                    selectOnClose: true,
                    theme: 'bootstrap4',
                    width: '100%',
                    //theme: 'bootstrap',
                    minimumInputLength: 1,
                    language: 'es',
                    //tags: true,
                    allowClear: true,
                    ajax: {
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        url: '<%=ResolveClientUrl("~")%>/Modulos/Facturas/FacturasWS.asmx/RemitosSeleccionarAjaxComboProductosListaPrecio', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                        delay: 500,
                        data: function (params) {
                            return JSON.stringify({
                                value: ddlProducto.val(), // search term");
                                filtro: params.term, // search term");
                                filialEntrega: ddlFilialEntrega,
                                cantidadCuotas: cantidadCuotas,
                                fecha: hdfFechaAcopio.val()
                            });
                            //var Productos = ObtenerProductosSeleccionadas();
                            //console.log(" array " + Productos);
                            //return "{filtro:" + JSON.stringify(params.term) + ", ListaProductos:" + JSON.stringify(Productos) + "}";
                        },
                        beforeSend: function (xhr, opts) {
                            if (ddlFilialEntrega == '') {
                                MostrarMensaje('Debe seleccionar una Filial', 'red');
                                xhr.abort();
                            }
                            var algo = JSON.parse(this.data); // this.data.split('"');
                            if (isNaN(algo.filtro)) {
                                if (algo.filtro.length < 4) {
                                    xhr.abort();
                                }
                            }
                            else {
                            }
                        },
                        processResults: function (data, params) {
                            //return { results: data.items };
                            return {
                                results: $.map(data.d, function (item) {
                                    return {
                                        text: item.ProductoDescripcionCombo,
                                        id: item.ProductoIdProducto,
                                        productoDescripcion: item.ProductoDescripcion,
                                        precio: item.Precio,
                                        precioUnitarioSinIva: item.PrecioUnitarioSinIva,
                                        noIncluidoEnAcopio: item.NoIncluidoEnAcopio,
                                        stockActual: item.StockActual,
                                        stockeable: item.Stockeable,
                                        //IdListaPrecioDetalle: item.IdListaPrecioDetalle,
                                    }
                                })
                            };
                            cache: true
                        }
                    }
                });
                ddlProducto.on('select2:select', function (e) {
                    lblProductoDescripcion.text(e.params.data.productoDescripcion);
                    hdfProductoDetalle.val(e.params.data.text);
                    hdfIdProducto.val(e.params.data.id);
                    txtPrecioUnitario.val(accounting.formatMoney(e.params.data.precioUnitarioSinIva, gblSimbolo, 2, gblSeparadorMil));
                    hdfPrecioUnitario.val(e.params.data.precioUnitarioSinIva);
                    lblStockActual.text(e.params.data.stockActual);
                    hdfStockActual.val(e.params.data.stockActual);
                    txtCantidad.val(accounting.formatNumber(1, 2, gblSeparadorMil, ","));
                    hdfNoIncluidoEnAcopio.val(e.params.data.noIncluidoEnAcopio);
                    hdfStockeable.val(e.params.data.stockeable);
                    //CalcularPrecio();
                    CalcularItem();
                });
                ddlProducto.on('select2:unselect', function (e) {
                    lblProductoDescripcion.text('');
                    hdfProductoDetalle.val('');
                    hdfIdProducto.val('');
                    txtPrecioUnitario.val(accounting.formatMoney(0, gblSimbolo, 2, gblSeparadorMil));
                    hdfPrecioUnitario.val('');
                    lblStockActual.val('');
                    hdfStockActual.val('');
                    hdfModificaPrecio.val('');
                    hdfNoIncluidoEnAcopio.val('');
                    hdfStockeable.val('');
                    CalcularItem();
                });
            }
            else {
                ddlProducto.select2({
                    placeholder: 'Ingrese el codigo o producto',
                    selectOnClose: true,
                    theme: 'bootstrap4',
                    width: '100%',
                    //theme: 'bootstrap',
                    minimumInputLength: 1,
                    language: 'es',
                    //tags: true,
                    allowClear: true,
                    ajax: {
                        type: 'POST',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        url: '<%=ResolveClientUrl("~")%>/Modulos/Facturas/FacturasWS.asmx/RemitosSeleccionarAjaxComboProductos', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                            delay: 500,
                            data: function (params) {
                                return JSON.stringify({
                                    value: ddlProducto.val(), // search term");
                                    filtro: params.term, // search term");
                                    filialEntrega: ddlFilialEntrega,
                                });
                                //var Productos = ObtenerProductosSeleccionadas();
                                //console.log(" array " + Productos);
                                //return "{filtro:" + JSON.stringify(params.term) + ", ListaProductos:" + JSON.stringify(Productos) + "}";
                            },
                            beforeSend: function (xhr, opts) {
                                if (ddlFilialEntrega == '') {
                                    MostrarMensaje('Debe seleccionar una Filial de Entrega', 'red');
                                    xhr.abort();
                                }
                                var algo = JSON.parse(this.data); // this.data.split('"');
                                //console.log(algo.filtro);
                                if (isNaN(algo.filtro)) {
                                    if (algo.filtro.length < 4) {
                                        xhr.abort();
                                    }
                                }
                                else {
                                }
                            },
                            processResults: function (data, params) {
                                //return { results: data.items };
                                return {
                                    results: $.map(data.d, function (item) {
                                        return {
                                            text: item.DescripcionCombo,
                                            id: item.IdProducto,
                                            productoDescripcion: item.Descripcion,
                                            stockActual: item.StockActual,
                                            stockeable: item.Stockeable,
                                            productoDescripcionCompleta: item.ProductoDescripcionCompleta,
                                            //IdListaPrecioDetalle: item.IdListaPrecioDetalle,
                                        }
                                    })
                                };
                                cache: true
                            }
                        }
                    });
                ddlProducto.on('select2:select', function (e) {
                    lblProductoDescripcion.text(e.params.data.productoDescripcion);
                    hdfProductoDetalle.val(e.params.data.productoDescripcionCompleta);
                    hdfIdProducto.val(e.params.data.id);
                    lblStockActual.text(e.params.data.stockActual);
                    hdfStockActual.val(e.params.data.stockActual);
                    hdfStockeable.val(e.params.data.stockeable);
                });
                ddlProducto.on('select2:unselect', function (e) {
                    lblProductoDescripcion.text('');
                    hdfProductoDetalle.val('');
                    hdfIdProducto.val('');
                    lblStockActual.val('');
                    hdfStockActual.val('');
                    hdfStockeable.val('');
                    //CalcularItem();
                });
            }
            rowindex++;
        });
    }
    function CalcularItem() {
        var subTotalItem = 0.00;
        var importeTotal = 0.00;
        $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {
         var cantidad = $(this).find('input:text[id*="txtCantidad"]').val().replace('.', '').replace(',', '.');
         var importe = $(this).find("input:text[id*='txtPrecioUnitario']").maskMoney('unmasked')[0]; //$("td:eq(4)", this).html();
         if (importe && cantidad) {
             subTotalItem = (parseFloat(importe) * parseFloat(cantidad));
             importeTotal += parseFloat(subTotalItem);
             subTotalItem = parseFloat(subTotalItem).toFixed(2);
         }
     });
     $("#<%=gvItems.ClientID %> [id$=lblTotal]").text(accounting.formatMoney(importeTotal, gblSimbolo, 2, "."));
    }
</script>

<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
<%--<asp:UpdatePanel ID="upPnlAfiliado" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="card">
            <div class="card-header">
                Datos del Cliente
            </div>
            <div class="card-body">
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroSocio" runat="server" Text="Numero" />
                    <div class="col-sm-2">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroSocio" AutoPostBack="true" OnTextChanged="txtNumeroSocio_TextChanged" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvNumeroSocioAceptar" ValidationGroup="Aceptar" ControlToValidate="txtNumeroSocio" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-sm-1">
                        <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="BuscarCliente" ID="btnBuscarSocio" Visible="true"
                            AlternateText="Buscar socio" ToolTip="Buscar" OnClick="btnBuscarCliente_Click" />
                    </div>

                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCUIT" runat="server" Text="CUIT" />
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtCUIT" Enabled="false" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSocio" runat="server" Text="Razon Social" />
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtSocio" Enabled="false" runat="server" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCondicionFiscal" runat="server" Text="Cond.Fiscal" />
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtCondicionFiscal" Enabled="false" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtEstado" Enabled="false" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <AUGE:popUpBuscarCliente ID="ctrBuscarClientePopUp" runat="server" />

    </ContentTemplate>
</asp:UpdatePanel>--%>
<AUGE:BuscarClienteAjax ID="ctrBuscarCliente" runat="server"></AUGE:BuscarClienteAjax>
<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="card">
            <div class="card-header">
                Datos del Remito
            </div>
            <div class="card-body">
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialDescripcion" runat="server" Text="Emisión"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlFilialPuntoVenta" runat="server" OnSelectedIndexChanged="ddlFilialPuntoVenta_SelectedIndexChanged"
                            AutoPostBack="true" />
                        <asp:RequiredFieldValidator ID="rfvFilialPuntoVenta" ValidationGroup="Aceptar" ControlToValidate="ddlFilialPuntoVenta" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoFactura" runat="server" Text="Compbte" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoFactura" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoFactura_SelectedIndexChanged" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvTipoFactura" ValidationGroup="Aceptar" ControlToValidate="ddlTipoFactura" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="Label1" runat="server" Text="Número"></asp:Label>
                    <div class="col-sm-1">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlPrefijoNumeroFactura" AutoPostBack="true" OnSelectedIndexChanged="ddlPrefijoNumeroFactura_SelectedIndexChanged" runat="server" />
                    </div>
                    <div class="col-sm-2">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroRemito" Enabled="false" runat="server" MaxLength="8" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaRemito" runat="server" Text="Fecha"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaRemito" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaRemito" ControlToValidate="txtFechaRemito" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstadoRemito" runat="server" Text="Estado" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstadosRemtios" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEstadosRemtios_SelectedIndexChanged" />
                        <asp:RequiredFieldValidator ID="rfvEstadosRemitos" ValidationGroup="Aceptar" ControlToValidate="ddlEstadosRemtios" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaEntrega" runat="server" Text="Entrega"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaEntrega" Enabled="true" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="Validador" Enabled="false" ID="rfvFechaEntrega" ControlToValidate="txtFechaEntrega" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialEntrega" runat="server" Text="Filial Entrega" ToolTip="Filial de la que se descontara el Stock!"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlFilialEntrega" runat="server" ToolTip="Filial de la que se descontara el Stock!" OnSelectedIndexChanged="ddlFilialEntrega_SelectedIndexChanged"
                            AutoPostBack="true" />
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFilialEntrega" ControlToValidate="ddlFilialEntrega" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Operacion" ToolTip="Tipo de Operacion"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoOperacion" Enabled="false" runat="server" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDomicilioEntrega" runat="server" Text="Domicilio de Entrega"></asp:Label>
                    <div class="col-sm-7">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlDomicilioEntrega" runat="server" Enabled="false" />
                    </div>
                    <div class="col-sm-4">
                        <AUGE:popUpDomicilio ID="ctrDomicilios" runat="server" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregarDomicilio" runat="server" Text="Agregar domicilio"
                            OnClick="btnAgregarDomicilio_Click" Visible="false" CausesValidation="false" />
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-6">
                        <asp:TextBox CssClass="form-control" Rows="2" ID="txtObservacionComprobante" Enabled="false" runat="server" Placeholder="Observacion Comprobante" TextMode="MultiLine" />
                    </div>
                    <div class="col-sm-6">
                        <asp:TextBox CssClass="form-control" Rows="2" ID="txtObservacionInterna" Enabled="false" runat="server" Placeholder="Observacion Interna" TextMode="MultiLine" />
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<AUGE:CamposValores ID="ctrCamposValores" runat="server" />
<asp:UpdatePanel ID="items" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="card">
            <div class="card-header">
                Detalle del Comprobante
            </div>
            <div class="card-body">
                <div class="form-group row">
                    <%--                        <AUGE:popUpBuscarProducto ID="ctrBuscarProductoPopUp" runat="server" />
                        <AUGE:popUpBuscarProductoPrecio ID="ctrBuscarProductoPrecioPoup" runat="server" />--%>
                    <AUGE:popUpBuscarPresupuesto ID="ctrBuscarPresupuestoPopUp" runat="server" />
                    <AUGE:popUpFacturas ID="ctrBuscarFacturasPopUp" runat="server" />
                    <AUGE:popUpBuscarNotaPedido ID="ctrBuscarNotaPedidoPopUp" runat="server" />
                    <asp:PlaceHolder ID="phAgregarItem" runat="server">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadAgregar" runat="server" Text="Cantidad"></asp:Label>
                        <div class="col-sm-1">
                            <asp:TextBox CssClass="form-control" ID="txtCantidadAgregar" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-sm-2">
                            <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" runat="server" Text="Agregar item" OnClick="btnAgregarItem_Click" />
                        </div>
                    </asp:PlaceHolder>
                    <div class="col-sm-5">
                        <div class="btn-group" role="group">
                            <button type="button" class="botonesEvol dropdown-toggle"
                                data-toggle="dropdown" aria-expanded="false">
                                Importar <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu" role="menu">
                                <li>
                                    <asp:Button CssClass="dropdown-item" ID="btnImportarFactura" Visible="false" runat="server" Text="Importar Factura" OnClick="btnImportarFactura_Click" /></li>
                                <li>
                                    <asp:Button CssClass="dropdown-item" ID="btnImportarPresupuesto" Visible="false" runat="server" Text="Importar Presupuesto" OnClick="btnImportarPresupuesto_Click" /></li>
                                <li>
                                    <asp:Button CssClass="dropdown-item" ID="btnImportarNotaPedido" Visible="false" runat="server" Text="Importar Notas de Pedido" OnClick="btnImportarNotaPedido_Click" /></li>
                                <li>
                            </ul>
                        </div>
                    </div>
                </div>
                <asp:PlaceHolder ID="phDetalleAcopio" Visible="false" runat="server">
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalleAcopio" runat="server" Text="Acopio"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control" ID="txtDetalleAcopio" Enabled="false" runat="server" />
                            <asp:HiddenField ID="hdfIdSolicitudPago" runat="server" />
                            <asp:HiddenField ID="hdfFechaAcopio" runat="server" />
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label text-right" ID="lblImorte" runat="server" Text="Importe"></asp:Label>
                        <div class="col-sm-2">
                            <asp:TextBox CssClass="form-control" ID="txtImporte" Enabled="false" runat="server" />
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteRecibido" runat="server" Text="Entregado"></asp:Label>
                        <div class="col-sm-2">
                            <asp:TextBox CssClass="form-control text-right" ID="txtImporteRecibido" Enabled="false" runat="server" />
                        </div>
                        <div class="col-sm-2">
                            <asp:Button CssClass="botonesEvol" ID="btnEliminarAcopio" Visible="false" runat="server" Text="Eliminar Acopio" OnClick="btnEliminarAcopio_Click" />
                        </div>
                    </div>
                </asp:PlaceHolder>
                <div class="table-responsive">
                    <asp:GridView ID="gvItems" DataKeyNames="IndiceColeccion" AllowPaging="false" AllowSorting="false"
                        OnRowCommand="gvItems_RowCommand" runat="server" SkinID="GrillaResponsive"
                        AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvItems_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Código - Producto" SortExpression="">
                                <ItemTemplate>
                                    <%--                                        <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigo" AutoPostBack="true" runat="server" Text='<%#Bind("Producto.IdProducto") %>' OnTextChanged="txtCodigo_TextChanged"></AUGE:NumericTextBox>--%>
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlProducto" runat="server" Enabled="false"></asp:DropDownList>
                                    <asp:HiddenField ID="hdfIdFacturaDetalle" Value='<%#Eval("IdFacturaDetalle") %>' runat="server" />
                                    <asp:HiddenField ID="hdfIdProducto" Value='<%#Bind("Producto.IdProducto") %>' runat="server" />
                                    <asp:HiddenField ID="hdfStockeable" Value='<%#Bind("Producto.Familia.Stockeable") %>' runat="server" />
                                    <asp:Label CssClass="col-form-label" ID="lblProducto" Visible="false" Enabled="false" Text='<%#Bind("Producto.Descripcion") %>' runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Detalle" SortExpression="">
                                <ItemTemplate>
                                    <%--<%#Eval("Producto.Descripcion") %>--%>
                                    <asp:Label CssClass="col-form-label" ID="lblProductoDescripcion" Visible="false" Text='<%#Bind("Descripcion")%>' runat="server"></asp:Label>
                                    <asp:TextBox CssClass="form-control" ID="txtDescripcion" Enabled="false" Text='<%#Bind("Descripcion") %>' runat="server"></asp:TextBox>
                                    <asp:HiddenField ID="hdfProductoDetalle" Value='<%#Bind("Producto.Descripcion") %>' runat="server" />
                                    <asp:HiddenField ID="hdfNoIncluidoEnAcopio" Value='<%#Bind("NoIncluidoEnAcopio") %>' runat="server" />
                                    <%--<asp:TextBox CssClass="form-control" ID="txtProducto" runat="server" Text='<%#Bind("Producto.Descripcion") %>' Enabled="true"></asp:TextBox>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:TextBox CssClass="form-control d-inline-block align-middle" ID="txtDescripcion" Enabled="false" Text='<%#Bind("Descripcion") %>' runat="server" TextMode="MultiLine"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Tipo y Nro. Factura" SortExpression="">
                                <ItemTemplate>
                                    <%#Eval("TipoNumeroFactura") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Stock Actual" SortExpression="StockActual">
                                <ItemTemplate>
                                    <%--<%# Eval("StockActual")%>--%>
                                    <asp:Label CssClass="col-form-label" ID="lblStockActual" Text='<%#Bind("StockActual")%>' runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdfStockActual" Value='<%#Bind("StockActual") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cantidad Entregada" SortExpression="CantidadEntregada">
                                <ItemTemplate>
                                    <%# Eval("CantidadEntregada")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cantidad" SortExpression="cantidad">
                                <ItemTemplate>
                                    <Evol:CurrencyTextBox CssClass="form-control" Prefix="" ID="txtCantidad" class="form-controlChico" runat="server" Text='<%# Eval("Cantidad", "{0:N2}")%>'></Evol:CurrencyTextBox>
                                    <asp:HiddenField ID="hdfCantidad" Value='<%#Eval("Cantidad") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Precio Unitario" Visible="false" SortExpression="">
                                <ItemTemplate>
                                    <Evol:CurrencyTextBox CssClass="form-control" Enabled="false" ID="txtPrecioUnitario" runat="server" Text='<%#Eval("PrecioUnitario","{0:C2}") %>'></Evol:CurrencyTextBox>
                                    <asp:HiddenField ID="hdfPrecioUnitario" Value='<%#Bind("PrecioUnitario", "{0:N2}") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SubTotal" HeaderStyle-CssClass="text-right" ItemStyle-Wrap="false" FooterStyle-Wrap="false" ItemStyle-CssClass="text-right" Visible="false" SortExpression="">
                                <ItemTemplate>
                                    <asp:Label CssClass="col-form-label" ID="lblSubTotal" runat="server"></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label CssClass="labelFooterEvol" ID="lblTotal" Visible="true" Text="0.00" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones" SortExpression="">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                        AlternateText="Elminiar" ToolTip="Eliminar" />
                                    <asp:Image CssClass="gvImage" ID="imgNoIncluidoEnAcopio" ImageUrl="~/Imagenes/9.png" Visible="false" AlternateText="El Producto No se encuentra en la lista de precio del Acopio y se trajo el precio actual" ToolTip="El Producto No se encuentra en la lista de precio del Acopio y se trajo el precio actual" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
    <asp:TabPanel runat="server" ID="tpComentarios" HeaderText="Comentarios">
        <ContentTemplate>
            <AUGE:Comentarios ID="ctrComentarios" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpArchivos" HeaderText="Archivos">
        <ContentTemplate>
            <AUGE:Archivos ID="ctrArchivos" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>
    <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria">
        <ContentTemplate>
            <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>
</asp:TabContainer>
<asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                    OnClick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                <asp:HiddenField ID="hfLinkFirmarDocumento" runat="server" />
                <button runat="server" visible="false" type="button" id="copyClipboard" data-tooltip="Se ha copiado el link" class="botonesEvol" onclick="CopyClipboard()">Copiar link</button>
                <asp:ImageButton CssClass="btn" ImageUrl="~/Imagenes/whatsup26x26.jpg" runat="server" ID="btnWhatsAppFirmarDocumento" Visible="false"
                    OnClick="btnWhatsAppFirmarDocumento_Click" AlternateText="Enviar Whatsapp para Firmar" ToolTip="Enviar Whatsapp para Firmar" />
                <asp:Button CssClass="botonesEvol" ID="btnFirmaDigital" Visible="false" runat="server" Text="Firma Digital" OnClick="btnFirmaDigital_Click" />
                <asp:Button CssClass="botonesEvol" ID="btnFirmaDigitalBaja" Visible="false" runat="server" Text="Eliminar Firma" OnClick="btnFirmaDigitalBaja_Click" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <%--<asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />--%>
                <asp:Button CssClass="botonesEvol" UseSubmitBehavior="false" ID="btnCancelar" runat="server" Text="Volver" OnClientClick="BotonVolver();" />
                <%--<button type="button" class="botonesEvol" onclick="BotonVolver();" >Cancel</button>--%>
            </div>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnFirmaDigital" />
    </Triggers>
</asp:UpdatePanel>