<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CuentasAhorrosTiposModificar.aspx.cs" Inherits="IU.Modulos.Ahorros.CuentasAhorrosTiposModificar" Title="" %>
<%@ Register Src="~/Modulos/Ahorros/Controles/CuentasAhorrosTiposDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModificarDatos" runat="server" />
</asp:Content>
