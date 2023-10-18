<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormasCobrosAfiliadosDatos.ascx.cs" Inherits="IU.Modulos.TGE.Control.FormasCobrosAfiliadosDatos" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>

<div class="FormasCobrosAfiliadosDatos">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-group row">
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblFormaCobro" runat="server" Text="Formas de Cobro"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlFormasCobros" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlFormasCobros_SelectedIndexChanged"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvFormasCobros" CssClass="Validador" ControlToValidate="ddlFormasCobros" runat="server" ValidationGroup="Aceptar" ErrorMessage="*"></asp:RequiredFieldValidator>
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPredeterminado" runat="server" Text="Predeterminado"></asp:Label>
                <div class="col-sm-3">
                    <asp:CheckBox ID="chkPredeterminado" runat="server" CssClass="form-control" />
                </div>
                <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado"></asp:Label>
                <div class="col-sm-3">
                    <asp:DropDownList CssClass="form-control select2" ID="ddlEstados" runat="server"></asp:DropDownList>
                </div>
            </div>
            <asp:Panel ID="pnlCamposValores" runat="server">
                <auge:CamposValores ID="ctrCamposValores" runat="server" />
            </asp:Panel>
            <center>
                <auge:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
