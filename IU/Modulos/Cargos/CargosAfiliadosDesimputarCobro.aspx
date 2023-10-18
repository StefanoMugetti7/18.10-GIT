<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="CargosAfiliadosDesimputarCobro.aspx.cs" Inherits="IU.Modulos.Cargos.CargosAfiliadosDesimputarCobro" %>
<%@ Register Src="~/Modulos/Cargos/Controles/CargoAfiliadoModificarDatosFormaCobro.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
