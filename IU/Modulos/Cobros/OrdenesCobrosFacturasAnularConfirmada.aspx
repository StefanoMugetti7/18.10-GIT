﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="OrdenesCobrosFacturasAnularConfirmada.aspx.cs" Inherits="IU.Modulos.Cobros.OrdenesCobrosFacturasAnularConfirmada" %>
<%@ Register Src="~/Modulos/Cobros/Controles/OrdenesCobrosFacturasDatos.ascx" TagPrefix="AUGE" TagName="AnularDatos" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:AnularDatos ID="AnularDatos" runat="server" />
</asp:Content>