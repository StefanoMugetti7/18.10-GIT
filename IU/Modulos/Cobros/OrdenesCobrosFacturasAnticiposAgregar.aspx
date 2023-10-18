<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="OrdenesCobrosFacturasAnticiposAgregar.aspx.cs" Inherits="IU.Modulos.Cobros.OrdenesCobrosFacturasAnticiposAgregar" %>
<%@ Register Src="~/Modulos/Cobros/Controles/OrdenesCobrosFacturasAnticiposDatos.ascx" TagPrefix="AUGE" TagName="AgregarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:AgregarDatos ID="AgregarDatos" runat="server" />
</asp:Content>