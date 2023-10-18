<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PanteonesConsultar.aspx.cs" Inherits="IU.Modulos.Nichos.PanteonesConsultar" %>
<%@ Register Src="~/Modulos/Nichos/Controles/PanteonesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>