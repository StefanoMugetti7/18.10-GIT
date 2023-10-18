<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProveedoresBuscarPopUp.ascx.cs" Inherits="IU.Modulos.CuentasPagar.Controles.ProveedoresBuscarPopUp" %>


<asp:Panel ID="pnlPopUp" runat="server" GroupingText="Buscar Proveedor" Style="display: none" CssClass="modalPopUpBuscarAfiliados" >
<div class="ProveedoresListarPopUp">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">

        <ContentTemplate>
            <asp:Panel ID="pnlBuscar" runat="server">
    
    <asp:Label CssClass="labelEvol" ID="lblNumeroProveedor" runat="server" Text="Número Proveedor"></asp:Label>
    <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtNumeroProveedor" runat="server" />
    <div class="Espacio"></div>
    <asp:Label CssClass="labelEvol" ID="lblCuit" runat="server" Text="CUIT"></asp:Label>
    <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtCuit" runat="server"    />
    <div class="Espacio"></div>
    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" 
        onclick="btnBuscar_Click" />
    <br />
    
    <asp:Label CssClass="labelEvol" ID="lblRazonSocial" runat="server" Text="Razón Social"></asp:Label>
    <asp:TextBox CssClass="textboxEvol" ID="txtRazonSocial" runat="server"></asp:TextBox>
    <div class="Espacio"></div>
    <asp:Label CssClass="labelEvol" ID="lblEstado"  runat="server" Text="Estado"></asp:Label>
    <asp:DropDownList CssClass="selectEvol" ID="ddlEstados"  runat="server">
    </asp:DropDownList>        
    </asp:Panel>
    <br />
    <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
    runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="true"
    onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting"
    >
        <Columns>
            <asp:TemplateField HeaderText="Número Proveedor" SortExpression="IdProveedor">
                    <ItemTemplate>
                        <%# Eval("IdProveedor")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Razón Social" SortExpression="RazonSocial">
                    <ItemTemplate>
                        <%# Eval("RazonSocial")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="CUIT" SortExpression="CUIT">
                <ItemTemplate>
                    <%# Eval("CUIT")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                <ItemTemplate>
                    <%# Eval("Estado.Descripcion")%>
                </ItemTemplate>
            </asp:TemplateField>            
             <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                        AlternateText="Mostrar" ToolTip="Mostrar" />
                    
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