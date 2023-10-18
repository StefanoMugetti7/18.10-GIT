<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CargosAfiliadosModificarFomasCobros.aspx.cs" Inherits="IU.Modulos.Cargos.CargosAfiliadosModificarFomasCobros" %>
<%@ Register Src="~/Modulos/Cargos/Controles/CargoAfiliadoModificarDatosFormaCobro.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
