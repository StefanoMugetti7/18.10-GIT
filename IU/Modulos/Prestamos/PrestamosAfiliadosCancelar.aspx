<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="PrestamosAfiliadosCancelar.aspx.cs" Inherits="IU.Modulos.Prestamos.PrestamosAfiliadosCancelar" Title="" %>
<%@ Register Src="~/Modulos/Prestamos/Controles/PrestamosAfiliadosModificarDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
