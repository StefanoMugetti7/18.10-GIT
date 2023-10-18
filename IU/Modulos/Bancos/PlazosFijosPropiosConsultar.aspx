<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PlazosFijosPropiosConsultar.aspx.cs" Inherits="IU.Modulos.Bancos.PlazosFijosPropiosConsultar" %>
<%@ Register Src="~/Modulos/Bancos/Controles/PlazosFijosPropiosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>