<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrdenesCobrosFacturasDatos.ascx.cs" Inherits="IU.Modulos.Cobros.Controles.OrdenesCobrosFacturasDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/ClientesDatosCabeceraAjax.ascx" TagName="BuscarClienteAjax" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Cobros/Controles/OrdenesCobrosValores.ascx" TagName="OrdenesCobrosValores" TagPrefix="auge" %>

<%@ Register Src="~/Modulos/Contabilidad/Controles/AsientosDatosMostrar.ascx" TagName="AsientoMostrar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/FechaCajaContable.ascx" TagName="FechaCajaContable" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" TagName="popUpGrillaGenerica" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpEnviarMail.ascx" TagName="popUpEnviarMail" TagPrefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpComprobantes.ascx" tagname="popUpComprobantes" tagprefix="auge" %>

<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />

<script language="javascript" type="text/javascript">

    $(document).ready(function () {
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CopmletarCerosComprobantes);
        SetTabIndexInput();
        CopmletarCerosComprobantes();
        $("input[type=text][id$='txtCodigoSocio']").focus();

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

        //Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(EndRequestHandlerCalcularItem);
        //Registro el evento qeu se dispara al finalizar un postback
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerCalcularItem);
    });

    function SetearFechaComprobante() {
        $("input[type=text][id$='txtFechaCajaContabilizacion']").val($("input[type=text][id$='txtFecha']").val());
    }

    function CopmletarCerosComprobantes() {
        $("input[type=text][id$='txtPrefijoNumeroRecibo']").blur(function () { $(this).addLeadingZeros(4); });
        $("input[type=text][id$='txtNumeroRecibo']").blur(function () { $(this).addLeadingZeros(8); });
        $("input[type=text][id$='txtNumeroRecibo']").blur(function () { $(this).addLeadingZeros(8); });
    }

    function SetTabIndexInput() {
        $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }

    //Funcion que se ejecuta al finalizar un postback
    function EndRequestHandlerCalcularItem(sender, args) {
        //if(sender._postBackSettings.sourceElement.id.includes("OrdenesCobrosValores")){
        CalcularItem();
        //}
    }

    function CalcularItem() {
        var importeValores = gvValoresCalcularImporteTotal();
        var importeCobros = importeValores;
        var importeTotalPagar = 0.00;
        var importeNotasCreditos = 0.00;
        var importeTotalRetenciones = 0.00;
        var importeTotalAnticipos = 0.00;

        //Retenciones
        $('#<%=gvRetenciones.ClientID%> tr').not(':first').not(':last').each(function () {
            var importeRetencion = $(this).find('input:text[id*="txtImporteRetencion"]').maskMoney('unmasked')[0];
            if (importeRetencion > 0) {
                importeTotalRetenciones += parseFloat(importeRetencion);
            }
        });
        importeValores += parseFloat(importeTotalRetenciones);
        importeCobros += parseFloat(importeTotalRetenciones);
        $("#<%=gvRetenciones.ClientID %> [id$=lblImporteTotalRetencion]").text(accounting.formatMoney(importeTotalRetenciones, gblSimbolo, 2, "."));

        //Primero las NOTAS DE CREDITO
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            var incluir = $(this).find('input:checkbox[id*="chkIncluir"]').is(":checked");
            //var importeParcial = $(this).find('input:text[id*="txtImporteParcial"]').maskMoney('unmasked')[0];
            var importeParcial = $(this).find("input[id*='hdfImporteParcial']").val().replace('.', '').replace(',', '.');
            var importePagar = 0.00;//$(this).find('input:text[id*="txtImportePagar"]').maskMoney('unmasked')[0]; //$("td:eq(4)", this).html();
            if (importeParcial < 0) {
                if (incluir) {
                    importePagar = importeParcial;
                    importeValores += importeParcial * -1;
                    importeNotasCreditos += parseFloat(importeParcial);
                }
                $(this).find('input:text[id*="txtImportePagar"]').val(accounting.formatMoney(importePagar, gblSimbolo, 2, "."));
            }
        });

        //Aplico Anticipos
        //importeValores += parseFloat(importeTotalAnticipos);
        //        if(parseFloat(importeTotalAnticipos) > 0) {
        //            importeValores -= parseFloat(importeTotalAnticipos);
        //        }
        importeTotalAnticipos = 0;
        $('#<%=gvAnticipos.ClientID%> tr').not(':first').not(':last').each(function () {
            var incluir = $(this).find('input:checkbox[id*="chkIncluir"]').is(":checked");
            var importeAnticipo = $(this).find('input:text[id*="txtImporteAnticipo"]').maskMoney('unmasked')[0];
            var hdfImpAnticipo = $(this).find("input[id*='hdfImporteAnticipo']").val().replace('.', '').replace(',', '.');
            if (parseFloat(importeAnticipo) > parseFloat(hdfImpAnticipo)) {
                importeAnticipo = parseFloat(hdfImpAnticipo);
                $(this).find('input:text[id*="txtImporteAnticipo"]').val(accounting.formatMoney(importeAnticipo, gblSimbolo, 2, "."));
            }
            if (incluir && parseFloat(importeAnticipo) > 0) {
                importeTotalAnticipos += parseFloat(importeAnticipo);
            }
            $("#<%=gvAnticipos.ClientID %> [id$=lblImporteTotalAnticipo]").text(accounting.formatMoney(importeTotalAnticipos, gblSimbolo, 2, "."));
        });
        importeValores += parseFloat(importeTotalAnticipos);
        //        alert (importeTotalAnticipos);
        //        alert(importeValores);
        //FACTURAS
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            var incluir = $(this).find('input:checkbox[id*="chkIncluir"]').is(":checked");
            //var importeParcial = $(this).find('input:text[id*="txtImporteParcial"]').maskMoney('unmasked')[0];
            var importeParcial = $(this).find("input[id*='hdfImporteParcial']").val().replace('.', '').replace(',', '.');
            var importePagar = 0.00;//$(this).find('input:text[id*="txtImportePagar"]').maskMoney('unmasked')[0]; //$("td:eq(4)", this).html();
            if (importeParcial > 0) {
                if (incluir && importeValores > 0) {
                    importePagar = importeValores > importeParcial ? importeParcial : importeValores;
                    //$(this).find('input:text[id*="txtImportePagar"]').val(accounting.formatMoney(importePagar, gblSimbolo, 2, "."));
                    importeValores -= parseFloat(importePagar);
                    importeTotalPagar += parseFloat(importePagar);
                }
                $(this).find('input:text[id*="txtImportePagar"]').val(accounting.formatMoney(importePagar, gblSimbolo, 2, "."));
            }
        });

        //QUITAR ANTICIPO EN CERO
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            var incluir = $(this).find('input:checkbox[id*="chkIncluir"]').is(":checked");
            var importePagar = $(this).find('input:text[id*="txtImportePagar"]').maskMoney('unmasked')[0];
            var esAnticipo = $(this).find("input[id*='hdfEsAnticipo']").val();
            if (esAnticipo.toLowerCase() == "true" && importePagar == 0) {
                //llamar funcion
                $('#<%=btnEliminarAnticipo.ClientID%>').click();
            }
        });

        importeTotalPagar = parseFloat(importeTotalPagar) + parseFloat(importeNotasCreditos);
        $("#<%=gvDatos.ClientID %> [id$=lblImporteSubTotalPagar]").text(accounting.formatMoney(importeTotalPagar, gblSimbolo, 2, "."));

        //Totales
        $("input[type=text][id$='txtSubTotal']").val(accounting.formatMoney(importeCobros, gblSimbolo, 2, "."));
        $("input[type=text][id$='txtImporteRetenciones']").val(accounting.formatMoney(importeTotalRetenciones, gblSimbolo, 2, "."));
        $("input[type=text][id$='txtTotalCobrar']").val(accounting.formatMoney(parseFloat(importeCobros) - parseFloat(importeTotalRetenciones), gblSimbolo, 2, "."));
    }

     //function CheckRow(objRef) {
     //       //Get the Row based on checkbox
     //       var row = objRef.parentNode.parentNode;
     //       //Get the reference of GridView
     //       var GridView = row.parentNode;
     //       //Get all input elements in Gridview
     //       var inputList = GridView.getElementsByTagName("input");
     //       for (var i = 0; i < inputList.length; i++) {
     //           //The First element is the Header Checkbox
     //           var headerCheckBox = inputList[0];
     //           //Based on all or none checkboxes
     //           //are checked check/uncheck Header Checkbox
     //           var checked = true;
     //           if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
     //               if (!inputList[i].checked) {
     //                   checked = false;
     //                   break;
     //               }
     //           }
     //       }
     //       headerCheckBox.checked = checked;
     //   }

    //    function checkAllRow(objRef) {
    //        var GridView = objRef.parentNode.parentNode.parentNode;
    //        var inputList = GridView.getElementsByTagName("input");
    //        for (var i = 0; i < inputList.length; i++) {
    //            //Get the Cell To find out ColumnIndex
    //            var row = inputList[i].parentNode.parentNode;
    //            if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
    //                if (objRef.checked) {
    //                    if (inputList[i].disabled == false) {
    //                        inputList[i].checked = true;
    //                    }
    //                    else {
    //                        inputList[i].checked = false;
    //                    }
    //                }
    //                else {
    //                    inputList[i].checked = false;
    //                }
    //            }
    //        }
    //}

    /*gridViewId va ser la grilla en la que deseo que suceda el selectallrows*/ 
    var gridViewId = '#<%= gvDatos.ClientID %>';

    function checkAllRow(selectAllCheckbox) {
        //get all checkboxes within item rows and select/deselect based on select all checked status
        //:checkbox is jquery selector to get all checkboxes
        $('td :checkbox', gridViewId).prop("checked", selectAllCheckbox.checked);
        updateSelectionLabel();
    }
    function CheckRow(selectCheckbox) {
        //if any item is unchecked, uncheck header checkbox as well
        if (!selectCheckbox.checked)
            $('th :checkbox', gridViewId).prop("checked", false);
        updateSelectionLabel();
    }
    function updateSelectionLabel() {
        //update the caption element with the count of selected items. 
        //:checked is jquery selector to get list of checked checkboxes
        $('caption', gridViewId).html($('td :checkbox:checked', gridViewId).length + " options selected");
    }

