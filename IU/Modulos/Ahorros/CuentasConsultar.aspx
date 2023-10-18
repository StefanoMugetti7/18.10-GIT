<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="CuentasConsultar.aspx.cs" Inherits="IU.Modulos.Ahorros.CuentasConsultar" Title="" %>
<%@ Register Src="~/Modulos/Ahorros/Controles/CuentasModificarDatos.ascx" TagPrefix="AUGE" TagName="ConsultarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ConsultarDatos ID="ConsultarDatos" runat="server" />
</asp:Content>