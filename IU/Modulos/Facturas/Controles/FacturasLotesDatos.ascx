<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FacturasLotesDatos.ascx.cs" Inherits="IU.Modulos.Facturas.Controles.FacturasLotesDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" TagName="popUpGrillaGenerica" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%--<%@ Register src="~/Modulos/Facturas/Controles/ProductosBuscarPopUp.ascx" tagname="popUpBuscarProducto" tagprefix="auge" %>--%>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmarJS.ascx" TagName="popUpBotonConfirmarJS" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<AUGE:popUpBotonConfirmarJS ID="popUpBotonConfJS" runat="server" />

<style type="text/css">
    .myProgress {
        width: 100%;
        background-color: grey;
    }

    .myBar {
        width: 0%;
        height: 30px;
        background-color: #15376e;
        text-align: center; /* To center it horizontally (if you want) */
        line-height: 30px; /* To center it vertically */
        color: white;
    }

    .myBarError {
        height: 30px;
        background-color: Red;
        text-align: center; /* To center it horizontally (if you want) */
        line-height: 30px; /* To center it vertically */
        color: white;
    }
</style>
<script type="text/javascript">
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ExpandCollapse);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(fnMostrarArchivo);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(msgTooltipError);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitProductoSelect2);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(Cargos);
        ExpandCollapse();
        InitProductoSelect2();
    });
    function Cargos() {
        $("select[name$='ddlTiposCargos']").select2({
            allowClear: true,
            selectOnClose: false,
            placeholder: "Cargos"
        });
    }
    function msgTooltipError() {
        $('[data-toggle="tooltip"]').tooltip()
    }

    function ExpandCollapse() {
        $("[src*=plus]").on('click', function () {
            var src = $(this).attr("src");
            if (src.indexOf("plus") >= 0) {
                $(this).attr("src", "../../Imagenes/minus.png");
                //$(this).closest('tr').next('tr').find("[id *='pnlDatosDetalles']").show();
                var panelDetalle = $(this).closest('tr').next('tr').find("[id *='pnlDatosDetalles']");
                panelDetalle.show();
                var hdfIdFacturaLoteEnviadoFactura = $(this).closest('tr').next('tr').find('[id*="hdfIdFacturaLoteEnviadoFactura"]');
                var hdfMostrarDetalle = $(this).closest('tr').next('tr').find('[id*="hdfMostrarDetalle"]');
                if (hdfMostrarDetalle.val() == 0) {
                    hdfMostrarDetalle.val(1);
                    $.ajax({
                        type: "POST",
                        url: "FacturasWS.asmx/FacturasLotesEnviadosFacturasDetallesSeleccionar",
                        data: JSON.stringify({ 'IdFacturaLoteEnviadoFactura': hdfIdFacturaLoteEnviadoFactura.val() }),
                        contentType: "application/json",
                        dataType: "json",
                        success: function (data) {
                            var grilla = '<table class="GridViewStyle" cellspacing="0" cellpadding="4" style="width:100%;border-collapse:collapse;">' +
                                '<tbody><tr class="GridViewHeaderStyle" style="white-space:nowrap;">' +
                                '<th scope="col">Descripcion</th>' +
                                '<th scope="col" align="right">Cantidad</th>' +
                                '<th scope="col" align="right">PrecioUnitarioSinIva</th>' +
                                '<th scope="col" align="right">SubTotal</th>' +
                                '<th scope="col" align="right">ImporteIVA</th>' +
                                '<th scope="col" align="right">SubTotalConIva</th></tr>';
                            for (var i = 0; i < data.d.length; i++) {
                                grilla = grilla + '<tr class="GridViewRowStyle"><td>' + data.d[i].Descripcion +
                                    '</td><td align="right">' + data.d[i].Cantidad +
                                    '</td><td align="right">' + accounting.formatMoney(data.d[i].PrecioUnitarioSinIva, gblSimbolo, 2, ".") +
                                    '</td><td align="right">' + accounting.formatMoney(data.d[i].SubTotal, gblSimbolo, 2, ".") +
                                    '</td><td align="right">' + accounting.formatMoney(data.d[i].ImporteIVA, gblSimbolo, 2, ".") +
                                    '</td><td align="right">' + accounting.formatMoney(data.d[i].SubTotalConIva, gblSimbolo, 2, ".") +
                                    '</td></tr>';
                            }
                            grilla = grilla + '<tr class="GridViewFooterStyle"><td colspan="6">&nbsp;</td></tr></tbody></table>';
                            panelDetalle.append(grilla);
                        }
                    })
                }
            }
            else {
                $(this).attr("src", "../../Imagenes/plus.png");
                $(this).closest('tr').next('tr').find("[id *='pnlDatosDetalles']").hide();
            }
        });
    }

    function showConfirmFacturasLotes(ctrl) {
        var cantidad = 0;
        var cantidadNo = 0;
        $('#<%=gvDatos.ClientID%> tr').each(function () {
            var checkbox = $(this).find('input:checkbox[id*="chkIncluir"]');
            if (checkbox) {
                if ($(checkbox).is(":checked")) {
                    cantidad++;
                } else {
                    cantidadNo++;
                }
            }
        });
        var msg = "";
        //        if (cantidadNo > 0) {
        //            msg = cantidadNo + " comprobantes quedaran pendientes. ";
        //        }
        msg = msg + "Se van a generar " + cantidad + " comprobantes. ¿Esta seguro que desea continuar?";
        showConfirm(ctrl, msg);
    }

    var cantidad = 0;
    function CalcularTotales() {
        var TotalSinIVA = 0.00;
        var TotalIVA = 0.00;
        var Total = 0;
        cantidad = 0;
        $('#<%=gvDatos.ClientID%> tr').each(function () {
            //var incluir = $("td:eq(6) :checkbox", this).is(":checked");
            var incluir = $(this).find('input:checkbox[id*="chkIncluir"]').is(":checked");
            var importeSinIva = $(this).find('input:hidden[id*="hdfImporteSinIVA"]').val();
            var ivaTotal = $(this).find('input:hidden[id*="hdfIvaTotal"]').val();
            var ImporteTotal = $(this).find('input:hidden[id*="hdfImporteTotal"]').val();
            if (incluir && importeSinIva && ivaTotal && ImporteTotal) {
                importeSinIva = importeSinIva.replace('.', '').replace(',', '.'); //remplazo . por nada y , por .
                ivaTotal = ivaTotal.replace('.', '').replace(',', '.'); //remplazo . por nada y , por .
                ImporteTotal = ImporteTotal.replace('.', '').replace(',', '.'); //remplazo . por nada y , por .
                TotalSinIVA += parseFloat(importeSinIva);
                TotalIVA += parseFloat(ivaTotal);
                Total += parseFloat(ImporteTotal);
                cantidad++;
            }
        });

        $("#<%=gvDatos.ClientID %> [id$=lblImporte]").text(accounting.formatMoney(TotalSinIVA, gblSimbolo, 2, "."));
        $("#<%=gvDatos.ClientID %> [id$=lblIvaTotal]").text(accounting.formatMoney(TotalIVA, gblSimbolo, 2, "."));
        $("#<%=gvDatos.ClientID %> [id$=lblImporteTotal]").text(accounting.formatMoney(Total, gblSimbolo, 2, "."));
        $("#<%=gvDatos.ClientID %> [id$=lblCantidadRegistrosSeleccionados]").text("Selec: " + cantidad + " ");

    }

    function CheckRow(objRef) {
        var row = objRef.parentNode.parentNode;
        var GridView = row.parentNode;
        var inputList = GridView.getElementsByTagName("input");
        for (var i = 0; i < inputList.length; i++) {
            var headerCheckBox = inputList[0];
            var checked = true;
            if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                if (!inputList[i].checked) {
                    checked = false;
                    break;
                }
            }
        }
        headerCheckBox.checked = checked;
    }

    function checkAllRow(objRef) {
        var GridView = objRef.parentNode.parentNode.parentNode;
        $('#<%=gvDatos.ClientID%> tr').not(':last').each(function () {
            if ($(this).find('input:checkbox[id*="chkIncluir"]').prop('disabled') == false)
                $(this).find('input:checkbox[id*="chkIncluir"]').prop('checked', objRef.checked);
        });
    }

    function fnMostrarArchivo() {
        //476
        var tipoLote = $('select[id$="ddlTiposLotes"] option:selected').val();;
        if (parseInt(tipoLote) == 476)
            $('[id$=divArchivo]').show();
        else
            $('[id$=divArchivo]').hide();
    }

    var procesando = "";;
    function fnEmpezarProceso() {
        procesando = "";
        $("body").addClass("loading");
        $("#myBar").removeClass("myBarError");
        $("#myBar").addClass("myBar");
        $("#myBar").width("0%");
        $("#myBar").html("");
        var divInicio = document.getElementById("divInicio");
        divInicio.innerHTML = "No salga de la pagina hasta que el proceso finalice";
        move(1);
        $("input[type=button][id$='btnAceptar']").hide();
        $("input[type=submit][id$='btnProcesar']").click();
        fnEjecutarProceso();
    }
    function fnEjecutarProceso() {
        $.ajax({
            type: "POST",
            url: "FacturasWS.asmx/ObetenerMensajes",
            //data: "{ name: '" + message + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: OnSuccess,
            error: function (xhr, errorType, exception) {
                //procesando = "#ERROR";
                var responseText = $.parseJSON(xhr.responseText);
                var divStatus = document.getElementById("divStatus");
                divStatus.innerHTML = "<span style='color:blue;font-weight:bold'>" + exception + " - " + responseText.Message + " - " + responseText.StackTrace + "</span>";
                //$("body").removeClass("loading");
                //$("input[type=button][id$='btnAceptar']").show();
                if (procesando == "#FINALIZADO") {
                    fnFinalizarProceso();
                } else if (procesando == "#PROCESANDO") {
                    fnEjecutarProceso();
                }
            },
            failure: function (xhr, errorType, exception) {
                //procesando = "#ERROR";
                var responseText = $.parseJSON(xhr.responseText);
                var divStatus = document.getElementById("divStatus");
                divStatus.innerHTML = "<span style='color:blue;font-weight:bold'>" + exception + " - " + responseText.Message + " - " + responseText.StackTrace + "</span>";
                //$("body").removeClass("loading");
                //$("input[type=button][id$='btnAceptar']").show();
                if (procesando == "#FINALIZADO") {
                    fnFinalizarProceso();
                } else if (procesando == "#PROCESANDO") {
                    fnEjecutarProceso();
                }
            }
        });
    }

    function fnProcesando() {
        $.ajax({
            type: "POST",
            url: "FacturasWS.asmx/Procesando",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                procesando = r.d;
            },
            error: function (xhr, errorType, exception) {
                procesando = "#ERROR";
                var responseText = $.parseJSON(xhr.responseText);
                var divStatus = document.getElementById("divStatus");
                divStatus.innerHTML = "<span style='color:blue;font-weight:bold'>" + exception + " - " + responseText.Message + " - " + responseText.StackTrace + "</span>";
                $("body").removeClass("loading");
                $("input[type=button][id$='btnAceptar']").show();
            },
            failure: function (xhr, errorType, exception) {
                procesando = "#ERROR";
                var responseText = $.parseJSON(xhr.responseText);
                var divStatus = document.getElementById("divStatus");
                divStatus.innerHTML = "<span style='color:blue;font-weight:bold'>" + exception + " - " + responseText.Message + " - " + responseText.StackTrace + "</span>";
                $("body").removeClass("loading");
                $("input[type=button][id$='btnAceptar']").show();
            }
        });
    }

    var procPendiente = 0;
    function OnSuccess(result) {
        var divStatus = document.getElementById("divStatus");
        var divContabiliza = document.getElementById("divContabiliza");
        var data = result.d;
        var html = "";
        var conta = "";
        var num = 0;
        var dataText = "";
        for (var i = 0; i < data.length; i++) {
            dataText = data[i].text;
            //if (dataText.includes("#CONTA#"))
            //{
            //    conta = dataText.replace("#CONTA#", "");
            //}
            //else
            //{
            html = html + dataText + "<BR />";
            //}
            num = data[i].number;
        }
        divStatus.innerHTML = html;
        divContabiliza.innerHTML = conta;
        move(num);
        setTimeout('', 2000);
        fnProcesando();
        if (procesando == "#FINALIZADO") {
            fnFinalizarProceso();
        } else if (procesando == "#PROCESANDO") {
            fnEjecutarProceso();
        } else if (procesando == "#ERROR") {
            $("#myBar").removeClass("myBar");
            $("#myBar").addClass("myBarError");
            $("input[type=button][id$='btnAceptar']").show();
            fnFinalizarProceso();
        } else {
            procPendiente++;
            if (parseInt(procPendiente) == 40) {
                procPendiente = 0;
                fnFinalizarProceso()
            } else {
                fnEjecutarProceso();
            }
        }
    }

    function fnFinalizarProceso() {
        move(100);
        $("body").removeClass("loading");
        $("input[type=submit][id$='btnFinalizar']").click();
    }

    function move(width) {
        var elem = document.getElementById("myBar");
        var barWith = elem.style.width;
        barWith = barWith.replace('%', '');
        var id = setInterval(frame, 10);
        function frame() {
            if (parseInt(barWith) >= parseInt(width)) {
                clearInterval(id);
            } else {
                barWith++;
                elem.style.width = barWith + '%';
                elem.innerHTML = barWith * 1 + '%';
            }
        }
    }

    function fnErrorValidaciones() {
        $("body").removeClass("loading");
        $("#myBar").removeClass("myBar");
        $("#myBar").addClass("myBarError");
        move(100);
        $("input[type=button][id$='btnAceptar']").show();
    }

    function fnValidarConfirmarLote() {
        //alert("Hola Mundo");
        fnConfirmarJS("Se van a generar " + cantidad + " comprobantes. ¿Esta seguro que desea continuar?", fnConfirmarLoteCallback);
    }

    function fnConfirmarLoteCallback(valor) {
        if (valor == true)
            fnEmpezarProceso();
    }

    function InitProductoSelect2() {
        var control = $("select[name$='ddlProducto']");
        var idAfiliado = 0;
        var idFilialPredeterminada = $("input[id*='hdfIdFilialPredeterminada']").val();
        var idUsuarioEvento = $("input[id*='hdfIdUsuarioEvento']").val();
        var cantidadCuotas = 1; //$('select[id$="ddlCantidadCuotas"] option:selected').val();
        control.select2({
            placeholder: 'Ingrese el codigo o descripcion',
            selectOnClose: true,
            theme: 'bootstrap4',
            minimumInputLength: 1,
            //width: '100%',
            language: 'es',
            //tags: true,
            //allowClear: true,
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
                        idMoneda: 1,
                        idUsuarioEvento: idUsuarioEvento,
                        idListaPrecio: 0,
                    });
                },
                beforeSend: function (xhr, opts) {
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
                            }
                        })
                    };
                    cache: true
                }
            }
        });

        control.on('select2:select', function (e) {
            $("input[id*='hdfIdProducto']").val(e.params.data.id);
            $("input[id*='hdfProductoDetalle']").val(e.params.data.text);
            //CargarTipoPuntoVenta();
        });

        control.on('select2:unselect', function (e) {
            $("input[id*='hdfIdProducto']").val('');
            $("input[id*='hdfProductoDetalle']").val('');
            control.val(null).trigger('change');
            //CargarTipoPuntoVenta();
        });
    }

