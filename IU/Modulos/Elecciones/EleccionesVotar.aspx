<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="EleccionesVotar.aspx.cs" Inherits="IU.Modulos.Elecciones.EleccionesVotar" %>
<%@ Register Src="~/Modulos/Elecciones/Controles/EleccionesVotacionDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
