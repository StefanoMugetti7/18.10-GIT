<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FacturasDatos.ascx.cs" Inherits="IU.Modulos.Facturas.Controles.FacturasDatos" %>

<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/ClientesDatosCabeceraAjax.ascx" TagName="BuscarClienteAjax" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Facturas/Controles/ProductosBuscarPopUp.ascx" TagName="popUpBuscarProducto" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Facturas/Controles/RemitosBuscarPopUp.ascx" TagName="popUpBuscarRemito" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Facturas/Controles/PresupuestosBuscarPopUp.ascx" TagName="popUpBuscarPresupuesto" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Facturas/Controles/NotasPedidosBuscarPopUp.ascx" TagName="popUpBuscarNotaPedido" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpEnviarMail.ascx" TagName="popUpEnviarMail" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/AsientosDatosMostrar.ascx" TagName="AsientoMostrar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" TagName="popUpGrillaGenerica" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadoModificarDatosDomicilioPopUp.ascx" TagName="popUpDomicilio" TagPrefix="auge" %>
<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />

<script lang="javascript" type="text/javascript">

    var procesandoCodigoBarra;
    $(document).ready(function () {
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CopmletarCerosComprobantes);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControlApellidoSelect2);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridDetalle);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CalcularPercepcion);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(MostrarRemitosAbrir);
        MostrarRemitosAbrir();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitProductoSelect2);
        InitProductoSelect2();
        SetTabIndexInput();
        CopmletarCerosComprobantes();
        InitControlApellidoSelect2();
        intiGridDetalle();
        //HabilitarPeriodosFechas();
        //$("input[type=text][id$='']").focus();
        //$(":input").each(function (i) { $(this).attr('tabindex', i + 1); });
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

    function CopmletarCerosComprobantes() {
        $("input[type=text][id$='txtNumeroFactura']").blur(function () { $(this).addLeadingZeros(8); });
        $("input[type=text][id$='txtPrefijoNumeroRecibo']").blur(function () { $(this).addLeadingZeros(4); });
        $("input[type=text][id$='txtNumeroRecibo']").blur(function () { $(this).addLeadingZeros(8); });
        $("input[type=text][id$='txtNumeroRemito']").blur(function () { $(this).addLeadingZeros(8); });
    }

    function SetTabIndexInput() {
        $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }


    //    Number.prototype.round = function (decimals) {
    //        return Number((Math.round(this + "e" + decimals) + "e-" + decimals));
    //    }

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

    function AgregarDetalle(ctrl) {
        var row = $(ctrl).parent().parent();
        $(row).find('input:text[id*="txtDescripcion"]').show();
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
        var descuento = 0.00;
        var cantidadCuotas = $('select[id$="ddlCantidadCuotas"] option:selected').val();
        var decimales = $('select[id$="ddlCantidadDecimales"] option:selected').val();
        var hdfNoCalculaImporteDescuento = $("input:hidden[id$='hdfNoCalculaImporteDescuento']");

        $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {

            //var incluir = $("td:eq(6) :checkbox", this).is(":checked");
            //             var incluir = $(this).find('input:checkbox[id$="chkIncluir"]').is(":checked");

            var importe = $(this).find("input:text[id*='txtPrecioUnitario']").maskMoney('unmasked')[0]; //$("td:eq(4)", this).html();
            var modifica = $(this).find("input[id*='hdfModificaPrecio']").val();
            var precio = $(this).find("input[id*='hdfPrecio']").val();
            var descuentoImporte = $(this).find("input:hidden[id*='hdfDescuentoImporte']").val();
            var margenPorcentaje = $(this).find("input[id*='hdfMargenPorcentaje']").val();
            var margenImporte = $(this).find("input[id*='hdfMargenImporte']").val();
            var financiacionPorcentaje = $(this).find("input[id*='hdfFinanciacionPorcentaje']").val();
            var cantidad = $(this).find("input:text[id*='txtCantidad']").maskMoney('unmasked')[0];
            //var alicuotaIVA = $(this).find('[id*="ddlAlicuotaIVA"] option:selected').val();
            var data = $(this).find('[id*="ddlAlicuotaIVA"] option:selected').val();
            var alicuotaIVA = data.split('|')[1];
            var idProducto = $(this).find('[id*="ddlProducto"] option:selected').val();
            var txtPrecioUnitario = $(this).find("input:text[id*='txtPrecioUnitario']");
            var idProductoLabel = $(this).find('span[id*="lblProducto"]').text();

            //if (descuentoImporte.includes('.')) {
            //    descuentoImporte = descuentoImporte.replace(',', '').replace('.', ',');
            //}

            if (modifica == 'true') {
                txtPrecioUnitario.prop("disabled", false);
            }

            if (!idProducto) {
                idProducto = idProductoLabel;
            }

            var descuentoPorcentaje = $(this).find('[id*="txtDescuentoPorcentual"]').maskMoney('unmasked')[0];
            if (descuentoPorcentaje > 100) {
                descuentoPorcentaje = 100;
                $(this).find('[id*="txtDescuentoPorcentual"]').val(accounting.formatMoney(descuentoPorcentaje, "", gblCantidadDecimales, "."));
            }
            if (idProducto.includes('-')) {
                idProducto = idProducto.split('-')[0];
            }

            precio = precio.replace('.', '').replace(',', '.');
            margenPorcentaje = margenPorcentaje.replace('.', '').replace(',', '.');
            margenImporte = margenImporte.replace('.', '').replace(',', '.');
            financiacionPorcentaje = financiacionPorcentaje.replace('.', '').replace(',', '.');


            if (importe && cantidad && precio && idProducto && idProducto > 0) {

                //descuentoImporte = descuentoImporte.replace('.', '').replace(',', '.');
                //VER DE AGREGAR UNA VARIABLE PARA IMPORTE FIJO EN CUOTAS!!!

                if (modifica == 'False') {
                    importe = (parseFloat(precio) * (1 + parseFloat(margenPorcentaje) / 100) + parseFloat(margenImporte)) * (1 + parseFloat(financiacionPorcentaje) * (cantidadCuotas == 1 ? 0 : cantidadCuotas) / 100) + parseFloat(margenImporte) * cantidadCuotas;
                }

                importe = round(importe, decimales); // Number(Math.round(importe + 'e2') + 'e-2');

                if (hdfNoCalculaImporteDescuento.val() == "1") {
                    descuento = descuentoImporte;
                    var txtdescuentoPorcentaje = $(this).find('[id*="txtDescuentoPorcentual"]');
                    txtdescuentoPorcentaje.prop("disabled", true);
                }
                else {
                    descuento = parseFloat(parseFloat(importe) * parseFloat(cantidad) * parseFloat(descuentoPorcentaje) / 100);
                    descuento = round(descuento, decimales); // Number(Math.round(descuento + 'e2') + 'e-2');
                }

                $(this).find("input[id*='hdfPreUnitario']").val(importe);
                $(this).find("input:text[id*='txtPrecioUnitario']").val(accounting.formatMoney(importe, gblSimbolo, decimales, "."));
                $(this).find('span[id*="lblDescuentoImporte"]').text(accounting.formatMoney(descuento, gblSimbolo, gblCantidadDecimales, "."));
                $(this).find('input:hidden[id*="hdfDescuentoImporte"]').val(descuento);//(accounting.formatMoney(descuento, "", 2, ""));
                subTotalItem = parseFloat(importe) * parseFloat(cantidad) - parseFloat(descuento);
                //subTotalItem = round(subTotalItem, 2);

                alicuotaIVA = alicuotaIVA.replace('.', '').replace(',', '.');
                if (alicuotaIVA == "" || isNaN(alicuotaIVA)) {
                    alicuotaIVA = 0.00;
                    importeIVA = 0.00;
                } else {
                    importeIVA = parseFloat(subTotalItem) * parseFloat(alicuotaIVA) / 100;
                }
                importeIVA = round(importeIVA, 2);
                subTotalItem = round(subTotalItem, 2);
                subTotalConIVA = parseFloat(subTotalItem) + parseFloat(importeIVA);
                TotalSinIVA += parseFloat(subTotalItem);
                totalIVA += parseFloat(importeIVA);

                $(this).find('span[id*="lblSubtotal"]').text(accounting.formatMoney(subTotalItem, gblSimbolo, gblCantidadDecimales, "."));
                $(this).find('input:hidden[id*="hdfSubtotal"]').val(subTotalItem);
                $(this).find('span[id*="lblImporteIva"]').text(accounting.formatMoney(importeIVA, gblSimbolo, gblCantidadDecimales, "."));
                $(this).find('input:hidden[id*="hdfImporteIva"]').val(importeIVA);
                $(this).find('span[id*="lblSubtotalConIva"]').text(accounting.formatMoney(subTotalConIVA, gblSimbolo, gblCantidadDecimales, "."));
                $(this).find('input:hidden[id*="hdfSubtotalConIva"]').val(subTotalConIVA);
            }
        });
        totalConIVA = parseFloat(TotalSinIVA) + parseFloat(totalIVA);
        MostrarTotales();
    }

    function CalcularPercepcion() {
        totalPercepciones = 0.00;
        var modulo = $("input:hidden[id$='hdfModulo']");
        $('#<%=gvPercepciones.ClientID%> tr').not(':first').not(':last').each(function () {

            if (modulo.val() == "turismo") {
                var importe = $(this).find('[id*="txtImportePercepcion"]').maskMoney('unmasked')[0];

                totalPercepciones += parseFloat(importe);

            }
            else {
                var porcentaje = $(this).find("input:text[id*='txtPorcentajePercepcion']").maskMoney('unmasked')[0];
                if (porcentaje) {
                    var importe = round(TotalSinIVA * porcentaje / 100, 2);
                    $(this).find('[id*="txtImportePercepcion"]').val(accounting.formatMoney(importe, "", gblCantidadDecimales, "."));
                    totalPercepciones += parseFloat(importe);
                }
            }

        });
        $("#<%=gvPercepciones.ClientID %> [id$=lblImportePercepciones]").text(accounting.formatMoney(totalPercepciones, gblSimbolo, gblCantidadDecimales, "."));
        MostrarTotales();
    }

    function MostrarTotales() {

        $("input[type=text][id$='txtTotalSinIva']").val(accounting.formatMoney(TotalSinIVA, gblSimbolo, 2, "."));
        $("input[type=text][id$='txtTotalIva']").val(accounting.formatMoney(totalIVA, gblSimbolo, 2, "."));
        $("input[type=text][id$='txtTotalConIva']").val(accounting.formatMoney(TotalSinIVA + totalIVA + totalPercepciones, gblSimbolo, gblCantidadDecimales, "."));
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
                //$(this).find('[id*="txtDescuentoPorcentual"]').val(accounting.formatNumber(aplicarPorcentajeDescuento, 2, gblSeparadorMil));
                //if (txtDescuentoPorcentaje == '0,00') {
                //    $(this).find('[id*="txtDescuentoPorcentual"]').val(accounting.formatNumber(aplicarPorcentajeDescuento, 2, gblSeparadorMil));
                //    //txtDescuentoPorcentaje.val(accounting.formatNumber(aplicarPorcentajeDescuento, 2, gblSeparadorMil));
                //}
            });
            CalcularItem();
        }
    }

    function MostrarRemitos() {

        var hdfRemitosMostrar = $("input:hidden[id$='hdfRemitosMostrar']");
        if (hdfRemitosMostrar.val() == "1")
            hdfRemitosMostrar.val("0");
        else
            hdfRemitosMostrar.val("1");
    }

    function MostrarRemitosAbrir() {

        var hdfRemitosMostrar = $("input:hidden[id$='hdfRemitosMostrar']");
        if (hdfRemitosMostrar.val() == "1") {

            $("[id$='collapseremitos']").removeClass("hide");
            $("[id$='collapseremitos']").addClass("show");
        }
        else
            $("[id$='collapseremitos']").addClass('hide');

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
        var idMoneda = $('select[id$="ddlMonedas"] option:selected').val();
        var hdfIdProducto = $("input:hidden[id*='hdfIdProductoCodigo']");
        var hdfStockeable = $("input:hidden[id*='hdfStockeableCodigo']");
        var idListaPrecio = $('select[id$="ddlListaPrecio"]').val();
        var txtPrecioUnitario = $(this).find("input:text[id*='txtPrecioUnitarioCodigo']");
        control.select2({
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
                        value: control.val(), // search term");
                        filtro: params.term, // search term");
                        cantidadCuotas: cantidadCuotas,
                        idAfiliado: idAfiliado,
                        idFilialPredeterminada: idFilialPredeterminada,
                        idMoneda: idMoneda,
                        idUsuarioEvento: idUsuarioEvento,
                        idListaPrecio: idListaPrecio,
                    });
                    //var Productos = ObtenerProductosSeleccionadas();
                    //console.log(" array " + Productos);
                    //return "{filtro:" + JSON.stringify(params.term) + ", ListaProductos:" + JSON.stringify(Productos) + "}";
                },
                beforeSend: function (xhr, opts) {
                    if (idMoneda == "") {
                        // MostrarMensaje('Debe Ingresar una Moneda para poder continuar', 'red');
                        //xhr.abort();
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
                                precioEditable: item.PrecioEditable,
                                stockeable: item.Stockeable,
                                //IdListaPrecioDetalle: item.IdListaPrecioDetalle,
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
            //CalcularPrecio();
            CargarProducto();
            //txtDescripcion.focus();
        });
        control.on('select2:unselect', function (e) {
            hdfProductoDetalle.val('');
            hdfIdProducto.val('');
            txtPrecioUnitario.val(accounting.formatMoney(0, gblSimbolo, 2, gblSeparadorMil));
            hdfPreUnitario.val('');
            hdfModificaPrecio.val('');
            hdfStockeable.val('');
            hdfPrecio.val('');
        });
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
    /******************************************************
       Grilla Detalle
   *******************************************************/
    function intiGridDetalle() {
        var rowindex = 0;
        var cantidadCuotas = $('select[id$="ddlCantidadCuotas"] option:selected').val();
        var idAfiliado = $("input[id*='hdfIdAfiliado']").val();
        var idFilialPredeterminada = $("input[id*='hdfIdFilialPredeterminada']").val();
        var idUsuarioEvento = $("input[id*='hdfIdUsuarioEvento']").val();
        var idMoneda = $('select[id$="ddlMonedas"] option:selected').val();
        var idListaPrecio = $('select[id$="ddlListaPrecio"]').val();
        $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {
            var ddlProducto = $(this).find('[id*="ddlProducto"]');
            var lblProductoDescripcion = $(this).find('[id*="lblProductoDescripcion"]');
            var hdfIdProducto = $(this).find("input[id*='hdfIdProducto']");
            var hdfPrecio = $(this).find("input[id*='hdfPrecio']");
            var hdfProductoDetalle = $(this).find("input:hidden[id*='hdfProductoDetalle']");
            var txtCantidad = $(this).find("input:text[id*='txtCantidad']");
            var txtDescripcion = $(this).find("input:text[id*='txtDescripcion']");
            var txtPrecioUnitario = $(this).find("input:text[id*='txtPrecioUnitario']");
            var hdfPreUnitario = $(this).find("input:hidden[id*='hdfPreUnitario']");
            var hdfModificaPrecio = $(this).find("input:hidden[id*='hdfModificaPrecio']");
            var hdfStockeable = $(this).find("input[id*='hdfStockeable']");
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
                            idMoneda: idMoneda,
                            idUsuarioEvento: idUsuarioEvento,
                            idListaPrecio: idListaPrecio,
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
                                    precioEditable: item.PrecioEditable,
                                    stockeable: item.Stockeable,
                                    //IdListaPrecioDetalle: item.IdListaPrecioDetalle,
                                }
                            })
                        };
                        cache: true
                    }
                },
            });

            ddlProducto.on('select2:select', function (e) {
                lblProductoDescripcion.text(e.params.data.productoDescripcion);
                hdfProductoDetalle.val(e.params.data.text);
                hdfIdProducto.val(e.params.data.id);
                txtPrecioUnitario.val(accounting.formatMoney(e.params.data.precioUnitarioSinIva, gblSimbolo, 2, gblSeparadorMil));
                hdfPreUnitario.val(e.params.data.precioUnitarioSinIva);
                txtCantidad.val(accounting.formatNumber(1, 2, gblSeparadorMil, ","));
                hdfModificaPrecio.val(e.params.data.precioEditable);
                hdfStockeable.val(e.params.data.stockeable);
                hdfPrecio.val(e.params.data.precio);
                //CalcularPrecio();
                CalcularItem();
                //txtDescripcion.focus();
            });
            ddlProducto.on('select2:unselect', function (e) {
                lblProductoDescripcion.text('');
                hdfProductoDetalle.val('');
                hdfIdProducto.val('');
                txtPrecioUnitario.val(accounting.formatMoney(0, gblSimbolo, 2, gblSeparadorMil));
                hdfPreUnitario.val('');
                hdfModificaPrecio.val('');
                hdfStockeable.val('');
                hdfPrecio.val('');
                CalcularItem();
            });

            rowindex++;
        });
    }
