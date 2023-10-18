<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrdenesPagosValores.ascx.cs" Inherits="IU.Modulos.CuentasPagar.Controles.OrdenesPagosValores" %>
<%@ Register Src="~/Modulos/Tesoreria/Controles/ChequesTercerosPopUp.ascx" TagName="popUpBuscarCheque" TagPrefix="auge" %>

<script lang="javascript" type="text/javascript">
    function gvValoresCalcularImporteTotal() {
        var importeTotal = 0.00;
        $('#<%=gvFormasCobros.ClientID%> tr').not(':first').not(':last').each(function () {
            //var incluir = $("td:eq(6) :checkbox", this).is(":checked");
            var importe = $(this).find("input[id*='hdfImporte']").val().replace('.', '').replace(',', '.'); //$(this).find('input:text[id*="Importe"]').maskMoney('unmasked')[0]; //$("td:eq(4)", this).html();

            if (importe) {
                importeTotal += parseFloat(importe);
            }
        });
        return importeTotal;
    }

    function HabilitarTransferencias() {
        var fechaPago = $("input[type=text][id$='txtFechaCajaContabilizacion']").val();

        if (fechaPago == '') {
            fechaPago = $("input[type=text][id$='txtFechaAlta']").val();
        }
        if (fechaPago) {
            //$("input[type=text][id$='txtFechaTransferencia']").val(fechaPago);
            $("input[type=text][id$='" + 'txtFechaTransferencia' + "']").datepicker({
                showOnFocus: true,
                uiLibrary: 'bootstrap4',
                locale: 'es-es',
                format: 'dd/mm/yyyy',
                value: fechaPago
            });
        }
    }

