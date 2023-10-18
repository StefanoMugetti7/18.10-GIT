<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="SubsidiosConsultar.aspx.cs" Inherits="IU.Modulos.Subsidios.SubsidiosConsultar" %>
<%@ Register Src="~/Modulos/Subsidios/Controles/SubsidiosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
