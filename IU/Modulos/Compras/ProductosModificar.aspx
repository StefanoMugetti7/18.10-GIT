﻿<%@ Page Title="" EnableEventValidation="false" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ProductosModificar.aspx.cs" Inherits="IU.Modulos.Compras.ProductosModificar" %>
<%@ Register Src="~/Modulos/Compras/Controles/ProductosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>