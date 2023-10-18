<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Impresion.ascx.cs" Inherits="IU.Modulos.Comunes.Impresion" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Panel ID="pnlPopUp" runat="server" GroupingText="Seleccionar Impresora" Style="display:none" CssClass="modalPopup">
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <div id="seleccionarimpresora" runat="server">
        <table>
            <tr>
                <td>
                    <asp:Label CssClass="labelEvol" ID="lblImpresoras" runat="server" Text="Impresoras"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList CssClass="selectEvol" ID="ddlImpresoras" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" />
                </td>
                <td>
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div id="procesoimpresion" runat="server">
        <center>
            <asp:Timer ID="tmrImpresion" runat="server" Enabled="false" Interval="5000" 
                ontick="tmrImpresion_Tick">
            </asp:Timer>
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Imagenes/impresion.gif" />
        </center>
    </div>
    </ContentTemplate>
</asp:UpdatePanel>
</asp:Panel>

<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" style="display:none;" runat="server" Text="Button" />
<cc1:ModalPopupExtender ID="mpePopUp" runat="server"
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp" 
    BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender> 