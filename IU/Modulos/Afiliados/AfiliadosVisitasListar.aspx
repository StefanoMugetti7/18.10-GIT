<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="AfiliadosVisitasListar.aspx.cs" Inherits="IU.Modulos.Afiliados.AfiliadosVisitasListar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
           <ContentTemplate>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroDocumento" Visible="false" runat="server" Text="Numero Documento"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="txtNumeroDocumento" Visible="false"  runat="server"></asp:DropDownList>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoDocumento" Visible="false" runat="server" Text="Tipo Documento"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoDocumento" Visible="false" runat="server"></asp:DropDownList>
        </div>
        <div class="col-sm-3"></div>
    </div>

    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNombre" Visible="false" runat="server" Text="Nombre"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtNombre" Visible="false" runat="server"></asp:TextBox>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblApellido" Visible="false" runat="server" Text="Apellido"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtApellido" Visible="false" runat="server"></asp:TextBox>
        </div>
        <div class="col-sm-3">
        </div>
    </div>

    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaHasta" runat="server" Text="Fecha Hasta"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" runat="server"></asp:TextBox>
        </div>
        <div class="col-sm-3">
                     <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" 
        onclick="btnBuscar_Click" />
    <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" 
    onclick="btnAgregar_Click" />
        </div>
    </div>


       
    <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="false" SkinID="GrillaBasicaFormal" 
    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdAfiliado,IdAfiliadoVisita" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
    runat="server" ShowFooter="true" onpageindexchanging="gvDatos_PageIndexChanging">
        <Columns>
            <asp:TemplateField HeaderText="Filial" SortExpression="Filial">
                <ItemTemplate>
                    <%# Eval("Filial")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Tipo Documento" SortExpression="TipoDocumento">
                <ItemTemplate>
                     <%# Eval("TipoDocumento")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Numero Documento" SortExpression="NumeroDocumento">
                <ItemTemplate>
                     <%# Eval("NumeroDocumento")%>
                </ItemTemplate>
            </asp:TemplateField>
<%--             <asp:BoundField  HeaderText="Apellido" DataField="Apellido" ItemStyle-Wrap="false" SortExpression="Apellido" />--%>
            <asp:TemplateField HeaderText="Apellido">
                <ItemTemplate>
                    <%# Eval("Apellido")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Nombre">
                <ItemTemplate>
                    <%# Eval("Nombre")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Fecha Ingreso">
                <ItemTemplate>
                    <%# Eval("FechaIngreso", "{0:dd/MM/yyyy}")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                <ItemTemplate>
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" Visible="false" runat="server" CommandName="Consultar" ID="btnConsultar"
                        AlternateText="Mostrar" ToolTip="Mostrar" />
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" Visible="false" runat="server" CommandName="Modificar" ID="btnModificar"
                        AlternateText="Modificar" ToolTip="Modificar" />
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                </FooterTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
                   
         </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
