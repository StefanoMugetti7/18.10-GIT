<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="NotasPedidosConsultar.aspx.cs" Inherits="IU.Modulos.Facturas.NotasPedidosConsultar" %>
<%@ Register Src="~/Modulos/Facturas/Controles/NotasPedidosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>