﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="OrdenesComprasAutorizar.aspx.cs" Inherits="IU.Modulos.Compras.OrdenesComprasAutorizar" %>
<%@ Register Src="~/Modulos/Compras/Controles/OrdenesComprasDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>