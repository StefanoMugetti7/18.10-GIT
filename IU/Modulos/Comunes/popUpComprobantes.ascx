<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="popUpComprobantes.ascx.cs" Inherits="IU.Modulos.Comunes.popUpComprobantes" %>
<%@ Register Assembly="PdfViewer" Namespace="PdfViewer" TagPrefix="AUGE" %>

<asp:Panel ID="pnlPopUp" runat="server" GroupingText="EVOL - SIM" Style='display:none' CssClass="modalPopupComprobantes" >
    <AUGE:ShowPdf ID="ShowPdf1" Width="100%" Height="100%" CssClass="Comprobantes" runat="server"  />
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