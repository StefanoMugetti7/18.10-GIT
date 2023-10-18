<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="CategoriasListar.aspx.cs" Inherits="IU.Modulos.Afiliados.CategoriasListar" Title="" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCategoria" Width="100px" runat="server" Text="Categoría" />
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtCategoria" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoCategoria" Width="100px" runat="server" Text="Tipo de Categoria" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoCategoria" Width="200px" runat="server" />
                    </div>
                    <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" Width="100px" runat="server" Text="Estado" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" Width="200px" runat="server" />






                    </div>

                </div>
            </div>
            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                <Columns>
                    <asp:BoundField HeaderText="IdCategoria" DataField="IdCategoria" ItemStyle-Wrap="false" SortExpression="IdCategoria" />
                    <asp:BoundField HeaderText="Código" DataField="Codigo" ItemStyle-Wrap="false" SortExpression="Codigo" />

                    <asp:BoundField HeaderText="Categoría" DataField="Categoria" SortExpression="Categoria" />
                    <asp:BoundField HeaderText="Tipo Categoría" DataField="TipoCategoria" SortExpression="Categoria" />
                    <%--                        <asp:BoundField  HeaderText="Importe Cuota" DataField="ImporteCuota" SortExpression="ImporteCuota" />
                        <asp:BoundField  HeaderText="Importe Gasto" DataField="ImporteGasto" SortExpression="ImporteCuota" />--%>
                    <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                        <ItemTemplate>
                            <%# Eval("Estado.Descripcion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                AlternateText="Mostrar" ToolTip="Mostrar" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                AlternateText="Modificar" ToolTip="Modificar" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

