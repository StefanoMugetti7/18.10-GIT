<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrestamoAfiliadoModificarDatos.ascx.cs" Inherits="IU.Modulos.Prestamos.Controles.PrestamoAfiliadoModificarDatos" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Prestamos/Controles/PrestamosCuotasPopUp.ascx" TagPrefix="AUGE" TagName="PrestamosCuotasPopUp" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpEnviarMail.ascx" TagName="popUpEnviarMail" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Prestamos/Controles/PrestamosChequesDatos.ascx" TagPrefix="AUGE" TagName="PrestamosCheques" %>

<script src="https://cdnjs.cloudflare.com/ajax/libs/clipboard.js/2.0.4/clipboard.min.js"></script>
<script lang="javascript" type="text/javascript">

    $(document).ready(function () {
        EstiloEstadosCuponera();
    });

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

    function CalcularItem() {
        var subTotal = 0.00;
        $('#<%=gvCuentaCorriente.ClientID%> tr').not(':first').not(':last').each(function () {
            //var incluir = $("td:eq(6) :checkbox", this).is(":checked");
            var incluir = $(this).find('input:checkbox[id*="chkIncluir"]').is(":checked");
            //var importe = $(this).find('span[id$="lblImporte"]').text();
            var importe = $("td:eq(4)", this).html();
            if (incluir && importe) {
                importe = importe.replace('$', '').replace('.', '').replace(',', '.'); //remplazo . por nada y , por .
                subTotal += parseFloat(importe);
            }
        });
        $("input[type=text][id$='txtImporteExedido']").val(accounting.formatMoney(subTotal, gblSimbolo, gblCantidadDecimales, "."));
        $("#<%=gvCuentaCorriente.ClientID %> [id$=lblImporteTotal]").text(accounting.formatMoney(subTotal, gblSimbolo, gblCantidadDecimales, "."));
    }

    function CalcularSolicitudPago() {
        var subTotal = 0.00;
        $('#<%=gvSolicitudesPagos.ClientID%> tr').not(':first').not(':last').each(function () {
            //var incluir = $("td:eq(6) :checkbox", this).is(":checked");
            var incluir = $(this).find('input:checkbox[id*="chkIncluir"]').is(":checked");
            //var importe = $(this).find('span[id$="lblImporte"]').text();
            var importe = $("td:eq(4)", this).html();
            if (incluir && importe) {
                importe = importe.replace('$', '').replace('.', '').replace(',', '.'); //remplazo . por nada y , por .
                subTotal += parseFloat(importe);
            }
        });
        $("input[type=text][id$='txtImporteSolicitudesPagos']").val(accounting.formatMoney(subTotal, gblSimbolo, gblCantidadDecimales, "."));
        $("#<%=gvSolicitudesPagos.ClientID %> [id$=lblImporteTotal]").text(accounting.formatMoney(subTotal, gblSimbolo, gblCantidadDecimales, "."));
    }

    function CalcularCancelaciones() {
        var subTotal = 0.00;
        $('#<%=gvCancelaciones.ClientID%> tr').not(':first').not(':last').each(function () {
            //var incluir = $("td:eq(6) :checkbox", this).is(":checked");
            var incluir = $(this).find('input:checkbox[id*="chkIncluir"]').is(":checked");
            //var importe = $(this).find('span[id$="lblImporte"]').text();
            //var importe = $("td:eq(4)", this).html();
            var importe = $(this).find("input:text[id*='txtImporteCancelacion']").maskMoney('unmasked')[0];
            if (incluir && importe) {
                //importe = importe.replace('$', '').replace('.', '').replace(',', '.'); //remplazo . por nada y , por .
                subTotal += parseFloat(importe);
            }
        });
        $("input[type=text][id$='txtImporteCancelaciones']").val(accounting.formatMoney(subTotal, gblSimbolo, gblCantidadDecimales, "."));
        $("#<%=gvCancelaciones.ClientID %> [id$=lblImporteTotal]").text(accounting.formatMoney(subTotal, gblSimbolo, gblCantidadDecimales, "."));
    }


    function CheckRow(objRef) {
        //Get the Row based on checkbox
        var row = objRef.parentNode.parentNode;
        //Get the reference of GridView
        var GridView = row.parentNode;
        //Get all input elements in Gridview
        var inputList = GridView.getElementsByTagName("input");
        for (var i = 0; i < inputList.length; i++) {
            //The First element is the Header Checkbox
            var headerCheckBox = inputList[0];
            //Based on all or none checkboxes
            //are checked check/uncheck Header Checkbox
            var checked = true;
            if (inputList[i].type == "checkbox" && inputList[i]
                != headerCheckBox) {
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
        var inputList = GridView.getElementsByTagName("input");
        for (var i = 0; i < inputList.length; i++) {
            //Get the Cell To find out ColumnIndex
            var row = inputList[i].parentNode.parentNode;
            if (inputList[i].type == "checkbox" && objRef
                != inputList[i]) {
                if (objRef.checked) {
                    inputList[i].checked = true;
                }
                else {
                    inputList[i].checked = false;
                }
            }
        }
    }

    function EstiloEstadosCuponera() {
        $('#<%=gvDatos.ClientID%> tr').each(function () {
            var idestado = $(this).find("input[id*='hdfIdEstado']").val();
            var image = $(this).find("img[id*='imgEstado']");
            if (idestado == '24' || idestado == '15' || idestado == '77') {
                image.attr('src', '<%=ResolveClientUrl("~")%>/Imagenes/' + idestado + '.png');
                if (idestado == '24') {
                    $(this).find('td').css('color', 'DarkGreen')
                }
                else {
                    $(this).find('td').css('color', 'Red')
                }

                image.show();
            }
            else {
                image.hide();
            }
        });

        //$('#<%=gvDatos.ClientID%> tr:even').each(function () {
        //    var idestado = $(this).find("input[id*='hdfIdEstado']").val();
        //    if (idestado == '24') {
        //        $(this).css('background-color', 'SpringGreen')
        //        //$(this).find('td').css('color', 'ForestGreen')
        //    }
        //});
    }

</script>

<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />

<asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="form-group row">
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvNumeroIdentificacion">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblNumeroIdentificacion" runat="server" Text="Nro de Prestamo" />
                    <div class="col-sm-9">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroIdentificacion" Enabled="false" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFechaAlta">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaAlta" runat="server" Text="Fecha Alta" />
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaAlta" Enabled="false" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvEstado">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" Enabled="false" />
                    </div>
                </div>
            </div>

            <%--    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>--%>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvTipoOperacion">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo de Operación" />
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoOperacion" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoOperacion_SelectedIndexChanged" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvTipoCargo">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTipoCargo" runat="server" Text="Tipo de Cargo" />
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTiposCargos" AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="ddlTiposCargos_SelectedIndexChanged" runat="server" />
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTiposCargos" runat="server" ControlToValidate="ddlTiposCargos"
                            ErrorMessage="*" ValidationGroup="Aceptar" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFormasCobro">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFormasCobro" runat="server" Text="Forma de Cobro" />
                    <div class="col-sm-6">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlFormasCobros" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFormasCobros_SelectedIndexChanged" />
                        <asp:RequiredFieldValidator ID="rfvFormasCobros" runat="server" ControlToValidate="ddlFormasCobros"
                            ErrorMessage="*" CssClass="Validador" ValidationGroup="Aceptar" />
                    </div>
                    <div class="col-sm-2">
                        <button class="botonesEvol" type="button" id="btnDetalleFormaCobro" data-toggle="collapse" data-target="#collapseExampleDetalleCobros" aria-expanded="false" aria-controls="collapseExample">
                            Detalle
                        </button>
                    </div>
                </div>
            </div>
            <div class="collapse" id="collapseExampleDetalleCobros">
                <div class="card card-body row vw-100">
                    <div class="col">
                        <div class="col-md-12 col-6">
                            <asp:GridView ID="gvDetalleFormaCobro" runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="true">
                            </asp:GridView>
                        </div>
                        <div class="col-md-12 col-6">
                            <asp:GridView ID="gvFormasCobrosCodigosConceptos" runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField HeaderText="Codigo Concepto" DataField="CodigoConcepto" />
                                    <asp:BoundField HeaderText="Codigo Concepto Plan" DataField="CodigoConceptoPrestamoPlan" />
                                    <asp:BoundField HeaderText="Sub Concepto" DataField="CodigoSubConcepto" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <%--        </ContentTemplate>
    </asp:UpdatePanel>--%>

            <%--<asp:UpdatePanel ID="upPlan" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>--%>

            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvMoneda">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlMoneda" Enabled="false" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMoneda_SelectedIndexChanged" />
                        <asp:RequiredFieldValidator ID="rfvMoneda" CssClass="Validador" runat="server" ControlToValidate="ddlMoneda"
                            ErrorMessage="*" ValidationGroup="Aceptar" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvPlan">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblPlan" runat="server" Text="Plan" />
                    <div class="col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlPlan" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPlan_SelectedIndexChanged" />
                        <asp:RequiredFieldValidator ID="rfvPlan" runat="server" ControlToValidate="ddlPlan"
                            ErrorMessage="*" CssClass="Validador" ValidationGroup="Aceptar" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvTasaIntereses">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTasaInteres" runat="server" Text="Tasa Interes" />
                    <div class="col-sm-9">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtTasaInteres" Prefix="" NumberOfDecimals="4" Enabled="false" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCantidadCuotas">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCantidadCuotas" runat="server" Text="Cantidad de Cuotas" />
                    <div class="col-sm-9">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtCantidadCuotas" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvImporteCuota" visible="false">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImporteCuota" runat="server" Text="Importe Cuota" />
                    <div class="col-sm-9">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteCuota" Enabled="false" runat="server" />
                    </div>
                </div>
            </div>

            <%--    </ContentTemplate>
    </asp:UpdatePanel>--%>

            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvImporteGastos">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImporteGastos" runat="server" Text="Gastos" />
                    <div class="col-sm-9">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteGastos" Enabled="false" runat="server" />
                    </div>
                </div>
            </div>

            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvTipoValor">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTipoValor" runat="server" Text="Tipo Valor" />
                    <div class="col-sm-9">
                        <asp:UpdatePanel ID="upTipoValor" UpdateMode="Conditional" RenderMode="Inline" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList CssClass="form-control select2" ID="ddlTipoValor" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoValor_SelectedIndexChanged" />
                                <asp:RequiredFieldValidator ID="rfvTipoValor" CssClass="Validador" runat="server" ControlToValidate="ddlTipoValor"
                                    ErrorMessage="*" ValidationGroup="Aceptar" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvImporteExcedido">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImporteExcedido" runat="server" Text="Importe Cargos" />
                    <div class="col-sm-9">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteExedido" Enabled="false" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvImporteCancelaciones">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImporteCancelaciones" runat="server" Text="Cancelaciones Prestamos" />
                    <div class="col-sm-9">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteCancelaciones" Enabled="false" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvImporteSolicitudesPagos">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImporteSolicitudesPagos" runat="server" Text="Importe Servicios" />
                    <div class="col-sm-9">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteSolicitudesPagos" Enabled="false" runat="server" />
                    </div>
                </div>
            </div>

        </div>

        <%--Unidades--%>
        <div class="form-group row" id="dvUnidades" runat="server" visible="false">
        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvTipoUnidad">
            <div class="row">
                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTipoUnidad" runat="server" Text="Tipo de Unidad" />
                <div class="col-sm-9">
                    <asp:TextBox CssClass="form-control" ID="txtTipoUnidad" Enabled="false" runat="server" />
                    <asp:HiddenField ID="hdfIdTipoUnidad" runat="server" />
                </div>
            </div>
        </div>
        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvValorUnidad" >
            <div class="row">
                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblValorUnidad" runat="server" Text="Valor de Unidad" />
                <div class="col-sm-9">
                    <Evol:CurrencyTextBox CssClass="form-control" Prefix="" NumberOfDecimals="6" ID="txtValorUnidad" Enabled="false" runat="server" />
                </div>
            </div>
        </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvTotalUnidades" >
            <div class="row">
                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTotalUnidades" runat="server" Text="Total de Unidad" />
                <div class="col-sm-9">
                    <Evol:CurrencyTextBox CssClass="form-control" Prefix="" NumberOfDecimals="2" ID="txtTotalUnidades" Enabled="false" runat="server" />
                </div>
            </div>
        </div>
            </div>

        <AUGE:PrestamosCheques ID="ctrPrestamosCheques" Visible="false" runat="server" />

        <asp:UpdatePanel ID="upMonto" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div class="form-group row">
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvMonto">
                        <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblMonto" runat="server" Text="Monto Solicitado" />
                            <div class="col-sm-9">
                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtMonto" runat="server" />
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvMonto" runat="server" InitialValue="" ControlToValidate="txtMonto"
                                    ErrorMessage="*" ValidationGroup="Aceptar" />
                            </div>
                        </div>
                    </div>
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvPreAutorizar">
                        <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblPreAutorizar" Visible="false" runat="server" Text="Pre Autorizo" />
                            <div class="col-sm-9">
                                <asp:TextBox CssClass="form-control" ID="txtPreAutorizar" Visible="false" Enabled="false" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="upCapitalSocial" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:PlaceHolder ID="phCapitalSocial" Visible="false" runat="server">
                    <div class="form-group row">
                        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvPorcentajeCapitalSocial">
                            <div class="row">
                                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblPorcentajeCapitalSocial" runat="server" Text="% Capital Social" />
                                <div class="col-sm-9">
                                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtPorcentajeCapitalSocial" Enabled="false" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvImporteCapitalSocial">
                            <div class="row">
                                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImporteCapitalSocial" runat="server" Text="Capital Social" />
                                <div class="col-sm-9">
                                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteCapitalSocial" Enabled="false" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:PlaceHolder>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Panel CssClass="form-group row" ID="pnlAutorizar" Visible="false" runat="server">
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvMontoAutorizado">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblMontoAutorizado" runat="server" Text="Monto Autorizado" />
                    <div class="col-sm-9">
                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtMontoAutorizado" runat="server" Enabled="false" />
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvMontoAutorizado" Enabled="false" runat="server" ControlToValidate="txtMontoAutorizado"
                            ErrorMessage="*" ValidationGroup="Aceptar" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvAutorizar">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblAutorizar" runat="server" Text="Autorizo" />
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtAutorizo" Enabled="false" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFechaValidezAutorizado">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaValidezAutorizado" runat="server" Text="Fecha Validez Autorizado" />
                    <div class="col-sm-9">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaValidezAutorizado" Enabled="false" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaValidezAutorizado" Enabled="false" runat="server" ControlToValidate="txtFechaValidezAutorizado"
                            ErrorMessage="*" ValidationGroup="Aceptar" />
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:UpdatePanel ID="upImprtePrestamo" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div class="form-group row">
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvImportePrestamo">
                        <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImportePrestamo" runat="server" Text="Importe Prestamo" />
                            <div class="col-sm-9">
                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtImportePrestamo" Enabled="false" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFilialPago">
                        <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFilialPago" runat="server" Text="Filial de Pago" />
                            <div class="col-sm-9">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlFilialPago" runat="server" />
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFilialPago" runat="server" ControlToValidate="ddlFilialPago"
                                    ErrorMessage="*" ValidationGroup="Aceptar" />
                            </div>
                        </div>
                    </div>
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvInicioPeriodoCuotas">
                        <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblInicioPeriodoCuotas" runat="server" Text="Periodo Primer Vencimiento" />
                            <div class="col-sm-9">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlPeriodoVencimiento" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodoVencimiento_SelectedIndexChanged" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Panel ID="pnlCancelacion" Visible="false" runat="server">
            <asp:UpdatePanel ID="upCancelacion" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="form-group row">
                        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvImporteCancelar">
                            <div class="row">
                                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImporteCancelar" runat="server" Text="Importe Cancelar"></asp:Label>
                                <div class="col-sm-9">
                                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteCancelacion" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFilialCancelacion">
                            <div class="row">
                                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFilialCancelacion" runat="server" Text="Filial de Cancelación" />
                                <div class="col-sm-9">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlFilialCancelacion" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvTipoValorCancelacion">
                            <div class="row">
                                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTipoValorCancelacion" runat="server" Text="Tipo Valor Cancelación" />
                                <div class="col-sm-9">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoValorCancelacion" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>

        <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
            <asp:TabPanel runat="server" ID="tpDetalleCuotas" HeaderText="Detalle de Cuotas">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upDetalleCuotas" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive">
                                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="false" AllowSorting="false"
                                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                                    <Columns>
                                        <asp:BoundField HeaderText="Cuota" DataField="CuotaNumero" SortExpression="CuotaNumero" />
                                        <asp:TemplateField HeaderText="Vencimiento" SortExpression="CuotaFechaVencimiento">
                                            <ItemTemplate>
                                                <%# Eval("CuotaFechaVencimiento", "{0:dd/MM/yyyy}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Importe" SortExpression="ImporteCuota" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# string.Concat(Eval("Moneda"), Eval("ImporteCuota", "{0:N2}"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Interes" SortExpression="ImporteInteres" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# string.Concat(Eval("Moneda"), Eval("ImporteInteres", "{0:N2}"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amortizacion" SortExpression="ImporteAmortizacion" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# string.Concat(Eval("Moneda"), Eval("ImporteAmortizacion", "{0:N2}"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Gasto" Visible="false" SortExpression="ImporteGastoCuota" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# string.Concat(Eval("Moneda"), Eval("ImporteGastoCuota", "{0:N2}"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Seguro" Visible="false" SortExpression="ImporteSeguroCuota" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# string.Concat(Eval("Moneda"), Eval("ImporteSeguroCuota", "{0:N2}"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IVA" Visible="false" SortExpression="ImporteIvaCuota" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# string.Concat(Eval("Moneda"), Eval("ImporteIvaCuota", "{0:N2}"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Saldo Capital" SortExpression="ImporteSaldo" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# string.Concat(Eval("Moneda"), Eval("ImporteSaldo", "{0:N2}"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Importe Cobrado" SortExpression="ImporteCobrado" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# string.Concat(Eval("Moneda"), Eval("ImporteCobrado", "{0:N2}"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Unidades" Visible="false" SortExpression="UnidadesCuota" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%#Eval("UnidadesCuota", "{0:N2}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="U.Amortizacion" Visible="false" SortExpression="UnidadesAmortizacion" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# Eval("UnidadesAmortizacion", "{0:N2}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="U.Interes" Visible="false" SortExpression="UnidadesIntereses" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# Eval("UnidadesIntereses", "{0:N2}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="U.Saldo" Visible="false" SortExpression="UnidadesSaldo" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# Eval("UnidadesSaldo", "{0:N2}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Valor Unidad" Visible="false" SortExpression="Coeficiente" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# Eval("Coeficiente", "{0:N4}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Dif.Aumento" Visible="false" SortExpression="DiferenciaVariacionCoeficiente" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <%# Eval("DiferenciaVariacionCoeficiente", "{0:N2}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Image ID="imgEstado" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                            <ItemTemplate>
                                                <%# Eval("Estado.Descripcion")%>
                                                <asp:HiddenField ID="hdfIdEstado" Value='<%#Bind("Estado.IdEstado") %>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="false">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="gvDatosCheckAll" runat="server" onclick="checkAllRow(this);" Enabled="false" Text="Incluir" TextAlign="Left" AutoPostBack="true" OnCheckedChanged="gvDatosChkIncluir_CheckedChanged" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="gvDatosChkIncluir" runat="server" onclick="CheckRow(this);" Enabled="false" AutoPostBack="true" OnCheckedChanged="gvDatosChkIncluir_CheckedChanged" Checked='<%# Eval("Incluir")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpCancelaciones" HeaderText="Cancelaciones Prestamos" Visible="false">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upCancelaciones" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <AUGE:PrestamosCuotasPopUp ID="ctrPrestamosCuotasPopUp" runat="server" />
                            <div class="table-responsive">
                                <asp:GridView ID="gvCancelaciones" DataKeyNames="IndiceColeccion" OnRowDataBound="gvCancelaciones_RowDataBound" OnPageIndexChanging="gvCancelaciones_PageIndexChanging"
                                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Fecha" SortExpression="FechaEvento">
                                            <ItemTemplate>
                                                <%# Eval("FechaPrestamo", "{0:dd/MM/yyyy}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%-- <asp:TemplateField HeaderText="Autorizado" SortExpression="FechaAutorizado">
                            <ItemTemplate>
                                <%# Convert.ToDateTime(Eval("FechaAutorizado")) > Convert.ToDateTime("1900/01/01") ?
                                    Eval("FechaAutorizado", "{0:dd/MM/yyyy}") : string.Empty %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="A partir de" SortExpression="FechaValidezAutorizado">
                            <ItemTemplate>
                                <%# Convert.ToDateTime(Eval("FechaValidezAutorizado")) > Convert.ToDateTime("1900/01/01") ?
                                    Eval("FechaValidezAutorizado", "{0:dd/MM/yyyy}") : string.Empty %>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Confirmado" SortExpression="FechaConfirmacion">
                                            <ItemTemplate>
                                                <%# Convert.ToDateTime(Eval("FechaConfirmacion")) > Convert.ToDateTime("1900/01/01") ?
                                    Eval("FechaConfirmacion", "{0:dd/MM/yyyy}") : string.Empty %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="#Prestamo" DataField="NroDeIdentificacion" SortExpression="NroDeIdentificacion" />
                                        <asp:BoundField HeaderText="Importe" DataFormatString="{0:C2}" DataField="ImporteAutorizado" SortExpression="ImporteAutorizado" />
                                        <asp:BoundField HeaderText="Cuota" DataFormatString="{0:C2}" DataField="ImporteCuota" SortExpression="ImporteCuota" />
                                        <asp:BoundField HeaderText="Deuda" DataFormatString="{0:C2}" DataField="SaldoDeuda" SortExpression="SaldoDeuda" />
                                        <asp:TemplateField HeaderText="Cuotas Canceladas" SortExpression="TipoOperacion.TipoOperacion">
                                            <ItemTemplate>
                                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadCuotasCanceladas" runat="server" Text='<%#Bind("CantidadCuotasCanceladas") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField  HeaderText="Importe Cancelar"  DataFormatString="{0:C2}" DataField="ImporteCancelacion" SortExpression="ImporteCancelacion" />--%>
                                        <asp:TemplateField HeaderText="Importe Cancelar" FooterStyle-HorizontalAlign="Right" SortExpression="TipoOperacion.TipoOperacion">
                                            <ItemTemplate>
                                                <Evol:CurrencyTextBox CssClass="gvTextBox" ID="txtImporteCancelacion" Enabled="false" runat="server" Text='<%#Bind("ImporteCancelacion", "{0:C2}") %>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Tipo Operación" SortExpression="TipoOperacion.TipoOperacion">
                                            <ItemTemplate>
                                                <%# Eval("TipoOperacion.TipoOperacion")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Detalle" SortExpression="Detalle">
                                            <ItemTemplate>
                                                <%# Eval("Detalle")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                            <ItemTemplate>
                                                <%# Eval("Estado.Descripcion")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Incluir" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIncluir" Visible="false" runat="server" AutoPostBack="true" OnCheckedChanged="chkIncluir_CheckedChanged" Checked='<%# Eval("Incluir")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpCargosPendientes" HeaderText="Cancelaciones Cargos" Visible="false">
                <ContentTemplate>
                  
                     
                            <div class="table-responsive">
                            <asp:GridView ID="gvCuentaCorriente" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false"
                                ShowFooter="true" OnRowDataBound="gvCuentaCorriente_RowDataBound" OnPageIndexChanging="gvCuentaCorriente_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField HeaderText="Fecha Movimiento" DataField="FechaMovimiento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" SortExpression="FechaMovimiento" />
                                    <asp:BoundField HeaderText="Periodo" DataField="Periodo" ItemStyle-Wrap="false" SortExpression="FechaMovimiento" />
                                    <asp:TemplateField HeaderText="Tipo Cargo" SortExpression="TipoCargo.TipoCargo">
                                        <ItemTemplate>
                                            <%# Eval("TipoCargo.TipoCargo")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Concepto" DataField="Concepto" SortExpression="Concepto" />
                                    <%--<asp:TemplateField HeaderText="Tipo Movimiento" SortExpression="TipoMovimiento.TipoMovimiento">
                                <ItemTemplate>
                                    <%# Eval("TipoOperacion.TipoMovimiento.TipoMovimiento")%>
                                </ItemTemplate>
                        </asp:TemplateField>--%>
                                    <%--<asp:TemplateField HeaderText="Tipo Valor" SortExpression="TipoValor.TipoValor">
                                <ItemTemplate>
                                    <%# Eval("TipoValor.TipoValor")%>
                                </ItemTemplate>
                        </asp:TemplateField>--%>
                                    <%--<asp:BoundField  HeaderText="Importe" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="Importe" SortExpression="Importe" />--%>
                                    <asp:TemplateField HeaderText="Importe" SortExpression="Importe" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%--<asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporte" runat="server" Text='<%# Eval("Importe", "{0:C2}")%>'></asp:Label>--%>
                                            <%# Eval("Importe", "{0:C2}")%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                        <ItemTemplate>
                                            <%# Eval("Estado.Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Incluir" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIncluir" Visible="false" runat="server" Checked='<%# Eval("Incluir")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView></div>
                   
                  
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpSolicitudesPagos" HeaderText="Solicitudes de Pagos - Facturas" Visible="false">
                <ContentTemplate>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:GridView ID="gvSolicitudesPagos" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false"
                                ShowFooter="true" OnRowDataBound="gvSolicitudesPagos_RowDataBound" OnPageIndexChanging="gvSolicitudesPagos_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="Número Solicitud" SortExpression="NumeroSolicitud">
                                        <ItemTemplate>
                                            <%# Eval("IdSolicitudPago")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Proveedor" SortExpression="Proveedor">
                                        <ItemTemplate>
                                            <%# Eval("Entidad.Nombre")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Fecha Solicitud" SortExpression="FechaAlta">
                            <ItemTemplate>
                                <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Fecha Factura" SortExpression="FechaFactura">
                                        <ItemTemplate>
                                            <%# Eval("FechaFactura", "{0:dd/MM/yyyy}")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nro Factura" SortExpression="NumeroFacturaCompleto">
                                        <ItemTemplate>
                                            <%# Eval("NumeroFacturaCompleto")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Importe Total" SortExpression="ImporteTotal">
                                        <ItemTemplate>
                                            <%# Eval("ImporteTotal", "{0:C2}")%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Incluir" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIncluir" Visible="false" runat="server" Checked='<%# Eval("IncluirEnOP")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpDetalleCobros" HeaderText="Detalle de Cobros" Visible="false">
                <ContentTemplate>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:GridView ID="gvDetalleCobros" runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Fecha Envio" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <%# Eval("FechaEnvio", "{0:dd/MM/yyyy}")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Nro Lote" DataField="IdTipoCargoLoteEnviado" />
                                    <asp:BoundField HeaderText="Forma de Cobro" DataField="FormaCobro" />
                                    <asp:BoundField HeaderText="Concepto" DataField="Concepto" />
                                    <asp:BoundField HeaderText="Importe Enviado" DataFormatString="{0:C2}" DataField="ImporteEnviado" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" />
                                    <asp:BoundField HeaderText="Importe Cobrado" DataFormatString="{0:C2}" DataField="ImporteImputado" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" />
                                    <asp:TemplateField HeaderText="Estado">
                                        <ItemTemplate>
                                            <%# Eval("Estado")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpDocumentosAsociados" Visible="false" HeaderText="Documentos Asociados">
                <ContentTemplate>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:GridView ID="gvDocumentosAsociados" OnRowCommand="gvDocumentosAsociados_RowCommand" AllowPaging="false" AllowSorting="false"
                                OnRowDataBound="gvDocumentosAsociados_RowDataBound"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Fecha Comprobante">
                                        <ItemTemplate>
                                            <%# Eval("FechaComprobante", "{0:dd/MM/yyyy}")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Numero Comprobante" DataField="NumeroComprobante" />
                                    <asp:BoundField HeaderText="Importe" DataFormatString="{0:C2}" DataField="Importe" />
                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                                AlternateText="Mostrar" ToolTip="Mostrar" />
                                            <asp:HiddenField ID="hdfIdTipoOperacion" runat="server" Value='<%#Eval("IdTipoOperacion") %>' />
                                            <asp:HiddenField ID="hdfIdRefTipoOperacion" runat="server" Value='<%#Eval("IdRefTipoOperacion") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
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
        <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
        <AUGE:popUpEnviarMail ID="popUpMail" runat="server" />
        <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:HiddenField ID="hfLinkFirmarDocumento" runat="server" />
                <button runat="server" visible="false" type="button" id="copyClipboard" data-tooltip="Se ha copiado el link" class="botonesEvol" onclick="CopyClipboard()">Copiar link</button>
                <asp:ImageButton CssClass="btn" ImageUrl="~/Imagenes/whatsup26x26.jpg" runat="server" ID="btnWhatsAppFirmarDocumento" Visible="false"
                    OnClick="btnWhatsAppFirmarDocumento_Click" AlternateText="Enviar Whatsapp para Firmar" ToolTip="Enviar Whatsapp para Firmar" />
                <asp:ImageButton CssClass="btn" ImageUrl="~/Imagenes/sendmail.png" runat="server" ID="btnFirmarDocumento" Visible="false"
                    OnClick="btnFirmarDocumento_Click" AlternateText="Enviar Mail para Firmar" ToolTip="Enviar Mail para Firmar" />
                <asp:Button CssClass="botonesEvol" ID="btnFirmarDocumentoBaja" Visible="false" runat="server" Text="Eliminar Firma" OnClick="btnFirmarDocumentoBaja_Click" />
                <asp:ImageButton CssClass="btn" ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                    OnClick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnVolver" Visible="false" runat="server" Text="Volver" OnClick="btnVolver_Click" CausesValidation="false" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
