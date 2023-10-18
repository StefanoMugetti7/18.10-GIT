<%@ Page Language="C#" MasterPageFile="~/Modulos/Tesoreria/nmpTesorerias.master" AutoEventWireup="true" CodeBehind="TesoreriasMovimientos.aspx.cs" Inherits="IU.Modulos.Tesoreria.TesoreriasMovimientos" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <div class="TesoreriasMovimientos">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <br />
                <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
                <br />
                <br />

                <asp:GridView ID="gvDatos" AllowPaging="true" AllowSorting="false" OnRowCommand="gvDatos_RowCommand"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion,IdTesoreriaMovimiento"
                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="Transaccion" SortExpression="Transaccion">
                            <ItemTemplate>
                                <%# Eval("Transaccion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo Movimiento" SortExpression="TipoOperacion.TipoMovimiento.TipoMovimiento">
                            <ItemTemplate>
                                <%# Eval("TipoOperacion.TipoMovimiento.TipoMovimiento")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Moneda" SortExpression="TesoreriaMoneda.Moneda.Descripcion">
                            <ItemTemplate>
                                <%# Eval("TesoreriaMoneda.Moneda.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- <asp:BoundField  HeaderText="Caja" DataField="" SortExpression="" />
                        <asp:BoundField  HeaderText="Usuario" DataField="" SortExpression="" />--%>
                        <asp:TemplateField HeaderText="Tipo Valor" SortExpression="TipoValor.TipoValor">
                            <ItemTemplate>
                                <%# Eval("TipoValor.TipoValor")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# string.Concat(Eval("TesoreriaMoneda.Moneda.Moneda"), Eval("ImporteSigno", "{0:N2}"))%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Consultar" ToolTip="Consultar" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
                <center>
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
