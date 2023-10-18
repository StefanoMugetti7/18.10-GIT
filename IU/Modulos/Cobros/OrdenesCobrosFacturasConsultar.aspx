<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="OrdenesCobrosFacturasConsultar.aspx.cs" Inherits="IU.Modulos.Cobros.OrdenesCobrosFacturasConsultar" %>
<%@ Register Src="~/Modulos/Cobros/Controles/OrdenesCobrosFacturasDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
