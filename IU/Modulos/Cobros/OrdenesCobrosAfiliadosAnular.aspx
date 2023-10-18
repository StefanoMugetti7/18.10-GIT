<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="OrdenesCobrosAfiliadosAnular.aspx.cs" Inherits="IU.Modulos.Cobros.OrdenesCobrosAfiliadosAnular" %>
<%@ Register src="~/Modulos/Cobros/Controles/OrdenesCobrosAfiliadosDatos.ascx" tagname="Controles" tagprefix="auge" %>
<%@ Register src="~/Modulos/Cobros/Controles/CancelacionAnticipadaPrestamosCuotas.ascx" tagname="ControlCancelacion" tagprefix="auge" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">--%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <auge:Controles ID="ctrlDatos" Visible="false" runat="server" />
    <auge:ControlCancelacion ID="ctrlCancelaciones" Visible="false" runat="server" />
</asp:Content>