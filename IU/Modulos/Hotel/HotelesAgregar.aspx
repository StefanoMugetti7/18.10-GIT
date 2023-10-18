<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="HotelesAgregar.aspx.cs" Inherits="IU.Modulos.Hotel.HotelesAgregar" %>
<%@ Register Src="~/Modulos/Hotel/Controles/HotelesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>