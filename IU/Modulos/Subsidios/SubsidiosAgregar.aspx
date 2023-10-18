<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="SubsidiosAgregar.aspx.cs" Inherits="IU.Modulos.Subsidios.SubsidiosAgregar" %>
<%@ Register Src="~/Modulos/Subsidios/Controles/SubsidiosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>