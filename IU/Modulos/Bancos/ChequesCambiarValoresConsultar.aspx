<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ChequesCambiarValoresConsultar.aspx.cs" Inherits="IU.Modulos.Bancos.ChequesCambiarValoresConsultar" %>
<%@ Register Src="~/Modulos/Bancos/Controles/ChequesCambiarValoresDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>