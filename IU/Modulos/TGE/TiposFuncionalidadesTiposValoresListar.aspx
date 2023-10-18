<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="TiposFuncionalidadesTiposValoresListar.aspx.cs" Inherits="IU.Modulos.TGE.TiposFuncionalidadesTiposValoresListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="ListasValoresListar">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <%--<asp:Label CssClass="labelEvol" ID="lblParametro" runat="server" Text="Codigo Relacion" />
                <asp:TextBox CssClass="textboxEvol" ID="txtParametro" runat="server"></asp:TextBox>
           <div class="Espacio"></div>
                <asp:Label CssClass="labelEvol" ID="lblCodigoDetalle" runat="server" Text="Codigo Sistema Detalle" />
                <asp:TextBox CssClass="textboxEvol" ID="txtCodigoSistema" runat="server"></asp:TextBox>
             <div class="Espacio"></div>--%>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoValor" runat="server" Text="Tipo Valor" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoValor" runat="server">
                        </asp:DropDownList>
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoFuncionalidad" runat="server" Text="Tipo Funcionalidad" Visible="true" />
                    <div class="col-sm-3">
                        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoFuncionalidad" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-2">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                        <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
                    </div>
                </div>
                <%-- <asp:Label CssClass="labelEvol" ID="lblEstado"  runat="server" Text="Estado"></asp:Label>
            <asp:DropDownList CssClass="selectEvol" ID="ddlEstados"  runat="server">
            </asp:DropDownList>
              <br /> --%>
                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdTipoFuncionalidad, IdTipoValor"
                    runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true">
                    <Columns>
                        <asp:BoundField HeaderText="IdTipoValor" DataField="IdTipoValor" ItemStyle-Wrap="false" SortExpression="IdTipoValor" />
                        <asp:BoundField HeaderText="Tipo Valor" DataField="TipoValor" ItemStyle-Wrap="false" SortExpression="TipoValor" />
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