</script>
<AUGE:BuscarClienteAjax Id="ctrBuscarCliente" runat="server" />
<asp:HiddenField ID="hdfIdUsuarioEvento" runat="server" />
<asp:HiddenField ID="hdfIdFilialPredeterminada" runat="server" />
<asp:HiddenField ID="hdfModulo" runat="server" />
<asp:HiddenField ID="hdfControlFocus" runat="server" />
<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="card">
            <div class="card-header">
                Datos del Comprobante
            </div>
            <div class="card-body">
                <div class="form-group row">
                    <div class="col-sm-4">
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTipoDoc" runat="server" Text="Tipo y Num. de doc."></asp:Label>
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlTipoDocumentoCliente" Enabled="false" runat="server" />
                            </div>
                            <div class="col-sm-4">
                                <Evol:CurrencyTextBox CssClass="form-control" Prefix="" ThousandsSeparator="" Enabled="false" NumberOfDecimals="0" ID="txtCuit" runat="server" />
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCuit" runat="server" ControlToValidate="txtCuit"
                                    ErrorMessage="*" ValidationGroup="Aceptar" />
                                <asp:RequiredFieldValidator ID="rfvCuit2" ValidationGroup="Aceptar" ControlToValidate="txtCuit" CssClass="ValidadorBootstrap" EnableViewState="true" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-2">

                                <asp:Button CssClass="botonesEvol" ID="btnTxtCuitBlur" Text="Validar" OnClick="btnTxtCuitBlur_Click" runat="server" />
                            </div>
                        </div>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRAzonSocial" runat="server" Text="Razon Social"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control " ID="txtRazonSocialCliente" Enabled="false" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvRazonSocialCliente" ValidationGroup="Aceptar" ControlToValidate="txtRazonSocialCliente" CssClass="ValidadorBootstrap" EnableViewState="true" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCondicionFiscalCliente" runat="server" Text="Condicion Fiscal" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlCondicionFiscalCliente" Enabled="false" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvCondicionFiscalCliente" ValidationGroup="Aceptar" ControlToValidate="ddlCondicionFiscalCliente" CssClass="ValidadorBootstrap" EnableViewState="true" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDomicilio" runat="server" Text="Domicilio"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control " ID="txtDomicilio" Enabled="false" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvDomicilio" ValidationGroup="Aceptar" Enabled="false" ControlToValidate="txtDomicilio" CssClass="ValidadorBootstrap" EnableViewState="true" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblLocalidad" runat="server" Text="Localidad"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control " ID="txtLocalidad" Enabled="false" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvLocalidad" ValidationGroup="Aceptar" Enabled="false" ControlToValidate="txtLocalidad" CssClass="ValidadorBootstrap" EnableViewState="true" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialDescripcion" runat="server" Text="Emisión"></asp:Label>
                    <%--<asp:TextBox CssClass="form-control" ID="txtFilialDescripcion" runat="server" Enabled="false" ></asp:TextBox>--%>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlFilialPuntoVenta" runat="server" OnSelectedIndexChanged="ddlFilialPuntoVenta_SelectedIndexChanged"
                            AutoPostBack="true" />
                        <asp:RequiredFieldValidator ID="rfvFilialPuntoVenta" ValidationGroup="Aceptar" ControlToValidate="ddlFilialPuntoVenta" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoFactura" runat="server" Text="Tipo Cbte." />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoFactura" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoFactura_SelectedIndexChanged" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvTipoFactura" ValidationGroup="Aceptar" ControlToValidate="ddlTipoFactura" CssClass="ValidadorBootstrap" EnableViewState="true" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroFactura" runat="server" Text="Nro. Cbte."></asp:Label>
                    <div class="col-sm-3">
                        <div class="select2PrefijoNumeroComprobante">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlPrefijoNumeroFactura" AutoPostBack="true" OnSelectedIndexChanged="ddlPrefijoNumeroFactura_SelectedIndexChanged" runat="server" />
                        </div>
                        <AUGE:NumericTextBox CssClass="form-control txtSufijoNumeroComprobante2" ID="txtNumeroFactura" Enabled="false" runat="server" MaxLength="8" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaFactura" runat="server" Text="Fecha Cbte."></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaFactura" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="ValidadorBootstrap" ID="rfvFechaFactura" ControlToValidate="txtFechaFactura" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaVencimiento" runat="server" Text="Fecha Vto."></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaVencimiento" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblListaPrecio" runat="server" Text="Lista de Precio" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlListaPrecio" AutoPostBack="true" OnSelectedIndexChanged="ddlListaPrecio_SelectedIndexChanged" runat="server" />
                    </div>
                </div>
                <asp:UpdatePanel ID="upConceptoComprobante" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblConceptoComprobante" runat="server" Text="Concepto" />
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlConceptoComprobante" AutoPostBack="true" OnSelectedIndexChanged="ddlConceptoComprobante_SelectedIndexChanged" runat="server" />
                                <asp:RequiredFieldValidator ID="rfvConceptoComprobante" ValidationGroup="Aceptar" ControlToValidate="ddlConceptoComprobante" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            </div>
                            <asp:PlaceHolder ID="phPeriodoFecha" Visible="false" runat="server">
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPeriodoFechaDesde" runat="server" Text="Periodo Desde"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox CssClass="form-control datepicker" ID="txtPeriodoFechaDesde" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="ValidadorBootstrap" Enabled="false" ID="rfvPeriodoFechaDesde" ControlToValidate="txtPeriodoFechaDesde" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                                </div>
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPeriodoFechaHasta" runat="server" Text="Hasta"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox CssClass="form-control datepicker" ID="txtPeriodoFechaHasta" runat="server"></asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="ValidadorBootstrap" Enabled="false" ID="rfvPeriodoFechaHasta" ControlToValidate="txtPeriodoFechaHasta" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                                </div>
                            </asp:PlaceHolder>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlMonedas" AutoPostBack="true" OnSelectedIndexChanged="ddlMonedas_OnSelectedIndexChanged" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvMonedas" ValidationGroup="Aceptar" ControlToValidate="ddlMonedas" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        <asp:HiddenField ID="hdfMonedaCotizacion" Value="" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadCuotas" runat="server" Text="Cant. Cuotas" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlCantidadCuotas" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblAcopioFinanciero" runat="server" Text="Acopio Financiero"></asp:Label>
                    <div class="col-sm-3">
                        <asp:CheckBox ID="chkAcopioFinanciero" CssClass="form-control" runat="server" />
                    </div>
                </div>
                <div class="form-group row" runat="server" id="dvFacturaContado">
                    <%--                <asp:UpdatePanel ID="upFacturaContado" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>--%>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFacturaContado" runat="server" Text="Factura Contado" />
                    <div class="col-sm-3">
                        <asp:CheckBox ID="chkFacturaContado" CssClass="form-control" OnCheckedChanged="chkFacturaContado_CheckedChanged" AutoPostBack="true" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBancoCuenta" runat="server" Text="Banco Cuenta" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlBancoCuenta" Enabled="false" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroRecibo" runat="server" Text="Nro. Recibo"></asp:Label>
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control txtPrefijoNumeroComprobante2" ID="txtPrefijoNumeroRecibo" runat="server" MaxLength="4" />
                        <AUGE:NumericTextBox CssClass="form-control txtSufijoNumeroComprobante2" ID="txtNumeroRecibo" runat="server" MaxLength="8" />
                    </div>
                    <%--                    </ContentTemplate>
                </asp:UpdatePanel>--%>
                </div>
                <div class="form-group row">
                    <%--<asp:Label CssClass="col-sm-1 col-form-label" ID="lblObservacion" runat="server" Text="Observacion" />--%>
                    <div class="col-sm-6">
                        <asp:TextBox CssClass="form-control" Rows="2" ID="txtObservacionComprobante" Enabled="false" runat="server" Placeholder="Observacion Comprobante" TextMode="MultiLine" />
                    </div>
                    <div class="col-sm-6">
                        <asp:TextBox CssClass="form-control" Rows="2" ID="txtObservacion" Enabled="false" runat="server" Placeholder="Observacion Interna" TextMode="MultiLine" />
                    </div>
                </div>
                <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
                <asp:Panel ID="pnlProveedores" runat="server" Visible="false">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProveedores" runat="server" Text="Vendedor" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlProveedores" runat="server" />
                    </div>
                    <div class="col-sm-8"></div>
                </asp:Panel>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:HiddenField ID="hdfRemitosMostrar" Value="0" runat="server" />
