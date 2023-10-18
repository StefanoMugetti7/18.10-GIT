<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="SegGoogleAuthenticator.aspx.cs" Inherits="IU.Modulos.Seguridad.SegGoogleAuthenticator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card">
        <div class="card-header">
            Configurar Autenticación de Google en 2 pasos
        </div>
        <div class="card-body">
            <div>
                <p>Para usar una aplicación de autenticación, siga los siguientes pasos::</p>
                <ol class="list">
                    <li>
                        <p>
                            Descargue una aplicación de autenticación de dos factores como
                Google Authenticator para  
                <a target="_blank" href="https://play.google.com/store/apps/details?id=com.google.android.apps.authenticator2&hl=en">Android</a> y  
                <a target="_blank" href="https://itunes.apple.com/us/app/google-authenticator/id388497605?mt=8">iOS</a>
                <%--Microsoft Authenticator para
                <a target="_blank" href="https://go.microsoft.com/fwlink/?Linkid=825071">Windows Phone</a>,  
                <a target="_blank" href="https://go.microsoft.com/fwlink/?Linkid=825072">Android</a> y
                <a target="_blank" href="https://go.microsoft.com/fwlink/?Linkid=825073">iOS</a>--%>
                        </p>
                    </li>
                    <li>
                        <p>Escanee el código QR o ingrese la clave en su aplicación de autenticación de dos factores.</p>
                        <asp:Image ID="imgQrCode" runat="server" />
                        <p></p>
                        <p>Clave secreta: <asp:Label runat="server" ID="lblManualSetupCode"></asp:Label></p>
                    </li>
                    <li>
                        <p>
                            Una vez que haya escaneado el código QR o ingresado la clave anterior, su aplicación de autenticación de dos factores le proporcionará
                con un código único. Introduzca el código en el cuadro de confirmación a continuación.
                        </p>
                        <asp:UpdatePanel ID="upVerificar" runat="server">
                        <ContentTemplate>
                        <div class="row">
                            <asp:Label CssClass="col-sm-3 col-md-6 col-lg-12 col-form-label" ID="lblCodigoVerificacion" runat="server" Text="Código de verificación"></asp:Label>
                            <div class="col-sm-12 col-md-6 col-lg-3">
                                <asp:TextBox CssClass="form-control" ID="txtCodigoVerificacion" runat="server" />
                                <asp:Button CssClass="botonesEvol" ID="btnVerificar" runat="server" Text="Verificar" onclick="btnVerificar_Click" ValidationGroup="Verificar" />
                            </div>
                            <div class="alert alert-success col-sm-12 col-md-6 col-lg-3" id="dvResultado" visible="false" runat="server" role="alert">  
                            </div>
                            <div class="alert alert-danger col-sm-12 col-md-6 col-lg-3" id="dvIncorrecto" visible="false" runat="server" role="alert">  
                            </div>
                        </div>
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    </li>
                </ol>
            </div>
        </div>
    </div>
</asp:Content>
