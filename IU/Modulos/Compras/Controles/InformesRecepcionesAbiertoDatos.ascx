<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InformesRecepcionesAbiertoDatos.ascx.cs" Inherits="IU.Modulos.Compras.Controles.InformesRecepcionesAbiertoDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/CuentasPagar/Controles/ProveedoresBuscarPopUp.ascx" TagName="popUpBuscarProveedor" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/CuentasPagar/Controles/ProductosBuscarPopUp.ascx" TagName="popUpBuscarProducto" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Compras/Controles/ImportarSolicitudesPagosDetallesPopUp.ascx" TagName="ImportarSolPagoDetalles" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Compras/Controles/ImportarOrdenesComprasDetallesPopUp.ascx" TagName="ImportarOrdenesComprasDetalles" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Proveedores/Controles/ProveedoresCabecerasDatos.ascx" TagName="BuscarProveedorAjax" TagPrefix="auge" %>

<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
<script lang="javascript" type="text/javascript">

    $(document).ready(function () {
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CopmletarCerosComprobantes);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CalcularItem);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridDetalle);
        intiGridDetalle();
        SetTabIndexInput();
        CopmletarCerosComprobantes();
        CalcularItem();
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
    });

    function CopmletarCerosComprobantes() {
        $("input[type=text][id$='txtNumeroRemito']").blur(function () { $(this).addLeadingZeros(8); });
        $("input[type=text][id$='txtPreNumeroRemito']").blur(function () { $(this).addLeadingZeros(4); });
    }

    function SetTabIndexInput() {
        $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }

    function CalcularItem() {
        var idSolicitudPago = $("input[type=hidden][id$='hdfIdSolicitudPago']").val();
        var total = 0;

        $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {
            var cantidad = $(this).find("input:text[id*='txtCantidad']").maskMoney('unmasked')[0];
            var hdfCantidadRecibida = $(this).find("input[id*='hdfCantidadRecibida']");
            var idSolicitudPagoDetalle = $("input[type=hidden][id*='hdfIdSolicitudPagoDetalle']").val();
            var idOrdenCompraDetalle = $("input[type=hidden][id*='hdfIdOrdenCompraDetalle']").val();
            //Acopio Financiero por Importe
            if (parseInt(idSolicitudPago) > 0) {
                var subTotalItem = 0;
                var precioUnitario = $(this).find("input:text[id*='txtPrecioUnitario']").maskMoney('unmasked')[0];
                if (cantidad && precioUnitario) {
                    subTotalItem = parseFloat(cantidad) * parseFloat(precioUnitario);
                    total += subTotalItem;

                    $(this).find('span[id*="lblSubTotal"]').text(accounting.formatMoney(subTotalItem, gblSimbolo, 4, "."));
                }
            }
            //Importado desde Factura por Cantidades
            else if (parseInt(idSolicitudPagoDetalle) > 0 || parseInt(idOrdenCompraDetalle) > 0) {
                $(this).find("input:text[id*='txtCantidad']").blur(function () {
                    var cantidadRecibida = hdfCantidadRecibida.val().replace('.', '').replace(',', '.');
                    if (parseFloat($(this).maskMoney('unmasked')[0]) > parseFloat(cantidadRecibida)) {
                        $(this).val(accounting.formatMoney(cantidadRecibida, "", 2, "."));
                        $(this).focus();
                    }
                });
            }
        });
        if (parseInt(idSolicitudPago) > 0) {
            $("#<%=gvItems.ClientID %> [id$=lblTotal]").text(accounting.formatMoney(total, gblSimbolo, 4, "."));
            //var PrecioTotal = $(this).find("input[id$='hdfPrecioTotal']");
            $("input[type=hidden][id*='hdfPrecioTotal']").val(total);
        }
    }

   <%-- function CalcularTotal() {
  
        var idSolicitudPago = $("input[type=hidden][id$='hdfIdSolicitudPago']").val();

        var Total1 = 0;

        if (parseInt(idSolicitudPago) > 0) {
          var Importe = $("input[type=text][id$='txtImporte']").maskMoney('unmasked')[0];
          var txtImporteRecibido = $("input[type=text][id$='txtImporteRecibido']").maskMoney('unmasked')[0];
            var Total = $("#<%=gvItems.ClientID %> [id$=lblTotal]").text();
            var Total2 = $("#<%=gvItems.ClientID %> [id$=hdfPrecioTotal]").val();
            console.log("total2: " + Total2);

            var TotalTotal = Importe - txtImporteRecibido;
            if (TotalTotal < Total) {
                console.log("entre");
                 MostrarMensaje('El total del remito no puede ser mayor', 'red');
            }

          
            
        }
    }--%>
    /******************************************************
     Grilla Detalle
 *******************************************************/
    function intiGridDetalle() {
        var rowindex = 0;
        var idSolicitudPago = $("input[type=hidden][id$='hdfIdSolicitudPago']").val();
        var idProveedor = $("input[id*='hdfIdProveedor']").val();

        var cantidadCuotas = 1; //$('select[id$="ddlCantidadCuotas"] option:selected').val();


        $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {
            var ddlProducto = $(this).find('[id*="ddlProducto"]');
            var hdfProductoDetalle = $(this).find("input:hidden[id*='hdfProductoDetalle']");
            var hdfIdProducto = $(this).find("input[id*='hdfIdProducto']");
            var hdfStockeable = $(this).find("input[id*='hdfStockeable']");

            var txtCantidad = $(this).find("input:text[id*='txtCantidad']");
            var txtPrecioUnitario = $(this).find("input:text[id*='txtPrecioUnitario']");
            var hdfPrecioUnitario = $(this).find("input:hidden[id*='hdfPrecioUnitario']");

            var hdfNoIncluidoEnAcopio = $(this).find("input:hidden[id*='hdfNoIncluidoEnAcopio']");
            var lblSubTotal = $(this).find('span[id*="lblSubTotal"]');

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
                        url: '<%=ResolveClientUrl("~")%>/Modulos/Compras/ComprasWS.asmx/CMPProductosSeleccionarFiltro', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                            delay: 500,
                            data: function (params) {
                                return JSON.stringify({
                                    value: ddlProducto.val(), // search term");
                                    filtro: params.term, // search term");
                                    cantidadCuotas: cantidadCuotas,
                                    proveedor: idProveedor,
                                });
                                //var Productos = ObtenerProductosSeleccionadas();
                                //console.log(" array " + Productos);
                                //return "{filtro:" + JSON.stringify(params.term) + ", ListaProductos:" + JSON.stringify(Productos) + "}";
                            },
                            beforeSend: function (xhr, opts) {

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
                                            text: item.DescripcionCombo,
                                            id: item.IdProducto,
                                            productoDescripcion: item.Descripcion,
                                            precio: item.Precio,
                                            precioUnitarioSinIva: item.precioUnitarioSinIva,
                                            noIncluidoEnAcopio: item.NoIncluidoEnAcopio,
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
                        hdfProductoDetalle.val(e.params.data.productoDescripcion);
                        hdfIdProducto.val(e.params.data.id);
                        txtPrecioUnitario.val(accounting.formatMoney(e.params.data.precioUnitarioSinIva, gblSimbolo, 2, gblSeparadorMil));
                        hdfPrecioUnitario.val(e.params.data.precioUnitarioSinIva);
                        txtCantidad.val(accounting.formatNumber(1, 2, gblSeparadorMil, ","));
                        hdfNoIncluidoEnAcopio.val(e.params.data.noIncluidoEnAcopio);
                        hdfStockeable.val(e.params.data.stockeable);
                        //CalcularPrecio();
                        CalcularItem();
                    });
                    ddlProducto.on('select2:unselect', function (e) {
                        txtCantidad.val('0,00');
                        hdfProductoDetalle.val('');
                        hdfIdProducto.val('');
                        txtPrecioUnitario.val(accounting.formatMoney(0, gblSimbolo, 2, gblSeparadorMil));
                        hdfPrecioUnitario.val('');
                        hdfStockeable.val('');
                        hdfNoIncluidoEnAcopio.val('');
                        lblSubTotal.text(accounting.formatMoney(0, gblSimbolo, 2, "."));
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
                            url: '<%=ResolveClientUrl("~")%>/Modulos/Compras/ComprasWS.asmx/CMPProductosSeleccionarFiltro', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                            delay: 500,
                            data: function (params) {
                                return JSON.stringify({
                                    value: ddlProducto.val(), // search term");
                                    filtro: params.term, // search term");
                                    proveedor: idProveedor,

                                });
                                //var Productos = ObtenerProductosSeleccionadas();
                                //console.log(" array " + Productos);
                                //return "{filtro:" + JSON.stringify(params.term) + ", ListaProductos:" + JSON.stringify(Productos) + "}";
                            },
                            beforeSend: function (xhr, opts) {

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
                                            text: item.DescripcionCombo,
                                            id: item.IdProducto,
                                            productoDescripcion: item.Descripcion,
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

                        hdfProductoDetalle.val(e.params.data.productoDescripcion);
                        hdfIdProducto.val(e.params.data.id);
                        txtPrecioUnitario.val(accounting.formatMoney(e.params.data.precioUnitarioSinIva, gblSimbolo, 2, gblSeparadorMil));
                        hdfPrecioUnitario.val(e.params.data.precioUnitarioSinIva);
                        txtCantidad.val(accounting.formatNumber(1, 2, gblSeparadorMil, ","));
                        hdfNoIncluidoEnAcopio.val(e.params.data.noIncluidoEnAcopio);
                        hdfStockeable.val(e.params.data.stockeable);
                        CalcularItem();
                    });
                    ddlProducto.on('select2:unselect', function (e) {
                        txtCantidad.val('0,00');
                        hdfProductoDetalle.val('');

                        hdfIdProducto.val('');
                        txtPrecioUnitario.val(accounting.formatMoney(0, gblSimbolo, 2, gblSeparadorMil));
                        hdfPrecioUnitario.val('');
                        hdfStockeable.val('');
                        hdfNoIncluidoEnAcopio.val('');
                        lblSubTotal.text(accounting.formatMoney(0, gblSimbolo, 2, "."));
                        CalcularItem();
                    });
                }
                rowindex++;
            });
    }

