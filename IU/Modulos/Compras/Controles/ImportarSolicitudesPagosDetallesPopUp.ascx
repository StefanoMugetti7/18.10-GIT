<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImportarSolicitudesPagosDetallesPopUp.ascx.cs" Inherits="IU.Modulos.Compras.Controles.ImportarSolicitudesPagosDetallesPopUp" %>

<script type="text/javascript" lang="javascript">
    function ShowModalBuscarSolicitudesPagosDetalles(){
        $("[id$='modalBuscarSolicitudesPagosDetalles']").modal('show');
    }

    function HideModalBuscarSolicitudesPagosDetalles() {
        $("[id$='modalBuscarSolicitudesPagosDetalles']").modal('hide');
    }

    function CheckUno(objRef) {
        $('#<%=gvAcopios.ClientID%> tr').not(':first').not(':last').each(function () {
            $(this).find("input:checkbox[id*='chkIncluir']").prop("checked",false);
        });
        var row = objRef.parentNode.parentNode;
        $(row).find("input:checkbox[id*='chkIncluir']").prop("checked", true);
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
<div class="modal" id="modalBuscarSolicitudesPagosDetalles" tabindex="-1" role="dialog" >
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
                <asp:Label CssClass="col-sm-2 col-form-label" ID="lblNumeroFactura" runat="server" Text="Número Factura"></asp:Label>
                <div class="col-sm-3">
                <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroFactura" runat="server"></AUGE:NumericTextBox>
                </div>
                <div class="col-sm-2">
                <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
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
    <asp:GridView ID="gvAcopios" DataKeyNames="IdSolicitudPago"
    runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="true" >
        <Columns>
            <asp:TemplateField HeaderText="Nro. Sol. Pago" SortExpression="IdSolicitudPago">
                    <ItemTemplate>
                        <%# Eval("IdSolicitudPago")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Fecha Factura" SortExpression="FechaFactura">
                    <ItemTemplate>
                        <%# Eval("FechaFactura", "{0:dd/MM/yyyy}")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Tipo y Numero Factura" SortExpression="NumeroFactura">
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
                    <asp:Button CssClass="botonesEvol" ID="btnAceptarAcopio" runat="server" Text="Aceptar" OnClientClick="HideModalBuscarSolicitudesPagosDetalles();" OnClick="btnAceptarAcopio_Click" CausesValidation="false" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelarAcopio" runat="server" Text="Volver" data-dismiss="modal" />
                </div>
            </div>
                </asp:PlaceHolder>
            <asp:PlaceHolder ID="phDatos" runat="server">
 <div class="form-group row">
     <div class="col-sm-12">
         <asp:Label CssClass="col col-form-label" ID="lblTituloDatos" runat="server" Text="Productos Pendientes de Entrega"></asp:Label>
         </div>
     </div>
                <div class="form-group row">
     <div class="col-sm-12">
    <asp:GridView ID="gvDatos" DataKeyNames="IdSolicitudPagoDetalle"
    runat="server" SkinID="GrillaBasicaPopUp" AutoGenerateColumns="false" ShowFooter="true" >
        <Columns>
            <asp:TemplateField HeaderText="Nro. Sol. Pago" SortExpression="IdSolicitudPago">
                    <ItemTemplate>
                        <%# Eval("IdSolicitudPago")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Codigo" SortExpression="IdProducto">
                    <ItemTemplate>
                        <%# Eval("IdProducto")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Descripcion" SortExpression="Descripcion">
                    <ItemTemplate>
                        <%# Eval("Descripcion")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Fecha Factura" SortExpression="FechaFactura">
                    <ItemTemplate>
                        <%# Eval("FechaFactura", "{0:dd/MM/yyyy}")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Tipo y Numero Factura" SortExpression="NumeroFactura">
                    <ItemTemplate>
                        <%# Eval("TipoNumeroFactura")%>
                    </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Cantidad Pendiente" SortExpression="Cantidad" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# Eval("Cantidad")%>
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
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" Visible="false" runat="server" Text="Aceptar" OnClientClick="HideModalBuscarSolicitudesPagosDetalles();" OnClick="btnAceptar_Click" CausesValidation="false" />
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