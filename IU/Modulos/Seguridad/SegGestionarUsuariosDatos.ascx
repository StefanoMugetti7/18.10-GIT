<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SegGestionarUsuariosDatos.ascx.cs" Inherits="IU.Modulos.Seguridad.SegGestionarUsuariosDatos" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="Auditoria" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Seguridad/Controles/SegUsuariosSectoresDatos.ascx" TagName="UsuariosSectores" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>




<style type="text/css">
    .VeryPoor {
        background: Red;
        color: White;
        font-weight: bold;
    }

    .Average {
        background: orange;
        color: black;
        font-weight: bold;
    }

    .Good {
        background: Yellow;
        color: White;
        font-weight: bold;
    }

    .Excellent {
        background: LimeGreen;
        color: White;
        font-weight: bold;
    }

    .border {
        border-style: solid;
        border-width: 0.5px;
        width: 210px;
        padding: 0px 0px 0px 0px;
    }
</style>


   <script lang="javascript" type="text/javascript">
 function checkAllRow(objRef) {
     $('input:checkbox[id*="chkFiliales"]').each(function () {
            $(this).prop('checked', objRef.checked);
        });
       }
       this.CargarComboFiliales();
   </script>


<div class="Usuarios Datos">

    <div class="form-group row">
        <asp:Label CssClass="col-form-label col-sm-1" ID="lblUsuario" runat="server" Text="Usuario"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtUsuario" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvUsuario" runat="server" ControlToValidate="txtUsuario" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>


        <asp:Label CssClass="col-form-label col-sm-1" ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" runat="server" ID="txtNombre"></asp:TextBox>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNombre" runat="server" ControlToValidate="txtNombre" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>
        <asp:Label CssClass="col-form-label col-sm-1" ID="lblApellido" runat="server" Text="Apellido"></asp:Label>
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtApellido" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvApellido" runat="server" ControlToValidate="txtApellido" ErrorMessage="*"></asp:RequiredFieldValidator>
        </div>

    </div>
    <div class="form-group row">
        <div id="pnlContrasenia" class="col-sm-4" runat="server">
             <div class="form-group row">
            <asp:Label CssClass="col-form-label col-sm-3" ID="lblContrasenia" runat="server" Text="Contrasenia"></asp:Label>
            <div class="col-sm-9">
                <asp:TextBox CssClass="form-control" ID="txtContrasenia" TextMode="Password" runat="server"></asp:TextBox>

                <Ajax:PasswordStrength ID="PasswordStrength1" runat="server" DisplayPosition="RightSide" StrengthIndicatorType="BarIndicator"
                    TargetControlID="txtContrasenia" PrefixText="Stength:" Enabled="true"
                    RequiresUpperAndLowerCaseCharacters="true" MinimumLowerCaseCharacters="0"
                    MinimumUpperCaseCharacters="0" MinimumSymbolCharacters="0"
                    MinimumNumericCharacters="0" PreferredPasswordLength="10"
                    TextStrengthDescriptions="VeryPoor; Average; Good; Excellent"
                    StrengthStyles="VeryPoor; Average; Good ;Excellent "
                    CalculationWeightings="35;25;15;25" BarBorderCssClass="border"
                    HelpStatusLabelID="Label1"></Ajax:PasswordStrength>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvContrasenia" runat="server" ControlToValidate="txtContrasenia" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div></div>
        </div>

        <asp:Label CssClass="col-form-label col-sm-1" ID="lblCorreo" runat="server" Text="Email"></asp:Label><div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtCorreoElectronico" runat="server"></asp:TextBox>
        </div>
        <%--<asp:RequiredFieldValidator CssClass="Validador" ID="rfvCorreo" runat="server" 
                    ControlToValidate="txtCorreoElectronico" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
    </div>
    <div class="form-group row">
        <div class="col-sm-1"></div>
        <div class="col-sm-3">
            <asp:CheckBox ID="chkCambiarContrasenia" CssClass="form-control" runat="server" Text="Cambiar Contrasenia en el Próximo Login" />
        </div>
        <div class="col-sm-1"></div>
        <div class="col-sm-3">
            <asp:CheckBox ID="chkBajaLogica" CssClass="form-control" runat="server" Text="Inhabilitar Usuario" />
        </div>
    </div>
    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
    <%--<asp:Label CssClass="form-control col-sm-1" ID="lblFilialPredeterminada" runat="server" Text="Filial Predeterminada"></asp:Label>
                    <asp:DropDownList CssClass="selectEvol" ID="ddlIdFilial" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlIdFilial_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFilial" runat="server" 
                    ControlToValidate="ddlIdFilial" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
    <%--<br />--%>

    <%--<asp:UpdatePanel ID="upSectores" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>--%>
    <%--<asp:Label CssClass="form-control col-sm-1" ID="lblSectorPredeterminado" runat="server" Text="Sector Predeterminada"></asp:Label>
                    <asp:DropDownList CssClass="selectEvol" ID="ddlIdSector" runat="server">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvSector" runat="server" 
                            ControlToValidate="ddlIdSector" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
    <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
    <asp:UpdatePanel ID="upSectores" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:TabContainer ID="tcUsuarios" runat="server" ActiveTabIndex="0"
                Width="100%" SkinID="MyTab" AutoPostBack="true"
                OnActiveTabChanged="tcUsuarios_ActiveTabChanged">
                <asp:TabPanel runat="server" ID="tpPerfiles" HeaderText="Perfiles">
                    <ContentTemplate>
                        <asp:CheckBoxList ID="chkPerfiles" RepeatColumns="4" RepeatDirection="Vertical" runat="server">
                        </asp:CheckBoxList>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tpBodegas" HeaderText="Seleccione Filiales">
                    <ContentTemplate>
                        <asp:CheckBox ID="chkTodos" Text="Todos" TextAlign="Right" runat="server" onclick="checkAllRow(this)" />
                        <asp:CheckBoxList ID="chkFiliales" RepeatColumns="4" RepeatDirection="Vertical" runat="server"
                            AutoPostBack="true" OnSelectedIndexChanged="chkFiliales_SelectedIndexChanged">
                        </asp:CheckBoxList>
                        <div class="form-group row">
                        <asp:Label CssClass="col-form-label col-sm-1" ID="lblFilialPredeterminada" runat="server" Text="Filial Predeterminada"></asp:Label>
                        <div class="col-sm-3">     <asp:DropDownList CssClass="form-control select2" ID="ddlIdFilial" runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlIdFilial_SelectedIndexChanged">
                        </asp:DropDownList>
                            </div>  </div>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tpSectores" HeaderText="Seleccione Sectores">
                    <ContentTemplate>
                        <AUGE:UsuariosSectores ID="ctrUsuariosSectores" runat="server" />
                          <div class="form-group row">
                        <asp:Label CssClass="col-form-label col-sm-1" ID="lblSectorPredeterminado" runat="server" Text="Sector Predeterminada"></asp:Label>
                       <div class="col-sm-3">    <asp:DropDownList CssClass="form-control select2" ID="ddlIdSector" runat="server"></asp:DropDownList>
