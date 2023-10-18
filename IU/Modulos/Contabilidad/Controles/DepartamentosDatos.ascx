<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DepartamentosDatos.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.DepartamentosDatos" %>

<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>

<div class="DepartamentosDatos">
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoDepartamento" runat="server" Text="Código" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtCodigoDepartamento" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCodigoCapitulo" runat="server" ErrorMessage="*"
                ControlToValidate="txtCodigoDepartamento" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDepartamento" runat="server" Text="Departamento" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtDepartamento" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDepartamento" runat="server" ErrorMessage="*"
                ControlToValidate="txtDepartamento" ValidationGroup="Aceptar" />
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
