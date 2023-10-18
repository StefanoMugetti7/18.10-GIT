<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" EnableEventValidation="false"  AutoEventWireup="true" CodeBehind="OrdenesCobrosAfiliadosAgregar.aspx.cs" Inherits="IU.Modulos.Cobros.OrdenesCobrosAfiliadosAgregar" %>
<%@ Register src="~/Modulos/Cobros/Controles/OrdenesCobrosAfiliadosDatos.ascx" tagname="Controles" tagprefix="auge" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">--%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <auge:Controles ID="ctrlDatos" runat="server" />
</asp:Content>