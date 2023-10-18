<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="FormasCobrosConsultar.aspx.cs" Inherits="IU.Modulos.TGE.FormasCobrosConsultar" %>
<%@ Register Src="~/Modulos/TGE/Control/FormasCobrosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>