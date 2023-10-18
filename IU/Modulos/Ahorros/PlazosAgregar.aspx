<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="PlazosAgregar.aspx.cs" Inherits="IU.Modulos.Ahorros.PlazosAgregar" Title="" %>
<%@ Register Src="~/Modulos/Ahorros/Controles/PlazosDatos.ascx" TagPrefix="AUGE" TagName="AgregarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:AgregarDatos ID="AgregarDatos" runat="server" />
</asp:Content>
