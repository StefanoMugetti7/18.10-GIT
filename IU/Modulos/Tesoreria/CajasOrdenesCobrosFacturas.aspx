<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Tesoreria/nmpCajas.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CajasOrdenesCobrosFacturas.aspx.cs" Inherits="IU.Modulos.Tesoreria.CajasOrdenesCobrosFacturas" %>
<%@ Register src="~/Modulos/Cobros/Controles/OrdenesCobrosFacturasDatos.ascx" tagname="Controles" tagprefix="auge" %>

<asp:Content ID="Content7" ContentPlaceHolderID="cphPrincipal" runat="server">
    <auge:Controles ID="ctrlDatos" runat="server" />
</asp:Content>






