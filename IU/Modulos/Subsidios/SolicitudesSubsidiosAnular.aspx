<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="SolicitudesSubsidiosAnular.aspx.cs" Inherits="IU.Modulos.Subsidios.SolicitudesSubsidiosAnular" %>
<%@ Register Src="~/Modulos/Subsidios/Controles/SolicitudesSubsidiosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
