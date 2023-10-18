<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PresupuestosBuscarPopUp.ascx.cs" Inherits="IU.Modulos.Facturas.Controles.PresupuestosBuscarPopUp" %>
<script type="text/javascript" lang="javascript">
    function ShowModalBuscarPresupuesto(){
        $("[id$='modalBuscarPresupuesto']").modal('show');
    }

    function HideModalBuscarPresupuesto() {
        $("[id$='modalBuscarPresupuesto']").modal('hide');
    }
</script>
<div class="modal" id="modalBuscarPresupuesto" tabindex="-1" role="dialog" >
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
    <div class="modal-dialog modal-dialog-scrollable modal-xl" role="document">
    <div class="modal-content modal-minHeight85">
        <div class="modal-header">
        <h5 class="modal-title">Buscar Presupuesto</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
        <div class="modal-body">
            <asp:Panel ID="pnlBuscar" runat="server">
            <div class="form-group row">
                <asp:Label CssClass="col-sm-2 col-form-label" ID="lblNumeroProducto" runat="server" Text="Número Presupuesto"></asp:Label>
                <div class="col-sm-3">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroProducto" runat="server"></AUGE:NumericTextBox>
                </div>
    <asp:Label CssClass="col-sm-2 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
    <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
    </div>
                <div class="col-sm-2">
                    <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" 
        onclick="btnBuscar_Click" />
                </div>
            </div>
            </asp:Panel>
    <asp:GridView ID="gvProductos" OnRowCommand="gvProductos_RowCommand" AllowPaging="true" AllowSorting="true" 
    OnRowDataBound="gvProductos_RowDataBound" DataKeyNames="IdPresupuesto"
    runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="true"
    onpageindexchanging="gvProductos_PageIndexChanging" onsorting="gvProductos_Sorting"
    >
        <Columns>
            <asp:TemplateField HeaderText="Código" SortExpression="IdPresupuesto">
                    <ItemTemplate>
                        <%# Eval("IdPresupuesto")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Fecha" SortExpression="FechaAlta">
                    <ItemTemplate>
                        <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Descripcion" SortExpression="Producto.Descripcion">
                    <ItemTemplate>
                        <%# Eval("Descripcion")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Importe" SortExpression="ImporteTotal" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# Eval("ImporteTotal", "{0:C2}")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Acciones" >
                <ItemTemplate>
                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/vector-arrows-right.png" runat="server" CommandName="Consultar" ID="btnSeleccionar"
                         AlternateText="Seleccionar" ToolTip="Seleccionar" OnClientClick="HideModalBuscarPresupuesto();" />
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