<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PrestacionesModificar.aspx.cs" Inherits="IU.Modulos.Medicina.PrestacionesModificar" %>
<%@ Register Src="~/Modulos/Medicina/Controles/PrestacionesModificarDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ctrModificarDatos" runat="server" />
</asp:Content>
