<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ControlSesion.aspx.cs" Inherits="IU.ControlSesion"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:Panel ID="pnlPopUp" GroupingText="Helmerich Payne" runat="server" Style='display:none; width:auto;' CssClass="modalPopup" >
    <br />
    <asp:Label CssClass="labelEvol" ID="lblMensaje" runat="server" Text="   La sesión ha expirado.   "></asp:Label>
    <br />
    <br />
    <center>
        <asp:Button CssClass="botonesEvol" ID="btnAceptarPnlMensajes" runat="server" Text="Accept" onclick="btnAceptarPnlMensajes_Click"/>
    </center>
</asp:Panel>

<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" style="display:none;" runat="server" Text="Button" />
<asp:ModalPopupExtender ID="mpePopUp" runat="server"
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp" 
    BackgroundCssClass="modalBackground"
    DropShadow="true" >
</asp:ModalPopupExtender>

</asp:Content>
