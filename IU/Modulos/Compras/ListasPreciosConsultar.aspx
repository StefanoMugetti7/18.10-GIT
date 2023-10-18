<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ListasPreciosConsultar.aspx.cs" Inherits="IU.Modulos.Compras.ListasPreciosConsultar" %>
<%@ Register Src="~/Modulos/Compras/Controles/ListasPreciosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
