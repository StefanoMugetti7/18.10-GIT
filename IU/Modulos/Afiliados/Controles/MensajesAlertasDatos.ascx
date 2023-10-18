<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MensajesAlertasDatos.ascx.cs" Inherits="IU.Modulos.Afiliados.Controles.MensajesAlertasDatos" %>
<%@ Register src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" tagname="AuditoriaDatos" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>

<div >
    <div class="form-group row">
 
    <asp:Label ID="lblMensaje" CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" runat="server" Text="Mensaje"></asp:Label>
    <div class="col-lg-7 col-md-7 col-sm-9">
    <asp:TextBox ID="txtMensaje" CssClass="form-control" ReadOnly="true" TextMode="MultiLine" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvMensaje" ControlToValidate="txtMensaje" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
       </div>
    
    <asp:Label ID="lblEstado" CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" runat="server" Enabled="false" Text="Estado"></asp:Label>
   <div class="col-lg-3 col-md-3 col-sm-9"> <asp:DropDownList ID="ddlEstados" CssClass="form-control select2" runat="server" AutoPostBack="false" />
    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvEstado" ControlToValidate="ddlEstados" ValidationGroup="Aceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
       </div></div>
    <asp:Panel runat="server" ID="pnlAuditoria" GroupingText="Auditoria" HeaderText="Auditoria" >
        <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
    </asp:Panel>


<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server" >
    <ContentTemplate>
      <div class="row justify-content-md-center">
            <div class="col-md-auto">
            <auge:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" Visible="false" 
                    onclick="btnAceptar_Click" ValidationGroup="AfiliadosModificarDatosAceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" CausesValidation="false"               
                    onclick="btnCancelar_Click" />
      </div></div>
    </ContentTemplate>
</asp:UpdatePanel> 

</div>
