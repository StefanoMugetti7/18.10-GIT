<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TiposOperacionesDatos.ascx.cs" Inherits="IU.Modulos.TGE.Control.TiposOperacionesDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>

<div class="TiposOperacionesDatos">
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoOperacion" runat="server" Text="Tipo Operacion" />
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:TextBox CssClass="form-control" ID="txtTipoOperacion" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoOperacion" runat="server" ControlToValidate="txtTipoOperacion"
                ErrorMessage="*" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoMovimiento" runat="server" Text="Tipo Movimiento" />
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoMovimiento" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoMovimiento" runat="server" ControlToValidate="ddlTipoMovimiento"
                ErrorMessage="*" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvEstados" runat="server" ControlToValidate="ddlEstados"
                ErrorMessage="*" ValidationGroup="Aceptar" />
        </div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigoOperacion" runat="server" Text="Código Operacion" />
        <div class="col-lg-3 col-md-3 col-sm-9">
            <AUGE:NumericTextBox CssClass="form-control" ID="txtCodigoOperacion" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblModulosSistema" runat="server" Text="Módulo Sistema" />
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:DropDownList CssClass="form-control select2" ID="ddlModulosSistema" runat="server" />
        </div>
        <asp:CheckBox ID="chkContabiliza" runat="server" Text="Contabiliza" />
        <div class="Espacio"></div>
        <asp:CheckBox ID="chkLibroIVA" runat="server" Text="Libro IVA" />
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