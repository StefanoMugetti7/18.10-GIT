<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SolicitudPagoDatos.ascx.cs" Inherits="IU.Modulos.CuentasPagar.Controles.SolicitudPagoDatos" %>

<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/CuentasPagar/Controles/ProveedoresBuscarPopUp.ascx" TagName="popUpBuscarProveedor" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/CuentasPagar/Controles/ProductosBuscarPopUp.ascx" TagName="popUpBuscarProducto" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/CuentasPagar/Controles/SolicitudPagoBuscarPopUp.ascx" TagName="popUpBuscarSolicitudPago" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Compras/Controles/OrdenesComprasTercerosBuscarPopUp.ascx" TagName="popUpBuscarOrdenesCompras" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/CuentasPagar/Controles/ImportarRemitoPopUp.ascx" TagName="popUpImportarRemito" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/AsientosDatosMostrar.ascx" TagName="AsientoMostrar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/CentrosCostosPrrorrateosDatosPopUp.ascx" TagName="popUpCentrosCostos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Proveedores/Controles/ProveedoresCabecerasDatos.ascx" TagName="BuscarProveedorAjax" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" TagName="popUpGrillaGenerica" TagPrefix="auge" %>
<%--<script src="../../../Recursos/jquery.maskMoney.min.js" type="text/javascript"></script>--%>
<script language="javascript" type="text/javascript">

    $(document).ready(function () {
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CopmletarCerosComprobantes);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(MostrarBotonesCentrosCostos);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridDetalle);
        SetTabIndexInput();
        intiGridDetalle();
        CopmletarCerosComprobantes();
        MostrarBotonesCentrosCostos();
        //$("input[type=text][id$='txtCodigo']").focus();

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

 $(document).keypress(function (e) {

        $(":input").each(function (){
            if (e.keyCode === 13) {
                e.preventDefault();
                return false;
            }
        }); //quita la funcion de enter a todos los imputs
       
    });

    function CopmletarCerosComprobantes() {
        $("input[type=text][id$='txtPreNumeroFactura']").blur(function () { $(this).addLeadingZeros(5); });
        $("input[type=text][id$='txtNumeroFactura']").blur(function () { $(this).addLeadingZeros(8); });
        $("input[type=text][id$='txtNumeroRemito']").blur(function () { $(this).addLeadingZeros(8); });
        $("input[type=text][id$='txtPrefijoNumeroRemito']").blur(function () { $(this).addLeadingZeros(4); });
    }


    function SetTabIndexInput() {
        $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }

    //function ceFechaFacturaSetFocus() {
    //    $("input[type=text][id$='txtPreNumeroFactura']").focus();
    //}

    function RepetirCentrosCostos() {
        var centroCosto = $('#<%=ddlCentrosCostosAsignar.ClientID %>').val();
        $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {
            $(this).find('[id*="ddlCentrosCostos"]').val(centroCosto);
        });
    }

    var TotalSinIVA = 0.00;
    var totalIVA = 0.00;
    var totalConIVA = 0.00;
    var totalPercepciones = 0.00;
    var totalDescuento = 0.00;

    function CalcularItem() {
        var importeIVA = 0.00;
        var subTotalConIVA = 0.00;
        TotalSinIVA = 0.00;
        var subTotalItem = 0.00;
        totalIVA = 0.00;
        totalConIVA = 0.00;
        //Calculo descuento... 
        totalDescuento = $("input[type=text][id$='txtDescuentoTotal']").maskMoney('unmasked')[0];
        if (isNaN(totalDescuento)) {
            totalDescuento = 0.00;
        }

        $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {

            var cantidad = $(this).find('input:text[id*="txtCantidad"]').val().replace('.', '').replace(',', '.');
            //var alicuotaIVA = $(this).find('[id*="ddlAlicuotaIVA"] option:selected').val();
            var data = $(this).find('[id*="ddlAlicuotaIVA"] option:selected').val();
            var alicuotaIVA = data.split('|')[1];

            var importe = $(this).find("input:text[id*='txtPrecioUnitario']").maskMoney('unmasked')[0]; //$("td:eq(4)", this).html();

            var noGravado = $(this).find('input:text[id*="txtPrecioNoGravado"]').maskMoney('unmasked')[0];
            var descuento = $(this).find('input:text[id*="txtDescuentoImporte"]').maskMoney('unmasked')[0];
            if (importe && cantidad) {
                subTotalItem = (parseFloat(importe) * parseFloat(cantidad)) + parseFloat(noGravado) - parseFloat(descuento);
                alicuotaIVA = alicuotaIVA.replace('.', '').replace(',', '.');
                if (alicuotaIVA == "" || isNaN(alicuotaIVA)) {
                    alicuotaIVA = 0.00;
                    importeIVA = 0.00;
                } else {
                    importeIVA = (parseFloat(subTotalItem) - parseFloat(noGravado)) * parseFloat(alicuotaIVA) / 100;
                }
                subTotalItem = parseFloat(subTotalItem).toFixed(2);
                importeIVA = parseFloat(importeIVA).toFixed(2);
                subTotalConIVA = parseFloat(subTotalItem) + parseFloat(importeIVA);
                TotalSinIVA += parseFloat(subTotalItem);
                totalIVA += parseFloat(importeIVA);
                $(this).find('span[id*="lblSubtotal"]').text(accounting.formatMoney(subTotalItem, gblSimbolo, 2, "."));
                $(this).find('span[id*="lblImporteIva"]').text(accounting.formatMoney(importeIVA, gblSimbolo, 2, "."));
                $(this).find('span[id*="lblSubtotalConIva"]').text(accounting.formatMoney(subTotalConIVA, gblSimbolo, 2, "."));
            }
        });

        MostrarTotales();
    }

    function CalcularPercepcion() {
        totalPercepciones = 0.00;
        $('#<%=gvPercepciones.ClientID%> tr').each(function () {

            var importe = $(this).find("input:text[id*='txtImportePercepcion']").maskMoney('unmasked')[0];

            if (importe) {
                totalPercepciones += parseFloat(importe);
            }
        });
        $("#<%=gvPercepciones.ClientID %> [id$=lblImportePercepciones]").text(accounting.formatMoney(totalPercepciones, gblSimbolo, 2, "."));
        MostrarTotales();
    }


    function MostrarTotales() {
        $("input[type=text][id$='txtTotalSinIva']").val(accounting.formatMoney(TotalSinIVA, gblSimbolo, 2, "."));
        $("input[type=text][id$='txtTotalIva']").val(accounting.formatMoney(totalIVA, gblSimbolo, 2, "."));
        $("input[type=text][id$='txtTotalConIva']").val(accounting.formatMoney(TotalSinIVA + totalIVA + totalPercepciones - totalDescuento, gblSimbolo, 2, "."));
        $("input[type=hidden][id$='hdfTotalConIva']").val(accounting.formatMoney(TotalSinIVA + totalIVA + totalPercepciones - totalDescuento, "", 2, "."));
    }

    function MostrarBotonesCentrosCostos() {
        $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {
            var data = $(this).find('[id*="ddlCentrosCostos"] option:selected').val();
            if (data === '') {
                $(this).find('input:image[id*="AgregarCentrosCostos"]').show();
                $(this).find('input:image[id*="ConsultarCentrosCostos"]').hide();
            }
            else {
                $(this).find('input:image[id*="AgregarCentrosCostos"]').hide();
                $(this).find('input:image[id*="ConsultarCentrosCostos"]').show();
            }
        });
    }

    function intiGridDetalle() {
        var rowindex = 0;
        var ddlTipoFactura = $('select[id$="ddlTipoFactura"] option:selected').val();
        var idProveedor = $("input[id*='hdfIdProveedor']").val();

        $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {
             var ddlProducto = $(this).find('[id*="ddlProducto"]');
             var hdfProductoDetalle = $(this).find("input:hidden[id*='hdfProductoDetalle']");
             var hdfIdProducto = $(this).find("input[id*='hdfIdProducto']");

             var txtCantidad = $(this).find("input:text[id*='txtCantidad']");
             var txtPrecioUnitario = $(this).find("input:text[id*='txtPrecioUnitario']");
            var hdfPrecioUnitario = $(this).find("input:hidden[id*='hdfPrecioUnitario']");

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
                            beforeSend: function (xhr, opts) {
                                if (ddlTipoFactura == '') {
                                    MostrarMensaje('Debe seleccionar un Tipo de Factura', 'red');
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
                    hdfProductoDetalle.val(e.params.data.productoDescripcion);
                    hdfIdProducto.val(e.params.data.id);
                    txtPrecioUnitario.val(accounting.formatMoney(e.params.data.precio, gblSimbolo, 2, gblSeparadorMil));
                    hdfPrecioUnitario.val(e.params.data.precio);
                    txtCantidad.val(accounting.formatNumber(1, 2, gblSeparadorMil, ","));
                    CalcularItem();

                });
                ddlProducto.on('select2:unselect', function (e) {
                    hdfProductoDetalle.val('');
                    hdfIdProducto.val('');
                    txtPrecioUnitario.val(accounting.formatMoney(0, gblSimbolo, 2, gblSeparadorMil));
                    hdfPrecioUnitario.val('');
                    txtCantidad.val('');
                    CalcularItem();
                });
                rowindex++;
            });

    }

