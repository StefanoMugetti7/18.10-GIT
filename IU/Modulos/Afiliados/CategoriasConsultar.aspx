<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="CategoriasConsultar.aspx.cs" Inherits="IU.Modulos.Afiliados.CategoriasConsultar" Title="" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/CategoriasModificarDatos.ascx" TagPrefix="AUGE" TagName="ConsultarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ConsultarDatos ID="ConsultarDatos" runat="server" />
</asp:Content>
