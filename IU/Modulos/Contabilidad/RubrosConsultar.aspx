<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="RubrosConsultar.aspx.cs" Inherits="IU.Modulos.Contabilidad.RubrosConsultar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/RubrosDatos.ascx" TagPrefix="AUGE" TagName="ConsultarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ConsultarDatos ID="ConsultarDatos" runat="server" />
</asp:Content>