</script>

<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />


    <%--    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>   
            <asp:Panel ID="pnlSolicitudPago" GroupingText="Solicitud de Pago" runat="server">
            <br />
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialDescripcion" runat="server" Text="Filial"></asp:Label>
            <div class="Espacio"></div>
                <asp:DropDownList CssClass="selectEvol" ID="ddlFilialDescripcion" runat="server" >
                </asp:DropDownList>
                <div class="Espacio"></div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoSolicitudPago" runat="server" Text="Tipo Solicitud" />
                <asp:DropDownList CssClass="selectEvol" ID="ddlTipoSolicitud" runat="server" Width = "150"> </asp:DropDownList>
                <div class="Espacio"></div>
                <br />
                <br />
            </asp:Panel>

            </ContentTemplate>
    </asp:UpdatePanel>--%>
    <asp:Panel ID="pnlDescuentoAfiliado" Visible="false" GroupingText="Datos descuento Socio" runat="server">
        <%--<asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuotasDescuentoAfiliado" runat="server" Text="Cuotas Descuento Socio" />
                <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtCuotasDescuentoAfiliado" Enabled="false" runat="server"></AUGE:NumericTextBox>
                <div class="Espacio"></div>--%>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFomraCobroAfiliado" runat="server" Text="Forma de Cobro Socio" />
        <asp:DropDownList CssClass="selectEvol" ID="ddlFormasCobros" Enabled="false" runat="server" />
        <%--<div class="Espacio"></div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuotasDescuentoProveedor" runat="server" Text="Cuotas Pago Proveedor" />
                <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtCuotasDescuentoProveedor" Enabled="false" runat="server"></AUGE:NumericTextBox>--%>
        <br />
        <br />
    </asp:Panel>
    <AUGE:BuscarProveedorAjax ID="ctrBuscarProveedor" runat="server"></AUGE:BuscarProveedorAjax>


    <div class="card">
        <div class="card-header">
            Datos de la Factura
        </div>
        <div class="card-body">
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaFactura" runat="server" Text="Fecha Factura"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaFactura" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaFactura" ControlToValidate="txtFechaFactura" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroFactura" runat="server" Text="Numero Factura"></asp:Label>
                <div class="col-sm-1">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtPreNumeroFactura" runat="server" MaxLength="5"></AUGE:NumericTextBox>
                    <asp:RequiredFieldValidator CssClass="Validador" Display="Dynamic" ID="rfvPreNumeroFactura" ControlToValidate="txtPreNumeroFactura" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-2">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroFactura" runat="server" MaxLength="8"></AUGE:NumericTextBox>
                    <asp:RequiredFieldValidator CssClass="Validador" Display="Dynamic" ID="rfvNumeroFactura" ControlToValidate="txtNumeroFactura" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtEstado" Enabled="false" runat="server"></asp:TextBox>
                </div>
            </div>

            <asp:UpdatePanel ID="upFactura" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="form-group row">
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoFactura" runat="server" Text="Tipo factura" />
                        <div class="col-sm-3">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoFactura" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoFactura_SelectedIndexChanged" runat="server" />
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoFactura" ControlToValidate="ddlTipoFactura" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaVencimiento" runat="server" Text="Fecha Vencimiento"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaVencimiento" runat="server"></asp:TextBox>
                        </div>
                        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaContable" runat="server" Text="Fecha Contable"></asp:Label>
                        <div class="col-sm-3">
                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaContable" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <%--  <div class="Espacio"></div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoSolicitudPago" runat="server" Text="Tipo Solicitud" />
                <asp:DropDownList CssClass="selectEvol" ID="ddlTipoSolicitud" runat="server" > </asp:DropDownList>
                <asp:RequiredFieldValidator CssClass="Validador"  ID="rfvTipoSolicitud" ControlToValidate="ddlTipoSolicitud" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    --%>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="form-group row">
                
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialPago" runat="server" Text="Filial Pago"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlFilialPago" Enabled="false" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFilialPago" Enabled="false" ControlToValidate="ddlFilialPago" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblAcopioFinanciero" runat="server" Text="Acopio Financiero"></asp:Label>
                <div class="col-sm-3">
                    <asp:CheckBox ID="chkAcopioFinanciero" CssClass="form-control" runat="server" />
                </div>
            </div>
            <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblObservacion" runat="server" Text="Observacion" />
                <div class="col-sm-11">
                    <asp:TextBox CssClass="form-control" ID="txtObservacion" runat="server" MaxLength="500" TextMode="MultiLine" />
                </div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="upRemitos" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pnlRemito" runat="server" Visible="false">
                <div class="card">
                    <div class="card-header">
                        Datos del Remito
                    </div>
                    <div class="card-body">
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRemitoAutomatico" runat="server" Text="Generar Remito" />
                            <div class="col-sm-3">
                                <asp:CheckBox ID="chkGenerarRemito" CssClass="form-control" runat="server" />
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroRemito" runat="server" Text="Número Remito"></asp:Label>
                            <div class="col-sm-1">
                                <AUGE:NumericTextBox CssClass="form-control" ID="txtPrefijoNumeroRemito" runat="server" MaxLength="4" />
                            </div>
                            <div class="col-sm-2">
                                <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroRemito" runat="server" MaxLength="8" />
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaEntrega" runat="server" Text="Fecha"></asp:Label>
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaEntrega" runat="server"></asp:TextBox>
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialEntrega" runat="server" Text="Filial Recepcion" ToolTip="Filial a la que se ingresarra el Stock!"></asp:Label>
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlFilialEntrega" runat="server" ToolTip="Filial a la que se ingresarra el Stock!" />
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
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
                                        <%# Eval("Entidad.IdRefEntidad")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tipo Comprobante" SortExpression="TipoFactura.Descripcion">
                                    <ItemTemplate>
                                        <%# Eval("TiposFacturas.Descripcion")%>
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


    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpItems" HeaderText="Detalle de Factura">
            <ContentTemplate>
                <asp:UpdatePanel ID="items" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" runat="server" Text="Agregar item" OnClick="btnAgregarItem_Click" />
                                <asp:Button CssClass="botonesEvol" ID="btnImportarRemito" runat="server" Text="Importar Remito" OnClick="btnImportarRemito_Click" />
                                <asp:Button CssClass="botonesEvol" ID="btnImportarFactura" runat="server" Visible="false" Text="Importar Factura" OnClick="btnImportarFactura_Click" />
                                <asp:Button CssClass="botonesEvol" ID="btnOrdenesCompras" runat="server" Visible="false" Text="Ordenes Compras Socios" OnClick="btnOrdenesCompras_Click" />
                                <AUGE:popUpBuscarSolicitudPago ID="ctrBuscarSolPago" runat="server" />
                                <AUGE:popUpBuscarOrdenesCompras ID="ctrBuscarOrdenesCompras" runat="server" />
                                <AUGE:popUpBuscarProducto ID="ctrBuscarProductoPopUp" runat="server" />
                                <AUGE:popUpImportarRemito ID="ctrImportarRemito" runat="server" />
                                <AUGE:popUpCentrosCostos ID="ctrCentrosCostos" runat="server" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadDecimales" runat="server" Text="Decimales Precio:"></asp:Label>
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlCantidadDecimales" runat="server" OnSelectedIndexChanged="ddlCantidadDecimales_OnClick" AutoPostBack="true" Enabled="false"></asp:DropDownList>
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCentrosCostosAsignar" runat="server" Text="Repetir Centro de Costo"></asp:Label>
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlCentrosCostosAsignar" runat="server" Enabled="false"></asp:DropDownList>
                                <div class="col-sm-4"></div>
                            </div>
                        </div>
                        <div class="table-responsive">
                            <asp:GridView ID="gvItems" AllowSorting="false" AllowPaging="false"
                                OnRowCommand="gvItems_RowCommand" DataKeyNames="IndiceColeccion"
                                runat="server" AutoGenerateColumns="false" ShowFooter="true"
                                OnRowDataBound="gvItems_RowDataBound" SkinID="GrillaResponsive">
                                <Columns>
                                    <asp:TemplateField HeaderText="Código - Producto" SortExpression="">
                                        <ItemTemplate>
                                            <%--                                        <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigo" AutoPostBack="true" runat="server" Text='<%#Bind("Producto.IdProducto") %>' OnTextChanged="txtCodigo_TextChanged"></AUGE:NumericTextBox>--%>
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlProducto" runat="server" Enabled="false"></asp:DropDownList>
                                            <asp:HiddenField ID="hdfProductoDetalle" Value='<%#Bind("Producto.Descripcion") %>' runat="server" />
                                            <asp:HiddenField ID="hdfIdProducto" Value='<%#Bind("Producto.IdProducto") %>' runat="server" />
                                            <asp:Label CssClass="col-form-label" ID="lblProducto" Visible="false" Enabled="false" Text='<%#Bind("Producto.IdProducto") %>' runat="server"></asp:Label>
                                            <asp:Label CssClass="col-form-label" ID="lblProductoDescripcion" Visible="false" Text='<%#Bind("DescripcionProducto")%>' runat="server"></asp:Label>
                                            <asp:TextBox CssClass="form-control" placeholder="Ingrese un detalle..." ID="txtDescripcionProducto" Enabled="false" Text='<%#Bind("DescripcionProducto") %>' runat="server"></asp:TextBox>
                                            <asp:HiddenField ID="hdfDescripcionProducto" Value='<%#Bind("DescripcionProducto") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="U.M." ItemStyle-Width="8%" SortExpression="">
                                        <ItemTemplate>
                                            <%#Eval("Producto.UnidadMedida.Descripcion") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Cant.&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" SortExpression="">
                                        <ItemTemplate>
                                            <Evol:CurrencyTextBox CssClass="form-control" Prefix="" ID="txtCantidad" runat="server" Text='<%#Bind("Cantidad") %>'></Evol:CurrencyTextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Importe&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" SortExpression="">
                                        <ItemTemplate>
                                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtPrecioUnitario" AllowNegative="true" Text='<%#Bind("PrecioUnitarioSinIva") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;No Gravado&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" SortExpression="">
                                        <ItemTemplate>
                                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtPrecioNoGravado" runat="server" Text='<%#Bind("PrecioNoGravado", "{0:C2}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Descuento&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" SortExpression="">
                                        <ItemTemplate>
                                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtDescuentoImporte" runat="server" Text='<%#Bind("DescuentoImporte", "{0:C2}") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Subtotal" SortExpression="" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSubtotal" runat="server" Text='<%#Bind("Subtotal", "{0:C2}") %>'></asp:Label>
                                            <%--<AUGE:NumericTextBox CssClass="textboxEvol" ID="txtSubtotal" runat="server" Text='<%#Bind("SubTotal", "{0:C2}") %>' Enabled="false" Width="50"></AUGE:NumericTextBox>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Alícuota" SortExpression="" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlAlicuotaIVA" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IVA" SortExpression="" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteIva" runat="server" Text='<%#Bind("ImporteIvaTotal", "{0:C2}") %>'></asp:Label>
                                            <%--<AUGE:NumericTextBox CssClass="textboxEvol" ID="txtImporteIva" runat="server" Text='<%#Bind("ImporteIVA", "{0:N2}") %>'  Enabled="false" Width="50"></AUGE:NumericTextBox>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Subtotal" SortExpression="" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSubtotalConIva" runat="server" Text='<%#Bind("PrecioTotalItem", "{0:C2}") %>'></asp:Label>
                                            <%--<font face="arial" size="5" color="red"></Font>--%>
                                            <%--<asp:TextBox CssClass="textboxEvol" ID="txtSubtotalConIva" runat="server" Text='<%#Bind("SubTotalConIva", "{0:N2}") %>' Enabled="false" Width="50"></asp:TextBox>--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Filial" SortExpression="" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlFiliales" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Centro de Costo" SortExpression="" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlCentrosCostos" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="AgregarCentrosCostos" ID="btnAgregarCentrosCostos"
                                                AlternateText="Agregar Centros de Costos" ToolTip="Agregar Centros de Costos" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="ConsultarCentrosCostos" ID="btnConsultarCentrosCostos"
                                                AlternateText="Ver detalle Centros de Costos" ToolTip="Ver detalle Centros de Costos" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" SortExpression="">
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



    <%-- Insertar Grilla --%>
    <asp:UpdatePanel ID="upPercepciones" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <div class="col-sm-12">
                    <asp:Button CssClass="botonesEvol" ID="AgregarPercepcion" runat="server" Text="Agregar Percepcion" Visible="false" OnClick="btnAgregarPercepcion_Click" />
                </div>
            </div>
            <div class="tabla-responsive">
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

                    <asp:TemplateField HeaderText="Importe Percepcion" SortExpression="">
                        <ItemTemplate>
                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtImportePercepcion" runat="server" Text='<%#Bind("Importe", "{0:N2}") %>' />
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
            </asp:GridView></div>
        </ContentTemplate>

    </asp:UpdatePanel>
    <%-- HERE --%>

    <asp:UpdatePanel ID="pnTotales" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblTotalSinIva" runat="server" Text="Subtotal" />
                <div class="col-sm-3">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtTotalSinIva" Enabled="false" runat="server"/>
                </div>
            </div>
            <asp:Panel ID="pnlDescuento" Visible="false" runat="server">
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblDescuentoTotal" runat="server" Text="Descuento" />
                    <div class="col-sm-3">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtDescuentoTotal" Enabled="true" runat="server" />
                    </div>
                </div>
            </asp:Panel>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblTotalIva" runat="server" Text="IVA" />
                <div class="col-sm-3">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtTotalIva" Enabled="false" runat="server" />
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblTotal" runat="server" Text="Total" />
                <div class="col-sm-3">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtTotalConIva" Enabled="false" runat="server" />
                    <asp:HiddenField ID="hdfTotalConIva" runat="server" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>



    <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <AUGE:AsientoMostrar ID="ctrAsientoMostrar" runat="server" />
            <center>
                    <%--<AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />--%>
                  <auge:popUpGrillaGenerica ID="ctrPopUpGrilla" runat="server" />
                                 <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                          onclick="btnImprimir_Click"  AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                    <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar Comprobante" Visible="false" onclick="btnAgregar_Click" />
                    <asp:Button CssClass="botonesEvol" ID="btnAgregarOP" runat="server" Text="Agregar Orden de Pago" Visible="false" onclick="btnAgregarOP_Click" />
                </center>
        </ContentTemplate>
    </asp:UpdatePanel>

