<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="CondicionesFiscalesTiposFacturasListar.aspx.cs" Inherits="IU.Modulos.TGE.CondicionesFiscalesTiposFacturasListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="ListasValoresListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <%--<asp:Label CssClass="labelEvol" ID="lblParametro" runat="server" Text="Codigo Relacion" />
                <asp:TextBox CssClass="textboxEvol" ID="txtParametro" runat="server"></asp:TextBox>
           <div class="Espacio"></div>
                <asp:Label CssClass="labelEvol" ID="lblCodigoDetalle" runat="server" Text="Codigo Sistema Detalle" />
                <asp:TextBox CssClass="textboxEvol" ID="txtCodigoSistema" runat="server"></asp:TextBox>
             <div class="Espacio"></div>--%>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCondicionFiscal" runat="server" Text="Condicion Fiscal" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlCondicionFiscal" runat="server"></asp:DropDownList>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoFactura" runat="server" Text="Tipo Factura" Visible="true" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoFactura" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdCondicionFiscal, IdTipoFactura"
                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
                    <Columns>
                        <asp:BoundField HeaderText="IdCondicionFiscal" DataField="IdCondicionFiscal" ItemStyle-Wrap="false" SortExpression="CondicionFiscalIdCondicionFiscal" />
                        <asp:BoundField HeaderText="CondicionFiscal" DataField="CondicionFiscalDescripcion" ItemStyle-Wrap="false" SortExpression="CondicionFiscalDescripcion" />
                        <asp:BoundField HeaderText="IdTipoFactura" DataField="IdTipoFactura" ItemStyle-Wrap="false" SortExpression="IdTipoFactura" />
                        <asp:BoundField HeaderText="TipoFactura" DataField="TipoFacturaDescripcion" ItemStyle-Wrap="false" SortExpression="TipoFacturaDescripcion" />
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnAnular"
                                    AlternateText="Eliminar" ToolTip="Eliminar" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblRegistros" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
