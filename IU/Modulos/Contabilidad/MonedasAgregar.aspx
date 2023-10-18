<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="MonedasAgregar.aspx.cs" Inherits="IU.Modulos.Contabilidad.MonedasAgregar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/MonedasDatos.ascx" TagPrefix="AUGE" TagName="AgregarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:AgregarDatos ID="AgregarDatos" runat="server" />
</asp:Content>