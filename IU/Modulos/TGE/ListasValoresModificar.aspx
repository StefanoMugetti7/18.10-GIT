<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="ListasValoresModificar.aspx.cs" Inherits="IU.Modulos.TGE.ListasValoresModificar" Title="" %>
<%@ Register Src="~/Modulos/TGE/Control/ListasValoresDetalles.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
