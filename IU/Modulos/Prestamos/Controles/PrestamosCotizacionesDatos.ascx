<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrestamosCotizacionesDatos.ascx.cs" Inherits="IU.Modulos.Prestamos.Controles.PrestamosCotizacionesDatos" %>
<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>

<auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"/>

<script type="text/javascript">
</script>

<div class="PrestamosCotizacionesDatos">
    <div class="form-group row">
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblTipoUnidad" runat="server" Text="Tipo Unidad"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoUnidad" runat="server"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvTipoUnidad" ControlToValidate="ddlTipoUnidad" ValidationGroup="Aceptar" CssClass="Validador" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCoeficiente" runat="server" Text="Coeficiente" />
        <div class="col-lg-3 col-md-3 col-sm-9">
            <Evol:CurrencyTextBox CssClass="form-control" ID="txtCoeficiente" runat="server" NumberOfDecimals="6" Prefix="" AllowZero="false" />
            <asp:RequiredFieldValidator ID="rfvCoeficiente" ControlToValidate="txtCoeficiente" ValidationGroup="Aceptar" CssClass="Validador" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFechaDesdeAplica" runat="server" Text="Fecha Desde que Aplica"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control datepicker" ID="txtFechaDesdeAplica" runat="server"/>
            <asp:RequiredFieldValidator ID="rfvFechaDesdeAplica" ControlToValidate="txtFechaDesdeAplica" ValidationGroup="Aceptar" CssClass="Validador" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
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




