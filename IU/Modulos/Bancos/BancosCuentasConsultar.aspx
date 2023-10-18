<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="BancosCuentasConsultar.aspx.cs" Inherits="IU.Modulos.Tesoreria.BancosCuentasConsultar" Title="" %>
<%@ Register Src="~/Modulos/Bancos/Controles/BancosCuentasDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>