<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="ApoderadosAgregar.aspx.cs" Inherits="IU.Modulos.Afiliados.ApoderadosAgregar" Title="" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadoModificarDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ModifDatos" runat="server" />
</asp:Content>
