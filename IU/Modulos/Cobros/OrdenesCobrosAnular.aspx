<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="OrdenesCobrosAnular.aspx.cs" Inherits="IU.Modulos.Cobros.OrdenesCobrosAnular" %>
<%@ Register src="~/Modulos/Cobros/Controles/OrdenesCobrosDatos.ascx" tagname="Controles" tagprefix="auge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <auge:Controles ID="ctrlDatos" runat="server" />
</asp:Content>
