<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="PrestamosAfiliadosPreAutorizar.aspx.cs" Inherits="IU.Modulos.Prestamos.PrestamosAfiliadosPreAutorizar" Title="" %>
<%@ Register Src="~/Modulos/Prestamos/Controles/PrestamosAfiliadosModificarDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
