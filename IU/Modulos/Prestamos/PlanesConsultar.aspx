<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="PlanesConsultar.aspx.cs" Inherits="IU.Modulos.Prestamos.PlanesConsultar" %>
<%@ Register Src="~/Modulos/Prestamos/Controles/PlanesDatos.ascx" TagPrefix="AUGE" TagName="MostrarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server" >
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:MostrarDatos ID="MostrarDatos" runat="server" />
</asp:Content>