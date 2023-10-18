<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MonedasDatos.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.MonedasDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>

<div class="MonedasDatos">
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoMoneda" runat="server" Text="Código Moneda" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtCodigoMoneda" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCodigoMoneda" runat="server" ErrorMessage="*"
                ControlToValidate="txtCodigoMoneda" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblMoneda" runat="server" Text="Moneda" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtMoneda" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvMoneda" runat="server" ErrorMessage="*"
                ControlToValidate="txtMoneda" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
        </div>
    </div>
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
