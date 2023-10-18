<%@ Page Title="" Language="C#"  EnableEventValidation="false" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PaquetesModificar.aspx.cs" Inherits="IU.Modulos.Turismo.PaquetesModificar" %>
<%@ Register Src="~/Modulos/Turismo/Controles/PaquetesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>

