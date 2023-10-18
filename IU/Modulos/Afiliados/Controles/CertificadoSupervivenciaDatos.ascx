<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CertificadoSupervivenciaDatos.ascx.cs" Inherits="IU.Modulos.Afiliados.Controles.CertificadoSupervivenciaDatos" %>
<%@ Register src="~/Modulos/Comunes/Archivos.ascx" tagname="Archivos" tagprefix="auge" %>
<%--<%@ Register src="~/Modulos/Comunes/Comentarios.ascx" tagname="Comentarios" tagprefix="auge" %>--%>
<%@ Register src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" tagname="AuditoriaDatos" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register src="~/Modulos/Comunes/popUpBotonConfirmar.ascx" tagname="popUpBotonConfirmar" tagprefix="auge" %>

<auge:popUpBotonConfirmar ID="popUpConfirmar" runat="server"   />

<div class="CertificadoSupervivencia">
    <asp:Label CssClass="labelEvol" ID="lblFechaCertificado"  runat="server" Text="Fecha Certificado" />
    <asp:TextBox CssClass="textboxEvol" ID="txtFechaCertificado" TabIndex="11" runat="server"></asp:TextBox>
    <div class="Calendario">
        <asp:Image ID="imgFechaCertificado" runat="server" ImageUrl="~/Imagenes/Calendario.png"/>
        <asp:CalendarExtender ID="ceFechaCertificado" runat="server" Enabled="true" 
        TargetControlID="txtFechaCertificado" PopupButtonID="imgFechaCertificado" Format="dd/MM/yyyy"></asp:CalendarExtender>
        <asp:RequiredFieldValidator CssClass="Validador"  ID="rfvFechaCertificado" ControlToValidate="txtFechaCertificado" ValidationGroup="CertificadoSupervivenciaAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    </div>

    <asp:Label CssClass="labelEvol" ID="lblEstado"  runat="server" Text="Estado" />
    <asp:DropDownList CssClass="selectEvol" ID="ddlEstado"  runat="server" />
    
    <br />
    
    <asp:Label CssClass="labelEvol" ID="lblDetalle"  runat="server" Text="Detalle" />
    <asp:TextBox CssClass="textboxEvol" ID="txtDetalle" TextMode="MultiLine" MaxLength="500" Width="580" Rows="1" runat="server"></asp:TextBox>
    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDetalle" ControlToValidate="txtDetalle" ValidationGroup="CertificadoSupervivenciaAceptar" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
    
<br />
<br />

    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab" >
<%--        <asp:TabPanel runat="server" ID="tpComentarios" HeaderText="Comentarios" >
            <ContentTemplate>
                <AUGE:Comentarios ID="ctrComentarios" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>--%>
        <asp:TabPanel runat="server" ID="tpArchivos" HeaderText="Archivos" >
            <ContentTemplate>
                <AUGE:Archivos ID="ctrArchivos" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria" >
            <ContentTemplate>
                <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>

<br />

    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
            <center>
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="CertificadoSupervivenciaAceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel> 
</div>