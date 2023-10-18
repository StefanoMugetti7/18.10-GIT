<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="IngresoSistema.aspx.cs" Inherits="IU.IngresoSistema" %>
<%--<%@ Register TagPrefix="recaptcha" Namespace="Recaptcha" Assembly="Recaptcha" %>--%>
<%@ Register Assembly="GoogleReCaptcha" Namespace="GoogleReCaptcha" TagPrefix="cc1" %>
<%--<%@ Register Assembly="EO.Web" Namespace="EO.Web" TagPrefix="eo" %>--%>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script lang="javascript" type="text/javascript">

    $(document).ready(function () {
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(RecargarPassword);
    });
        function RecargarPassword() {
            $("input[id$='txtContrasenia']").password();
            //$('#password').password();
        }
        
        </script> 
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="form-group row justify-content-center">
            <div class="col-12 col-md-8 col-lg-6">
                <div class="card">
              <div class="card-header text-center">
               EVOL S.R.L. Sistemas Informaticos
              </div>
              <div class="card-body">
                  <div class="form-group row justify-content-center">
                     <div class="text-center">
                         <asp:Image ID="imgLogo" class="rounded" runat="server" />
                    </div>
                </div>
                   <div class="form-group row">
                       <div class="col">&nbsp;</div>
                   </div>
                  <div ID="dvLogin" runat="server" visible="true">
                  <div class="form-group row justify-content-center">
                    <asp:Label CssClass="col-4 col-form-label text-sm-right" ID="lblUsuario" runat="server" Text="Usuario"></asp:Label>
                      <div class="col-7">
                    <asp:TextBox CssClass="form-control" ID="txtUsuario" runat="server"></asp:TextBox>
                          <asp:RequiredFieldValidator CssClass="Validador" ID="rfvUsuario" runat="server" 
                        ControlToValidate="txtUsuario" ErrorMessage=""></asp:RequiredFieldValidator>
                          </div>
                  </div>
                  <div class="form-group row justify-content-center">
                    <asp:Label CssClass="col-4 col-form-label text-sm-right" ID="lblContrasenia" runat="server" 
                        Text="Contraseña"></asp:Label>
                    <div class="col-7">
                    <asp:TextBox CssClass="form-control"  ID="txtContrasenia" runat="server" TextMode="Password" data-toggle="password"  
                        ></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvContrasenia" runat="server" 
                        ControlToValidate="txtContrasenia" ErrorMessage=""></asp:RequiredFieldValidator>
                          </div>
                    </div>
                  <div class="form-group row justify-content-center">
                    <asp:Label CssClass="col-4 col-form-label text-sm-right" ID="lblTipoAutenticacion" runat="server" 
                        Text="Tipo de Autenticación"></asp:Label>
                    <div class="col-7">
                    <asp:DropDownList ID="ddlTipoAutenticacion" CssClass="form-control select2" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoAutenticacion_SelectedIndexChanged" runat="server"></asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvTipoAutenticacion" runat="server" 
                        ControlToValidate="ddlTipoAutenticacion" ErrorMessage=""></asp:RequiredFieldValidator>
                          </div>
                    </div>
                    <div class="form-group row justify-content-center" runat="server" id="dvGoogleReCaptcha" visible="false">
                            <div runat="server" id="pbTarget" visible="false"></div> 
                            <asp:Panel ID="pnlGoogleReCaptcha" runat="server">
                           </asp:Panel>
                    </div>
                     <div class="form-group row justify-content-center">
                            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" 
                                onclick="btnAceptar_Click" Text="Aceptar" />
                    </div>
                  
                   <div class="form-group row">
                       <div class="col">&nbsp;</div>
                   </div>
                    <div class="form-group row justify-content-center">
                          <asp:LinkButton ID="lnkRecuperarUsuarioContrasenia" CausesValidation="false" PostBackUrl="~/RecuperarContrasenia.aspx" runat="server">Olvide mi usuario o contraseña</asp:LinkButton>
                    </div>
                      
                    </div>  
                  <div runat="server" id="dvGoogleAuthenticator" visible="false">
                  <div class="form-group row justify-content-center" >
                         <asp:Label CssClass="col-4 col-form-label text-sm-right" ID="lblCode" runat="server" Text="Código de Verificación"></asp:Label>
                          <div class="col-7">
                        <asp:TextBox CssClass="form-control" ID="txtCodigo" runat="server"></asp:TextBox>
                              <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCodigo" runat="server" 
                            ControlToValidate="txtCodigo" ValidationGroup="vgCodigo" ErrorMessage=""></asp:RequiredFieldValidator>
                              </div>

                    </div>
                      <div class="form-group row">
                          <div class="col-4"></div>
                        <div class="alert alert-danger col-7" id="dvIncorrecto" visible="false" runat="server" role="alert">  
                            ¡Código incorrecto!</div>
                        </div>
                  <div class="form-group row justify-content-center">
                            <asp:Button CssClass="botonesEvol" ID="btnValidar" runat="server" 
                                onclick="btnValidar_Click" ValidationGroup="vgCodigo" Text="Validar" />
                        &nbsp;
                            <asp:Button CssClass="botonesEvol" ID="btnVolver" runat="server" 
                                onclick="btnVolver_Click" Text="Volver" />
                    </div>
                      </div>
                  </div>
            </div>
            </div>
            </div>
        </ContentTemplate> 
    </asp:UpdatePanel>  
</asp:Content>
