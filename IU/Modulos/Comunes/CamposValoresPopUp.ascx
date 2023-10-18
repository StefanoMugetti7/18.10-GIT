<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CamposValoresPopUp.ascx.cs" Inherits="IU.Modulos.Comunes.CamposValoresPopUp" %>

<asp:Panel ID="pnlPopUp" runat="server" GroupingText="Sistema de Gestion para Mutuales" Style='display:none; width:auto;' CssClass="modalPopup" >
    <asp:Panel ID="pnlControlesDinamicos" runat="server">
    <asp:Label CssClass="labelEvol" ID="lblTitulo" runat="server" Text=""></asp:Label>
    </asp:Panel>
    <br />
    <center>
        <asp:Button CssClass="botonesEvol" ID="btnAceptar" OnClick="btnAceptar_Click" runat="server" Text="Aceptar" />
        <asp:Button CssClass="botonesEvol" ID="btnCancelar" Text="Volver" runat="server" />
    </center>
</asp:Panel>
<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" Style="display: none;" runat="server" Text="Button" />
<asp:ModalPopupExtender 
    ID="mpePopUp" runat="server" 
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp"
    BackgroundCssClass="modalBackground" 
    BehaviorID="bhidMpePopUpCamposValores"
    CancelControlID="btnCancelar"
    >
</asp:ModalPopupExtender>