<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ProcesosDatosListar.aspx.cs" Inherits="IU.Modulos.ProcesosDatos.ProcesosDatosListar" Title="" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceEncabezado" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="ProcesosDatos">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProcesos" runat="server" Text="Procesos"></asp:Label>
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlFiltro" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltro_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFiltro" Visible="true" runat="server" Text="Filtro"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtDescripcion" Visible="true" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-sm-2">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Visible="true" Text="Buscar" OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Visible="true" Text="Agregar" OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <Evol:EvolGridView ID="gvDatos" AllowPaging="true" OnRowDataBound="gvDatos_RowDataBound"
                    DataKeyNames="IdProcesoProcesamiento" OnRowCommand="gvDatos_RowCommand" OnPageIndexChanging="gvDatos_PageIndexChanging"
                    runat="server" SkinID="GrillaBasicaFormalSticky" AutoGenerateColumns="false" ShowFooter="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Codigo" SortExpression="IdProcesoProcesamiento">
                            <ItemTemplate>
                                <%# Eval("IdProcesoProcesamiento")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fecha" SortExpression="FechaEvento">
                            <ItemTemplate>
                                <%# Eval("FechaEvento")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Detalle" SortExpression="Archivo">
                            <ItemTemplate>
                                <%# Eval("Archivo")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Registros" SortExpression="RegistrosProcesados">
                            <ItemTemplate>
                                <%# Eval("RegistrosProcesados")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Usuario" SortExpression="UsuarioLogueado.Usuario">
                            <ItemTemplate>
                                <%# Eval("UsuarioLogueadoUsuario")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Estado" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("EstadoDescripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Consultar" ToolTip="Consultar" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </Evol:EvolGridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>