<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PeriodosIvasConsultar.aspx.cs" Inherits="IU.Modulos.Contabilidad.PeriodosIvasConsultar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/PeriodosIvasDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
