<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="CuentasAhorrosTiposConsultar.aspx.cs" Inherits="IU.Modulos.Ahorros.CuentasAhorrosTiposConsultar" Title="" %>
<%@ Register Src="~/Modulos/Ahorros/Controles/CuentasAhorrosTiposDatos.ascx" TagPrefix="AUGE" TagName="ConsultarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ConsultarDatos ID="ConsultarDatos" runat="server" />
</asp:Content>