<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="AsientosModelosConsultar.aspx.cs" Inherits="IU.Modulos.Contabilidad.AsientosModelosConsultar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/AsientosModelosDatos.ascx" TagPrefix="AUGE" TagName="ConsultarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ConsultarDatos ID="ConsultarDatos" runat="server" />
</asp:Content>
