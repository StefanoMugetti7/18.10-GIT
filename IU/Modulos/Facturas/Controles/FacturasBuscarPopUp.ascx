<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FacturasBuscarPopUp.ascx.cs" Inherits="IU.Modulos.Facturas.Controles.FacturasBuscarPopUp" %>

        <script type="text/javascript">
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

            function ShowModalBuscarFactura() {
                $("[id$='modalBuscarFactura']").modal('show');
            }

            function HideModalBuscarFactura() {
                $("[id$='modalBuscarFactura']").modal('hide');
            }

            function CheckUno(objRef) {
                $('#<%=gvAcopios.ClientID%> tr').not(':first').not(':last').each(function () {
                    $(this).find("input:checkbox[id*='chkIncluir']").prop("checked", false);
                });
                var row = objRef.parentNode.parentNode;
                $(row).find("input:checkbox[id*='chkIncluir']").prop("checked", true);
            }
        </script>
<div class="modal" id="modalBuscarFactura" tabindex="-1" role="dialog">
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
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFacturas" runat="server" Text="Factura" />
                            <div class="col-sm-3">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlFacturas" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFacturas_SelectedIndexChanged" />
                                <asp:RequiredFieldValidator ID="rfvFacturas" ValidationGroup="Buscar" ControlToValidate="ddlFacturas" CssClass="Validador" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <asp:PlaceHolder ID="phAcopios" runat="server">
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <asp:Label CssClass="col col-form-label" ID="lblTituloAcopios" runat="server" Text="Acopios Pendientes de Entrega"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <asp:GridView ID="gvAcopios" DataKeyNames="IdFactura"
                                        runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Fecha Factura" SortExpression="FechaFactura">
                                                <ItemTemplate>
                                                    <%# Eval("FechaFactura", "{0:dd/MM/yyyy}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tipo y Numero Factura" SortExpression="TipoNumeroFactura">
                                                <ItemTemplate>
                                                    <%# Eval("TipoNumeroFactura")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Importe" SortExpression="ImporteSinIVA" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%# Eval("ImporteSinIVA", "{0:C2}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Importe Pendiente" SortExpression="Cantidad" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%# Eval("ImportePendiente", "{0:C2}")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIncluir" runat="server" onclick="CheckUno(this);" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="row justify-content-md-center">
                                <div class="col-md-auto">
                                    <asp:Button CssClass="botonesEvol" ID="btnAceptarAcopio" runat="server" Text="Aceptar" OnClientClick="HideModalBuscarFactura();" OnClick="btnAceptarAcopio_Click" CausesValidation="false" />
                                    <asp:Button CssClass="botonesEvol" ID="btnCancelarAcopio" runat="server" Text="Volver" data-dismiss="modal" />
                                </div>
                            </div>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="phDatos" runat="server">
                            <div class="form-group row">
                                <div class="col-sm-12">
                                    <asp:GridView ID="gvDatos"
                                        OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IdFacturaDetalle"
                                        runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="true" >

                                        <Columns>
                                            <asp:TemplateField HeaderText="Tipo Y Numero de Factura" SortExpression="TipoNumeroFactura">
                                                <ItemTemplate>
                                                    <%# Eval("TipoNumeroFactura")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Codigo" SortExpression="IdProducto">
                                                <ItemTemplate>
                                                    <%# Eval("IdProducto")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Producto" SortExpression="ProductoDetalle">
                                                <ItemTemplate>
                                                    <%# string.Concat(Eval("ProductoDescripcion"), ". ", Eval("ProductoDetalle"))%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stock Actual" SortExpression="StockActual">
                                                <ItemTemplate>
                                                    <%# Eval("StockActual")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Entregado" SortExpression="CantidadEntregada">
                                                <ItemTemplate>
                                                    <%# Eval("CantidadEntregada")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pedido" SortExpression="Cantidad">
                                                <ItemTemplate>
                                                    <%# Eval("Cantidad")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pendiente" SortExpression="CantidadPendiente">
                                                <ItemTemplate>
                                                    <%# Eval("CantidadPendiente")%>
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
                            <div class="row justify-content-md-center">
                                <div class="col-md-auto">
                                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClientClick="HideModalBuscarFactura();" OnClick="btnAceptar_Click" CausesValidation="false" />
                                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" data-dismiss="modal" />
                                </div>
                            </div>
                        </asp:PlaceHolder>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
