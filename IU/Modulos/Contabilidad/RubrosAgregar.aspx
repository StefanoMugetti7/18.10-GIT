﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="RubrosAgregar.aspx.cs" Inherits="IU.Modulos.Contabilidad.RubrosAgregar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/RubrosDatos.ascx" TagPrefix="AUGE" TagName="AgregarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:AgregarDatos ID="AgregarDatos" runat="server" />
</asp:Content>
