<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="FormasCobrosAfiliadosListar.aspx.cs" Inherits="IU.Modulos.TGE.FormasCobrosAfiliadosListar" Title="" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click" />
            <br />
            <br />
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" 
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false"
                >
                    <Columns>
                        <asp:TemplateField HeaderText="Forma Cobro" SortExpression="FormaCobro.FormaCobro">
                            <ItemTemplate>
                                <%# Eval("FormaCobro.FormaCobro")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Predeterminado" HeaderText="Predeterminado" />
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
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
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
