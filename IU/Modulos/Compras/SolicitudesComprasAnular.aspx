<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="SolicitudesComprasAnular.aspx.cs" Inherits="IU.Modulos.Compras.SolicitudesComprasAnular" %>
<%@ Register Src="~/Modulos/Compras/Controles/SolicitudesComprasDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
