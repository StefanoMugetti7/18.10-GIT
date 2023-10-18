<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="MensajesAlertasAgregar.aspx.cs" Inherits="IU.Modulos.Afiliados.MensajesAlertasAgregar" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/MensajesAlertasDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
