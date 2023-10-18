<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="OrdenesComprasConsultar.aspx.cs" Inherits="IU.Modulos.Compras.OrdenesComprasConsultar" %>
<%@ Register Src="~/Modulos/Compras/Controles/OrdenesComprasDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
