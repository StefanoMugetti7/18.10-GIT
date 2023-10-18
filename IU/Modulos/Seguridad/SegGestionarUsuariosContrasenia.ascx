<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SegGestionarUsuariosContrasenia.ascx.cs" Inherits="IU.Modulos.Seguridad.SegGestionarUsuariosContrasenia" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Panel ID="pnlPopUp" runat="server" GroupingText="Cambiar contraseña" Style="display: none;" CssClass="modalPopup" Width="40%">
    <div class="Contraseña PopUp">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-2 col-form-label" ID="lblContrasenia" runat="server" Text="Contraseña: "></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtContrasenia" TextMode="Password" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="Validador" ValidationGroup="UsuariosCambiarContrasenia" ID="rfvContrasenia" ControlToValidate="txtContrasenia" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group row">
                    <asp:Label CssClass="col-sm-2 col-form-label" ID="lblContraseniaVerificar" runat="server" Text="Verificar Contraseña:"></asp:Label>
                    <div class="col-sm-3">
                        <asp:TextBox CssClass="form-control" ID="txtContraseniaVerificar" TextMode="Password" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="Validador" ValidationGroup="UsuariosCambiarContrasenia" ID="rfvContraseniaVerificar" ControlToValidate="txtContraseniaVerificar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvContraseniaVerificar" ControlToValidate="txtContrasenia" ControlToCompare="txtContraseniaVerificar" runat="server" ErrorMessage="*"></asp:CompareValidator>
                    </div>
                </div>
                <%--colspan="2" align="center">--%>
                <center>
                    <asp:Button CssClass="botonesEvol" ID="btnPopUpAceptar" runat="server" Text="Aceptar"
                        onclick="btnPopUpAceptar_Click" ValidationGroup="UsuariosCambiarContrasenia" />
                    <asp:Button CssClass="botonesEvol" ID="btnPopUpCancelar" CausesValidation="false"
                        runat="server" Text="Volver" onclick="btnPopUpCancelar_Click" />
                </center>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Panel>
<asp:Button CssClass="botonesEvol" ID="MeNecesitaElExtender" Style="display: none;" runat="server" Text="Button" />
<cc1:ModalPopupExtender ID="mpePopUp" runat="server"
    TargetControlID="MeNecesitaElExtender"
    PopupControlID="pnlPopUp"
    BackgroundCssClass="modalBackground">
</cc1:ModalPopupExtender>
