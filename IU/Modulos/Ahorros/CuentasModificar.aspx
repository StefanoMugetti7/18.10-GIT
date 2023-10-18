<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="CuentasModificar.aspx.cs" Inherits="IU.Modulos.Ahorros.CuentasModificar" Title="" %>
<%@ Register Src="~/Modulos/Ahorros/Controles/CuentasModificarDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
