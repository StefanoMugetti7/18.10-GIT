<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PresupuestosModificar.aspx.cs" Inherits="IU.Modulos.Facturas.PresupuestosModificar" %>
<%@ Register Src="~/Modulos/Facturas/Controles/PresupuestosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>


<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>

