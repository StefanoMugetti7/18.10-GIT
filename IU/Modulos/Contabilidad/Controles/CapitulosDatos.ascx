<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CapitulosDatos.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.CapitulosDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>

<div class="CapitulosDatos">
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCapitulo" runat="server" Text="Capítulo" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtCapitulo" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCapitulo" runat="server" ErrorMessage="*"
                ControlToValidate="txtCapitulo" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoCapitulo" runat="server" Text="Código Capítulo" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtCodigoCapitulo" runat="server" MaxLength="1" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCodigoCapitulo" runat="server" ErrorMessage="*"
                ControlToValidate="txtCodigoCapitulo" ValidationGroup="Aceptar" />
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
