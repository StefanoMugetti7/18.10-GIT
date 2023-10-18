<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoriasModificarDatos.ascx.cs" Inherits="IU.Modulos.Afiliados.Controles.CategoriasModificarDatos" %>
<%@ Register Src="~/Modulos/Comunes/Archivos.ascx" TagName="Archivos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/Comentarios.ascx" TagName="Comentarios" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>

<div class="CategoriasModificarDatos">
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCodigo" runat="server" Text="Código" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtCodigo" runat="server" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblCategoria" runat="server" Text="Categoría" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtCategoria" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvCategoria" runat="server" ErrorMessage="*"
                ControlToValidate="txtCategoria" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstado" runat="server" Text="Estado" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlEstado" runat="server" />
        </div>
        <div class="col-sm-3"></div>
    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblNumeroSocioDesde" runat="server" Text="Nro. Socio Desde" />
        <div class="col-sm-3">
            <asp:TextBox CssClass="form-control" ID="txtNumeroSocioDesde" runat="server" />
        </div>
         <asp:Label CssClass="col-sm-1 col-form-label" ID="lblTipoCategoria" runat="server" Text="Tipo de Categoria" />
        <div class="col-sm-3">
            <asp:DropDownList CssClass="form-control select2" ID="ddlTipoCategoria" runat="server" />
        </div>
        <div class="col-sm-8"></div>
    </div>
    <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpComentarios" HeaderText="Comentarios">
            <ContentTemplate>
                <AUGE:Comentarios ID="ctrComentarios" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpArchivos" HeaderText="Archivos">
            <ContentTemplate>
                <AUGE:Archivos ID="ctrArchivos" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
        <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria">
            <ContentTemplate>
                <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>

    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="row justify-content-md-center">
                <div class="col-md-auto">
                    <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                    <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" OnClick="btnAceptar_Click" ValidationGroup="Aceptar" />
                    <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" OnClick="btnCancelar_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>

