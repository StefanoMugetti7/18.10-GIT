<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="CajasAfiliadosExtracciones.aspx.cs" Inherits="IU.Modulos.Tesoreria.CajasAfiliadosExtracciones" %>
<%@ Register Src="~/Modulos/Ahorros/Controles/CuentasMovimientosModificarDatos.ascx" TagPrefix="AUGE" TagName="ExtraerDatos" %>


<asp:Content ID="Content7" ContentPlaceHolderID="cphPrincipal" runat="server">
      <AUGE:ExtraerDatos ID="ExtraerDatos" runat="server" />
</asp:Content>
