﻿<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CargosAfiliadosModificar.aspx.cs" Inherits="IU.Modulos.Cargos.CargosAfiliadosModificar" Title="" %>
<%@ Register Src="~/Modulos/Cargos/Controles/CargoAfiliadoModificarDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
