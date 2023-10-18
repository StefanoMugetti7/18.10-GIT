<%@ Page Language="C#" EnableEventValidation="false" MasterPageFile="~/Maestra.Master" AutoEventWireup="true" CodeBehind="AfiliadosModificar.aspx.cs" Inherits="IU.Modulos.Afiliados.AfiliadosModificar" %>
<%@ Register Src="~/Modulos/Afiliados/Controles/AfiliadoModificarDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ModifDatos" runat="server" />
</asp:Content>
