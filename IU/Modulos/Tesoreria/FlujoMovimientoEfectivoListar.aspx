<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="True" CodeBehind="FlujoMovimientoEfectivoListar.aspx.cs" Inherits="IU.Modulos.Tesoreria.FlujoMovimientoEfectivoListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="FlujoMovimientoEfectivoListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaHasta" runat="server" Text="Fecha Hasta"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlFilial" runat="server"></asp:DropDownList>
                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFilial" ControlToValidate="ddlFilial" ValidationGroup="Buscar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" ValidationGroup="Buscar"
                            OnClick="btnBuscar_Click" />
                        <%--                        <asp:Button CssClass="botonesEvol" ID="btnAgregarMovimiento" runat="server" OnClick="btnAgregarMovimiento_Click"
                            Text="Agregar" />--%>
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-3">
                        <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                            runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                        <div class="col-sm-8"></div>
                    </div>
                </div>
                <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="false" AllowSorting="false"
                    OnRowDataBound="gvDatos_RowDataBound" runat="server" DataKeyNames="NroMovimiento, Origen"
                    SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>
                        <asp:TemplateField HeaderText="Fecha" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <%# Eval("Fecha", "{0:dd/MM/yyyy}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Detalle">
                            <ItemTemplate>
                                <%# Eval("Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                $<%# Eval("Importe", "{0:N2}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Saldo" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                $<%# Eval("SaldoActual", "{0:N2}")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                    AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" Visible="false"/>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/sendmail.png" Visible="false" runat="server" CommandName="EnviarMail" ID="btnEnviarMail"
                                    AlternateText="Enviar Mail" ToolTip="Enviar Mail" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" Visible="false"/>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnAnular"
                                    AlternateText="Anular Movimiento" ToolTip="Anular Movimiento" Visible="false" />
                            </ItemTemplate>
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
