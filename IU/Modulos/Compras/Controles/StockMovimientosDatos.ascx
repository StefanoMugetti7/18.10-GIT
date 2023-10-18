   <%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StockMovimientosDatos.ascx.cs" Inherits="IU.Modulos.Compras.Controles.StockMovimientosDatos" %>

<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>

<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>


<script language="javascript" type="text/javascript">

    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CopmletarCerosComprobantes);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridDetalle);
        intiGridDetalle();
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(CalcularValorizacion);
        SetTabIndexInput();
        //CopmletarCerosComprobantes();
        CalcularValorizacion();
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

        var stockFinal = 0.00;

        $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {

            var cantidad = $(this).find('input:text[id*="txtCantidad"]').val();
            var stockActual = $(this).find('span[id*="lblStockActual"]').text();
         

            if (cantidad) {
                cantidad = cantidad.replace('.', '').replace(',', '.');
                stockActual = stockActual.replace('.', '').replace(',', '.');
              
                stockFinal = parseFloat(stockActual) - parseFloat(cantidad);
    
                $(this).find('span[id*="lblStockFinal"]').text(accounting.formatNumber(parseFloat(stockFinal), 2, gblSeparadorMil, ","));
            }
        });
    }

    function CalcularItemAgregar() {

        var stockFinal = 0.00;

        $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {

            var cantidad = $(this).find('input:text[id*="txtCantidad"]').val();
            var stockActual = $(this).find('span[id*="lblStockActual"]').text();

            if (cantidad) {
                cantidad = cantidad.replace('.', '').replace(',', '.');
                stockActual = stockActual.replace('.', '').replace(',', '.');
            
                stockFinal = parseFloat(stockActual) + parseFloat(cantidad);
                     
  
                $(this).find('span[id*="lblStockFinal"]').text(accounting.formatNumber(parseFloat(stockFinal), 2, gblSeparadorMil, ","));
            }
        });
    }

     function CalcularValorizacion() {
        var subTotal = 0.00;
        var total = 0.00;
        $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {
            //var hdfPrecioUnitario = $(this).find("input[id*='hdfPrecioUnitario']");
            var hdfSubTotal = $(this).find("input[id*='hdfSubTotal']");
            var txtCantidad = $(this).find("input:text[id*='txtCantidad']");
            var cantidad = txtCantidad.maskMoney('unmasked')[0];
            var txtPrecioUnitario = $(this).find("input:text[id*='txtPrecioUnitario']");
            var precioUnitario = txtPrecioUnitario.maskMoney('unmasked')[0];
            var codigoProducto = $(this).find("input:text[id*='txtCodigoProducto']");
            //var precioUnitario = hdfPrecioUnitario.val();
            //precioUnitario = precioUnitario.replace('.', '').replace(',', '.');
            var lblSubTotal = $(this).find("span[id*='lblSubTotal']");
            subTotal = 0.00;
            if (precioUnitario && cantidad && codigoProducto) {
                subTotal = parseFloat(precioUnitario) * parseFloat(cantidad);
                total += parseFloat(subTotal);                
                hdfSubTotal.val(subTotal);
            }
            lblSubTotal.text(accounting.formatMoney(subTotal, gblSimbolo, 2, "."));
        });
        $("#<%=gvItems.ClientID %> [id$=lblTotal]").text(accounting.formatMoney(total, gblSimbolo, 2, "."));
    }


     function intiGridDetalle() {
            var rowindex = 0;
         var hdfIdProveedor = 0;//$("input[id*='hdfIdProveedor']").val();
         var idFilial =  $('select[id$="ddlFilial"] option:selected').val();
            $('#<%=gvItems.ClientID%> tr').not(':first').not(':last').each(function () {
                /*Variables Productos*/
                var ddlProducto = $(this).find('[id*="ddlProducto"]');
                var hdfProductoDetalle = $(this).find("input:hidden[id*='hdfProductoDetalle']");
                  var hdfStockActual = $(this).find("input:hidden[id*='hdfStockActual']");
                var hdfIdProducto = $(this).find("input[id*='hdfIdProducto']");
                var txtCantidad = $(this).find("input:text[id*='txtCantidad']");
                 var txtStockFinal = $(this).find("input[id*='lblStockFinal']");
               var txtPrecioUnitario = $(this).find('[id*="txtPrecioUnitario"]');
                var lblStockActual = $(this).find('[id*="lblStockActual"]');
                //var hdfPrecio = $(this).find("input:hidden[id*='hdfPrecio']");
                //var ddlTipoComprobante = $('select[id$="ddlTipoComprobante"] option:selected').val();
                /*-----------------------------------------------------------------------------------*/

                /*ddlProducto*/
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
                        url: '<%=ResolveClientUrl("~")%>/Modulos/Compras/ComprasWS.asmx/CMPProductosSeleccionarStockFiltro', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                        delay: 500,
                        data: function (params) {
                            return JSON.stringify({
                                value: ddlProducto.val(), // search term");
                                filtro: params.term, // search term");
                                idFilial: idFilial,
                            });
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
                                        precio: item.PrecioUnitario,

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
                    //txtPrecio.val(accounting.formatMoney(e.params.data.precio, gblSimbolo, 2, gblSeparadorMil));
                    //hdfPrecio.val(e.params.data.precio);
                   hdfStockActual.val(accounting.formatNumber(e.params.data.stockActual, 2, gblSeparadorMil, ","));
                    txtCantidad.val(accounting.formatNumber(1, 2, gblSeparadorMil, ","));
                    txtPrecioUnitario.val(accounting.formatNumber(e.params.data.precio, 2, gblSeparadorMil, ","));
                    lblStockActual.text(accounting.formatNumber(e.params.data.stockActual, 2, gblSeparadorMil, ","));
  
                 
                    //hdfNoIncluidoEnAcopio.val(e.params.data.noIncluidoEnAcopio);

                    CalcularItemAgregar();
                    
                });
                ddlProducto.on('select2:unselect', function (e) {
                    hdfProductoDetalle.val('');
                    hdfIdProducto.val('');
                    hdfStockActual.val('')
                    txtStockFinal.val('0');
                    lblStockActual.text('0');
                    txtPrecioUnitario.val('0');
                    
                    //txtPrecio.val(accounting.formatMoney(0, gblSimbolo, 2, gblSeparadorMil));
                    //hdfPrecio.val('');
                    txtCantidad.val('0');
                    //hdfNoIncluidoEnAcopio.val('');
                    CalcularItem();
                });
                /*Fin ddlProducto*/
                /*------------------------------------------------------------------------------------------*/ 

                rowindex++;
            });
    }
     function UpdPanelUpdate() {
        __doPostBack("<%=button.UniqueID %>", "");
        //document.getElementById('<%= button.ClientID %>').click();        
    }



