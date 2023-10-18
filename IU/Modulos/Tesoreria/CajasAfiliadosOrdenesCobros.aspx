<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="CajasAfiliadosOrdenesCobros.aspx.cs" Inherits="IU.Modulos.Tesoreria.CajasAfiliadosOrdenesCobros" %>
<%@ Register src="~/Modulos/Cobros/Controles/OrdenesCobrosAfiliadosDatos.ascx" tagname="Controles" tagprefix="auge" %>

<asp:Content ID="Content7" ContentPlaceHolderID="cphPrincipal" runat="server">
     <auge:Controles ID="ctrlDatos" runat="server" />
</asp:Content>
