<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ListaEsperaModificar.aspx.cs" Inherits="IU.Modulos.Hotel.ListaEsperaModificar" %>
<%@ Register Src="~/Modulos/Hotel/Controles/ListaEsperaDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>