<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ExpedientesModificar.aspx.cs" Inherits="IU.Modulos.Expedientes.ExpedientesModificar" %>
<%@ Register Src="~/Modulos/Expedientes/Controles/ExpedientesModificarDatos.ascx" TagPrefix="AUGE" TagName="ControlDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ControlDatos ID="ctrDatos" runat="server" />
</asp:Content>
