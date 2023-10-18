<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CotizacionesBuscarPopUp.ascx.cs" Inherits="IU.Modulos.Compras.Controles.CotizacionesBuscarPopUp" %>

<asp:Panel ID="pnlPopUp" runat="server" GroupingText="Buscar Cotizacion" Style="display: none" CssClass="modalPopUpBuscarAfiliados" >
<div class="CotizacionesListar">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="pnlBuscaCotizacion" runat="server" Text="Buscar Cotizacón" >
    
    <asp:Label CssClass="labelEvol" ID="lblNumeroProducto" runat="server" Text="Numero Producto" Enabled="true"></asp:Label>
    <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtNumeroProducto" runat="server"></AUGE:NumericTextBox>
    
    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" 
        onclick="btnBuscar_Click" />
    <br />
    
    <br />
    <br />
    </asp:Panel>
    <asp:GridView ID="gvCotizacion" OnRowCommand="gvCotizacion_RowCommand" AllowPaging="true" AllowSorting="true" 
    OnRowDataBound="gvCotizacion_RowDataBound" DataKeyNames="IndiceColeccion"
    runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="true"
    onpageindexchanging="gvCotizacion_PageIndexChanging" onsorting="gvCotizacion_Sorting"
    >
        <Columns>
            <asp:TemplateField HeaderText="Numero Cotizacion" SortExpression="IdCotizacion">
                    <ItemTemplate>
                        <%# Eval("IdCotizacion")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Nombre Proveedor" SortExpression="Proveedor.RazonSocial">
                    <ItemTemplate>
                        <%# Eval("RazonSocial")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Numero Producto" SortExpression="IdProducto">
                    <ItemTemplate>
                        <%# Eval("Producto.IdProducto")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Descripcion" SortExpression="Descripcion">
                    <ItemTemplate>
                        <%# Eval("Producto.Descripcion")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Precio" SortExpression="Precio">
                    <ItemTemplate>
                        <%# Eval("PrecioUnitario")%>
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