<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductosBuscarPopUp.ascx.cs" Inherits="IU.Modulos.Facturas.Controles.BuscarProductoPopUp" %>

<script type="text/javascript" lang="javascript">
    function ShowModalBuscarProducto(){
        $("[id$='modalBuscarProducto']").modal('show');
    }

    function HideModalBuscarProducto() {
        $("[id$='modalBuscarProducto']").modal('hide');
    }
</script>
<div class="modal" id="modalBuscarProducto" tabindex="-1" role="dialog" >
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
    <div class="modal-dialog modal-dialog-scrollable modal-xl" role="document">
    <div class="modal-content modal-minHeight85">
        <div class="modal-header">
        <h5 class="modal-title">Buscar Producto</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
        <div class="modal-body">
            <asp:Panel ID="pnlBuscar" runat="server">
            <div class="form-group row">
                <asp:Label CssClass="col-sm-2 col-form-label" ID="lblNumeroProducto" runat="server" Text="Número Producto"></asp:Label>
                <div class="col-sm-3">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroProducto" runat="server"></AUGE:NumericTextBox>
                </div>
    <asp:Label CssClass="col-sm-2 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
    <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
    </div>
                <div class="col-sm-2"></div>
            </div>
            <div class="form-group row">
            <asp:Label CssClass="col-sm-2 col-form-label" ID="lblFamilia" runat="server" Text="Familia"></asp:Label>
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlFamilias" runat="server">
            </asp:DropDownList>
                </div>
                <div class="col-sm-5"></div>
                <div class="col-sm-2">
                <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" 
        onclick="btnBuscar_Click" />
                 </div>
        </div>
            </asp:Panel>
    <asp:GridView ID="gvProductos" OnRowCommand="gvProductos_RowCommand" AllowPaging="true" AllowSorting="true" 
    OnRowDataBound="gvProductos_RowDataBound" DataKeyNames="IndiceColeccion"
    runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="true"
    onpageindexchanging="gvProductos_PageIndexChanging" onsorting="gvProductos_Sorting"
    >
        <Columns>
            <asp:TemplateField HeaderText="Código" SortExpression="Producto.IdProducto">
                    <ItemTemplate>
                        <%# Eval("Producto.IdProducto")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Descripcion" SortExpression="Producto.Descripcion">
                    <ItemTemplate>
                        <%# Eval("Producto.Descripcion")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Familia" SortExpression="Producto.Familia.Descripcion">
                    <ItemTemplate>
                        <%# Eval("Producto.Familia.Descripcion")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Precio" SortExpression="Precio" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# Eval("Precio", "{0:C2}")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Stock Actual" SortExpression="StockActual" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# Eval("StockActual")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Acciones" >
                <ItemTemplate>
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Consultar" ID="btnSeleccionar"
                         AlternateText="Seleccionar" ToolTip="Seleccionar" OnClientClick="HideModalBuscarProducto();" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    </div>
    </div>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</div>