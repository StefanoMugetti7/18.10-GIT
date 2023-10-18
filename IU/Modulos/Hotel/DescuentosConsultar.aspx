<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="DescuentosConsultar.aspx.cs" Inherits="IU.Modulos.Hotel.DescuentosConsultar" %>
<%@ Register Src="~/Modulos/Hotel/Controles/DescuentosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>


<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
