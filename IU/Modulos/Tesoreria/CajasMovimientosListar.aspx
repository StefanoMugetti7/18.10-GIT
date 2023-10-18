<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="True" CodeBehind="CajasMovimientosListar.aspx.cs" Inherits="IU.Modulos.Tesoreria.CajasMovimientosListar" %>

<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="FacturasListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha"></asp:Label>
                    <div class="col-sm-3">
                <div class="form-group row">
                        <div class="col-sm-6">
                            <asp:TextBox CssClass="form-control datepicker" Placeholder="Desde" ID="txtFechaDesde" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-sm-6">
                            <asp:TextBox CssClass="form-control datepicker" Placeholder="Hasta" ID="txtFechaHasta" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumero" runat="server" Text="Numero" />
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtNumero" runat="server" />
                    </div>
                               <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                            OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregarMovimiento" runat="server" OnClick="btnAgregarMovimiento_Click"
                            Text="Agregar" />
                    </div>

                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlFilial" runat="server">
                        </asp:DropDownList>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo de Operacion"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoOperacion" runat="server">
                        </asp:DropDownList>
                    </div>
         
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripción" />
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtDescripcion" TextMode="MultiLine"  Enabled="true" runat="server"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group row">
                    <div class="col-sm-3">
                        <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                            runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                        <div class="col-sm-8"></div>
                    </div>
                </div>
                <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                <%--<auge:popUpEnviarMail ID="popUpMail" runat="server"   />--%>
                <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdCajaMovimiento"
                    runat="server" SkinID="GrillaBasicaFormalSticky" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="# Movimiento">
                            <ItemTemplate>
                                <%# Eval("IdCajaMovimiento")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <%# Eval("Fecha", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo Operacion">
                            <ItemTemplate>
                                <%# Eval("TipoOperacion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descripcion">
                            <ItemTemplate>
                                <%# Eval("Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# string.Concat(Eval("Moneda"), Eval("Importe", "{0:N2}"))%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado">
                            <ItemTemplate>
                                <%# Eval("EstadoDescripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                    AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/sendmail.png" Visible="false" runat="server" CommandName="EnviarMail" ID="btnEnviarMail"
                                    AlternateText="Enviar Mail" ToolTip="Enviar Mail" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:HiddenField ID="hdfIdTipoOperacion" runat="server" Value='<%#Eval("IdTipoOperacion") %>' />
                                <asp:HiddenField ID="hdfIdRefTipoOperacion" runat="server" Value='<%#Eval("IdRefTipoOperacion") %>' />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnAnular"
                                    AlternateText="Anular Movimiento" ToolTip="Anular Movimiento" Visible="false" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Evol:EvolGridView>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnExportarExcel" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</asp:Content>
