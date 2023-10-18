<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrdenesComprasAbiertaDatos.ascx.cs" Inherits="IU.Modulos.Compras.Controles.OrdenesComprasAbiertaDatos" %>

<%@ Register Src="~/Modulos/Proveedores/Controles/ProveedoresCabecerasDatos.ascx" TagName="BuscarProveedorAjax" TagPrefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%--<%@ Register src="~/Modulos/CuentasPagar/Controles/ProductosBuscarPopUp.ascx" tagname="popUpBuscarProducto" tagprefix="auge" %>--%>  
<%--<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>--%>


<script language="javascript" type="text/javascript">

     $(document).ready(function () {

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridDetalle);

        intiGridDetalle();

    });

    function CalcularItem() {
        var importeIVA = 0.00;
        var subTotalConIVA = 0.00;
        var TotalSinIVA = 0.00;
        var subTotalItem = 0.00;
        var totalIVA = 0.00;
        var totalConIVA = 0.00;

        $('#<%=gvItems.ClientID%> tr').each(function () {

            //var incluir = $("td:eq(6) :checkbox", this).is(":checked");
            //             var incluir = $(this).find('input:checkbox[id$="chkIncluir"]').is(":checked");

            var importe = $(this).find("input:text[id*='txtPrecioUnitario']").maskMoney('unmasked')[0]; //.val(); //$("td:eq(4)", this).html();
            var cantidad = $(this).find('input:text[id*="txtCantidad"]').val();
            var alicuotaIVA = $(this).find('[id*="ddlAlicuotaIVA"] option:selected').val();
            
            if (importe && cantidad) {
                //importe = importe.replace('.', '').replace(',', '.'); //remplazo . por nada y , por .
                subTotalItem = parseFloat(importe) * parseFloat(cantidad);
                $(this).find('span[id*="lblImporte"]').text(accounting.formatMoney(subTotalItem, "$ ", 2, "."));
                alicuotaIVA = alicuotaIVA.replace('.', '').replace(',', '.');
                if (alicuotaIVA == "" || isNaN(alicuotaIVA)) {
                    alicuotaIVA = 0.00;
                    importeIVA = 0.00;
                } else {
                    importeIVA = parseFloat(subTotalItem) * parseFloat(alicuotaIVA) / 100;
                }

                subTotalConIVA = parseFloat(subTotalItem) + parseFloat(importeIVA);
                TotalSinIVA += parseFloat(subTotalItem);
                totalIVA += parseFloat(importeIVA);
                //                 totalConIVA += parseFloat(subTotalItem) + parseFloat(importeIVA);
                $(this).find('span[id$="lblImporteIva"]').text(accounting.formatMoney(importeIVA, "$ ", 2, "."));
                $(this).find('span[id$="lblImporteConIva"]').text(accounting.formatMoney(subTotalConIVA, "$ ", 2, "."));
            }
        });
        totalConIVA = parseFloat(TotalSinIVA) + parseFloat(totalIVA);
        $("input[type=text][id$='txtTotalSinIva']").val(accounting.formatMoney(TotalSinIVA, "$ ", 2, "."));
        $("input[type=text][id$='txtTotalIva']").val(accounting.formatMoney(totalIVA, "$ ", 2, "."));
        $("input[type=text][id$='txtTotalConIva']").val(accounting.formatMoney(totalConIVA, "$ ", 2, "."));
    }

    function intiGridDetalle() {

        var rowindex = 0;
        var ddlTipoFactura = $('select[id$="ddlTipoFactura"] option:selected').val();
        var idProveedor = $("input[id*='hdfIdProveedor']").val();

        $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {
             var ddlProducto = $(this).find('[id*="ddlProducto"]');
             var hdfProductoDetalle = $(this).find("input:hidden[id*='hdfProductoDetalle']");
             var hdfIdProducto = $(this).find("input[id*='hdfIdProducto']");

             var txtCantidad = $(this).find("input:text[id*='txtCantidad']");
             var txtPrecioUnitario = $(this).find("input:text[id*='txtPrecioUnitario']");
            var hdfPrecioUnitario = $(this).find("input:hidden[id*='hdfPrecioUnitario']");

            if (hdfIdProducto.val() > 0) {
                var newOption = new Option(hdfProductoDetalle.val(), hdfIdProducto.val(), false, true);
                ddlProducto.append(newOption).trigger('change');
            }

             ddlProducto.select2({
                 placeholder: 'Ingrese el codigo o producto',
                 selectOnClose: true,
                 theme: 'bootstrap4',
                 width: '100%',
                 //theme: 'bootstrap',
                 minimumInputLength: 1,
                 language: 'es',
                 //tags: true,
                 allowClear: true,
                 ajax: {
                     type: 'POST',
                     contentType: 'application/json; charset=utf-8',
                     dataType: 'json',
                     url: '<%=ResolveClientUrl("~")%>/Modulos/Compras/ComprasWS.asmx/CMPProductosSeleccionarFiltro', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                            delay: 500,
                            data: function (params) {
                                return JSON.stringify({
                                    value: ddlProducto.val(), // search term");
                                    filtro: params.term, // search term");
                                    proveedor: idProveedor,
                                });
                            },
                            beforeSend: function (xhr, opts) {
                                var algo = JSON.parse(this.data); // this.data.split('"');
                                if (isNaN(algo.filtro)) {
                                    if (algo.filtro.length < 4) {
                                        xhr.abort();
                                    }
                                }
                                else {
                                }
                            },
                            processResults: function (data, params) {
                                //return { results: data.items };
                                return {
                                    results: $.map(data.d, function (item) {
                                        return {
                                            text: item.DescripcionCombo,
                                            id: item.IdProducto,
                                            productoDescripcion: item.Descripcion,
                                            stockActual: item.StockActual,
                                            precio: item.Precio,
                                            //IdListaPrecioDetalle: item.IdListaPrecioDetalle,
                                        }
                                    })
                                };
                                cache: true
                            }
                 }
             });
             ddlProducto.on('select2:select', function (e) {
                    hdfProductoDetalle.val(e.params.data.productoDescripcion);
                    hdfIdProducto.val(e.params.data.id);
                    txtPrecioUnitario.val(accounting.formatMoney(e.params.data.precio, gblSimbolo, 2, gblSeparadorMil));
                    hdfPrecioUnitario.val(e.params.data.precio);
                    txtCantidad.val(accounting.formatNumber(1, 2, gblSeparadorMil, ","));
                    CalcularItem();

             });
             ddlProducto.on('select2:unselect', function (e) {
                    hdfProductoDetalle.val('');
                    hdfIdProducto.val('');
                    txtPrecioUnitario.val(accounting.formatMoney(0, gblSimbolo, 2, gblSeparadorMil));
                    hdfPrecioUnitario.val('');
                    txtCantidad.val('');
                    CalcularItem();
             });
             rowindex++;
        });

     }
 



