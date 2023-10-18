<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="OrdenesComprasAfiliadosConsultar.aspx.cs" Inherits="IU.Modulos.Compras.OrdenesComprasAfiliadosConsultar" %>
<%@ Register Src="~/Modulos/Compras/Controles/OrdenesComprasAbiertaDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
