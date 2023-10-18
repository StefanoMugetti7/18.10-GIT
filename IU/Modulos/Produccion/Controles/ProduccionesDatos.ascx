<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProduccionesDatos.ascx.cs" Inherits="IU.Modulos.Produccion.Controles.ProduccionesDatos" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" TagName="popUpBotonConfirmar" TagPrefix="auge" %>
<script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/select2/js/select2.full.min.js"></script>
<script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/select2/js/i18n/es.js"></script>

<AUGE:popUpBotonConfirmar ID="popUpConfirmar" runat="server" />
<script type="text/javascript">

    $(document).ready(function () {
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(intiGridDetalle);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(initSelects);
        SetTabIndexInput();
        intiGridDetalle();
        initSelects();
    });

    function SetTabIndexInput() {
        $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }

    function initSelects() {
        var ddlProductoProducido = $('select[id$="ddlProductoProducido"]');
        ddlProductoProducido.select2({
            placeholder: 'Ingrese el Producto',
            selectOnClose: true,
            width: '100%',
            //theme: 'bootstrap',
            minimumInputLength: 4,
            language: 'es',
            //tags: true,
            allowClear: true,
            ajax: {
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                url: '<%=ResolveClientUrl("~")%>/Modulos/Produccion/ProduccionWS.asmx/ProduccionSeleccionarAjaxComboProductos', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                delay: 500,
                data: function (params) {
                    return JSON.stringify({
                        filtro: params.term
                    });
                },
                processResults: function (data, params) {
                    //return { results: data.items };
                    return {
                        results: $.map(data.d, function (item) {
                            return {
                                text: item.text,// + ' | Stock: ' + item.Producto.StockActual,
                                id: item.id
                            }
                        })
                    };
                    cache: true
                }
            }
        });
    }
    /******************************************************
        Grilla Detalle
    *******************************************************/
    function intiGridDetalle() {
        var idFilial = $('select[id$="ddlFiliales"] option:selected').val();
        var idProduccion = $('input[id$="txtNumeroProduccion"]').val();
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            var ddlTipoProducto = $(this).find('[id*="ddTipoMovimiento"]');
            var ddlDetalleGastos = $(this).find('[id*="ddlProductos"]');
            var hdfDetalleGastos = $(this).find("input[id*='hdfDetalleProducto']");
            var txtCantidad = $(this).find("input:text[id*='txtCantidad']");
            var hdfValorizacion = $(this).find("input[id*='hdfValorizacion']");
            var lblValorizacion = $(this).find("span[id*='lblValorizacion']");
            var hdfStockActual = $(this).find("input[id*='hdfStockActual']");
            var lblStockActual = $(this).find("span[id*='lblStockActual']");
            var lblUnidadMedida = $(this).find("span[id*='lblUnidadMedida']");
            var hdfUnidadMedidaDescripcion = $(this).find("input[id*='hdfUnidadMedidaDescripcion']");
            var hdfIdUnidadMedida = $(this).find("input[id*='hdfIdUnidadMedida']");
            var hdfSubTotal = $(this).find("input[id*='hdfSubTotal']");
            var lblSubTotal = $(this).find("span[id*='lblSubTotal']");

            txtCantidad.blur(CalcularValorizacion);
            ddlTipoProducto.select2({ width: '100%' });
            ddlTipoProducto.change(function () {
                ddlDetalleGastos.val(null).trigger('change');
                ddlDetalleGastos.focus();
            });
            ddlDetalleGastos.select2({
                placeholder: 'Ingrese el Producto',
                selectOnClose: true,
                theme: 'bootstrap4',
                minimumInputLength: 4,
                language: 'es',
                allowClear: true,
                ajax: {
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    url: '<%=ResolveClientUrl("~")%>/Modulos/Produccion/ProduccionWS.asmx/ProduccionSeleccionarAjaxComboProductosStock', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                    delay: 500,
                    data: function (params) {
                        return JSON.stringify({
                            value: ddlTipoProducto.val(), // search term");
                            filtro: params.term, // search term");
                            idFilial: idFilial,
                            idProduccion: idProduccion,
                        });
                    },
                    processResults: function (data, params) {
                        //return { results: data.items };
                        return {
                            results: $.map(data.d, function (item) {
                                return {
                                    text: item.Producto.Descripcion,// + ' | Stock: ' + item.Producto.StockActual,
                                    id: item.Producto.IdProducto,
                                    valorizacion: item.Valorizacion,
                                    stockActual: item.Producto.StockActual,
                                    unidadMedida: item.Producto.UnidadMedida.Descripcion,
                                    unidadMedidaId: item.Producto.UnidadMedida.IdUnidadMedida
                                }
                            })
                        };
                        cache: true
                    }
                }
            });
            ddlDetalleGastos.on('select2:select', function (e) {
                hdfDetalleGastos.val(e.params.data.text);
                hdfValorizacion.val(accounting.formatMoney(e.params.data.valorizacion, "", 2, ""));//separador de mil vacio! ALL 2020/11/02
                lblValorizacion.text(accounting.formatMoney(e.params.data.valorizacion, gblSimbolo, 2, "."));
                hdfStockActual.val(e.params.data.stockActual);
                lblStockActual.text(accounting.formatMoney(e.params.data.stockActual, "", 2, "."));
                hdfUnidadMedidaDescripcion.val(e.params.data.unidadMedida);
                hdfIdUnidadMedida.val(e.params.data.unidadMedidaId);
                lblUnidadMedida.text(e.params.data.unidadMedida);
                //txtCantidad.val(1,00);
                txtCantidad.maskMoney({ thousands: '.', decimal: ',' });
                txtCantidad.maskMoney('mask', 1.00);
                txtCantidad.focus().select();
                CalcularValorizacion();
            });
            ddlDetalleGastos.on('select2:unselect', function (e) {
                hdfDetalleGastos.val('');
                hdfValorizacion.val('0');
                hdfIdUnidadMedida.val('');
                txtCantidad.maskMoney({ thousands: '.', decimal: ',' });
                txtCantidad.maskMoney('mask', 0.00);
                CalcularValorizacion();
            });
        });
    }

    function CalcularValorizacion() {
        var subTotal = 0.00;
        var total = 0.00;
        $('#<%=gvDatos.ClientID%> tr').not(':first').not(':last').each(function () {
            var signo = $(this).find('select[id*="ddTipoMovimiento"] option:selected').val();
            var hdfValorizacion = $(this).find("input[id*='hdfValorizacion']");
            var hdfSubTotal = $(this).find("input[id*='hdfSubTotal']");
            var txtCantidad = $(this).find("input:text[id*='txtCantidad']");
            var cantidad = txtCantidad.maskMoney('unmasked')[0];
            var valorizacion = hdfValorizacion.val();
            valorizacion = valorizacion.replace('.', '').replace(',', '.');
            var lblSubTotal = $(this).find("span[id*='lblSubTotal']");
            subTotal = 0.00;
            console.log(valorizacion); console.log(cantidad); console.log(signo);
            if (valorizacion && cantidad) {
                subTotal = parseFloat(valorizacion) * parseFloat(cantidad) * parseFloat(signo);
                total += parseFloat(subTotal);
                hdfSubTotal.val(subTotal);
            }
            lblSubTotal.text(accounting.formatMoney(subTotal, gblSimbolo, 2, "."));
        });
        $("#<%=gvDatos.ClientID %> [id$=lblTotal]").text(accounting.formatMoney(total, gblSimbolo, 2, "."));
    }

    function ValidarShowConfirm(ctrl, msg) {
        if ($('select[id$="ddlProductoProducido"]').val() == null
            || $('input[id$="txtCantidadProducida"]').val() == ''
            || $('input[id$="txtCantidadProducida"]').val() == 0) {
            showConfirm(ctrl, msg);
        } else {
            __doPostBack(ctrl.name, '');
        }
    }

