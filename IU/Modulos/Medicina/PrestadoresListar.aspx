<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PrestadoresListar.aspx.cs" Inherits="IU.Modulos.Medicina.PrestadoresListar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="AfiliadosListar">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoDocumento" runat="server" Text="Tipo documento"></asp:Label>
                <div class="col-sm-3">  <asp:DropDownList CssClass="form-control select2" ID="ddlTipoDocumento" runat="server">
        </asp:DropDownList></div>

    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroDocumento" runat="server" Text="Número"></asp:Label>
    <div class="col-sm-3"> <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroDocumento" runat="server"></AUGE:NumericTextBox></div>

             <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMatricula" runat="server" Text="Matricula"></asp:Label>
    <div class="col-sm-3"> <AUGE:NumericTextBox CssClass="form-control select2" ID="txtMatricula" runat="server"></AUGE:NumericTextBox></div>
            </div>
             
   
<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblApellido" runat="server" Text="Apellido"></asp:Label>
     <div class="col-sm-3"><asp:TextBox CssClass="form-control" ID="txtApellido" runat="server"></asp:TextBox></div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
     <div class="col-sm-3"><asp:TextBox CssClass="form-control" ID="txtNombre"  runat="server"></asp:TextBox></div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado"  runat="server" Text="Estado"></asp:Label>
    <div class="col-sm-3"> <asp:DropDownList CssClass="form-control select2" ID="ddlEstados"  runat="server">
    </asp:DropDownList></div></div>


              
            <div class="form-group row">
                <div class="col-sm-8"></div>
                <div class="col-sm-4"> <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" 
        onclick="btnBuscar_Click" />
    <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" 
    onclick="btnAgregar_Click" /></div>

            </div>


              <div class="table-responsive">
    <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion" ShowHeaderWhenEmpty="true"
    runat="server" SkinID="GrillaResponsive"   AutoGenerateColumns="false" ShowFooter="true"
    onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting"
    >
        <Columns>
            <asp:TemplateField HeaderText="Tipo" SortExpression="TipoDocumento.TipoDocumento">
                    <ItemTemplate>
                        <%# Eval("TipoDocumento.TipoDocumento")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField  HeaderText="Número" DataField="NumeroDocumento" ItemStyle-Wrap="false" SortExpression="NumeroDocumento" />
            <asp:BoundField  HeaderText="Apellido y Nombre" DataField="ApellidoNombre" SortExpression="ApellidoNombre" />
            <asp:BoundField  HeaderText="Matricula" DataField="Matricula" SortExpression="Matricula" />
            <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                <ItemTemplate>
                    <%# Eval("Estado.Descripcion")%>
                </ItemTemplate>
            </asp:TemplateField>            
                <asp:TemplateField HeaderText="Acciones">
                    <ItemTemplate>
                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                            AlternateText="Mostrar" ToolTip="Mostrar" />
                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                                AlternateText="Modificar" ToolTip="Modificar" Visible="false" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
    </asp:GridView>
                  </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>