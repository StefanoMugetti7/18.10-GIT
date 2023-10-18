<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CargosAfiliadosAgregar.aspx.cs" Inherits="IU.Modulos.Cargos.CargosAfiliadosAgregar" Title="" %>
<%@ Register Src="~/Modulos/Cargos/Controles/CargoAfiliadoModificarDatos.ascx" TagPrefix="AUGE" TagName="AgregarDatos" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:AgregarDatos ID="AgregarDatos" runat="server" />
</asp:Content>
