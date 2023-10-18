<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="MonedasConsultar.aspx.cs" Inherits="IU.Modulos.Contabilidad.MonedasConsultar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/MonedasDatos.ascx" TagPrefix="AUGE" TagName="ConsultarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ConsultarDatos ID="ConsultarDatos" runat="server" />
</asp:Content>

