<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ProveedoresConsultar.aspx.cs" Inherits="IU.Modulos.Proveedores.ProveedoresConsultar" %>
<%@ Register Src="~/Modulos/Proveedores/Controles/ProveedoresDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
