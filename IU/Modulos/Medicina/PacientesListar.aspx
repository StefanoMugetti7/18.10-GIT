<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false"  AutoEventWireup="true" CodeBehind="PacientesListar.aspx.cs" Inherits="IU.Modulos.Afiliados.PacientesListar" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="AfiliadosListar">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
             
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoDocumento" runat="server" Text="Tipo doc."></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoDocumento" runat="server">
                    </asp:DropDownList>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroDocumento" runat="server" Text="Número"></asp:Label>
                <div class="col-sm-3">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroDocumento" runat="server"></AUGE:NumericTextBox>
                </div>
                 <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblApellido" runat="server" Text="Apellido"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtApellido" runat="server"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtNombre" runat="server"></asp:TextBox>
                </div>
               
            </div>
          
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
    <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdAfiliado"
    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
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
          <asp:TemplateField HeaderText="Apellido y Nombre" SortExpression="ApellidoNombre">
                    <ItemTemplate>
                        <%# Eval("ApellidoNombre")%>
                    </ItemTemplate>
            </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tipo de Socio" SortExpression="AfiliadoTipo">
                    <ItemTemplate>
                        <%# Eval("AfiliadoTipo")%>
                    </ItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField HeaderText="Entidad" SortExpression="ObraSocial">
                    <ItemTemplate>
                        <%# Eval("ObraSocial")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Condicion Fiscal" SortExpression="CondicionFiscalDescripcion">
                <ItemTemplate>
                    <%# Eval("CondicionFiscalDescripcion")%>
                </ItemTemplate>
            </asp:TemplateField>
           <%-- <asp:TemplateField HeaderText="Saldo Actual" SortExpression="SaldoActual" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" FooterStyle-Wrap="false">
                <ItemTemplate>
                    <%# Eval("SaldoActual", "{0:C2}")%>
                </ItemTemplate>
                <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                        </FooterTemplate>
            </asp:TemplateField>--%>
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