<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="AfiliadosAgregar.aspx.cs" Inherits="IU.Modulos.Afiliados.AfiliadosAgregar"  %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadoModificarDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceEncabezado" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceIzquierdoArriba" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceCentroArriba" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceDerechaArriba" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="ContentPlaceIzquierdoCentro" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModifDatos" runat="server" />
</asp:Content>
