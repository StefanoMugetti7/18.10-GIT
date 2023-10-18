<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SectoresDatos.ascx.cs" Inherits="IU.Modulos.TGE.Control.SectoresDatos" %>
<%@ Register src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" tagname="popUpMensajesPostBack" tagprefix="auge" %>
<%@ Register src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" tagname="AuditoriaDatos" tagprefix="auge" %>
<%--<%@ Register src="~/Modulos/Comunes/Comentarios.ascx" tagname="Comentarios" tagprefix="auge" %>--%>

<div class="SectoresDatos">

       <div class="form-group row">
    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblSector"  runat="server" Text="Sector" />
   <div class="col-lg-3 col-md-3 col-sm-9">  <asp:TextBox CssClass="form-control" ID="txtSector"  runat="server" />
    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvSector" runat="server" ControlToValidate="txtSector" 
        ErrorMessage="*" ValidationGroup="Aceptar"/>
</div>

    <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFilial"  runat="server" Text="Filial" />
    <div class="col-lg-3 col-md-3 col-sm-9"> <asp:DropDownList CssClass="form-control select2" ID="ddlFiliales"  runat="server" />
    <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFiliales" runat="server" ControlToValidate="ddlFiliales" 
        ErrorMessage="*" ValidationGroup="Aceptar"/>
   </div></div>

    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab" >
        <%--<asp:TabPanel runat="server" ID="tpComentarios" HeaderText="Comentarios" >
            <ContentTemplate>
                <AUGE:Comentarios ID="ctrComentarios" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>--%>
        <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria" >
            <ContentTemplate>
                <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>

<br />
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server" >
        <ContentTemplate>
          <div class="row justify-content-md-center">
            <div class="col-md-auto">
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
         </div></div>
        </ContentTemplate>
</asp:UpdatePanel> 

</div>