</div></div>
                    </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel runat="server" ID="tpAuditoria" HeaderText="Historico">
                    <ContentTemplate>
                        <AUGE:Auditoria ID="ctrlAuditoria" runat="server" />
                    </ContentTemplate>
                </asp:TabPanel>
            </asp:TabContainer>
            <asp:Label CssClass="form-control col-sm-1" ID="lblValidarFilial" runat="server" Visible="false" Width="100%" ForeColor="Red" Text="* Debe seleccionar al menos una Filial en la solapa Filiales"></asp:Label>
            <asp:Label CssClass="form-control col-sm-1" ID="lblValidarSectores" runat="server" Visible="false" Width="100%" ForeColor="Red" Text="* Debe seleccionar al menos un Sector en la solapa Sectores"></asp:Label>

        </ContentTemplate>
    </asp:UpdatePanel>

    <center>    
<asp:UpdatePanel ID="upBotones" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>  
    <asp:ImageButton ImageUrl="~/Imagenes/sendmail.png" runat="server" ID="btnEnviarMail" Visible="false"
                          onclick="btnEnviarMail_Click"  AlternateText="Enviar Mail Alta de Usuario" ToolTip="Enviar Mail Alta de Usuario" />
                          &nbsp;
    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" onclick="btnAceptar_Click" 
                        Text="Aceptar" />
    
    &nbsp;<asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" onclick="btnCancelar_Click" 
        Text="Volver" CausesValidation="false"  />
    </ContentTemplate>
</asp:UpdatePanel>
</center>

</div>
