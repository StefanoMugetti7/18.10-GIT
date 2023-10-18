<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="NotasCreditosCargosAfiliadosAgregar.aspx.cs" Inherits="IU.Modulos.Cobros.NotasCreditosCargosAfiliadosAgregar" %>
<%@ Register src="~/Modulos/Cobros/Controles/OrdenesCobrosAfiliadosDatos.ascx" tagname="Controles" tagprefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <auge:Controles ID="ctrlDatos" runat="server" />
</asp:Content>
