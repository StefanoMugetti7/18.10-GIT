<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TiposOperacionesComprobantesDatos.ascx.cs" Inherits="IU.Modulos.TGE.Control.TiposOperacionesComprobantesDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<div class="col-sm-12">
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblOperaciones" runat="server" Text="Tipo Operación"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoOperacionOC" runat="server">
            </asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvOperaciones" runat="server" ControlToValidate="ddlTipoOperacionOC"
                ErrorMessage="*" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFacturas" runat="server" Text="Tipo Factura"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoFactura" runat="server">
            </asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFacturas" runat="server" ControlToValidate="ddlTipoFactura"
                ErrorMessage="*" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstados" runat="server" Text="Estado"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server">
            </asp:DropDownList>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvEstados" runat="server" ControlToValidate="ddlEstado"
                ErrorMessage="*" ValidationGroup="Aceptar" />
        </div>
    </div>
</div>
<div class="col-sm-12">
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="Label1" runat="server" Text="Signo"></asp:Label>
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlSigno" runat="server">
                <asp:ListItem Value="1" Text="Positivo" Selected="True"></asp:ListItem>
                <asp:ListItem Value="-1" Text="Negativo"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="MostrarIVA" runat="server" Text="MostrarIVA"></asp:Label>
        <div class="col-sm-3">
            <asp:CheckBox ID="chkMostrarIVA" CssClass="form-control" runat="server" />
        </div>
    </div>
    <asp:UpdatePanel ID="upBotones" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="row justify-content-md-center">
                <div class="col-md-auto">
                    <AUGE:popupmensajespostback id="popUpMensajes" runat="server" />
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"
                        OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"
                        OnClick="btnCancelar_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