</script>

<%--<asp:UpdatePanel ID="upEntidades" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="card">
            <div class="card-header">
                Datos del Cliente
            </div>
            <div class="card-body">
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoSocio" runat="server" Text="Codigo Cliente" />
                    <div class="col-sm-2">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigoSocio" AutoPostBack="true" OnTextChanged="txtCodigoSocio_TextChanged" runat="server" Enabled="false" />
                    </div>
                    <div class="col-sm-1">
                        <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="BuscarCliente" ID="btnBuscarSocio" Visible="true" Enabled="false"
                            AlternateText="Buscar socio" ToolTip="Buscar" OnClick="btnBuscarCliente_Click" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuil" runat="server" Text="CUIT"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtCuil" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRazonSocial" runat="server" Text="Razon Social"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtRazonSocial" Enabled="false" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalle" runat="server" Text="Detalle" />
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtDetalle" Enabled="false" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <AUGE:popUpBuscarCliente ID="ctrBuscarClientePopUp" runat="server" />
    </ContentTemplate>
</asp:UpdatePanel>--%>
<auge:BuscarClienteAjax Id="ctrBuscarCliente" runat="server" />
<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblOrdenCobro" runat="server" Text="Numero"></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control" ID="txtOrdenCobro" Enabled="false" runat="server"></asp:TextBox>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFecha" runat="server" Text="Fecha"></asp:Label>
    <div class="col-sm-3">
        <asp:TextBox CssClass="form-control datepicker" ID="txtFecha" Enabled="false" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFecha" ControlToValidate="txtFecha" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>
    <AUGE:FechaCajaContable ID="ctrFechaCajaContable" LabelFechaCajaContabilizacion="Fecha de Cobro" runat="server" />
