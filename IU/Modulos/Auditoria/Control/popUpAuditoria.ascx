<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="popUpAuditoria.ascx.cs" Inherits="IU.Modulos.Auditoria.Control.popUpAuditoria" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagPrefix="AUGE" TagName="Auditoria" %>

<asp:Panel ID="pnlPopUp" runat="server" GroupingText="Sistema de Gestion para Mutuales" Style='display:none' CssClass="modalPopupComprobantes" >
    <AUGE:Auditoria ID="ctrAuditoria" runat="server" />
    <br />
    <center>
        <asp:Button CssClass="botonesEvol" ID="btnVolver" CausesValidation="false" runat="server" Text="Volver" />
    </center>
</asp:Panel>
<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" Style="display: none;" runat="server" Text="Button" />
<asp:ModalPopupExtender 
    ID="mpePopUp" runat="server" 
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp"
    BackgroundCssClass="modalBackground" 
    CancelControlID="btnVolver"
    >
</asp:ModalPopupExtender>