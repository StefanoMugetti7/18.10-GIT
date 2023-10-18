<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="CamposListar.aspx.cs" Inherits="IU.Modulos.TGE.CamposListar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="CamposListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTablaAsociada" runat="server" Text="Tabla Asociada"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTablaAsociada" runat="server">
                        </asp:DropDownList>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTitulo" runat="server" Text="Titulo"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtTitulo" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-sm-3">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTablaParametro" runat="server" Text="Tabla Parametro"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTablaParametro" runat="server">
                        </asp:DropDownList>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblValorParametro" runat="server" Text="Valor Parametro"></asp:Label>
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtValorParametro" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstados" runat="server" Text="Estado"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                    <Columns>
                        <asp:BoundField HeaderText="Nombre" DataField="Nombre" SortExpression="Nombre" />
                        <asp:BoundField HeaderText="Titulo" DataField="Titulo" SortExpression="Titulo" />
                        <asp:BoundField HeaderText="Tabla Asociada" DataField="TablaValor" SortExpression="TablaValor" />
                        <asp:BoundField HeaderText="Tabla Parametro" DataField="Tabla" SortExpression="Tabla" />
                        <asp:BoundField HeaderText="Valor Parametro" DataField="IdRefTabla" SortExpression="IdRefTabla" />
                        <%--<asp:BoundField  HeaderText="Valor Detalle" DataField="ListaValor.ListaValor" SortExpression="IdRefTabla" />--%>
                        <asp:BoundField HeaderText="Requerido" DataField="Requerido" SortExpression="Requerido" />
                        <asp:BoundField HeaderText="Orden" DataField="Orden" SortExpression="Orden" />
                        <asp:BoundField Visible="false" HeaderText="Minimo" DataField="TamanioMinimo" SortExpression="TamanioMinimo" />
                        <asp:BoundField Visible="false" HeaderText="Maximo" DataField="TamanioMaximo" SortExpression="TamanioMaximo" />
                        <asp:TemplateField HeaderText="Campo Tipo" SortExpression="CampoTipo.CampoTipo">
                            <ItemTemplate>
                                <%# Eval("CampoTipo.CampoTipo")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" HeaderText="Tipo Control" SortExpression="CampoTipo.Control">
                            <ItemTemplate>
                                <%# Eval("CampoTipo.Control")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("Estado.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Salto Linea" DataField="SaltoLinea" SortExpression="SaltoLinea" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
