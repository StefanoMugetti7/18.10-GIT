<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="PlazosFijosAgregar.aspx.cs" Inherits="IU.Modulos.Ahorros.PlazosFijosAgregar" Title="" %>
<%@ Register Src="~/Modulos/Ahorros/Controles/PlazosFijosDatos.ascx" TagPrefix="AUGE" TagName="AgregarDatos" %>

<asp:Content ID="Content3" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:AgregarDatos ID="AgregarDatos" runat="server" />
</asp:Content>