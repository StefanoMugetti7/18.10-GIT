<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="AsientosModelosModificar.aspx.cs" Inherits="IU.Modulos.Contabilidad.AsientosModelosModificar" %>
<%@ Register Src="~/Modulos/Contabilidad/Controles/AsientosModelosDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
