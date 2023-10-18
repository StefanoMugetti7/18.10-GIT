<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ParametrosMailsModificar.aspx.cs" Inherits="IU.Modulos.TGE.ParametrosMailsModificar" %>
<%@ Register Src="~/Modulos/TGE/Control/ParametrosMailsDatos.ascx" TagPrefix="AUGE" TagName="ConsultarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ConsultarDatos ID="ctrParametrosMails" runat="server" />
</asp:Content>

<asp:Content ID="Content8" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="ContentPlaceDerechaCentro" runat="server">
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="ContentPlaceIzquierdoAbajo" runat="server">
</asp:Content>
<asp:Content ID="Content11" ContentPlaceHolderID="ContentPlaceDerechaAbajo" runat="server">
</asp:Content>
