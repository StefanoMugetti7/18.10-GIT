<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="PlazosModificar.aspx.cs" Inherits="IU.Modulos.Ahorros.PlazosModificar" Title="" %>
<%@ Register Src="~/Modulos/Ahorros/Controles/PlazosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>