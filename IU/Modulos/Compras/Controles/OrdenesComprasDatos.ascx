<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrdenesComprasDatos.ascx.cs" Inherits="IU.Modulos.Compras.Controles.OrdenesComprasDatos" %>
<%--<%@ Register src="~/Modulos/CuentasPagar/Controles/ProveedoresBuscarPopUp.ascx" tagname="popUpBuscarProveedor" tagprefix="auge" %>--%>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/CuentasPagar/Controles/ProductosBuscarPopUp.ascx" TagName="popUpBuscarProducto" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Proveedores/Controles/ProveedoresCabecerasDatos.ascx" TagName="BuscarProveedorAjax" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Compras/Controles/OrdenesComprasValores.ascx" TagName="OrdenesComprasValores" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" TagName="popUpGrillaGenerica" TagPrefix="auge" %>

<script lang="javascript" type="text/javascript">
    $(document).ready(function () {
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridDetalle);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CalcularValores);
        SetTabIndexInput();
        intiGridDetalle();
        CalcularValores();
    });

    function SetTabIndexInput() {
        $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }
    function CalcularValores() {
        var neto = CalcularItem();
        var importe = gvValoresCalcularImporteTotal();

        if (neto == "" || neto == undefined) {
            neto = 0.00;
        }
        if (importe == "" || importe == undefined) {
            importe = 0.00;
        }
        var total = neto - importe;
        $("input[type=text][id$='txtDiferencia']").val(accounting.formatMoney(total, "$ ", 2, "."));
        /*
         122.21
         100
         */
    }
    function CalcularItem() {
        var importeIVA = 0.00;
        var subTotalConIVA = 0.00;
        TotalSinIVA = 0.00;
        var subTotalItem = 0.00;
        totalIVA = 0.00;
        totalConIVA = 0.00;
        //Calculo descuento... 
        var importeTotal = 0.00;
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {

            var cantidad = $(this).find('input:text[id*="txtCantidad"]').val().replace('.', '').replace(',', '.');
            //var alicuotaIVA = $(this).find('[id*="ddlAlicuotaIVA"] option:selected').val();
            var hdfTotalImporte = $(this).find("input[id*='hdfTotalImporte']");
            //var importeTotal = 0.00;// $(this).find('span[id*="lblSubtotalConIva"]').text(); //$("td:eq(4)", this).html();
            var importe = $(this).find("input:text[id*='txtImporte']").maskMoney('unmasked')[0]; //$("td:eq(4)", this).html();
            if (importe && cantidad) {
                subTotalItem = (parseFloat(importe) * parseFloat(cantidad));

                var data = $(this).find('[id*="ddlAlicuotaIVA"] option:selected').val();
                var alicuotaIVA = data.split('|')[1];
                alicuotaIVA = alicuotaIVA.replace('.', '').replace(',', '.');
                if (alicuotaIVA == "" || isNaN(alicuotaIVA)) {
                    alicuotaIVA = 0.00;
                    importeIVA = 0.00;
                } else {
                    importeIVA = (subTotalItem * parseFloat(alicuotaIVA)) / 100;
                }

                subTotalItem = parseFloat(subTotalItem).toFixed(2);
                importeIVA = parseFloat(importeIVA).toFixed(2);
                subTotalConIVA = parseFloat(subTotalItem) + parseFloat(importeIVA);
                importeTotal = parseFloat(importeTotal) + subTotalConIVA;
                TotalSinIVA += parseFloat(subTotalItem);
                totalIVA += parseFloat(importeIVA);

                $(this).find('span[id*="lblImporteIva"]').text(accounting.formatMoney(importeIVA, gblSimbolo, 2, "."));
                $(this).find('input:hidden[id*="hdfImporteIva"]').val(importeIVA);
                $(this).find('span[id*="lblSubtotalConIva"]').text(accounting.formatMoney(subTotalConIVA, gblSimbolo, 2, "."));
                $(this).find('input:hidden[id*="hdfSubtotalConIva"]').val(subTotalConIVA);
                $(this).find('span[id$="lblTotalOrden"]').html(accounting.formatMoney(importeTotal, gblSimbolo, 2, "."));
            }
            else {
                $(this).find('span[id*="lblImporteIva"]').text(accounting.formatMoney(0, gblSimbolo, 2, "."));
                $(this).find('input:hidden[id*="hdfImporteIva"]').val(0);
                $(this).find('span[id*="lblSubtotalConIva"]').text(accounting.formatMoney(0, gblSimbolo, 2, "."));
                $(this).find('input:hidden[id*="hdfSubtotalConIva"]').val(0);
            }
        });
        var importe = gvValoresCalcularImporteTotal();
        var total = importeTotal - importe;
        $("input[type=text][id$='txtDiferencia']").val(accounting.formatMoney(total, "$ ", 2, "."));

        $("#<%=gvDatos.ClientID %> [id$=lblTotalOrden]").text(accounting.formatMoney(importeTotal, gblSimbolo, 2, "."));
        return importeTotal;
    }

    function CalcularTotal() {
        ImporteTotal = 0.00;
        ImporteTotal = CalcularItem();
        $("#<%=gvDatos.ClientID %> [id$=lblTotalOrden]").text(accounting.formatMoney(ImporteTotal, gblSimbolo, 2, "."));

    }

    function intiGridDetalle() {
        var rowindex = 0;
        var idProveedor = $("input[id*='hdfIdProveedor']").val();
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            //CompletarCerosComprobantes
            $(this).find("input[type=text][id*='txtNumeroPuntoVenta']").blur(function () { $(this).addLeadingZeros(4); });
            $(this).find("input[type=text][id*='txtNumeroFactura']").blur(function () { $(this).addLeadingZeros(8); });
            /*Variables Productos*/
            var ddlProducto = $(this).find('[id*="ddlProducto"]');

            var hdfProductoDetalle = $(this).find("input:hidden[id*='hdfProductoDetalle']");
            var hdfIdProducto = $(this).find("input[id*='hdfIdProducto']");
            var txtCantidad = $(this).find("input:text[id*='txtCantidad']");
            var txtPrecio = $(this).find("input:text[id*='txtImporte']");
            var txtNumeroReferencia = $(this).find("input:text[id*='txtNumeroReferencia']");
            //var hdfPrecio = $(this).find("input:hidden[id*='hdfPrecio']");
            var ddlTipoComprobante = $('select[id$="ddlTipoComprobante"] option:selected').val();
            /*-----------------------------------------------------------------------------------*/

            /*Variables de Proveedores*/
            var ddlAfiliado = $(this).find('[id*="ddlAfiliado"]');
            var hdfIdAfiliado = $(this).find("input[id*='hdfIdAfiliado']");
            var hdfAfiliadoDescripcion = $(this).find("input[id*='hdfAfiliadoDescripcion']");
            /*---------------------------------------------------------------------------------*/

            /*ddlProducto*/
            if (hdfIdProducto.val() > 0) {
                var newOption = new Option(hdfProductoDetalle.val(), hdfIdProducto.val(), false, true);
                ddlProducto.append(newOption).trigger('change');
            }

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
                                    precio: item.Precio,
                                    //IdListaPrecioDetalle: item.IdListaPrecioDetalle,
                                }
                            })
                        };
                        cache: true
                    }
                }
            });
            ddlProducto.on('select2:select', function (e) {
                hdfProductoDetalle.val(e.params.data.text);
                hdfIdProducto.val(e.params.data.id);
                //txtPrecio.val(accounting.formatMoney(e.params.data.precio, gblSimbolo, 2, gblSeparadorMil));
                //hdfPrecio.val(e.params.data.precio);
                txtCantidad.val(accounting.formatNumber(1, 2, gblSeparadorMil, ","));
                //hdfNoIncluidoEnAcopio.val(e.params.data.noIncluidoEnAcopio);
                CalcularItem();
            });
            ddlProducto.on('select2:unselect', function (e) {
                hdfProductoDetalle.val('');
                hdfIdProducto.val('');

                //hdfPrecio.val('');
                txtCantidad.val('0')
                hdfIdAfiliado.val('');
                hdfAfiliadoDescripcion.val('');
                ddlAfiliado.val(null).trigger('change');

                txtPrecio.val(accounting.formatMoney(0, gblSimbolo, 2, gblSeparadorMil));
                //hdfPrecio.val('');
                // txtPrecio.val(null).trigger('change');
                CalcularItem();
                //hdfNoIncluidoEnAcopio.val('');
            });
            /*Fin ddlProducto*/
            /*------------------------------------------------------------------------------------------*/
            /*ddlProveedor*/
            if (hdfIdAfiliado.val() > 0) {
                var newOption = new Option(hdfAfiliadoDescripcion.val(), hdfIdAfiliado.val(), false, true);
                ddlAfiliado.append(newOption).trigger('change');
            }

            ddlAfiliado.select2({
                placeholder: 'Ingrese el codigo o Razón Social',
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
                    url: '<%=ResolveClientUrl("~")%>/Modulos/Afiliados/AfiliadosWS.asmx/AfiliadosCombo',

                    delay: 500,
                    data: function (params) {
                        return JSON.stringify({
                            value: ddlAfiliado.val(), // search term");
                            filtro: params.term, // search term");
                        });
                    },
                    processResults: function (data, params) {
                        //return { results: data.items };
                        return {
                            results: $.map(data.d, function (item) {
                                return {
                                    id: item.IdAfiliado,
                                    text: item.DescripcionCombo,
                                    NumeroDocumento: item.NumeroDocumento,
                                    Estado: item.EstadoDescripcion,
                                }
                            })
                        };
                        cache: true
                    }
                },
            });
            ddlAfiliado.on('select2:select', function (e) {
                hdfIdAfiliado.val(e.params.data.id);
                hdfAfiliadoDescripcion.val(e.params.data.text);
                //onclick();
            });
            ddlAfiliado.on('select2:unselect', function (e) {
                hdfIdAfiliado.val('');
                hdfAfiliadoDescripcion.val('');
                //txtPrecio.val(accounting.formatMoney(0, gblSimbolo, 2, gblSeparadorMil));
                //hdfPrecio.val('');
                //hdfNoIncluidoEnAcopio.val('');
                CalcularItem();
            });
            /*Fin ddlProveedor*/
            rowindex++;
        });
    }
