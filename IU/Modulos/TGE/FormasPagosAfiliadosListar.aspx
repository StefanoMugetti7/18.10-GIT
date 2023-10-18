<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="FormasPagosAfiliadosListar.aspx.cs" Inherits="IU.Modulos.TGE.FormasPagosAfiliadosListar" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <asp:Button CssClass="botonesEvol" ID="btnAgregar" Visible="false" runat="server" Text="Agregar" onclick="btnAgregar_Click" />
            <br />
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false"
                >
                    <Columns>
                        <asp:TemplateField HeaderText="Forma Pago" SortExpression="FormaPago.FormaPago">
                            <ItemTemplate>
                                <%# Eval("FormaPago.FormaPago")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Filial" SortExpression="Filial.Filial">
                            <ItemTemplate>
                                <%# Eval("Filial.Filial")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField  HeaderText="Zona Grupo" DataField="ZG" SortExpression="ZG" />
                        <%--<asp:BoundField DataField="Predeterminado" HeaderText="Predeterminado" />--%>
                        <asp:BoundField  HeaderText="Fecha Alta" DataField="FechaAlta" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FechaAlta" />
                        <asp:TemplateField HeaderText="Usuario Alta" SortExpression="UsuarioAlta.ApellidoNombre">
                            <ItemTemplate>
                                <%# Eval("UsuarioAlta.ApellidoNombre")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center"  >
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar" 
                                    AlternateText="Consultar" ToolTip="Consultar" />
                            </ItemTemplate>
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                                    AlternateText="Modificar" ToolTip="Modificar" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        </Columns>
                </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>