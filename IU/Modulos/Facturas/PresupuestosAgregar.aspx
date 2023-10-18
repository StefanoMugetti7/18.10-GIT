<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PresupuestosAgregar.aspx.cs" Inherits="IU.Modulos.Facturas.PresupuestosAgregar" %>
<%@ Register Src="~/Modulos/Facturas/Controles/PresupuestosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>


<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
