<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PeriodosContablesConsultar.aspx.cs" Inherits="IU.Modulos.Contabilidad.PeriodosContablesConsultar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/PeriodosContablesDatos.ascx" TagPrefix="AUGE" TagName="ConsultarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ConsultarDatos ID="ConsultarDatos" runat="server" />
</asp:Content>
