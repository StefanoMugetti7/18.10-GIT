<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PlazosFijosPropiosDatos.ascx.cs" Inherits="IU.Modulos.Bancos.Controles.PlazosFijosPropiosDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpGrillaGenerica.ascx" TagName="popUpGrillaGenerica" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<script type="text/javascript">
    $(document).ready(function () {
        SetTabIndexInput();
    });
    function SetTabIndexInput() {
        $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }
    function CalcularTotal() {
        var total = 0.00;
        var importe = $("input[type=text][id*='txtImporte']").maskMoney('unmasked')[0];;
        var interes = $("input[type=text][id*='txtImporteInteres']").maskMoney('unmasked')[0];;
        total = parseFloat(parseFloat(importe) + parseFloat(interes)).toFixed(2);
        $("input[type=text][id*='txtImporteTotal']").val(accounting.formatMoney(total, gblSimbolo, gblCantidadDecimales, "."));
    }
</script>
<div class="PlazosFijosDatos">
    <asp:UpdatePanel ID="upSaldoActual" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCuenta" runat="server" Text="Cuenta" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlCuenta" runat="server" OnSelectedIndexChanged="ddlCuenta_SelectedIndexChanged" AutoPostBack="true" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCuenta" runat="server" InitialValue="" ControlToValidate="ddlCuenta"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSaldoActual" runat="server" Text="Saldo Actual" />
                <div class="col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtSaldoActual" runat="server" />

                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoRenovacion" runat="server" Text="Tipo Renovación" Visible="false" />
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlTipoRenovacion" runat="server" Visible="false" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoRenovacion" runat="server" ControlToValidate="ddlTipoRenovacion"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upTasaInteres" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaInicioVigencia" runat="server" Text="Fecha Inicio Vigencia" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaInicioVigencia" runat="server" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaInicioVigencia" runat="server" ControlToValidate="txtFechaInicioVigencia"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaVencimiento" runat="server" Text="Fecha Vencimiento" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control datepicker" ID="txtFechaVencimiento" Enabled="false" runat="server" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFechaVencimiento" runat="server" ControlToValidate="txtFechaVencimiento"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporte" runat="server" Text="Importe" />
                <div class="col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporte" runat="server" />
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporte" runat="server" ControlToValidate="txtImporte"
                        ErrorMessage="*" ValidationGroup="Aceptar" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteInteres" runat="server" Text="Estimulo" />
                <div class="col-sm-3">
                    <Evol:CurrencyTextBox CssClass="form-control" ID="txtImporteInteres" runat="server" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporteTotal" runat="server" Text="Total" />
                <div class="col-sm-3">
                    <asp:TextBox CssClass="form-control" ID="txtImporteTotal" Style="text-align: right;" Enabled="false" runat="server" />
                </div>
            </div>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion"></asp:Label>
                <div class="col-sm-7">
                    <asp:TextBox CssClass="form-control" Rows="2" ID="txtDescripcion" Enabled="true" runat="server" Placeholder="Descripción" TextMode="MultiLine" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpArchivos" HeaderText="Archivos">
            <ContentTemplate>
                <AUGE:Archivos ID="ctrArchivos" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="row justify-content-md-center">
                <div class="col-md-auto">
                    <AUGE:popUpGrillaGenerica ID="ctrPopUpGrilla" runat="server" />
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
