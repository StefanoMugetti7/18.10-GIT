<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ProveedoresAgregar.aspx.cs" Inherits="IU.Modulos.Proveedores.ProveedoresAgregar" %>
<%@ Register Src="~/Modulos/Proveedores/Controles/ProveedoresDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>


<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
