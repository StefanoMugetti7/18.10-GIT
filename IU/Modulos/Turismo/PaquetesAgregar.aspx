<%@ Page Title=""  EnableEventValidation="false" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PaquetesAgregar.aspx.cs" Inherits="IU.Modulos.Turismo.PaquetesAgregar" %>
<%@ Register Src="~/Modulos/Turismo/Controles/PaquetesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>