<asp:UpdatePanel ID="upRemitos" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="card">
                <div class="card-header">
                    <a data-toggle="collapse" href="#collapseremitos" onclick="MostrarRemitos();" aria-expanded="true" aria-controls="collapse-remitos" id="heading-remitos" class="text-reset text-decoration-none">Datos del Remito
                        <i class="fa fa-chevron-down pull-right"></i>
                    </a>
                </div>
                <div id="collapseremitos" class="collapse hide" aria-labelledby="heading-remitos">
                    <div class="card-body">
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRemitoAutomatico" runat="server" Text="Generar" />
                            <div class="col-sm-1">
                                <asp:CheckBox ID="chkGenerarRemito" CssClass="form-control" AutoPostBack="true" OnCheckedChanged="chkGenerarRemito_CheckedChanged" runat="server" />
                            </div>
                            <div class="col-sm-2">
                                <asp:DropDownList CssClass="form-control select2" Enabled="false" ID="ddlTipoOperacionRemito" runat="server" />
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoRemito" runat="server" Text="Tipo Cbte." />
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlTipoRemito" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoRemito_SelectedIndexChanged" runat="server" />
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroRemito" runat="server" Text="Nro. Remito"></asp:Label>
                            <div class="col-sm-3">
                                <div class="select2PrefijoNumeroComprobante">
                                    <asp:DropDownList CssClass="form-control select2 select2PrefijoNumeroComprobante" ID="ddlPrefijoNumeroRemito" AutoPostBack="true" OnSelectedIndexChanged="ddlPrefijoNumeroRemito_SelectedIndexChanged" runat="server" />
                                </div>
                                <AUGE:NumericTextBox CssClass="form-control txtSufijoNumeroComprobante2" ID="txtNumeroRemito" Enabled="false" runat="server" MaxLength="8" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstadoRemito" runat="server" Text="Estado" />
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlEstadosRemtios" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEstadosRemtios_SelectedIndexChanged" />
                                <asp:RequiredFieldValidator ID="rfvEstadosRemitos" Enabled="false" ValidationGroup="Aceptar" ControlToValidate="ddlEstadosRemtios" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaEntrega" runat="server" Text="Fecha Entrega"></asp:Label>
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaEntrega" Enabled="false" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator CssClass="ValidadorBootstrap" Enabled="false" ID="rfvFechaEntrega" ControlToValidate="txtFechaEntrega" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialEntrega" runat="server" Text="Filial Entrega" ToolTip="Filial de la que se descontara el Stock!"></asp:Label>
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlFilialEntrega" runat="server" ToolTip="Filial de la que se descontara el Stock!" />
                                <asp:RequiredFieldValidator CssClass="ValidadorBootstrap" Enabled="false" ID="rfvFilialEntrega" ControlToValidate="ddlFilialEntrega" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
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
                                <asp:TextBox CssClass="form-control" Rows="2" ID="txtRemitoObservacionComprobante" Enabled="false" runat="server" Placeholder="Observacion Comprobante" TextMode="MultiLine" />
                            </div>
                            <div class="col-sm-6">
                                <asp:TextBox CssClass="form-control" Rows="2" ID="txtRemitoObservacionInterna" Enabled="false" runat="server" Placeholder="Observacion Interna" TextMode="MultiLine" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upComprobantesAsociados" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlComprobantesAsociados" Visible="false" runat="server">
            <div class="card">
                <div class="card-header">
                    Comprobantes Asociados
                </div>
                <div class="card-body">
                    <div class="form-group row">
                        <%--<asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoComprobanteAsociado" runat="server" Text="Tipo" />
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoFacturaAsociado" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvTipoFacturaAsociado" ValidationGroup="AgregarComprobante" ControlToValidate="ddlTipoFacturaAsociado" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>--%>
                        <%--<asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroComprobanteAsociado" runat="server" Text="Nro. Cbte."></asp:Label>
                        <div class="col-sm-3">
                            <AUGE:NumericTextBox CssClass="form-control txtPrefijoNumeroComprobante2" ID="txtPrefijoNumeroFacturaAsociado" runat="server" MaxLength="4" />
                            <AUGE:NumericTextBox CssClass="form-control txtSufijoNumeroComprobante2" ID="txtNumeroFacturaAsociado" runat="server" MaxLength="8" />
                            <asp:RequiredFieldValidator ID="rfvPrefijoNumeroFacturaAsociado" ValidationGroup="AgregarComprobante" ControlToValidate="txtPrefijoNumeroFacturaAsociado" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="rfvSufijoNumeroFacturaAsociado" ValidationGroup="AgregarComprobante" ControlToValidate="txtNumeroFacturaAsociado" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>--%>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblddlTipoComprobanteAsociado" runat="server" Text="Tipo Cbte."></asp:Label>
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoComprobanteAsociado" runat="server" />
                        </div>
                        <div class="col-sm-4">
                            <asp:Button CssClass="botonesEvol" ID="btnAgregarComprobanteAsociado" runat="server" Text="Agregar" OnClick="btnAgregar_Click" ValidationGroup="AgregarComprobante" />
                        </div>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="False" DataKeyNames="IndiceColeccion"
                            runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="False"
                            OnPageIndexChanging="gvDatos_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="Numero" SortExpression="Afiliado.IdAfiliado">
                                    <ItemTemplate>
                                        <%# Eval("Afiliado.IdAfiliado")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Razon Social" Visible="false" SortExpression="Afiliado.Apellido">
                                    <ItemTemplate>
                                        <%# Eval("Afiliado.Apellido")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tipo Comprobante" SortExpression="TipoFactura.Descripcion">
                                    <ItemTemplate>
                                        <%# Eval("TipoFactura.Descripcion")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Numero Comprobante" SortExpression="NumeroFactura">
                                    <ItemTemplate>
                                        <%# Eval("NumeroFactura")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fecha Factura" SortExpression="FechaFactura">
                                    <ItemTemplate>
                                        <%# Eval("FechaFactura", "{0:dd/MM/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--                    <asp:TemplateField HeaderText="Fecha Vencimiento" SortExpression="FechaVencimiento">
                        <ItemTemplate>
                            <%# Eval("FechaVencimiento", "{0:dd/MM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Importe" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="ImporteSinIVA">
                                    <ItemTemplate>
                                        <%# Eval("ImporteSinIVA", "{0:C2}")%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblImporte" runat="server" Text=""></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Iva" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="IvaTotal">
                                    <ItemTemplate>
                                        <%# Eval("IvaTotal", "{0:C2}")%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblIvaTotal" runat="server" Text=""></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Importe Total" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="ImporteTotal">
                                    <ItemTemplate>
                                        <%# Eval("ImporteTotal", "{0:C2}")%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                    <ItemTemplate>
                                        <%# Eval("Estado.Descripcion")%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Acciones">
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                            AlternateText="Eliminar" ToolTip="Eliminar" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upItems" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <AUGE:popUpBuscarRemito ID="ctrBuscarRemitoPopUp" runat="server" />
        <AUGE:popUpBuscarPresupuesto ID="ctrBuscarPresupuestoPopUp" runat="server" />
        <AUGE:popUpBuscarNotaPedido ID="ctrBuscarNotaPedidoPopUp" runat="server" />
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
                            <asp:TextBox CssClass="form-control" ID="txtCantidadAgregar" Enabled="false" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-sm-2">
                            <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" runat="server" Text="Agregar item" OnClick="btnAgregarItem_Click" />
                        </div>
                    </asp:PlaceHolder>
                    <div class="col-sm-3">
                        <div class="btn-group" id="btnImportarCosas" runat="server" role="group">
                            <button type="button" class="botonesEvol dropdown-toggle"
                                data-toggle="dropdown" aria-expanded="false">
                                Importar <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu" role="menu">
                                <li>
                                    <asp:Button CssClass="dropdown-item" ID="btnImportarRemito" Visible="false" runat="server" Text="Importar Remito" OnClick="btnImportarRemito_Click" /></li>
                                <li>
                                    <asp:Button CssClass="dropdown-item" ID="btnImportarPresupuesto" Visible="false" runat="server" Text="Importar Presupuesto" OnClick="btnImportarPresupuesto_Click" /></li>
                                <li>
                                    <asp:Button CssClass="dropdown-item" ID="btnImportarNotaPedido" Visible="false" runat="server" Text="Importar Notas de Pedido" OnClick="btnImportarNotaPedido_Click" /></li>
                                <li>
                            </ul>
                        </div>
                    </div>
                    <asp:PlaceHolder ID="phTurismoPorcentajeFactura" runat="server" Visible="false">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPorcentajeFacturar" runat="server" Text="% a Facturar"></asp:Label>
                        <div class="col-sm-1">
                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtPorcentajeTurismo" Prefix="" NumberOfDecimals="2" runat="server" />
                            <asp:RequiredFieldValidator CssClass="ValidadorBootstrap" ID="rfvPorcentajeTurismo" ControlToValidate="txtPorcentajeTurismo" ValidationGroup="CalcularPorcentaje" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-sm-1">
                            <asp:Button CssClass="botonesEvol" ID="btnCalcularPorcentajeTurismo" runat="server" Text="Calcular" ValidationGroup="CalcularPorcentaje" OnClick="btnCalcularPorcentajeTurismo_Click" />
                        </div>
                    </asp:PlaceHolder>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadDecimales" runat="server" Text="Decimales"></asp:Label>
                    <div class="col-sm-2">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlCantidadDecimales" runat="server" OnSelectedIndexChanged="ddlCantidadDecimales_OnClick" AutoPostBack="true" Enabled="false"></asp:DropDownList>
                        <asp:HiddenField ID="hdfNoCalculaImporteDescuento" Value="" runat="server" />
                    </div>
                </div>
                <div runat="server" id="dvCargaCodigoBarras">
                    <hr />
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProducto" runat="server" Text="Producto" />
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlProductoCodigo" Enabled="true" runat="server" />
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
                    <hr />
                </div>
                <div class="table-responsive">
                    <asp:GridView ID="gvItems" DataKeyNames="IndiceColeccion" AllowPaging="false" AllowSorting="false"
                        OnRowCommand="gvItems_RowCommand" runat="server" SkinID="GrillaResponsive"
                        AutoGenerateColumns="false" ShowFooter="true" OnRowDataBound="gvItems_RowDataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Código - Producto / Descripcion" SortExpression="">
                                <ItemTemplate>
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlProducto" runat="server" Enabled="false"></asp:DropDownList>
                                    <asp:HiddenField ID="hdfIdProducto" Value='<%#Bind("ListaPrecioDetalle.Producto.IdProducto") %>' runat="server" />
                                    <asp:Label CssClass="col-form-label" ID="lblProducto" Visible="false" Enabled="false" Text='<%#Bind("DescripcionProducto") %>' runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdfStockeable" Value='<%#Bind("ListaPrecioDetalle.Producto.Familia.Stockeable") %>' runat="server" />
                                    <asp:Label CssClass="col-form-label" ID="lblProductoDescripcion" Visible="false" Text='<%#Bind("Descripcion")%>' runat="server"></asp:Label>
                                    <asp:TextBox CssClass="form-control" placeholder="Ingrese un detalle..." ID="txtDescripcion" Enabled="false" Text='<%#Bind("Descripcion") %>' runat="server"></asp:TextBox>
                                    <asp:HiddenField ID="hdfProductoDetalle" Value='<%#Bind("DescripcionProducto") %>' runat="server" />
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
                            <asp:TemplateField HeaderText="% Desc." ItemStyle-Wrap="false" SortExpression="">
                                <ItemTemplate>
                                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtDescuentoPorcentual" Prefix="" Enabled="false" runat="server" Text='<%# Eval("DescuentoPorcentual")==null ? "0,00" : Eval("DescuentoPorcentual", "{0:N2}") %>'></Evol:CurrencyTextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Importe Desc." HeaderStyle-CssClass="text-right" ItemStyle-Wrap="false" SortExpression="">
                                <ItemTemplate>
                                    <asp:Label CssClass="col-form-label" ID="lblDescuentoImporte" runat="server" Text='<%#Bind("DescuentoImporte", "{0:C2}") %>'></asp:Label>
                                    <asp:HiddenField ID="hdfDescuentoImporte" Value='<%#(Eval("DescuentoImporte") == null ? string.Empty : Eval("DescuentoImporte")).ToString().Replace(",",".") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subtotal" HeaderStyle-CssClass="text-right" ItemStyle-Wrap="false" SortExpression="">
                                <ItemTemplate>
                                    <asp:Label CssClass="col-form-label" ID="lblSubtotal" runat="server" Text='<%#Bind("SubTotal", "{0:C2}") %>'></asp:Label>
                                    <asp:HiddenField ID="hdfSubtotal" Value='<%#(Eval("SubTotal") == null ? string.Empty : Eval("SubTotal")).ToString().Replace(",",".")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Alícuota IVA" ItemStyle-Wrap="false" SortExpression="">
                                <ItemTemplate>
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlAlicuotaIVA" runat="server" Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Importe IVA" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" SortExpression="">
                                <ItemTemplate>
                                    <asp:Label CssClass="col-form-label" ID="lblImporteIva" runat="server" Text='<%#Bind("ImporteIVA", "{0:C2}") %>'></asp:Label>
                                    <asp:HiddenField ID="hdfImporteIva" Value='<%#Bind("ImporteIVA")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Subtotal c/IVA" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" SortExpression="">
                                <ItemTemplate>
                                    <asp:Label CssClass="col-form-label" ID="lblSubtotalConIva" runat="server" Text='<%#Bind("SubTotalConIva", "{0:C2}") %>'></asp:Label>
                                    <asp:HiddenField ID="hdfSubtotalConIva" Value='<%# (Eval("SubTotalConIva") == null ? string.Empty : Eval("SubTotalConIva")).ToString().Replace(",",".")%>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Centro de Costo" SortExpression="" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlCentrosCostos" runat="server" />
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
                <asp:UpdatePanel ID="upPercepciones" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:Button CssClass="botonesEvol" ID="AgregarPercepcion" runat="server" Text="Agregar Percepcion" Visible="false" OnClick="btnAgregarPercepcion_Click" />
                            </div>
                        </div>
                        <div class="table-responsive">
                            <asp:GridView ID="gvPercepciones" AllowPaging="true" AllowSorting="true"
                                OnRowCommand="gvPercepciones_RowCommand" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                                OnRowDataBound="gvPercepciones_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="AFIP Percepciones" SortExpression="" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlPercepciones" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Alicuota" SortExpression="">
                                        <ItemTemplate>
                                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtPorcentajePercepcion" Prefix="" Enabled="false" runat="server" Text='<%#Bind("Porcentaje", "{0:N2}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Importe Percepcion" SortExpression="">
                                        <ItemTemplate>
                                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtImportePercepcion" Enabled="false" runat="server" Text='<%#Bind("Importe", "{0:N2}") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblImportePercepciones" runat="server" Text="0.00"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Eliminar" SortExpression="">
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
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtTotalSinIva" Enabled="false" runat="server" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblTotalIva" runat="server" Text="Total iva" />
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtTotalIva" Enabled="false" runat="server" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblTotal" runat="server" Text="Total con iva" />
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtTotalConIva" Enabled="false" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <AUGE:AsientoMostrar ID="ctrAsientoMostrar" runat="server" />
        <%--<AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />--%>
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
                <asp:Button CssClass="botonesEvol" ID="btnAgregarOC" runat="server" Text="Agregar Orden de Cobro" Visible="false" OnClick="btnAgregarOC_Click" CausesValidation="false" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" CausesValidation="false" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>