<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="TiposOperacionesComprobantesModificar.aspx.cs" Inherits="IU.Modulos.TGE.TiposOperacionesComprobantesModificar" %>
<%@ Register Src="~/Modulos/TGE/Control/TiposOperacionesComprobantesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>