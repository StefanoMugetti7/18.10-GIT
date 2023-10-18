<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="CuentasContablesAgregar.aspx.cs" Inherits="IU.Modulos.Contabilidad.CuentasContablesAgregar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/CuentasContablesDatos.ascx" TagPrefix="AUGE" TagName="AgregarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:AgregarDatos ID="AgregarDatos" runat="server" />
</asp:Content>