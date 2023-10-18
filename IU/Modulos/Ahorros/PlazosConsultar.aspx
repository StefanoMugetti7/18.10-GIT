<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PlazosConsultar.aspx.cs" Inherits="IU.Modulos.Ahorros.PlazosConsultar" Title="" %>
<%@ Register Src="~/Modulos/Ahorros/Controles/PlazosDatos.ascx" TagPrefix="AUGE" TagName="ConsultarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ConsultarDatos ID="ConsultarDatos" runat="server" />
</asp:Content>
