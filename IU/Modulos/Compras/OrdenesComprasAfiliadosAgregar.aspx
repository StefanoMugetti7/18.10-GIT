<%@ Page Title="" Language="C#" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="OrdenesComprasAfiliadosAgregar.aspx.cs" Inherits="IU.Modulos.Compras.OrdenesComprasAfiliadosAgregar" %>
<%@ Register Src="~/Modulos/Compras/Controles/OrdenesComprasAbiertaDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>
 
<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
