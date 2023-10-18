<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="RecuperarContrasenia.aspx.cs" Inherits="IU.RecuperarContrasenia" %>
<%@ Register Assembly="GoogleReCaptcha" Namespace="GoogleReCaptcha" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
                <div class="IniciarSistema">
                     <div class="logo">
                         <asp:Image ID="imgLogo" runat="server" />
                    </div>
                    <br />
                    <h2>Recuperar usuario y/o contraseña</h2>
                    <br />
                    <asp:Label CssClass="labelEvol" ID="lblCorreoEletronico" runat="server" Text="Correo Electronico"></asp:Label>
                    <asp:TextBox CssClass="textboxEvol" ID="txtCorreoElectronico" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvUsuario" runat="server" 
                        ControlToValidate="txtCorreoElectronico" ErrorMessage="*"></asp:RequiredFieldValidator>
                    <br />
                    <%--<asp:Label CssClass="labelEvol" ID="lblCUIT" runat="server" 
                        Text="CUIT Empresa"></asp:Label>
                    <Evol:CurrencyTextBox CssClass="textboxEvol" ID="txtCUI" NumberOfDecimals="0" ThousandsSeparator="" Prefix="" runat="server" ></Evol:CurrencyTextBox>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvContrasenia" runat="server" 
                        ControlToValidate="txtCUI" ErrorMessage="*"></asp:RequiredFieldValidator>
                        <br />--%>
                        <br />
                        <center>
                            <div runat="server" id="pbTarget" visible="false"></div> 
                            <asp:Panel ID="pnlGoogleReCaptcha" runat="server">
                            </asp:Panel>
                        </center>
                      <br />
                      <center>
                            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" 
                                onclick="btnAceptar_Click" Text="Recuperar" />
                            <asp:Button CssClass="botonesEvol" ID="btnVolver" runat="server" 
                                onclick="btnVolver_Click" Text="Volver" Visible="false" CausesValidation="false" />
                    </center>  
                       
                    </div>             
        </ContentTemplate> 
    </asp:UpdatePanel>  
</asp:Content>

