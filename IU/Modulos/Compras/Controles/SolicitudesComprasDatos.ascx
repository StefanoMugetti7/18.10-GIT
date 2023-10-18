<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SolicitudesComprasDatos.ascx.cs" Inherits="IU.Modulos.Compras.Controles.SolicitudesComprasDatos" %>

<%--<%@ Register src="~/Modulos/CuentasPagar/Controles/ProveedoresBuscarPopUp.ascx" tagname="popUpBuscarProveedor" tagprefix="auge" %>--%>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register src="~/Modulos/CuentasPagar/Controles/ProductosBuscarPopUp.ascx" tagname="popUpBuscarProducto" tagprefix="auge" %>  
<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>
<%@ Register src="~/Modulos/Compras/Controles/CotizacionesBuscarPopUp.ascx" tagname="popUpBuscarCotizaciones" tagprefix="auge" %> 
 <script language="javascript" type="text/javascript">

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

             var importe = $(this).find('input:text[id$="txtPrecioUnitario"]').val(); //$("td:eq(4)", this).html();
             var cantidad = $(this).find('input:text[id$="txtCantidad"]').val();
             var alicuotaIVA = $(this).find('[id$="ddlAlicuotaIVA"] option:selected').text();

             // alert(alicuotaIVA);

             if (importe && cantidad) {

                 importe = importe.replace('.', '').replace(',', '.'); //remplazo . por nada y , por .
                 //alert(importe);
                 subTotalItem = parseFloat(importe) * parseFloat(cantidad);

                 $(this).find('span[id$="lblSubtotal"]').text(accounting.formatMoney(subTotalItem, "$ ", 2, "."));

                 alicuotaIVA = alicuotaIVA.replace('.', '').replace(',', '.');
                 if (!isNaN(alicuotaIVA)) {                     

                     importeIVA = parseFloat(subTotalItem) * parseFloat(alicuotaIVA) / 100;
                     subTotalConIVA = parseFloat(subTotalItem) + parseFloat(importeIVA);

                     $(this).find('span[id$="lblImporteIva"]').text(accounting.formatMoney(importeIVA, "$ ", 2, "."));
                     $(this).find('span[id$="lblSubtotalConIva"]').text(accounting.formatMoney(subTotalConIVA, "$ ", 2, "."));
                     
                     totalIVA += parseFloat(importeIVA);
                     totalConIVA += parseFloat(subTotalConIVA);

                 }

                 TotalSinIVA += parseFloat(subTotalItem);

             }

         });

         $("input[type=text][id$='txtTotalIva']").val(accounting.formatMoney(totalIVA, "$ ", 2, "."));
         $("input[type=text][id$='txtTotalConIva']").val(accounting.formatMoney(totalConIVA, "$ ", 2, "."));
         $("input[type=text][id$='txtTotalSinIva']").val(accounting.formatMoney(TotalSinIVA, "$ ", 2, "."));

     }

</script>


<div class="SolicitudesComprasDatos">

