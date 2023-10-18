<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="NotasPedidosAgregar.aspx.cs" Inherits="IU.Modulos.Facturas.NotasPedidosAgregar" %>
<%@ Register Src="~/Modulos/Facturas/Controles/NotasPedidosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>


<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>