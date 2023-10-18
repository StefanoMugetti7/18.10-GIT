<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ListasPreciosModificar.aspx.cs" Inherits="IU.Modulos.Compras.ListasPreciosModificar" %>
<%@ Register Src="~/Modulos/Compras/Controles/ListasPreciosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
