<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrdenesCobrosValores.ascx.cs" Inherits="IU.Modulos.Cobros.Controles.OrdenesCobrosValores" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>


<script lang="javascript" type="text/javascript">
    function gvValoresCalcularImporteTotal() {
        var importeTotal = 0.00;
        $('#<%=gvFormasCobros.ClientID%> tr').not(':first').not(':last').each(function () {
            //var incluir = $("td:eq(6) :checkbox", this).is(":checked");
            var importe = $(this).find("input[id*='hdfImporte']").val().replace('.', '').replace(',', '.'); //$(this).find('input:text[id*="Importe"]').maskMoney('unmasked')[0]; //$("td:eq(4)", this).html();
            if (importe) {
                //importe = importe.replace('.', '').replace(',', '.'); //remplazo . por nada y , por .
                importeTotal += parseFloat(importe);
            }
        });
        return importeTotal;
    }

    function ValidateTipoValor() {
        var isValid, isValidTipo = true;
        isValid = Page_ClientValidate('IngresarCobro');
        switch ($("select[id$='ddlTiposValores']").val()) {
            case "3":
                isValidTipo = Page_ClientValidate('IngresarTransferencia');
                break;
            case "4":
                isValidTipo = Page_ClientValidate('IngresarCheque');
                break;
            case "5":
                isValidTipo = Page_ClientValidate('IngresarTarjeta');
                break;
            case "12":
                isValidTipo = Page_ClientValidate('IngresarCajaAhorro');
                break;
            default:
                break;
        }
        if (!isValid || !isValidTipo)
            return false;
        else
            return true;
    }

    function BuscarFechaCajaCont(idControlFecha) {
        var txtFechaContable = $("input[type=text][id$='txtFechaCajaContabilizacion']");//.val();//.val();
        if (txtFechaContable) {
            //var txtFecha = $("input[type=text][id$='" + idControlFecha + "']");
            //txtFecha.destroy();
            $("input[type=text][id$='" + idControlFecha + "']").datepicker({
                showOnFocus: true,
                uiLibrary: 'bootstrap4',
                locale: 'es-es',
                format: 'dd/mm/yyyy',
                value: txtFechaContable.val()
            });
            //if (txtFecha) {
            //    var dFecha = new Date(toDate(txtFechaContable.val()));
            //    txtFecha.val(dFecha);
            //}
        }
    }
</script>

