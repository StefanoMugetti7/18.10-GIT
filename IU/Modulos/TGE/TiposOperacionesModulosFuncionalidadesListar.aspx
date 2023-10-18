<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="TiposOperacionesModulosFuncionalidadesListar.aspx.cs" Inherits="IU.Modulos.TGE.TiposOperacionesModulosFuncionalidadesListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="ListasValoresListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo Operacion" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoOperacion" runat="server"></asp:DropDownList>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblModuloSistema" runat="server" Text="Modulo Sistema" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlModuloSistema" runat="server"></asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoFuncionalidad" runat="server" Text="Tipo Funcionalidad" Visible="true" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoFuncionalidad" runat="server"></asp:DropDownList>
                    </div>
                </div>
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdTipoOperacion,IdTipoFuncionalidad,IdModuloSistema"
                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
                    <Columns>
                        <asp:BoundField HeaderText="IdModuloSistema" DataField="IdModuloSistema" ItemStyle-Wrap="false" SortExpression="IdModuloSistema" />
                        <asp:BoundField HeaderText="Modulo Sistema" DataField="ModuloSistema" ItemStyle-Wrap="false" SortExpression="ModuloSistema" />
                        <asp:BoundField HeaderText="IdTipoOperacion" DataField="IdTipoOperacion" ItemStyle-Wrap="false" SortExpression="IdTipoOperacion" />
                        <asp:BoundField HeaderText="Tipo Operacion" DataField="TipoOperacion" ItemStyle-Wrap="false" SortExpression="TipoOperacion" />
                        <asp:BoundField HeaderText="IdTipoFuncionalidad" DataField="IdTipoFuncionalidad" ItemStyle-Wrap="false" SortExpression="IdTipoFuncionalidad" />
                        <asp:BoundField HeaderText="Tipo Funcionalidad" DataField="TipoFuncionalidad" ItemStyle-Wrap="false" SortExpression="TipoFuncionalidad" />
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnAnular"
                                    AlternateText="Eliminar" ToolTip="Eliminar" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblRegistros" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
