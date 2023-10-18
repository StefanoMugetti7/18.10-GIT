<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="OrdenesCobrosAgregar.aspx.cs" Inherits="IU.Modulos.Cobros.OrdenesCobrosAgregar" Title="" %>
<%@ Register src="~/Modulos/Cobros/Controles/OrdenesCobrosDatos.ascx" tagname="Controles" tagprefix="auge" %>

<%--<asp:Content ID="Content1" ContentPlaceHolderID="cphPrincipal" runat="server">--%>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <auge:Controles ID="ctrlDatos" runat="server" />
</asp:Content>