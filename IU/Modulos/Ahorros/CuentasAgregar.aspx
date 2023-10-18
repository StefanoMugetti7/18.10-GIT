<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="CuentasAgregar.aspx.cs" Inherits="IU.Modulos.Ahorros.CuentasAgregar" Title="" %>
<%@ Register Src="~/Modulos/Ahorros/Controles/CuentasModificarDatos.ascx" TagPrefix="AUGE" TagName="AgregarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:AgregarDatos ID="AgregarDatos" runat="server" />
</asp:Content>
