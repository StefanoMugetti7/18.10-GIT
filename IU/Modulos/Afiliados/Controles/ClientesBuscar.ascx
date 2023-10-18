<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientesBuscar.ascx.cs" Inherits="IU.Modulos.Afiliados.Controles.ClientesBuscar" %>

<div class="AfiliadosListar">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlBuscar" runat="server">
    <asp:Label CssClass="labelEvol" ID="lblNumeroSocio" runat="server" Text="Código"></asp:Label>
    <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtNumeroSocio" runat="server"></AUGE:NumericTextBox>
    
    <asp:Label CssClass="labelEvol" ID="lblTipoDocumento" runat="server" Text="Tipo documento"></asp:Label>
        <asp:DropDownList CssClass="selectEvol" ID="ddlTipoDocumento" runat="server">
        </asp:DropDownList>
    
    <asp:Label CssClass="labelEvol" ID="lblNumeroDocumento" runat="server" Text="Número Documento"></asp:Label>
    <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtNumeroDocumento" runat="server"></AUGE:NumericTextBox>
    
    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" CausesValidation="false"
        onclick="btnBuscar_Click" />
    <br />
    <asp:Label CssClass="labelEvol" ID="lblApellido" runat="server" Text="Razon Social"></asp:Label>
    <asp:TextBox CssClass="textboxEvol" ID="txtApellido" runat="server"></asp:TextBox>
    
    <asp:Label CssClass="labelEvol" ID="lblMatricula" runat="server" Text="Legajo"></asp:Label>
    <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtMatricula" runat="server"></AUGE:NumericTextBox>

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
            <asp:TemplateField HeaderText="Codigo" SortExpression="IdAfiliado">
                    <ItemTemplate>
                        <%# Eval("IdAfiliado")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Tipo" SortExpression="TipoDocumento.TipoDocumento">
                    <ItemTemplate>
                        <%# Eval("TipoDocumento.TipoDocumento")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField  HeaderText="Número" DataField="NumeroDocumento" ItemStyle-Wrap="false" SortExpression="NumeroDocumento" />
            <asp:BoundField  HeaderText="Razon Social" DataField="RazonSocial" SortExpression="Apellido" />
            <asp:BoundField  HeaderText="Detalle" DataField="Detalle" SortExpression="Detalle" />
            <asp:TemplateField HeaderText="Tipo" SortExpression="CondicionFiscal.Descripcion">
                    <ItemTemplate>
                        <%# Eval("CondicionFiscal.Descripcion")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                <ItemTemplate>
                    <%# Eval("Estado.Descripcion")%>
                </ItemTemplate>
            </asp:TemplateField>            
            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center"  >
                <ItemTemplate>
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                         AlternateText="Seleccionar" ToolTip="Seleccionar" />
                </ItemTemplate>
            </asp:TemplateField>
            </Columns>
    </asp:GridView>
    </ContentTemplate>
    </asp:UpdatePanel>
</div>