</script>

<div class="Producciones">
    <div id="deshabilitarControles">
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroProduccion" runat="server" Text="Nro Producción"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtNumeroProduccion" Enabled="false" runat="server"></asp:TextBox>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion" />
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDescripcion" ControlToValidate="txtDescripcion" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaInicio" runat="server" Text="Fecha Inicio"></asp:Label>
            <div class="col-sm-3">
                <div class="form-group row">
                    <div class="col-sm-6">
                        <asp:TextBox CssClass="form-control datepicker" Placeholder="Desde" ID="txtFechaInicio" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-sm-6">
                        <asp:TextBox CssClass="form-control datepicker" Placeholder="Hasta" ID="txtFechaFin" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFilial" runat="server" Text="Filial" />
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlFiliales" runat="server" Enabled="false" />
            </div>
            <%--<asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaFin" ControlToValidate="txtFechaFin" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblProductoProducido" runat="server" Text="Producto Producido"></asp:Label>
            <div class="col-sm-3">
                <asp:DropDownList CssClass="form-control select2" ID="ddlProductoProducido" runat="server"></asp:DropDownList>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadProducida" runat="server" Text="Cantidad Producida"></asp:Label>
            <div class="col-sm-3">
                <Evol:CurrencyTextBox CssClass="form-control" ID="txtCantidadProducida" Prefix="" NumberOfDecimals="2" runat="server"></Evol:CurrencyTextBox>
            </div>
            <div class="col-sm-3"></div>
        </div>
        <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
        <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0"
            SkinID="MyTab">
            <asp:TabPanel runat="server" ID="tpTotalesProductos"
                HeaderText="Totales de Productos">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upTotalesProducto" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gvTotalesProductos" ShowFooter="true"
                                OnRowDataBound="gvTotalesProductos_RowDataBound" runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Producto" ItemStyle-Width="35%">
                                        <ItemTemplate>
                                            <%#Eval("Descripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cant." ItemStyle-Wrap="false" ItemStyle-Width="5%" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%#Eval("Cantidad", "{0:N2}")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SubTotal" ItemStyle-Wrap="false" FooterStyle-Wrap="false" ItemStyle-Width="15%" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <%#Eval("SubTotal", "{0:C2}")%>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label CssClass="gvLabelMoneda labelFooterEvol" ID="lblTotal" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpDetalleProductos"
                HeaderText="Detalle de Productos">
                <ContentTemplate>
                    <asp:UpdatePanel ID="upDetalleItems" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <asp:Button CssClass="botonesEvol" ID="btnAgregarItem" OnClick="btnAgregarItem_Click" runat="server" Text="Agregar item"
                                CausesValidation="false" />
                            <asp:GridView ID="gvDatos" ShowFooter="true" OnRowCommand="gvDatos_RowCommand"
                                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaBasicaFormal" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Fecha&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:TextBox CssClass="form-control datepicker" ID="txtFecha" Enabled="false" Text='<%#Eval("Fecha", "{0:dd/MM/yyyy}") %>' runat="server"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo Movimiento" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddTipoMovimiento" Enabled="false" CssClass="select2" runat="server">
                                                <asp:ListItem Value="1" Text="Consumo" Selected="True"></asp:ListItem>
                                                <asp:ListItem Value="-1" Text="Devolucion"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Producto" ItemStyle-Width="35%">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlProductos" Enabled="false" CssClass="select2" runat="server"></asp:DropDownList>
                                            <asp:HiddenField ID="hdfIdProducto" Value='<%#Bind("Producto.IdProducto") %>' runat="server" />
                                            <asp:HiddenField ID="hdfDetalleProducto" Value='<%#Bind("Producto.Descripcion") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Stock" ItemStyle-Wrap="false" ItemStyle-Width="5%" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label CssClass="gvLabelMoneda" ID="lblStockActual" runat="server" Text='<%#Eval("Producto.StockActual", "{0:N2}")%>'></asp:Label>
                                            <asp:HiddenField ID="hdfStockActual" Value='<%#Eval("Producto.StockActual", "{0:N2}") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cantidad" ItemStyle-Wrap="false" ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtCantidad" Enabled="false" Prefix="" NumberOfDecimals="2" runat="server" Text='<%#Bind("Cantidad", "{0:N2}") %>'></Evol:CurrencyTextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unidad Medida" ItemStyle-Wrap="false" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <asp:Label CssClass="gvLabelMoneda" ID="lblUnidadMedida" runat="server" Text='<%#Eval("Producto.UnidadMedida.Descripcion")%>'></asp:Label>
                                            <asp:HiddenField ID="hdfUnidadMedidaDescripcion" Value='<%#Eval("Producto.UnidadMedida.Descripcion") %>' runat="server" />
                                            <asp:HiddenField ID="hdfIdUnidadMedida" Value='<%#Bind("Producto.UnidadMedida.IdUnidadMedida") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Valorizacion" ItemStyle-Wrap="false" FooterStyle-Wrap="false" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="text-right">
                                        <ItemTemplate>
                                            <asp:Label CssClass="gvLabelMoneda" ID="lblValorizacion" runat="server" Text='<%#Eval("Valorizacion", "{0:C2}")%>'></asp:Label>
                                            <asp:HiddenField ID="hdfValorizacion" Value='<%#Eval("Valorizacion", "{0:N2}") %>' runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="SubTotal" ItemStyle-Wrap="false" FooterStyle-Wrap="false" ItemStyle-Width="10%" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label CssClass="gvLabelMoneda" ID="lblSubTotal" runat="server"></asp:Label>
                                            <asp:HiddenField ID="hdfSubTotal" runat="server" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label CssClass="gvLabelMoneda labelFooterEvol" ID="lblTotal" runat="server"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Usuario" ItemStyle-Wrap="false" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <%#Eval("UsuarioAlta.ApellidoNombre") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false" ItemStyle-Width="5%">
                                        <ItemTemplate>
                                            <%--<asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar" Visible="false"
                                                         AlternateText="Consultar" ToolTip="Consultar" />
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" Visible="false"
                                                         AlternateText="Modificar" ToolTip="Modificar" />--%>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                                AlternateText="Elminiar" ToolTip="Eliminar" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpDetalleGastos" Visible="false"
                HeaderText="Detalle de Gastos">
                <ContentTemplate>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpCentrosCostos" Visible="false"
                HeaderText="Centros de Costos">
                <ContentTemplate>
                </ContentTemplate>
            </asp:TabPanel>
            <asp:TabPanel runat="server" ID="tpComentarios" HeaderText="Comentarios">
                <ContentTemplate>
                    <AUGE:Comentarios ID="ctrComentarios" runat="server" />
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
    </div>
    <asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="row justify-content-md-center">
                <div class="col-md-auto">
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