</script>
<asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDetalle" Visible="false" runat="server">
            <div class="card">
                <div class="card-header">
                    Ingreso de Valores
                </div>
                <div class="card-body">
                    <asp:UpdatePanel ID="upValores" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div class="form-group row">
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTiposValores" runat="server" Text="Tipo de Valor" />
                                <div class="col-sm-3">
                                    <asp:DropDownList CssClass="form-control select2" ID="ddlTiposValores" runat="server" OnSelectedIndexChanged="ddlTiposValores_SelectedIndexChanged"
                                        AutoPostBack="true" />
                                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTiposValores" runat="server" ControlToValidate="ddlTiposValores"
                                        ErrorMessage="*" ValidationGroup="IngresarCobro" />
                                </div>
                                <%--BOTON INGRESAR CHEQUE--%>
                                <AUGE:popUpBuscarCheque ID="ctrBuscarCheque" runat="server" />
                                <div class="col-sm-1">
                                    <asp:Button CssClass="botonesEvol" ID="btnAgregarCheque" Visible="false" runat="server" Text="Buscar Cheques"
                                        OnClick="btnBuscarCheque_Click" />
                                </div>
                                <asp:Label CssClass="col-sm-1 col-form-label" ID="Label1" runat="server" Text="Importe" />
                                <div class="col-sm-3">
                                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporte" runat="server" />
                                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporte" runat="server" ControlToValidate="txtImporte"
                                        ErrorMessage="*" ValidationGroup="IngresarCobro" />
                                    <%--<asp:RangeValidator ID="rgvImporte" MinimumValue="1" MaximumValue="999999999" ControlToValidate="txtImporte" runat="server" 
                    ErrorMessage="*" ValidationGroup="IngresarCobro"></asp:RangeValidator>--%>
                                </div>
                                <div class="col-sm-2">
                                    <asp:Button CssClass="botonesEvol" ID="btnIngresarCobro" runat="server" Text="Ingresar Valor"
                                        OnClick="btnIngresarCobro_Click" ValidationGroup="IngresarCobro" />
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:Panel ID="pnlTransferencias" Visible="false" runat="server">
                        <div class="card">
                            <div class="card-header">
                                Datos de la Cuenta
                            </div>
                            <div class="card-body">
                                <div class="form-group row">
                                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaTransferencia" runat="server" Text="Fecha"></asp:Label>
                                    <div class="col-sm-3">
                                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaTransferencia" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaTransferencia" Enabled="false" runat="server" ControlToValidate="txtFechaTransferencia"
                                            ErrorMessage="*" ValidationGroup="IngresarTransferencia" />
                                    </div>
                                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblBancosCuentas" runat="server" Text="Cuenta"></asp:Label>
                                    <div class="col-sm-3">
                                        <asp:DropDownList CssClass="form-control select2" ID="ddlBancosCuentasTransferencias" runat="server">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvBancosCuentasTransferencias" runat="server" ControlToValidate="ddlBancosCuentasTransferencias"
                                            ErrorMessage="*" Enabled="true" ValidationGroup="IngresarTransferencia" />
                                    </div>
                                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroTransferencia" runat="server" Text="Numero Movimiento"></asp:Label>
                                    <div class="col-sm-3">
                                        <asp:TextBox CssClass="form-control" ID="txtNumeroTransferencia" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group row">
                                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCBU" runat="server" Text="CBU"></asp:Label>
                                    <div class="col-sm-3">
                                        <asp:TextBox CssClass="form-control" ID="txtCbu" MaxLength="22" AutoPostBack="true" OnTextChanged="txtCbu_TextChanged" runat="server"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator CssClass="Validador" ID="rfvCbu" runat="server" ControlToValidate="txtCbu"
                                            ErrorMessage="*" enabled="true" ValidationGroup="IngresarTransferencia" />--%>
                                    </div>
                                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblDatosCbu" runat="server" Text="."></asp:Label>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlTarjetasCredito" Visible="false" runat="server">
                        <div class="card">
                            <div class="card-header">
                                Datos de la Tarjeta
                            </div>
                            <div class="card-body">
                                <div class="form-group row">
                                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaTarjeta" runat="server" Text="Fecha"></asp:Label>
                                    <div class="col-sm-3">
                                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaTarjeta" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaTarjeta" runat="server" ControlToValidate="txtFechaTarjeta"
                                            ErrorMessage="*" ValidationGroup="IngresarTarjeta" />
                                    </div>
                                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTarjetasCuentas" runat="server" Text="Tarjeta Cuenta"></asp:Label>
                                    <div class="col-sm-3">
                                        <asp:DropDownList CssClass="form-control select2" ID="ddlBancosCuentasTarjetas" runat="server">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvBancosCuentasTarjetas" runat="server" ControlToValidate="ddlBancosCuentasTarjetas"
                                            ErrorMessage="*" Enabled="false" ValidationGroup="IngresarTarjeta" />
                                    </div>
                                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadCuotas" runat="server" Text="Cantidad de Cuotas"></asp:Label>
                                    <div class="col-sm-3">
                                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtCantidadCuotas" NumberOfDecimals="0" ThousandsSeparator="" Prefix="" runat="server" />
                                        <asp:RequiredFieldValidator CssClass="Validador" ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFechaTarjeta"
                                            ErrorMessage="*" ValidationGroup="IngresarTarjeta" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlChequesPropios" Visible="false" runat="server">
                        <div class="card">
                            <div class="card-header">
                                Datos del Cheque
                            </div>
                            <div class="card-body">
                                <div class="form-group row">
                                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFecha" runat="server" Text="Fecha"></asp:Label>
                                    <div class="col-sm-3">
                                        <asp:TextBox CssClass="form-control datepicker" ID="txtFecha" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFecha" runat="server" ControlToValidate="txtFecha"
                                            ErrorMessage="*" Enabled="false" ValidationGroup="IngresarCheque" />
                                    </div>
                                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDiferido" runat="server" Text="Fecha Diferido"></asp:Label>
                                    <div class="col-sm-3">
                                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDiferido" runat="server"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator CssClass="Validador" ID="rfvChequeDiferido" runat="server" ControlToValidate="txtFechaDiferido" 
                ErrorMessage="*" Enabled="false" ValidationGroup="IngresarCheque"/>--%>
                                    </div>
                                    <div class="col-sm-3"></div>
                                </div>
                                <div class="form-group row">
                                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroCheque" runat="server" Text="Numero Cheque"></asp:Label>
                                    <div class="col-sm-3">
                                        <asp:TextBox CssClass="form-control" ID="txtNumeroCheque" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumeroCheque" runat="server" ControlToValidate="txtNumeroCheque"
                                            ErrorMessage="*" Enabled="false" ValidationGroup="IngresarCheque" />
                                    </div>
                                    <asp:Label CssClass="col-sm-1 col-form-label" ID="Label2" runat="server" Text="Banco Cuenta"></asp:Label>
                                    <div class="col-sm-3">
                                        <asp:DropDownList CssClass="form-control select2" ID="ddlBancosCuentasCheques" runat="server">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvBancosCuentasCheques" runat="server" ControlToValidate="ddlBancosCuentasCheques"
                                            ErrorMessage="*" Enabled="false" ValidationGroup="IngresarCheque" />
                                    </div>
                                    <div class="col-sm-3"></div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlAfipRetencionesPercepciones" Visible="false" runat="server">
                        <div class="card">
                            <div class="card-header">
                                Datos de la Retencion/Percepcion
                            </div>
                            <div class="card-body">
                                <div class="form-group row">
                                    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoRecionPercepcion" runat="server" Text="Tipo"></asp:Label>
                                    <div class="col-sm-3">
                                        <asp:DropDownList CssClass="form-control select2" ID="ddlTiposRetencionesPercepciones" runat="server">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTiposRetencionesPercepciones" runat="server" ControlToValidate="ddlTiposRetencionesPercepciones"
                                            ErrorMessage="*" Enabled="false" ValidationGroup="IngresarRetencionPercepcion" />
                                    </div>
                                    <div class="col-sm-8"></div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnlDetalleIngresos" GroupingText="Detalle de Valores" runat="server">
            <div class="table-responsive">
                <asp:GridView ID="gvFormasCobros" DataKeyNames="IndiceColeccion"
                    runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                    OnRowDataBound="gvFormasCobros_RowDataBound" OnPageIndexChanging="gvFormasCobros_PageIndexChanging"
                    OnRowCommand="gvFormasCobros_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="Tipo Valor" HeaderStyle-Wrap="false" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <%# Eval("TipoValor.TipoValor")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Detalle">
                            <ItemTemplate>
                                <%# Eval("BancoCuenta.DescripcionFilialBancoTipoCuentaNumero")%>
                                <%# Eval("Cheque.BancoTitularCuit")%>
                                <%# Eval("ListaValorSistemaDetalle.Descripcion")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Fecha" ItemStyle-Wrap="false" DataFormatString="{0:dd/MM/yyyy}" DataField="Fecha" />
                        <%--<asp:BoundField  HeaderText="FechaDiferido" ItemStyle-Wrap="false" DataFormatString="{0:dd/MM/yyyy}" DataField="FechaDiferido" />--%>
                        <asp:TemplateField HeaderText="Fecha Diferido">
                            <ItemTemplate>
                                <asp:TextBox CssClass="gvTxtFecha" ID="txtFechaDiferido" Text='<%# Eval("FechaDiferido")%>' Enabled="false" runat="server"></asp:TextBox>
                                <div class="Calendario" id="dvFechaDiferido" runat="server" visible="false">
                                    <asp:Image ID="imgFechaDiferido" runat="server" ImageUrl="~/Imagenes/Calendario.png" />
                                    <asp:CalendarExtender ID="ceFechaBanco" runat="server" Enabled="true"
                                        TargetControlID="txtFechaDiferido" PopupButtonID="imgFechaDiferido" Format="dd/MM/yyyy"></asp:CalendarExtender>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Numero Cheque" ItemStyle-Wrap="false" DataField="NumeroCheque" />
                        <asp:BoundField HeaderText="Cantidad Cuotas" ItemStyle-Wrap="false" DataField="CantidadCuotas" />
                        <asp:TemplateField HeaderText="Importe Cuota" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right"
                            ItemStyle-Wrap="false" FooterStyle-HorizontalAlign="Right" FooterStyle-Wrap="false">
                            <ItemTemplate>
                                <%# Eval("ImporteCuota", "{0:C2}")%>
                                <asp:HiddenField ID="hdfCuota" Value='<%#Bind("ImporteCuota") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Importe" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right"
                            ItemStyle-Wrap="false" FooterStyle-HorizontalAlign="Right" FooterStyle-Wrap="false">
                            <ItemTemplate>
                                <%# Eval("Importe", "{0:C2}")%>
                                <asp:HiddenField ID="hdfImporte" Value='<%#Bind("Importe") %>' runat="server" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblTotal" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField  HeaderText="Importe" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="Importe" SortExpression="Importe" />--%>
                        <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                    AlternateText="Elminiar" ToolTip="Eliminar" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
