<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="FacturasLotesListar.aspx.cs" Inherits="IU.Modulos.Facturas.FacturasLotesListar" %>
<asp:Content ID="headScript" ContentPlaceHolderID="head" runat="server">
 
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="FacturasLotesListar">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label CssClass="labelEvol" ID="lblPeriodo" runat="server" Text="Periodo"></asp:Label>
            <AUGE:NumericTextBox CssClass="txtCodigoBuscador" ID="txtPeriodo" runat="server" />
            <asp:Label CssClass="labelEvol" ID="lblTipoLote" runat="server" Text="Tipo Lote" />
            <asp:DropDownList CssClass="selectEvol" ID="ddlTipoLote" runat="server" />
            <div class="Espacio"></div>
            <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" 
                onclick="btnBuscar_Click" />
            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" 
            onclick="btnAgregar_Click" />
            <br />
            <br />
            <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdFactura"
            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
            onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" 
            >
                <Columns>
                    <asp:TemplateField HeaderText="Periodo" SortExpression="Periodo">
                            <ItemTemplate>
                                <%# Eval("Periodo")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tipo de Lote" SortExpression="TipoLoteEnviadoDescripcion">
                            <ItemTemplate>
                                <%# Eval("TipoLoteEnviadoDescripcion")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cantidad" SortExpression="CantidadRegistros">
                            <ItemTemplate>
                                <%# Eval("CantidadRegistros")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe Total" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="ImporteTotal">
                        <ItemTemplate>
                            <%# Eval("ImporteTotal", "{0:C2}")%>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Usuario Alta" SortExpression="UsuarioAltaApellidoNombre">
                            <ItemTemplate>
                                <%# Eval("UsuarioAltaApellidoNombre")%>
                            </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                            <ItemTemplate>
                                <%# Eval("EstadoDescripcion")%>
                                <asp:Image ID="imgAlerta" runat="server" Visible="false" ToolTip="Factura Electronica Pendiente de Validar en AFIP" AlternateText="Factura Electronica Pendiente de Validar en AFIP" ImageUrl="~/Imagenes/alerta.png" />
                            </ItemTemplate>
                    </asp:TemplateField> 
                     <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                AlternateText="Mostrar" ToolTip="Mostrar" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnAnular" 
                                AlternateText="Anular Lote Factura" ToolTip="Anular Lote Factura"  Visible="false"/>
                        </ItemTemplate>
                        <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                     </asp:TemplateField>
                    </Columns>
            </Evol:EvolGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>
