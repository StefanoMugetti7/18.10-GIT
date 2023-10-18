<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false"  AutoEventWireup="true" CodeBehind="ClientesListar.aspx.cs" Inherits="IU.Modulos.Afiliados.ClientesListar" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--<script type="text/javascript" src="../../assets/js/ScrollableGridPlugin.js"></script>--%>
<%--    <script type="text/javascript">
        $(document).ready(function () {
            GridScrollable();
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function () { GridScrollable(); });
        });
        function GridScrollable() {
            $('#<%=gvDatos.ClientID %>').Scrollable();
         }

    </script>--%>
<div class="AfiliadosListar">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblIdAfiliado" runat="server" Text="Codigo"></asp:Label>
                <div class="col-sm-3">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtIdAfiliado" runat="server"></AUGE:NumericTextBox>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoDocumento" runat="server" Text="Tipo doc."></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoDocumento" runat="server">
                    </asp:DropDownList>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroDocumento" runat="server" Text="Número"></asp:Label>
                <div class="col-sm-3">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroDocumento" runat="server"></AUGE:NumericTextBox>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblApellido" runat="server" Text="Razon Social"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtApellido" runat="server"></asp:TextBox>
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
                <div class="col-sm-8"></div>
            <div class="col-sm-3">
                <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                    OnClick="btnBuscar_Click" />
                <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar"
                    OnClick="btnAgregar_Click" />
                </div>
                </div>

            <div class="form-group row">
                <div class="col-sm-12">
                    <asp:ImageButton ID="btnExportarExcel" ImageUrl="~/Imagenes/Excel-icon.png"
                        runat="server" OnClick="btnExportarExcel_Click" Visible="false" />
                </div>
            </div>
    <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdAfiliado"
    runat="server" SkinID="GrillaBasicaFormalSticky" AutoGenerateColumns="false" ShowFooter="true"
    onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting"
    >
        <Columns>
            <asp:TemplateField HeaderText="Codigo" SortExpression="NumeroSocio">
                    <ItemTemplate>
                        <%# Eval("IdAfiliado")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Tipo" SortExpression="TipoDocumentoTipoDocumento">
                    <ItemTemplate>
                        <%# Eval("TipoDocumentoTipoDocumento")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField  HeaderText="Número" DataField="NumeroDocumento" ItemStyle-Wrap="false" SortExpression="NumeroDocumento" />
            <asp:BoundField  HeaderText="Razon Social" DataField="RazonSocial" SortExpression="RazonSocial" />
            <asp:BoundField  HeaderText="Detalle" DataField="Detalle" SortExpression="Detalle" />
            <asp:TemplateField HeaderText="Condicion Fiscal" SortExpression="CondicionFiscalDescripcion">
                <ItemTemplate>
                    <%# Eval("CondicionFiscalDescripcion")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Saldo Actual" SortExpression="SaldoActual" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" FooterStyle-Wrap="false">
              <ItemTemplate>
                                    <%# string.Concat(Eval("MonedaPesos"), Eval("SaldoActual", "{0:N2}"))%>
                                </ItemTemplate>
                <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                        </FooterTemplate>
            </asp:TemplateField>
              <asp:TemplateField HeaderText="Saldo Actual Dolar" SortExpression="SaldoActualDolar" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" FooterStyle-Wrap="false">
     
                   <ItemTemplate>
                                    <%# string.Concat(Eval("MonedaDolar"), Eval("SaldoActualDolar", "{0:N2}"))%>
                                </ItemTemplate>
                <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotalDolar" runat="server" Text=""></asp:Label>
                        </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                <ItemTemplate>
                    <%# Eval("EstadoDescripcion")%>
                </ItemTemplate>
            </asp:TemplateField>            
            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" FooterStyle-Wrap="false" >
                <ItemTemplate>
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                        AlternateText="Mostrar" ToolTip="Mostrar" />
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                        AlternateText="Modificar" ToolTip="Modificar" />
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/movimientos.png" runat="server" CommandName="AgregarComprobante" ID="btnAgregarComprobante" 
                        AlternateText="Agregar Comprobante" ToolTip="Agregar Comprobante" />
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