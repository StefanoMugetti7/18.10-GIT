<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="FacturasModificar.aspx.cs" Inherits="IU.Modulos.Facturas.Controles.FacturasModificar" %>
<%@ Register Src="~/Modulos/Facturas/Controles/FacturasDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
