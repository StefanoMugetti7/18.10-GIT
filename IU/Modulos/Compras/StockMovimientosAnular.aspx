<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="StockMovimientosAnular.aspx.cs" Inherits="IU.Modulos.Compras.StockMovimientosAnular" %>
<%@ Register Src="~/Modulos/Compras/Controles/StockMovimientosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
