<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="CuentasMovimientosDepositar.aspx.cs" Inherits="IU.Modulos.Ahorros.CuentasMovimientosDepositar" Title="" %>
<%@ Register Src="~/Modulos/Ahorros/Controles/CuentasMovimientosModificarDatos.ascx" TagPrefix="AUGE" TagName="DepositarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:DepositarDatos ID="DepositarDatos" runat="server" />
</asp:Content>
