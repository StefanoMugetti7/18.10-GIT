<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="DescuentosListar.aspx.cs" Inherits="IU.Modulos.Hotel.DescuentosListar" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/select2/js/select2.full.min.js"></script>
    <script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/select2/js/i18n/es.js"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
            SetTabIndexInput();
        });

        function SetTabIndexInput() {
            $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblHotel" runat="server" Text="Hotel"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlHoteles" runat="server"></asp:DropDownList>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoDescuento" runat="server" Text="Tipo Descuento"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoDescuento" runat="server"></asp:DropDownList>
                </div>
                <div class="col-sm-4">
                    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                    <asp:Button CssClass="botonesEvol" ID="btnAgregar" runat="server" Text="Agregar" OnClick="btnAgregar_Click" />
                </div>
            </div>
            <Evol:EvolGridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="true" AllowSorting="true"
                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdDescuento"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                <Columns>
                    <asp:TemplateField HeaderText="Hotel" SortExpression="Hotel">
                        <ItemTemplate>
                            <%# Eval("Hotel")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tipo Descuento" SortExpression="TipoDescuento">
                        <ItemTemplate>
                            <%# Eval("Descripcion")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Porcentaje" SortExpression="Porcentaje">
                        <ItemTemplate>
                            <%# Eval("DescuentoPorcentaje")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe" SortExpression="Importe">
                        <ItemTemplate>
                            <%# Eval("DescuentoImporte")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                AlternateText="Mostrar" ToolTip="Mostrar" />
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                AlternateText="Modificar" ToolTip="Modificar" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </Evol:EvolGridView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
