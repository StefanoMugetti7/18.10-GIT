<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportesParametrosDatos.ascx.cs" Inherits="IU.Modulos.Reportes.Controles.ReportesParametrosDatos" %>
<%@ Register src="~/Modulos/Comunes/popUpComprobantes.ascx" tagname="popUpComprobantes" tagprefix="auge" %>

<div class="ReportesDatos" >
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <br />
            <asp:Panel ID="pnlParametros" GroupingText="Parameters" Visible="false" runat="server">
            <%--<asp:Panel ID="tablaParametros" runat="server">
            </asp:Panel>--%>
            </asp:Panel>
            <br /><br />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>