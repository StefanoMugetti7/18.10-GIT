<%@ Page Language="C#" MasterPageFile="~/Maestra.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="BancosCuentasAgregar.aspx.cs" Inherits="IU.Modulos.Tesoreria.BancosCuentasAgregar" Title="" %>
<%@ Register Src="~/Modulos/Bancos/Controles/BancosCuentasDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos2" %>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos2 ID="ModificarDatos2" runat="server" />
</asp:Content>