</script>
<asp:HiddenField ID="hdfIdFilialPredeterminada" runat="server" />
<asp:HiddenField ID="hdfIdUsuarioEvento" runat="server" />

<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
<asp:UpdatePanel ID="upFacturasLotes" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblLoteEnvio" runat="server" Text="Nro. Lote" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtLoteEnvio" Enabled="false" runat="server" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPeriodo" runat="server" Text="Periodo"></asp:Label>
            <div class="col-sm-3">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtPeriodo" Enabled="false" runat="server" MaxLength="6" />
                <asp:RequiredFieldValidator ID="rfvPeriodo" ValidationGroup="Aceptar" ControlToValidate="txtPeriodo" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEmpresa" runat="server" Text="Empresa"></asp:Label>
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlEmpresas" Enabled="false" runat="server" />
                <asp:RequiredFieldValidator ID="rfvEmpresas" ValidationGroup="Aceptar" ControlToValidate="ddlEmpresas" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoLote" runat="server" Text="Tipo de Facturacion"></asp:Label>
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlTiposLotes" Enabled="false" runat="server" OnSelectedIndexChanged="ddlTiposLotes_SelectedIndexChanged"
                    AutoPostBack="true" />
                <asp:RequiredFieldValidator ID="rfvTiposLotes" ValidationGroup="Aceptar" ControlToValidate="ddlTiposLotes" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialDescripcion" runat="server" Text="Tipo de Emisión"></asp:Label>
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlFilialPuntoVenta" runat="server" OnSelectedIndexChanged="ddlFilialPuntoVenta_SelectedIndexChanged"
                    AutoPostBack="true" />
                <asp:RequiredFieldValidator ID="rfvFilialPuntoVenta" ValidationGroup="Aceptar" ControlToValidate="ddlFilialPuntoVenta" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroFactura" runat="server" Text="Punto de Venta"></asp:Label>
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlPrefijoNumeroFactura" runat="server" />
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo de Operación" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlTipoOperacionOC" runat="server" />
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoOperacion" runat="server" ControlToValidate="ddlTipoOperacionOC"
                    ErrorMessage="" ValidationGroup="Aceptar" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaFactura" runat="server" Text="Fecha Comprobante"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaFactura" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaFactura" ControlToValidate="txtFechaFactura" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaVencimiento" runat="server" Text="Fecha Vencimiento"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaVencimiento" runat="server"></asp:TextBox>
            </div>
        </div>
        <asp:UpdatePanel ID="upConceptoComprobante" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblConceptoComprobante" runat="server" Text="Concepto Comprobante" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlConceptoComprobante" AutoPostBack="true" OnSelectedIndexChanged="ddlConceptoComprobante_SelectedIndexChanged" runat="server" />
                        <asp:RequiredFieldValidator ID="rfvConceptoComprobante" ValidationGroup="Aceptar" ControlToValidate="ddlConceptoComprobante" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <asp:PlaceHolder ID="pnlPeriodoFechas" Visible="false" runat="server">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPeriodoFechaDesde" runat="server" Text="Periodo Fecha Desde"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control datepicker" ID="txtPeriodoFechaDesde" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="Validador" Enabled="false" ID="rfvPeriodoFechaDesde" ControlToValidate="txtPeriodoFechaDesde" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPeriodoFechaHasta" runat="server" Text="Hasta"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control datepicker" ID="txtPeriodoFechaHasta" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="Validador" Enabled="false" ID="rfvPeriodoFechaHasta" ControlToValidate="txtPeriodoFechaHasta" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                    </asp:PlaceHolder>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlMonedas" OnSelectedIndexChanged="ddlMonedas_OnSelectedIndexChanged" AutoPostBack="true" runat="server" />
                <asp:RequiredFieldValidator ID="rfvMonedas" ValidationGroup="Aceptar" ControlToValidate="ddlMonedas" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblIVA" runat="server" Text="IVA"></asp:Label>
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlIvas" Enabled="false" runat="server" />
                <asp:RequiredFieldValidator ID="rfvIvas" ValidationGroup="Aceptar" ControlToValidate="ddlIvas" CssClass="Validador" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
            </div>
        </div>
        <asp:UpdatePanel ID="upBuscarProducto" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlBuscarProducto" runat="server">
                    <div class="form-group row">
                        <%--<AUGE:popUpBuscarProducto ID="ctrBuscarProductoPopUp" runat="server" />
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProducto" runat="server" Text="Codigo" />
                    <div class="col-sm-2">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigo" Enabled="false" AutoPostBack="true" runat="server" onTextChanged="txtCodigo_TextChanged" ></AUGE:NumericTextBox>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCodigo" ControlToValidate="txtCodigo" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-sm-1">
                        <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" AutoPostBack="true" ID="btnBuscarProducto" Visible="false"
                                    AlternateText="Buscar producto" ToolTip="Buscar"  onclick="btnBuscarProducto_Click"  />
                    </div>     --%>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProducto" runat="server" Text="Producto" />
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlProducto" Enabled="false" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvProducto" ValidationGroup="Aceptar" ControlToValidate="ddlProducto" CssClass="ValidadorBootstrap" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:HiddenField ID="hdfIdProducto" runat="server" />
                            <asp:HiddenField ID="hdfProductoDetalle" runat="server" />
                        </div>
                        <%--<asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion" />
                    <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtProductoDescripcion" runat="server"></asp:TextBox>
                    </div>--%>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="form-group row" id="divArchivo" style="display: none;">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="Label1" runat="server" Text="Seleccione un archivo"></asp:Label>
            <div class="col-sm-3">
                <asp:AsyncFileUpload ID="AsyncFileUpload1" Width="211px" runat="server"
                    OnUploadedComplete="FileUploadComplete" Height="21px" Font-Size="Larger" />
            </div>
        </div>
        <div class="form-group row" runat="server" id="dvTiposCargos" visible="false">
            <asp:Label CssClass="col-lg-1 col-form-label" ID="Label2" runat="server" Text="Tipos de Cargos" />
            <div class="col-sm-3">
                <%--<asp:CheckBoxList ID="chkTiposCargos" runat="server" ></asp:CheckBoxList>--%>
                <asp:ListBox CssClass="form-control select2" ID="ddlTiposCargos" SelectionMode="multiple" Placeholder="Cargos" runat="server"></asp:ListBox>
            </div>
        </div>
        <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:Button CssClass="botonesEvol" ID="btnGenerarLote" runat="server" Text="Buscar Datos a Facturar" OnClick="btnGenerarLote_Click" ValidationGroup="Aceptar" Visible="false" />
            </div>
        </div>

        <div class="form-group row">
            <div class="col-sm-12">
                <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                    runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
            </div>
        </div>
        <div class="form-group row">
            <div class="col-sm-12">
                <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="false" AllowSorting="false"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdFacturaLoteEnviadoFactura"
                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>

                        <asp:TemplateField HeaderText="# Cliente" SortExpression="AfiliadoIdAfiliado">
                            <ItemTemplate>
                                <%# Eval("IdAfiliado")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="# Documento" SortExpression="NumeroDocumento">
                            <ItemTemplate>
                                <%# Eval("NumeroDocumento")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Razon Social" SortExpression="ClienteRazonSocial">
                            <ItemTemplate>
                                <%# Eval("ClienteRazonSocial")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo Comprobante" ItemStyle-Wrap="false" SortExpression="TipoFacturaDescripcion">
                            <ItemTemplate>
                                <%# Eval("TipoFacturaDescripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Número Comprobante" ItemStyle-Wrap="false" SortExpression="NumeroFactura">
                            <ItemTemplate>
                                <%# Eval("NumeroFacturaCompleto")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha Factura" ItemStyle-Wrap="false" SortExpression="FechaFactura">
                            <ItemTemplate>
                                <%# Eval("FechaFactura", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--                    <asp:TemplateField HeaderText="Fecha Vencimiento" SortExpression="FechaVencimiento">
                        <ItemTemplate>
                            <%# Eval("FechaVencimiento", "{0:dd/MM/yyyy}")%>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="ImporteSinIVA">
                            <ItemTemplate>
                                <%# Eval("ImporteSinIVA", "{0:C2}")%>
                                <asp:HiddenField ID="hdfImporteSinIVA" Value='<%# Eval("ImporteSinIVA")%>' runat="server" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblImporte" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Iva" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="IvaTotal">
                            <ItemTemplate>
                                <%# Eval("IvaTotal", "{0:C2}")%>
                                <asp:HiddenField ID="hdfIvaTotal" Value='<%# Eval("IvaTotal")%>' runat="server" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblIvaTotal" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Importe Total" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="ImporteTotal">
                            <ItemTemplate>
                                <%# Eval("ImporteTotal", "{0:C2}")%>
                                <asp:HiddenField ID="hdfImporteTotal" Value='<%# Eval("ImporteTotal")%>' runat="server" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                            <ItemTemplate>
                                <%# Eval("EstadoDescripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false" FooterStyle-Wrap="false">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkTodos" Text="Acciones  " TextAlign="Left" runat="server" onclick="checkAllRow(this); CalcularTotales();" Visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%--<asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/plus.png" runat="server" CommandName="Consultar" ID="btnConsultar" 
                                AlternateText="Ver detalle" ToolTip="Ver detalle" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/minus.png" runat="server" CommandName="Ocultar" ID="btnOcultar" 
                                AlternateText="Ocultar detalle" ToolTip="Ocultar detalle" Visible="false" />--%>
                                <img alt="Mostrar / Ocultar" id="imgExpandCollapse" style="cursor: pointer; vertical-align: middle;" src="../../Imagenes/plus.png" />
                                <asp:CheckBox ID="chkIncluir" onclick="CheckRow(this); CalcularTotales();" runat="server" />
                                <a href="#" data-toggle="tooltip" data-placement="top" title='<%# Eval("MensajeError") %>'>
                                    <asp:Image ID="imgInfo" ImageUrl="~/Imagenes/alerta.png" runat="server" Visible='<%# !Convert.ToBoolean(Eval("Habilitada")) %>' />
                                </a>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistrosSeleccionados" runat="server" Text=""></asp:Label>
                                <br />
                                <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <!-- this row has the child grid-->
                                </td>
                                </tr>
                                <tr>
                                    <td colspan="100%" style="padding: 0px;">
                                        <%--<asp:UpdatePanel ID="upDatosDetalles" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>--%>
                                        <asp:HiddenField ID="hdfIdFacturaLoteEnviadoFactura" Value='<%#Bind("IdFacturaLoteEnviadoFactura") %>' runat="server" />
                                        <asp:HiddenField ID="hdfMostrarDetalle" Value="0" runat="server" />
                                        <asp:Panel ID="pnlDatosDetalles" runat="server" Style="display: none">
                                            <asp:GridView ID="gvDatosDetalles" runat="server" AutoGenerateColumns="false" SkinID="GrillaBasicaFormal"
                                                ShowFooter="true">
                                                <Columns>
                                                    <asp:BoundField DataField="Descripcion" HeaderText="Producto" />
                                                    <asp:BoundField DataField="Cantidad" HeaderText="Cant." />
                                                    <asp:BoundField DataField="PrecioUnitarioSinIva" DataFormatString="{0:C2}" HeaderText="Prec. Unitario" />
                                                    <asp:BoundField DataField="SubTotal" DataFormatString="{0:C2}" HeaderText="Subtotal" />
                                                    <asp:BoundField DataField="ImporteIVA" DataFormatString="{0:C2}" HeaderText="Importe IVA" />
                                                    <asp:BoundField DataField="SubTotalConIva" DataFormatString="{0:C2}" HeaderText="Subtotal c/IVA" />
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                        <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Evol:EvolGridView>
            </div>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnExportarExcel" />
    </Triggers>
