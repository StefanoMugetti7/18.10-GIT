<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="InformesRecepcionesAgregar.aspx.cs" Inherits="IU.Modulos.Compras.InformesRecepcionesAgregar" %>
<%@ Register Src="~/Modulos/Compras/Controles/InformesRecepcionesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
