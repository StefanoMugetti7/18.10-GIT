<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="AsientosAgregar.aspx.cs" Inherits="IU.Modulos.Contabilidad.AsientosAgregar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/AsientosDatos.ascx" TagPrefix="AUGE" TagName="AgregarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:AgregarDatos ID="AgregarDatos" runat="server" />
</asp:Content>
