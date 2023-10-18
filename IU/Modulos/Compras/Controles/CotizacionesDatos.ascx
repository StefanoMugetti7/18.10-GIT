<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CotizacionesDatos.ascx.cs" Inherits="IU.Modulos.Compras.Controles.CotizacionesDatos" %>
<%@ Register src="~/Modulos/CuentasPagar/Controles/ProveedoresBuscarPopUp.ascx" tagname="popUpBuscarProveedor" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register src="~/Modulos/CuentasPagar/Controles/ProductosBuscarPopUp.ascx" tagname="popUpBuscarProducto" tagprefix="auge" %>  
<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>
<%@ Register Src="~/Modulos/Proveedores/Controles/ProveedoresCabecerasDatos.ascx" TagName="BuscarProveedorAjax" TagPrefix="auge" %>


<div class="CotizacionesDatos">

<script language="javascript" type="text/javascript">


     $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
  
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CalcularItem);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridDetalle);
          intiGridDetalle();
        SetTabIndexInput();
  
        CalcularItem();
        $.fn.addLeadingZeros = function (length) {
            for (var el of this) {
                _value = el.value.replace(/^0+/, '');
                length = length - _value.length;
                if (length > 0) {
                    while (length--) _value = '0' + _value;
                }
                el.value = _value;
            }
        };
    });

     function SetTabIndexInput() {
        $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }
    function CalcularItem() {
        var descuentoTotal = 0.00;
        $('#<%=gvItems.ClientID%> tr').each(function () {

            var descuento = $(this).find('input:text[id$="txtDescuento"]').val(); ; //$("td:eq(4)", this).html();
            //alert(descuento);
            if (descuento) {
                descuento = descuento.replace('.', '').replace(',', '.'); //remplazo . por nada y , por .
                descuentoTotal += parseFloat(descuento);
                //alert(descuentoTotal);
                //HASTA ACA la variable descuentoTotal se carga correctamente, rompe abajo ¬
            }
        });
        //ESTA LINEA NO HACE LO QUE DEBERIA, FALTA QUE SE CARGUE EL LBL "txtDescuentoTotal". Le puse TXT porque ya hay un LBLdesctotal
        $(this).find('span[id$="txtDescuentoTotal"]').text(accounting.formatMoney(descuentoTotal, "$ ", 2, "."));

    }



   function intiGridDetalle() {
        var rowindex = 0;
        var idSolicitudPago = $("input[type=hidden][id$='hdfIdSolicitudPago']").val();

        var cantidadCuotas = 1; //$('select[id$="ddlCantidadCuotas"] option:selected').val();

        
            $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {
                var ddlProducto = $(this).find('[id*="ddlProducto"]');
                    var hdfProductoDetalle = $(this).find("input:hidden[id*='hdfProductoDetalle']");
                var hdfIdProducto = $(this).find("input[id*='hdfIdProducto']");
                var hdfStockeable = $(this).find("input[id*='hdfStockeable']");

                var txtCantidad = $(this).find("input:text[id*='Cantidad']");
                var txtPrecioUnitario = $(this).find("input:text[id*='txtPrecioUnitario']");
                var hdfPrecioUnitario = $(this).find("input:hidden[id*='hdfPrecioUnitario']");
           
                var hdfNoIncluidoEnAcopio = $(this).find("input:hidden[id*='hdfNoIncluidoEnAcopio']");
                var lblSubTotal = $(this).find('span[id*="lblSubTotal"]');

                if (hdfIdProducto.val() > 0) {
                var newOption = new Option(hdfProductoDetalle.val(), hdfIdProducto.val(), false, true);
                ddlProducto.append(newOption).trigger('change');
                }

                if (idSolicitudPago > 0) {
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
                                   
                                    cantidadCuotas: cantidadCuotas,
                                });
                                //var Productos = ObtenerProductosSeleccionadas();
                                //console.log(" array " + Productos);
                                //return "{filtro:" + JSON.stringify(params.term) + ", ListaProductos:" + JSON.stringify(Productos) + "}";
                            },
                            beforeSend: function (xhr, opts) {
                              
                                var algo = JSON.parse(this.data); // this.data.split('"');
                                console.log(algo.filtro);
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
                                            precio: item.Precio,
                                            precioUnitarioSinIva: item.precioUnitarioSinIva,
                                            noIncluidoEnAcopio: item.NoIncluidoEnAcopio,
                                            stockeable: item.Stockeable,
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
                        txtPrecioUnitario.val(accounting.formatMoney(e.params.data.precioUnitarioSinIva, gblSimbolo, 2, gblSeparadorMil));
                        hdfPrecioUnitario.val(e.params.data.precioUnitarioSinIva);
                        txtCantidad.val(accounting.formatNumber(1, 2, gblSeparadorMil, ","));
                        hdfNoIncluidoEnAcopio.val(e.params.data.noIncluidoEnAcopio);
                        hdfStockeable.val(e.params.data.stockeable);
                        //CalcularPrecio();
                        CalcularItem();
                       

                    });
                    ddlProducto.on('select2:unselect', function (e) {
                            txtCantidad.val('0,00');
              hdfProductoDetalle.val('');
                        hdfIdProducto.val('');
                        txtPrecioUnitario.val(accounting.formatMoney(0, gblSimbolo, 2, gblSeparadorMil));
                        hdfPrecioUnitario.val('');
                        hdfStockeable.val('');
                        hdfNoIncluidoEnAcopio.val('');
                        lblSubTotal.text(accounting.formatMoney(0, gblSimbolo, 2, "."));
                        CalcularItem();
                             
                    });

                }
                else {

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
                              
                                });
                                //var Productos = ObtenerProductosSeleccionadas();
                                //console.log(" array " + Productos);
                                //return "{filtro:" + JSON.stringify(params.term) + ", ListaProductos:" + JSON.stringify(Productos) + "}";
                            },
                            beforeSend: function (xhr, opts) {
                              
                                var algo = JSON.parse(this.data); // this.data.split('"');
                                console.log(algo.filtro);
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
                                            stockeable: item.Stockeable,
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
                        txtPrecioUnitario.val(accounting.formatMoney(e.params.data.precioUnitarioSinIva, gblSimbolo, 2, gblSeparadorMil));
                        hdfPrecioUnitario.val(e.params.data.precioUnitarioSinIva);
                        txtCantidad.val(accounting.formatNumber(1, 2, gblSeparadorMil, ","));
                        hdfNoIncluidoEnAcopio.val(e.params.data.noIncluidoEnAcopio);
                        hdfStockeable.val(e.params.data.stockeable);
                             CalcularItem();
                    });
                    ddlProducto.on('select2:unselect', function (e) {
                        txtCantidad.val('0,00');
                        hdfProductoDetalle.val('');
                        
                        hdfIdProducto.val('');
                        txtPrecioUnitario.val(accounting.formatMoney(0, gblSimbolo, 2, gblSeparadorMil));
                        hdfPrecioUnitario.val('');
                        hdfStockeable.val('');
                        hdfNoIncluidoEnAcopio.val('');
                          lblSubTotal.text(accounting.formatMoney(0, gblSimbolo, 2, "."));
                        CalcularItem();
                           
                        //CalcularItem();
                    });
                }
                rowindex++;
            });
       
    }


