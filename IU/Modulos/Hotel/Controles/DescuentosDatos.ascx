<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DescuentosDatos.ascx.cs" Inherits="IU.Modulos.Hotel.Controles.DescuentosDatos" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/select2/js/select2.full.min.js"></script>
<script src="<%=ResolveClientUrl("~")%>/assets/global/plugins/select2/js/i18n/es.js"></script>

<script type="text/javascript">

    $(document).ready(function () {
        SetTabIndexInput();
    });

    function SetTabIndexInput() {
        $(":input:not([type=hidden])").each(function (i) { $(this).attr('tabindex', i + 1); }); //setea el TabIndex de todos los elementos tipo Input
    }
</script>
<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblHotel" runat="server" Text="Hotel"></asp:Label>
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlHoteles" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvHoteles" ControlToValidate="ddlHoteles" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTiposDescuentos" runat="server" Text="Tipos Descuentos"></asp:Label>
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlTiposDescuentos" runat="server"></asp:DropDownList>
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTiposDescuentos" ControlToValidate="ddlTiposDescuentos" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
    <div class="col-sm-3">
        <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server"></asp:DropDownList>
    </div>
</div>
<div class="form-group row">
    <asp:Label CssClass="col-sm-1 col-form-label" ID="Porcentaje" runat="server" Text="Porcentaje" />
    <div class="col-sm-3">
        <Evol:CurrencyTextBox CssClass="form-control" Prefix="" ThousandsSeparator="" NumberOfDecimals="4" ID="txtPorcentaje" runat="server" />
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPorcentaje" ControlToValidate="txtPorcentaje" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>
    <asp:Label CssClass="col-sm-1 col-form-label" ID="lblImporte" runat="server" Text="Importe" />
    <div class="col-sm-3">
        <Evol:CurrencyTextBox CssClass="form-control" Prefix="" ThousandsSeparator="" NumberOfDecimals="2" ID="txtImporte" runat="server" />
        <asp:RequiredFieldValidator CssClass="Validador" ID="rfvImporte" ControlToValidate="txtImporte" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>
    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
</div>
<asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <center>
            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
            <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
        </center>
    </ContentTemplate>
</asp:UpdatePanel>
