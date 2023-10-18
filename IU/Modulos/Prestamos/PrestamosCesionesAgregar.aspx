<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PrestamosCesionesAgregar.aspx.cs" Inherits="IU.Modulos.Prestamos.PrestamosCesionesAgregar" %>
<%@ Register Src="~/Modulos/Prestamos/Controles/PrestamosCesionesDatos.ascx" TagPrefix="AUGE" TagName="AgregarDatos" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:AgregarDatos ID="AgregarDatos" runat="server" />
</asp:Content>

