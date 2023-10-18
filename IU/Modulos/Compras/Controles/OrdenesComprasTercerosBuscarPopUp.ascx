<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrdenesComprasTercerosBuscarPopUp.ascx.cs" Inherits="IU.Modulos.Compras.Controles.OrdenesComprasTercerosBuscarPopUp" %>

<asp:Panel ID="pnlPopUp" runat="server" GroupingText="Buscar Orden Compra" Style="display: none" CssClass="modalPopUpBuscarAfiliados" >
<div class="OrdensComprasBuscarPopUp">
    <asp:Label CssClass="labelEvol" ID="lblPeriodo" runat="server" Text="Periodo"></asp:Label>
            <asp:DropDownList CssClass="selectEvol" ID="ddlPeriodo" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged" >
            </asp:DropDownList>
            <br /><br />
     <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
    <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
    runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="true"
    onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting"
    >
        <Columns>
            <asp:TemplateField HeaderText="Código Orden" SortExpression="CodigoOrden">
                            <ItemTemplate>
                                <%# Eval("IdOrdenCompra")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Proveedor" SortExpression="Proveedor">
                            <ItemTemplate>
                                <%# Eval("Proveedor.RazonSocial")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fecha Orden Compra" SortExpression="FechaOrden">
                            <ItemTemplate>
                                <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Socio" SortExpression="ApellidoNombre">
                            <ItemTemplate>
                                <%# Eval("Afiliado.ApellidoNombre")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Observacion" SortExpression="Observacion">
                            <ItemTemplate>
                                <%# Eval("Observacion")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <%--<asp:TemplateField HeaderText="Total" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="TotalOrden">
                        <ItemTemplate>
                            <%# Eval("TotalOrden", "{0:C2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>    --%>   
                    <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="ImporteDescontar">
                        <ItemTemplate>
                            <%# Eval("ImporteDescontar", "{0:C2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>       
                   <asp:TemplateField HeaderText="Estado" SortExpression="Estado">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                    </asp:TemplateField>  
                     <asp:TemplateField HeaderText="Acciones">
                        <HeaderTemplate>
                      <asp:CheckBox ID="checkAll" runat="server" onclick="checkAllRow(this);" Text="Incluir" TextAlign="Left" />
                    </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkIncluir" Visible="true" Checked='<%#Eval("Check") %>' runat="server" />
                        </ItemTemplate>
                     </asp:TemplateField>          
            </Columns>
    </asp:GridView>
    </ContentTemplate>
  </asp:UpdatePanel>
    <br />
    <center>
        <asp:Button CssClass="botonesEvol" ID="btnAceptar" CausesValidation="false" runat="server" Text="Aceptar" onclick="btnAceptar_Click" />
        <asp:Button CssClass="botonesEvol" ID="btnVolver" CausesValidation="false" runat="server" Text="Volver" />
    </center>
</div>
</asp:Panel>
  
<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" style="display:none;" runat="server" Text="Button" />
<asp:ModalPopupExtender ID="mpePopUp" runat="server"
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp"
    BackgroundCssClass="modalBackground"
    CancelControlID="btnVolver" >
</asp:ModalPopupExtender>

<script type="text/javascript">
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