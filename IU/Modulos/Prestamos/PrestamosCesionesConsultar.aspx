<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PrestamosCesionesConsultar.aspx.cs" Inherits="IU.Modulos.Prestamos.PrestamosCesionesConsultar" %>
<%@ Register Src="~/Modulos/Prestamos/Controles/PrestamosCesionesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