</div>

<asp:UpdatePanel ID="upTipoOperacion" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo de Operación" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlTipoOperacionOC" runat="server" AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="ddlTipoOperacionOC_SelectedIndexChanged" />
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoOperacion" runat="server" ControlToValidate="ddlTipoOperacionOC"
                    ErrorMessage="*" ValidationGroup="Aceptar" />
            </div>
              <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlMoneda" runat="server" Enabled="false" OnSelectedIndexChanged="ddlMoneda_OnSelectedIndexChanged" AutoPostBack="true"/>
                       <asp:RequiredFieldValidator CssClass="Validador" ID="rfvMoneda" runat="server" ControlToValidate="ddlMoneda"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialCobro" runat="server" Text="Filial Cobro" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlFilialCobro" runat="server" Enabled="false" />
            </div>
                </div>
          <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroRemito" runat="server" Text="Número Recibo"></asp:Label>
            <div class="col-sm-1">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtPrefijoNumeroRecibo" Enabled="false" runat="server" MaxLength="4" />
            </div>
            <div class="col-sm-2">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroRecibo" Enabled="false" runat="server" MaxLength="8" />
            </div>
    </div>
    </ContentTemplate>
</asp:UpdatePanel>

<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalleOrden" runat="server" Text="Detalle"></asp:Label>
    <div class="col-sm-7">
        <asp:TextBox CssClass="form-control" ID="txtDetalleOrden" TextMode="MultiLine" runat="server"></asp:TextBox>
        <%--<asp:RequiredFieldValidator ID="rfvDetalle" ValidationGroup="OrdenesCobrosDatos" ControlToValidate="txtDetalle" CssClass="Validador" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
    </div>
    <div class="col-sm-4"></div>
