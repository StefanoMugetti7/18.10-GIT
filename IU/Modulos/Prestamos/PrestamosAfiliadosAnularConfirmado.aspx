<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="PrestamosAfiliadosAnularConfirmado.aspx.cs" Inherits="IU.Modulos.Prestamos.PrestamosAfiliadosAnularConfirmado" Title="" %>
<%@ Register Src="~/Modulos/Prestamos/Controles/PrestamosAfiliadosModificarDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>