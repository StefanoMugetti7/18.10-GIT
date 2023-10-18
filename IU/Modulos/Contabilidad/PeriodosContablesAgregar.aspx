<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PeriodosContablesAgregar.aspx.cs" Inherits="IU.Modulos.Contabilidad.PeriodosContablesAgregar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/PeriodosContablesDatos.ascx" TagPrefix="AUGE" TagName="AgregarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:AgregarDatos ID="AgregarDatos" runat="server" />
</asp:Content>
