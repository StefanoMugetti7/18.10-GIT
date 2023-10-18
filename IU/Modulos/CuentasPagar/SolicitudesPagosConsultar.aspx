<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="SolicitudesPagosConsultar.aspx.cs" Inherits="IU.Modulos.CuentasPagar.SolicitudesPagosConsultar" %>
<%@ Register Src="~/Modulos/CuentasPagar/Controles/SolicitudPagoDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>


<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
