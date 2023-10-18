<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="SolicitudesComprasAgregar.aspx.cs" Inherits="IU.Modulos.Compras.SolicitudesComprasAgregar" %>
<%@ Register Src="~/Modulos/Compras/Controles/SolicitudesComprasDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
