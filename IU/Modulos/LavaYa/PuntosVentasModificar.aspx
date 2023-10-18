<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="PuntosVentasModificar.aspx.cs" Inherits="IU.Modulos.LavaYa.PuntosVentasModificar" %>
<%@ Register Src="~/Modulos/LavaYa/Controles/PuntosVentasDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>

