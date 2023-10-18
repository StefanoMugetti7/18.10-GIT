<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SegGestionarPerfilesMenuesControlesPaginas.ascx.cs" Inherits="IU.Modulos.Seguridad.SegGestionarPerfilesMenuesControlesPaginas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<div class="modalPopupMenues">
<asp:Panel ID="pnlPopUp" runat="server" GroupingText="Menues: Controles con Seguridad" Style="display: none; " CssClass="modalPopup" Width="50%" >
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <div style="float:left; width:45%">
            <asp:Label CssClass="labelEvol" ID="lblContrasenia" runat="server" Text="Controles Seguros de la Pagina"></asp:Label><br /><br />
            <asp:ListBox ID="lbxControlesPaginas" runat="server"></asp:ListBox>

        </div>
        <div style="float:left; width:10%;">
            <div class="form-group row">
                <div class="col-sm-8">
            <asp:Button CssClass="botonesEvol" ID="btnAgregarTodos" runat="server" Text=">>" 
                onclick="btnAgregarTodos_Click" /><br />
            <asp:Button CssClass="botonesEvol" ID="btnAgregarUno" runat="server" Text=">" 
                onclick="btnAgregarUno_Click" /><br />
            <asp:Button CssClass="botonesEvol" ID="btnBorrarUno" runat="server" Text="<" 
                onclick="btnBorrarUno_Click" /><br />
            <asp:Button CssClass="botonesEvol" ID="btnBorrarTodos" runat="server" Text="<<" 
                onclick="btnBorrarTodos_Click" />
                </div>
                </div>
        </div>
        <div style="float:left; width:45%">
            <asp:Label CssClass="labelEvol" ID="Label1" runat="server" Text="Controles Seguros Seleccionados"></asp:Label><br /><br />
            <asp:ListBox ID="lbxControlesPaginaSeleccionados" runat="server"></asp:ListBox>
        </div>
        <div class="borrar"></div>
        <center>
                <asp:Button CssClass="botonesEvol" ID="btnPopUpAceptar" runat="server" Text="Accept" 
                    onclick="btnPopUpAceptar_Click" ValidationGroup="UsuariosCambiarContrasenia" />
            <asp:Button CssClass="botonesEvol" ID="btnPopUpCancelar"  CausesValidation="false" 
                runat="server" Text="Cancel" />
        </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
</div>

<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" style="display:none;" runat="server" Text="Button" />
<cc1:ModalPopupExtender ID="mpePopUp" runat="server"
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp"
    BackgroundCssClass="modalBackground"
    CancelControlID="btnPopUpCancelar" >
</cc1:ModalPopupExtender>