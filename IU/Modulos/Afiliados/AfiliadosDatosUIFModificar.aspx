<%@ Page Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="AfiliadosDatosUIFModificar.aspx.cs" Inherits="IU.Modulos.Afiliados.AfiliadosDatosUIFModificar" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadoDatosUIF.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>





<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
       <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
