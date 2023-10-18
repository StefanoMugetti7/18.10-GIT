<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SolicitudPagoDetalleBuscar.ascx.cs" Inherits="IU.Modulos.Contabilidad.Controles.SolicitudPagoDetalle" %>
<%@ Register src="~/Modulos/Contabilidad/Controles/SolicitudPagoDetalleBuscarPopUp.ascx" tagname="popUpSolicitudPagoDetalleBuscar" tagprefix="auge" %>

<div class="SolicitudPagoDetalle">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Label CssClass="labelEvol" ID="lblDescripcion" runat="server" Text="Descripción" />
            <asp:TextBox CssClass="textboxEvol" ID="txtDescripcion" runat="server" OnTextChanged="txtDescripcion_TextChanged" AutoPostBack="true"/>
            <div class="Espacio"></div>
            <asp:Button CssClass="botonesEvol" ID="btnBuscar" runat="server" Text="Buscar" onclick="btnBuscar_Click" />
            <asp:HiddenField ID="hfIdSolicitudPagoDetalle" runat="server" />
            <AUGE:popUpSolicitudPagoDetalleBuscar ID="puSolicitudPagoDetalle" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>