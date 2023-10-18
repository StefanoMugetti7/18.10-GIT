<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PeriodosContablesModificar.aspx.cs" Inherits="IU.Modulos.Contabilidad.PeriodosContablesModificar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/PeriodosContablesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
