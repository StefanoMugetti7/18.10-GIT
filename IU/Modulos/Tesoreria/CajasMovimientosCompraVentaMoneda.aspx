<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CajasMovimientosCompraVentaMoneda.aspx.cs" Inherits="IU.Modulos.Tesoreria.CajasMovimientosCompraVentaMoneda" %>
<%@ Register Src="~/Modulos/Tesoreria/Controles/CompraVentaMonedaDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>


<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
