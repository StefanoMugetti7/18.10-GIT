<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="AsientosConsultar.aspx.cs" Inherits="IU.Modulos.Contabilidad.AsientosConsultar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/AsientosDatos.ascx" TagPrefix="AUGE" TagName="ConsultarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ConsultarDatos ID="ConsultarDatos" runat="server" />
</asp:Content>
