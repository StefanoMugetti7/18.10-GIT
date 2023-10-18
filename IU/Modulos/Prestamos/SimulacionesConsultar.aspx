<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="SimulacionesConsultar.aspx.cs" Inherits="IU.Modulos.Prestamos.SimulacionesConsultar"  %>
<%@ Register Src="~/Modulos/Prestamos/Controles/PrestamosSimulacionModificarDatos.ascx" TagPrefix="AUGE" TagName="ConsultarDatos" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ConsultarDatos ID="ConsultarDatos" runat="server" />
</asp:Content>
