<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CancelacionAnticipadaPrestamosCuotas.ascx.cs" Inherits="IU.Modulos.Cobros.Controles.CancelacionAnticipadaPrestamosCuotas" %>
<%--<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>--%>
<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Cobros/Controles/OrdenesCobrosValores.ascx" TagName="OrdenesCobrosValores" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/FechaCajaContable.ascx" TagName="FechaCajaContable" TagPrefix="auge" %>

<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
<div class="CancelacionAnticipadaPrestamosCuotas">
    <script language="javascript" type="text/javascript">
        function CalcularItem() {
            var importeTotal = 0.00;
            $('#<%=gvCuentaCorriente.ClientID%> tr').each(function () {
                var incluir = $(this).find('input:checkbox[id*="chkIncluir"]').is(":checked");
                var importe = $(this).find('input:text[id*="txtACobrar"]').maskMoney('unmasked')[0];
                //            alert($(this).find('input[id*="hdfImporteTope"]').val());
                //            var importeTope = $(this).find('input[id*="hdfImporteTope"]').val().replace('.', '').replace(',', '.');
                //            if (parseFloat(importe) > parseFloat(importeTope)) {
                //                importe = parseFloat(importeTope);
                //                $(this).find('input:text[id*="txtACobrar"]').val(accounting.formatMoney(importe, gblSimbolo, 2, "."));
                //            }
                if (incluir && importe) {
                    importeTotal += parseFloat(importe);
                }
            });
            $("#<%=gvCuentaCorriente.ClientID %> [id$=lblTotalACobrar]").text(accounting.formatMoney(importeTotal, "$ ", 2, "."));
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
    </script>

    <asp:UpdatePanel ID="upArmarCobros" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtTipoOperacion" Enabled="false" runat="server"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblOrdenCobro" runat="server" Text="Numero"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtOrdenCobro" Enabled="false" runat="server"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFecha" runat="server" Text="Fecha"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtFecha" Enabled="false" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialCobro" runat="server" Text="Filial Cobro" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlFilialCobro" runat="server" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalle" runat="server" Text="Detalle"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtDetalle" TextMode="MultiLine" runat="server"></asp:TextBox>
                </div>
            </div>
            <asp:GridView ID="gvCuentaCorriente" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false"
                ShowFooter="true" OnRowDataBound="gvCuentaCorriente_RowDataBound">
                <Columns>
                    <asp:BoundField HeaderText="Detalle" DataField="Detalle" />
                    <asp:TemplateField HeaderText="Importe" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:Label CssClass="gvLabelMoneda" ID="lblImporte" Visible="false" runat="server" Text='<%#Bind("Importe", "{0:C2}") %>'></asp:Label>
                            <asp:HiddenField ID="hdfImporteTope" Value='<%#Bind("Importe") %>' runat="server" />
                            <Evol:CurrencyTextBox CssClass="gvTextBox" ID="txtACobrar" Enabled="false" Visible="false" runat="server" Text='<%#Bind("Importe", "{0:C2}") %>'></Evol:CurrencyTextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblTotalACobrar" runat="server" Text="0.00"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="checkAll" Visible="false" runat="server" onclick="checkAllRow(this); CalcularItem();" Text="Incluir" TextAlign="Left" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIncluir" Visible="false" onclick="CheckRow(this); CalcularItem();" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <AUGE:OrdenesCobrosValores ID="ctrOrdenesCobrosValores" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                    onclick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"
                    onclick="btnAceptar_Click" ValidationGroup="OrdenesCobrosDatos" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"
                    onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