<%--<asp:UpdatePanel ID="UpdatePanelProovedor" UpdateMode="Conditional" runat="server" >
             <ContentTemplate> 
            <asp:Panel ID="pnlProveedor" GroupingText="Datos Proveedor" runat="server">

                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigo" runat="server" Text="Codigo"></asp:Label>
                <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtCodigo" AutoPostBack="true" Enabled="false"  onTextChanged="txtCodigo_TextChanged" runat="server" />
                <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="BuscarProveedor" ID="btnBuscarProveedor" Visible="false"
                AlternateText="Buscar proveedor" ToolTip="Buscar" onclick="btnBuscarProveedor_Click"  />
                <asp:RequiredFieldValidator CssClass="Validador"  ID="rfvCodigo" ControlToValidate="txtCodigo" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                <div class="EspacioValidador"></div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRazonSocial" runat="server" Text="Razon Social" />
                <asp:TextBox CssClass="textboxEvol" ID="txtRazonSocial" Enabled="false" runat="server" />
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCUIT" Text="N° CUIT:" runat="server"/>
                <AUGE:NumericTextBox CssClass="textboxEvol" ID="txtCUIT" Enabled="false" runat="server" />

                <br />
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBeneficiario" runat="server" Text="Beneficiario"></asp:Label>
                <asp:TextBox CssClass="textboxEvol" ID="txtBeneficiario" Enabled="false" runat="server"></asp:TextBox>
                <div class="Espacio" ></div>
                
               <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCondicionFiscal" runat="server" Text="Condicion Fiscal:" Enabled="false"></asp:Label>
                <asp:DropDownList CssClass="selectEvol" ID="ddlCondicionFiscal" runat="server" Enabled = "false"> </asp:DropDownList>

                <br />
                
                <AUGE:popUpBuscarProveedor ID="ctrBuscarProveedorPopUp" runat="server" />
            </asp:Panel>

            </ContentTemplate> 
                            
            </asp:UpdatePanel>--%>

    <div class="card">
        <div class="card-header">
            Solicitud Compra
        </div>
        <div class="card-body">
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoSolicitudCompra" runat="server" Text="Tipo Solicitud" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoSolicitudCompra" runat="server" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoSolicitudCompra" ControlToValidate="ddlTipoSolicitudCompra" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPlazoEntrega" runat="server" Text="Plazo de Entrega"></asp:Label>
                <div class="col-sm-3">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtPlazoEntrega" runat="server" MaxLength="4"></AUGE:NumericTextBox>
                    <%--<asp:RequiredFieldValidator CssClass="Validador"  ID="rfvPlazoEntrega" ControlToValidate="txtPlazoEntrega" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                </div>
                <div class="col-sm-3"></div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblObservacion" runat="server" Text="Observacion" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtObservacion" Enabled="false" runat="server" TextMode="MultiLine" />
                </div>
                <div class="col-sm-8"></div>
            </div>
        </div>
    </div>

    <AUGE:popUpBuscarProducto ID="ctrBuscarProductoPopUp" runat="server" />


    <asp:UpdatePanel ID="items" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" runat="server" Text="Agregar item" OnClick="btnAgregarItem_Click" />
            <AUGE:popUpBuscarCotizaciones ID="ctrBuscarCotizacionesPopUp" runat="server" />

            <asp:GridView ID="gvItems" AllowPaging="true" AllowSorting="true"
                OnRowCommand="gvItems_RowCommand"
                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false" ShowFooter="true"
                OnRowDataBound="gvItems_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Código" SortExpression="">
                        <ItemTemplate>
                            <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigoProducto" AutoPostBack="true" runat="server" Text='<%#Bind("Producto.IdProducto") %>' OnTextChanged="txtCodigoProducto_TextChanged"></AUGE:NumericTextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" SortExpression="">
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" AutoPostBack="true" CommandName="BuscarProducto" CommandArgument='<%# Container.DisplayIndex%>' ID="btnBuscarProducto" Visible="true"
                                AlternateText="Buscar producto" ToolTip="Buscar" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Producto" SortExpression="">
                        <ItemTemplate>
                            <asp:TextBox CssClass="form-control" ID="txtProducto" runat="server" Text='<%#Bind("Producto.Descripcion") %>' Enabled="false"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cant." SortExpression="">
                        <ItemTemplate>
                            <AUGE:NumericTextBox CssClass="form-control" ID="txtCantidad" runat="server" Text='<%#Bind("Cantidad") %>'></AUGE:NumericTextBox>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Monedas" SortExpression="">
                        <ItemTemplate>
                            <asp:DropDownList CssClass="form-control select2" ID="ddlMonedas" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Prec. Unitario" SortExpression="">
                        <ItemTemplate>
                            <AUGE:CurrencyTextBox CssClass="form-control" ID="txtPrecioUnitario" runat="server" Text='<%#Bind("PrecioUnitario", "{0:N2}") %>'></AUGE:CurrencyTextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Plazo Entrega" SortExpression="">
                        <ItemTemplate>
                            <AUGE:NumericTextBox CssClass="form-control" ID="txtPlazoEntrega" runat="server" Text='<%#Bind("PlazoEntrega") %>'></AUGE:NumericTextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subtotal" SortExpression="" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSubtotal" runat="server" Text='<%#Bind("Subtotal", "{0:C2}") %>'></asp:Label>
                            <%--<AUGE:NumericTextBox CssClass="textboxEvol" ID="txtSubtotal" runat="server" Text='<%#Bind("SubTotal", "{0:C2}") %>' Enabled="false" Width="50"></AUGE:NumericTextBox>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Alícuota IVA" SortExpression="">
                        <ItemTemplate>
                            <asp:DropDownList CssClass="form-control select2" ID="ddlAlicuotaIVA" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Importe IVA" SortExpression="" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteIva" runat="server" Text='<%#Bind("ImporteIvaItem", "{0:C2}") %>'></asp:Label>
                            <%--<AUGE:NumericTextBox CssClass="textboxEvol" ID="txtImporteIva" runat="server" Text='<%#Bind("ImporteIVA", "{0:N2}") %>'  Enabled="false" Width="50"></AUGE:NumericTextBox>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Subtotal c/IVA" SortExpression="" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSubtotalConIva" runat="server" Text='<%#Bind("PrecioTotalItem", "{0:C2}") %>'></asp:Label>
                            <%--<font face="arial" size="5" color="red"></Font>--%>
                            <%--<asp:TextBox CssClass="textboxEvol" ID="txtSubtotalConIva" runat="server" Text='<%#Bind("SubTotalConIva", "{0:N2}") %>' Enabled="false" Width="50"></asp:TextBox>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Eliminar" SortExpression="">
                        <ItemTemplate>
                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar"
                                AlternateText="Eliminar ítem" ToolTip="Eliminar ítem" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cotizar" SortExpression="">
                        <ItemTemplate>
                            <asp:ImageButton ImageUrl="~/Imagenes/Ver.png" runat="server" AutoPostBack="true" CommandName="BuscarCotizacion" CommandArgument='<%# Container.DisplayIndex%>' ID="btnCotizar" Visible="false"
                                AlternateText="Cotizar detalle" ToolTip="Cotizar ítem" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

        </ContentTemplate>

    </asp:UpdatePanel>


    <asp:UpdatePanel ID="pnTotales" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-9 col-form-label text-right" ID="lblTotalSinIva" runat="server" Text="Subtotal" />
                <div class="col-sm-3">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtTotalSinIva" Enabled="false" runat="server" />
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


    <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                    <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
                      <%--<asp:Button CssClass="botonesEvol" ID="btnCalcular" runat="server" Text="Calcular" onclick="btnCalcular_Click" />--%>
                </center>
        </ContentTemplate>
    </asp:UpdatePanel>

</div>
