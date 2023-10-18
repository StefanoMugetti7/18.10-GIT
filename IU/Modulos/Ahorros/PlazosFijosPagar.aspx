<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="PlazosFijosPagar.aspx.cs" Inherits="IU.Modulos.Ahorros.PlazosFijosPagar" %>
<%@ Register Src="~/Modulos/Ahorros/Controles/PlazosFijosDatos.ascx" TagPrefix="AUGE" TagName="CancelarDatos" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:CancelarDatos ID="CancelarDatos" runat="server" />
</asp:Content>

