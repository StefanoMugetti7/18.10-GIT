<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlazosFijosDatos.ascx.cs" Inherits="IU.Modulos.Ahorros.Controles.PlazosFijosDatos" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadosBuscarPopUp.ascx" TagName="popUpAfiliadosBuscar" TagPrefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>


<auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />

<script type="text/javascript">
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControls);
        SetTabIndexInput();
        var txtFechaInicioVigencia;
        InitControls();
    });

    function SetTabIndexInput() {
        $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }

    function CalcularImporte() {
     
        var idcuenta = $('[id*="ddlCuenta"] option:selected').val();
        var importe = $("input[type=text][id$='txtImporte']").maskMoney('unmasked')[0];
       var importeRenovacion = $("input[type=hidden][id$='hdfImporteRenovacion']").val().replace('.', '').replace(',', '.');
        var SaldoActual = $("input[type=text][id$='txtSaldoActual']").maskMoney('unmasked')[0];
        //if (idcuenta > 0) {
        //    var ImporteTotal = (parseFloat(SaldoActual) + parseFloat(importeRenovacion) ).toFixed(2);    
        //    if ((parseFloat(ImporteTotal) - parseFloat(importe)) < 0) {
        //        $("input[type=text][id$='txtImporte']").val(accounting.formatMoney(ImporteTotal, gblSimbolo, 2, "."));
        //        //alert("El importe del Plazo Fijo no puede ser mayo al Saldo de la cuenta.")
        //    }
        //}
    }

    function CalcularTotal() {
        var total = 0.00;
        //var importe = ctrl.value.replace('.', '').replace(',', '.');
        var importe = $("input[type=text][id*='txtImporte']").maskMoney('unmasked')[0];
        var interes = $("input[type=text][id*='txtTasaInteres']").maskMoney('unmasked')[0];
        var dias = $("input[type=text][id*='txtDias']").val().replace('.', '').replace(',', '.');

        if (!(importe && dias && interes))
            return;

        //alert(importe);
        //Math.Round(pParametro.ImporteCapital * pParametro.TasaInteres / 365 * pParametro.PlazoDias / 100))
        var diasPorAnio = $("input[type=hidden][id$='hdfDiasPorAnio']").val();
        if (isNaN(diasPorAnio)) {
            diasPorAnio = 365;
        }
        
        var importeInteres = Math.round(importe * interes / parseInt(diasPorAnio) * dias) / 100;
        total = parseFloat(parseFloat(importe) + parseFloat(importeInteres)).toFixed(2);
        //total = total.replace('.', ',');
        $("input[type=text][id*='txtImporteInteres']").val(accounting.formatMoney(parseFloat(importeInteres).toFixed(2), gblSimbolo, gblCantidadDecimales, "."));
        $("input[type=text][id*='txtImporteTotal']").val(accounting.formatMoney(total, gblSimbolo, gblCantidadDecimales, "."));
    }


    function InitControls() {
        $("input[type=text][id$='txtImporte']").change(CalcularImporte);
        $("input[type=text][id$='txtImporte']").change(CalcularTotal);
        $("input[type=text][id$='txtTasaInteres']").change(CalcularTotal);

        var isDisabled = $('input:text[id$=txtFechaInicioVigencia]').prop('disabled');
        if (!isDisabled) {
            txtFechaInicioVigencia = $('input:text[id$=txtFechaInicioVigencia]').datepicker({
                showOnFocus: true,
                uiLibrary: 'bootstrap4',
                locale: 'es-es',
                format: 'dd/mm/yyyy',
            });
            txtFechaInicioVigencia.change(OnSelectedFechaInicioVigencia);
        }
    }

    function OnSelectedFechaInicioVigencia() {
        var egreso = new Date(toDate(txtFechaInicioVigencia.value()));
        var cantidadDias = $("input[type=text][id*='txtDias']").val().replace('.', '').replace(',', '.');
        //var cantidadDias = txtCantidadDias.val() == '' ? 1 : txtCantidadDias.val();
        $("input:text[id$='txtDias']").val(cantidadDias);
        egreso.setDate(egreso.getDate() + parseInt(cantidadDias));
        $('input:text[id$=txtFechaVencimiento]').val(toStrDate(egreso));
    }

    function ValidarShowConfirmRegistraCajaAhorro(ctrl, msg) {
        //if (Page_ClientValidate("Aceptar")) {
        var importeRenovacion = $("input[type=hidden][id$='hdfImporteRenovacion']").val().replace('.', '').replace(',', '.');
        var importe = $("input[type=text][id*='txtImporte']").maskMoney('unmasked')[0];
        //console.log(importeRenovacion);
        //console.log(importe);
        //chkRegistrarCaja
        //chkCancelarCajaAhorros
        //if (($('[id$="ddlCuenta"] option:selected').val() > 0 && importeRenovacion == "0") 
        //    || parseFloat(importeRenovacion) == parseFloat(importe)) {
        //        showConfirm(ctrl, msg);
        //    } else {
        //        __doPostBack(ctrl.name, '');
        //    }
        if (($("input[id$='chkRegistrarCaja']").is(':checked')) 
            || parseFloat(importeRenovacion) == parseFloat(importe)
            ) {
                showConfirm(ctrl, msg);
            } else {
                __doPostBack(ctrl.name, '');
            }
       // }
    }

    function ValidarShowConfirmCancelaCajaAhorro(ctrl, msg) {
        if ($("input[id$='chkCancelarCajaAhorros']").is(':checked')) {
            showConfirm(ctrl, msg);
        }
        else {
            __doPostBack(ctrl.name, '');
        }
    }

    function ValidarShowConfirmBaja(ctrl, msg) {
        //if (Page_ClientValidate("Aceptar")) {
        var bcheck = $("input[id$='chkCancelarCajaAhorros']").prop('checked');
        //console.log(importeRenovacion);
        //console.log(importe);
   
        if (bcheck) {
                showConfirm(ctrl, msg);
            } else {
                __doPostBack(ctrl.name, '');
            }
       // }
    }
