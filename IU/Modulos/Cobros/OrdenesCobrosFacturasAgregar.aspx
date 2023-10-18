<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="OrdenesCobrosFacturasAgregar.aspx.cs" Inherits="IU.Modulos.Cobros.OrdenesCobrosFacturasAgregar" %>
<%@ Register Src="~/Modulos/Cobros/Controles/OrdenesCobrosFacturasDatos.ascx" TagPrefix="AUGE" TagName="AgregarDatos" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:AgregarDatos ID="AgregarDatos" runat="server" />
</asp:Content>