</script>

<div class="OrdenesComprasDatos">
    <asp:Panel ID="pnlCuotas" Visible="true" runat="server">
        <div class="card">
            <div class="card-header">
                Datos descuento Socio
            </div>
            <div class="card-body">
                <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuotasDescuentoAfiliado" runat="server" Text="Cuotas Descuento Socio" />
        <div class="col-sm-3">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtCuotasDescuentoAfiliado" runat="server"></AUGE:NumericTextBox>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFomraCobroAfiliado" runat="server" Text="Forma de Cobro Socio" />
        <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlFormasCobros" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuotasDescuentoProveedor" runat="server" Text="Cuotas Pago Proveedor" />
        <div class="col-sm-3">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtCuotasDescuentoProveedor" runat="server"></AUGE:NumericTextBox>
    </div>
                    </div>
                </div>
            </div>
            </asp:Panel>
  <auge:BuscarProveedorAjax ID="ctrBuscarProveedor" runat="server"></auge:BuscarProveedorAjax>


<asp:Panel ID="pnlOrdenCompra" runat="server">
     <div class="card">
            <div class="card-header">
                Orden Compra
            </div>
            <div class="card-body">
                
<%--	<asp:Label CssClass="col-sm-1 col-form-label" ID="lblCondicionPago" runat="server" Text="Condicion Pago:" />
    <asp:DropDownList CssClass="selectEvol" ID="ddlCondicionPago" runat="server"/>
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCondicionPago" ControlToValidate="ddlCondicionPago" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        <div class="EspacioValidador"></div>
    
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaEntrega" runat="server" Text="Fecha Entrega"></asp:Label>
        <asp:TextBox CssClass="textboxEvol" ID="txtFechaEntrega" Enabled="false" runat="server"></asp:TextBox>
            <div class="Calendario">
                <asp:Image ID="imgFechaEntrega" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
                <asp:CalendarExtender ID="cdFechaEntrega" runat="server" Enabled="false" 
                    TargetControlID="txtFechaEntrega" PopupButtonID="imgFechaEntrega" Format="dd/MM/yyyy"></asp:CalendarExtender>
            </div>
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaEntrega" ControlToValidate="txtFechaEntrega" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        <div class="EspacioValidador"></div> --%>
<div class="form-group row">
<asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOrden" runat="server" Text="Tipo Orden Compra:" />
<div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlTipoOrden" runat="server"/>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoOrden" ControlToValidate="ddlTipoOrden" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
                    <div class="col-sm-8"></div>