</script>

<AUGE:BuscarProveedorAjax ID="ctrBuscarProveedor" runat="server"></AUGE:BuscarProveedorAjax>
<%--<asp:UpdatePanel ID="UpdatePanelProovedor" UpdateMode="Conditional" runat="server">
    <ContentTemplate>--%>

<%--    <div class="card">
            <div class="card-header">
                Datos del Proveedor
            </div>
            <div class="card-body">
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigo" runat="server" Text="Codigo"></asp:Label>
                    <div class="col-sm-2">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigo" AutoPostBack="true" Enabled="false" OnTextChanged="txtCodigo_TextChanged" runat="server" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCodigo" ControlToValidate="txtCodigo" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-sm-1">
                    <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="BuscarProveedor" ID="btnBuscarProveedor" Enabled="false"
                        AlternateText="Buscar proveedor" ToolTip="Buscar" OnClick="btnBuscarProveedor_Click" />
                    <AUGE:popUpBuscarProveedor ID="ctrBuscarProveedorPopUp" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRazonSocial" runat="server" Text="Razon Social" />
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtRazonSocial" Enabled="false" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCUIT" Text="N° CUIT:" runat="server" />
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtCUIT" Enabled="false" runat="server" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBeneficiario" runat="server" Text="Beneficiario"></asp:Label>
                    <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtBeneficiario" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCondicionFiscal" runat="server" Text="Condicion Fiscal:" Enabled="false"></asp:Label>
                    <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlCondicionFiscal" Enabled="false" runat="server"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>--%>
