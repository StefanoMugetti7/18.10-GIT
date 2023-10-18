<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ProductosListar.aspx.cs" Inherits="IU.Modulos.Compras.ProductosListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="ProductosListar">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProducto" runat="server" Text="Producto" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFamilias" runat="server" Text="Familia" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlFamilias" runat="server" />
           </div>
                <div class="col-sm-3">
            <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
            <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" onclick="btnAgregar_Click" />
            </div>
                </div>
            <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
           </div>
                <div class="col-sm-8"></div>
                </div>
                <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true" 
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdProducto"
                runat="server" SkinID="GrillaBasicaFormalSticky" AutoGenerateColumns="false" ShowFooter="true"
                onpageindexchanging="gvDatos_PageIndexChanging" onsorting="gvDatos_Sorting" >
                    <Columns>
                        <asp:BoundField  HeaderText="Codigo" ItemStyle-HorizontalAlign="Right" DataField="IdProducto" SortExpression="IdProducto" />
                        <asp:BoundField  HeaderText="Producto" DataField="Descripcion" SortExpression="Descripcion" />
                        <asp:TemplateField HeaderText="Familia" SortExpression="FamiliaDescripcion">
                            <ItemTemplate>
                                <%# Eval("FamiliaDescripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Iva" SortExpression="AliCuotaIva">
                            <ItemTemplate>
                                <%# Eval("AliCuotaIva")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField  HeaderText="Compra" DataField="Compra" SortExpression="Compra" />
                        <asp:BoundField  HeaderText="Venta" DataField="Venta" SortExpression="Venta" />                        
                        <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                            <ItemTemplate>
                                <%# Eval("EstadoDescripcion")%>
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
                </Evol:EvolGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>