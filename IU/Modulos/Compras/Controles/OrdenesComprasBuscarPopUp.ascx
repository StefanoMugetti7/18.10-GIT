<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrdenesComprasBuscarPopUp.ascx.cs" Inherits="IU.Modulos.Compras.Controles.OrdenesComprasBuscarPopUp" %>

<asp:Panel ID="pnlPopUp" runat="server" GroupingText="Buscar Orden Compra" Style="display: none" CssClass="modalPopUpBuscarAfiliados" >
<div class="OrdensComprasBuscarPopUp">


    <asp:Panel ID="pnlBuscar" runat="server">
    
    <asp:Label CssClass="labelEvol" ID="lblCodigoOrden" runat="server" Text="Codigo Orden Compra"></asp:Label>
            <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtCodigoOrdenCompra" runat="server"></AUGE:NumericTextBox>
            <div class="Espacio"></div>
            <asp:Label CssClass="labelEvol" ID="lblCondicionPago" runat="server" Text="Condicion de pago" />
            <asp:DropDownList CssClass="selectEvol" ID="ddlCondicionPago" runat="server" />
            <div class="Espacio"></div>
            <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" 
        onclick="btnBuscar_Click" />
            <br />

            <asp:Label CssClass="labelEvol" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
            <asp:TextBox CssClass="textboxEvol" ID="txtFechaDesde" runat="server"></asp:TextBox>
            <div class="Calendario">
                <asp:Image ID="imgFechaDesde" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                <asp:CalendarExtender ID="cdFechaDesde" runat="server" Enabled="true" 
                    TargetControlID="txtFechaDesde" PopupButtonID="imgFechaDesde" Format="dd/MM/yyyy"></asp:CalendarExtender>
            </div>
            <asp:Label CssClass="labelEvol" ID="lblFechaHasta" runat="server" Text="Fecha Hasta"></asp:Label>
            <asp:TextBox CssClass="textboxEvol" ID="txtFechaHasta" runat="server"></asp:TextBox>
            <div class="Calendario">
                <asp:Image ID="imgFechaHasta" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                <asp:CalendarExtender ID="cdFechaHasta" runat="server" Enabled="true" 
                    TargetControlID="txtFechaHasta" PopupButtonID="imgFechaHasta" Format="dd/MM/yyyy"></asp:CalendarExtender>
            </div>
                       
            <br />
                       
            <asp:Label CssClass="labelEvol" ID="lblProveedor" runat="server" Text="Numero Proveedor"></asp:Label>
            <asp:TextBox CssClass="textboxEvol" ID="txtProveedor" Enabled="false" runat="server"></asp:TextBox>

            <%--<div class="Espacio"></div>
            <asp:Label CssClass="labelEvol" ID="lblEstado"  runat="server" Text="Estado"></asp:Label>
            <asp:DropDownList CssClass="selectEvol" ID="ddlEstados"  runat="server">
            </asp:DropDownList>--%>
            
    
    
    </asp:Panel>
    <br />
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
                    <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="TotalOrden">
                        <ItemTemplate>
                            <%# Eval("TotalOrden", "{0:C2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>       
                   <asp:TemplateField HeaderText="Estado" SortExpression="Estado">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                    </asp:TemplateField>            
             <asp:TemplateField HeaderText="Acciones">
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