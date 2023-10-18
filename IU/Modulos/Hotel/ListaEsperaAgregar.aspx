<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ListaEsperaAgregar.aspx.cs" Inherits="IU.Modulos.Hotel.ListaEsperaAgregar" %>
<%@ Register Src="~/Modulos/Hotel/Controles/ListaEsperaDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>