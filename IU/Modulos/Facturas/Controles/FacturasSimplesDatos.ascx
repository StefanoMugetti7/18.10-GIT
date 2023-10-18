<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FacturasSimplesDatos.ascx.cs" Inherits="IU.Modulos.Facturas.Controles.FacturasSimplesDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpEnviarMail.ascx" TagName="popUpEnviarMail" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/AsientosDatosMostrar.ascx" TagName="AsientoMostrar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" TagName="popUpGrillaGenerica" TagPrefix="auge" %>
<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />

<script lang="javascript" type="text/javascript">

    var procesandoCodigoBarra;
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControlApellidoSelect2);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitProductoSelect2);
        InitProductoSelect2();
        SetTabIndexInput();
        InitControlApellidoSelect2();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitClienteSelect2);
        InitClienteSelect2();
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

    function InitControlApellidoSelect2() {
        $("input[type='hidden'][id$='hdfIdAfiliado']").bind("change", function () {
            alert("Value of hidden field after updating: "
                + $(this).val());
        });
        //CargarTipoPuntoVenta();
    }

    function SetTabIndexInput() {
        $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }

    function round(value, exp) {
        if (typeof exp === 'undefined' || +exp === 0)
            return Math.round(value);

        value = +value;
        exp = +exp;

        if (isNaN(value) || !(typeof exp === 'number' && exp % 1 === 0))
            return NaN;

        // Shift
        value = value.toString().split('e');
        value = Math.round(+(value[0] + 'e' + (value[1] ? (+value[1] + exp) : exp)));

        // Shift back
        value = value.toString().split('e');
        return +(value[0] + 'e' + (value[1] ? (+value[1] - exp) : -exp));
    }

    function ValidarShowConfirm(ctrl, msg) {
        if (Page_ClientValidate("Aceptar")) {
            if ($("input[id$='chkGenerarRemito']").is(':checked')) {
                showConfirm(ctrl, msg);
            } else {
                __doPostBack(ctrl.name, '');
            }
        }
    }
    function ModificaPrecio(ctrl) {
        $('#' + ctrl).val('True');
    }

    var TotalSinIVA = 0.00;
    var totalIVA = 0.00;
    var totalConIVA = 0.00;
    var totalPercepciones = 0.00;

    function CalcularItem() {
        TotalSinIVA = 0.00;
        totalIVA = 0.00;
        totalConIVA = 0.00;
        var importeIVA = 0.00;
        var subTotalConIVA = 0.00;
        var subTotalItem = 0.00;
        var cantidadCuotas = 1;
        var decimales = 2;
        $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {

            var alicuotaIVA = $("input:hidden[id*='hdfAlicuotaIva']").val();
            var importe = $(this).find("input:text[id*='txtPrecioUnitario']").maskMoney('unmasked')[0]; //$("td:eq(4)", this).html();
            var modifica = $(this).find("input[id*='hdfModificaPrecio']").val();
            var precio = $(this).find("input[id*='hdfPrecio']").val();
            var margenImporte = $(this).find("input[id*='hdfMargenImporte']").val();
            var margenPorcentaje = $(this).find("input[id*='hdfMargenPorcentaje']").val();
            var cantidad = $(this).find("input:text[id*='txtCantidad']").maskMoney('unmasked')[0];
            var financiacionPorcentaje = $(this).find("input[id*='hdfFinanciacionPorcentaje']").val();

            var idProducto = $(this).find('[id*="ddlProducto"] option:selected').val();
            var txtPrecioUnitario = $(this).find("input:text[id*='txtPrecioUnitario']");

            if (modifica == 'true') {
                txtPrecioUnitario.prop("disabled", false);
            }

            if (alicuotaIVA == undefined)
                alicuotaIVA = 0.00;
            else
                alicuotaIVA = alicuotaIVA.replace('.', '').replace(',', '.');

            if (idProducto == undefined)
                idProducto = 0;
            else {
                if (idProducto.includes('-')) {
                    idProducto = idProducto.split('-')[0];
                }
            }

            precio = precio.replace('.', '').replace(',', '.');
            margenImporte = margenImporte.replace('.', '').replace(',', '.');

            if (importe && cantidad && precio && idProducto && idProducto > 0) {

                //descuentoImporte = descuentoImporte.replace('.', '').replace(',', '.');

                if (modifica == 'False') {
                    importe = (parseFloat(precio) * (1 + parseFloat(margenPorcentaje) / 100) + parseFloat(margenImporte)) * (1 + parseFloat(financiacionPorcentaje) * (cantidadCuotas == 1 ? 0 : cantidadCuotas) / 100) + parseFloat(margenImporte) * cantidadCuotas;
                }

                importe = round(importe, decimales); // Number(Math.round(importe + 'e2') + 'e-2');

                $(this).find("input[id*='hdfPreUnitario']").val(importe);
                $(this).find("input:text[id*='txtPrecioUnitario']").val(accounting.formatMoney(importe, gblSimbolo, decimales, "."));
                subTotalItem = parseFloat(importe) * parseFloat(cantidad);

                importeIVA = parseFloat(subTotalItem) * parseFloat(alicuotaIVA) / 100;
                importeIVA = round(importeIVA, 2);
                subTotalItem = round(subTotalItem, 2);
                subTotalConIVA = parseFloat(subTotalItem) + parseFloat(importeIVA);
                TotalSinIVA += parseFloat(subTotalItem);
                totalIVA += parseFloat(importeIVA);

                $(this).find('span[id*="lblSubtotal"]').text(accounting.formatMoney(subTotalItem, gblSimbolo, gblCantidadDecimales, "."));
                $(this).find('input:hidden[id*="hdfSubtotal"]').val(subTotalItem);
                $(this).find('input:hidden[id*="hdfImporteIva"]').val(importeIVA);
                $(this).find('span[id*="lblSubtotalConIva"]').text(accounting.formatMoney(subTotalConIVA, gblSimbolo, gblCantidadDecimales, "."));
                $(this).find('input:hidden[id*="hdfSubtotalConIva"]').val(subTotalConIVA);
            }
        });
        totalConIVA = parseFloat(TotalSinIVA) + parseFloat(totalIVA);
        MostrarTotales();
    }
    function MostrarTotales() {
        $("input[type=text][id$='txtTotalConIva']").val(accounting.formatMoney(TotalSinIVA + totalIVA + totalPercepciones, gblSimbolo, gblCantidadDecimales, "."));
    }
    function ValidarFacturaCargo() {
        var importePagar = $("input[type=text][id$='txtPrecioUnitario']").maskMoney('unmasked')[0];
        var controlImportePagar = $("input[id$='hdfPreUnitario']").val().replace('.', '').replace(',', '.');
        if (parseFloat(importePagar) > parseFloat(controlImportePagar)) {
            importePagar = controlImportePagar;
            $("input[type=text][id$='txtPrecioUnitario']").val(accounting.formatMoney(importePagar, gblSimbolo, gblCantidadDecimales, "."));
        }
    }
    /***************************************************
     Productos ajax
     ****************************************************/
    function InitProductoSelect2() {
        var control = $("select[name$='ddlProductoCodigo']");
        var idAfiliado = 0;
        var idFilialPredeterminada = $("input[id*='hdfIdFilialPredeterminada']").val();
        var idUsuarioEvento = $("input[id*='hdfIdUsuarioEvento']").val();
        var hdfProductoDetalle = $("input:hidden[id*='hdfProductoDetalleCodigo']");
        var cantidadCuotas = 1; //$('select[id$="ddlCantidadCuotas"] option:selected').val();
        var hdfPreUnitario = $("input:hidden[id*='hdfPreUnitarioCodigo']");
        var hdfModificaPrecio = $("input:hidden[id*='hdfModificaPrecioCodigo']");
        var hdfPrecio = $("input:hidden[id*='hdfPrecioCodigo']");
        var idMoneda = 1;
        var hdfIdProducto = $("input:hidden[id*='hdfIdProductoCodigo']");
        var hdfStockeable = $("input:hidden[id*='hdfStockeableCodigo']");
        var idListaPrecio = $('select[id$="ddlListaPrecio"]').val();
        var txtPrecioUnitario = $(this).find("input:text[id*='txtPrecioUnitarioCodigo']");
        var hdfAlicuotaIva = $("input:hidden[id*='hdfAlicuotaIva']");
        var hdfIdIva = $("input:hidden[id*='hdfIdIva']");
        control.select2({
            placeholder: 'Ingrese el codigo o producto',
            selectOnClose: true,
            theme: 'bootstrap4',
            width: '100%',
            minimumInputLength: 1,
            language: 'es',
            //tags: true,
            templateResult: formatProductoOption,
            allowClear: true,
            ajax: {
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                url: '<%=ResolveClientUrl("~")%>/Modulos/Facturas/FacturasWS.asmx/FacturasSeleccionarAjaxComboProductos', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                delay: 500,
                data: function (params) {
                    return JSON.stringify({
                        value: control.val(), // search term");
                        filtro: params.term, // search term");
                        cantidadCuotas: cantidadCuotas,
                        idAfiliado: idAfiliado,
                        idFilialPredeterminada: idFilialPredeterminada,
                        idMoneda: idMoneda,
                        idUsuarioEvento: idUsuarioEvento,
                        idListaPrecio: idListaPrecio,
                    });
                },
                beforeSend: function (xhr, opts) {
                    if (idMoneda == "") {
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
                    return {
                        results: $.map(data.d, function (item) {
                            return {
                                text: item.ProductoDescripcionCombo,
                                id: item.ProductoIdProducto,
                                productoDescripcion: item.ProductoDescripcion,
                                precio: item.Precio,
                                precioUnitarioSinIva: item.PrecioUnitarioSinIva,
                                precioEditable: item.PrecioEditable,
                                stockeable: item.Stockeable,
                                idIva: item.idIva,
                                alicuotaIva: item.alicuotaIVA,
                            }
                        })
                    };
                    cache: true
                }
            },
        });
        control.on('select2:select', function (e) {
            hdfProductoDetalle.val(e.params.data.text);
            hdfIdProducto.val(e.params.data.id);
            txtPrecioUnitario.val(accounting.formatMoney(e.params.data.precioUnitarioSinIva, gblSimbolo, 2, gblSeparadorMil));
            hdfPreUnitario.val(e.params.data.precioUnitarioSinIva);
            hdfModificaPrecio.val(e.params.data.precioEditable);
            hdfStockeable.val(e.params.data.stockeable);
            hdfPrecio.val(e.params.data.precio);
            hdfIdIva.val(e.params.data.idIva);
            hdfAlicuotaIva.val(e.params.data.alicuotaIva);
            //CalcularPrecio();
            CargarProducto();
        });
        control.on('select2:unselect', function (e) {
            hdfProductoDetalle.val('');
            hdfIdProducto.val('');
            txtPrecioUnitario.val(accounting.formatMoney(0, gblSimbolo, 2, gblSeparadorMil));
            hdfPreUnitario.val('');
            hdfModificaPrecio.val('');
            hdfStockeable.val('');
            hdfPrecio.val('');
            hdfIdIva.val('');
            hdfAlicuotaIva.val('');
        });
    }
    var firstEmptySelect = true; // Indicate header was create
    function formatProductoOption(item) {
        if (!item.id) {
            // trigger when remote query
            firstEmptySelect = true; // reset
            return item.text;
        }
        var $container; // This is custom templete container.

        if (firstEmptySelect) {

            firstEmptySelect = false;

            $container = $(
                '<div class="row" width="">' +
                '<div class="col-sm-8"><b>Codigo - Descripcion</b></div>' +
                '<div class="col-sm-4"><b>Precio</b></div>' +
                '</div>' +
                '<div class="row">' +
                '<div class="col-sm-8">' + item.productoDescripcion + '</div>' +
                '<div class="col-sm-4">' + "$" + item.precio + '</div>' +
                '</div>'
            );
        }
        else {
            $container = $('<div class="row">' +
                '<div class="col-sm-8">' + item.productoDescripcion + '</div>' +
                '<div class="col-sm-4">' + "$" + item.precio + '</div>' +
                '</div>');
        }
        return $container;
    }
    function CargarProducto() {
        procesandoCodigoBarra = 1;
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ControlFocus);
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CargarProductosCallBack);
        __doPostBack("<%=button.UniqueID %>", "");
        // CalcularItem();
    }
    function ControlFocus() {
        if (procesandoCodigoBarra == 1) {
            control = $("select[name$='ddlProductoCodigo']")
            control.focus();

            control.select2('open');
            procesandoCodigoBarra = 0;
        }
    }
    /***************************************************
  Afiliados ajax
  ****************************************************/
    function InitClienteSelect2() {
        var control = $("select[name$='ddlNumeroSocio']");
        control.select2({
            placeholder: 'Ingrese el codigo o Razón Social',
            selectOnClose: true,
            theme: 'bootstrap4',
            minimumInputLength: 1,
            language: 'es',
            allowClear: true,
            ajax: {
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                url: '<%=ResolveClientUrl("~")%>/Modulos/Afiliados/AfiliadosWS.asmx/AfiliadosClienteCombo', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                delay: 500,
                data: function (params) {
                    return JSON.stringify({
                        value: control.val(), // search term");
                        filtro: params.term // search term");
                    });
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
                    return {
                        results: $.map(data.d, function (item) {
                            return {
                                text: item.DescripcionCombo,
                                id: item.IdAfiliado,
                                descripcionAfiliado: item.DescripcionAfiliado,
                            }
                        })
                    };
                    cache: true
                }
            }
        });

        control.on('select2:select', function (e) {
            $("input[id*='hdfIdAfiliado']").val(e.params.data.id);
            $("input[id*='hdfRazonSocial']").val(e.params.data.descripcionAfiliado);
            __doPostBack("<%=buttonAfi.UniqueID %>", "");
        });
        control.on('select2:unselect', function (e) {
            $("select[id$='ddlNumeroSocio']").val(null).trigger("change");
            $("input[id*='hdfIdAfiliado']").val('');
            $("input[id*='hdfRazonSocial']").val('');
            control.val(null).trigger('change');
            __doPostBack("<%=buttonAfi.UniqueID %>", "");
        });
    }
