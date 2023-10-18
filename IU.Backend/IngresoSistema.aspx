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
            <div class="col-12 col-md-8 col-lg-4">
                <div class="card">
              <div class="card-header text-center">
               EVOL S.R.L. Backend
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
                  <div id="dvLogin" runat="server">
                  <div class="form-group row justify-content-center">
                    <asp:Label CssClass="col-3 col-form-label text-sm-right" ID="lblUsuario" runat="server" Text="Usuario"></asp:Label>
                      <div class="col-9">
                    <asp:TextBox CssClass="form-control" ID="txtUsuario" runat="server"></asp:TextBox>
                          <asp:RequiredFieldValidator CssClass="Validador" ID="rfvUsuario" runat="server" 
                        ControlToValidate="txtUsuario" ErrorMessage="" ValidationGroup="Aceptar"></asp:RequiredFieldValidator>
                          </div>
                  </div>
                  <div class="form-group row justify-content-center">
                    <asp:Label CssClass="col-3 col-form-label text-sm-right" ID="lblContrasenia" runat="server" 
                        Text="Contraseña"></asp:Label>
                    <div class="col-9">
                    <asp:TextBox CssClass="form-control"  ID="txtContrasenia" runat="server" TextMode="Password" data-toggle="password"  
                        ></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvContrasenia" runat="server" 
                        ControlToValidate="txtContrasenia" ErrorMessage="" ValidationGroup="Aceptar"></asp:RequiredFieldValidator>
                          </div>
                    </div>
                      </div>
                   <div id="dvCodigo" runat="server" visible="false">
                      <div class="form-group row justify-content-center">
                    <asp:Label CssClass="col-3 col-form-label text-sm-right" ID="Label1" runat="server" Text="Codigo"></asp:Label>
                      <div class="col-9">
                    <asp:TextBox CssClass="form-control" ID="txtCodigo" runat="server"></asp:TextBox>
                          <asp:RequiredFieldValidator CssClass="Validador" ID="RequiredFieldValidator1" runat="server" 
                        ControlToValidate="txtCodigo" ErrorMessage="" ValidationGroup="Codigo"></asp:RequiredFieldValidator>
                          </div>
                  </div>
                    </div>
                  <div class="form-group row justify-content-center">
                            <div runat="server" id="pbTarget" visible="false"></div> 
                            <asp:Panel ID="pnlGoogleReCaptcha" runat="server">
                           </asp:Panel>
                    </div>
                     <div class="form-group row justify-content-center">
                            <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" 
                                onclick="btnAceptar_Click" Text="Aceptar"  ValidationGroup="Aceptar"/>
                         <asp:Button CssClass="botonesEvol" ID="btnCodigo" runat="server" visible="false"
                                onclick="btnCodigo_Click" Text="Ingresar Codigo" ValidationGroup="Codigo" />
                           &nbsp;&nbsp;&nbsp;
                           <asp:Button CssClass="botonesEvol" ID="btnVolver" runat="server" visible="false"
                                onclick="btnVolver_Click" Text="Volver" CausesValidation="false"/>
                    </div>
                       <div class="form-group row">
                       <div class="col">&nbsp;</div>
                   </div>
                    <div class="form-group row justify-content-center" id="dvRecuperarContraseña" runat="server">
                          <asp:LinkButton ID="lnkRecuperarUsuarioContrasenia" CausesValidation="false" PostBackUrl="~/RecuperarContrasenia.aspx" runat="server">Olvide mi usuario o contraseña</asp:LinkButton>
                    </div>
                
                  
                      
                    </div>    
            </div>
            </div>
            </div>
        </ContentTemplate> 
    </asp:UpdatePanel>  
</asp:Content>
