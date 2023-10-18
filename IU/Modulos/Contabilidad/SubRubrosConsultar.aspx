<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="SubRubrosConsultar.aspx.cs" Inherits="IU.Modulos.Contabilidad.SubRubrosConsultar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/SubRubrosDatos.ascx" TagPrefix="AUGE" TagName="ConsultarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ConsultarDatos ID="ConsultarDatos" runat="server" />
</asp:Content>