</asp:UpdatePanel>
<div class="form-group row" style="height: 50px;">
    <div class="col-sm-12">
        <div id="myProgress" class="myProgress">
            <div id="myBar"></div>
        </div>
    </div>
</div>
<div class="row justify-content-md-center" style="height: 50px;">
    <div class="col-md-auto">
        <div id="divInicio"></div>
        <div id="divStatus"></div>
        <div id="divContabiliza"></div>
    </div>
</div>
<asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <AUGE:popUpGrillaGenerica ID="ctrPopUpGrilla" runat="server" />
                <input type="button" runat="server" class="botonesEvol" id="btnAceptar" value="Procesar" onclick="fnValidarConfirmarLote();" />
                <%--<asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Generar Lote de Facturas" onclick="btnAceptar_Click" ValidationGroup="Aceptar" Visible="false" />--%>
                <asp:Button CssClass="botonesEvol" ID="btnProcesar" OnClick="btnProcesar_Click" runat="server" CausesValidation="false"
                    Text="Procesar Hidden" Style="display: none" />
                <asp:Button CssClass="botonesEvol" ID="btnFinalizar" OnClick="btnFinalizarProceso_Click" runat="server" CausesValidation="false"
                    Text="Finalizar Hidden" Style="display: none" />
                <asp:Button CssClass="botonesEvol" ID="btnContinuar" runat="server" Text="Continuar" OnClick="btnContinuar_Click" CausesValidation="false" Visible="false" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
