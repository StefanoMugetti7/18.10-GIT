<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="RequerimientosAfiliadosModificar.aspx.cs" Inherits="IU.Modulos.CRM.RequerimientosAfiliadosModificar" %>
<%@ Register Src="~/Modulos/CRM/Controles/RequerimientosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos2 ID="ModificarDatos2" runat="server" />
</asp:Content>

