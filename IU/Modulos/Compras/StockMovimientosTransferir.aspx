<%@ Page Title="" Language="C#"  EnableEventValidation="false" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="StockMovimientosTransferir.aspx.cs" Inherits="IU.Modulos.Compras.StockMovimientosTransferir" %>
<%@ Register Src="~/Modulos/Compras/Controles/StockMovimientosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>

