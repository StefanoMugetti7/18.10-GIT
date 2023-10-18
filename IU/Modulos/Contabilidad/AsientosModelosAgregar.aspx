<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="AsientosModelosAgregar.aspx.cs" Inherits="IU.Modulos.Contabilidad.AsientosModelosAgregar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/AsientosModelosDatos.ascx" TagPrefix="AUGE" TagName="AgregarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:AgregarDatos ID="AgregarDatos" runat="server" />
</asp:Content>
