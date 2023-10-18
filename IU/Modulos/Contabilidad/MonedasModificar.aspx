<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="MonedasModificar.aspx.cs" Inherits="IU.Modulos.Contabilidad.MonedasModificar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/MonedasDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
