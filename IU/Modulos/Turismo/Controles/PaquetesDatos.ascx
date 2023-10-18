<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaquetesDatos.ascx.cs" Inherits="IU.Modulos.Turismo.Controles.PaquetesDatos" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>

<script lang="javascript" type="text/javascript">
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitApellidoSelect2);
        SetTabIndexInput();
        InitApellidoSelect2();
        $("input[type=submit][id$='btnAceptar']").click(function (e) {
            var txtFechaIda = $("input[id$='txtFechaSalida']").val();
            var txtFechaVuelta = $("input[id$='txtFechaRegreso']").val();
            if (txtFechaIda != '' && txtFechaVuelta != '') {
                var fechaIda = new Date(toDate(txtFechaIda));
                var fechaVuelta = new Date(toDate(txtFechaVuelta));
                if (fechaIda > fechaVuelta) {
                    e.preventDefault();
                    MostrarMensaje('La Fecha de Regreso debe ser mayor a la Fecha de Salida.', 'red');
                }
            }
        });
    });
    function SetTabIndexInput() {
        $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }
    function InitApellidoSelect2() {
        var control = $("select[name$='ddlProveedor']");
        control.select2({
            placeholder: 'Ingrese el codigo o Razón Social',
            selectOnClose: true,
            theme: 'bootstrap4',
            minimumInputLength: 1,
            language: 'es',
            ajax: {
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                url: '<%=ResolveClientUrl("~")%>/Modulos/Proveedores/ProveedoresWS.asmx/ProveedoresCombo', //", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                delay: 500,
                data: function (params) {
                    return JSON.stringify({
                        value: control.val(), // search term");
                        filtro: params.term // search term");
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
                    return {
                        results: $.map(data.d, function (item) {
                            return {
                                id: item.IdProveedor,
                                text: item.CodigoProveedor,
                            }
                        })
                    };
                    cache: true
                }
            }
        });
        control.on('select2:select', function (e) {
            $("input[id*='hdfIdProveedor']").val(e.params.data.id);//.trigger("change");
            $("input[id*='hdfProveedor']").val(e.params.data.text);
        });
        control.on('select2:unselect', function (e) {
            control.val(null).trigger('change');
            $("input[id*='hdfIdProveedor']").val('');//.trigger("change")
            $("input[id*='hdfProveedor']").val('');
        });
    }
    function CalcularTotalImporte() {
        var importeNuevo = 0;
        var costoNuevo = 0;
        var cantidadPlazas = $("input:text[id$='txtCantidad']").val();
        $('#<%=gvPaquetes.ClientID%> tr').not(':first').not(':last').each(function () {
            var importe = $(this).find("input[id$='hdfImporte']").val();
            var costo = $(this).find("input[id$='hdfCosto']").val();
            if (costo != undefined) {
                var costoParseado = parseFloat(costo.replace(",", "."));
                if (costoParseado > 0)
                    costoNuevo += costoParseado;
            }
            if (importe != undefined) {
                var importeParseado = parseFloat(importe.replace(",", "."));
                if (importeParseado > 0)
                    importeNuevo += importeParseado;
            }
        });
        importeNuevo = importeNuevo * cantidadPlazas;
        costoNuevo = costoNuevo * cantidadPlazas;
        $("input:text[id$='txtImporte']").val((accounting.formatMoney(importeNuevo, gblSimbolo, 2, gblSeparadorMil)));
        $("input:text[id$='txtCostoTotal']").val((accounting.formatMoney(costoNuevo, gblSimbolo, 2, gblSeparadorMil)));
    }
</script>

<asp:UpdatePanel ID="pnlPrinc" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="PaquetesDatos">
            <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNroPaquete" runat="server" Text="Nro. Paquete"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:TextBox CssClass="form-control" ID="txtNroPaquete" Enabled="false" runat="server"></asp:TextBox>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:TextBox CssClass="form-control" ID="txtNombre" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvNombre" ControlToValidate="txtNombre" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvEstado" ControlToValidate="ddlEstado" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTipoCargo" runat="server" Text="Cargo"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoCargo" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvTipoCargo" ControlToValidate="ddlTipoCargo" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCantidad" runat="server" Text="Cant. Total Plazas"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtCantidad" MaxLength="4" NumberOfDecimals="0" ThousandsSeparator="" runat="server" Prefix=""></Evol:CurrencyTextBox>
                    <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvCantidad" ControlToValidate="txtCantidad" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblImporte" runat="server" Text="Importe Total"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporte" Enabled="false" runat="server" Prefix=""></Evol:CurrencyTextBox>
                    <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvImporte" ControlToValidate="txtImporte" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblMoneda" runat="server" Text="Moneda"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlMoneda" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMoneda_SelectedIndexChanged"></asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvMoneda" ControlToValidate="ddlMoneda" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCantidadDisponibles" runat="server" Text="Cant. Plazas Disp."></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtCantidadDisponible" MaxLength="4" NumberOfDecimals="0" ThousandsSeparator="" runat="server" Prefix=""></Evol:CurrencyTextBox>
                    <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvCantidadDisponible" ControlToValidate="txtCantidadDisponible" ValidationGroup="Aceptar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCostoTotal" runat="server" Text="Costo Total"></asp:Label>
                <div class="col-lg-3 col-md-3 col-sm-9">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtCostoTotal" Enabled="false" runat="server" Prefix=""></Evol:CurrencyTextBox>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaSalida" runat="server" Text="Fecha de Salida" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaSalida" runat="server" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaSalida" ControlToValidate="txtFechaSalida" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaRegreso" runat="server" Text="Fecha de Regreso" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaRegreso" runat="server" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaRegreso" ControlToValidate="txtFechaRegreso" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
            </div>
            <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
                <asp:TabPanel runat="server" ID="tpPaquetes" HeaderText="Paquetes">
                    <ContentTemplate>
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblServicio" runat="server" Text="Servicio"></asp:Label>
                            <div class="col-lg-3 col-md-3 col-sm-9">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlServicio" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlServicio_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvServicio" ControlToValidate="ddlServicio" ValidationGroup="Cargar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            </div>
                            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblProveedor" runat="server" Text="Proveedor"></asp:Label>
                            <div class="col-lg-3 col-md-3 col-sm-9">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlProveedor" runat="server" />
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvProveedor" ControlToValidate="ddlProveedor" ValidationGroup="Cargar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                <asp:HiddenField ID="hdfIdProveedor" runat="server" />
                                <asp:HiddenField ID="hdfProveedor" runat="server" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblImporteDetalle" runat="server" Text="Importe"></asp:Label>
                            <div class="col-lg-3 col-md-3 col-sm-9">
                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtImpDetalle" MaxLength="18" NumberOfDecimals="2" ThousandsSeparator="." runat="server" Prefix="$"></Evol:CurrencyTextBox>
                                <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvImporteDetalle" ControlToValidate="txtImpDetalle" ValidationGroup="Cargar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            </div>
                            <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCostoDetalle" runat="server" Text="Costo"></asp:Label>
                            <div class="col-lg-3 col-md-3 col-sm-9">
                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtCostoDetalle" MaxLength="18" NumberOfDecimals="2" ThousandsSeparator="." runat="server" Prefix="$"></Evol:CurrencyTextBox>
                                <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvCostoDetalle" ControlToValidate="txtCostoDetalle" ValidationGroup="Cargar" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-sm-2">
                                <asp:Button CssClass="botonesEvol" ID="btnCargar" runat="server" Text="Cargar" OnClick="btnAgregar_Click" ValidationGroup="Cargar" />
                            </div>
                        </div>
                        <div class="row">
                            <asp:PlaceHolder ID="pnlCamposDinamicosTurismoServicios" runat="server"></asp:PlaceHolder>
                        </div>
                        <asp:UpdatePanel ID="upPaquetes" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <div class="table-responsive">
                                    <asp:HiddenField ID="hdfIdPaquete" runat="server" />
                                    <asp:HiddenField ID="hdfTotalServicios" runat="server" />
                                    <asp:GridView ID="gvPaquetes" OnRowCommand="gvPaquetes_RowCommand"
                                        OnRowDataBound="gvPaquetes_RowDataBound" ShowFooter="true" DataKeyNames="IdPaqueteDetalle"
                                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Tipo de Servicio">
                                                <ItemTemplate>
                                                    <%# Eval("ProductoDescripcion")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Proveedor">
                                                <ItemTemplate>
                                                    <%# Eval("Proveedor")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Detalle">
                                                <ItemTemplate>
                                                    <asp:Literal ID="ltlDetalleCampos" runat="server"></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%# Eval("Importe", "{0:C2}")%>
                                                    <asp:HiddenField ID="hdfImporte" Value='<%#Bind("Importe") %>' runat="server" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text="0.00" FooterStyle-Wrap="false" ItemStyle-Wrap="false"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Costo" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%# Eval("Costo", "{0:C2}")%>
                                                    <asp:HiddenField ID="hdfCosto" Value='<%#Bind("Costo") %>' runat="server" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label CssClass="labelFooterEvol" ID="lblCostoTotal" runat="server" Text="0.00" FooterStyle-Wrap="false" ItemStyle-Wrap="false"></asp:Label>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                                                <ItemTemplate>
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar" Visible="true"
                                                        AlternateText="Modificar" ToolTip="Modificar" />
                                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnBaja"
                                                        AlternateText="Eliminiar" ToolTip="Eliminar" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:HiddenField ID="hdfIdPaqueteDetalle" runat="server" />
                                    <asp:HiddenField ID="hdfModificarDatos" runat="server" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tpHabitaciones" HeaderText="Habitaciones">
                    <ContentTemplate>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tpTurismo" HeaderText="Reservas">
                    <ContentTemplate>
                        <div class="form-group row">
                            <div class="col-sm-6"></div>
                            <div class="col-sm-6">
                                <div class="table-responsive">
                                    <asp:GridView ID="gvPagosValores" OnRowDataBound="gvPagosValores_RowDataBound"
                                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true">
                                        <Columns>
                                            <asp:BoundField HeaderText="Proveedor" DataField="Proveedor" SortExpression="Proveedor" />
                                            <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="Importe">
                                                <ItemTemplate>
                                                    <%# string.Concat(Eval("Moneda"), Eval("Importe", "{0:N2}"))%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pago" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="Pago">
                                                <ItemTemplate>
                                                    <%# string.Concat(Eval("Moneda"), Eval("Pago", "{0:N2}"))%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Saldo" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="Saldo">
                                                <ItemTemplate>
                                                    <%# string.Concat(Eval("Moneda"), Eval("Saldo", "{0:N2}"))%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-12">
                            <Evol:EvolGridView ID="gvReservasTurismo" OnRowCommand="gvReservasTurismo_RowCommand" AllowPaging="true"
                                OnRowDataBound="gvReservasTurismo_RowDataBound" DataKeyNames="IdTipoCargoAfiliadoFormaCobro,IdAfiliado"
                                runat="server" SkinID="GrillaBasicaFormalSticky" AutoGenerateColumns="false" ShowFooter="true"
                                OnPageIndexChanging="gvReservasTurismo_PageIndexChanging">
                                <Columns>
                                    <asp:BoundField HeaderText="Nro. de Reserva" DataField="NumeroReserva" ItemStyle-Wrap="false" SortExpression="NumeroReserva" />
                                    <asp:BoundField HeaderText="Fecha" DataField="Fecha" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Wrap="false" />
                                    <asp:BoundField HeaderText="Detalle" DataField="Detalle" />
                                    <asp:TemplateField HeaderText="Forma de Cobro">
                                        <ItemTemplate>
                                            <%# Eval("FormaCobro")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Total" SortExpression="ImporteTotal" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("Moneda"), Eval("ImporteTotal", "{0:N2}"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Pagado" SortExpression="TipoCargo.Importe" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("Moneda"), Eval("ImportePagado", "{0:N2}"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Cobrado" SortExpression="CantidadCuotas">
                                        <ItemTemplate>
                                            <%# string.Concat(Eval("Moneda"), Eval("ImporteCobrado", "{0:N2}"))%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdfIdEstado" runat="server" Value='<%#Eval("IdEstado")%>' />
                                            <%# Eval("EstadoDescripcion")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                                AlternateText="Consultar" ToolTip="Consultar" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" ID="btnModificar"
                                                AlternateText="Modificar" ToolTip="Modificar" />
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                                AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label CssClass="labelFooterEvol" ID="lblCantidadRegistros" runat="server" Text=""></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </Evol:EvolGridView>
                        </div>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tpPagos" HeaderText="Pagos">
                    <ContentTemplate>
                        <asp:UpdatePanel runat="server" ID="upPagos" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div id="dvPagos" runat="server">
                                    <div class="form-group row">
                                        <div class="col-12 col-md-8 col-lg-4" runat="server" id="Div1">
                                            <div class="row">
                                                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblProveedoresPagos" runat="server" Text="Proveedor" />
                                                <div class="col-sm-9">
                                                    <asp:DropDownList CssClass="form-control select2" ID="ddlProveedoresPagos" Enabled="true" runat="server"
                                                        AutoPostBack="true" OnSelectedIndexChanged="ddlProveedoresPagos_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator CssClass="rfvValidador" ID="rfvProveedoresPagos" ControlToValidate="ddlProveedoresPagos" ValidationGroup="AgregarPago" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-12 col-md-8 col-lg-4" runat="server" id="Div2">
                                            <div class="row">
                                                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImporteAnticipo" runat="server" Text="Importe" />
                                                <div class="col-sm-9">
                                                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteAnticipo" Prefix="$" NumberOfDecimals="2" runat="server" />
                                                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTasaInteres" runat="server" ControlToValidate="txtImporteAnticipo"
                                                        ErrorMessage="*" ValidationGroup="AgregarPago" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-1 col-md-1 col-lg-1" runat="server" id="dvAgregarPago">
                                            <div class="row">
                                                <asp:Button CssClass="botonesEvol" ID="btnAgregarPago" runat="server" Text="Agregar Anticipo"
                                                    OnClick="btnAgregarPago_Click" ValidationGroup="AgregarPago" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="table-responsive">
                                        <asp:GridView ID="gvPagos" OnRowCommand="gvPagos_RowCommand" AllowPaging="false" AllowSorting="false"
                                            OnRowDataBound="gvPagos_RowDataBound" DataKeyNames="IdOrdenPago,IdSolicitudPago, IdTipoOperacion"
                                            runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Número" SortExpression="IdOrdenPago">
                                                    <ItemTemplate>
                                                        <%# Eval("IdOrdenPago")%>
                                                        <asp:HiddenField ID="hdfIdTipoOperacion" runat="server" Value='<%#Eval("IdTipoOperacion") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Beneficiario" SortExpression="EntidadNombre">
                                                    <ItemTemplate>
                                                        <%# Eval("EntidadNombre")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cuit" SortExpression="EntidadCuit">
                                                    <ItemTemplate>
                                                        <%# Eval("EntidadCuit")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Fecha" SortExpression="FechaAlta">
                                                    <ItemTemplate>
                                                        <%# Eval("FechaAlta", "{0:dd/MM/yyyy}")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Fecha Pago" SortExpression="FechaPago">
                                                    <ItemTemplate>
                                                        <%# Eval("FechaPago", "{0:dd/MM/yyyy}")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Importe Total" SortExpression="ImporteTotal">
                                                    <ItemTemplate>
                                                        <%# string.Concat(Eval("Moneda"), Eval("ImporteTotal", "{0:N2}"))%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Estado" SortExpression="EstadoDescripcion">
                                                    <ItemTemplate>
                                                        <%# Eval("EstadoDescripcion")%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Acciones" ItemStyle-Wrap="false">
                                                    <ItemTemplate>
                                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                                            AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" Visible="false" />
                                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Ver.png" runat="server" CommandName="Consultar" ID="btnConsultar"
                                                            AlternateText="Mostrar" ToolTip="Mostrar" />
                                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Modificar.png" runat="server" CommandName="Modificar" Visible="false" ID="btnModificar"
                                                            AlternateText="Modificar Solicitud" ToolTip="Modificar Solicitud" />
                                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Autorizar.png" runat="server" CommandName="Autorizar" ID="btnAutorizar"
                                                            AlternateText="Autorizar" ToolTip="Autorizar" Visible="false" />
                                                        <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnAnular"
                                                            AlternateText="Anular Orden Pago" ToolTip="Anular Orden Pago" Visible="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
            <asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
                    <center>
                        <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                        <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                    </center>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