</div>

<asp:Panel ID="pnlComprobantes" GroupingText="Detalle de Comprobantes" runat="server">
    <asp:UpdatePanel ID="upOrdenPagoDetalle" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="table-responsive">
            <asp:GridView ID="gvDatos" AllowPaging="false" AllowSorting="false"
                OnRowDataBound="gvDatos_RowDataBound" OnRowCommand="gvDatos_RowCommand" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true">
                <Columns>
                    <asp:TemplateField HeaderText="Detalle" SortExpression="Detalle">
                        <ItemTemplate>
                            <%# Eval("Detalle")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha Cbte." SortExpression="Factura.FechaFactura">
                        <ItemTemplate>
                            <%# Convert.ToInt32(Eval("Factura.IdFactura")) > 0 ? Eval("Factura.FechaFactura", "{0:dd/MM/yyyy}") : string.Empty %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Importe Cobrado" SortExpression="Importe" ItemStyle-Wrap="false">
                <ItemTemplate>
                    <%# Eval("Factura.ImporteParcialCobrado", "{0:C2}")%>
                </ItemTemplate>
            </asp:TemplateField>--%>
                     <asp:TemplateField HeaderText="Importe Cbte." SortExpression="Importe" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# string.Concat(Eval("Factura.Moneda.Moneda"), Eval("Factura.ImporteTotal", "{0:N2}"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="ImporteTotal" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:HiddenField ID="hdfImporteParcial" Value='<%#Bind("Factura.ImporteParcial") %>' runat="server" />
                            <asp:HiddenField ID="hdfEsAnticipo" Value='<%#Bind("EsAnticipo") %>' runat="server" />
                            <%--<Evol:CurrencyTextBox CssClass="gvTextBox" ID="txtImporteParcial" Visible="false" Enabled="false"  runat="server" Text='<%#Bind("Factura.ImporteParcial", "{0:C2}") %>'></Evol:CurrencyTextBox>--%>
                             <%# string.Concat(Eval("Factura.Moneda.Moneda"), Eval("Factura.ImporteParcial", "{0:N2}"))%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label CssClass="gvLabelMoneda labelFooterEvol" ID="lblImporteSubTotal" runat="server"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false" HeaderText="A Cobrar" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="ImporteTotal" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtImportePagar" Visible="false" Enabled="false" AllowNegative="true" runat="server"></Evol:CurrencyTextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label CssClass="gvLabelMoneda labelFooterEvol" ID="lblImporteSubTotalPagar" runat="server"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false" HeaderText="Acciones">
                        <HeaderTemplate>
                            <asp:CheckBox ID="checkAll" runat="server" onclick="checkAllRow(this); CalcularItem();" Text="Incluir" TextAlign="Left" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIncluir" Visible="false" onclick="CheckRow(this);" Checked='<%# Convert.ToBoolean (Eval("IncluirEnOP")) %>' runat="server" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                AlternateText="Elminiar" ToolTip="Eliminar" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
                </div>
            <asp:Button CssClass="botonesEvol" runat="server" ID="btnEliminarAnticipo" Text="" Style="display: none;" OnClick="btnEliminarAnticipo_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

