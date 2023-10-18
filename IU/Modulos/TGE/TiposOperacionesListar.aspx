<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="TiposOperacionesListar.aspx.cs" Inherits="IU.Modulos.TGE.TiposOperacionesListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="TiposOperacionesListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoOperacion" runat="server" Text="Codigo Operación"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigoOperacion" runat="server"></AUGE:NumericTextBox>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo Operación"></asp:Label>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:TextBox CssClass="form-control" ID="txtTipoOperacion" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoMovimiento" runat="server" Text="Tipo Movimiento" />
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoMovimiento" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstados" runat="server" Text="Estado"></asp:Label>
                    
                    <div class="col-lg-3 col-md-3 col-sm-9">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server" />
                    </div>
                </div>
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdTipoOperacion"
                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
                    <Columns>
                        <asp:BoundField HeaderText="Codigo Op." DataField="IdTipoOperacion" ItemStyle-Wrap="false" SortExpression="IdTipoOperacion" />
                        <asp:BoundField HeaderText="Tipo Operacion" DataField="TipoOperacion" ItemStyle-Wrap="false" SortExpression="TipoOperacion" />
                        <asp:BoundField HeaderText="Tipo Movimiento" DataField="TipoMovimientoTipoMovimiento" ItemStyle-Wrap="false" SortExpression="TipoMovimiento" />
                        <asp:TemplateField HeaderText="Estado" ItemStyle-Wrap="false" SortExpression="Estado.Descripcion">
                            <ItemTemplate>
                                <%# Eval("EstadoDescripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                    AlternateText="Mostrar" ToolTip="Mostrar" />
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                    AlternateText="Modificar" ToolTip="Modificar" />
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
