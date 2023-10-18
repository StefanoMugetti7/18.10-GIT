<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TiposCambiosDatosPopUp.ascx.cs" Inherits="IU.Modulos.TGE.Control.TiposCambiosDatosPopUp" %>

<asp:Panel ID="pnlPopUp" runat="server" GroupingText="Exchange" Style="display:none" CssClass="modalPopUpDomicilios">

<div class="TiposCambiosDatosPopUp">
    <asp:Label CssClass="labelEvol" ID="lblPais" runat="server" Text="County"></asp:Label>
    <asp:DropDownList CssClass="selectEvol" ID="ddlPaises" runat="server">
    </asp:DropDownList>
    <br />
    <asp:Label CssClass="labelEvol" ID="lblMoneda" runat="server" Text="Currency"></asp:Label>
    <asp:DropDownList CssClass="selectEvol" ID="ddlMonedas" runat="server">
    </asp:DropDownList>
    <br />
    <asp:Label CssClass="labelEvol" ID="lblFechaDesde" runat="server" Text="Start Date"></asp:Label>
    <asp:TextBox CssClass="textboxEvol" ID="txtFechaDesde" runat="server"></asp:TextBox>
    <br />
    <asp:Label CssClass="labelEvol" ID="lblTipoCambio" runat="server" Text="Exchange"></asp:Label>
    <AUGE:CurrencyTextBox CssClass="textboxEvol" ID="txtTipoCambio" NumberOfDecimals="2" runat="server"></AUGE:CurrencyTextBox>
    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoCambio" runat="server" Display="Dynamic" ValidationGroup="Aceptar" ControlToValidate="txtTipoCambio" ErrorMessage="*"></asp:RequiredFieldValidator>

        <br />
        <center>
        <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" 
            onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
        <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" 
            onclick="btnCancelar_Click" />
        </center>
    </div>
    
</asp:Panel>

<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" style="display:none;" runat="server" Text="Button" />
<asp:ModalPopupExtender ID="mpePopUp" runat="server"
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp" 
    BackgroundCssClass="modalBackground"
    CancelControlID="btnCancelar"
    >
</asp:ModalPopupExtender>