</script>
<div class="PlazosFijosDatos">
    <asp:HiddenField ID="hdfDiasPorAnio" runat="server" />
    <asp:UpdatePanel ID="upSaldoActual" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuenta" runat="server" Text="Cuenta" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlCuenta" Enabled="false" runat="server" OnSelectedIndexChanged="ddlCuenta_SelectedIndexChanged"
                        AutoPostBack="true" />
                    <%--<asp:RequiredFieldValidator CssClass="Validador" ID="rfvCuenta" runat="server" InitialValue="" ControlToValidate="ddlCuenta" 
                ErrorMessage="*" ValidationGroup="Aceptar"/>--%>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSaldoActual" runat="server" Text="Saldo Actual" />
                <div class="col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtSaldoActual" runat="server" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoRenovacion" runat="server" Text="Tipo Renovación" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoRenovacion" runat="server" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoRenovacion" runat="server" ControlToValidate="ddlTipoRenovacion"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upTasaInteres" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlMoneda" Enabled="false" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlMoneda_SelectedIndexChanged" />
                    <asp:RequiredFieldValidator ID="rfvMoneda" CssClass="Validador" runat="server" ControlToValidate="ddlMoneda"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPlazo" runat="server" Text="Plazo - Interes" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlPlazo" runat="server" OnSelectedIndexChanged="ddlPlazo_SelectedIndexChanged"
                        AutoPostBack="true" />
                    <asp:RequiredFieldValidator ID="rfvPlazo" CssClass="Validador" runat="server" ControlToValidate="ddlPlazo"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCantidadDias" runat="server" Text="Cantidad Días" />
                <div class="col-sm-3">
                    <AUGE:NumericTextBox CssClass="form-control" ID="txtDias" runat="server" AutoPostBack="true" OnTextChanged="txtDias_TextChanged" />
                     <asp:RequiredFieldValidator ID="rfvDias" CssClass="Validador" runat="server" ControlToValidate="txtDias"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTasaEspecial" runat="server" Text="Tasa Especial" />
                <div class="col-sm-3">
                    <asp:CheckBox ID="cbTasaEspecial" CssClass="form-control" runat="server" OnCheckedChanged="cbTasaEspecial_CheckedChanged" AutoPostBack="true" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTasaInteres" runat="server" Text="Tasa Interes" />
                <div class="col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtTasaInteres" Prefix="" runat="server" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTasaInteres" runat="server" InitialValue="" ControlToValidate="txtTasaInteres"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                    <asp:RangeValidator ID="rvTasaInteres" runat="server" ErrorMessage="*" ControlToValidate="txtTasaInteres"
                        MinimumValue="0" MaximumValue="100" Type="Currency" ValidationGroup="Aceptar" />
                </div>
                <div class="col-sm-3"></div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaInicioVigencia" runat="server" Text="Fecha Inicio Vigencia" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtFechaInicioVigencia" runat="server" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaVencimiento" runat="server" Text="Fecha Vencimiento" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtFechaVencimiento" Enabled="false" runat="server" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporte" runat="server" Text="Importe" />
                <div class="col-sm-3">
                    <asp:HiddenField ID="hdfImporteRenovacion" Value="0" runat="server" />
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporte" runat="server" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporte" runat="server" ControlToValidate="txtImporte"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteInteres" runat="server" Text="Estimulo" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtImporteInteres" Style="text-align: right;" Enabled="false" runat="server" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteTotal" runat="server" Text="Total" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtImporteTotal" Style="text-align: right;" Enabled="false" runat="server" />
                </div>
            </div>
            <div class="form-group row">
                  <asp:Label CssClass="col-sm-1 col-form-label" ID="lblRegistrarCaja" runat="server" Text="Registrar Caja Ahorros" />
                <div class="col-sm-3">
                    <asp:CheckBox ID="chkRegistrarCaja" Enabled="false" CssClass="form-control" runat="server" />
                </div>
            </div>
                 <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
          
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlPago" Visible="false" runat="server">
        <div class="form-group row">
             <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvFilialPago">
                <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lblFilialPago" runat="server" Text="Filial de Pago" />
            <div class="col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlFilialPago" runat="server" />
            </div>
                </div>
            </div>
             <div class="col-12 col-md-8 col-lg-4" runat="server" visible="false" id="dvTipoValorPago">
                <div class="row">
            <asp:Label CssClass="col-sm-3 col-form-label" ID="lblTipoValorPago" Visible="false" runat="server" Text="Tipo Valor Pago" />
            <div class="col-sm-9">
                <asp:DropDownList CssClass="form-control select2" ID="ddlTipoValorPago" Visible="false" runat="server" />
            </div>
                    </div>
                 </div>
             <div class="col-12 col-md-8 col-lg-4" runat="server" visible="false" id="dvCancelarCajaAhorros">
                <div class="row">
              <asp:Label CssClass="col-sm-3 col-form-label" ID="lblCancelarCajaAhorros" Visible="false" runat="server" Text="Cancelar a Caja de Ahorros" />
                <div class="col-sm-9">
                    <asp:CheckBox ID="chkCancelarCajaAhorros" CssClass="form-control" Enabled="false" Visible="false" runat="server"/>
                </div>
                    </div>
                 </div>
        </div>
    </asp:Panel>
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpCotitulares" HeaderText="Cotitulares">
            <ContentTemplate>
                <asp:UpdatePanel ID="upCotitulares" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <AUGE:popUpAfiliadosBuscar ID="ctrAfiliados" runat="server" />
                        <div class="form-group row">
                            <div class="col-sm-3">
                                <asp:Button CssClass="botonesEvol" ID="btnAgregarCotitular" runat="server" Text="Agregar Cotitular"
                                    OnClick="btnAgregarCotitular_Click" Visible="false" CausesValidation="false" />
                            </div>
                            <div class="col-sm-6"></div>
                        </div>
                        <div class="table-responsive">
                            <asp:GridView ID="gvDatos" OnRowCommand="gvDatos_RowCommand"
                                OnRowDataBound="gvDatos_RowDataBound" DataKeyNames="IndiceColeccion"
                                runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="Apellido" SortExpression="Afiliado.Apellido">
                                        <ItemTemplate>
                                            <%# Eval("Afiliado.Apellido")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nombre" SortExpression="Afiliado.Nombre">
                                        <ItemTemplate>
                                            <%# Eval("Afiliado.Nombre")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo" SortExpression="Afiliado.TipoDocumento.TipoDocumento">
                                        <ItemTemplate>
                                            <%# Eval("Afiliado.TipoDocumento.TipoDocumento")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Número" SortExpression="Afiliado.NumeroDocumento" ItemStyle-Wrap="false">
                                        <ItemTemplate>
                                            <%# Eval("Afiliado.NumeroDocumento")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Categoria" SortExpression="Afiliado.Categoria.Categoria">
                                        <ItemTemplate>
                                            <%# Eval("Afiliado.Categoria.Categoria")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Parentesco" SortExpression="Afiliado.Parentesco.Parentesco">
                                        <ItemTemplate>
                                            <%# Eval("Afiliado.Parentesco.Parentesco")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Acciones" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:ImageButton CommandArgument='<%# Container.DisplayIndex%>' ImageUrl="~/Imagenes/Baja.png" runat="server" CommandName="Borrar" ID="btnEliminar" Visible="false"
                                                AlternateText="Elminiar" ToolTip="Eliminar" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
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

    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="row justify-content-md-center">
                <div class="col-md-auto">
                    <asp:ImageButton ImageUrl="~/Imagenes/print_f2.png" runat="server" ID="btnImprimir" Visible="false"
                    OnClick="btnImprimir_Click" AlternateText="Imprimir Comprobante" ToolTip="Imprimir Comprobante" />
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
