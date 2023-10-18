<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ProduccionesConsultar.aspx.cs" Inherits="IU.Modulos.Produccion.ProduccionesConsultar" %>
<%@ Register Src="~/Modulos/Produccion/Controles/ProduccionesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
