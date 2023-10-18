<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="CertificadoSupervivenciaModificar.aspx.cs" Inherits="IU.Modulos.Afiliados.CertificadoSupervivenciaModificar" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/CertificadoSupervivenciaDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
