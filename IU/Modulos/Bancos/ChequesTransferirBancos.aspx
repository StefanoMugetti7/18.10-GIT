<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="ChequesTransferirBancos.aspx.cs" Inherits="IU.Modulos.Bancos.ChequesTransferirBancos" Title="" %>
<%@ Register Src="~/Modulos/Bancos/Controles/ChequesModificarListar.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
