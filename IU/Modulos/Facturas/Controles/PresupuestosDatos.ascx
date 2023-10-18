<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PresupuestosDatos.ascx.cs" Inherits="IU.Modulos.Facturas.Controles.PresupuestosDatos" %>

<%--<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>--%>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/ClientesDatosCabeceraAjax.ascx" TagName="BuscarClienteAjax" TagPrefix="auge" %>
<%--<%@ Register src="~/Modulos/Facturas/Controles/ProductosBuscarPopUp.ascx" tagname="popUpBuscarProducto" tagprefix="auge" %>--%>
<%@ Register Src="~/Modulos/Facturas/Controles/RemitosBuscarPopUp.ascx" TagName="popUpBuscarRemito" TagPrefix="auge" %>

<script lang="javascript" type="text/javascript">
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridDetalle);
        intiGridDetalle();
    });

    function AgregarDetalle(ctrl) {
        var row = $(ctrl).parent().parent();
        $(row).find('input:text[id*="txtDescripcion"]').show();
    }

    function CalcularItem() {
        var importeIVA = 0.00;
        var subTotalConIVA = 0.00;
        var TotalSinIVA = 0.00;
        var subTotalItem = 0.00;
        var totalIVA = 0.00;
        var totalConIVA = 0.00;
        var descuento = 0.00;
        var cantidadCuotas = 1; // $('select[id$="ddlCantidadCuotas"] option:selected').val();

        $('#<%=gvItems.ClientID%> tbody tr').each(function () {

            //var incluir = $("td:eq(6) :checkbox", this).is(":checked");
            //             var incluir = $(this).find('input:checkbox[id$="chkIncluir"]').is(":checked");

            var importe = $(this).find("input:text[id*='txtPrecioUnitario']").maskMoney('unmasked')[0]; //$("td:eq(4)", this).html();
            var precio = $(this).find("input[id*='hdfPrecio']").val();
            var margenPorcentaje = $(this).find("input[id*='hdfMargenPorcentaje']").val();
            var margenImporte = $(this).find("input[id*='hdfMargenImporte']").val();
            var financiacionPorcentaje = $(this).find("input[id*='hdfFinanciacionPorcentaje']").val();
            var cantidad = $(this).find('input:text[id*="txtCantidad"]').maskMoney('unmasked')[0];
            var alicuotaIVA = $(this).find('[id*="ddlAlicuotaIVA"] option:selected').val();
            var descuentoPorcentaje = $(this).find('[id*="txtDescuentoPorcentual"]').maskMoney('unmasked')[0];
            var idProducto = $(this).find('[id*="ddlProducto"] option:selected').val();
            //var idProducto = $(this).find('input:text[id*="txtCodigo"]').val();
            //Math.Round((this.Precio * (1 + this.ListaPrecio.MargenPorcentaje / 100) + this.ListaPrecio.MargenImporte) * (1 + this.ListaPrecio.FinanciacionPorcentaje * (pCantidadCuotas == 1 ? 0 : pCantidadCuotas) / 100) + this.ListaPrecio.MargenImporte, 2);

            if (descuentoPorcentaje > 100) {
                descuentoPorcentaje = 100;
                $(this).find('[id*="txtDescuentoPorcentual"]').val(accounting.formatMoney(descuentoPorcentaje, "", gblCantidadDecimales, "."));
            }

            if (importe && cantidad && precio && idProducto && idProducto > 0) {
                //importe = importe.replace('.', '').replace(',', '.'); //remplazo . por nada y , por .
                precio = precio.replace('.', '').replace(',', '.');
                margenPorcentaje = margenPorcentaje.replace('.', '').replace(',', '.');
                margenImporte = margenImporte.replace('.', '').replace(',', '.');
                financiacionPorcentaje = financiacionPorcentaje.replace('.', '').replace(',', '.');

                //VER DE AGREGAR UNA VARIABLE PARA IMPORTE FIJO EN CUOTAS!!!
                //importe = (parseFloat(importe) * (1 + parseFloat(margenPorcentaje) / 100) + parseFloat(margenImporte)) * (1 + parseFloat(financiacionPorcentaje) * (cantidadCuotas == 1 ? 0 : cantidadCuotas) / 100) + parseFloat(margenImporte);
                //importe = parseFloat(importe).toFixed(2);
                //descuentoPorcentaje = descuentoPorcentaje.replace('.', '').replace(',', '.');
                descuento = parseFloat(parseFloat(importe) * parseFloat(cantidad) * parseFloat(descuentoPorcentaje) / 100).toFixed(2);
                $(this).find('span[id*="lblDescuentoImporte"]').text(accounting.formatMoney(descuento, gblSimbolo, 2, "."));
                subTotalItem = parseFloat(importe) * parseFloat(cantidad) - parseFloat(descuento);
                $(this).find('span[id*="lblSubtotal"]').text(accounting.formatMoney(subTotalItem, gblSimbolo, 2, "."));
                alicuotaIVA = alicuotaIVA.replace('.', '').replace(',', '.');
                if (alicuotaIVA == "" || isNaN(alicuotaIVA)) {
                    alicuotaIVA = 0.00;
                    importeIVA = 0.00;
                } else {
                    importeIVA = parseFloat(subTotalItem) * parseFloat(alicuotaIVA) / 100;
                }

                subTotalConIVA = parseFloat(subTotalItem) + parseFloat(importeIVA);
                TotalSinIVA += parseFloat(subTotalItem);
                totalIVA += parseFloat(importeIVA);
                //                 totalConIVA += parseFloat(subTotalItem) + parseFloat(importeIVA);
                $(this).find('span[id*="lblImporteIva"]').text(accounting.formatMoney(importeIVA, gblSimbolo, 2, "."));
                $(this).find('span[id*="lblSubtotalConIva"]').text(accounting.formatMoney(subTotalConIVA, gblSimbolo, 2, "."));
            }
        });
        totalConIVA = parseFloat(TotalSinIVA) + parseFloat(totalIVA);
        $("input[type=text][id$='txtTotalSinIva']").val(accounting.formatMoney(TotalSinIVA, gblSimbolo, 2, "."));
        $("input[type=text][id$='txtTotalIva']").val(accounting.formatMoney(totalIVA, gblSimbolo, 2, "."));
        $("input[type=text][id$='txtTotalConIva']").val(accounting.formatMoney(totalConIVA, gblSimbolo, 2, "."));
    }

    /******************************************************
       Grilla Detalle
   *******************************************************/
    function intiGridDetalle() {
        var rowindex = 0;
        var cantidadCuotas = 1;// $('select[id$="ddlCantidadCuotas"] option:selected').val();
        var idAfiliado = $("input[id*='hdfIdAfiliado']").val();
        var idFilialPredeterminada = $("input[id*='hdfIdFilialPredeterminada']").val();
        var idUsuarioEvento = $("input[id*='hdfIdUsuarioEvento']").val();
        var idMoneda = $('select[id$="ddlMoneda"] option:selected').val();
        $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {
            var ddlProducto = $(this).find('[id*="ddlProducto"]');
            var lblProductoDescripcion = $(this).find('[id*="lblProductoDescripcion"]');
            var hdfIdProducto = $(this).find("input[id*='hdfIdProducto']");
            var hdfIdListaPrecioDetalle = $(this).find("input[id*='hdfIdListaPrecioDetalle']");
            var hdfProductoDetalle = $(this).find("input:hidden[id*='hdfProductoDetalle']");
            var txtCantidad = $(this).find("input:text[id*='txtCantidad']");
            var txtPrecioUnitario = $(this).find("input:text[id*='txtPrecioUnitario']");
            var hdfPrecio = $(this).find("input:hidden[id*='hdfPrecio']");
            var costo = $(this).find("input:hidden[id*='hdfCosto']");
            var margenPorcentaje = $(this).find("input:hidden[id*='hdfMargenPorcentaje']");
            var lblCosto = $(this).find('[id*="lblCosto"]');

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
                    url: '<%=ResolveClientUrl("~")%>/Modulos/Facturas/FacturasWS.asmx/FacturasSeleccionarAjaxComboProductos', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                    delay: 500,
                    data: function (params) {
                        return JSON.stringify({
                            value: ddlProducto.val(), // search term");
                            filtro: params.term, // search term");
                            cantidadCuotas: cantidadCuotas,
                            idAfiliado: idAfiliado,
                            idFilialPredeterminada: idFilialPredeterminada,
                            idMoneda: idMoneda, //hardcodeo xq se agrego idmoneda
                            idUsuarioEvento: idUsuarioEvento,
                            idListaPrecio: 0,
                        });
                        //var Productos = ObtenerProductosSeleccionadas();
                        //console.log(" array " + Productos);
                        //return "{filtro:" + JSON.stringify(params.term) + ", ListaProductos:" + JSON.stringify(Productos) + "}";
                    },
                    beforeSend: function (xhr, opts) {
                        if (idMoneda == "") {
                            MostrarMensaje('Debe Ingresar una Moneda para poder continuar', 'red');
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
                                    text: item.ProductoDescripcionCombo,
                                    id: item.ProductoIdProducto,
                                    productoDescripcion: item.ProductoDescripcion,
                                    precio: item.Precio,
                                    precioUnitarioSinIva: item.PrecioUnitarioSinIva,
                                    //precioEditable: item.PrecioEditable,
                                    idListaPrecioDetalle: item.IdListaPrecioDetalle,
                                    margenPorcentaje: item.margenPorcentaje,
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
                hdfPrecio.val(e.params.data.precioUnitarioSinIva);
                txtCantidad.val(accounting.formatNumber(1, 2, gblSeparadorMil, ","));
                lblCosto.text(accounting.formatMoney(e.params.data.precioUnitarioSinIva, gblSimbolo, 2, gblSeparadorMil));
                hdfIdListaPrecioDetalle.val(e.params.data.idListaPrecioDetalle);
                costo.val(e.params.data.precioUnitarioSinIva);
                margenPorcentaje.val(e.params.data.margenPorcentaje);
                //CalcularPrecio();
                CalcularItem();

            });
            ddlProducto.on('select2:unselect', function (e) {
                lblProductoDescripcion.text('');
                hdfProductoDetalle.val('');
                hdfIdListaPrecioDetalle.val('');
                hdfIdProducto.val('');
                txtPrecioUnitario.val(accounting.formatMoney(0, gblSimbolo, 2, gblSeparadorMil));
                hdfPrecio.val('');
                lblCosto.text('');
                costo.val('');
                margenPorcentaje.val('');
                CalcularItem();
            });

            rowindex++;
        });
    }

    function AplicarPorcentaje() {
        var aplicarPorcentajeDescuento = $("input[type=text][id$='txtAplicarPorcentajeDescuento']").maskMoney('unmasked')[0];
        if (aplicarPorcentajeDescuento) {
            $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {
                var txtDescuentoPorcentaje = $(this).find('[id*="txtDescuentoPorcentual"]').val();
                if (aplicarPorcentajeDescuento > 100) {
                    aplicarPorcentajeDescuento = 100.00;
                    $("input[type=text][id$='txtAplicarPorcentajeDescuento']").val(accounting.formatMoney(aplicarPorcentajeDescuento, '', gblCantidadDecimales, '.'));
                }
                $(this).find('[id*="txtDescuentoPorcentual"]').val(accounting.formatMoney(aplicarPorcentajeDescuento, '', gblCantidadDecimales, gblSeparadorMil));
                //if (txtDescuentoPorcentaje == '0,00') {
                //    $(this).find('[id*="txtDescuentoPorcentual"]').val(accounting.formatNumber(aplicarPorcentajeDescuento, 2, gblSeparadorMil));
                //    //txtDescuentoPorcentaje.val(accounting.formatNumber(aplicarPorcentajeDescuento, 2, gblSeparadorMil));
                //}
            });
            CalcularItem();
        }
    }
