<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PlantillasModificar.aspx.cs" Inherits="IU.Modulos.Plantillas.PlantillasModificar" %>
<%@ Register Src="~/Modulos/Plantillas/Controles/PlantillasModificarDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>