<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="TiposFuncionalidadesPorEstadosListar.aspx.cs" Inherits="IU.Modulos.TGE.TiposFuncionalidadesPorEstadosListar" Title="" %>
<%@ Register Src="~/Modulos/TGE/Control/TiposFuncionalidadesPorEstadosModificarDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ctrModifDatos" runat="server" />
</asp:Content>

