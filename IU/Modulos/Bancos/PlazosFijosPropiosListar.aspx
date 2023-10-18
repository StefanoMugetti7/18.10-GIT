<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PlazosFijosPropiosListar.aspx.cs" Inherits="IU.Modulos.Bancos.PlazosFijosPropiosListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBanco" runat="server" Text="Banco" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlBancos" runat="server" />
                    </div>
                    <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" OnClick="btnBuscar_Click" Text="Buscar" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" OnClick="btnAgregar_Click" Text="Agregar" />
                    </div>
                </div>
                <div class="table-responsive">
                    <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                        OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdPlazoFijo"
                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                        OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                        <Columns>
                            <asp:BoundField HeaderText="Número" DataField="IdPlazoFijo" SortExpression="IdPlazoFijo" />
                            <asp:TemplateField HeaderText="Cuenta" SortExpression="Cuenta.IdCuenta">
                                <ItemTemplate>
                                    <%# Eval("BancoCuentaDenominacion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Detalle" SortExpression="Descripcion">
                                <ItemTemplate>
                                    <%# Eval("Descripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fecha" SortExpression="FechaInicioVigencia">
                                <ItemTemplate>
                                    <%# Eval("FechaInicioVigencia", "{0:dd/MM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Moneda" SortExpression="Moneda.Moneda">
                                <ItemTemplate>
                                    <%# Eval("BancoCuentaMonedaMoneda")%>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Vencimiento" SortExpression="FechaVencimiento">
                                <ItemTemplate>
                                    <%# Eval("FechaVencimiento", "{0:dd/MM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Capital" ItemStyle-CssClass="text-right" SortExpression="Capital">
                                <ItemTemplate>
                                    <%# string.Concat(Eval("BancoCuentaMonedaMoneda"), Eval("ImporteCapital", "{0:N2}"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Interes" ItemStyle-CssClass="text-right" SortExpression="Interes">
                                <ItemTemplate>
                                    <%# string.Concat(Eval("BancoCuentaMonedaMoneda"), Eval("ImporteInteres", "{0:N2}"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total" ItemStyle-CssClass="text-right" SortExpression="Total">
                                <ItemTemplate>
                                    <%# string.Concat(Eval("BancoCuentaMonedaMoneda"), Eval("ImporteTotal", "{0:N2}"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                                <ItemTemplate>
                                    <%# Eval("EstadoDescripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" Visible="false" CommandName="Consultar" ID="btnConsultar"
                                        AlternateText="Consultar" ToolTip="Consultar" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" Visible="false" CommandName="Modificar" ID="btnModificar"
                                        AlternateText="Modificar" ToolTip="Modificar" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/cancel_f2.png" runat="server" Visible="false" CommandName="Anular" ID="btnAnular"
                                        AlternateText="Anular" ToolTip="Anular" />
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/pesos.png" runat="server" Visible="false" CommandName="Pagar" ID="btnPagar"
                                        AlternateText="Pagar Plazo Fijo" ToolTip="Pagar Plazo Fijo" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>