<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ListasEleccionesModificar.aspx.cs" Inherits="IU.Modulos.Elecciones.ListasEleccionesModificar" %>
<%@ Register Src="~/Modulos/Elecciones/Controles/EleccionesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>


<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