</script>
<asp:HiddenField ID="hdfControlFocus" runat="server" />
<asp:HiddenField ID="hdfIdUsuarioEvento" runat="server" />
<asp:HiddenField ID="hdfIdFilialPredeterminada" runat="server" />
<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="card">
            <div class="card-header">
                Datos del Comprobante
            </div>
            <div class="card-body">
                <div class="form-group row">
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroSocio" runat="server" Text="Cliente" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlNumeroSocio" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvNumeroSocio" ValidationGroup="Aceptar" ControlToValidate="ddlNumeroSocio" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hdfIdAfiliado" runat="server" />
                        <asp:HiddenField ID="hdfRazonSocial" runat="server" />
                        <asp:Button CssClass="botonesEvol" ID="buttonAfi" OnClick="buttonAfi_Click" runat="server" Style="display: none;" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialDescripcion" runat="server" Text="Emisión"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlFilialPuntoVenta" runat="server" OnSelectedIndexChanged="ddlFilialPuntoVenta_SelectedIndexChanged" AutoPostBack="true" />
                        <asp:RequiredFieldValidator ID="rfvFilialPuntoVenta" ValidationGroup="Aceptar" ControlToValidate="ddlFilialPuntoVenta" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoFactura" runat="server" Text="Tipo Cbte." />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoFactura" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoFactura_SelectedIndexChanged" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvTipoFactura" ValidationGroup="Aceptar" ControlToValidate="ddlTipoFactura" CssClass="ValidadorBootstrap" EnableViewState="true" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaFactura" runat="server" Text="Fecha Cbte."></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaFactura" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="ValidadorBootstrap" ID="rfvFechaFactura" ControlToValidate="txtFechaFactura" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblListaPrecio" runat="server" Text="Lista de Precio" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlListaPrecio" runat="server" />
                    </div>

                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upItems" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="card">
            <div class="card-header">
                Detalle del Comprobante
            </div>
            <div class="card-body">
                <div runat="server" id="dvCargaCodigoBarras">
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProducto" runat="server" Text="Producto" />
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2 form-group" ID="ddlProductoCodigo" Enabled="true" runat="server" />
                            <asp:HiddenField ID="hdfIdProductoCodigo" runat="server" />
                            <asp:HiddenField ID="hdfProductoDetalleCodigo" runat="server" />
                            <asp:HiddenField ID="hdfPreUnitarioCodigo" runat="server" />
                            <asp:HiddenField ID="hdfModificaPrecioCodigo" runat="server" />
                            <asp:HiddenField ID="hdfPrecioCodigo" runat="server" />
                            <asp:HiddenField ID="hdfMargenPorcentajeCodigo" runat="server" />
                            <asp:HiddenField ID="hdfMargenImporteCodigo" runat="server" />
                            <asp:HiddenField ID="hdfStockeableCodigo" runat="server" />
                            <asp:HiddenField ID="hdfFinanciacionPorcentajeCodigo" runat="server" />
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadCodigo" runat="server" Text="Cantidad"></asp:Label>
                        <div class="col-sm-1">
                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtCantidadCodigo" Text="1" Prefix="" NumberOfDecimals="0" runat="server" />
                        </div>
                        <asp:Button CssClass="botonesEvol" ID="button" OnClick="button_Click" runat="server" Style="display: none;" />
                    </div>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-12 col-form-label" ID="lblDescripcionProductoCodigo" runat="server" Text="Ultimo Producto Cargado:"></asp:Label>
                    </div>
                </div>
                <div class="table-responsive">
                    <asp:GridView ID="gvItems" DataKeyNames="IndiceColeccion" AllowPaging="false" AllowSorting="false"
                        OnRowCommand="gvItems_RowCommand" runat="server" SkinID="GrillaResponsive"
                        AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvItems_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Código - Producto / Descripcion" SortExpression="">
                                <ItemTemplate>
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlProducto" runat="server" Style="overflow-x: hidden;" Enabled="false"></asp:DropDownList>
                                    <asp:HiddenField ID="hdfIdProducto" Value='<%#Bind("ListaPrecioDetalle.Producto.IdProducto") %>' runat="server" />
                                    <asp:HiddenField ID="hdfStockeable" Value='<%#Bind("ListaPrecioDetalle.Producto.Familia.Stockeable") %>' runat="server" />
                                    <asp:HiddenField ID="hdfProductoDetalle" Value='<%#Bind("DescripcionProducto") %>' runat="server" />
                                    <asp:HiddenField ID="hdfAlicuotaIva" Value='<%#Bind("IVA.Alicuota") %>' runat="server" />
                                    <asp:HiddenField ID="hdfIdIva" Value='<%#Bind("IVA.IdIVA") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cantidad" ItemStyle-Wrap="false" SortExpression="">
                                <ItemTemplate>
                                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtCantidad" Prefix="" NumberOfDecimals="2" Enabled="false" runat="server" Text='<%#Bind("Cantidad", "{0:N2}") %>'></Evol:CurrencyTextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Prec. Unitario" ItemStyle-Wrap="false" SortExpression="">
                                <ItemTemplate>
                                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtPrecioUnitario" AllowNegative="true" Enabled="true" Text='<%#Bind("PrecioUnitarioSinIva", "{0:C4}") %>' runat="server"></Evol:CurrencyTextBox>
                                    <asp:HiddenField ID="hdfPreUnitario" Value='<%#Bind("PrecioUnitarioSinIva") %>' runat="server" />
                                    <asp:HiddenField ID="hdfModificaPrecio" Value='<%#Bind("ModificaPrecio") %>' runat="server" />
                                    <asp:HiddenField ID="hdfPrecio" Value='<%#Bind("ListaPrecioDetalle.Precio") %>' runat="server" />
                                    <asp:HiddenField ID="hdfMargenPorcentaje" Value='<%#Bind("ListaPrecioDetalle.ListaPrecio.MargenPorcentaje") %>' runat="server" />
                                    <asp:HiddenField ID="hdfMargenImporte" Value='<%#Bind("ListaPrecioDetalle.ListaPrecio.MargenImporte") %>' runat="server" />
                                    <asp:HiddenField ID="hdfFinanciacionPorcentaje" Value='<%#Bind("ListaPrecioDetalle.ListaPrecio.FinanciacionPorcentaje") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subtotal" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" SortExpression="">
                                <ItemTemplate>
                                    <asp:Label CssClass="col-form-label" ID="lblSubtotal" runat="server" Text='<%#Bind("SubTotal", "{0:C2}") %>'></asp:Label>
                                    <asp:HiddenField ID="hdfSubtotal" Value='<%#(Eval("SubTotal") == null ? string.Empty : Eval("SubTotal")).ToString().Replace(",",".")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subtotal c/IVA" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" SortExpression="">
                                <ItemTemplate>
                                    <asp:Label CssClass="col-form-label" ID="lblSubtotalConIva" runat="server" Text='<%#Bind("SubTotalConIva", "{0:C2}") %>'></asp:Label>
                                    <asp:HiddenField ID="hdfSubtotalConIva" Value='<%# (Eval("SubTotalConIva") == null ? string.Empty : Eval("SubTotalConIva")).ToString().Replace(",",".")%>' runat="server" />
                                    <asp:HiddenField ID="hdfImporteIva" Value='<%#Bind("ImporteIVA")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Eliminar" SortExpression="">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                        AlternateText="Elminiar" ToolTip="Eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblTotal" runat="server" Text="Total" />
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtTotalConIva" Enabled="false" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upCobro" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="card">
            <div class="card-header">
                Datos del Cobro
            </div>
            <div class="card-body">
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBancoCuenta" runat="server" Text="Cuenta de Cobro" />
                    <div class="col-sm-3">
                        <Evol:GroupedDropDownList runat="server" ID="ddlBancoCuentaAgrupado" CssClass="form-control select2" OnSelectedIndexChanged="ddlBancoCuentaAgrupado_SelectedIndexChanged" AutoPostBack="true"></Evol:GroupedDropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="Aceptar" ControlToValidate="ddlBancoCuentaAgrupado" CssClass="ValidadorBootstrap" EnableViewState="true" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblLoteEnvio" runat="server" Text="Nro. Lote" Visible="false" />
                    <div class="col-sm-3">
                        <AUGE:CurrencyTextBox CssClass="form-control" ID="txtLoteEnvio" runat="server" Prefix="" NumberOfDecimals="0" Visible="false"></AUGE:CurrencyTextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCupon" runat="server" Text="Nro. Cupon" Visible="false" />
                    <div class="col-sm-3">
                        <AUGE:CurrencyTextBox CssClass="form-control" ID="txtCupon" runat="server" Prefix="" NumberOfDecimals="0" Visible="false"></AUGE:CurrencyTextBox>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <AUGE:AsientoMostrar ID="ctrAsientoMostrar" runat="server" />
        <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
        <AUGE:popUpEnviarMail ID="popUpMail" runat="server" />
        <AUGE:popUpGrillaGenerica ID="ctrPopUpGrilla" runat="server" />
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                    OnClick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                <asp:ImageButton ImageUrl="~/Imagenes/sendmail.png" runat="server" ID="btnEnviarMail" Visible="false"
                    OnClick="btnEnviarMail_Click" AlternateText="Enviar Comprobante" ToolTip="Enviar Comprobante" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" CausesValidation="false" />
                <asp:Button CssClass="botonesEvol" ID="btnAgregarNuevo" Visible="false" runat="server" Text="Nuevo" OnClick="btnAgregarNuevo_Click" CausesValidation="false" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