</script>

<%--<asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
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
                    </div>
                    <div class="col-sm-1">
                        <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="BuscarCliente" ID="btnBuscarSocio" Visible="true"
                        AlternateText="Buscar socio" ToolTip="Buscar" OnClick="btnBuscarCliente_Click" />
                    <asp:ImageButton ImageUrl="~/Imagenes/Baja.png" runat="server" ID="btnLimpiar"
                        AlternateText="Limpiar" ToolTip="Limpiar" OnClick="btnLimpiar_Click" Visible="false" />
                        </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSocio" runat="server" Text="Razon Social" />
                    <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtSocio" runat="server" />
                    <asp:RequiredFieldValidator ID="rfvSocio" ValidationGroup="Aceptar" ControlToValidate="txtSocio" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblContacto" runat="server" Text="Contacto"></asp:Label>
                    <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtContacto" runat="server"></asp:TextBox>
                        </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTelefono" runat="server" Text="Telefono" />
                    <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtTelefono" runat="server" />
                        </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCorreoElectronico" runat="server" Text="Email" />
                    <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtCorreoElectronico" runat="server" />
                        </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>    --%>
<AUGE:BuscarClienteAjax ID="ctrBuscarCliente" runat="server" />
<asp:HiddenField ID="hdfIdFilialPredeterminada" runat="server" />
<asp:HiddenField ID="hdfIdUsuarioEvento" runat="server" />

