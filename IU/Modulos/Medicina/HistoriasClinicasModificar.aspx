<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="HistoriasClinicasModificar.aspx.cs" Inherits="IU.Modulos.Medicina.HistoriasClinicasModificar" %>
<%@ Register Src="~/Modulos/Medicina/Controles/HistoriasClinicasModificarDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ctrModificarDatos" runat="server" />
</asp:Content>
