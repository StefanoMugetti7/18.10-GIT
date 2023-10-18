<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="CargosAfiliadosConsultar.aspx.cs" Inherits="IU.Modulos.Cargos.CargosAfiliadosConsultar" %>
<%@ Register Src="~/Modulos/Cargos/Controles/CargoAfiliadoModificarDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
