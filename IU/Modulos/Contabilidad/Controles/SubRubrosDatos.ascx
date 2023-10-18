<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SubRubrosDatos.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.SubRubrosDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>

<div class="SubRubrosDatos">
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoSubRubro" runat="server" Text="Código SubRubro" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtCodigoSubRubro" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCodigoSubRubro" runat="server" ErrorMessage="*"
                ControlToValidate="txtCodigoSubRubro" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblSubRubro" runat="server" Text="SubRubro" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtSubRubro" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvSubRubro" runat="server" ErrorMessage="*"
                ControlToValidate="txtSubRubro" ValidationGroup="Aceptar" />
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
