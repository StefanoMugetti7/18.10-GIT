<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="NotasPedidosModificar.aspx.cs" Inherits="IU.Modulos.Facturas.NotasPedidosModificar" %>
<%@ Register Src="~/Modulos/Facturas/Controles/NotasPedidosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
