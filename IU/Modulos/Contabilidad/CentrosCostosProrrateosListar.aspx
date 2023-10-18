<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="CentrosCostosProrrateosListar.aspx.cs" Inherits="IU.Modulos.Contabilidad.CentrosCostosProrrateosListar" %>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="CentrosCostosProrrateosListar">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
              <div class="form-group row">
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCentroCostoProrrateo" runat="server" Text="Escenario de Centros de Costos" />
           <div class="col-lg-3 col-md-3 col-sm-9">  <asp:TextBox CssClass="form-control" ID="txtCentrosCostosProrrateo" runat="server" />
    </div>
            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
          <div class="col-lg-3 col-md-3 col-sm-9">   <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
    </div>
              <div class="col-lg-3 col-md-3 col-sm-9">  <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click" /></div>
    </div>

            <div class="data-table">
            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
            OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
            runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
            onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                <Columns>
                    <asp:TemplateField HeaderText="Escenarios de Centros de Costos" SortExpression="CentroCostoProrrateo">
                        <ItemTemplate>
                            <%# Eval("CentroCostoProrrateo")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Filial" SortExpression="Filial.Filial">
                        <ItemTemplate>
                            <%# Eval("Filial.Filial")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                        <ItemTemplate>
                            <%# Eval("Estado.Descripcion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" >
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                AlternateText="Mostrar" ToolTip="Mostrar" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" 
                                AlternateText="Modificar" ToolTip="Modificar" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    </Columns>
            </asp:GridView></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>

