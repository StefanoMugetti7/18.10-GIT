﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="FondoSuplementarioAgregar.aspx.cs" Inherits="IU.Modulos.Haberes.FondoSuplementarioAgregar" %>
<%@ Register Src="~/Modulos/Haberes/Controles/FondoSuplementarioDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>


<asp:Content ID="Content7" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ctrModificarDatos" runat="server" />
</asp:Content>