<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CuentasMovimientosEspecialesExtraer.aspx.cs" Inherits="IU.Modulos.Ahorros.CuentasMovimientosEspecialesExtraer" %>
<%@ Register Src="~/Modulos/Ahorros/Controles/CuentasMovimientosModificarDatos.ascx" TagPrefix="AUGE" TagName="DepositarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:DepositarDatos ID="DepositarDatos" runat="server" />
</asp:Content>