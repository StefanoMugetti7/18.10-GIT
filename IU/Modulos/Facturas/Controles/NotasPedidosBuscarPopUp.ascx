<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NotasPedidosBuscarPopUp.ascx.cs" Inherits="IU.Modulos.Facturas.Controles.NotasPedidosBuscarPopUp" %>
<script type="text/javascript" lang="javascript">
    function ShowModalBuscarNotaPedido() {
        $("[id$='modalBuscarNotaPedido']").modal('show');
    }

    function HideModalBuscarNotaPedido() {
        $("[id$='modalBuscarNotaPedido']").modal('hide');
    }
</script>
<div class="modal" id="modalBuscarNotaPedido" tabindex="-1" role="dialog">
    <div class="modal-dialog modal-dialog-scrollable modal-xl" role="document">
        <div class="modal-content modal-minHeight85">
            <div class="modal-header">
                <h5 class="modal-title">Buscar Nota de Pedido</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="pnlBuscar" runat="server">
                            <div class="form-group row">
                                <asp:Label CssClass="col-sm-2 col-form-label" ID="lblDescripcion" runat="server" Text="Buscar:"></asp:Label>
                                <div class="col-sm-3">
                                    <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-2">
                                    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar"
                                        OnClick="btnBuscar_Click" />
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:GridView ID="gvProductos" OnRowCommand="gvProductos_RowCommand" AllowPaging="false" AllowSorting="true"
                                    OnRowDataBound="gvProductos_RowDataBound" DataKeyNames="IdNotaPedido"
                                    runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="true"
                                    OnPageIndexChanging="gvProductos_PageIndexChanging" OnSorting="gvProductos_Sorting">
                                    <Columns>
                                        <asp:TemplateField HeaderText="#" SortExpression="IdNotaPedido">
                                            <ItemTemplate>
                                                <%# Eval("IdNotaPedido")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Descripcion" SortExpression="Descripcion">
                                            <ItemTemplate>
                                                <%# Eval("Descripcion")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Codigo Producto" SortExpression="CodigoProducto">
                                            <ItemTemplate>
                                                <%# Eval("Producto.IdProducto")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Producto" SortExpression="Producto">
                                            <ItemTemplate>
                                                <%# Eval("Producto.Descripcion")%>
                                                <asp:HiddenField ID="hdfProductoDescripcion" Value='<%# Eval("Producto.Descripcion")%>' runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Cantidad" SortExpression="Cantidad">
                                            <ItemTemplate>
                                                <%# Eval("Cantidad")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Precio Lista" SortExpression="Precio">
                                            <ItemTemplate>
                                                <itemtemplate>
                                                    <%# string.Concat(Eval("Moneda.Moneda"), Eval("Producto.Precio", "{0:N2}"))%>
                                                </itemtemplate>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAllRow(this);" Text="Incluir" TextAlign="Left" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIncluir" runat="server" onclick="CheckRow(this);" Checked='<%# Eval("Incluir")%>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="row justify-content-md-center">
                    <div class="col-md-auto">
                        <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" OnClientClick="HideModalBuscarNotaPedido();" />
                        <asp:Button CssClass="botonesEvol" ID="btnVolver" CausesValidation="false" OnClientClick="HideModalBuscarNotaPedido();" runat="server" Text="Volver" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
