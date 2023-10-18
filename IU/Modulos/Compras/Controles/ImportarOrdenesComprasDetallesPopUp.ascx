<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportarOrdenesComprasDetallesPopUp.ascx.cs" Inherits="IU.Modulos.Compras.Controles.ImportarOrdenesComprasDetallesPopUp" %>

<script type="text/javascript" lang="javascript">
    function ShowModalBuscarOrdenesComprasDetalles() {
        $("[id$='modalBuscarOrdenesComprasDetalles']").modal('show');
    }
    function HideModalBuscarOrdenesComprasDetalles() {
        $("[id$='modalBuscarOrdenesComprasDetalles']").modal('hide');
    }

    function CheckRow(objRef) {
        //Get the Row based on checkbox
        var row = objRef.parentNode.parentNode;
        //Get the reference of GridView
        var GridView = row.parentNode;
        //Get all input elements in Gridview
        var inputList = GridView.getElementsByTagName("input");
        for (var i = 0; i < inputList.length; i++) {
            //The First element is the Header Checkbox
            var headerCheckBox = inputList[0];
            //Based on all or none checkboxes
            //are checked check/uncheck Header Checkbox
            var checked = true;
            if (inputList[i].type == "checkbox" && inputList[i]
                != headerCheckBox) {
                if (!inputList[i].checked) {
                    checked = false;
                    break;
                }
            }
        }
        headerCheckBox.checked = checked;
    }

    function checkAllRow(objRef) {
        var GridView = objRef.parentNode.parentNode.parentNode;
        var inputList = GridView.getElementsByTagName("input");
        for (var i = 0; i < inputList.length; i++) {
            //Get the Cell To find out ColumnIndex
            var row = inputList[i].parentNode.parentNode;
            if (inputList[i].type == "checkbox" && objRef
                != inputList[i]) {
                if (objRef.checked) {
                    inputList[i].checked = true;
                }
                else {
                    inputList[i].checked = false;
                }
            }
        }
    }
</script>
<div class="modal" id="modalBuscarOrdenesComprasDetalles" tabindex="-1" role="dialog">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="modal-dialog modal-dialog-scrollable modal-xl" role="document">
                <div class="modal-content modal-minHeight85">
                    <div class="modal-header">
                        <h5 class="modal-title">Buscar Items</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoOrden" runat="server" Text="Codigo Orden Compra"></asp:Label>
                            <div class="col-sm-3">
                                <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigoOrdenCompra" runat="server"></AUGE:NumericTextBox>
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCondicionPago" runat="server" Text="Condicion de pago" />
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlCondicionPago" runat="server" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesde" runat="server" Text="Fecha Desde"></asp:Label>
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesde" runat="server"></asp:TextBox>
                            </div>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaHasta" runat="server" Text="Fecha Hasta"></asp:Label>
                            <div class="col-sm-3">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaHasta" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <asp:PlaceHolder ID="phDatos" runat="server">
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <asp:GridView ID="gvDatos" DataKeyNames="IdOrdenCompraDetalle"
                                        runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Código Orden" SortExpression="CodigoOrden">
                                                <ItemTemplate>
                                                    <%# Eval("IdOrdenCompraDetalle")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Descripcion">
                                                <ItemTemplate>
                                                    <%# Eval("ProductoDescripcion")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Cantidad Pendiente">
                                                <ItemTemplate>
                                                    <%# Eval("CantidadPendiente")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Observacion" SortExpression="Observacion">
                                                <ItemTemplate>
                                                    <%# Eval("Observacion")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Fecha Orden Compra" SortExpression="FechaOrden">
                                                <ItemTemplate>
                                                    <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="checkAll" runat="server" onclick="checkAllRow(this);" Text="Incluir" TextAlign="Left" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIncluir" runat="server" onclick="CheckRow(this);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="row justify-content-md-center">
                                <div class="col-md-auto">
                                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" Visible="false" runat="server" Text="Aceptar" OnClientClick="HideModalBuscarOrdenesComprasDetalles();" OnClick="btnAceptar_Click" CausesValidation="false" />
                                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" Visible="false" runat="server" Text="Volver" data-dismiss="modal" />
                                </div>
                            </div>
                        </asp:PlaceHolder>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