<asp:UpdatePanel ID="upOrdenesCobrosAnticipos" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="pnlOrdenesCobrosAnticipos" GroupingText="Detalle de Anticipos" Visible="false" runat="server">
            <div class="table-responsive">
            <asp:GridView ID="gvAnticipos" OnRowDataBound="gvAnticipos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true">
                <Columns>
                    <asp:BoundField HeaderText="Nro." DataField="IdOrdenCobro" SortExpression="IdOrdenCobro" />
                    <asp:BoundField HeaderText="Fecha" DataField="FechaEmision" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FechaEmision" />
                    <asp:BoundField HeaderText="Detalle" DataField="Detalle" SortExpression="Detalle" />
                    <asp:TemplateField HeaderText="Filial" SortExpression="Filial.Filial">
                        <ItemTemplate>
                            <%# Eval("Filial.Filial")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Importe Anticipo" DataFormatString="{0:C2}" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataField="ImporteTotal" SortExpression="ImporteTotal" />
                    <asp:TemplateField HeaderText="Importe Aplicado" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteAnticipo" Visible="false" runat="server" Text='<%#Bind("ImporteAplicar") %>'></Evol:CurrencyTextBox>
                            <asp:HiddenField ID="hdfImporteAnticipo" Value='<%#Bind("ImporteAplicar") %>' runat="server" />
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteAnticipo" runat="server" Text='<%#Bind("ImporteAplicado", "{0:C2}") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotalAnticipo" runat="server" Text="0.00"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIncluir" Visible="false" Checked='<%# Convert.ToBoolean (Eval("IncluirEnOC")) %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
                </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:Panel ID="pnlDetalleValores" GroupingText="Detalle de Valores y Descuentos" runat="server">
    <AUGE:OrdenesCobrosValores ID="ctrOrdenesCobrosValores" runat="server" />
    <asp:UpdatePanel ID="upRetenciones" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <div class="col-sm-3">
                    <asp:Button CssClass="botonesEvol" ID="btnAgregarRetencion" runat="server" Text="Agregar Retencion" Visible="false" OnClick="btnAgregarRetencion_Click" />
                </div>
                <div class="col-sm-8"></div>
            </div>
            <div class="table-responsive">
            <asp:GridView ID="gvRetenciones" AllowPaging="false" AllowSorting="false"
                OnRowDataBound="gvRetenciones_RowDataBound" OnRowCommand="gvRetenciones_RowCommand" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true">
                <Columns>
                    <asp:TemplateField HeaderText="Tipo Retencion" SortExpression="TipoRetencion.Descripcion">
                        <ItemTemplate>
                            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoRetenciones" OnSelectedIndexChanged="ddlTipoRetenciones_SelectedIndexChanged" AutoPostBack="true" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Numero Certificado" SortExpression="OrdenCobroTipoRetencion.NumeroCertificado">
                        <ItemTemplate>
                            <asp:TextBox CssClass="form-control" ID="txtNumeroCertificado" runat="server" Text='<%#Bind("NumeroCertificado") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Concepto" SortExpression="Concepto">
                        <ItemTemplate>
                            <asp:DropDownList CssClass="form-control" ID="ddlTipoConceptos" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteRetencion" OnTextChanged="txtImporteRetencion_TextChanged" AutoPostBack="true" runat="server" Text='<%#Bind("ImporteTotalRetencion", "{0:N2}") %>'></Evol:CurrencyTextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotalRetencion" runat="server" Text="0.00"></asp:Label>
                        </FooterTemplate>
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
</asp:Panel>

<asp:UpdatePanel ID="upTotales" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblSubTotal" runat="server" Text="Importe Total"></asp:Label>
            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtSubTotal" runat="server" Enabled="false" />
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblImporteRetenciones" runat="server" Text="Retenciones"></asp:Label>
            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteRetenciones" runat="server" Enabled="false" />
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblTotalCobrar" runat="server" Text="Neto a Cobrar"></asp:Label>
            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtTotalCobrar" runat="server" Enabled="false" />
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
    <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria">
        <ContentTemplate>
            <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
        </ContentTemplate>
    </asp:TabPanel>
</asp:TabContainer>
<asp:UpdatePanel ID="upAcciones" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <AUGE:AsientoMostrar ID="ctrAsientoMostrar" runat="server" />
    <div class="row justify-content-md-center">
            <div class="col-md-auto">
      
            <auge:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                   <AUGE:popUpEnviarMail ID="popUpMail" runat="server" />
                <auge:popUpGrillaGenerica ID="ctrPopUpGrilla" runat="server" />

                <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                          onclick="btnImprimir_Click"  AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                   <asp:ImageButton ImageUrl="~/Imagenes/sendmail.png" runat="server" ID="btnEnviarMail" Visible="false"
                    OnClick="btnEnviarMail_Click" AlternateText="Enviar Comprobante" ToolTip="Enviar Comprobante" />
            
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" ValidationGroup="Aceptar" onclick="btngrabar_Click"/>
            <asp:Button CssClass="botonesEvol" UseSubmitBehavior="false" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
 </div></div>
    </ContentTemplate>
</asp:UpdatePanel>
