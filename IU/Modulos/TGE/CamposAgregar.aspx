<%@ Page Title="" Language="C#" MasterPageFile="~/Maestra.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="CamposAgregar.aspx.cs" Inherits="IU.Modulos.TGE.CamposAgregar" %>
<%@ Register Src="~/Modulos/TGE/Control/CamposDatos.ascx" TagPrefix="AUGE" TagName="ModificarDatos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <AUGE:ModificarDatos ID="ctrDaos" runat="server" />
</asp:Content>
