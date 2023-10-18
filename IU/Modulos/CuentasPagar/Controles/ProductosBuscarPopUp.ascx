<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductosBuscarPopUp.ascx.cs" Inherits="IU.Modulos.CuentasPagar.Controles.BuscarProductoPopUp" %>

<asp:Panel ID="pnlPopUp" runat="server" GroupingText="Buscar Producto" Style="display: none" CssClass="modalPopUpBuscarAfiliados" >
<div class="ProductosListar">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlBuscar" runat="server">
    <asp:Label CssClass="labelEvol" ID="lblNumeroProducto" runat="server" Text="Número Producto"></asp:Label>
    <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtNumeroProducto" runat="server"></AUGE:NumericTextBox>
    
    <asp:Label CssClass="labelEvol" ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
    <asp:TextBox CssClass="textboxEvol" ID="txtDescripcion" runat="server"></asp:TextBox>
    
    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" 
        onclick="btnBuscar_Click" />
    <br />
    <asp:Label CssClass="labelEvol" ID="lblFamilia" runat="server" Text="Familia"></asp:Label>
    <asp:DropDownList CssClass="selectEvol" ID="ddlFamilias" runat="server">
    </asp:DropDownList>
    <br />
    <br />
    </asp:Panel>
    <asp:GridView ID="gvProductos" OnRowCommand="gvProductos_RowCommand" AllowPaging="true" AllowSorting="true" 
    OnRowDataBound="gvProductos_RowDataBound" DataKeyNames="IdProducto,Descripcion,FamiliaIdFamilia,FamiliaDescripcion"
    runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="true"
    onpageindexchanging="gvProductos_PageIndexChanging" onsorting="gvProductos_Sorting"
    >
        <Columns>
            <asp:TemplateField HeaderText="Numero Producto" SortExpression="IdProducto">
                    <ItemTemplate>
                        <%# Eval("IdProducto")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Descripcion" SortExpression="Descripcion">
                    <ItemTemplate>
                        <%# Eval("Descripcion")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Familia" SortExpression="FamiliaDescripcion">
                    <ItemTemplate>
                        <%# Eval("FamiliaDescripcion")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Stock Actual" SortExpression="Descripcion" >
                    <ItemTemplate>
                        <%# Eval("StockActual")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Acciones" >
                <ItemTemplate>
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Consultar" ID="btnSeleccionar"
                         AlternateText="Seleccionar" ToolTip="Seleccionar" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <center>
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