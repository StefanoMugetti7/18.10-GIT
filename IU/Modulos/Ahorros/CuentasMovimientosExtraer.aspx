<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CuentasMovimientosExtraer.aspx.cs" Inherits="IU.Modulos.Ahorros.CuentasMovimientosExtraer" Title="" %>
<%@ Register Src="~/Modulos/Ahorros/Controles/CuentasMovimientosModificarDatos.ascx" TagPrefix="AUGE" TagName="ExtraerDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ExtraerDatos ID="ExtraerDatos" runat="server" />
</asp:Content>
