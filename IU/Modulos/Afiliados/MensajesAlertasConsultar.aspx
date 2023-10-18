<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="MensajesAlertasConsultar.aspx.cs" Inherits="IU.Modulos.Afiliados.MensajesAlertasConsultar" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/MensajesAlertasDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
