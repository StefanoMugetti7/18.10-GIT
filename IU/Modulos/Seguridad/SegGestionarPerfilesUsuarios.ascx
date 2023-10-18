<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SegGestionarPerfilesUsuarios.ascx.cs" Inherits="IU.Modulos.Seguridad.SegGestionarPerfilesUsuarios" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Panel ID="pnlPopUp" runat="server" GroupingText="Usuarios" Style="display: none; " CssClass="modalPopup" Width="50%" >
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
         <table width="100%">
                <tr>
                    <td valign="top" align="center">
                        <asp:ListBox ID="lbxUsuarios" runat="server" Width="50%" Height="250px"></asp:ListBox>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                         <asp:Button CssClass="botonesEvol" ID="btnPopUpAceptar" runat="server" Text="Volver" 
                             />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>

<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" style="display:none;" runat="server" Text="Button" />
<cc1:ModalPopupExtender ID="mpePopUp" runat="server"
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp"
    BackgroundCssClass="modalBackground"
    OkControlID="btnPopUpAceptar" >
</cc1:ModalPopupExtender>