</script>
<asp:UpdatePanel ID="UpdatePanelProovedor" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <AUGE:BuscarProveedorAjax ID="ctrBuscarProveedor" runat="server"></AUGE:BuscarProveedorAjax>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:Panel ID="pnlOrdenCompra" GroupingText="Orden Compra" runat="server">
    <div class="form-group row">
        <%--        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCondicionPago" runat="server" Text="Condicion Pago:" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlCondicionPago" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCondicionPago" ControlToValidate="ddlCondicionPago" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>--%>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaEntrega" runat="server" Text="Fecha Entrega"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaEntrega" Enabled="false" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaEntrega" ControlToValidate="txtFechaEntrega" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOrden" runat="server" Text="Tipo" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoOrden" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoOrden" ControlToValidate="ddlTipoOrden" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial de Recepción" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlFiliales" runat="server" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDireccion" runat="server" Text="Direccion de entrega" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtDireccion" runat="server" Enabled="false" TextMode="MultiLine" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblObservacion" runat="server" Text="Observacion" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtObservacion" runat="server" Enabled="false" TextMode="MultiLine" />
        </div>
    </div>
</asp:Panel>
<asp:Panel ID="pnlDetalles" GroupingText="Detalles de la Orden" runat="server">
    <asp:UpdatePanel ID="upOrdenCompraDetalle" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadAgregar" runat="server" Text="Cantidad"></asp:Label>
                <div class="col-sm-1">
                    <asp:TextBox CssClass="form-control" ID="txtCantidadAgregar" runat="server"></asp:TextBox>
                </div>
                <div class="col-sm-2">
                    <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" runat="server" Text="Agregar item" OnClick="btnAgregarItem_Click" />
                </div>
                <div class="col-md-8"></div>
            </div>
            <div class="table-responsive">
                <asp:GridView ID="gvDatos" AllowSorting="false" AllowPaging="false"
                    OnRowCommand="gvDatos_RowCommand" DataKeyNames="IndiceColeccion"
                    runat="server" AutoGenerateColumns="false" ShowFooter="true"
                    OnRowDataBound="gvDatos_RowDataBound" SkinID="GrillaResponsive">
                    <Columns>
                        <asp:TemplateField HeaderText="Código - Producto" SortExpression="">
                            <ItemTemplate>
                                <%--<AUGE:NumericTextBox CssClass="form-control" ID="txtCodigo" AutoPostBack="true" runat="server" Text='<%#Bind("Producto.IdProducto") %>' OnTextChanged="txtCodigo_TextChanged"></AUGE:NumericTextBox>--%>
                                <asp:DropDownList CssClass="form-control select2" ID="ddlProducto" runat="server" Enabled="false"></asp:DropDownList>
                                <asp:HiddenField ID="hdfProductoDetalle" Value='<%#Bind("Producto.Descripcion") %>' runat="server" />
                                <asp:HiddenField ID="hdfIdProducto" Value='<%#Bind("Producto.IdProducto") %>' runat="server" />
                                <asp:Label CssClass="col-form-label" ID="lblProducto" Visible="false" Enabled="false" Text='<%#Bind("Producto.IdProducto") %>' runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Cant.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" SortExpression="">
                            <ItemTemplate>
                                <Evol:CurrencyTextBox CssClass="form-control" Prefix="" ID="txtCantidad" runat="server" Text='<%#Bind("Cantidad") %>'></Evol:CurrencyTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Importe&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" SortExpression="">
                            <ItemTemplate>
                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporte" Text='<%#Bind("Importe") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Alícuota" SortExpression="" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="form-control select2" ID="ddlAlicuotaIVA" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="IVA" SortExpression="" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteIva" runat="server" Text='<%#Bind("ImporteIva", "{0:C2}") %>'></asp:Label>
                                <asp:HiddenField ID="hdfImporteIva" Value='<%#Bind("ImporteIVA")%>' runat="server" />
                                <%--<AUGE:NumericTextBox CssClass="textboxEvol" ID="txtImporteIva" runat="server" Text='<%#Bind("ImporteIVA", "{0:N2}") %>'  Enabled="false" Width="50"></AUGE:NumericTextBox>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Socio">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="form-control select2" ID="ddlAfiliado" runat="server"></asp:DropDownList>
                                <asp:HiddenField ID="hdfIdAfiliado" Value='<%#Eval("Afiliado.IdAfiliado") %>' runat="server" />
                                <asp:HiddenField ID="hdfAfiliadoDescripcion" Value='<%#Eval("Afiliado.DescripcionAfiliado") %>' runat="server" />
                                <%--                                <asp:Label CssClass="col-form-label" ID="lblProducto" Visible="false" Enabled="false" Text='<%#Bind("Producto.IdProducto") %>' runat="server"></asp:Label>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Numero Referencia" SortExpression="">
                            <ItemTemplate>
                                <asp:TextBox CssClass="form-control" ID="txtNumeroReferencia" Value='<%#Eval("NumeroReferencia") %>' Enabled="false" runat="server"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Subtotal" SortExpression="" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSubtotalConIva" runat="server" Text='<%#Bind("ImporteConIva", "{0:C2}") %>'></asp:Label>
                                <asp:HiddenField ID="hdfSubtotalConIva" Value='<%#Bind("ImporteConIva")%>' runat="server" />
                                <%--<font face="arial" size="5" color="red"></Font>--%>
                                <%--<asp:TextBox CssClass="textboxEvol" ID="txtSubtotalConIva" runat="server" Text='<%#Bind("SubTotalConIva", "{0:N2}") %>' Enabled="false" Width="50"></asp:TextBox>--%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblTotalOrden" runat="server" Text="0.00"></asp:Label>
                                <asp:HiddenField ID="hdfTotalImporte" runat="server" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" SortExpression="">
                            <%--     <ItemTemplate>
                                <asp:CheckBox ID="chkIncluir" Visible="false" Checked='<%#Eval("IncluirEnOP") %>' runat="server" />
                            </ItemTemplate>--%>
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar"
                                    AlternateText="Eliminar ítem" ToolTip="Eliminar ítem" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<AUGE:OrdenesComprasValores ID="ctrOrdenesComprasValores" runat="server" />
<div class="form-group row">
    <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblDiferencia" runat="server" Text="Diferencia"></asp:Label>
    <div class="col-sm-3">
        <Evol:CurrencyTextBox CssClass="form-control" ID="txtDiferencia" runat="server" Enabled="false" Text="0.00" />
    </div>
</div>
<asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <AUGE:popUpGrillaGenerica ID="ctrPopUpGrilla" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
