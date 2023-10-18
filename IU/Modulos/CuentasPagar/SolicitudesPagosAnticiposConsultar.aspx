<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="SolicitudesPagosAnticiposConsultar.aspx.cs" Inherits="IU.Modulos.CuentasPagar.SolicitudesPagosAnticiposConsultar" %>
<%@ Register Src="~/Modulos/CuentasPagar/Controles/SolicitudesPagosAnticipos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
