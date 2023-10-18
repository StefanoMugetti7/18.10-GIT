<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="PlazosFijosRenovacionAnticipada.aspx.cs" Inherits="IU.Modulos.Ahorros.PlazosFijosRenovacionAnticipada" %>
<%@ Register Src="~/Modulos/Ahorros/Controles/PlazosFijosDatos.ascx" TagPrefix="AUGE" TagName="CancelarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:CancelarDatos ID="CancelarDatos" runat="server" />
</asp:Content>
