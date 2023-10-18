<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="BancosCuentasModificar.aspx.cs" Inherits="IU.Modulos.Tesoreria.BancosCuentasModificar" Title="" %>
<%@ Register Src="~/Modulos/Bancos/Controles/BancosCuentasDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
