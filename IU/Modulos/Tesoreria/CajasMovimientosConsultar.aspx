<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="CajasMovimientosConsultar.aspx.cs" Inherits="IU.Modulos.Tesoreria.CajasMovimientosConsultar" %>
<%@ Register Src="~/Modulos/Tesoreria/Controles/CajasMovimientosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>