</script>

<asp:UpdatePanel ID="UpdatePanelProovedor" UpdateMode="Conditional" runat="server" >
             <ContentTemplate> 


                    <auge:BuscarProveedorAjax ID="ctrBuscarProveedor" runat="server"></auge:BuscarProveedorAjax>

    <%--        <asp:Panel ID="pnlProveedor" GroupingText="Datos Proveedor" runat="server">

                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCodigo" runat="server" Text="Codigo"></asp:Label>
                <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigo" AutoPostBack="true" Enabled="false"  onTextChanged="txtCodigo_TextChanged" runat="server" />
                <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="BuscarProveedor" ID="btnBuscarProveedor" Visible="false"
                AlternateText="Buscar proveedor" ToolTip="Buscar" onclick="btnBuscarProveedor_Click"  />
                <asp:RequiredFieldValidator CssClass="Validador"  ID="rfvCodigo" ControlToValidate="txtCodigo" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                <div class="EspacioValidador"></div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblRazonSocial" runat="server" Text="Razon Social" />
                <asp:TextBox CssClass="form-control" ID="txtRazonSocial" Enabled="false" runat="server" />
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCUIT" Text="N° CUIT:" runat="server"/>
                <AUGE:NumericTextBox CssClass="form-control" ID="txtCUIT" Enabled="false" runat="server" />

                <br />
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblBeneficiario" runat="server" Text="Beneficiario"></asp:Label>
                <asp:TextBox CssClass="form-control" ID="txtBeneficiario" Enabled="false" runat="server"></asp:TextBox>
                <div class="Espacio" ></div>
                
               <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCondicionFiscal" runat="server" Text="Condicion Fiscal:" Enabled="false"></asp:Label>
                <asp:DropDownList CssClass="form-control select2" ID="ddlCondicionFiscal" runat="server" Enabled = "false"> </asp:DropDownList>

                <br />
                
                <AUGE:popUpBuscarProveedor ID="ctrBuscarProveedorPopUp" runat="server" />
            </asp:Panel>--%>

            </ContentTemplate> 