<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="card">
            <div class="card-header">
                Presupuesto
            </div>
            <div class="card-body">
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaPresupuesto" runat="server" Text="Fecha Presupuesto"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaPresupuesto" Enabled="false" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaPresupuesto" ControlToValidate="txtFechaPresupuesto" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlMoneda" AutoPostBack="true" OnSelectedIndexChanged="ddlMoneda_SelectedIndexChanged" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvMonedas" ValidationGroup="Aceptar" ControlToValidate="ddlMoneda" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hdfMonedaCotizacion" Value="" runat="server" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblObservacion" runat="server" Text="Observacion" />
                    <div class="col-sm-7">
                        <asp:TextBox CssClass="form-control" ID="txtObservacion" runat="server" TextMode="MultiLine" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblContacto" runat="server" Text="Contacto"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtContacto" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTelefono" runat="server" Text="Telefono" />
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtTelefono" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCorreoElectronico" runat="server" Text="Email" />
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtCorreoElectronico" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<asp:UpdatePanel ID="upItems" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <%--<AUGE:popUpBuscarProducto ID="ctrBuscarProductoPopUp" runat="server" />--%>
        <div class="card">
            <div class="card-header">
                Detalle del Comprobante
            </div>
            <div class="card-body">
                <div class="form-group row">
                    <asp:PlaceHolder ID="phAgregarItem" runat="server">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadAgregar" runat="server" Text="Cantidad"></asp:Label>
                        <div class="col-sm-1">
                            <asp:TextBox CssClass="form-control" ID="txtCantidadAgregar" runat="server"></asp:TextBox>
                        </div>
                        <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" runat="server" Text="Agregar item" OnClick="btnAgregarItem_Click" />
                    </asp:PlaceHolder>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12 table-responsive-xl">
                        <asp:GridView ID="gvItems" AllowPaging="false" DataKeyNames="IndiceColeccion" OnPreRender="gvItems_PreRender" UseAccessibleHeader="true"
                            OnRowCommand="gvItems_RowCommand" runat="server" SkinID="GrillaBasicaFormal" CssClass="table-responsive-sm"
                            AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvItems_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Código - Producto / Descripcion" SortExpression="">
                                    <ItemTemplate>
                                        <asp:DropDownList CssClass="form-control select2" ID="ddlProducto" runat="server" Enabled="false"></asp:DropDownList>
                                        <asp:HiddenField ID="hdfIdProducto" Value='<%#Bind("ListaPrecioDetalle.Producto.IdProducto") %>' runat="server" />
                                        <asp:HiddenField ID="hdfIdListaPrecioDetalle" Value='' runat="server" />
                                        <asp:Label CssClass="col-form-label" ID="lblProducto" Visible="false" Enabled="false" Text='<%#Bind("DescripcionProducto") %>' runat="server"></asp:Label>
                                        <%--</ItemTemplate>
                            </asp:TemplateField>
                        <asp:TemplateField HeaderText="Detalle" SortExpression="">
                            <ItemTemplate>--%>
                                        <asp:Label CssClass="col-form-label" ID="lblProductoDescripcion" Visible="false" Text='<%#Bind("Descripcion")%>' runat="server"></asp:Label>
                                        <asp:TextBox CssClass="form-control" placeholder="Ingrese un detalle..." ID="txtDescripcion" Enabled="false" Text='<%#Bind("Descripcion") %>' runat="server"></asp:TextBox>
                                        <asp:HiddenField ID="hdfProductoDetalle" Value='<%#Bind("DescripcionProducto") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Costo" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-CssClass="text-right" SortExpression="Costo">
                                    <ItemTemplate>
                                        <asp:Label CssClass="col-form-label MonedagblSymbolo" ID="lblCosto" Visible="false" Enabled="false" Text='<%#Bind("Costo", "{0:C2}") %>' runat="server"></asp:Label>
                                        <asp:HiddenField ID="hdfCosto" Value='<%#Bind("Costo") %>' runat="server" />
                                        <%--  <%# Eval("Costo", "{0:C2}")%>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="% Marg." ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-CssClass="text-right" SortExpression="MargenPorcentaje">
                                    <ItemTemplate>
                                        <%# string.Concat( Eval("MargenImporte", "{0:N2}"), "%")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cantidad" ItemStyle-Wrap="false" SortExpression="">
                                    <ItemTemplate>
                                        <Evol:CurrencyTextBox CssClass="form-control" Prefix="" ID="txtCantidad" Enabled="false" class="form-control" runat="server" Text='<%#Bind("Cantidad") %>'></Evol:CurrencyTextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Precio Unitario" ItemStyle-Wrap="false" SortExpression="">
                                    <ItemTemplate>
                                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtPrecioUnitario" Enabled="false" runat="server" Text='<%#Bind("PrecioUnitarioSinIva", "{0:C2}") %>'></Evol:CurrencyTextBox>
                                        <asp:HiddenField ID="hdfPrecio" Value='<%#Bind("ListaPrecioDetalle.Precio", "{0:N2}") %>' runat="server" />
                                        <asp:HiddenField ID="hdfMargenPorcentaje" Value='<%#Bind("MargenImporte", "{0:N2}") %>' runat="server" />
                                        <asp:HiddenField ID="hdfMargenImporte" Value='<%#Bind("ListaPrecioDetalle.ListaPrecio.MargenImporte", "{0:N2}") %>' runat="server" />
                                        <asp:HiddenField ID="hdfFinanciacionPorcentaje" Value='<%#Bind("ListaPrecioDetalle.ListaPrecio.FinanciacionPorcentaje", "{0:N2}") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="% Desc." ItemStyle-Wrap="false" SortExpression="">
                                    <ItemTemplate>
                                        <Evol:CurrencyTextBox CssClass="form-control" Prefix="" ID="txtDescuentoPorcentual" Enabled="false" runat="server" Text='<%# Eval("DescuentoPorcentual")==null ? "0,00" : Eval("DescuentoPorcentual", "{0:N2}") %>'></Evol:CurrencyTextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" HeaderStyle-CssClass="text-right" ItemStyle-Wrap="false" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label CssClass="gvLabelMoneda" Style="visibility: hidden" ID="lblDescuentoImporte" runat="server" Text='<%#Bind("DescuentoImporte", "{0:N2}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Subtotal" HeaderStyle-CssClass="text-right" ItemStyle-Wrap="false" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label CssClass="gvLabelMoneda MonedagblSymbolo" ID="lblSubtotal" runat="server" Text='<%#Bind("SubTotal", "{0:C2}") %>'></asp:Label>
                                        <asp:HiddenField ID="hdfSubtotal" Value='<%#Bind("SubTotal")%>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Alícuota IVA" ItemStyle-Wrap="false" SortExpression="">
                                    <ItemTemplate>
                                        <asp:DropDownList CssClass="form-control select2" ID="ddlAlicuotaIVA" runat="server" Enabled="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label CssClass="gvLabelMoneda MonedagblSymbolo" Style="visibility: hidden" ID="lblImporteIva" runat="server" Text='<%#Bind("ImporteIVA", "{0:C2}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Subtotal c/IVA" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label CssClass="gvLabelMoneda MonedagblSymbolo" ID="lblSubtotalConIva" runat="server" Text='<%#Bind("SubTotalConIva", "{0:C2}") %>'></asp:Label>
                                        <asp:HiddenField ID="hdfSubtotalConIva" Value='<%#Bind("SubTotalConIva")%>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                            AlternateText="Elminiar" ToolTip="Eliminar" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <asp:PlaceHolder ID="phAplicarPorcentaje" Visible="false" runat="server">
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblAplicarPorcentajeDescuento" runat="server" Text="Aplicar % de descuento" />
                        <div class="col-sm-3">
                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtAplicarPorcentajeDescuento" Prefix="" runat="server"></Evol:CurrencyTextBox>
                        </div>
                    </div>
                </asp:PlaceHolder>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblTotalSinIva" runat="server" Text="Total sin iva" />
                    <div class="col-sm-3">

                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtTotalSinIva" Enabled="false" runat="server"></Evol:CurrencyTextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblTotalIva" runat="server" Text="Total iva" />
                    <div class="col-sm-3">

                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtTotalIva" Enabled="false" runat="server"></Evol:CurrencyTextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-9 col-form-label  text-right" ID="lblTotal" runat="server" Text="Total con iva" />
                    <div class="col-sm-3">

                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtTotalConIva" Enabled="false" runat="server"></Evol:CurrencyTextBox>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaEntrega" runat="server" Text="Fecha de Entrega"></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaEntrega" Enabled="true" runat="server"></asp:TextBox>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPlazoEntrega" runat="server" Text="Plazo de Entrega"></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtPlazoEntrega" runat="server"></asp:TextBox>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblGarantia" runat="server" Text="Garantia"></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtGrantia" runat="server"></asp:TextBox>
    </div>
</div>
<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFormaPago" runat="server" Text="Forma de Pago"></asp:Label>
    <div class="col-sm-7">
        <asp:TextBox CssClass="form-control" ID="txtFormaPago" runat="server" TextMode="MultiLine" />
    </div>
</div>
<asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                    OnClick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                <asp:Button CssClass="botonesEvol" ID="btnCopiar" runat="server" Text="Copiar Presupuesto" OnClick="btnCopiar_Click" Visible="false" CausesValidation="false" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
