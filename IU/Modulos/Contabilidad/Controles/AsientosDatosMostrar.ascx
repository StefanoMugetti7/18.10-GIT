<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AsientosDatosMostrar.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.AsientosDatosMostrar" %>

<div class="AsientosDatosMostrar">
    <asp:Panel ID="pnlAsientoMostrar" Visible="false" runat="server">
        <asp:Accordion
            ID="MyAccordion"
            runat="Server"
            SelectedIndex="-1"
            HeaderCssClass="accordionHeader"
            HeaderSelectedCssClass="accordionHeaderSelected"
            ContentCssClass="accordionContent"
            AutoSize="None"
            FadeTransitions="true"
            TransitionDuration="250"
            FramesPerSecond="40"
            RequireOpenedPane="false"
            SuppressHeaderPostbacks="true">
            <Panes>
                <asp:AccordionPane
                    HeaderCssClass="accordionHeader"
                    HeaderSelectedCssClass="accordionHeaderSelected"
                    ContentCssClass="accordionContent">
                    <Header>Asiento Contable</Header>
                    <Content>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEjercicioContable" runat="server" Text="Ejercicio:" />
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control" ID="txtEjercicioContable" Enabled="false" runat="server" />
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroAsiento" runat="server" Text="Número Asiento" />
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control" ID="txtNumeroAsiento" runat="server" Enabled="false" />
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaAsiento" runat="server" Text="Fecha" />
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control" ID="txtFechaAsiento" Enabled="false" runat="server" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial" />
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control" ID="txtFiliales" Enabled="false" runat="server" />
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control" ID="txtEstado" Enabled="false" runat="server" />
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo Operación" />
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control" ID="txtTipoOperacion" Enabled="false" runat="server" />
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRefTipoOperacion" runat="server" Text="Número Referencia" />
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control" ID="txtRefTipoOperacion" Enabled="false" runat="server" />
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalle" runat="server" Text="Detalle" />
                            <div class="col-sm-7">
                                <asp:TextBox CssClass="form-control" ID="txtDetalle" runat="server" TextMode="multiline" Rows="2" Enabled="false" />
                            </div>
                        </div>
                        <br />
                        <asp:GridView ID="gvDatos" AllowPaging="false" AllowSorting="false"
                            runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                            OnRowDataBound="gvDatos_RowDataBound">
                            <Columns>
                                <asp:TemplateField HeaderText="Cuenta Contable" SortExpression="CuentaContable.NumeroCuenta">
                                    <ItemTemplate>
                                        <%# Eval("CuentaContable.NumeroCuenta")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Descripción" SortExpression="CuentaContable.Descripcion">
                                    <ItemTemplate>
                                        <%# Eval("CuentaContable.Descripcion")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Debe" SortExpression="Debe" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%# Eval("Debe")%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblDebe" runat="server" Text="0.00" Style="text-align: right" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Haber" SortExpression="Haber" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <%# Eval("Haber")%>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblHaber" runat="server" Text="0.00" Style="text-align: right" />
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </Content>
                </asp:AccordionPane>
            </Panes>
        </asp:Accordion>
        <br />
    </asp:Panel>
</div>
