<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RemitosBuscarPopUp.ascx.cs" Inherits="IU.Modulos.Facturas.Controles.RemitosBuscarPopUp" %>

<script type="text/javascript">

    function CheckRow(objRef) {
        var row = objRef.parentNode.parentNode;
        var GridView = row.parentNode;
        var inputList = GridView.getElementsByTagName("input");
        for (var i = 0; i < inputList.length; i++) {
            var headerCheckBox = inputList[0];
            var checked = true;
            if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                if (!inputList[i].checked) {
                    checked = false;
                    break;
                }
            }
        }
        headerCheckBox.checked = checked;
        ValidarListaPrecio()
    }

    function checkAllRow(objRef) {
        var GridView = objRef.parentNode.parentNode.parentNode;
        var inputList = GridView.getElementsByTagName("input");
        for (var i = 0; i < inputList.length; i++) {
            //Get the Cell To find out ColumnIndex
            var row = inputList[i].parentNode.parentNode;
            if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                if (objRef.checked) {
                    if (inputList[i].disabled == false) {
                        inputList[i].checked = true;
                    }
                    else {
                        inputList[i].checked = false;
                    }
                }
                else {
                    inputList[i].checked = false;
                }
            }
        }
        ValidarListaPrecio()
    }

    function ValidarListaPrecio() {
        var mostrar = false;
        var MsgDetalle = 'Los siguientes Productos no se pueden seleccionar por no estar en una lista de Precios: <BR />';
        var separador = ' ';
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            var listaPrecio = $(this).find("input[id*='hdfListaPrecio']").val();
            var bcheck = $(this).find("input[id*='chkIncluir']").prop('checked');
            if ((listaPrecio == 'false' || listaPrecio == 'False') && bcheck) {
                mostrar = true;
                MsgDetalle += separador + $(this).find("input[id*='hdfProductoDescripcion']").val();
                separador = ' | ';
                $(this).find("input[id*='chkIncluir']").prop('checked', false);
            }
        });
        if (mostrar) {
            MostrarMensaje(MsgDetalle);
        }
    }

    function ShowModalBuscarRemito(){
        $("[id$='modalBuscarRemito']").modal('show');
    }

    function HideModalBuscarRemitoto() {
        $("[id$='modalBuscarRemito']").modal('hide');
    }


</script>
<div class="modal" id="modalBuscarRemito" tabindex="-1" role="dialog">
    <div class="modal-dialog modal-dialog-scrollable modal-xl" role="document">
        <div class="modal-content modal-minHeight85">
            <div class="modal-header">
                <h5 class="modal-title">Buscar Remito</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoRemito" runat="server" Text="Codigo Remito" />
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigoRemito" runat="server" />
                    </div>
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroRemito" runat="server" Text="Numero Remito" />
                    <div class="col-sm-1">
                        <asp:TextBox CssClass="form-control" ID="txtNumeroRemitoPrefijo" Enabled="true" MaxLength="4" runat="server" />
                    </div>
                    <div class="col-sm-2">
                        <asp:TextBox CssClass="form-control" ID="txtNumeroRemitoSuFijo" Enabled="true" MaxLength="8" runat="server" />
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
                    <div class="col-sm-4">
                        <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" OnClick="btnBuscar_Click" />
                    </div>
                </div>
                
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand" AllowPaging="false" AllowSorting="true"
                                    OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                                    runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="true"
                                    OnPageIndexChanging="gvDatos_PageIndexChanging" OnSorting="gvDatos_Sorting">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Número Remito" SortExpression="NumeroRemito">
                                            <ItemTemplate>
                                                <%# Eval("NumeroRemitoCompleto")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Fecha Remito" SortExpression="FechaRemito">
                                            <ItemTemplate>
                                                <%# Eval("FechaRemito", "{0:dd/MM/yyyy}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="Codigo Producto" SortExpression="CodigoProducto">
                                            <ItemTemplate>
                                                <%# Eval("Producto.IdProducto")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
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
                                                  <ItemTemplate>
                                                  <%# string.Concat(Eval("Moneda.Moneda"), Eval("Producto.Precio", "{0:N2}"))%>
                                            </ItemTemplate>     </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="checkAll" runat="server" onclick="checkAllRow(this);" Text="Incluir" TextAlign="Left" />
                                            </HeaderTemplate>

                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIncluir" runat="server" onclick="CheckRow(this);" Checked='<%# Eval("Incluir")%>' />
                                                <asp:HiddenField ID="hdfListaPrecio" Value='<%# Eval("ListaPrecio")%>' runat="server" />
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
                        <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" OnClientClick="HideModalBuscarRemitoto();" />
                        <asp:Button CssClass="botonesEvol" ID="btnVolver" CausesValidation="false"  OnClientClick="HideModalBuscarRemitoto();" runat="server" Text="Volver" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
