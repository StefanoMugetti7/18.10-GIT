<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="Imprimir.aspx.cs" Inherits="IU.Modulos.Haberes.Imprimir" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
    <div class="col-sm-12">
                    <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
                        SkinID="GrillaBasicaFormalSticky" OnRowDataBound="gvDatos_RowDataBound" runat="server" ShowFooter="false"
                        DataKeyNames="IdAfiliado,Periodo,IdTipoRecibo" AutoGenerateColumns="false" 
                        OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                        <Columns>
                            <asp:TemplateField HeaderText="Periodo" SortExpression="Periodo">
                                <ItemTemplate>
                                    <%# Eval("Periodo")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tipo de Recibo" SortExpression="TipoRecibo">
                                <ItemTemplate>
                                    <%# Eval("TipoRecibo")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Haberes" SortExpression="Haberes" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" FooterStyle-Wrap="false">
                                <ItemTemplate>
                                    <%# Eval("Haberes", "{0:C2}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Descuentos" SortExpression="Descuentos" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" FooterStyle-Wrap="false">
                                <ItemTemplate>
                                    <%# Eval("Descuentos", "{0:C2}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Neto a Pagar" SortExpression="NetoPagar" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" FooterStyle-Wrap="false">
                                <ItemTemplate>
                                    <%# Eval("NetoPagar", "{0:C2}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                                <ItemTemplate>
                                    <%# Eval("EstadoDescripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                        AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </Evol:EvolGridView>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
