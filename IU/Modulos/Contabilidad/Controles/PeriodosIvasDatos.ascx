<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PeriodosIvasDatos.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.PeriodosIvasDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>

<script lang="javascript" type="text/javascript">
    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControl);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(SetTabIndexInput);
        SetTabIndexInput();
        InitControl();
    });
    function InitControl() {
        $("input:text[id$='txtIVAPosicion']").bind("change", CalcularTotalImporte);
        $("input:text[id$='txtPercepciones']").bind("change", CalcularTotalImporte);
        $("input:text[id$='txtRetenciones']").bind("change", CalcularTotalImporte);
        $("input:text[id$='txtIVAVentas']").bind("change", CalcularTotalImporte);
        $("input:text[id$='txtIVACompras']").bind("change", CalcularTotalImporte);
    }
    function CalcularTotalImporte() {
        var importeNuevo = 0;
        var importeIVAVentas = $("input:text[id*='txtIVAVentas']").maskMoney('unmasked')[0]
        var importeIVACompras = $("input:text[id*='txtIVACompras']").maskMoney('unmasked')[0]

        importeNuevo2 = importeIVAVentas - importeIVACompras;

        $("input:text[id$='txtIVAPosicion']").val((accounting.formatMoney(importeNuevo2, "$ ", 2, ".")));

        var importeNuevo2 = 0;
        var importeIVAPosicion = $("input:text[id*='txtIVAPosicion']").maskMoney('unmasked')[0]
        var importePercepciones = $("input:text[id*='txtPercepciones']").maskMoney('unmasked')[0]
        var importeRetenciones = $("input:text[id*='txtRetenciones']").maskMoney('unmasked')[0]
        var importeSaldoTecnico = $("input:text[id*='txtSaldoTecnico']").maskMoney('unmasked')[0]
        importeNuevo2 = importeIVAPosicion - importePercepciones - importeRetenciones - importeSaldoTecnico;
        $("input:text[id$='txtIVAAPagar']").val((accounting.formatMoney(importeNuevo2, "$ ", 2, ".")));
    }
    function SetTabIndexInput() {
        $(":input").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }
</script>

<div class="PeriodosIVASDatos">
    <asp:UpdatePanel ID="upPeriodo" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEjercicioContable" runat="server" Text="Ejercicio:" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEjercicioContable" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEjerciciosContables_SelectedIndexChanged" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvEjercicio" runat="server" ErrorMessage="*" ControlToValidate="ddlEjercicioContable" ValidationGroup="Aceptar" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPeriodo" runat="server" Text="Período AAAAMM" />
                <div class="col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtPeriodo" Enabled="false" MaxLength="6" Prefix="" NumberOfDecimals="0" ThousandsSeparator="" runat="server" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaCierre" runat="server" Text="Fecha Cierre" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaCierre" runat="server" Enabled="false" />
                </div>
                <div class="col-sm-4">
                    <asp:Button CssClass="botonesEvol" ID="btnArmarLiquidacion" runat="server" Text="Armar Liquidacion" OnClick="btnArmarLiquidacion_Click" required />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaContable" runat="server" Text="Fecha Contable" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaContable" runat="server" Enabled="true" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upIvas" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvIVAVentas">
                    <div class="row">
                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblIVAVentas" runat="server" Text="IVA Ventas" />
                        <div class="col-sm-9">
                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtIVAVentas" Enabled="true" Visible="true" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvIVACompras">
                    <div class="row">
                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblIVACompras" runat="server" Text="IVA Compras" />
                        <div class="col-sm-9">
                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtIVACompras" Enabled="true" Visible="true" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-12 col-md-8 col-lg-4" runat="server" id="dvIVAPosicion">
                    <div class="row">
                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblIVAPosicion" runat="server" Text="Posicion de IVA" />
                        <div class="col-sm-9">
                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtIVAPosicion" Enabled="false" Visible="true" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="form-group row">
                <div class="col-12 col-md-8 col-lg-4" runat="server" id="Div1">
                    <div class="row">
                    <asp:Label CssClass="col-sm-3 col-form-label" ID="lvlPercepciones" runat="server" Text="Percepciones" />
                        <div class="col-sm-9">
                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtPercepciones" Enabled="true" Visible="true" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-12 col-md-8 col-lg-4" runat="server" id="Div2">
                    <div class="row">
               <asp:Label CssClass="col-sm-3 col-form-label" ID="lblRetenciones" runat="server" Text="Retenciones" />
                        <div class="col-sm-9">
                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtRetenciones" Enabled="true" Visible="true" runat="server" />
                        </div>
                    </div>
                </div>
            </div>


            <div class="form-group row">
                <div class="col-12 col-md-8 col-lg-4" runat="server" id="Div3">
                    <div class="row">
                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblSaldoTecnico" runat="server" Text="Saldo Tecnico Anterior" />
                        <div class="col-sm-9">
                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtSaldoTecnico" Enabled="false" Visible="true" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-4"></div>
                <div class="col-12 col-md-8 col-lg-4" runat="server" id="Div4">
                    <div class="row">
                        <asp:Label CssClass="col-sm-3 col-form-label" ID="lblIVAAPagar" runat="server" Text="Neto a Pagar" />
                        <div class="col-sm-9">
                            <Evol:CurrencyTextBox CssClass="form-control" ID="txtIVAAPagar" Enabled="false" Visible="true" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <br />
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <center>
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
