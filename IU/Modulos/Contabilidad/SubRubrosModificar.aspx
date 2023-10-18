<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="SubRubrosModificar.aspx.cs" Inherits="IU.Modulos.Contabilidad.SubRubrosModificar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/SubRubrosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