</div>

    <%--<asp:Label CssClass="col-sm-1 col-form-label" ID="lblDireccion" runat="server" Text="Direccion de entrega:" />
    <asp:TextBox CssClass="textboxEvol" ID="txtDireccion" runat="server" TextMode="MultiLine" />
    <br />--%>
                <div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblObservacion" runat="server" Text="Observacion:" />
    <div class="col-sm-7">
    <asp:TextBox CssClass="form-control" ID="txtObservacion"  runat="server" TextMode="MultiLine" />
    </div>
    <div class="col-sm-4"></div>
</div>
    <asp:UpdatePanel ID="upOrdenCompraDetalle" UpdateMode="Conditional" runat="server" >
                <ContentTemplate>         
            <%--<AUGE:popUpBuscarProducto ID="ctrBuscarProductoPopUp" runat="server" />
            <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" runat="server" Text="Agregar item" onclick="btnAgregarItem_Click" />
            <br />--%>
                                <asp:GridView ID="gvItems" AllowPaging="true" AllowSorting="true" 
                                OnRowCommand="gvItems_RowCommand"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                                onrowdatabound="gvItems_RowDataBound" 
                                >
                                    <Columns>
                                     <asp:TemplateField HeaderText="Código - Producto" SortExpression="">
                                    <ItemTemplate>
