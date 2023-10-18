<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PrestadoresConsultar.aspx.cs" Inherits="IU.Modulos.Medicina.PrestadoresConsultar" %>
<%@ Register Src="~/Modulos/Medicina/Controles/PrestadoresModificarDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ctrModificarDatos" runat="server" />
</asp:Content>