</asp:UpdatePanel>

<asp:Panel ID="pnlCotizaciones" GroupingText="Cotizaciones" runat="server">
          <ContentTemplate>        <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCondicionPago" runat="server" Text="Condicion Pago" />
               <div class="col-lg-3 col-md-3 col-sm-9">  <asp:DropDownList CssClass="form-control select2" ID="ddlCondicionPago" Enabled="false" runat="server"/>
                <asp:RequiredFieldValidator CssClass="Validador"  ID="rfvCondicionPago" ControlToValidate="ddlCondicionPago" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
           </div>

                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblMail" runat="server" Text="Mail" ></asp:Label>
              <div class="col-lg-3 col-md-3 col-sm-9">   <asp:TextBox CssClass="form-control" ID="txtMail" Enabled="false" runat="server" />
             </div>   <%--<asp:RequiredFieldValidator CssClass="Validador"  ID="rfvPlazoEntrega" ControlToValidate="txtPlazoEntrega" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
        
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFechaRecibido" runat="server" Text="Fecha Recibido"></asp:Label>
         <div class="col-lg-3 col-md-3 col-sm-9">    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaRecibido" Enabled="true" runat="server"></asp:TextBox>
        </div></div>
              <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCostoFlete" runat="server" Text="Costo Flete" />
               <div class="col-lg-3 col-md-3 col-sm-9">  <AUGE:CurrencyTextBox CssClass="form-control" ID="txtCostoFlete" Enabled="false" runat="server" ></AUGE:CurrencyTextBox>
        </div>
            </div>  <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblObservacion" runat="server" Text="Observacion" />
                <div class="col-lg-3 col-md-3 col-sm-9"> <asp:TextBox CssClass="form-control" ID="txtObservacion" Enabled="false" runat="server" TextMode="MultiLine"/>
                </div></div>
          </ContentTemplate>
</asp:Panel>

<AUGE:popUpBuscarProducto ID="ctrBuscarProductoPopUp" runat="server" />

            
            <br />
      
      
      <asp:UpdatePanel ID="items" UpdateMode="Conditional" runat="server" >
           
                <ContentTemplate>    
          
                <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" Visible="false" runat="server" Text="Agregar item" onclick="btnAgregarItem_Click" />       
        

                    <div class="table-responsive">
                                <asp:GridView ID="gvItems" AllowPaging="true" AllowSorting="true" 
                                OnRowCommand="gvItems_RowCommand"
                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                                onrowdatabound="gvItems_RowDataBound" 
                                >
                                    <Columns>
                                           <asp:TemplateField HeaderText="Código - Producto" SortExpression="">
                                    <ItemTemplate>
