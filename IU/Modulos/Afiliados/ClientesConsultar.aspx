<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ClientesConsultar.aspx.cs" Inherits="IU.Modulos.Afiliados.ClientesConsultar" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/ClientesModificarDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
       <AUGE:ModificarDatos ID="ModifDatos" runat="server" />
</asp:Content>