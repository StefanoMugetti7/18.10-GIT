<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/Modulos/Comunes/nmpAfiliados.master" AutoEventWireup="true" CodeBehind="FamiliaresAgregar.aspx.cs" Inherits="IU.Modulos.Afiliados.FamiliaresAgregar" Title="" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadoModificarDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">
    <AUGE:ModificarDatos ID="ModifDatos" runat="server" />
</asp:Content>
