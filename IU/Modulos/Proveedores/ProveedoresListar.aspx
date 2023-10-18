<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ProveedoresListar.aspx.cs" Inherits="IU.Modulos.Proveedores.ProveedoresListar" Title="" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="AfiliadosListar">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroProveedor" runat="server" Text="Número Proveedor"></asp:Label>
                <div class="col-sm-3">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroProveedor" runat="server" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuit" runat="server" Text="CUIT"></asp:Label>
                <div class="col-sm-3">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtCuit" runat="server" />
                </div>
                <div class="col-sm-3">
                    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                        OnClick="btnBuscar_Click" />
                    <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"
                        OnClick="btnAgregar_Click" />
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRazonSocial" runat="server" Text="Razón Social"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtRazonSocial" runat="server"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDetalle" runat="server" Text="Detalle"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtDetalle" runat="server"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>







                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTieneSaldo" runat="server" Text="Tiene Saldo"></asp:Label>
                <div class="col-sm-3">
                    <asp:CheckBox ID="chkTieneSaldo" CssClass="form-control" runat="server" />
                </div>
                <div class="col-sm-8"></div>
            </div>
            <AUGE:CamposValores ID="ctrCamposValores" runat="server" />

            <div class="form-group row">
                <div class="col-sm-12">
                    <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                        runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                </div>
            </div>
    <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdProveedor"
    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
    onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting"
    >
        <Columns>
            <asp:TemplateField HeaderText="Número Proveedor" SortExpression="IdProveedor">
                    <ItemTemplate>
                        <%# Eval("IdProveedor")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Razón Social" SortExpression="RazonSocial">
                    <ItemTemplate>
                        <%# Eval("RazonSocial")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="CUIT" SortExpression="CUIT">
                <ItemTemplate>
                    <%# Eval("CUIT")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Saldo Actual" SortExpression="SaldoActual" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" FooterStyle-Wrap="false">
                <ItemTemplate>
                    <%# Eval("SaldoActual", "{0:C2}")%>
                </ItemTemplate>
                <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                        </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                <ItemTemplate>
                    <%# Eval("EstadoDescripcion")%>
                </ItemTemplate>
            </asp:TemplateField>            
             <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                        AlternateText="Mostrar" ToolTip="Mostrar" />
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
</div>
</asp:Content>