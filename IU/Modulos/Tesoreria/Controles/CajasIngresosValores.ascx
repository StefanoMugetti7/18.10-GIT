<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CajasIngresosValores.ascx.cs" Inherits="IU.Modulos.Tesoreria.Controles.CajasIngresosValores" %>
<%@ Register Src="~/Modulos/Tesoreria/Controles/ChequesTercerosPopUp.ascx" TagName="popUpBuscarCheque" TagPrefix="auge" %>
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

</script>
<asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlDetalle" Visible="false" GroupingText="Detalle de Movimiento" runat="server">
            <asp:Panel ID="pnlFormasCobros" runat="server">

                <div class="form-group row">

                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresFormaCobro">
                        <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFormaCobro" runat="server" Text="Tipo de Valor" />
                            <div class="col-sm-9">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlTiposValores" runat="server" OnSelectedIndexChanged="ddlTiposValores_SelectedIndexChanged"
                                    AutoPostBack="true" />

                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTiposValores" runat="server" ControlToValidate="ddlTiposValores"
                                    ErrorMessage="*" ValidationGroup="IngresarCobro" />
                            </div>
                        </div>
                    </div>


                    <%--BOTON INGRESAR CHEQUE--%>
                    <AUGE:popUpBuscarCheque ID="ctrBuscarCheque" runat="server" />
                    
                            <asp:Button CssClass="botonesEvol" ID="btnAgregarCheque" Visible="false" runat="server" Text="Buscar Cheques terceros"
                                OnClick="btnBuscarCheque_Click" />

                       

                
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresImporte">
                        <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImporte" runat="server" Text="Importe" />
                            <div class="col-sm-9">
                                <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporte" runat="server" />
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporte" runat="server" ControlToValidate="txtImporte"
                                    ErrorMessage="*" ValidationGroup="IngresarCobro" />
                            </div>
                        </div>
                    </div>
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvbtnIngresarCobro">
                        <div class="row">

                            <asp:Button CssClass="botonesEvol" ID="btnIngresarCobro" runat="server" Text="Ingresar Valor"
                                OnClick="btnIngresarCobro_Click" ValidationGroup="IngresarCobro" />
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlTransferencias" GroupingText="Datos de la Cuenta" Visible="false" runat="server">
                <div class="form-group row">
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresFechaTransferencia">
                        <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaTransferencia" runat="server" Text="Fecha"></asp:Label>
                            <div class="col-sm-9">
                                <asp:TextBox CssClass="form-control datepicker" ID="txtFechaTransferencia" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaTransferencia" runat="server" ControlToValidate="txtFechaTransferencia"
                                    ErrorMessage="*" Enabled="false" ValidationGroup="IngresarTransferencia" />
                            </div>
                        </div>
                    </div>
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresBancosCuentas">
                        <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblBancosCuentas" runat="server" Text="Cuenta"></asp:Label>
                            <div class="col-sm-9">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlBancosCuentasTransferencias" runat="server">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvBancosCuentasTransferencias" runat="server" ControlToValidate="ddlBancosCuentasTransferencias"
                                    ErrorMessage="*" Enabled="false" ValidationGroup="IngresarTransferencia" />
                            </div>
                        </div>
                    </div>
                </div>
                <AUGE:CamposValores ID="ctrCamposValoresTransferencias" ctrValidationGroup="IngresarCobro" runat="server" />
            </asp:Panel>
            <asp:Panel ID="pnlCheques" GroupingText="Datos del Cheque" Visible="false" runat="server">
                <div class="form-group row">
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresFecha">
                         <div class="row">
                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFecha" runat="server" Text="Fecha"></asp:Label>
                        <div class="col-sm-9">
                            <asp:TextBox CssClass="form-control datepicker" ID="txtFecha" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFecha" runat="server" ControlToValidate="txtFecha"
                                ErrorMessage="*" Enabled="false" ValidationGroup="IngresarCheque" />
                        </div></div>
                    </div>
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresFechaDiferido"> <div class="row">
                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaDiferido" runat="server" Text="Fecha Diferido"></asp:Label>
                        <div class="col-sm-9">
                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDiferido" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvChequeDiferido" runat="server" ControlToValidate="txtFechaDiferido"
                                ErrorMessage="*" Enabled="false" ValidationGroup="IngresarCheque" />
                        </div></div>
                    </div>

                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresNumeroCheque"> <div class="row">
                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblNumeroCheque" runat="server" Text="Numero Cheque"></asp:Label>
                        <div class="col-sm-9">
                            <asp:TextBox CssClass="form-control" ID="txtNumeroCheque" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumeroCheque" runat="server" ControlToValidate="txtNumeroCheque"
                                ErrorMessage="*" Enabled="false" ValidationGroup="IngresarCheque" />
                        </div></div>
                    </div>
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresConcepto"> <div class="row">
                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblConcepto" runat="server" Text="Concepto"></asp:Label>
                        <div class="col-sm-9">
                            <asp:TextBox CssClass="form-control" ID="txtConcepto" runat="server"></asp:TextBox>
                        </div></div>
                    </div>
                </div>
                <asp:Panel ID="pnlChequesPropios" Visible="false" runat="server">
                    <div class="form-group row">
                        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresBancosCuentasCheques"> <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="Label2" runat="server" Text="Banco Cuenta"></asp:Label>
                            <div class="col-sm-9">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlBancosCuentasCheques" runat="server">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvBancosCuentasCheques" runat="server" ControlToValidate="ddlBancosCuentasCheques"
                                    ErrorMessage="*" Enabled="false" ValidationGroup="IngresarCheque" />
                            </div></div>
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlChequesTerceros" Visible="false" runat="server">
                    <div class="form-group row">
                        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresBancos"> <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblBanco" runat="server" Text="Banco"></asp:Label>
                            <div class="col-sm-9">
                                <asp:DropDownList CssClass="form-control select2" ID="ddlBancos" runat="server">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvBancos" runat="server" ControlToValidate="ddlBancos"
                                    ErrorMessage="*" Enabled="false" ValidationGroup="IngresarCheque" />
                            </div>
                        </div></div>
                        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresCUIT"> <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCuit" runat="server" Text="CUIT"></asp:Label>
                            <div class="col-sm-9">
                                <AUGE:NumericTextBox CssClass="form-control" ID="txtCUIT" MaxLength="11" runat="server"></AUGE:NumericTextBox>
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCUIT" runat="server" ControlToValidate="txtCUIT"
                                    ErrorMessage="*" Enabled="false" ValidationGroup="IngresarCheque" />
                            </div></div>
                        </div>
                        <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresTitular"> <div class="row">
                            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTitular" runat="server" Text="Titular"></asp:Label>
                            <div class="col-sm-9">
                                <asp:TextBox CssClass="form-control" ID="txtTitular" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTitular" runat="server" ControlToValidate="txtTitular"
                                    ErrorMessage="*" Enabled="false" ValidationGroup="IngresarCheque" />
                            </div></div>
                        </div>
                    </div>
                </asp:Panel>


            </asp:Panel>
            <asp:Panel ID="pnlTarjetasCreditos" Visible="false" GroupingText="Datos de la Tarjeta" runat="server">
                <div class="form-group row">
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresTarjetas"> <div class="row">
                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTarjetas" runat="server" Text="Tarjeta:"></asp:Label>
                        <div class="col-sm-9">
                            <asp:DropDownList CssClass="form-control select2" ID="ddlTarjetas" runat="server"></asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTarjetas" runat="server" ControlToValidate="ddlTarjetas"
                                ErrorMessage="*" Enabled="false" ValidationGroup="IngresarTarjeta" />
                        </div></div>
                    </div>
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresNumeroTarjeta"> <div class="row">
                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblNumeroTarjeta" runat="server" Text="Numero Tarjeta"></asp:Label>
                        <div class="col-sm-9">
                            <AUGE:NumericTextBox CssClass="form-control ValidarNumeroTarjeta" ID="txtNumeroTarjeta" MaxLength="20" runat="server"></AUGE:NumericTextBox>
                            <%--<asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumeroTarjeta" runat="server" ControlToValidate="txtNumeroTarjeta" 
                    ErrorMessage="*" Enabled="false" ValidationGroup="IngresarTarjeta"/>--%>
                        </div></div>
                    </div>
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresTitularTarjeta"> <div class="row">
                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTitularTarjeta" runat="server" Text="Titular"></asp:Label>
                        <div class="col-sm-9">
                            <asp:TextBox CssClass="form-control" ID="txtTitularTarjeta" runat="server"></asp:TextBox>
                        </div></div>
                    </div>
           
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresVencimientoMM"> <div class="row">
                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblVencimientoMM" runat="server" Text="Vencimiento (MM)"></asp:Label>
                        <div class="col-sm-9">
                            <AUGE:NumericTextBox CssClass="form-control" ID="txtVencimientoMM" MaxLength="2" runat="server"></AUGE:NumericTextBox>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvVencimientoMM" runat="server" ControlToValidate="txtVencimientoMM"
                                ErrorMessage="*" Enabled="false" ValidationGroup="IngresarTarjeta" />
                        </div></div>
                    </div>
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresVencimientoAA"> <div class="row">
                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblVencimientoAA" runat="server" Text="Vencimiento (AA)"></asp:Label>
                        <div class="col-sm-9">
                            <AUGE:NumericTextBox CssClass="form-control" ID="txtVencimientoAA" MaxLength="2" runat="server"></AUGE:NumericTextBox>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvVencimientoAA" runat="server" ControlToValidate="txtVencimientoAA"
                                ErrorMessage="*" Enabled="false" ValidationGroup="IngresarTarjeta" />
                        </div></div>
                    </div>
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresPosnet"> <div class="row">
                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblPosnet" runat="server" Text="Numero Posnet"></asp:Label>
                        <div class="col-sm-9">
                            <asp:TextBox CssClass="form-control" ID="txtPosnet" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPosnet" runat="server" ControlToValidate="txtPosnet"
                                ErrorMessage="*" Enabled="false" ValidationGroup="IngresarTarjeta" />
                        </div></div>
                    </div>
            
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresNumeroLote"> <div class="row">
                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblNumeroLote" runat="server" Text="Numero Lote"></asp:Label>
                        <div class="col-sm-9">
                            <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroLote" runat="server"></AUGE:NumericTextBox>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNumeroLote" runat="server" ControlToValidate="txtNumeroLote"
                                ErrorMessage="*" Enabled="false" ValidationGroup="IngresarTarjeta" />
                        </div>
                    </div></div>
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresCuotas"> <div class="row">
                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCuotas" runat="server" Text="Cant. Cuotas"></asp:Label>
                        <div class="col-sm-9">
                            <AUGE:NumericTextBox CssClass="form-control" ID="txtCuotas" runat="server"></AUGE:NumericTextBox>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCuotas" runat="server" ControlToValidate="txtCuotas"
                                ErrorMessage="*" Enabled="false" ValidationGroup="IngresarTarjeta" />
                        </div></div>
                    </div>
           
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresFechaTransaccion"> <div class="row">
                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFechaTransaccion" runat="server" Text="Fecha Transaccion"></asp:Label>
                        <div class="col-sm-9">
                            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaTransaccion" runat="server"></asp:TextBox>

                        </div>
                    </div></div>
                    <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvCajasIngresosValoresObservaciones"> <div class="row">
                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblObservaciones" runat="server" Text="Observaciones"></asp:Label>
                        <div class="col-sm-9">
                            <asp:TextBox CssClass="form-control" ID="txtObservaciones" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </div>
                    </div></div>
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

            <AUGE:CamposValores ID="ctrCamposValores" runat="server" />

            <asp:Panel ID="pnlDetalleIngresos" GroupingText="" runat="server">
                <div class="table-responsive">
                    <asp:GridView ID="gvFormasCobros" DataKeyNames="IndiceColeccion"
                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                        OnRowDataBound="gvFormasCobros_RowDataBound" OnPageIndexChanging="gvFormasCobros_PageIndexChanging"
                        OnRowCommand="gvFormasCobros_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Tipo Valor">
                                <ItemTemplate>
                                    <%# Eval("TipoValor.TipoValor")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Fecha" ItemStyle-Wrap="false" DataFormatString="{0:dd/MM/yyyy}" DataField="Fecha" />
                            <asp:TemplateField HeaderText="Descripcion">
                                <ItemTemplate>
                                    <%# Eval("Descripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="Importe">
                                <ItemTemplate>
                                    <%# Eval("Importe", "{0:C2}")%>
                                    <asp:HiddenField ID="hdfImporte" Value='<%#Bind("Importe") %>' runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label CssClass="labelFooterEvol" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
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
            
            <asp:Panel ID="pnlDetalleValores" GroupingText="Detalle de Cheques" Visible="false" runat="server">
                <div class="table-responsive">
                    <asp:GridView ID="gvCheques" DataKeyNames="IndiceColeccion"
                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                        OnRowDataBound="gvCheques_RowDataBound" OnPageIndexChanging="gvCheques_PageIndexChanging"
                        OnRowCommand="gvCheques_RowCommand">
                        <Columns>
                            <asp:BoundField HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" DataField="Fecha" />
                            <%--<asp:BoundField  HeaderText="FechaDiferido" DataFormatString="{0:dd/MM/yyyy}" DataField="FechaDiferido" />--%>
                            <asp:TemplateField HeaderText="Fecha Diferido">
                                <ItemTemplate>
                                    <div class="col-sm-9">
                                        <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDiferido" Text='<%# Eval("FechaDiferido")%>' Enabled="false" runat="server"></asp:TextBox>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Concepto" DataField="Concepto" />
                            <%--<asp:BoundField  HeaderText="Numero Cheque" DataField="NumeroCheque" />--%>
                            <asp:TemplateField HeaderText="Numero Cheque">
                                <ItemTemplate>
                                    <AUGE:NumericTextBox CssClass="form-control" ID="txtNumeroCheque" Text='<%# Eval("NumeroCheque")%>' MaxLength="11" runat="server"></AUGE:NumericTextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Banco">
                                <ItemTemplate>
                                    <%# Eval("Banco.Descripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="CUIT" DataField="CUIT" />
                            <asp:BoundField HeaderText="Titular" DataField="TitularCheque" />
                            <asp:TemplateField HeaderText="Importe" ItemStyle-Wrap="false" FooterStyle-Wrap="false" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" SortExpression="Importe">
                                <ItemTemplate>
                                    <%# Eval("Importe", "{0:C2}")%>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblImporteTotal" runat="server" Text=""></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="true"
                                        AlternateText="Elminiar" ToolTip="Eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlDetalleChequesTerceros" GroupingText="Detalle de Cheques Terceros" Visible="false" runat="server">
                <div class="table-responsive">
                    <asp:GridView ID="gvChequesTerceros" DataKeyNames="IndiceColeccion"
                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                        OnRowDataBound="gvChequesTerceros_RowDataBound" OnPageIndexChanging="gvChequesTerceros_PageIndexChanging"
                        OnRowCommand="gvChequesTerceros_RowCommand">
                        <Columns>
                            <asp:BoundField HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" DataField="Fecha" />
                            <asp:BoundField HeaderText="Fecha Dif" DataFormatString="{0:dd/MM/yyyy}" DataField="FechaDiferido" />
                            <asp:BoundField HeaderText="Concepto" DataField="Concepto" />
                            <asp:BoundField HeaderText="Numero Cheque" DataField="NumeroCheque" />
                            <asp:TemplateField HeaderText="Banco">
                                <ItemTemplate>
                                    <%# Eval("Banco.Descripcion")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="CUIT" DataField="CUIT" />
                            <asp:BoundField HeaderText="Titular" DataField="TitularCheque" />
                            <asp:BoundField HeaderText="Importe" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="Importe" SortExpression="Importe" />
                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="true"
                                        AlternateText="Elminiar" ToolTip="Eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlDetalleTransferencia" GroupingText="Detalle de Transferencias" Visible="false" runat="server">
                <div class="table-responsive">
                    <asp:GridView ID="gvTransferencias" AllowPaging="true"
                        OnRowDataBound="gvTransferencias_RowDataBound" DataKeyNames="IndiceColeccion"
                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                        OnPageIndexChanging="gvTransferencias_PageIndexChanging"
                        OnRowCommand="gvTransferencias_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Fecha">
                                <ItemTemplate>
                                    <%# Eval("FechaMovimiento", "{0:dd/MM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Tipo Operacion" >
                                <ItemTemplate>
                                    <%# Eval("TipoOperacion.TipoOperacion")%>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:BoundField HeaderText="Numero Operación" DataField="NumeroTipoOperacion" />
                            <asp:TemplateField HeaderText="Detalle">
                                <ItemTemplate>
                                    <%# string.Concat( Eval("BancoCuenta.DescripcionFilialBancoTipoCuentaNumero"), " - ", Eval("BancoCuenta.Denominacion"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Importe" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# Eval("Importe", "{0:C2}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="true"
                                        AlternateText="Elminiar" ToolTip="Eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlDetalleFondoFijo" GroupingText="Detalle de Cuenta" Visible="false" runat="server">
                <div class="table-responsive">
                    <asp:GridView ID="gvFondoFijo" AllowPaging="true"
                        OnRowDataBound="gvFondoFijo_RowDataBound" DataKeyNames="IndiceColeccion"
                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                        OnPageIndexChanging="gvFondoFijo_PageIndexChanging"
                        OnRowCommand="gvFondoFijo_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Fecha">
                                <ItemTemplate>
                                    <%# Eval("FechaMovimiento", "{0:dd/MM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Tipo Operacion" >
                                <ItemTemplate>
                                    <%# Eval("TipoOperacion.TipoOperacion")%>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:BoundField HeaderText="Numero Operación" DataField="NumeroTipoOperacion" />
                            <asp:TemplateField HeaderText="Detalle">
                                <ItemTemplate>
                                    <%# string.Concat( Eval("BancoCuenta.Denominacion"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Importe" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# Eval("Importe", "{0:C2}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="true"
                                        AlternateText="Elminiar" ToolTip="Eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlDetalleTarjetasPropias" GroupingText="Detalle de Tarjetas Propias" Visible="false" runat="server">
                <div class="table-responsive">
                    <asp:GridView ID="gvTarjetasPropias" AllowPaging="true"
                        OnRowDataBound="gvTarjetasPropias_RowDataBound" DataKeyNames="IndiceColeccion"
                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                        OnPageIndexChanging="gvTarjetasPropias_PageIndexChanging"
                        OnRowCommand="gvTarjetasPropias_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Fecha">
                                <ItemTemplate>
                                    <%# Eval("FechaMovimiento", "{0:dd/MM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Tipo Operacion" >
                                <ItemTemplate>
                                    <%# Eval("TipoOperacion.TipoOperacion")%>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:BoundField HeaderText="Numero Operación" DataField="NumeroTipoOperacion" />
                            <asp:TemplateField HeaderText="Detalle">
                                <ItemTemplate>
                                    <%# string.Concat( Eval("BancoCuenta.DescripcionFilialBancoTipoCuentaNumero"), " - ", Eval("BancoCuenta.Denominacion"))%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Importe" HeaderStyle-CssClass="text-right" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%# Eval("Importe", "{0:C2}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="true"
                                        AlternateText="Elminiar" ToolTip="Eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlDetalleTarjetas" GroupingText="Detalle de Tarjetas" Visible="false" runat="server">
                <div class="table-responsive">
                    <asp:GridView ID="gvTarjetas"
                        OnRowDataBound="gvTarjetas_RowDataBound" DataKeyNames="IndiceColeccion"
                        runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false" ShowFooter="true"
                        OnPageIndexChanging="gvTarjetas_PageIndexChanging" OnRowCommand="gvTarjetas_RowCommand">
                        <Columns>

                            <asp:BoundField HeaderText="Tarjeta" DataField="TarjetaDescripcion" />
                            <asp:BoundField HeaderText="Numero" DataField="NumeroTarjetaCredito" />
                            <asp:BoundField HeaderText="Vencimiento MM" DataField="VencimientoMes" />
                            <asp:BoundField HeaderText="Vencimiento AA" DataField="VencimientoAnio" />
                            <asp:BoundField HeaderText="Importe" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C2}" DataField="Importe" SortExpression="Importe" />

                            <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="true"
                                        AlternateText="Elminiar" ToolTip="Eliminar" />
                                </ItemTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
           
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
