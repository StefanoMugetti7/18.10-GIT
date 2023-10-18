<%@ Page Title="" Language="C#"  EnableEventValidation="false" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ConveniosModificar.aspx.cs" Inherits="IU.Modulos.Turismo.ConveniosModificar" %>
<%@ Register Src="~/Modulos/Turismo/Controles/ConveniosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>

