<%@ Page Language="C#" MasterPageFile="~/Modulos/Tesoreria/nmpCajas.master" AutoEventWireup="true" CodeBehind="CajasDetalles.aspx.cs" Inherits="IU.Modulos.Tesoreria.CajasDetalles" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <asp:Panel ID="pnlDatos" CssClass="DatosCajas" GroupingText="Detalle Movimientos" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
        <asp:GridView ID="gvDatos" DataKeyNames="IndiceColeccion" OnRowCommand="gvDatos_RowCommand"
            OnRowDataBound="gvDatos_RowDataBound"
            AllowPaging="true" AllowSorting="true" onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" 
            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="false"
            >
            <Columns>
                <asp:BoundField  HeaderText="Fecha" DataField="Fecha" DataFormatString="{0:dd/MM/yyyy}" SortExpression="Fecha" />
                <asp:BoundField  HeaderText="Descripcion" DataField="Descripcion" SortExpression="Descripcion" />
                <asp:TemplateField HeaderText="Tipo Operación" SortExpression="TipoOperacion.TipoOperacion">
                    <ItemTemplate>
                        <%# Eval("TipoOperacion.TipoOperacion")%>
                        <asp:HiddenField ID="hdfIdTipoOperacion" runat="server" Value='<%#Eval("TipoOperacion.IdTipoOperacion") %>' />
                                <asp:HiddenField ID="hdfIdRefTipoOperacion" runat="server" Value='<%#Eval("IdRefTipoOperacion") %>' />
                        <asp:HiddenField ID="hdfIdCajaMovimiento" runat="server" Value='<%#Eval("IdCajaMovimiento") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField  HeaderText="Número Referencia" DataField="IdRefTipoOperacion" ItemStyle-HorizontalAlign="Right" SortExpression="IdRefTipoOperacion" />
                <asp:TemplateField HeaderText="Socio" SortExpression="Afiliado.ApellidoNombre">
                    <ItemTemplate>
                        <%# Convert.ToInt32(Eval("Afiliado.IdAfiliado"))>0? Eval("Afiliado.ApellidoNombre") : string.Empty %>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="Tipo Valor" SortExpression="TipoValor.TipoValor">
                    <ItemTemplate>
                        <%# Eval("TipoValor.TipoValor")%>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Importe" SortExpression="Importe" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# string.Concat(Eval("CajaMoneda.Moneda.Moneda"), Eval("Importe", "{0:N2}"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center"  >
                    <ItemTemplate>
                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir" 
                            AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" Visible="true" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel> 
    </asp:Panel>
</asp:Content>
