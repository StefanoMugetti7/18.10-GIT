<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="SimulacionesAgregar.aspx.cs" Inherits="IU.Modulos.Prestamos.SimulacionesAgregar" Title="" %>
<%@ Register Src="~/Modulos/Prestamos/Controles/PrestamosSimulacionModificarDatos.ascx" TagPrefix="AUGE" TagName="AgregarDatos" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:AgregarDatos ID="AgregarDatos" runat="server" />
</asp:Content>