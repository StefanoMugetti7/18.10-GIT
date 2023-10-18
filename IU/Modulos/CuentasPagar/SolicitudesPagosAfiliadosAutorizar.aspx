<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="SolicitudesPagosAfiliadosAutorizar.aspx.cs" Inherits="IU.Modulos.CuentasPagar.SolicitudesPagosAfiliadosAutorizar" %>
<%@ Register Src="~/Modulos/CuentasPagar/Controles/SolicitudPagoDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
