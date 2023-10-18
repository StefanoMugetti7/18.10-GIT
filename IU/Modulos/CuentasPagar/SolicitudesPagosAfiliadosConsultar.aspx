<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="SolicitudesPagosAfiliadosConsultar.aspx.cs" Inherits="IU.Modulos.CuentasPagar.SolicitudesPagosAfiliadosConsultar" %>
<%@ Register Src="~/Modulos/CuentasPagar/Controles/SolicitudPagoDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
