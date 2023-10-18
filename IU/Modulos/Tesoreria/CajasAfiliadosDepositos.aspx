<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="CajasAfiliadosDepositos.aspx.cs" Inherits="IU.Modulos.Tesoreria.CajasAfiliadosDepositos" %>
<%@ Register Src="~/Modulos/Ahorros/Controles/CuentasMovimientosModificarDatos.ascx" TagPrefix="AUGE" TagName="DepositarDatos" %>


<asp:Content ID="Content7" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:DepositarDatos ID="DepositarDatos" runat="server" />
</asp:Content>