</script>

                                <asp:HiddenField ID="hdfIdProveedor" runat="server" />

<div class="StockMovimientos">
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFecha" runat="server" Text="Fecha:" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control datepicker" ID="txtFecha" runat="server" Enabled="false" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipo" runat="server" Text="Tipo:" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlTipo" runat="server" Enabled="false"></asp:DropDownList>
        </div>
        <div class="col-sm-3"></div>
    </div>
    <asp:UpdatePanel ID="upFilial" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial:" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlFilial" runat="server" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlFilial_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilialDestino" runat="server" Text="Filial Destino:" Visible="false" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlFilialDestino" runat="server" Enabled="true" Visible="false"></asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFilialDestino" Enabled="false" ControlToValidate="ddlFilialDestino" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
                <div class="col-sm-3"></div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripción:" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server" Enabled="false" />
        </div>
        <div class="col-sm-8"></div>
    </div>

    <!--Grilla-->
    <asp:UpdatePanel ID="upTabControl" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpGrilla" HeaderText="Detalles de Movimientos">
            <ContentTemplate>

                <asp:UpdatePanel ID="upGrilla" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" runat="server" Text="Agregar item" OnClick="btnAgregarItem_Click" />
                        
                  
                        <div class="data-table">
                        <asp:GridView ID="gvItems" AllowSorting="true"
                            OnRowCommand="gvItems_RowCommand" DataKeyNames="IndiceColeccion"
                            runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                            OnRowDataBound="gvItems_RowDataBound">
                            <Columns>

                                <asp:TemplateField HeaderText="Producto" SortExpression="">
                                    <ItemTemplate>
                                       <asp:DropDownList CssClass="form-control select2" ID="ddlProducto" OnTextChanged="txtCodigoProducto_TextChanged" runat="server"></asp:DropDownList>
                                <asp:HiddenField ID="hdfIdProducto" Value="" runat="server" />

                                <asp:HiddenField ID="hdfProductoDetalle" Value=""  runat="server" />
                                         <asp:Label CssClass="col-form-label" ID="lblProducto" Visible="false" Enabled="false"  runat="server"></asp:Label>
                              
                         </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Stock Actual" SortExpression="">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdfStockActual" Value='<%#Bind("StockActual") %>' runat="server" />
                                        <asp:Label CssClass="gvLabel" ID="lblStockActual" runat="server" Text='<%#Bind("StockActual") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Cantidad" SortExpression="">
                                    <ItemTemplate>
                                        <Evol:CurrencyTextBox CssClass="form-control" Prefix=" " ID="txtCantidad" runat="server" Text='<%#Bind("Cantidad") %>'></Evol:CurrencyTextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Stock Final" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label CssClass="form-control" ID="lblStockFinal" runat="server" Text='<%#Bind("StockFinal") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Valorización" SortExpression="">
                                    <ItemTemplate>
                                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtPrecioUnitario" runat="server" Text='<%#Eval("PrecioUnitario","{0:C2}") %>'></Evol:CurrencyTextBox>
                                        <asp:HiddenField ID="hdfPrecioUnitario" Value='<%#Eval("PrecioUnitario", "{0:N2}") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SubTotal" FooterStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-CssClass="text-right" SortExpression="">
                                    <ItemTemplate>
                                        <asp:Label CssClass="col-form-label" ID="lblSubTotal" runat="server"></asp:Label>
                                    <asp:HiddenField ID="hdfSubTotal" runat="server" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label CssClass="labelFooterEvol" ID="lblTotal" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Acciones" SortExpression="">
                                    <ItemTemplate>
                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar"
                                            AlternateText="Eliminar ítem" ToolTip="Eliminar ítem" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                            </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </ContentTemplate>
        </asp:TabPanel>
         <asp:TabPanel runat="server" ID="tpImportarArchivo" HeaderText="Importar Archivo">
                <ContentTemplate>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <asp:Button CssClass="botonesEvol" ID="button" OnClick="button_Click" runat="server" Style="display: none;" />
                    <asp:Button CssClass="botonesEvol" ID="btnDescargarPlanCuentas" runat="server" Text="Descargar Plantilla" OnClick="btnDescargarPlantilla_Click" CausesValidation="false" />
                            </div>
                    </div>
                    <div class="form-group row">
                    <asp:Label CssClass="col-sm-12 col-form-label" ID="lblColumnas" runat="server" Width="100%" Text="Nombre de Columnas Obligatorias"></asp:Label>
                    </div>
                    <div class="form-group row">
                    <asp:Label CssClass="col-sm-12 col-form-label" ID="lblColumnasDetalles" runat="server" Width="100%" Text="IdProducto, Descripcion, Stock Actual, Cantidad, IdFilial, Valorizacion"></asp:Label>
                    </div>
                    <div class="form-group row">
                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblArchivo" runat="server" Text="Adjuntar archivo"></asp:Label>
                    <asp:AsyncFileUpload ID="afuArchivo" OnClientUploadComplete="UpdPanelUpdate" OnUploadedComplete="uploadComplete_Action" CssClass="imageUploaderField" ToolTip="Seleccione archivo" runat="server"
                        UploadingBackColor="#CCFFFF" ThrobberID="imgUploadFile" UploaderStyle="Traditional" />
                    <asp:ImageButton ID="imgUploadFile" Visible="false" ImageUrl="~/Imagenes/updateprogress.gif" runat="server" />
                    </div>
                </ContentTemplate>
            </asp:TabPanel>
        <!-- TERMINA TAB Panel de la grilla-->

        <asp:TabPanel runat="server" ID="tpComentarios" HeaderText="Comentarios">
            <ContentTemplate>
                <AUGE:Comentarios ID="ctrComentarios" runat="server" />d
            </ContentTemplate>
        </asp:TabPanel>

        <asp:TabPanel runat="server" ID="tpArchivos" HeaderText="Archivos">
            <ContentTemplate>
                <AUGE:Archivos ID="ctrArchivos" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>

        <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria">
            <ContentTemplate>
                <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>

    </asp:TabContainer>
        </ContentTemplate>
         <Triggers>
        <asp:PostBackTrigger ControlID="tcDatos$tpImportarArchivo$btnDescargarPlanCuentas" />
    </Triggers>
        </asp:UpdatePanel>
    <!-- TERMINA EL TAB CONTAINER-->

    <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                    <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
                </center>
        </ContentTemplate>
    </asp:UpdatePanel>

</div>
