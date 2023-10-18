<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="InformesRecepcionesConsultar.aspx.cs" Inherits="IU.Modulos.Compras.InformesRecepcionesConsultar" %>
<%@ Register Src="~/Modulos/Compras/Controles/InformesRecepcionesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
