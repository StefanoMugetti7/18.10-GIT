<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="OrdenesCobrosListar.aspx.cs" Inherits="IU.Modulos.Cobros.OrdenesCobrosListar" Title="" %>

<%@ Register Src="~/Modulos/Comunes/popUpComprobantes.ascx" TagName="popUpComprobantes" TagPrefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="OrdenesCobrosListar">
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
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
                    </div>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFilial" runat="server" Text="Filial" />
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlFiliales" runat="server" />
                    </div>
                </div>
                <div class="table-responsive">
                    <AUGE:popUpComprobantes ID="ctrPopUpComprobantes" runat="server" />
                    <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>
                        <asp:BoundField HeaderText="Nro." DataField="IdOrdenCobro" SortExpression="IdOrdenCobro" />
                        <asp:BoundField HeaderText="Fecha" DataField="FechaEmision" DataFormatString="{0:dd/MM/yyyy}" SortExpression="FechaEmision" />
                        <asp:TemplateField HeaderText="Tipo Operacion" SortExpression="TipoOperacion.TipoOperacion">
                            <ItemTemplate>
                                <%# Eval("TipoOperacionTipoOperacion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Detalle" DataField="Detalle" SortExpression="Detalle" />
                        <asp:TemplateField HeaderText="Filial" SortExpression="Filial.Filial">
                            <ItemTemplate>
                                <%# Eval("FilialFilial")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Filial Cobro" SortExpression="FilialCobro.Filial">
                            <ItemTemplate>
                                <%# Eval("FilialCobroFilial")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Importe" SortExpression="Importe" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# string.Concat(Eval("MonedaMoneda"), Eval("ImporteTotal", "{0:N2}"))%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("EstadoDescripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                    AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/sendmail.png" runat="server" CommandName="EnviarMail" ID="btnEnviarMail"
                                            AlternateText="Enviar Mail" ToolTip="Enviar Mail" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnAnular" Visible="false"
                                    AlternateText="Anular" ToolTip="Anular" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="AnularConfirmar" ID="btnAnularConfirmar" Visible="false"
                                    AlternateText="Anular Cobrada" ToolTip="Anular Cobrada" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Evol:EvolGridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
