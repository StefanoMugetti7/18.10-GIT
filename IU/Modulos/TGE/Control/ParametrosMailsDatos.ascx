<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ParametrosMailsDatos.ascx.cs" Inherits="IU.Modulos.TGE.Control.ParametrosMailsDatos" %>
<%@ Register src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" tagname="AuditoriaDatos" tagprefix="auge" %>
<%--<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>--%>

 
<asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
    <ContentTemplate>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblDireccionCorreo" runat="server" Text="Direccion de Correo"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtDireccionCorreo" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDireccionCorreo" ControlToValidate="txtDireccionCorreo" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNombre" runat="server" Text="Nombre"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtNombre" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvNombre" ControlToValidate="txtNombre" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <div class="col-sm-3"></div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblServidor" runat="server" Text="Servidor"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtServidor" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvServidor" ControlToValidate="txtServidor" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblHabilitarSsl" runat="server" Text="Habilitar SSL"></asp:Label>
            <div class="col-sm-3">
                <asp:CheckBox ID="chkHabilitar" CssClass="form-control" runat="server" />
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblPuerto" runat="server" Text="Puerto"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtPuerto" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvPuerto" ControlToValidate="txtPuerto" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="form-group row">
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblUsuario" runat="server" Text="Usuario"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox CssClass="form-control" ID="txtUsuario" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvUsuario" ControlToValidate="txtUsuario" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-sm-1 col-form-label" ID="lblContrasena" runat="server" Text="Contraseña"></asp:Label>
            <div class="col-sm-3">
                <asp:TextBox TextMode="Password" data-toggle="password" CssClass="form-control" ID="txtContrasena" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator CssClass="Validador" ID="rfvContrasena" ControlToValidate="txtContrasena" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
            </div>
            <div class="col-sm-3">
                <asp:Button CssClass="botonesEvol" ID="btnPrueba" runat="server" Text="Prueba"
                    OnClick="btnPrueba_Click" ToolTip="Prueba de Configuracion de Correo Electronico" />
                <asp:Button CssClass="botonesEvol" ID="btnVerificarPrueba" runat="server" Text="Verificar Prueba"
                    OnClick="btnVerificarPrueba_Click" ToolTip="Verificar Prueba" Visible="false" />
            </div>
        </div>

         <div class="table-responsive">
                <asp:GridView ID="gvDatos" Visible="false" runat="server" SkinID="GrillaResponsive" AutoGenerateColumns="true" >
                </asp:GridView>
        </div>

        <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />

        <%--<AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />--%>
        <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar"
                    OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"
                    OnClick="btnCancelar_Click" />
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