<%--                                        <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigo" AutoPostBack="true" runat="server" Text='<%#Bind("Producto.IdProducto") %>' OnTextChanged="txtCodigo_TextChanged"></AUGE:NumericTextBox>--%>
                                        <asp:DropDownList CssClass="form-control select2" ID="ddlProducto" runat="server" />
                               <asp:HiddenField ID="hdfProductoDetalle" Value='<%#Bind("Producto.Descripcion") %>' runat="server" />
                                        <asp:HiddenField ID="hdfIdProducto" Value='<%#Bind("Producto.IdProducto") %>' runat="server" />
                                    <asp:Label CssClass="col-form-label" ID="lblProducto" Visible="false" Enabled="false" Text='<%#Bind("Producto.IdProducto") %>' runat="server"></asp:Label>
                                        <asp:HiddenField ID="hdfStockeable" Value='<%#Bind("Producto.Familia.Stockeable") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                        <asp:TemplateField HeaderText ="Cant." ItemStyle-Width="5%" SortExpression ="">
                                            <ItemTemplate>
                                                <AUGE:NumericTextBox CssClass="form-control" ID="txtCantidad" runat="server" Text='<%#Bind("Cantidad") %>' ></AUGE:NumericTextBox>
                                        </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText ="Monedas" ItemStyle-Width="5%" SortExpression ="" >
                                            <ItemTemplate>
                                                <asp:DropDownList CssClass="form-control select2" ID="ddlMonedas" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText ="Prec. Unitario" ItemStyle-Width="10%" SortExpression ="">
                                            <ItemTemplate>
                                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtPrecioUnitario" runat="server" Text='<%#Bind("Precio", "{0:C2}") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <asp:TemplateField HeaderText ="Plazo Entrega" ItemStyle-Width="5%" SortExpression ="" >
                                            <ItemTemplate>
                                                <AUGE:NumericTextBox CssClass="form-control" ID="txtPlazoEntrega" runat="server"  Text='<%#Bind("PlazoEntrega") %>' ></AUGE:NumericTextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText ="Subtotal" ItemStyle-Width="10%" SortExpression ="" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporte" runat="server" Text='<%#Bind("Importe", "{0:C2}") %>' ></asp:Label>
                                                <%--<AUGE:NumericTextBox CssClass="textboxEvol" ID="txtSubtotal" runat="server" Text='<%#Bind("SubTotal", "{0:C2}") %>' Enabled="false" Width="50"></AUGE:NumericTextBox>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                       <asp:TemplateField HeaderText ="Alícuota IVA" ItemStyle-Width="10%" SortExpression ="" >
                                            <ItemTemplate>
                                                <asp:DropDownList CssClass="form-control select2" ID="ddlAlicuotaIVA" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText ="Importe IVA" ItemStyle-Width="10%" SortExpression ="" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteIva" runat="server" Text='<%#Bind("ImporteIVA", "{0:C2}") %>' ></asp:Label>
                                                <%--<AUGE:NumericTextBox CssClass="textboxEvol" ID="txtImporteIva" runat="server" Text='<%#Bind("ImporteIVA", "{0:N2}") %>'  Enabled="false" Width="50"></AUGE:NumericTextBox>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText ="Subtotal c/IVA" ItemStyle-Width="10%" SortExpression ="" ItemStyle-Wrap="false">
                                            <ItemTemplate>
                                               <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteConIva" runat="server" Text='<%#Bind("ImporteConIVA", "{0:C2}") %>' ></asp:Label>
                                               <%--<font face="arial" size="5" color="red"></Font>--%>
                                                <%--<asp:TextBox CssClass="textboxEvol" ID="txtSubtotalConIva" runat="server" Text='<%#Bind("SubTotalConIva", "{0:N2}") %>' Enabled="false" Width="50"></asp:TextBox>--%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText ="Eliminar" ItemStyle-Width="5%" SortExpression ="">
                                            <ItemTemplate>
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" 
                                                    AlternateText="Eliminar ítem" ToolTip="Eliminar ítem" Visible="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                </ContentTemplate>        
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="pnTotales" UpdateMode="Conditional" runat="server" >
                <ContentTemplate>
                    <div class="form-group row">
                    <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblTotalSinIva" runat="server" Text="Subtotal" />
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtTotalSinIva" Enabled="false" runat="server"  />
                    </div>
                        </div>
                    <div class="form-group row">
                    <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblTotalIva" runat="server" Text="IVA" />
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtTotalIva" Enabled="false" runat="server" />
                    </div>
                        </div>
                    <div class="form-group row">
                    <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblTotal" runat="server" Text="Total" />
                    <div class="col-sm-3">
                        <AUGE:NumericTextBox CssClass="form-control" ID="txtTotalConIva" Enabled="false" runat="server" />
                    </div>
                        </div>
                </ContentTemplate> 
            </asp:UpdatePanel>
                </div>
         </div>
</asp:Panel>


<asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server" >
    <ContentTemplate>
         <div class="row justify-content-md-center">
            <div class="col-md-auto">
            <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </div>
             </div>
    </ContentTemplate>
</asp:UpdatePanel> 


</div>