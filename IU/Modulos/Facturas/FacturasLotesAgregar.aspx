<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="FacturasLotesAgregar.aspx.cs" Inherits="IU.Modulos.Facturas.FacturasLotesAgregar" %>
<%@ Register Src="~/Modulos/Facturas/Controles/FacturasLotesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>


<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>

