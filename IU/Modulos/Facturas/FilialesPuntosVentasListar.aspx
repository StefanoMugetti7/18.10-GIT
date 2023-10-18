<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="FilialesPuntosVentasListar.aspx.cs" Inherits="IU.Modulos.Facturas.FilialesPuntosVentasListar" %>

<%@ Register Src="~/Modulos/Facturas/Controles/FilialesPuntosVentasDatos.ascx" TagName="popUpPuntoVentaDatos" TagPrefix="auge" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumero" runat="server" Text="Nro. Punto de Venta"></asp:Label>
                <div class="col-sm-3">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtPrefijoNumeroFactura" runat="server" maxlength="8" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoPuntoVenta" runat="server" Text="Tipo de Punto de Venta"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlTiposPuntosVentas" runat="server">
                    </asp:DropDownList>
                </div>
                <div class="col-sm-3">
                    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                    <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial" ToolTip="Filial a la que esta asociado el Punto de Venta"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlFilial" runat="server" ToolTip="Filial a la que esta asociado el Punto de Venta" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoFactura" runat="server" Text="Tipo de Comprobante"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlTiposFacturas" runat="server"></asp:DropDownList>
                </div>
                <div class="col-sm-3"></div>
            </div>
            <div class="form-group row">
                <div class="col-sm-12">
                    <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                        runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                </div>
            </div>
            <AUGE:popUpPuntoVentaDatos ID="ctrPuntoVentaDatos" runat="server" />
            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdFilialPuntoVenta"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                <Columns>
                    <asp:TemplateField HeaderText="Codigo" SortExpression="IdFilialPuntoVenta">
                        <ItemTemplate>
                            <%# Eval("IdFilialPuntoVenta")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nro Pto Vta" SortExpression="AfipPuntoVenta">
                        <ItemTemplate>
                            <%# Eval("AfipPuntoVenta")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Filial" SortExpression="Filial">
                        <ItemTemplate>
                            <%# Eval("Filial")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tipo PuntoVenta" SortExpression="TipoPuntoVenta">
                        <ItemTemplate>
                            <%# Eval("TipoPuntoVenta")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tipo Factura" SortExpression="TipoFactura">
                        <ItemTemplate>
                            <%# Eval("TipoFactura")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ultimo Numero Factura" SortExpression="UltimoNumeroFacturaAnterior">
                        <ItemTemplate>
                            <%# Eval("UltimoNumeroFacturaAnterior")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                        <ItemTemplate>
                            <%# Eval("EstadoDescripcion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones">
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                AlternateText="Modificar" ToolTip="Modificar" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportarExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
