<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CuentasAhorrosTiposAgregar.aspx.cs" Inherits="IU.Modulos.Ahorros.CuentasAhorrosTiposAgregar" Title="" %>
<%@ Register Src="~/Modulos/Ahorros/Controles/CuentasAhorrosTiposDatos.ascx" TagPrefix="AUGE" TagName="AgregarDatos" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:AgregarDatos ID="AgregarDatos" runat="server" />
</asp:Content>
