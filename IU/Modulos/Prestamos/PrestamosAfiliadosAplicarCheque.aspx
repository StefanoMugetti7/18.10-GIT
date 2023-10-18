<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="PrestamosAfiliadosAplicarCheque.aspx.cs" Inherits="IU.Modulos.Prestamos.PrestamosAfiliadosAplicarCheque" %>
<%@ Register Src="~/Modulos/Prestamos/Controles/PrestamosAfiliadosModificarDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>