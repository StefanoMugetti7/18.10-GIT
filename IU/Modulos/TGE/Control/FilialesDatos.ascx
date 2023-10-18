<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FilialesDatos.ascx.cs" Inherits="IU.Modulos.TGE.Control.FilialesDatos" %>
<%@ Register Src="~/Modulos/Comunes/popUpMensajesPostBack.ascx" TagName="popUpMensajesPostBack" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Auditoria/Control/AuditoriaDatos.ascx" TagName="AuditoriaDatos" TagPrefix="auge" %>
<%@ Register Src="~/Modulos/Comunes/CamposValores.ascx" TagPrefix="AUGE" TagName="CamposValores" %>
<%--<%@ Register src="~/Modulos/Comunes/Comentarios.ascx" tagname="Comentarios" tagprefix="auge" %>--%>

<div class="FilialesDatos">
    <div class="form-group row">
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblFilial" runat="server" Text="Filial" />
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:TextBox CssClass="form-control" ID="txtFilial" runat="server" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvFilial" runat="server" ControlToValidate="txtFilial"
                ErrorMessage="*" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblCodigoFilial" runat="server" Text="Codigo Filial" />
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:TextBox CssClass="form-control" ID="txtCodigoFilial" runat="server" />
        </div>

    </div>
    <div class="form-group row">
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDescripcion" runat="server" Text="Descripcion" />
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server" TextMode="MultiLine" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDescripcion" runat="server" ControlToValidate="txtDescripcion"
                ErrorMessage="*" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-lg-1 col-md-3 col-sm-3 col-form-label" ID="lblDireccion" runat="server" Text="Direccion" />
        <div class="col-lg-3 col-md-3 col-sm-9">
            <asp:TextBox CssClass="form-control" ID="txtDireccion" runat="server" TextMode="MultiLine" />
            <asp:RequiredFieldValidator CssClass="Validador" ID="rfvDireccion" runat="server" ControlToValidate="txtDireccion"
                ErrorMessage="*" ValidationGroup="Aceptar" />
        </div>
        <asp:Label CssClass="col-sm-1 col-form-label" ID="lblEstiloPlantilla" runat="server" Text="Estilo Plantilla" />
        <div class="col-sm-3">
            <asp:DropDownList ID="ddlEstiloPlantilla" Enabled="true" CssClass="form-control select2" runat="server" />
        </div>
    </div>
    <asp:TabContainer ID="tcDatos" runat="server" ActiveTabIndex="0" SkinID="MyTab">
        <asp:TabPanel runat="server" ID="tpHistorial" HeaderText="Auditoria">
            <ContentTemplate>
                <AUGE:AuditoriaDatos ID="ctrAuditoria" runat="server" />
            </ContentTemplate>
        </asp:TabPanel>
    </asp:TabContainer>
    <br />
    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <AUGE:CamposValores ID="ctrCamposValores" runat="server" />
            <center>
                <AUGE:popUpMensajesPostBack ID="popUpMensajes" runat="server" />
                <asp:Button CssClass="botonesEvol" ID="btnAceptar" runat="server" Text="Aceptar" onclick="btnAceptar_Click" ValidationGroup="Aceptar" />
                <asp:Button CssClass="botonesEvol" ID="btnCancelar" runat="server" Text="Volver" onclick="btnCancelar_Click" />
            </center>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
