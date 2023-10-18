<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="BancosLotesConsultar.aspx.cs" Inherits="IU.Modulos.Bancos.BancosLotesConsultar" Title="" %>
<%@ Register Src="~/Modulos/Bancos/Controles/BancosLotesDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
