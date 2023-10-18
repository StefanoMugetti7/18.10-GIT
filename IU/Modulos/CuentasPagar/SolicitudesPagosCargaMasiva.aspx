<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="SolicitudesPagosCargaMasiva.aspx.cs" Inherits="IU.Modulos.CuentasPagar.SolicitudesPagosCargaMasiva" %>

<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
    <script lang="javascript" type="text/javascript">
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridDetalle);
            //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
            //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CompletarCerosComprobantes);
            intiGridDetalle();
            //InitApellidoSelect2();
            //CompletarCerosComprobantes();
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
        //function CompletarCerosComprobantes() {
        //    $("input[type=text][id$='txtNumeroPuntoVenta']").blur(function () { $(this).addLeadingZeros(4); });
        //    $("input[type=text][id$='txtNumeroFactura']").blur(function () { $(this).addLeadingZeros(8); });
        //}
        function CalcularItem() {
            var subTotalConIVA = 0.00;
            var importeIVA = 0.00;
            var TotalSinIVA = 0.00;
            var subTotalItem = 0.00;
            var totalIVA = 0.00;
            //totalConIVA = 0.00;
            //Calculo descuento... 
            $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {

                var cantidad = $(this).find("input:text[id*='txtCantidad']").maskMoney('unmasked')[0];
                var data = $(this).find('[id*="ddlIVA"] option:selected').val();
                var alicuotaIVA = data.split('|')[1];
                var dataComprobante = $(this).find('[id*="ddlTipoComprobante"] option:selected').val();
                var tipoComprobante = dataComprobante.split('|')[2];
                var importe = $(this).find("input:text[id*='txtPrecio']").maskMoney('unmasked')[0];
                var hdfPrecio = $(this).find("input:hidden[id*='hdfPrecio']").maskMoney('unmasked')[0];
                var hdfImportado = $(this).find("input:hidden[id*='hdfImportado']").val();

                if (dataComprobante) {
                    var tipoFactura = dataComprobante.split('|')[1].slice(-1);
                }
                if (hdfImportado != "True") {
                    if (importe && cantidad) {

                        if (alicuotaIVA) {
                            alicuotaIVA = alicuotaIVA.replace('.', '').replace(',', '.');
                            if (alicuotaIVA == "" || isNaN(alicuotaIVA)) {
                                alicuotaIVA = 0.00;
                                importeIVA = 0.00;
                            } else {
                                /* if */
                                if (tipoComprobante == 0) {
                                    if (tipoFactura != 'C') {
                                        hdfPrecio = (parseFloat(importe) / parseFloat((parseFloat((100 + parseFloat(alicuotaIVA))) / 100))).toFixed(2);
                                        subTotalConIVA = (parseFloat(importe) * parseFloat(cantidad));
                                        importeIVA = (parseFloat(subTotalConIVA) - parseFloat(hdfPrecio) * parseFloat(cantidad)).toFixed(2);
                                        TotalSinIVA = parseFloat(hdfPrecio);
                                        totalIVA = parseFloat(importeIVA);
                                    }
                                    else {
                                        hdfPrecio = parseFloat(importe);
                                        subTotalConIVA = (parseFloat(importe) * parseFloat(cantidad));
                                        importeIVA = (parseFloat(subTotalConIVA) - parseFloat(hdfPrecio) * parseFloat(cantidad)).toFixed(2);
                                        TotalSinIVA = parseFloat(hdfPrecio);
                                        totalIVA = parseFloat(importeIVA);
                                    }
                                }
                                else {
                                    hdfPrecio = parseFloat(importe);
                                    subTotalItem = (parseFloat(importe) * parseFloat(cantidad)).toFixed(2);
                                    importeIVA = (parseFloat(subTotalItem) * parseFloat(alicuotaIVA) / 100).toFixed(2);
                                    subTotalConIVA = parseFloat(subTotalItem) + parseFloat(importeIVA);
                                    TotalSinIVA = parseFloat(hdfPrecio);
                                    totalIVA = parseFloat(importeIVA);
                                }
                            }
                            //$(this).find('span[id*="lblSubtotal"]').text(accounting.formatMoney(subTotalItem, gblSimbolo, 2, "."));
                            $(this).find('span[id*="lblTotal"]').text(accounting.formatMoney(subTotalConIVA, gblSimbolo, 2, "."));
                            $(this).find('input:hidden[id*="hdfTotal"]').val(subTotalConIVA);
                            $(this).find('input:hidden[id*="hdfImporteSinIVA"]').val(TotalSinIVA);
                            $(this).find('input:hidden[id*="hdfIvaTotal"]').val(totalIVA);
                            $(this).find("input[type=text][id$='txtPrecio']").val(accounting.formatMoney(importe, gblSimbolo, 2, "."));
                        }
                        else {
                            $(this).find('span[id*="lblTotal"]').text(accounting.formatMoney(subTotalItem, gblSimbolo, 2, "."));
                            $(this).find('input:hidden[id*="hdfTotal"]').val(accounting.formatMoney(subTotalConIVA, "", 2, "."));
                            //$(this).find("input:text[id*='txtPrecio']").val(accounting.formatMoney(importe, gblSimbolo, 2, "."));
                            $(this).find("input[type=text][id$='txtPrecio']").val(accounting.formatMoney(importe, gblSimbolo, 2, "."));
                        }
                    }
                }
            });
        }
        function intiGridDetalle() {
            var rowindex = 0;
            $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
                //CompletarCerosComprobantes
                $(this).find("input[type=text][id*='txtNumeroPuntoVenta']").blur(function () { $(this).addLeadingZeros(5); });
                $(this).find("input[type=text][id*='txtNumeroFactura']").blur(function () { $(this).addLeadingZeros(8); });
                /*Variables Productos*/
                var ddlProducto = $(this).find('[id*="ddlProducto"]');
                var hdfProductoDetalle = $(this).find("input:hidden[id*='hdfProductoDetalle']");
                var hdfIdProducto = $(this).find("input[id*='hdfIdProducto']");
                var txtCantidad = $(this).find("input:text[id*='txtCantidad']");
                var txtPrecio = $(this).find("input:text[id*='txtPrecio']");
                //var hdfPrecio = $(this).find("input:hidden[id*='hdfPrecio']");
                var ddlTipoComprobante = $('select[id$="ddlTipoComprobante"] option:selected').val();
                /*-----------------------------------------------------------------------------------*/
                /*Variables de Proveedores*/
                var ddlProveedor = $(this).find('[id*="ddlProveedor"]');
                var hdfIdProveedor = $(this).find("input[id*='hdfIdProveedor']");
                var hdfProveedorDescripcion = $(this).find("input[id*='hdfProveedorDescripcion']");
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
                                proveedor: hdfIdProveedor.val(),
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
                    hdfProductoDetalle.val(e.params.data.id + ' - ' + e.params.data.productoDescripcion);
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
                    //txtPrecio.val(accounting.formatMoney(0, gblSimbolo, 2, gblSeparadorMil));
                    //hdfPrecio.val('');
                    txtCantidad.val('');
                    //hdfNoIncluidoEnAcopio.val('');
                    CalcularItem();
                });
                /*Fin ddlProducto*/
                /*------------------------------------------------------------------------------------------*/

                /*ddlProveedor*/
                if (hdfIdProveedor.val() > 0) {
                    var newOption = new Option(hdfProveedorDescripcion.val(), hdfIdProveedor.val(), false, true);
                    ddlProveedor.append(newOption).trigger('change');
                }
                ddlProveedor.select2({
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
                        url: '<%=ResolveClientUrl("~")%>/Modulos/Proveedores/ProveedoresWS.asmx/ProveedoresCombo',
                        delay: 500,
                        data: function (params) {
                            return JSON.stringify({
                                value: ddlProveedor.val(), // search term");
                                filtro: params.term, // search term");
                            });
                        },
                        processResults: function (data, params) {
                            //return { results: data.items };
                            return {
                                results: $.map(data.d, function (item) {
                                    return {
                                        id: item.IdProveedor,
                                        text: item.CodigoProveedor,
                                        Cuit: item.TipoDocumentoDescripcion,
                                        NumeroDocumento: item.NumeroDocumento,
                                        RazonSocial: item.RazonSocial,
                                        IdCondicionFiscal: item.IdCondicionFiscal,
                                        Estado: item.EstadoDescripcion,
                                        Beneficiario: item.Beneficiario,
                                    }
                                })
                            };
                            cache: true
                        }
                    },
                });

                ddlProveedor.on('select2:select', function (e) {
                    hdfIdProveedor.val(e.params.data.id);
                    hdfProveedorDescripcion.val(e.params.data.id + ' - ' + e.params.data.RazonSocial);
                    BuscarProductoPorDefecto(ddlProducto, hdfIdProducto, hdfProductoDetalle, txtCantidad, e.params.data.id);
                    $("input[id*='hdfCargoArchivo']").val("1");
                    console.log($("input[id*='hdfCargoArchivo']").val());
                });
                ddlProveedor.on('select2:unselect', function (e) {
                    hdfIdProveedor.val('');
                    hdfProveedorDescripcion.val('');
                    ddlProducto.val(null).trigger('change');
                    hdfProductoDetalle.val('');
                    hdfIdProducto.val('');
                    //txtPrecio.val(accounting.formatMoney(0, gblSimbolo, 2, gblSeparadorMil));
                    //hdfPrecio.val('');
                    txtCantidad.val('');
                    //hdfNoIncluidoEnAcopio.val('');
                    CalcularItem();
                });
                /*Fin ddlProveedor*/
                rowindex++;
            });
        }

        function BuscarProductoPorDefecto(ddlProducto, hdfProductoDetalle, hdfIdProducto, txtCantidad, e) {
            $.ajax({
                type: "POST",
                url: '<%=ResolveClientUrl("~")%>/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica',
                dataType: "json",
                contentType: "application/json",
                data: JSON.stringify({ 'sp': 'CAPProveedoresSeleccionarProductoPorDefecto', 'value': e, 'filtro': '' }),
                //beforeSend: function (xhr, opts) {
                //    if (tx.val() == '' || ddlHoteles.val() == '') {
                //        //console.log("Fecha " + txtReservaFechaIngreso.val());
                //       alert('que onda');
                //    }
                //},
                success: function (res) {

                    $.each(res.d, function (data, value) {
                        //console.log(value.id + '-' + value.text);
                        $(ddlProducto).empty().append($("<option></option>").val(value.id).html(value.text));


                        $(txtCantidad).val(accounting.formatNumber(1, 2, gblSeparadorMil, ","));
                        $(hdfProductoDetalle).val(value.id);
                        $(hdfIdProducto).val(value.text);
                    })
                }
            });
        }
        <%--function onclick() {
            __doPostBack("<%=button.UniqueID %>", "");
        }--%>

        function Validar(ctr, msg) {
            var rowindex = 0;
            var contador = 0;
            var contadordos = 0;
            $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
                var hdfIdProveedor = $(this).find("input[id*='hdfIdProveedor']");


                if (hdfIdProveedor.val() == "") {
                    contador++;
                }
                else {
                    contadordos++;
                }
                rowindex++;
            });
            if (contadordos > 0) {
                msg = msg.replace('###', contadordos);
                showConfirm(ctr, msg);
            }
            else {
                MostrarMensaje('Debe ingresar al menos un item', 'red');
                //return false;
            }
        }
        function ValidarCargados(ctr, msg) {
            var contador = 0;
            $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
                var hdfIdProveedor = $(this).find("input[id*='hdfIdProveedor']");
                if (hdfIdProveedor.val() != "") {
                    contador++;
                }
            });
            console.log(contador);
            if (contador > 0) {
                showConfirm(ctr, msg);
            } else {
                __doPostBack(ctr.name, '');
            }
        }
    </script>
    <%--    <asp:HiddenField ID="hdfIdSolicitudPago" runat="server" />--%>
    <%--    <asp:Button CssClass="botonesEvol" ID="button" OnClick="button_Click" runat="server" style="display:none;"/>--%>
    <asp:UpdatePanel ID="items" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hdfCargoArchivo" runat="server" Value="0" />
            <asp:HiddenField ID="hdfAtacheEventoArchivo" runat="server" Value="0" />
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadAgregar" runat="server" Text="Cantidad"></asp:Label>
                <div class="col-sm-2">
                    <asp:TextBox CssClass="form-control" ID="txtCantidadAgregar" runat="server"></asp:TextBox>
                </div>
                <div class="col-sm-1">
                    <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" runat="server" Text="Agregar item" OnClick="btnAgregarItem_Click" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server"></asp:DropDownList>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="Label1" runat="server" Text="Importar mis Comprobantes AFIP"></asp:Label>
                <div class="col-sm-2">
                    <asp:Panel ID="pnlArchivo" runat="server">
                        <asp:AsyncFileUpload ID="AsyncFileUpload1" Width="211px" runat="server"
                            OnUploadedComplete="FileUploadComplete" Height="21px" Font-Size="Larger" />
                    </asp:Panel>
                </div>
                <div class="col-sm-1">
                    <asp:Button CssClass="botonesEvol" ID="btnCargarArchivo" runat="server" Text="Importar" OnClick="btnCargarArchivo_Click" />
                </div>
            </div>
            <div class="table-responsive">
                <asp:GridView ID="gvDatos" DataKeyNames="IdSolicitudPagoDetalle" AllowPaging="false" AllowSorting="false"
                    runat="server" SkinID="GrillaResponsive"
                    AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvDatos_RowDataBound" OnRowCommand="gvDatos_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Proveedor">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="form-control select2" ID="ddlProveedor" runat="server"></asp:DropDownList>
                                <asp:HiddenField ID="hdfIdProveedor" Value='<%#Eval("ProveedorIdProveedor") %>' runat="server" />
                                <asp:HiddenField ID="hdfProveedorDescripcion" Value='<%#Eval("ProveedorDescripcion") %>' runat="server" />
                                <%--                                <asp:Label CssClass="col-form-label" ID="lblProducto" Visible="false" Enabled="false" Text='<%#Bind("Producto.IdProducto") %>' runat="server"></asp:Label>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Factura&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;">
                            <ItemTemplate>
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaFactura" Text='<%#Eval("FechaFactura", "{0:dd/MM/yyyy}") %>' runat="server"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pto. de Venta">
                            <ItemTemplate>
                                <Evol:CurrencyTextBox CssClass="form-control" Prefix="" DecimalSeparator="" MaxLength="5" ThousandsSeparator="" ID="txtNumeroPuntoVenta" runat="server" Text='<%# Eval("NumeroPuntoVenta")%>'></Evol:CurrencyTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nro. Factura">
                            <ItemTemplate>
                                <Evol:CurrencyTextBox CssClass="form-control" Prefix="" DecimalSeparator="" MaxLength="8" ThousandsSeparator="" ID="txtNumeroFactura" runat="server" Text='<%# Eval("NumeroFactura")%>'></Evol:CurrencyTextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo de Comprobante">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="form-control select2" ID="ddlTipoComprobante" runat="server"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Contable&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;">
                            <ItemTemplate>
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaContable" Text='<%#Eval("FechaContable", "{0:dd/MM/yyyy}")%>' runat="server"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Producto">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="form-control select2" ID="ddlProducto" runat="server"></asp:DropDownList>
                                <asp:HiddenField ID="hdfIdProducto" Value='<%#Bind("ProductoIdProducto") %>' runat="server" />
                                <asp:HiddenField ID="hdfProductoDetalle" Value='<%#Bind("ProductoDescripcion") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cantidad">
                            <ItemTemplate>
                                <Evol:CurrencyTextBox CssClass="form-control" Prefix="" ID="txtCantidad" runat="server" Text='<%# Eval("Cantidad", "{0:N2}")%>'></Evol:CurrencyTextBox>
                                <%--                                <asp:HiddenField ID="hdfCantidad" Value='<%#Eval("Cantidad") %>' runat="server" />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Precio&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;">
                            <ItemTemplate>
                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtPrecio" runat="server" Text='<%#Bind("Precio", "{0:C2}") %>'></Evol:CurrencyTextBox>
                                <asp:HiddenField ID="hdfPrecio" Value='<%#Bind("Precio", "{0:C2}") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Alicuota">
                            <ItemTemplate>
                                <asp:DropDownList CssClass="form-control select2" ID="ddlIVA" runat="server" />
                                <asp:HiddenField ID="hdfIvaTotal" Value='<%#Bind("IvaTotal")%>' runat="server" />
                                <asp:HiddenField ID="hdfImporteSinIVA" Value='<%#Bind("ImporteSinIVA")%>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total" SortExpression="" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTotal" runat="server" Text='<%#Bind("ImporteTotal", "{0:C2}") %>'></asp:Label>
                                <asp:HiddenField ID="hdfTotal" Value='<%#Eval("ImporteTotal") %>' runat="server" />
                                <asp:HiddenField ID="hdfImportado" Value='<%#Eval("Importado") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" SortExpression="">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar"
                                    AlternateText="Eliminar" ToolTip="Eliminar" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="row justify-content-md-center">
                <div class="col-md-auto">
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"
                        OnClick="btnAceptar_Click" ValidationGroup="SolicitudesAceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"
                        OnClick="btnCancelar_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