<%--                                        <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigo" AutoPostBack="true" runat="server" Text='<%#Bind("Producto.IdProducto") %>' OnTextChanged="txtCodigo_TextChanged"></AUGE:NumericTextBox>--%>
                                        <asp:DropDownList CssClass="form-control select2" ID="ddlProducto" runat="server" Enabled="false"></asp:DropDownList>
                               <asp:HiddenField ID="hdfProductoDetalle" Value='<%#Bind("Producto.Descripcion") %>' runat="server" />
                                        <asp:HiddenField ID="hdfIdProducto" Value='<%#Bind("Producto.IdProducto") %>' runat="server" />
                                    <asp:Label CssClass="col-form-label" ID="lblProducto" Visible="false" Enabled="false" Text='<%#Bind("Producto.IdProducto") %>' runat="server"></asp:Label>
                                        <asp:HiddenField ID="hdfStockeable" Value='<%#Bind("Producto.Familia.Stockeable") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                      
                                     
                                        <asp:TemplateField HeaderText ="Cant." SortExpression ="">
                                            <ItemTemplate>
                                             <AUGE:CurrencyTextBox CssClass="form-control" ID="txtCantidad" runat="server" Text='<%#Bind("Cantidad") %>' ></AUGE:CurrencyTextBox>
                                            
                                     </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText ="Monedas" SortExpression ="" >
                                            <ItemTemplate>
                                          <asp:DropDownList CssClass="form-control select2" ID="ddlMonedas" runat="server" />
                                       </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText ="Prec. Unitario" SortExpression ="">
                                            <ItemTemplate>
                                             <AUGE:CurrencyTextBox CssClass="form-control" ID="txtPrecioUnitario" runat="server" Text='<%#Bind("PrecioUnitario", "{0:N2}") %>'></AUGE:CurrencyTextBox>
                                      <asp:HiddenField ID="hdfPrecioUnitario" Value='<%#Bind("PrecioUnitario", "{0:N2}") %>' runat="server" />
                                                </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText ="Precio Cant." SortExpression ="">
                                            <ItemTemplate>
                                        <AUGE:CurrencyTextBox CssClass="form-control" ID="txtPrecioCantidad" runat="server" Text='<%#Bind("PrecioCantidad", "{0:N2}") %>'></AUGE:CurrencyTextBox>
                                         </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText ="Descuento" SortExpression ="">
                                            <ItemTemplate>
                                                <AUGE:CurrencyTextBox CssClass="form-control" ID="txtDescuento" runat="server" Text='<%#Bind("Descuento", "{0:N2}") %>'></AUGE:CurrencyTextBox>
                                        </ItemTemplate>
                                        </asp:TemplateField>

                                       <asp:TemplateField HeaderText ="Plazo Entrega" SortExpression ="" >
                                            <ItemTemplate>
                                              <AUGE:NumericTextBox CssClass="form-control" ID="txtPlazoEntrega" runat="server"  Text='<%#Bind("PlazoEntrega") %>' ></AUGE:NumericTextBox>
                                     </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                       <asp:TemplateField HeaderText ="Alícuota IVA" SortExpression ="" >
                                            <ItemTemplate>
                                          <asp:DropDownList CssClass="form-control select2" ID="ddlAlicuotaIVA" runat="server" />
                                      </ItemTemplate>
                                        </asp:TemplateField>
                                        
                                        <asp:TemplateField HeaderText ="Eliminar" SortExpression ="">
                                            <ItemTemplate>
                                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" 
                                                    AlternateText="Eliminar ítem" ToolTip="Eliminar ítem" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView></div>

                </ContentTemplate> 
                            
            </asp:UpdatePanel>
      
   <asp:UpdatePanel ID="pnlDesc" UpdateMode="Conditional" runat="server" >   
                <ContentTemplate>
                <br />
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDescuentoTotal" runat="server" Text="Descuento Total" />
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="txtDescuentoTotal" runat="server" Text='0.00' />
                <br />
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDescuentoDetalle" runat="server" Text="Detalle del Descuento" />
              <div class="col-lg-3 col-md-3 col-sm-9">   <asp:TextBox CssClass="form-control" ID="txtDescuentoDetalle" Enabled="false" runat="server" TextMode="MultiLine"/>
             </div>   </ContentTemplate>
   </asp:UpdatePanel>
           
<asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server" >
            <ContentTemplate>
               <div class="row justify-content-md-center">
            <div class="col-md-auto">
                    <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                 
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
                
                      <%--<asp:Button CssClass="botonesEvol" ID="btnCalcular" runat="server" Text="Calcular" onclick="btnCalcular_Click" />--%>
        </div></div>
            </ContentTemplate>
    
    
    </asp:UpdatePanel> 

</div>





