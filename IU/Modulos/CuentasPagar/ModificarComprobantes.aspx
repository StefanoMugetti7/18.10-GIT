<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ModificarComprobantes.aspx.cs" Inherits="IU.Modulos.CuentasPagar.ModificarComprobantes" %>
<%@ Register Src="~/Modulos/CuentasPagar/Controles/ModificarComprobantesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>