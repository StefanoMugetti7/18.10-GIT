<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="SolicitudesSubsidiosConsultar.aspx.cs" Inherits="IU.Modulos.Subsidios.SolicitudesSubsidiosConsultar" %>
<%@ Register Src="~/Modulos/Subsidios/Controles/SolicitudesSubsidiosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