<%-- </ContentTemplate>

</asp:UpdatePanel>--%>

<div class="card">
    <div class="card-header">
        Remito
    </div>
    <div class="card-body">
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaRemito" runat="server" Text="Fecha Remito"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaRemito" Enabled="false" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaRemito" ControlToValidate="txtFechaRemito" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroRemito" runat="server" Text="Numero Remito"></asp:Label>
            <div class="col-sm-1">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtPreNumeroRemito" Enabled="false" runat="server" MaxLength="4"></AUGE:NumericTextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPreNumeroRemito" ControlToValidate="txtPreNumeroRemito" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <div class="col-sm-2">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroRemito" Enabled="false" runat="server" MaxLength="8"></AUGE:NumericTextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumeroRemito" ControlToValidate="txtNumeroRemito" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>

            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial de Recepción" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlFiliales" runat="server" />
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblObservacion" runat="server" Text="Observacion" />
            <div class="col-sm-7">
                <asp:TextBox CssClass="form-control" ID="txtObservacion" Enabled="false" runat="server" MaxLength="500" TextMode="MultiLine" />
            </div>
        </div>
    </div>
</div>

<asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
    <asp:TabPanel runat="server" ID="tpItems" HeaderText="Detalle de Remito">
        <ContentTemplate>
            <asp:UpdatePanel ID="items" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="form-group row">
                        <div class="col-sm-7">
                            <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" runat="server" Text="Agregar item" OnClick="btnAgregarItem_Click" />
                            <asp:Button CssClass="botonesEvol" ID="btnImportarFactura" Visible="false" runat="server" Text="Importar Factura" OnClick="btnImportarFactura_Click" />
                            <asp:Button CssClass="botonesEvol" ID="btnImportarOC" runat="server" Text="Importar OC" OnClick="btnImportarOC_Click" />
                        </div>
                        <asp:Label CssClass="col-sm-2 col-form-label" ID="lblCantidadDecimales" runat="server" Visible="false" Text="Decimales Precio:"></asp:Label>
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlCantidadDecimales" runat="server" Visible="false" OnSelectedIndexChanged="ddlCantidadDecimales_OnClick" AutoPostBack="true" Enabled="false"></asp:DropDownList>
                        </div>
                    </div>
                    <asp:PlaceHolder ID="phDetalleAcopio" Visible="false" runat="server">
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalleAcopio" runat="server" Text="Acopio"></asp:Label>
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control" ID="txtDetalleAcopio" Enabled="false" runat="server" />
                                <asp:HiddenField ID="hdfIdSolicitudPago" runat="server" />
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label text-right" ID="lblImorte" runat="server" Text="Importe"></asp:Label>
                            <div class="col-sm-2">
                                <asp:TextBox CssClass="form-control" ID="txtImporte" Enabled="false" runat="server" />
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteRecibido" runat="server" Text="Recibido"></asp:Label>
                            <div class="col-sm-2">
                                <asp:TextBox CssClass="form-control text-right" ID="txtImporteRecibido" Enabled="false" runat="server" />
                                <asp:HiddenField ID="hdfPrecioTotal" runat="server" />
                            </div>
                            <div class="col-sm-2">
                                <asp:Button CssClass="botonesEvol" ID="btnEliminarAcopio" Visible="false" runat="server" Text="Eliminar Acopio" OnClick="btnEliminarAcopio_Click" />
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <AUGE:ImportarSolPagoDetalles ID="ctrImportarSolPagosDetalles" runat="server" />
                    <AUGE:ImportarOrdenesComprasDetalles ID="ctrImportarOrdenesComprasDetalles" runat="server" />
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <div class="table-responsive">
                                <asp:GridView ID="gvItems" DataKeyNames="IndiceColeccion" AllowPaging="false" AllowSorting="false"
                                    OnRowCommand="gvItems_RowCommand" runat="server" SkinID="GrillaResponsive"
                                    AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvItems_RowDataBound">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Código - Producto" SortExpression="">
                                            <ItemTemplate>
                                                <%--                                        <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigo" AutoPostBack="true" runat="server" Text='<%#Bind("Producto.IdProducto") %>' OnTextChanged="txtCodigo_TextChanged"></AUGE:NumericTextBox>--%>
                                                <asp:DropDownList CssClass="form-control select2" ID="ddlProducto" runat="server" Enabled="false"></asp:DropDownList>
                                                <asp:HiddenField ID="hdfProductoDetalle" Value='<%#Bind("Producto.Descripcion") %>' runat="server" />
                                                <asp:HiddenField ID="hdfIdProducto" Value='<%#Bind("Producto.IdProducto") %>' runat="server" />
                                                <asp:Label CssClass="col-form-label" ID="lblProducto" Visible="false" Enabled="false" Text='<%#Bind("Producto.IdProducto") %>' runat="server"></asp:Label>
                                                <asp:HiddenField ID="hdfStockeable" Value='<%#Bind("Producto.Familia.Stockeable") %>' runat="server" />
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
                                        <asp:TemplateField HeaderText="Cantidad" SortExpression="cantidad">
                                            <ItemTemplate>
                                                <Evol:CurrencyTextBox CssClass="form-control" Prefix="" ID="txtCantidad" class="form-controlChico" runat="server" Text='<%# Eval("CantidadRecibida", "{0:N2}")%>'></Evol:CurrencyTextBox>
                                                <asp:HiddenField ID="hdfCantidadRecibida" Value='<%#Eval("CantidadRecibida", "{0:N2}") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Precio Unitario" Visible="false" SortExpression="">
                                            <ItemTemplate>
                                                <Evol:CurrencyTextBox CssClass="form-control" Enabled="true" ID="txtPrecioUnitario" runat="server" Text='<%#Eval("PrecioUnitario","{0:C4}") %>'></Evol:CurrencyTextBox>
                                                <asp:HiddenField ID="hdfPrecioUnitario" Value='<%#Bind("PrecioUnitario", "{0:N4}") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="SubTotal" HeaderStyle-CssClass="text-right" ItemStyle-Wrap="false" FooterStyle-Wrap="false" ItemStyle-CssClass="text-right" Visible="false" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label CssClass="col-form-label" ID="lblSubTotal" runat="server"></asp:Label>
                                                <asp:HiddenField ID="hdfSubTotal" runat="server" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label CssClass="labelFooterEvol" ID="lblTotal" runat="server"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Acciones" SortExpression="">
                                            <ItemTemplate>
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                                    AlternateText="Eliminar" ToolTip="Eliminar" />
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
        </ContentTemplate>
    </asp:TabPanel>
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
<br />
<asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
