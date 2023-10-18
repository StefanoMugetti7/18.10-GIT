<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportarRemitoPopUp.ascx.cs" Inherits="IU.Modulos.CuentasPagar.Controles.ImportarRemitoPopUp" %>

<%--<asp:Panel ID="pnlPopUp" runat="server" GroupingText="Buscar Remito" Style="display: none" CssClass="modalPopUpBuscarAfiliados">
    <div class="RemitosListar">--%>

<script type="text/javascript">

    //function CheckRow(objRef) {
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

    //function checkAllRow(objRef) {
    //    var GridView = objRef.parentNode.parentNode.parentNode;
    //    var inputList = GridView.getElementsByTagName("input");

    //    for (var i = 0; i < inputList.length; i++) {
    //        //Get the Cell To find out ColumnIndex                
    //        var row = inputList[i].parentNode.parentNode;

    //        if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
    //            if (objRef.checked) {
    //                if (inputList[i].disabled == false) {
    //                    inputList[i].checked = true;
    //                }
    //                else {
    //                    inputList[i].checked = false;
    //                }
    //            }
    //            else {
    //                inputList[i].checked = false;
    //            }
    //        }
    //    }
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

    function ShowModalImportarRemito() {
         $('.modal-backdrop').remove();
        $("[id$='modalImportarRemito']").modal('show');
    }

    function HideModalImportarRemito() {
               $('body').removeClass('modal-open');
        $('.modal-backdrop').remove();
        $("[id$='modalImportarRemito']").modal('hide');
    }

</script>

<%--DATOS A FILTRAR--%>
<div class="modal" id="modalImportarRemito" tabindex="-1" role="dialog">
    <div class="modal-dialog modal-dialog-scrollable modal-xl" role="document">
        <div class="modal-content modal-minHeight85">
            <div class="modal-header">
                <h5 class="modal-title">Buscar Remito</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoRemito" runat="server" Text="Codigo Remito" />
                            <div class="col-sm-3">
                                <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigoRemito" runat="server" />
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroRemito" runat="server" Text="Numero Remito" />
                            <div class="col-sm-1">
                                <asp:TextBox CssClass="form-control" ID="txtNumeroRemitoPrefijo" Enabled="true" MaxLength="4" runat="server" />
                            </div>
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control" ID="txtNumeroRemitoSuFijo" Enabled="true" MaxLength="8" runat="server" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaHasta" runat="server" Text="Fecha Hasta"></asp:Label>
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3"></div>
                        </div>
                        <%--<asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado"  runat="server" Text="Estado"></asp:Label>
        <asp:DropDownList CssClass="selectEvol" ID="ddlEstados"  runat="server"></asp:DropDownList>
        <br />
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroSocio" runat="server" Text="Numero Socio" />
        <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtNumeroSocio" AutoPostBack="true" runat="server" />--%>
                        <%--GRILLA--%>
                        <%--        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>--%>
                         <div class="form-group row">
                            <div class="col-sm-12">
                                <div class="table-responsive">
                        <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="false" AllowSorting="false"
                            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                            runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="true"
                            OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                            <Columns>
                                <asp:TemplateField HeaderText="Número Remito" SortExpression="NumeroRemito">
                                    <ItemTemplate>
                                        <%# Eval("NumeroRemitoCompleto")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Fecha Remito" SortExpression="FechaRemito">
                            <ItemTemplate>
                                <%# Eval("FechaRemito", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                    </asp:TemplateField--%>
                                <asp:TemplateField HeaderText="Codigo Producto" SortExpression="CodigoProducto">
                                    <ItemTemplate>
                                        <%# Eval("Producto.IdProducto")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Producto" SortExpression="Producto">
                                    <ItemTemplate>
                                        <%# Eval("Producto.Descripcion")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cantidad" SortExpression="Cantidad">
                                    <ItemTemplate>
                                        <%# Eval("CantidadRecibida")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <%--<asp:TemplateField HeaderText="Precio Lista" SortExpression="Precio">
                            <ItemTemplate>
                                <Evol:CurrencyTextBox CssClass="textboxEvol" ID="txtPrecio" class="gvTextBoxChico" runat="server" Text='<%# Eval("Producto.Precio")%>'></Evol:CurrencyTextBox>
                        </ItemTemplate>
                        </asp:TemplateField>--%>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="checkAll" runat="server" onclick="checkAllRow(this);" Text="Incluir" TextAlign="Left" />
                                    </HeaderTemplate>

                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIncluir" runat="server" onclick="CheckRow(this);" Checked='<%# Eval("Incluir")%>' />
                                    </ItemTemplate>

                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                                    </div>
                                </div>
                             </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="row justify-content-md-center">
                    <div class="col-md-auto">
                        <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" OnClientClick="HideModalImportarRemito();" />
                        <asp:Button CssClass="botonesEvol" ID="btnVolver" OnClientClick="HideModalImportarRemito();" CausesValidation="false" runat="server" Text="Volver" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<%--    </div>
</asp:Panel>--%>

<%--<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" Style="display: none;" runat="server" Text="Button" />
<asp:ModalPopupExtender ID="mpePopUp" runat="server"
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp"
    BackgroundCssClass="modalBackground"
    CancelControlID="btnVolver">
</asp:ModalPopupExtender>--%>