<asp:UpdatePanel ID="upCobrosValores" UpdateMode="Conditional" runat="server">
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
                                <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresTiposValores"><div class="row">
                                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTiposValores" runat="server" Text="Tipo de Valor" />
                                    <div class="col-sm-9">
                                        <asp:DropDownList CssClass="form-control select2" ID="ddlTiposValores" runat="server" OnSelectedIndexChanged="ddlTiposValores_SelectedIndexChanged" AutoPostBack="true" />
                                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTiposValores" runat="server" ControlToValidate="ddlTiposValores" ErrorMessage="*" ValidationGroup="IngresarCobro" />
                                    </div></div>
                                </div>
                                <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresImporte"><div class="row">
                                    <asp:Label CssClass="col-sm-3 col-form-label" ID="Label1" runat="server" Text="Importe" />
                                    <div class="col-sm-9">
                                        <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporte" runat="server" />
                                        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporte" runat="server" ControlToValidate="txtImporte" ErrorMessage="*" ValidationGroup="IngresarCobro" />
                                        <%--<asp:RangeValidator ID="rgvImporte" MinimumValue="1" MaximumValue="999999999" ControlToValidate="txtImporte" runat="server" 
                    ErrorMessage="*" ValidationGroup="IngresarCobro"></asp:RangeValidator>--%>
                                    </div></div>
                                </div>
                                 <div class="col-12 col-md-8 col-lg-4" runat="server" id="Div1"><div class="row">
                              
                                    <asp:Button CssClass="botonesEvol" ID="btnIngresarCobro" runat="server" Text="Ingresar Valor"
                                        OnClick="btnIngresarCobro_Click" ValidationGroup="IngresarCobro" OnClientClick="ValidateTipoValor();" />
                                </div></div>
                            </div>
                            <AUGE:CamposValores ID="CamposValoresTipoValor" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:Panel ID="pnlTransferencias" Visible="false" runat="server">
                        <div class="card">
                            <div class="card-header">
                                Datos de la Cuenta
                            </div>
                            <div class="card-body">
                                <div class="form-group row">
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresFechaTransferencia">
                                             <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaTransferencia" runat="server" Text="Fecha"></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaTransferencia" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaTransferencia" runat="server" ControlToValidate="txtFechaTransferencia"
                                                ErrorMessage="*" ValidationGroup="IngresarTransferencia" />
                                        </div></div>
                                    </div>
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresBancosCuentas">
                                             <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblBancosCuentas" runat="server" Text="Cuenta"></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlBancosCuentasTransferencias" runat="server">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvBancosCuentasTransferencias" runat="server" ControlToValidate="ddlBancosCuentasTransferencias"
                                                ErrorMessage="*" ValidationGroup="IngresarTransferencia" />
                                        </div></div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlCajaAhorro" Visible="false" runat="server">
                        <div class="card">
                            <div class="card-header">
                                Datos de la Caja de Ahorro
                            </div>
                            <div class="card-body">
                                <div class="form-group row">
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresCuentaBancaria">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCuentaBancaria" runat="server" Text="Cuenta de Ahorro"></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlCuentaBancariaCajaAhorro" runat="server">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCuentaBancariaCajaAhorro" runat="server" ControlToValidate="ddlCuentaBancariaCajaAhorro"
                                                ErrorMessage="*" ValidationGroup="IngresarCajaAhorro" />
                                        </div></div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </asp:Panel>


                    <asp:Panel ID="pnlCheques" Visible="false" runat="server">
                        <div class="card">
                            <div class="card-header">
                                Datos del Cheque
                            </div>
                            <div class="card-body">
                                <div class="form-group row">
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresFecha">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFecha" runat="server" Text="Fecha"></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:TextBox CssClass="form-control datepicker" ID="txtFecha" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFecha" runat="server" ControlToValidate="txtFecha"
                                                ErrorMessage="*" ValidationGroup="IngresarCheque" />
                                        </div></div>
                                    </div>
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresFechaDiferido">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaDiferido" runat="server" Text="Fecha Diferido"></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDiferido" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvChequeDiferido" runat="server" ControlToValidate="txtFechaDiferido"
                                                ErrorMessage="*" ValidationGroup="IngresarCheque" />
                                        </div></div>
                                    </div>

                         
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresNumeroCheque">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblNumeroCheque" runat="server" Text="Numero Cheque"></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:TextBox CssClass="form-control" ID="txtNumeroCheque" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumeroCheque" runat="server" ControlToValidate="txtNumeroCheque"
                                                ErrorMessage="*" ValidationGroup="IngresarCheque" />
                                        </div></div>
                                    </div>
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresConcepto">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblConcepto" runat="server" Text="Concepto"></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:TextBox CssClass="form-control" ID="txtConcepto" runat="server"></asp:TextBox>
                                        </div></div>
                                    </div>

                           
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresBanco">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblBanco" runat="server" Text="Banco"></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlBancos" runat="server">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvBancos" runat="server" ControlToValidate="ddlBancos"
                                                ErrorMessage="*" ValidationGroup="IngresarCheque" />
                                        </div></div>
                                    </div>

                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresCUIT">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCuit" runat="server" Text="CUIT"></asp:Label>
                                        <div class="col-sm-9">
                                            <AUGE:NumericTextBox CssClass="form-control" ID="txtCUIT" MaxLength="11" runat="server"></AUGE:NumericTextBox>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCUIT" runat="server" ControlToValidate="txtCUIT"
                                                ErrorMessage="*" ValidationGroup="IngresarCheque" />
                                        </div>
                                    </div></div>
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresTitular">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTitular" runat="server" Text="Titular"></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:TextBox CssClass="form-control" ID="txtTitular" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTitular" runat="server" ControlToValidate="txtTitular"
                                                ErrorMessage="*" ValidationGroup="IngresarCheque" />
                                        </div></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlTarjetasCreditos" Visible="false" runat="server">
                        <div class="card">
                            <div class="card-header">
                                Datos de la Tarjeta
                            </div>
                            <div class="card-body">
                                <div class="form-group row">
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresTarjeta">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTarjetas" runat="server" Text="Tarjeta:"></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlTarjetas" runat="server"></asp:DropDownList>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTarjetas" runat="server" ControlToValidate="ddlTarjetas"
                                                ErrorMessage="*" ValidationGroup="IngresarTarjeta" />
                                        </div></div>
                                    </div>
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresNumeroTarjeta">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblNumeroTarjeta" runat="server" Text="Numero Tarjeta"></asp:Label>
                                        <div class="col-sm-9">
                                            <AUGE:NumericTextBox CssClass="form-control ValidarNumeroTarjeta" ID="txtNumeroTarjeta" MaxLength="20" runat="server"></AUGE:NumericTextBox>
                                            <%--<asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumeroTarjeta" runat="server" ControlToValidate="txtNumeroTarjeta" 
                    ErrorMessage="*" Enabled="false" ValidationGroup="IngresarTarjeta"/>--%>
                                        </div></div>
                                    </div>
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresTitularTarjeta">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTitularTarjeta" runat="server" Text="Titular"></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:TextBox CssClass="form-control" ID="txtTitularTarjeta" runat="server"></asp:TextBox>
                                        </div></div>
                                    </div>

                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresVencimientoMM">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblVencimientoMM" runat="server" Text="Vencimiento (MM)"></asp:Label>
                                        <div class="col-sm-9">
                                            <AUGE:NumericTextBox CssClass="form-control" ID="txtVencimientoMM" MaxLength="2" runat="server"></AUGE:NumericTextBox>
                                            <%--<asp:RequiredFieldValidator CssClass="Validador" ID="rfvVencimientoMM" runat="server" ControlToValidate="txtVencimientoMM"
                                                ErrorMessage="*" ValidationGroup="IngresarTarjeta" />--%>
                                        </div></div>
                                    </div>
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresVencimientoAA">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblVencimientoAA" runat="server" Text="Vencimiento (AA)"></asp:Label>
                                        <div class="col-sm-9">
                                            <AUGE:NumericTextBox CssClass="form-control" ID="txtVencimientoAA" MaxLength="2" runat="server"></AUGE:NumericTextBox>
                                      
                                        </div></div>
                                    </div>
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresPosnet">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblPosnet" runat="server" Text="Numero Posnet"></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:TextBox CssClass="form-control" ID="txtPosnet" runat="server"></asp:TextBox>
                                           <%-- <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPosnet" runat="server" ControlToValidate="txtPosnet"
                                                ErrorMessage="*" ValidationGroup="IngresarTarjeta" />--%>
                                        </div></div>
                                    </div>

                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresNumeroLote">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblNumeroLote" runat="server" Text="Numero Lote"></asp:Label>
                                        <div class="col-sm-9">
                                            <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroLote" runat="server"></AUGE:NumericTextBox>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumeroLote" runat="server" ControlToValidate="txtNumeroLote"
                                                ErrorMessage="*" ValidationGroup="IngresarTarjeta" />
                                        </div></div>
                                    </div>
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresCuotas">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCuotas" runat="server" Text="Cant. Cuotas"></asp:Label>
                                        <div class="col-sm-9">
                                            <AUGE:NumericTextBox CssClass="form-control" ID="txtCuotas" runat="server"></AUGE:NumericTextBox>
                                      <%--      <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCuotas" runat="server" ControlToValidate="txtCuotas"
                                                ErrorMessage="*" ValidationGroup="IngresarTarjeta" />--%>
                                        </div></div>
                                    </div>
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresFechaTransaccion">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaTransaccion" runat="server" Text="Fecha Transaccion"></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaTransaccion" runat="server"></asp:TextBox>
                                        </div></div>
                                    </div>

                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresObservaciones">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblObservaciones" runat="server" Text="Observacion"></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:TextBox CssClass="form-control" ID="txtObservaciones" runat="server" TextMode="MultiLine"></asp:TextBox>
                                        </div></div>
                                    </div>
                                  
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlDescuentoCargos" Visible="false" runat="server">
                        <div class="card">
                            <div class="card-header">
                                Datos para Descuento
                            </div>
                            <div class="card-body">
                                <div class="form-group row">
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresFormaCobroFtoCargo">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFormaCobroDtoCargo" runat="server" Text="Forma de Cobro"></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlFormasCobros" runat="server"></asp:DropDownList>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFormasCobros" runat="server" ControlToValidate="ddlFormasCobros"
                                                ErrorMessage="*" ValidationGroup="IngresarDescuentoCargos" />
                                        </div></div>
                                    </div>
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresPrimerVtoDtoCargo">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblPrimerVtoDtoCargo" runat="server" Text="Periodo Primer Vto."></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlPrimerVtoDtoCargo" runat="server"></asp:DropDownList>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPrimerVtoDtoCargo" runat="server" ControlToValidate="ddlPrimerVtoDtoCargo"
                                                ErrorMessage="*" ValidationGroup="IngresarDescuentoCargos" />
                                        </div></div>
                                    </div>
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresCantidadCuotasDtoCargo">     <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCantidadCuotasDtoCargo" runat="server" Text="Cantidad Cuotas" />
                                        <div class="col-sm-9">
                                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtCantidadCuotasDtoCargo" Prefix="" NumberOfDecimals="0" runat="server" />
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCantidadCuotasDtoCargo" runat="server" ControlToValidate="txtCantidadCuotasDtoCargo"
                                                ErrorMessage="*" ValidationGroup="IngresarDescuentoCargos" />
                                        </div></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlPrestamos" Visible="false" runat="server">
                        <div class="card">
                            <div class="card-header">
                                Datos para Prestamo
                            </div>
                            <div class="card-body">
                                <div class="form-group row">
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresTipoOperacionPrestamo">
                                        <div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTipoOperacionPrestamo" runat="server" Text="Tipo Operacion"></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoOperacionPrestamo" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoOperacionPrestamo_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoOperacionPrestamo" runat="server" ControlToValidate="ddlTipoOperacionPrestamo"
                                                ErrorMessage="*" ValidationGroup="IngresarPrestamo" />
                                        </div></div>
                                    </div>
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresFormaCobroPrestamo"><div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFormaCobroPrestamo" runat="server" Text="Forma de Cobro"></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlFormasCobrosPrestamos" AutoPostBack="true" OnSelectedIndexChanged="ddlFormasCobrosPrestamos_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFormasCobrosPrestamos" runat="server" ControlToValidate="ddlFormasCobrosPrestamos"
                                                ErrorMessage="*" ValidationGroup="IngresarPrestamo" />
                                        </div>
                                    </div></div>

                         
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresPlanesPrestamos"><div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblPlanesPrestamos" runat="server" Text="Plan"></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlPlanesPrestamos" AutoPostBack="true" OnSelectedIndexChanged="ddlPlanesPrestamos_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPlanesPrestamos" runat="server" ControlToValidate="ddlPlanesPrestamos"
                                                ErrorMessage="*" ValidationGroup="IngresarPrestamo" />
                                        </div>
                                    </div></div>
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresPeriodoPrimerVtoPrestamo"><div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblPeriodoPrimerVtoPrestamo" runat="server" Text="Periodo Primer Vto."></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlPeriodoPrimerVtoPrestamo" runat="server"></asp:DropDownList>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPeriodoPrimerVtoPrestamo" runat="server" ControlToValidate="ddlPeriodoPrimerVtoPrestamo"
                                                ErrorMessage="*" ValidationGroup="IngresarPrestamo" />
                                        </div>
                                    </div></div>

                             
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresCantidadCuotas"><div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCantidadCuotasPrestamo" runat="server" Text="Cantidad Cuotas" />
                                        <div class="col-sm-9">
                                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtCantidadCuotas" Prefix="" NumberOfDecimals="0" AutoPostBack="true" OnTextChanged="txtCantidadCuotas_TextChanged" runat="server" />
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCantidadCuotas" runat="server" ControlToValidate="txtCantidadCuotas"
                                                ErrorMessage="*" ValidationGroup="IngresarPrestamo" />
                                        </div></div>
                                    </div>
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresImporteCuota"><div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImporteCuota" runat="server" Text="Importe Cuotas" />
                                        <div class="col-sm-9">
                                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteCuotas" Enabled="false" runat="server" />
                                        </div></div>
                                    </div>

                                </div>
                                <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
                                    </ContentTemplate>
                                </asp:UpdatePanel>
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
                                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresTipoRetencionesPercepciones"><div class="row">
                                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTipoRetencionPercepcion" runat="server" Text="Tipo"></asp:Label>
                                        <div class="col-sm-9">
                                            <asp:DropDownList CssClass="form-control select2" ID="ddlTiposRetencionesPercepciones" runat="server">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTiposRetencionesPercepciones" runat="server" ControlToValidate="ddlTiposRetencionesPercepciones"
                                                ErrorMessage="*" Enabled="false" ValidationGroup="IngresarRetencionPercepcion" />
                                        </div></div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlRemitos" Visible="false" runat="server">
                        <div class="card">
                            <div class="card-header">
                                Remitos Asociados
                            </div>
                            <div class="card-body">
                                <asp:UpdatePanel ID="upRemitos" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <div class="form-group row">
                                            <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvOrdenesCobrosValoresRemitos"><div class="row">
                                                <asp:Label CssClass="col-sm-3 col-form-label" ID="lblRemitos" runat="server" Text="Remitos"></asp:Label>
                                                <div class="col-sm-9">
                                                    <asp:DropDownList CssClass="form-control select2" ID="ddlRemitos" AutoPostBack="true" OnSelectedIndexChanged="ddlRemitos_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvRemitos" runat="server" ControlToValidate="ddlRemitos"
                                                        ErrorMessage="*" ValidationGroup="IngresarRemitos" />
                                                </div></div>
                                            </div>

                                        </div>
                                        <div class="table-responsive">
                                            <asp:GridView ID="gvRemitosDatos" OnRowCommand="gvRemitosDatos_RowCommand"
                                                DataKeyNames="IndiceColeccion"
                                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Codigo Remito" SortExpression="NumeroRemito">
                                                        <ItemTemplate>
                                                            <%# Eval("IdRemito")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Fecha Remito" SortExpression="FechaRemito">
                                                        <ItemTemplate>
                                                            <%# Eval("FechaRemito", "{0:dd/MM/yyyy}")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Numero Remito" SortExpression="NumeroRemito">
                                                        <ItemTemplate>
                                                            <%# Eval("NumeroRemitoCompleto")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Estado" SortExpression="EstadoRemito">
                                                        <ItemTemplate>
                                                            <%# Eval("Estado.Descripcion")%>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Acciones">
                                                        <ItemTemplate>
                                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/print_f2.png" runat="server" CommandName="Impresion" ID="btnImprimir"
                                                                AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Anular" ID="btnBaja"
                                                                AlternateText="Eliminiar" ToolTip="Eliminiar" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </asp:Panel>


        <asp:Panel ID="pnlDetalleIngresos" runat="server">
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
                                <%# Eval("Detalle")%>
                                <%--<%# Eval("BancoCuenta.DescripcionFilialBancoTipoCuentaNumero")%>
                                <%# Eval("Cheque.BancoTitularCuit")%>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Fecha" ItemStyle-Wrap="false" DataFormatString="{0:dd/MM/yyyy}" DataField="Fecha" />
                        <%--<asp:BoundField  HeaderText="FechaDiferido" ItemStyle-Wrap="false" DataFormatString="{0:dd/MM/yyyy}" DataField="FechaDiferido" />--%>
                        <asp:TemplateField HeaderText="Fecha Diferido">
                            <ItemTemplate>
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDiferido" Text='<%# Eval("FechaDiferido")%>' Enabled="false" runat="server"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Numero" ItemStyle-Wrap="false" DataField="NumeroCheque" />
                        <asp:TemplateField HeaderText="Importe" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right"
                            ItemStyle-Wrap="false" FooterStyle-HorizontalAlign="Right" FooterStyle-Wrap="false">
                            <ItemTemplate>
                                <%# Eval("Importe", "{0:C2}")%>
                                <asp:HiddenField ID="hdfImporte" Value='<%#Bind("Importe") %>' runat="server" />
                                <%--<Evol:CurrencyTextBox CssClass="gvTextBox" ID="Importe" runat="server" Enabled="false" Text='<%#Bind("Importe", "{0:C2}") %>'></Evol:CurrencyTextBox>--%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label CssClass="labelFooterEvol" ID="lblTotal" runat="server" Text=""></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField  HeaderText="Importe" ItemStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="Importe" SortExpression="Importe" />--%>
                        <asp:TemplateField Visible="false" HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
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
