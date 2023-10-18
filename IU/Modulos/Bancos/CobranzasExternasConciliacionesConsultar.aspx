<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="CobranzasExternasConciliacionesConsultar.aspx.cs" Inherits="IU.Modulos.Bancos.CobranzasExternasConciliacionesConsultar" %>
<%@ Register Src="~/Modulos/Bancos/Controles/CobranzasExternasConciliacionesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>