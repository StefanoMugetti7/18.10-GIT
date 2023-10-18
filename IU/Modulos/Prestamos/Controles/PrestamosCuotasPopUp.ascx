<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrestamosCuotasPopUp.ascx.cs" Inherits="IU.Modulos.Prestamos.Controles.PrestamosCuotasPopUp" %>

<script type="text/javascript" lang="javascript">

    function CalcularCancelacionesPopUp() {
        var subTotal = 0.00;
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            var incluir = $(this).find('input:checkbox[id*="chkIncluir"]').is(":checked");
            var importe = $(this).find("input[id*='hdfImporte']").val();
            importe = importe.replace('.', '').replace(',', '.');
            var importeCobrado = $(this).find("input[id*='hdfImporteCobrado']").val();
            importeCobrado = importeCobrado.replace('.', '').replace(',', '.');
            if (incluir && importe) {
                subTotal += parseFloat(importe) - parseFloat(importeCobrado);
            }
        });
        $("#<%=gvDatos.ClientID %> [id$=lblImporteTotal]").text(accounting.formatMoney(subTotal, "$ ", 2, "."));
    }
 
    //function CheckRowpp(objRef) {
    //    //Get the Row based on checkbox
    //    var row = objRef.parentNode.parentNode;
    //    //Get the reference of GridView
    //    var GridView = row.parentNode;
    //    //Get all input elements in Gridview
    //    var inputList = GridView.getElementsByTagName("input");
    //    for (var i = 0; i < inputList.length; i++) {
    //        //The First element is the Header Checkbox
    //        var headerCheckBox = inputList[0];
    //        //Based on all or none checkboxes
    //        //are checked check/uncheck Header Checkbox
    //        var checked = true;
    //        if (inputList[i].type == "checkbox" && inputList[i]
    //            != headerCheckBox) {
    //            if (!inputList[i].checked) {
    //                checked = false;
    //                break;
    //            }
    //        }
    //    }
    //    headerCheckBox.checked = checked;
    //}

    //function checkAllRowpp(objRef) {
    //    var GridView = objRef.parentNode.parentNode.parentNode;
    //    var inputList = GridView.getElementsByTagName("input");
    //    for (var i = 0; i < inputList.length; i++) {
    //        //Get the Cell To find out ColumnIndex
    //        var row = inputList[i].parentNode.parentNode;
    //        if (inputList[i].type == "checkbox" && objRef
    //            != inputList[i]) {
    //            if (objRef.checked) {
    //                inputList[i].checked = true;
    //            }
    //            else {
    //                inputList[i].checked = false;
    //            }
    //        }
    //    }
    //}

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

    function ShowModalPopUpPrestamos() {
        $("[id$='modalPopUpPrestamosCuotas']").modal('show');
    }

    function HideModalPopUpPrestamos() {
        $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalPopUpPrestamosCuotas']").modal('hide');
    }
</script>

<div class="modal" id="modalPopUpPrestamosCuotas" tabindex="-1" role="dialog">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-scrollable modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">Sistema de gestión para mutuales</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">



                        <asp:GridView ID="gvDatos" DataKeyNames="IdPrestamoCuota" AllowPaging="false"
                            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                            OnPageIndexChanging="gvDatos_PageIndexChanging" OnRowDataBound="gvDatos_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderText="Cuota" DataField="CuotaNumero" SortExpression="CuotaNumero" />
                                <asp:TemplateField HeaderText="Vencimiento" SortExpression="CuotaFechaVencimiento">
                                    <ItemTemplate>
                                        <%# Eval("CuotaFechaVencimiento", "{0:dd/MM/yyyy}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Importe" DataFormatString="{0:C2}" DataField="ImporteCuota" />
                                <asp:BoundField HeaderText="Interes" DataFormatString="{0:C2}" DataField="ImporteInteres" />
                                <asp:BoundField HeaderText="Amortizacion" DataFormatString="{0:C2}" DataField="ImporteAmortizacion" />
                                <asp:BoundField HeaderText="Saldo" DataFormatString="{0:C2}" DataField="ImporteSaldo" />
                                <asp:BoundField HeaderText="Importe Cobrado" DataFormatString="{0:C2}" DataField="ImporteCobrado" />
                                <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                    <ItemTemplate>
                                        <%# Eval("Estado.Descripcion")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <%--<asp:CheckBox ID="checkAll" runat="server" onclick="checkAllRowpp(this);" Text="Incluir" TextAlign="Left" />--%>
                                         <asp:CheckBox ID="checkAll" runat="server" onclick="checkAllRow(this);" Text="Incluir" TextAlign="Left" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIncluir" runat="server" Checked='<%# Eval("Incluir")%>' />
                                        <asp:HiddenField ID="hdfImporte" Value='<%#Bind("ImporteCuota") %>' runat="server" />
                                        <asp:HiddenField ID="hdfImporteCobrado" Value='<%#Bind("ImporteCobrado") %>' runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="modal-footer">
                        <div class="row justify-content-md-center">
                            <div class="col-md-auto">
                                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" CausesValidation="false" />
                                <asp:Button CssClass="botonesEvol" ID="btnVolver" OnClick="btnVolver_Click" CausesValidation="false" runat="server" Text="Volver" />
                           

                            </div>


                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
    </asp:UpdatePanel>
</div>
